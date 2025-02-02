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
using System.Text.RegularExpressions;
using System.Data.OleDb;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using Clinic.Report;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Net;
using NAudio.Wave;
using System.Media;
using System.Web;
using DevExpress.XtraEditors.Controls;
using System.Collections;
using System.Threading; 

namespace Clinic
{
    public partial class InspectionPregnant : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Diagnosa> listDiagnosa = new List<Diagnosa>();
        List<DiagnosaType> listDiagnosaType = new List<DiagnosaType>();
        List<Room> listRoom = new List<Room>();
        List<Medicine> listMedicine = new List<Medicine>(); List<MedicineUmum> listMedicineU = new List<MedicineUmum>();
        List<MedicineInfo> listMedicineInfo = new List<MedicineInfo>();
        List<Kehamilan> listKehamilan = new List<Kehamilan>();
        List<KetHamil> listKetHamil = new List<KetHamil>();
        List<Stat> listHours = new List<Stat>();
        List<Formula> listFormula = new List<Formula>();
        List<Formula2> listFormula2 = new List<Formula2>(); List<Formula2> listFormulaU = new List<Formula2>();
        List<Stat> listLayanan = new List<Stat>();
        List<Stat> listGrpLaya = new List<Stat>();
        List<Layanan> listLaya = new List<Layanan>();
        List<Layanan> listLaya2 = new List<Layanan>();
        List<Layanan> listLaya3 = new List<Layanan>();
        List<Dosis> listDosis = new List<Dosis>();

        DataSet dsRujukan = new DataSet();
        DataSet dsRekomendasi = new DataSet();
        DataSet dsSkd = new DataSet();
        DataSet dsMRUmum = new DataSet();
        DataSet dscutiHamil = new DataSet();
        DataSet dsMRHamil = new DataSet();
        DataSet dsMRKb = new DataSet();
        DataSet dsAction = new DataSet();
        DataTable dtGlDiag = new DataTable();
        DataTable dtGlMed = new DataTable(); DataTable dtGlMedU = new DataTable();

        RepositoryItemGridLookUpEdit glmed = new RepositoryItemGridLookUpEdit();
        //RepositoryItemGridLookUpEdit glmedRacik = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit glfor = new RepositoryItemGridLookUpEdit();
        RepositoryItemLookUpEdit medicineInfoLookup = new RepositoryItemLookUpEdit();
        RepositoryItemLookUpEdit dosisLookup = new RepositoryItemLookUpEdit();
        //RepositoryItemLookUpEdit racikLookup = new RepositoryItemLookUpEdit();
        RepositoryItemLookUpEdit LookDiagnosa = new RepositoryItemLookUpEdit();
        RepositoryItemGridLookUpEdit LookDiagnosaGrid = new RepositoryItemGridLookUpEdit();

        RepositoryItemGridLookUpEdit LokObatGrid = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit LokObatGridU = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit LokObatGridR = new RepositoryItemGridLookUpEdit();

        public string  v_name = "", v_anamnesa = "", v_amkn = "", v_aobat = "", p_statuscls = "";
        string tmp_now = "", tmp_old = "", tmp_fam = "", tmp_fisik = "", tmp_add = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string pub_nama = ""; string idvisit = "", s_stat = "";
        //string today = "2019-11-27";
        int timer = 0, timer2 = 0, cek_interval = 180;
        private LabelControl _currentLabel;
        string lsMSG = ""; int lsOK = 0; bool bl_klap = true;

        public InspectionPregnant()
        {
            InitializeComponent();
        }

        private void Inspection_Load(object sender, EventArgs e)
        {
            string sql = "";
            InitData();
            LoadDataPasien();
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "InspectionPregnant");

            sql = "";
            sql = " select max(a.ID_DOKTER) ID_DOKTER from KLINIK.CS_DOKTER a where NIK_DOKTER = '" + ConnOra.v_nik.ToString() + "' and F_AKTIF ='Y' and upper(SPESIALIS) ='BIDAN' ";

            try
            {
                OleDbConnection sqldok = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSqldok = new OleDbDataAdapter(sql, sqldok);
                DataTable dtdok = new DataTable();
                adSqldok.Fill(dtdok);
                if (dtdok.Rows.Count > 0)
                {
                    ConnOra.v_iddokter = dtdok.Rows[0]["ID_DOKTER"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void InitData()
        {
            btnAddAnam.Enabled = false;
            btnSaveAnam.Enabled = false;
            btnCreate.Enabled = false;

            btnDelDiag.Enabled = false;
            btnAddDiag.Enabled = false;
            btnSaveDiag.Enabled = false;
            btnCanDiag.Enabled = false;

            comboBox1.Items.Clear();
            comboBox1.Items.Add("All");
            //comboBox1.Items.Add("First Inspection");
            comboBox1.Items.Add("Inspection");
            //comboBox1.Items.Add("Observation");
            comboBox1.Items.Add("Medicine");
            comboBox1.Items.Add("Payment");
            comboBox1.Items.Add("Completed");
            comboBox1.SelectedIndex = 1;

            dtGlDiag.Clear();
            string sql_poli = " select item_cd, initcap(item_name) item_name from KLINIK.cs_diagnosa_item where status = 'A'   order by item_name ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_poli, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            dtGlDiag = dt;
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

            //string sql_room = " select room_id, room_name, bed_qty from KLINIK.cs_room where status = 'A' ";
            //OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_room, sqlConnect2);
            //DataTable dt2 = new DataTable();
            //adSql2.Fill(dt2);

            //listRoom.Clear();
            //for (int i = 0; i < dt2.Rows.Count; i++)
            //{
            //    listRoom.Add(new Room() { roomCode = dt2.Rows[i]["room_id"].ToString(), roomName = dt2.Rows[i]["room_name"].ToString(), roomQty = dt2.Rows[i]["bed_qty"].ToString() });
            //}
            //luObsRoom.Properties.NullText = "";

            //dtGlMed.Clear();
            //string sql_med = "  select med_cd, initcap(med_name) || ' (BPJS: ' || bpjs_cover || ')' med_name from KLINIK.cs_medicine where status = 'A' and MED_GROUP ='OBAT' order by med_name ";
            //OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_med, sqlConnect3);
            //DataTable dt3 = new DataTable();
            //dtGlMed = dt3;
            //adSql3.Fill(dt3);
            //listMedicine.Clear();
            //for (int i = 0; i < dt3.Rows.Count; i++)
            //{
            //    listMedicine.Add(new Medicine() { medicineCode = dt3.Rows[i]["med_cd"].ToString(), medicineName = dt3.Rows[i]["med_name"].ToString() });
            //}

            listMedicineInfo.Clear();
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "A", medicineInfoName = "(P.C.) Sesudah Makan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "B", medicineInfoName = "(A.C.) Sebelum Makan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "C", medicineInfoName = "(P.R.N.) Bila Perlu" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "D", medicineInfoName = "(G.T.T) Diteteskan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "E", medicineInfoName = "(U.E) Pemakaian Luar" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "F", medicineInfoName = "(D.C.) Pada Waktu Makan" });

            listFormula.Clear();
            listFormula2.Clear();

            grpSkdUmum.Visible = true;
            grpSkdUmum.Dock = DockStyle.Fill;
            grpSkdKec.Visible = false;

            listKehamilan.Clear();
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K1", kehamilanName = "K1" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K2", kehamilanName = "K2" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K3", kehamilanName = "K3" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K4", kehamilanName = "K4" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K5", kehamilanName = "K5" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K6", kehamilanName = "K6" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K7", kehamilanName = "K7" });

            listKetHamil.Clear();
            listKetHamil.Add(new KetHamil() { ketHamilCode = "B", ketHamilName = "Melahirkan" });
            listKetHamil.Add(new KetHamil() { ketHamilCode = "M", ketHamilName = "Keguguran" });

            //string sql_period = " select periode from ( select 'a' as s, '' as periode from dual union select distinct 'b' as s, periode from cs_mcu) order by s asc ";
            //OleDbConnection sqlConnect4 = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adSql4 = new OleDbDataAdapter(sql_period, sqlConnect4);
            //DataTable dt4 = new DataTable();
            //adSql4.Fill(dt4);
            //cMcuPeriode.Items.Clear();
            //for (int i = 0; i < dt4.Rows.Count; i++)
            //{
            //    cMcuPeriode.Items.Add(dt4.Rows[i][0].ToString());
            //}
            //cMcuPeriode.SelectedIndex = 0;

            ////luObsRoom.Text = "RM001";
            //luObsRoom.Properties.DataSource = listRoom;
            //luObsRoom.Properties.ValueMember = "roomCode";
            //luObsRoom.Properties.DisplayMember = "roomName";

            //luObsRoom.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //luObsRoom.Properties.DropDownRows = listRoom.Count;
            //luObsRoom.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            //luObsRoom.Properties.AutoSearchColumnIndex = 1;
            //luObsRoom.Properties.NullText = "Select Room";

            cmbPersetujuan.Items.Clear();
            cmbPersetujuan.Items.Add("");
            cmbPersetujuan.Items.Add("Setuju");
            cmbPersetujuan.Items.Add("Tidak Setuju");

            listHours.Clear();
            listHours.Add(new Stat() { statCode = "0.5", statName = "30 Menit" });
            listHours.Add(new Stat() { statCode = "1", statName = "1 Jam" });
            listHours.Add(new Stat() { statCode = "1.5", statName = "1,5 Jam" });
            listHours.Add(new Stat() { statCode = "2", statName = "2 Jam" });
            listHours.Add(new Stat() { statCode = "2.5", statName = "2,5 Jam" });
            listHours.Add(new Stat() { statCode = "3", statName = "3 Jam" });

            string sql_lay = " select treat_type_id trt_id, initcap(treat_type_name) trt_name from KLINIK.cs_treatment_type where 1=1 and treat_type_id = 'TRT01' ";
            OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_lay, oraConnectf);
            DataTable dtf = new DataTable();
            adOraf.Fill(dtf);
            listLaya.Clear();
            for (int i = 0; i < dtf.Rows.Count; i++)
            {
                listLaya.Add(new Layanan() { layananCode = dtf.Rows[i]["trt_id"].ToString(), layananName = dtf.Rows[i]["trt_name"].ToString()});
            }

            listLayanan.Clear();
            listLayanan.Add(new Stat() { statCode = "OPN", statName = "Aktif" });
            listLayanan.Add(new Stat() { statCode = "CLS", statName = "Selesai" });
            listLayanan.Add(new Stat() { statCode = "CAN", statName = "Batal" });

            string SQL = "";
            SQL = SQL + Environment.NewLine + " select treat_item_id, initcap(treat_item_name) treat_item_name ";
            SQL = SQL + Environment.NewLine + "   from KLINIK.cs_treatment_item ";
            SQL = SQL + Environment.NewLine + "  where 1=1 and treat_type_id = 'TRT01' and treat_group_id in ('TRG07')  ";



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
        }

        private void dataFormula(string policd)
        {
            string SQL = " ", plgroup = "";

            plgroup = "TRG07";
            
            SQL = " ";
            SQL = SQL + Environment.NewLine + " select treat_item_id, initcap(treat_item_name) treat_item_name ";
            SQL = SQL + Environment.NewLine + "   from KLINIK.cs_treatment_item ";
            SQL = SQL + Environment.NewLine + "  where 1=1 and treat_type_id = 'TRT01'  ";
            SQL = SQL + Environment.NewLine + "    and treat_group_id  = '" + plgroup.ToString() + "'  ";
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
        }
        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            LoadDataPasien();
        }

        private void LoadDataPasien()
        {
            string sql_search;

            sql_search = " ";

            //sql_search = sql_search + " select que01, patient_no, NAME, gender, age, " +
            //                          " (select poli_name from KLINIK.cs_policlinic where status='A' and poli_cd=aa.poli_cd) as poli_nm, " +
            //                          " type_patient, status, visit_date, type_mr,   " +
            //                          " (select max(rm_no) from KLINIK.cs_patient where status='A' and patient_no=aa.patient_no) as rm_no, work_accident, vdate,poli_cd,id_visit " +
            //                          " from (  " +
            //                          " select que01, a.patient_no, b.name, gender, round(((sysdate-birth_date)/30)/12) age,  " + 
            //                          " poli_cd, type_patient, case when a.status='NUR' then 'First Inspection'   " +
            //                          " when a.status='INS' then 'Inspection'  " +
            //                          " when a.status='PAY' then 'Payment'  " +
            //                          " when a.status='MED' then 'Medicine'  " + 
            //                          " when a.status='CLS' then 'Completed' end status,  " +
            //                          " to_char(visit_date,'yyyy-mm-dd') as visit_date,  " +
            //                          " case when a.poli_cd = 'POL0002' then 'PREG'  " +
            //                          " when a.poli_cd = 'POL0003' then 'FAMP' else 'COMM' end as type_mr, work_accident,   " +
            //                          " visit_date as vdate, id_visit, a.POLI_CD  " +
            //                          " from KLINIK.cs_visit a  " +
            //                          " join KLINIK.cs_patient_info b on a.patient_no = b.patient_no  " +
            //                          " where 1 = 1  " +
            //                          " and to_char(visit_date,'yyyy-mm-dd')= '" + today + "'  " +

            //                          " and purpose = 'MID'  ";

            sql_search = " "; 
            sql_search = sql_search + Environment.NewLine + " select que01, patient_no, initcap(name)  NAME, gender, age, blood_type, type_patient, status, visit_date, type_mr, ";
            sql_search = sql_search + Environment.NewLine + " (select max(rm_no) from KLINIK.cs_patient where status='A' and patient_no=aa.patient_no) as rm_no, work_accident, vdate,ID_VISIT, POLI_CD ,  decode(type_patient,'B','BPJS','A','Asuransi','Umum') ctype";
            sql_search = sql_search + Environment.NewLine + "   from (  "; 
            sql_search = sql_search + Environment.NewLine + "          select que01, a.patient_no, b.name, gender,   ";
            sql_search = sql_search + Environment.NewLine + "                 round(((sysdate-b.birth_date)/30)/12) age,   ";
            sql_search = sql_search + Environment.NewLine + "                 b.GOL_DARAH blood_type, type_patient, case when a.status='NUR' then 'First Inspection'   ";
            sql_search = sql_search + Environment.NewLine + "                 when a.status='INS' then 'Inspection'   ";
            sql_search = sql_search + Environment.NewLine + "                 when a.status='MED' then 'Medicine'   ";
            sql_search = sql_search + Environment.NewLine + "                 when a.status='PAY' then 'Payment'   ";
            sql_search = sql_search + Environment.NewLine + "                 when a.status='CLS' then 'Completed' end status,    ";
            sql_search = sql_search + Environment.NewLine + "                      to_char(A.visit_date,'yyyy-mm-dd') as visit_date,    ";
            sql_search = sql_search + Environment.NewLine + "                case when a.poli_cd = 'POL0002' then 'PREG'     ";
            sql_search = sql_search + Environment.NewLine + "           when a.poli_cd = 'POL0003' then 'FAMP' else 'COMM' end as type_mr, work_accident,    ";
            sql_search = sql_search + Environment.NewLine + "                A.visit_date as vdate, a.ID_VISIT, a.POLI_CD    ";
            sql_search = sql_search + Environment.NewLine + "           from KLINIK.cs_visit a    ";
            sql_search = sql_search + Environment.NewLine + "                join KLINIK.cs_patient_info b on a.patient_no = b.patient_no    ";
            sql_search = sql_search + Environment.NewLine + "                left JOIN KLINIK.cs_treatment_head C ON (a.id_visit = c.id_visit)  ";
            sql_search = sql_search + Environment.NewLine + "                left JOIN KLINIK.cs_treatment_DETAIL D ON (C.head_id = D.head_id)  ";
            sql_search = sql_search + Environment.NewLine + "                left JOIN KLINIK.cs_treatment_item E ON (D.treat_item_id=E.treat_item_id AND UPPER(TREAT_ITEM_NAME) not LIKE '%USG%')  "; 
            sql_search = sql_search + Environment.NewLine + "          where 1 = 1  ";
            sql_search = sql_search + Environment.NewLine + "            and to_char(a.visit_date,'yyyy-mm-dd')= '" + today + "'  ";
            sql_search = sql_search + Environment.NewLine + "            and a.poli_cd in ('POL0002','POL0003') ";
            sql_search = sql_search + Environment.NewLine + "            and purpose = 'MID'  ";

            if (comboBox1.Text == "All")
            {
                sql_search = sql_search + " and a.status in ('NUR','INS','OBS','MED','PAY','CLS') ";
            }
            //else if (comboBox1.Text == "First Inspection")
            //{
            //    sql_search = sql_search + " and a.status in ('NUR') ";
            //}
            else if (comboBox1.Text == "Inspection")
            {
                sql_search = sql_search + " and a.status in ('NUR','INS') ";
            }
            else if (comboBox1.Text == "Observation")
            {
                sql_search = sql_search + " and a.status in ('OBS') ";
            }
            else if (comboBox1.Text == "Medicine")
            {
                sql_search = sql_search + " and a.status in ('MED') ";
            }
            else if (comboBox1.Text == "Payment")
            {
                sql_search = sql_search + " and a.status in ('PAY') ";
            }
            else if (comboBox1.Text == "Completed")
            {
                sql_search = sql_search + " and a.status in ('CLS') ";
            }

            //sql_search = sql_search + " ) aa where 1=1 order by aa.que01 ";
            sql_search = sql_search + " ) aa where 1=1 order by aa.vdate ";

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
                gridView1.Columns[1].Caption = "Pasien No";
                gridView1.Columns[2].Caption = "Nama";
                gridView1.Columns[3].Caption = "L/P";
                gridView1.Columns[4].Caption = "Umur";
                gridView1.Columns[5].Caption = "Gol Darah";
                gridView1.Columns[6].Caption = "Pasien";
                gridView1.Columns[7].Caption = "Status";
                gridView1.Columns[8].Caption = "Tanggal";
                gridView1.Columns[9].Caption = "Tipe";
                gridView1.Columns[10].Caption = "Medical Record";
                gridView1.Columns[11].Caption = "KK";
                gridView1.Columns[12].Caption = "vdate";
                gridView1.Columns[13].Caption = "idvisit";
                gridView1.Columns[14].Caption = "Poli";
                gridView1.Columns[15].Caption = "Type";
                gridView1.Columns[5].MinWidth = 80;
                gridView1.Columns[5].MaxWidth = 80;
                gridView1.Columns[11].MinWidth = 30;
                gridView1.Columns[11].MaxWidth = 30;

                gridView1.Columns[5].Visible = false;
                gridView1.Columns[6].Visible = false;
                //gridView1.Columns[7].Visible = false;
                gridView1.Columns[8].Visible = false;
                gridView1.Columns[9].Visible = false;
                gridView1.Columns[10].Visible = false;
                gridView1.Columns[11].Visible = false;
                gridView1.Columns[12].Visible = false;
                gridView1.Columns[13].Visible = false;
                gridView1.Columns[14].Visible = false;

                if (gridView1.RowCount > 0)
                {
                    btnLoadRujukan.Enabled = true;
                    loadTindakan.Enabled = true;
                    loadRekomendasi.Enabled = true;
                    loadObservasi.Enabled = true;
                    loadResep.Enabled = true;
                    loadSKD.Enabled = true;
                    loadMR.Enabled = true;
                    loadSuratHamil.Enabled = true;
                    loadMCU.Enabled = true;
                    loadTind.Enabled = true;
                }
                else
                {
                    btnLoadRujukan.Enabled = false;
                    loadTindakan.Enabled = false;
                    loadRekomendasi.Enabled = false;
                    loadObservasi.Enabled = false;
                    loadResep.Enabled = false;
                    loadSKD.Enabled = false;
                    loadMR.Enabled = false;
                    loadSuratHamil.Enabled = false;
                    loadMCU.Enabled = false;
                    loadTind.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            //GridView View = sender as GridView;
            //if (e.Column.Caption == "Pasien")
            //{
            //    string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[7]);
            //    if (kk == "Emergency")
            //    {
            //        e.Appearance.BackColor = Color.FromArgb(150, Color.Red);
            //        e.Appearance.BackColor2 = Color.FromArgb(150, Color.Red);
            //        e.Appearance.ForeColor = Color.White;
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //    }
            //}
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string stat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[7]);
                string tipe = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);

                //if (stat == "First Inspection")
                //{
                //    //e.Appearance.BackColor = Color.FromArgb(40, Color.DodgerBlue);
                //    //e.Appearance.BackColor = Color.GreenYellow;
                //    //e.Appearance.BackColor2 = Color.White;
                //    e.Appearance.ForeColor = Color.LightGray;
                //    //e.Appearance.Font = new Font("Arial", 9, FontStyle.Bold);
                //    //e.Appearance.FontStyleDelta = FontStyle.Bold;
                //    //e.HighPriority = true;
                //}

                //if (stat == "Medicine")
                //{
                //    e.Appearance.BackColor = Color.LimeGreen;
                //    e.Appearance.BackColor2 = Color.Green;
                //    //e.Appearance.BackColor2 = Color.White;
                //    e.Appearance.ForeColor = Color.White;
                //    e.Appearance.Font = new Font("Arial", 9, FontStyle.Bold);
                //    //e.Appearance.FontStyleDelta = FontStyle.Bold;
                //    e.HighPriority = true;
                //}

                if (tipe == "E")
                {
                    e.Appearance.BackColor = Color.LightCoral;
                    e.Appearance.BackColor2 = Color.Crimson;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                    e.HighPriority = true;
                }

                //if (stat == "Inspection")
                //{
                //    e.Appearance.BackColor = Color.DodgerBlue;
                //    e.Appearance.BackColor2 = Color.RoyalBlue;
                //    //e.Appearance.BackColor2 = Color.White;
                //    e.Appearance.ForeColor = Color.White;
                //    e.Appearance.Font = new Font("Arial", 9, FontStyle.Bold);
                //    //e.Appearance.FontStyleDelta = FontStyle.Bold;
                //    e.HighPriority = true;
                //}
            }
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {

            if (gridView1.RowCount < 1)
                return;

            btnAddAnam.Enabled = false;
            btnSaveAnam.Enabled = false;
            btnDelDiag.Enabled = false;
            btnAddDiag.Enabled = false;
            btnSaveDiag.Enabled = false;
            btnCanDiag.Enabled = false;
            btnCall.Enabled = false;
            btnDoInsp.Enabled = false;

            tmp_now = "";
            tmp_old = "";
            tmp_fam = "";
            tmp_fisik = "";
            tmp_add = "";

            GridView View = sender as GridView;

            if (View.RowCount < 1)
                return;

            string s_nik = "", s_que = "", s_rm = "", s_date = "", p_rnow = "", p_rthen = "", p_rfam = "", p_rfisik = "", p_radd = "", s_poli = "", s_nama = "";
            string s_infop1 = "", s_infop2 = "", s_infop3 = "", s_infop4 = "", s_infop5 = "";
            string sql_his = "", sql_anam = "", stype = ""  ;
            string s_tatus = View.GetRowCellDisplayText(e.RowHandle, View.Columns[7]);

            s_nik = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
            s_nama = View.GetRowCellDisplayText(e.RowHandle, View.Columns[2]);
            s_poli = View.GetRowCellDisplayText(e.RowHandle, View.Columns[14]);
            stype = View.GetRowCellDisplayText(e.RowHandle, View.Columns[15]);
            idvisit = View.GetRowCellDisplayText(e.RowHandle, View.Columns[13]);

            pub_nama = s_nama;

            sql_his = sql_his + Environment.NewLine + "select visit_date, (select max(rm_no) from KLINIK.cs_patient where status='A' and patient_no=aa.patient_no) as rm_no, ";
            sql_his = sql_his + Environment.NewLine + "poli_cd, poli_name, work_accident, type, type_mr, que01, patient_no, visit_time ,ID_VISIT";
            sql_his = sql_his + Environment.NewLine + "from(  ";
            sql_his = sql_his + Environment.NewLine + "select patient_no, to_char(visit_date, 'yyyy-mm-dd') visit_date,  ";
            sql_his = sql_his + Environment.NewLine + "a.poli_cd, poli_name,  ";
            sql_his = sql_his + Environment.NewLine + "decode(work_accident, 'N', 'No', 'Yes') work_accident,  ";
            sql_his = sql_his + Environment.NewLine + "decode(type_patient, 'A', 'Asuransi', 'B','BPJS','Umum') type,  ";
            sql_his = sql_his + Environment.NewLine + "case when a.poli_cd = 'POL0002' then 'PREG'  ";
            sql_his = sql_his + Environment.NewLine + "when a.poli_cd = 'POL0003' then 'FAMP' else 'COMM' end as type_mr, que01,  ";
            sql_his = sql_his + Environment.NewLine + "to_char(visit_date, 'hh24:mi:ss') visit_time ,ID_VISIT ";
            sql_his = sql_his + Environment.NewLine + "from KLINIK.cs_visit a  ";
            sql_his = sql_his + Environment.NewLine + "join KLINIK.cs_policlinic b on (a.poli_cd = b.poli_cd and b.status = 'A')  ";
            sql_his = sql_his + Environment.NewLine + "where 1 = 1  ";
            sql_his = sql_his + Environment.NewLine + "and purpose = 'MID' and ID_VISIT = " + idvisit + "  ";
            sql_his = sql_his + Environment.NewLine + "and patient_no = '" + s_nik + "') aa order by 1 desc ";


            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_his, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl2.DataSource = null;
            gridView2.Columns.Clear();
            gridControl2.DataSource = dt;

            gridView2.OptionsView.ColumnAutoWidth = true;
            gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView2.IndicatorWidth = 30;
            gridView2.OptionsBehavior.Editable = false;
            gridView2.BestFitColumns();

            gridView2.Columns[0].Caption = "Tanggal";
            gridView2.Columns[1].Caption = "Medical Record";
            gridView2.Columns[2].Caption = "Poli Code";
            gridView2.Columns[3].Caption = "Poli";
            gridView2.Columns[4].Caption = "Kecelakaan kerja";
            gridView2.Columns[5].Caption = "Pasien";
            gridView2.Columns[6].Caption = "Type Record";
            gridView2.Columns[7].Caption = "Antrian";
            gridView2.Columns[8].Caption = "Pasien No";
            gridView2.Columns[9].Caption = "Jam";
            gridView2.Columns[9].VisibleIndex = 1;


            gridView2.Columns[2].Visible = false;

            gridView2.Columns[6].Visible = false;
            gridView2.Columns[7].Visible = false;
            gridView2.Columns[8].Visible = false;

            s_que = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            s_date = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);
            s_rm = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
            //idvisit = View.GetRowCellDisplayText(e.RowHandle, View.Columns[13]);

            sql_anam = " select blood_press, pulse, temperature, allergy, anamnesa,   " +
                       " disease_now, disease_then, disease_family, anamnesa_physical,  " +
                       " anamnesa_other,bb, infop1, infop2, infop3, infop4, infop5, 'U' action, INFO_K,'" + s_nama + "' nama, tb, cholesterol, blood_sugar, uric_acid,a.ANAMNESA_ID, b.ALERGI_MKN, b.ALERGI_OBAT " +
                       " from KLINIK.cs_anamnesa a, cs_anamnesa_dtl b   " +
                       " where a.ANAMNESA_ID = b.ANAMNESA_ID(+) and a.rm_no = '" + s_rm + "' and id_visit  = '" + idvisit + "'  " + 
                       " and visit_no = '" + s_que + "' ";

            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_anam, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);

            gridControl3.DataSource = null;
            gridView3.Columns.Clear();
            gridControl3.DataSource = dt2;

            gridView3.OptionsView.ColumnAutoWidth = true;
            gridView3.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView3.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView3.IndicatorWidth = 30;
            //gridView3.OptionsBehavior.Editable = false;
            gridView3.BestFitColumns();



            gridView3.Columns[0].Caption = "Tensi";
            gridView3.Columns[1].Caption = "Nadi";
            gridView3.Columns[2].Caption = "Suhu";
            gridView3.Columns[10].Caption = "BB (Kg)";
            gridView3.Columns[19].Caption = "TB (Cm)";
            gridView3.Columns[3].Caption = "Alergi";
            gridView3.Columns[4].Caption = "Keluhan";
            gridView3.Columns[17].Caption = "Kehamilan";
            gridView3.Columns[18].Caption = "Nama";
            gridView3.Columns[18].VisibleIndex = 0;

            gridView3.Columns[18].OptionsColumn.AllowEdit = false;

            gridView3.Columns[10].VisibleIndex = 4;
            gridView3.Columns[19].VisibleIndex = 5;

            gridView3.Columns[0].Width = 22;
            gridView3.Columns[1].Width = 22;
            gridView3.Columns[2].Width = 22;
            gridView3.Columns[4].Width = 60;
            gridView3.Columns[10].Width = 30;
            gridView3.Columns[17].Width = 50;
            gridView3.Columns[19].Width = 30;
            gridView3.Columns[20].Width = 50;

            RepositoryItemLookUpEdit kLookup = new RepositoryItemLookUpEdit();
            kLookup.DataSource = listKehamilan;
            kLookup.ValueMember = "kehamilanCode";
            kLookup.DisplayMember = "kehamilanName";

            kLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            kLookup.DropDownRows = listKehamilan.Count;
            kLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            kLookup.AutoSearchColumnIndex = 1;
            kLookup.NullText = "";
            gridView3.Columns[17].ColumnEdit = kLookup;

            gridView3.Columns[5].Visible = false;
            gridView3.Columns[6].Visible = false;
            gridView3.Columns[7].Visible = false;
            gridView3.Columns[8].Visible = false;
            gridView3.Columns[9].Visible = false;
            gridView3.Columns[11].Visible = false;
            gridView3.Columns[12].Visible = false;
            gridView3.Columns[13].Visible = false;
            gridView3.Columns[14].Visible = false;
            gridView3.Columns[15].Visible = false;
            gridView3.Columns[16].Visible = false;
            gridView3.Columns[23].Visible = false;
            if (s_poli == "POL0002") { gridView3.Columns[17].Visible = true; } else { gridView3.Columns[17].Visible = false; } 

            p_rnow = gridView3.GetRowCellDisplayText(0, gridView3.Columns[5]);
            p_rthen = gridView3.GetRowCellDisplayText(0, gridView3.Columns[6]);
            p_rfam = gridView3.GetRowCellDisplayText(0, gridView3.Columns[7]);
            p_rfisik = gridView3.GetRowCellDisplayText(0, gridView3.Columns[8]);
            p_radd = gridView3.GetRowCellDisplayText(0, gridView3.Columns[9]);
            v_anamnesa = gridView3.GetRowCellDisplayText(0, gridView3.Columns[23]);
            v_amkn = gridView3.GetRowCellDisplayText(0, gridView3.Columns[24]);
            v_aobat = gridView3.GetRowCellDisplayText(0, gridView3.Columns[25]); 

            rNow.Text = p_rnow;
            rOld.Text = p_rthen;
            rFam.Text = p_rfam;
            pFisik.Text = p_rfisik;
            pAdd.Text = p_radd;

            tmp_now = p_rnow;
            tmp_old = p_rthen;
            tmp_fam = p_rfam;
            tmp_fisik = p_rfisik;
            tmp_add = p_radd;

            //if (stype.ToString().Equals("B"))
            //{
            //    chOUmum.Visible = true;
            //    chOUmum.CheckState = CheckState.Unchecked;
            //}
            //else
            //{
            //    chOUmum.Visible = false;
            //}
            splitContainer1.Panel2Collapsed = true;
            //if (p_rnow == "")
            //{
            //    rNow.ForeColor = Color.Gray;
            //    rNow.Text = null;
            //}
            //else
            //{
            //    rNow.ForeColor = Color.Black;
            //    rNow.Text = p_rnow;
            //}

            //if (p_rthen == "")
            //{
            //    rOld.ForeColor = Color.Gray;
            //    rOld.Text = null;
            //}
            //else
            //{
            //    rOld.ForeColor = Color.Black;
            //    rOld.Text = p_rthen;
            //}

            //if (p_rfam == "")
            //{
            //    rFam.ForeColor = Color.Gray;
            //    rFam.Text = null;
            //}
            //else
            //{
            //    rFam.ForeColor = Color.Black;
            //    rFam.Text = p_rfam;
            //}

            //if (p_rfisik == "")
            //{
            //    pFisik.ForeColor = Color.Gray;
            //    pFisik.Text = null;
            //}
            //else
            //{
            //    pFisik.ForeColor = Color.Black;
            //    pFisik.Text = p_rfisik;
            //}

            //if (p_radd == "")
            //{
            //    pAdd.ForeColor = Color.Gray;
            //    pAdd.Text = null;
            //}
            //else
            //{
            //    pAdd.ForeColor = Color.Black;
            //    pAdd.Text = p_radd;
            //}

            s_infop1 = gridView3.GetRowCellDisplayText(0, gridView3.Columns[11]);
            s_infop2 = gridView3.GetRowCellDisplayText(0, gridView3.Columns[12]);
            s_infop3 = gridView3.GetRowCellDisplayText(0, gridView3.Columns[13]);
            s_infop4 = gridView3.GetRowCellDisplayText(0, gridView3.Columns[14]);
            s_infop5 = gridView3.GetRowCellDisplayText(0, gridView3.Columns[15]);
             

            //if (stype.ToString().Equals("BPJS"))
            //{
            //    chOUmum.Visible = true;
            //}
            //else
            //{
            //    chOUmum.Visible = false;
            //}

            //if (s_rm == "")
            //{
            //    btnAddAnam.Enabled = false;
            //    btnSaveAnam.Enabled = false;
            //    //btnCreate.Enabled = true;
            //}
            //else if (gridView3.RowCount<=0)
            //{
            //    btnAddAnam.Enabled = true;
            //    btnSaveAnam.Enabled = false;
            //    //btnCreate.Enabled = false;
            //}
            //else if (gridView3.RowCount > 0)
            //{
            //    btnAddAnam.Enabled = false;
            //    btnSaveAnam.Enabled = true;
            //    btnCreate.Enabled = false;
            //}

            LoadDiagnosa(s_rm, s_date, s_que, s_poli);

            if (s_poli == "POL0002" && s_rm != "")
            {
                string sql_add_info = "", p_add_empid = "", p_add_rm = "";
                string p_info01 = "", p_info02 = "", p_info03 = "", p_info04 = "", p_info05 = "";
                string p_info06 = "", p_info07 = "", p_info08 = "", p_info09 = "", p_info10 = "";

                sql_add_info = " select A.patient_no, rm_no, info01, info02, info03, info04, info05,  " +
                               " info06, info07, info08, info09, info10  " +
                               " from KLINIK.cs_patient A, cs_visit B " +
                               " where  A.patient_no = B.patient_no AND A.status='A'  " +
                               " and A.patient_no= '" + s_nik + "' AND ID_VISIT = '" + idvisit + "' " +
                               " and rm_no = '" + s_rm + "'  ";

                OleDbConnection sqlConnect4 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql4 = new OleDbDataAdapter(sql_add_info, sqlConnect4);
                DataTable dt4 = new DataTable();
                adSql4.Fill(dt4);

                p_add_empid = dt4.Rows[0]["patient_no"].ToString();
                p_add_rm = dt4.Rows[0]["rm_no"].ToString();
                p_info01 = dt4.Rows[0]["info01"].ToString();
                p_info02 = dt4.Rows[0]["info02"].ToString();
                p_info03 = dt4.Rows[0]["info03"].ToString();
                p_info04 = dt4.Rows[0]["info04"].ToString();
                p_info05 = dt4.Rows[0]["info05"].ToString();
                p_info06 = dt4.Rows[0]["info06"].ToString();
                p_info07 = dt4.Rows[0]["info07"].ToString();
                p_info08 = dt4.Rows[0]["info08"].ToString();
                p_info09 = dt4.Rows[0]["info09"].ToString();
                p_info10 = dt4.Rows[0]["info10"].ToString();

                lAddEmpid.Text = p_add_empid;
                lAddRM.Text = p_add_rm;

                tAdd01.Text = p_info01;
                tAdd02.Text = p_info02;
                tAdd03.Text = p_info03;
                tAdd04.Text = p_info04;
                tAdd05.Text = p_info05;

                tAdd06.Text = p_info06;
                tAdd07.Text = p_info07;
                tAdd08.Text = p_info08;
                tAdd09.Text = p_info09;
                tAdd10.Text = p_info10;
            }
            else
            {
                lAddEmpid.Text = "";
                lAddRM.Text = "";

                tAdd01.Text = "";
                tAdd02.Text = "";
                tAdd03.Text = "";
                tAdd04.Text = "";
                tAdd05.Text = "";

                tAdd06.Text = "";
                tAdd07.Text = "";
                tAdd08.Text = "";
                tAdd09.Text = "";
                tAdd10.Text = "";
            }

            if (s_rm == "")
            {
                btnSaveAdd.Enabled = false;
            }
            else
            {
                btnSaveAdd.Enabled = true;
            }

            if (s_tatus == "Completed" || s_tatus == "Payment" || s_tatus == "Medicine")
            {
                btnDelTindakan.Enabled = false;
                btnAddTindakan.Enabled = false;
                btnSaveTindakan.Enabled = false;
                btnAddTind.Enabled = false;
                btnCall.Enabled = false; btnDoInsp.Enabled = false;
                btnDelTind.Enabled = false; btnSaveTind.Enabled = false;
            }
            else
            {
                btnDelTindakan.Enabled = true;
                btnAddTindakan.Enabled = true;
                btnSaveTindakan.Enabled = true;
                btnAddTind.Enabled = true;
                btnCall.Enabled = true; btnDoInsp.Enabled = true;
            }
            //loadResep.PerformClick();
            //loadResep_Click.per
            //loadTind.PerformClick();
            //LoadDiagnosa(s_rm, s_date, s_que);
            //loadResep.PerformClick();
            loadResep_Click(sender, e);
            loadTind_Click(sender, e);
            //loadResep.Click += loadResep_Click;
            //loadTind.Click += loadTind_Click;
        }

        private void LoadDiagnosa(string s_rm, string s_date, string s_que, string spoli)
        {
            string sql_diag = "";
            sql_diag = " select a.item_cd, initcap(c.cat_name) category_name, a.item_cd,  " +
                       " type_diagnosa, a.remark, 'S' a, a.diagnosa_id  " +
                       " from KLINIK.cs_diagnosa a  " +
                       " join KLINIK.cs_diagnosa_item b on a.item_cd = b.item_cd  " +
                       " join KLINIK.cs_diagnosa_category c on b.cat_id = c.cat_id  " +
                       " JOIN KLINIK.cs_anamnesa D ON A.ANAMNESA_ID = D.ANAMNESA_ID " +
                       " JOIN KLINIK.CS_VISIT E ON E.id_visit = D.id_visit " +
                       " where a.rm_no = '" + s_rm + "'  " +
                       " and to_char(a.insp_date,'yyyy-mm-dd') = '" + s_date + "'  and  E.id_visit = '" + idvisit + "' " +
                       " and a.visit_no = '" + s_que + "'  " +
                       " order by type_diagnosa ";

            OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_diag, sqlConnect3);
            DataTable dt3 = new DataTable();
            adSql3.Fill(dt3);

            gridControl4.DataSource = null;
            gridView4.Columns.Clear();
            gridControl4.DataSource = dt3;

            gridView4.OptionsView.ColumnAutoWidth = true;
            gridView4.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView4.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView4.IndicatorWidth = 30;
            //gridView4.OptionsBehavior.Editable = false;


            gridView4.Columns[0].Caption = "Kode";
            gridView4.Columns[1].Caption = "Kategori";
            gridView4.Columns[2].Caption = "Diagnosa";
            gridView4.Columns[3].Caption = "Tipe";
            gridView4.Columns[4].Caption = "Remark";
            gridView4.Columns[5].Caption = "Action";
            gridView4.Columns[6].Caption = "ID";

            //RepositoryItemLookUpEdit diagnosaLookup = new RepositoryItemLookUpEdit();
            //diagnosaLookup.DataSource = listDiagnosa;
            //diagnosaLookup.ValueMember = "diagnosaCode";
            //diagnosaLookup.DisplayMember = "diagnosaName";

            //diagnosaLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //diagnosaLookup.DropDownRows = listDiagnosa.Count;
            //diagnosaLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            //diagnosaLookup.AutoSearchColumnIndex = 1;
            //diagnosaLookup.NullText = "";
            //gridView4.Columns[2].ColumnEdit = diagnosaLookup;

            //RepositoryItemGridLookUpEdit gldiag = new RepositoryItemGridLookUpEdit();
            //gldiag.DataSource = listDiagnosa;
            //gldiag.ValueMember = "diagnosaCode";
            //gldiag.DisplayMember = "diagnosaName";

            //gldiag.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //gldiag.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            //gldiag.ImmediatePopup = true;
            //gldiag.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            //gldiag.NullText = "";
            //gridView4.Columns[2].ColumnEdit = gldiag;

            ConnOra.LookUpGridFilter(listDiagnosa, gridView4, "diagnosaCode", "diagnosaName", LookDiagnosaGrid, 2);
            LookDiagnosaGrid.ImmediatePopup = false;
            LookDiagnosaGrid.PopupFilterMode = PopupFilterMode.Contains;

            //LookDiagnosaGrid.DataSource = listDiagnosa;
            //LookDiagnosaGrid.ValueMember = "diagnosaCode";
            //LookDiagnosaGrid.DisplayMember = "diagnosaName";
            ////LookDiagnosaGrid.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //var gridView = LookDiagnosaGrid.View;
            //gridView.OptionsView.ShowAutoFilterRow = true; // Tampilkan AutoFilterRow
            //gridView.OptionsCustomization.AllowSort = true;

            //foreach (DevExpress.XtraGrid.Columns.GridColumn column in gridView.Columns)
            //{
            //    column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            //}
            //if (gridView.Columns["diagnosaCode"] == null)
            //{
            //    gridView.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            //    {
            //        FieldName = "diagnosaCode",
            //        Caption = "diagnosaCode",
            //        Visible = true
            //    });
            //}
            //if (gridView.Columns["diagnosaName"] == null)
            //{
            //    gridView.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            //    {
            //        FieldName = "diagnosaName",
            //        Caption = "diagnosaName",
            //        Visible = true
            //    });
            //}
            //gridView.OptionsView.ColumnAutoWidth = false;
            //gridView.Columns["diagnosaCode"].Width = 110; // Kolom pertama
            //gridView.Columns["diagnosaName"].Width = 530;
            //gridView.RowHeight = 27;
            //gridView.Appearance.Row.Font = new Font("Arial", 11, FontStyle.Regular);        // Baris data
            //gridView.Appearance.HeaderPanel.Font = new Font("Arial", 11, FontStyle.Bold);  // Header kolom
            //gridView.Appearance.FocusedRow.Font = new Font("Arial", 11, FontStyle.Regular);

            //LookDiagnosaGrid.PopupFormWidth = 700;
            //LookDiagnosaGrid.ImmediatePopup = true;
            //LookDiagnosaGrid.Appearance.Font = new Font("Arial", 11, FontStyle.Regular);
            //LookDiagnosaGrid.Appearance.Options.UseFont = true;
            //// Mengatur ukuran font pada dropdown
            //LookDiagnosaGrid.AppearanceDropDown.Font = new Font("Arial", 11, FontStyle.Regular);
            //LookDiagnosaGrid.AppearanceDropDown.Options.UseFont = true;

            //LookDiagnosaGrid.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            //LookDiagnosaGrid.NullText = "";
            //gridView4.Columns[2].ColumnEdit = LookDiagnosaGrid;

            RepositoryItemLookUpEdit diagnosaTypeLookup = new RepositoryItemLookUpEdit();
            diagnosaTypeLookup.DataSource = listDiagnosaType;
            diagnosaTypeLookup.ValueMember = "diagnosaTypeCode";
            diagnosaTypeLookup.DisplayMember = "diagnosaTypeName";

            diagnosaTypeLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            diagnosaTypeLookup.DropDownRows = listDiagnosaType.Count;
            diagnosaTypeLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            diagnosaTypeLookup.AutoSearchColumnIndex = 1;
            diagnosaTypeLookup.NullText = "";
            gridView4.Columns[3].ColumnEdit = diagnosaTypeLookup;


            gridView4.Columns[0].OptionsColumn.ReadOnly = true;
            gridView4.Columns[1].OptionsColumn.ReadOnly = true;
            gridView4.Columns[5].OptionsColumn.ReadOnly = true;
            gridView4.Columns[6].OptionsColumn.ReadOnly = true;
            gridView4.Columns[5].Visible = false;
            gridView4.Columns[6].Visible = false;
            gridView4.BestFitColumns();

            //if (gridView4.RowCount <= 0)
            //{
            //    btnAddDiag.Enabled = true;
            //    btnDelDiag.Enabled = false;
            //    btnSaveDiag.Enabled = false;
            //}
            //else
            //{
            //    btnAddDiag.Enabled = true;
            //    btnDelDiag.Enabled = true;
            //    btnSaveDiag.Enabled = true;
            //}


        }

        private void gridView2_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.Column.Caption == "Pasien")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[5]);
                if (kk == "BPJS")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.ForestGreen);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.ForestGreen);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            //if (e.Column.Caption == "Kecelakaan kerja")
            //{
            //    string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[4]);
            //    if (kk == "Yes")
            //    {
            //        e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
            //        e.Appearance.BackColor2 = Color.FromArgb(150, Color.OrangeRed);
            //        e.Appearance.ForeColor = Color.White;
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //    }
            //}
        }

        private void gridView2_RowClick(object sender, RowClickEventArgs e)
        {
            GridView View = sender as GridView;
            string s_rm = "";

            s_rm = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);

            if (s_rm == "")
            {
                btnCreate.Enabled = true;
            }
            else
            {
                btnCreate.Enabled = false;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string sql_insert = "";
            string rm_no = "", nik = "", grp = "", cd1 = "", cd2 = "", cd3 = "";

            nik = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[8]).ToString();
            grp = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[6]).ToString();

            cd1 = grp.Substring(0, 1);
            cd2 = nik.Substring(2);
            cd3 = DateTime.Now.ToString("yyMMdd");

            rm_no = cd1 + cd2 + cd3;

            sql_insert = " insert into KLINIK.cs_patient (rm_no, patient_no, group_patient, status, ins_date, ins_emp) values ('" + rm_no + "', '" + nik + "', '" + grp + "', 'A', sysdate, '" + DB.vUserId + "') ";
            try
            {
                OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
                OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect3);
                oraConnect3.Open();
                cm.ExecuteNonQuery();
                oraConnect3.Close();
                cm.Dispose();

                //MessageBox.Show(sql_insert);
                //MessageBox.Show("Query Exec : " + sql_insert);

                btnCreate.Enabled = false;
                MessageBox.Show("Data Berhasil disimpan.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAll.Checked == true)
            {
                checkP1.Checked = true;
                checkP2.Checked = true;
                checkP3.Checked = true;
                checkP4.Checked = true;
                checkP5.Checked = true;
            }
            else
            {
                checkP1.Checked = false;
                checkP2.Checked = false;
                checkP3.Checked = false;
                checkP4.Checked = false;
                checkP5.Checked = false;
            }
        }

        private void btnAddAnam_Click(object sender, EventArgs e)
        {
            gridView3.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView3.AddNewRow();
            btnAddAnam.Enabled = false;
            btnSaveAnam.Enabled = true;
        }

        private void gridView3_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[16], "I");

            gridView3.Columns[5].OptionsColumn.ReadOnly = true;
        }

        private void btnSaveAnam_Click(object sender, EventArgs e)
        {
            if (gridView3.RowCount < 1)
                return;

            string date = "", que = "", tensi = "", nadi = "", suhu = "", alergi = "", keluhan = "", action = "", rm_no = "", nik = "", status = "", kehamilan = "", bb = "", tb = "";
            string rnow = "", rold = "", rfam = "", pfisik = "", padd = "" ;
            string infop1 = "", infop2 = "", infop3 = "", infop4 = "", infop5 = "";
            string sql_update2 = "", sql_cnt = "", sql_insert = "", sql_update = "", anam_cnt = ""; 

            date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            status = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[7]).ToString();
            idvisit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[13]).ToString();

            tensi = gridView3.GetRowCellValue(0, gridView3.Columns[0]).ToString();
            nadi = gridView3.GetRowCellValue(0, gridView3.Columns[1]).ToString();
            suhu = gridView3.GetRowCellValue(0, gridView3.Columns[2]).ToString();
            alergi = gridView3.GetRowCellValue(0, gridView3.Columns[3]).ToString();
            keluhan = gridView3.GetRowCellValue(0, gridView3.Columns[4]).ToString();
            kehamilan = gridView3.GetRowCellValue(0, gridView3.Columns[17]).ToString();
            action = gridView3.GetRowCellValue(0, gridView3.Columns[16]).ToString();
            bb = gridView3.GetRowCellValue(0, gridView3.Columns[10]).ToString();
            tb = gridView3.GetRowCellValue(0, gridView3.Columns[19]).ToString();




            rnow = rNow.Text;
            rold = rOld.Text;
            rfam = rFam.Text;
            pfisik = pFisik.Text;
            padd = pAdd.Text;

            //if (rNow.Text == "Riwayat penyakit sekarang") { rnow = ""; } else { rnow = rNow.Text; }
            //if (rOld.Text == "Riwayat penyakit dahulu") { rold = ""; } else { rold = rOld.Text; }
            //if (rFam.Text == "Riwayat penyakit keluarga") { rfam = ""; } else { rfam = rFam.Text; }
            //if (pFisik.Text == "Pemeriksaan Fisik") { pfisik = ""; } else { pfisik = pFisik.Text; }
            //if (pAdd.Text == "Pemeriksaan Tambahan") { padd = ""; } else { padd = pAdd.Text; }

            if (checkP1.Checked == true) { infop1 = "Rujukan"; } else { infop1 = ""; }
            if (checkP2.Checked == true) { infop2 = "Tindakan"; } else { infop2 = ""; }
            if (checkP3.Checked == true) { infop3 = "Rekomendasi"; } else { infop3 = ""; }
            if (checkP4.Checked == true) { infop4 = "Observasi"; } else { infop4 = ""; }
            if (checkP5.Checked == true) { infop5 = "Terapi"; } else { infop5 = ""; }

            if (tensi == "")
            {
                MessageBox.Show("Tensi harus diisi"); return;
            }
            else if (nadi == "")
            {
                MessageBox.Show("Nadi harus diisi"); return;
            }
            else if (bb == "")
            {
                MessageBox.Show("BB harus diisi"); return;
            }
            else if (tb == "")
            {
                MessageBox.Show("TB harus diisi"); return;
            }
            else if (keluhan == "")
            {
                MessageBox.Show("Keluhan harus diisi"); return;
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

                        try
                        {

                            trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                            command.Connection = oraConnectTrans;
                            command.Transaction = trans;

                            command.CommandText = " insert into cs_anamnesa (anamnesa_id, rm_no, insp_date, blood_press, pulse, temperature, allergy, anamnesa, visit_no, disease_now, disease_then, disease_family, anamnesa_physical, anamnesa_other, infop1, infop2, infop3, infop4, infop5, info_k, bb, tb, ins_date, ins_emp, ID_VISIT) values(cs_anamnesa_seq.nextval, '" + rm_no + "', to_date('" + date + "', 'yyyy-mm-dd'), '" + tensi + "', '" + nadi + "', '" + suhu + "', '" + alergi + "', '" + keluhan + "', '" + que + "', '" + rnow + "', '" + rold + "', '" + rfam + "', '" + pfisik + "', '" + padd + "', '" + infop1 + "', '" + infop2 + "', '" + infop3 + "', '" + infop4 + "', '" + infop5 + "', '" + kehamilan + "', '" + bb + "', '" + tb + "', sysdate, '" + DB.vUserId + "', " + idvisit + ") ";
                            command.ExecuteNonQuery();

                            //if (status == "First Inspection")
                            //{
                            //    command.CommandText = " update cs_visit set status = 'INS', time_reservation=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where empid = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                            //    command.ExecuteNonQuery();
                            //}


                            trans.Commit();
                            //string cek = " insert into cs_anamnesa (anamnesa_id, rm_no, insp_date, blood_press, pulse, temperature, allergy, anamnesa, visit_no, disease_now, disease_then, disease_family, anamnesa_physical, anamnesa_other, infop1, infop2, infop3, infop4, infop5, ins_date, ins_emp) values(cs_anamnesa_seq.nextval, '" + rm_no + "', to_date('" + date + "', 'yyyy-mm-dd'), '" + tensi + "', '" + nadi + "', '" + suhu + "', '" + alergi + "', '" + keluhan + "', '" + que + "', '" + rnow + "', '" + rold + "', '" + rfam + "', '" + pfisik + "', '" + padd + "', '" + infop1 + "', '" + infop2 + "', '" + infop3 + "', '" + infop4 + "', '" + infop5 + "', sysdate, '" + DB.vUserId + "') ";
                            //MessageBox.Show(sql_insert);
                            //MessageBox.Show("Query Exec : " + cek);
                            //MessageBox.Show("Data Berhasil disimpan.");
                            labelControl122.Visible = true;
                            labelControl122.Text = "Verifikasi Berhasil";
                            Blinking(labelControl122, 1);
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

                    sql_update = sql_update + " update cs_anamnesa" +
                                 " set blood_press = '" + tensi + "', pulse = '" + nadi + "', " +
                                 " temperature = '" + suhu + "', allergy = '" + alergi + "', anamnesa = '" + keluhan + "',  " +
                                 " disease_now = '" + rnow + "', disease_then = '" + rold + "', " +
                                 " disease_family = '" + rfam + "', anamnesa_physical = '" + pfisik + "', anamnesa_other = '" + padd + "', " +
                                 " infop1 = '" + infop1 + "', infop2 = '" + infop2 + "', " +
                                 " infop3 = '" + infop3 + "', infop4 = '" + infop4 + "', " +
                                 " infop5 = '" + infop5 + "', info_k = '" + kehamilan + "', bb = '" + bb + "', tb = '" + tb + "', ";

                    sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                    sql_update = sql_update + " where rm_no = '" + rm_no + "' and  ID_VISIT = " + idvisit + " ";

                    OleDbConnection oraConnectTrans2 = ConnOra.Create_Connect_Ora();
                    OleDbCommand command2 = new OleDbCommand();
                    OleDbTransaction trans2 = null;

                    command2.Connection = oraConnectTrans2;
                    oraConnectTrans2.Open();

                    try
                    {
                        //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        //OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                        //oraConnect.Open();
                        //cm.ExecuteNonQuery();
                        //oraConnect.Close();
                        //cm.Dispose();

                        trans2 = oraConnectTrans2.BeginTransaction(IsolationLevel.ReadCommitted);
                        command2.Connection = oraConnectTrans2;
                        command2.Transaction = trans2;

                        command2.CommandText = sql_update;
                        command2.ExecuteNonQuery();

                        if (status == "First Inspection")
                        {
                            //command2.CommandText = " update cs_visit set status = 'INS', time_reservation=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where empid = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                            command2.CommandText = " update cs_visit set time_reservation=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + nik + "' and ID_VISIT = " + idvisit + "  ";
                            command2.ExecuteNonQuery();
                        }

                        //MessageBox.Show("Query Exec : " + sql_update);

                        //MessageBox.Show("Data Berhasil diupdate");
                        trans2.Commit();

                        labelControl122.Visible = true;
                        labelControl122.Text = "Verifikasi Berhasil";
                        Blinking(labelControl122, 1);

                        tmp_now = rnow;
                        tmp_old = rold;
                        tmp_fam = rfam;
                        tmp_fisik = pfisik;
                        tmp_add = padd;
                    }
                    catch (Exception ex)
                    {
                        trans2.Rollback();
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                    oraConnectTrans2.Close();
                }
            }
        }

        private void btnAddDiag_Click(object sender, EventArgs e)
        {
            gridView4.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView4.AddNewRow();


        }

        private void gridView4_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns[3], "P");
            view.SetRowCellValue(e.RowHandle, view.Columns[5], "I");
        }

        private void btnSaveDiag_Click(object sender, EventArgs e)
        {
            string date = "", que = "", diagnosa = "", tipe = "", remark = "", action = "", rm_no = "", nik = "", id = "";
            string sql_cnt = "", diag_cnt = "", sql_update = "", s_poli;
            int ssave = 0;
            string s_infop1 = "", s_infop2 = "", s_infop3 = "", s_infop4 = "", s_infop5 = "";

            date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            s_poli = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();
            for (int i = 0; i < gridView4.DataRowCount; i++)
            {
                diagnosa = gridView4.GetRowCellValue(i, gridView4.Columns[2]).ToString();
                tipe = gridView4.GetRowCellValue(i, gridView4.Columns[3]).ToString();
                remark = gridView4.GetRowCellValue(i, gridView4.Columns[4]).ToString();
                action = gridView4.GetRowCellValue(i, gridView4.Columns[5]).ToString();
                id = gridView4.GetRowCellValue(i, gridView4.Columns[6]).ToString();

                if (diagnosa == "")
                {
                    MessageBox.Show("Diagnosa harus diisi");
                }
                else if (tipe == "")
                {
                    MessageBox.Show("Tipe Diagnosa harus diisi");
                }
                else
                {
                    if (action == "I")
                    {
                        sql_cnt = " select count(0) cnt from KLINIK.cs_diagnosa where to_char(insp_date,'yyyy-mm-dd') = '" + today + "' and visit_no = '" + que + "' and rm_no = '" + rm_no + "' " + " and item_cd = '" + diagnosa + "' and ANAMNESA_ID =  " + v_anamnesa + " ";
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

                                command.CommandText = " insert into cs_diagnosa (diagnosa_id, rm_no, insp_date, item_cd, type_diagnosa, remark, visit_no, ins_date, ins_emp, ANAMNESA_ID) values(cs_diagnosa_seq.nextval, '" + rm_no + "', to_date('" + date + "', 'yyyy-mm-dd'), '" + diagnosa + "', '" + tipe + "', '" + remark + "', '" + que + "', sysdate, '" + DB.vUserId + "', " + v_anamnesa + ") ";
                                command.ExecuteNonQuery();

                                //command.CommandText = " update cs_visit set status = 'NUR', time_reservation=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where empid = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                                //command.ExecuteNonQuery();

                                trans.Commit();
                                ssave = 1;
                                //MessageBox.Show(sql_insert);
                                //MessageBox.Show("Query Exec : " + sql_insert);
                                //MessageBox.Show("Data Berhasil disimpan.");
                               
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

                        sql_update = sql_update + " update cs_diagnosa" +
                                     " set item_cd = '" + diagnosa + "', type_diagnosa = '" + tipe + "', remark = '" + remark + "', ";
                        sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                        sql_update = sql_update + " where rm_no = '" + rm_no + "' and to_char(insp_date,'yyyy-mm-dd') = '" + date + "' and visit_no = '" + que + "' and diagnosa_id = '" + id + "' ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            ssave = 0;
                            //MessageBox.Show("Query Exec : " + sql_update);

                            //MessageBox.Show("Data Berhasil diupdate");
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
            }
            if (checkP1.Checked)
                s_infop1 = "Rujukan";
            else
                s_infop1 = "";

            if (checkP2.Checked)
                s_infop2 = "Tindakan";
            else
                s_infop2 = "";

            if (checkP3.Checked)
                s_infop3 = "Rawat Inap";
            else
                s_infop3 = "";

            if (checkP4.Checked)
                s_infop4 = "Observasi";
            else
                s_infop4 = "";

            if (checkP5.Checked)
                s_infop5 = "Terapi";
            else
                s_infop5 = "";

            sql_update = "";
            sql_update = sql_update + " update KLINIK.cs_anamnesa" +
                         " set  " +
                         " infop1 = '" + s_infop1 + "', infop2 = '" + s_infop2 + "', " +
                         " infop3 = '" + s_infop3 + "', infop4 = '" + s_infop4 + "', " +
                         " infop5 = '" + s_infop5 + "'  " + 
                         " where rm_no = '" + rm_no + "' and to_char(insp_date,'yyyy-mm-dd') = '" + date + "' and ANAMNESA_ID =  " + v_anamnesa + " ";

            ConnOra.ExeNonQuery(sql_update);

            if (ssave == 0)
            {
                labelControl189.Visible = true;
                labelControl189.Text = "Update Diagnosa Berhasil";
                Blinking(labelControl189, 1);
            }
            else if (ssave == 2)
            {

                labelControl189.Visible = true;
                labelControl189.Text = "Simpan Diagnosa Berhasil";
                Blinking(labelControl189, 1);
            }

            if (gridView4.RowCount <= 0)
            {
                btnAddDiag.Enabled = true;
                btnDelDiag.Enabled = false;
                btnSaveDiag.Enabled = false;
            }
            else
            {
                btnAddDiag.Enabled = true;
                btnDelDiag.Enabled = true;
                btnSaveDiag.Enabled = true;
            }

            pelayanandefault();

            LoadDiagnosa(rm_no, date, que, s_poli);
        }
        private void pelayanandefault()
        {
            string date = "", que = "", rm_no = "", pasno = "", nama_laya = "", status = "", remark = "", action = "", stbyr = "", insu_flag = "", pid_visit = "", headid = "", policd = "", sql_visit = "";
            string sql_cnt = "", diag_cnt = "", sql_update = "", sstatvisit = "";
            int stsimpan = 0;

            date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            policd = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();

            pid_visit = idvisit;

            //for (int i = 0; i < gridView13.DataRowCount; i++)
            //{
            nama_laya = "TRT01";// gridView13.GetRowCellValue(i, gridView13.Columns[6]).ToString();
            status = "OPN"; // gridView13.GetRowCellValue(i, gridView13.Columns[7]).ToString();
            remark = "";// gridView13.GetRowCellValue(i, gridView13.Columns[8]).ToString();
            action = "I";// gridView13.GetRowCellValue(i, gridView13.Columns[9]).ToString();
            stbyr = "OPN"; // gridView13.GetRowCellValue(i, gridView13.Columns[10]).ToString();
            insu_flag = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[15]).ToString();

            sql_visit = " select status from KLINIK.cs_visit where ID_VISIT =" + idvisit + " ";
            OleDbConnection oraConnects2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOras2 = new OleDbDataAdapter(sql_visit, oraConnects2);
            DataTable dts2 = new DataTable();
            adOras2.Fill(dts2);
            if (dts2.Rows.Count > 0)
            {
                sstatvisit = dts2.Rows[0]["status"].ToString();

                if (!sstatvisit.ToString().Equals("PRE") && !sstatvisit.ToString().Equals("RSV") && !sstatvisit.ToString().Equals("INS") && !sstatvisit.ToString().Equals("NUR"))
                    return;
            }

            if (insu_flag.ToString().Equals("Asuransi"))
                insu_flag = "A";
            else if (insu_flag.ToString().Equals("Umum"))
                insu_flag = "U";
            else if (insu_flag.ToString().Equals("BPJS"))
                insu_flag = "B";

            string sql_cek = "", seq_va = "";
             
             
            sql_cek = sql_cek + Environment.NewLine + "   select nvl(max(b.detail_id),0) seq ";
            sql_cek = sql_cek + Environment.NewLine + "    from KLINIK.cs_treatment_head a  ";
            sql_cek = sql_cek + Environment.NewLine + "    join KLINIK.cs_treatment_detail b on (a.head_id=b.head_id)  ";
            sql_cek = sql_cek + Environment.NewLine + "    join KLINIK.cs_treatment_item c on (b.treat_item_id=c.treat_item_id)  ";
            sql_cek = sql_cek + Environment.NewLine + "    where 1=1 ";
            sql_cek = sql_cek + Environment.NewLine + "    and id_visit =" + idvisit + " "; 

            OleDbConnection oraConD = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra2D = new OleDbDataAdapter(sql_cek, oraConD);
            DataTable dt2D = new DataTable();
            adOra2D.Fill(dt2D);
            seq_va = dt2D.Rows[0]["seq"].ToString();

            if (Convert.ToInt32(seq_va) > 0)
            {
                return;
            }


            sql_cnt = " select count(0) cnt, max(head_id) headid from KLINIK.cs_treatment_head where ID_VISIT =" + pid_visit + " ";
            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);
            diag_cnt = dt.Rows[0]["cnt"].ToString();
            if (Convert.ToInt32(diag_cnt) > 0)
            {
                headid = dt.Rows[0]["headid"].ToString();
                OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                OleDbCommand command = new OleDbCommand();
                OleDbTransaction trans = null;


                command.Connection = oraConnectTrans;
                oraConnectTrans.Open();
                try
                {
                    string sql_seq2 = "", seq_val2 = "", sql_tmp = "", sql_seq = "", seq_val = "";
                    trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                    command.Connection = oraConnectTrans;
                    command.Transaction = trans;

                    sql_tmp = " ";
                    sql_tmp = sql_tmp + "insert into KLINIK.cs_treatment_detail ";
                    sql_tmp = sql_tmp + "select CS_TREATMENT_DETAIL_SEQ.nextval det_id, " + headid + " head_id,  b.treat_item_id, to_date('" + date.ToString().Substring(0, 10) + "', 'yyyy-mm-dd') visit_date, ";
                    sql_tmp = sql_tmp + "     1 treat_qty, 'Initial' remark, sysdate ins_date, '" + DB.vUserId + "' ins_emp, ";
                    sql_tmp = sql_tmp + "  null upd_date, null upd_emp, b.treat_item_price, b.treat_item_price total_price, TO_CHAR(sysdate,'HH24:MI') jam, 'gridView13' GRID_NAME, '" + ConnOra.v_iddokter + "' ID_DOKTER, null att1, null att2, 'Y' F_ACTIVE ";
                    sql_tmp = sql_tmp + "  from KLINIK.cs_treatment_type a ";
                    sql_tmp = sql_tmp + "  join KLINIK.cs_treatment_item b on (a.treat_type_id=b.treat_type_id) ";
                    sql_tmp = sql_tmp + "  join KLINIK.cs_treatment_group c on (b.treat_group_id=c.treat_group_id) ";
                    sql_tmp = sql_tmp + " where 1=1";
                    sql_tmp = sql_tmp + "   and default_st='Y' "; 
                    sql_tmp = sql_tmp + "and a.treat_type_id = 'TRT01' ";
                    sql_tmp = sql_tmp + "and b.treat_group_id = 'TRG07' and b.F_STATUS ='" + insu_flag + "'";

                    command.CommandText = sql_tmp;
                    command.ExecuteNonQuery();

                    trans.Commit();
                    stsimpan = 1;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("ERROR: " + ex.Message);
                }

                oraConnectTrans.Close();

            }
            else
            {
                string sql_seq = "", seq_val = "", sql_tmp = "";




                sql_seq = " select CS_TREATMENT_HEAD_SEQ.nextval seq from dual ";
                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_seq, oraConnect2);
                DataTable dt2 = new DataTable();
                adOra2.Fill(dt2);
                seq_val = dt2.Rows[0]["seq"].ToString();

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
                    //DB.vUserId = "1";
                    if (insu_flag.ToString().Equals("A"))
                        insu_flag = "A";
                    else if (insu_flag.ToString().Equals("B"))
                        insu_flag = "B";
                    else
                        insu_flag = "U";
                    command.CommandText = " insert into KLINIK.cs_treatment_head (head_id, rm_no, patient_no, visit_date, visit_no, treat_type_id, status, remarks, pay_status, insu_flag, ins_date, ins_emp,ID_VISIT) values ('" + seq_val + "', '" + rm_no + "', '" + pasno + "', to_date('" + date + "', 'yyyy-mm-dd'), '" + que + "', '" + nama_laya + "', 'OPN', '" + remark + "', 'OPN', '" + insu_flag + "', sysdate, '" + DB.vUserId + "', '" + pid_visit + "') ";
                    command.ExecuteNonQuery();

                    sql_tmp = " ";
                    sql_tmp = sql_tmp + "insert into KLINIK.cs_treatment_detail ";
                    sql_tmp = sql_tmp + "select CS_TREATMENT_DETAIL_SEQ.nextval det_id, " + seq_val + " head_id,  b.treat_item_id, to_date('" + date.ToString().Substring(0, 10) + "', 'yyyy-mm-dd') visit_date, ";
                    sql_tmp = sql_tmp + "     1 treat_qty, 'Initial' remark, sysdate ins_date, '" + DB.vUserId + "' ins_emp, ";
                    sql_tmp = sql_tmp + "  null upd_date, null upd_emp, b.treat_item_price, b.treat_item_price total_price, TO_CHAR(sysdate,'HH24:MI') jam, 'gridView13' GRID_NAME, '" + ConnOra.v_iddokter + "' ID_DOKTER, null att1, null att2, 'Y' F_ACTIVE ";
                    sql_tmp = sql_tmp + "  from KLINIK.cs_treatment_type a ";
                    sql_tmp = sql_tmp + "  join KLINIK.cs_treatment_item b on (a.treat_type_id=b.treat_type_id) ";
                    sql_tmp = sql_tmp + "  join KLINIK.cs_treatment_group c on (b.treat_group_id=c.treat_group_id) ";
                    sql_tmp = sql_tmp + " where 1=1";
                    sql_tmp = sql_tmp + "   and default_st='Y' ";
                    sql_tmp = sql_tmp + "and a.treat_type_id = 'TRT01' ";
                    sql_tmp = sql_tmp + "and b.treat_group_id = 'TRG07' and b.F_STATUS ='" + insu_flag + "'";

                    command.CommandText = sql_tmp;
                    command.ExecuteNonQuery();

                    trans.Commit();
                    //MessageBox.Show(sql_insert);
                    //MessageBox.Show("Query Exec : " + sql_insert);
                    //MessageBox.Show("Data Berhasil disimpan.");
                    stsimpan = 1;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("ERROR: " + ex.Message);
                }

                oraConnectTrans.Close();
            }
        }

        private void gridView4_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveDiag.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Diagnosa" || e.Column.Caption == "Tipe" || e.Column.Caption == "Remark")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[5]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "U");
                }
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

                id = gridView4.GetRowCellValue(gridView4.FocusedRowHandle, gridView4.Columns[6]).ToString();

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
                    gridView4.DeleteRow(gridView4.FocusedRowHandle);
                    MessageBox.Show("Data Berhasil didelete");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }

        }

        private void btnLoadRujukan_Click(object sender, EventArgs e)
        {
            string sql_load = "";
            string s_rm = "", s_que = "", s_date = "", p_rm = "", p_que = "", p_date = "", p_name = "", p_ref_date = "", p_hos_doc = "", p_hos_name = "", p_anamnesa = "", p_diagnosa = "";
            string p_nik = "", p_address = "", p_age = "", p_gender = "", p_riwayat = "", p_resep = "", p_ref_id = "", p_no = "";

            s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();

            sql_load = sql_load + Environment.NewLine + "select a.patient_no, a.name, a.address, round(((sysdate-birth_date)/30)/12) age,  ";
            sql_load = sql_load + Environment.NewLine + "decode(a.gender,'L','Laki-Laki','Perempuan') gender,  ";
            sql_load = sql_load + Environment.NewLine + "c.rm_no, to_char(b.visit_date,'yyyy-mm-dd') visit_date, que01, refer_id, ";
            sql_load = sql_load + Environment.NewLine + "decode(work_accident,'N','Bukan Kecelakaan Kerja','Kecelakaan Kerja') work_accident,  ";
            sql_load = sql_load + Environment.NewLine + "to_char(letter_dt,'mm') || '/' || to_char(letter_dt,'yyyy') mm, ";
            sql_load = sql_load + Environment.NewLine + "(select 'BB:' || bb || ', TB:' || tb || ', Keluhan:' || anamnesa as anamnesa  ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_anamnesa  ";
            sql_load = sql_load + Environment.NewLine + "where rm_no=c.rm_no  ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)   ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) anamnesa,   ";
            sql_load = sql_load + Environment.NewLine + "(select disease_now || ' - ' || disease_then     ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_anamnesa  ";
            sql_load = sql_load + Environment.NewLine + "where rm_no=c.rm_no  ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)   ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) riwayat,   ";
            sql_load = sql_load + Environment.NewLine + "(select LISTAGG(initcap(item_name), ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa   ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_diagnosa a   ";
            sql_load = sql_load + Environment.NewLine + "join KLINIK.cs_diagnosa_item b on (a.item_cd=b.item_cd)   ";
            sql_load = sql_load + Environment.NewLine + "where b.status='A'   ";
            sql_load = sql_load + Environment.NewLine + "and rm_no=c.rm_no   ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)  ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) as diagnosa,  ";
            sql_load = sql_load + Environment.NewLine + "(select LISTAGG(initcap(med_name), ', ') WITHIN GROUP (ORDER BY med_name asc) resep    ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_receipt a  ";
            sql_load = sql_load + Environment.NewLine + "join KLINIK.cs_medicine b on (a.med_cd=b.med_cd)   ";
            sql_load = sql_load + Environment.NewLine + "where b.status='A'   ";
            sql_load = sql_load + Environment.NewLine + "and rm_no=c.rm_no   ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)  ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) as resep, ";
            sql_load = sql_load + Environment.NewLine + "to_char(nvl(letter_dt,sysdate),'yyyy-mm-dd') letter_dt, hos_doc, hos_name, letter_no,  ";
            sql_load = sql_load + Environment.NewLine + "TO_CHAR(letter_dt, 'dd Month yyyy','nls_date_language = INDONESIAN') letter_dt2  ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_patient_info a   ";
            sql_load = sql_load + Environment.NewLine + "join KLINIK.cs_visit b on (a.patient_no = b.patient_no)   ";
            sql_load = sql_load + Environment.NewLine + "join KLINIK.cs_patient c on(b.patient_no = c.patient_no)   ";
            sql_load = sql_load + Environment.NewLine + "left join KLINIK.cs_refer d on (c.rm_no = d.rm_no and trunc(visit_date)=d.insp_date and que01=d.visit_no)   ";
            sql_load = sql_load + Environment.NewLine + "where id_visit = " + idvisit.ToString() + "   ";
            sql_load = sql_load + Environment.NewLine + "and c.status = 'A'   ";
            //sql_load = sql_load + Environment.NewLine + "and b.que01 = '" + s_que + "'   ";
            //sql_load = sql_load + Environment.NewLine + "and c.group_patient <> 'COMM'   ";
            sql_load = sql_load + Environment.NewLine + "and c.rm_no = '" + s_rm + "' ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_load, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            p_rm = dt.Rows[0]["rm_no"].ToString();
            p_que = dt.Rows[0]["que01"].ToString();
            p_date = dt.Rows[0]["visit_date"].ToString();
            p_ref_id = dt.Rows[0]["refer_id"].ToString();

            p_nik = dt.Rows[0]["patient_no"].ToString();
            p_name = dt.Rows[0]["name"].ToString();
            p_address = dt.Rows[0]["address"].ToString();
            p_age = dt.Rows[0]["age"].ToString();
            p_gender = dt.Rows[0]["gender"].ToString();

            p_anamnesa = dt.Rows[0]["anamnesa"].ToString();
            p_riwayat = dt.Rows[0]["riwayat"].ToString();
            p_diagnosa = dt.Rows[0]["diagnosa"].ToString();
            p_resep = dt.Rows[0]["resep"].ToString();

            p_no = dt.Rows[0]["letter_no"].ToString();
            p_ref_date = dt.Rows[0]["letter_dt"].ToString();
            p_hos_doc = dt.Rows[0]["hos_doc"].ToString();
            p_hos_name = dt.Rows[0]["hos_name"].ToString();

            lRefName.Text = p_name;
            lRefAddr.Text = p_address;
            lRefAnam.Text = p_anamnesa;
            lRefHis.Text = p_riwayat;
            lRefDiag.Text = p_diagnosa;
            lRefRec.Text = p_resep;

            tRefNo.Text = p_no;
            dRefDate.Text = p_ref_date;
            tRefDoc.Text = p_hos_doc;
            tRefHos.Text = p_hos_name;

            lRefRm.Text = p_rm;
            lRefQue.Text = p_que;
            lRefDate.Text = p_date;
            lRefID.Text = p_ref_id;

            if (p_hos_doc == "")
            {
                btnRefPrint.Enabled = false;
                btnRefDel.Enabled = false;
            }
            else
            {
                btnRefPrint.Enabled = true;
                btnRefDel.Enabled = true;
            }
            dsRujukan.Tables.Clear();
            dsRujukan.Tables.Add(dt);
        } 

        private void loadTindakan_Click(object sender, EventArgs e)
        {
            lActName.Text = pub_nama;


            string SQL = "", date = "", que = "", rm_no = "";

            date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();

            SQL = "";
            SQL = SQL + Environment.NewLine + " select act_id, to_char(a.insp_date,'yyyy-mm-dd') insp_date, treat_item_id  ";
            SQL = SQL + Environment.NewLine + " from KLINIK.cs_action a ";
            SQL = SQL + Environment.NewLine + " join KLINIK.cs_treatment_detail b on (a.detail_id=b.detail_id) ";
            SQL = SQL + Environment.NewLine + " where 1=1   ";
            SQL = SQL + Environment.NewLine + " and rm_no = '" + rm_no + "'   ";
            SQL = SQL + Environment.NewLine + " and to_char(a.visit_dt, 'yyyy-mm-dd') = '" + date + "'   ";
            SQL = SQL + Environment.NewLine + " and visit_no = '" + que + "'  ";
            SQL = SQL + Environment.NewLine + " order by a.insp_date  ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl15.DataSource = null;
            gridView15.Columns.Clear();
            gridControl15.DataSource = dt;

            gridView15.OptionsView.ColumnAutoWidth = true;
            gridView15.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView15.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView15.IndicatorWidth = 40;
            gridView15.OptionsBehavior.Editable = false;

            gridView15.Columns[0].Caption = "ID";
            gridView15.Columns[1].Caption = "Tanggal";
            gridView15.Columns[2].Caption = "Nama Tindakan";

            RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
            glLaya.DataSource = listLaya3;
            glLaya.ValueMember = "layananCode";
            glLaya.DisplayMember = "layananName";

            glLaya.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glLaya.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glLaya.ImmediatePopup = true;
            glLaya.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glLaya.NullText = "";
            gridView15.Columns[2].ColumnEdit = glLaya;

            gridView15.Columns[0].Visible = false;
            gridView15.Columns[1].MinWidth = 70;
            gridView15.Columns[1].MaxWidth = 70;

            mActName.Text = "";
            mActRemark.Text = "";

            if (gridView15.RowCount > 0)
            {
                btnActSave.Enabled = true;
            }
            else
            {
                btnActSave.Enabled = false;
            }
        } 
        private void btnActSave_Click(object sender, EventArgs e)
        {
            string sql_update = "";

            sql_update = " update cs_action set act_name = '" + mActName.Text + "', act_remark = '" + mActRemark.Text + "', upd_emp='" + DB.vUserId + "', upd_date = sysdate " +
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

        private void btnActDel_Click(object sender, EventArgs e)
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

                sql_delete = " delete from KLINIK.cs_action where act_id = '" + lActID.Text + "'  ";

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

        private void btnActPrint_Click(object sender, EventArgs e)
        {
            ReportAction report = new ReportAction(dsAction);
            report.ShowPreviewDialog();
        }

        private void btnRefSave_Click(object sender, EventArgs e)
        {
            string sql_cnt = "";
            string ref_cnt = "";

            if (lRefRm.Text == "")
            {
                MessageBox.Show("Silahkan load data pasien");
            }
            else if (tRefDoc.Text == "")
            {
                MessageBox.Show("Nama Dokter Rujukan harus diisi");
            }
            else if (tRefHos.Text == "")
            {
                MessageBox.Show("Nama Rumah Sakit Rujukan harus diisi");
            }
            else
            {
                sql_cnt = " select count(0) cnt from KLINIK.cs_refer where to_char(insp_date,'yyyy-mm-dd') = '" + lRefDate.Text + "' and visit_no = '" + lRefQue.Text + "' and rm_no = '" + lRefRm.Text + "' ";
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);
                ref_cnt = dt.Rows[0]["cnt"].ToString();
                if (Convert.ToInt32(ref_cnt) > 0)
                {
                    // update data

                    string sql_update = "";

                    sql_update = " update cs_refer set  letter_dt = to_date('" + dRefDate.Text + "','yyyy-mm-dd'), hos_doc = '" + tRefDoc.Text + "', hos_name = '" + tRefHos.Text + "', letter_no = '" + tRefNo.Text + "',upd_emp='" + DB.vUserId + "', upd_date = sysdate " +
                                 " where refer_id='" + lRefID.Text + "'  ";

                    try
                    {
                        OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm3 = new OleDbCommand(sql_update, oraConnect3);
                        oraConnect3.Open();
                        cm3.ExecuteNonQuery();
                        oraConnect3.Close();
                        cm3.Dispose();

                        //MessageBox.Show("Query Exec : " + sql_delete); labelControl222

                        //MessageBox.Show("Data Berhasil dirubah");
                        labelControl222.Visible = true;
                        labelControl222.Text = "Data Rujukan Berhasil di ubah.";
                        Blinking(labelControl222, 1);
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

                    sql_insert = " insert into cs_refer (refer_id, rm_no, insp_date, letter_dt, hos_doc, hos_name, letter_no, visit_no, ins_date, ins_emp)  " +
                                 " values (cs_refer_seq.nextval,'" + lRefRm.Text + "',to_date('" + lRefDate.Text + "','yyyy-mm-dd'),to_date('" + dRefDate.Text + "','yyyy-mm-dd'),'" + tRefDoc.Text + "','" + tRefHos.Text + "','" + tRefNo.Text + "','" + lRefQue.Text + "',sysdate,'" + DB.vUserId + "')  ";

                    try
                    {
                        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm2 = new OleDbCommand(sql_insert, oraConnect2);
                        oraConnect2.Open();
                        cm2.ExecuteNonQuery();
                        oraConnect2.Close();
                        cm2.Dispose();

                        //MessageBox.Show("Query Exec : " + sql_delete);

                        //MessageBox.Show("Data Berhasil ditambah");
                        labelControl222.Visible = true;
                        labelControl222.Text = "Data Rujukan Berhasil di Buat.";
                        Blinking(labelControl222, 1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }
            }
        }

        private void btnRefDel_Click(object sender, EventArgs e)
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

                sql_delete = " delete from KLINIK.cs_refer where refer_id = '" + lRefID.Text + "'  ";

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

        private void loadRekomendasi_Click(object sender, EventArgs e)
        {
            string sql_load = "";
            string s_rm = "", s_que = "", s_date = "", p_rm = "", p_que = "", p_date = "", p_name = "", p_nik = "", p_dept = "", p_age = "", p_poli = "";
            string p_alamat = "", p_hamil = "", p_minggu = "", p_anak = "", p_hpht = "", p_no_surat = "";
            string p_recom01 = "", p_recom02 = "", p_recom03 = "", p_recom04 = "";
            string p_rekom_dt = "", p_rekom = "", p_rekom_id = "";

            s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();

            sql_load = "";
            sql_load = sql_load + Environment.NewLine + "SELECT x.patient_no, x.NAME, x.line, x.age, x.rm_no, x.poli_cd, x.visit_date, ";
            sql_load = sql_load + Environment.NewLine + "       que01, recom_id, address, letter_no, info01, info02, info03, info05, ";
            sql_load = sql_load + Environment.NewLine + "       NVL (recom_01, 'N') recom_01, NVL (recom_02, 'N') recom_02, ";
            sql_load = sql_load + Environment.NewLine + "       NVL (recom_03, 'N') recom_03, NVL (recom_04, 'N') recom_04, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (NVL (letter_dt, SYSDATE), 'yyyy-mm-dd') letter_dt,recom_remark, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (letter_dt,'dd fmMonth yyyy','nls_date_language = INDONESIAN') letter_dt2 ";
            sql_load = sql_load + Environment.NewLine + "  FROM (SELECT a.patient_no, a.NAME, null line, round(((sysdate-birth_date)/30)/12) age, c.rm_no, b.poli_cd, ";
            sql_load = sql_load + Environment.NewLine + "               TO_CHAR (b.visit_date, 'yyyy-mm-dd') visit_date, address, ";
            sql_load = sql_load + Environment.NewLine + "               que01, info01, info02, info03, info05 ";
            sql_load = sql_load + Environment.NewLine + "          FROM KLINIK.cs_patient_info a JOIN cs_visit b ON (a.patient_no = b.patient_no) ";
            sql_load = sql_load + Environment.NewLine + "               JOIN KLINIK.cs_patient c ON (b.patient_no = c.patient_no) ";
            sql_load = sql_load + Environment.NewLine + "         WHERE c.status = 'A' AND c.group_patient <> 'COMM') x ";
            sql_load = sql_load + Environment.NewLine + "  LEFT JOIN cs_recommendation y  ";
            sql_load = sql_load + Environment.NewLine + "       ON ( x.rm_no = y.rm_no AND TO_DATE (x.visit_date, 'yyyy-mm-dd') = y.insp_date AND x.que01 = y.visit_no ) ";
            sql_load = sql_load + Environment.NewLine + " WHERE 1 = 1  ";
            //sql_load = sql_load + Environment.NewLine + "   AND  que01 = '" + s_que + "'  ";
            //sql_load = sql_load + Environment.NewLine + "   AND  x.visit_date = '" + s_date + "'  ";
            sql_load = sql_load + Environment.NewLine + "   AND (recom_id is not null or rownum<=1)";
            sql_load = sql_load + Environment.NewLine + "   AND x.rm_no = '" + s_rm + "' ";

            //sql_load = " select a.empid, a.name, a.line, a.age,  " +
            //           " c.rm_no, b.poli_cd, to_char(b.visit_date,'yyyy-mm-dd') visit_date, que01, recom_id, " +
            //           " address, letter_no, info01, info02, info03, info05, " +
            //           " nvl(recom_01, 'N') recom_01,  " +
            //           " nvl(recom_02, 'N') recom_02,  " +
            //           " nvl(recom_03, 'N') recom_03,  " +
            //           " nvl(recom_04, 'N') recom_04,  " +









            //           " to_char(nvl(letter_dt,sysdate),'yyyy-mm-dd') letter_dt, recom_remark, " +
            //           " TO_CHAR(letter_dt, 'dd fmMonth yyyy','nls_date_language = INDONESIAN') letter_dt2  " +
            //           " from cs_employees a  " +
            //           " join cs_visit b on (a.empid = b.empid)  " +
            //           " join cs_patient c on(b.empid = c.empid)  " +
            //           " left join cs_recommendation d on (c.rm_no=d.rm_no) " +
            //           " where  1=1  " +
            //           //" and to_char(b.visit_date, 'yyyy-mm-dd') = '" + s_date + "' " +
            //           " and c.status = 'A'  " +
            //           //" and b.que01 = '" + s_que + "'  " +
            //           " and c.group_patient <> 'COMM'  " +
            //           " and c.rm_no = '" + s_rm + "' ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_load, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);


            p_rm = dt.Rows[0]["rm_no"].ToString();
            p_que = dt.Rows[0]["que01"].ToString();
            p_date = dt.Rows[0]["visit_date"].ToString();
            p_rekom_id = dt.Rows[0]["recom_id"].ToString();


            p_poli = dt.Rows[0]["poli_cd"].ToString();
            p_name = dt.Rows[0]["name"].ToString();
            p_nik = dt.Rows[0]["patient_no"].ToString();
            p_dept = dt.Rows[0]["line"].ToString();
            p_age = dt.Rows[0]["age"].ToString();

            p_alamat = dt.Rows[0]["address"].ToString();
            p_hamil = dt.Rows[0]["info01"].ToString();
            p_minggu = dt.Rows[0]["info02"].ToString();
            p_anak = dt.Rows[0]["info03"].ToString();
            p_hpht = dt.Rows[0]["info05"].ToString();

            p_no_surat = dt.Rows[0]["letter_no"].ToString();
            p_rekom_dt = dt.Rows[0]["letter_dt"].ToString();
            p_rekom = dt.Rows[0]["recom_remark"].ToString();

            p_recom01 = dt.Rows[0]["recom_01"].ToString();
            p_recom02 = dt.Rows[0]["recom_02"].ToString();
            p_recom03 = dt.Rows[0]["recom_03"].ToString();
            p_recom04 = dt.Rows[0]["recom_04"].ToString();

            lRecPoli.Text = p_poli;
            lRecName.Text = p_name;
            lRecNik.Text = p_nik;
            lRecDept.Text = p_dept;
            lRecAge.Text = p_age;
            lRecAddr.Text = p_alamat;
            lRecHamil.Text = p_hamil;
            lRecMinggu.Text = p_minggu;
            lRecAnak.Text = p_anak;
            lRecHPHT.Text = p_hpht;

            tRecNo.Text = p_no_surat;
            dRecDate.Text = p_rekom_dt;

            if (p_recom01 == "N") { cRecom01.Checked = false; } else { cRecom01.Checked = true; }
            if (p_recom02 == "N") { cRecom02.Checked = false; } else { cRecom02.Checked = true; }
            if (p_recom03 == "N") { cRecom03.Checked = false; } else { cRecom03.Checked = true; }
            if (p_recom04 == "N") { cRecom04.Checked = false; } else { cRecom04.Checked = true; }




            lRecRm.Text = p_rm;
            lRecQue.Text = p_que;
            lRecDate.Text = p_date;
            lRecID.Text = p_rekom_id;

            if (p_rekom == "")
            {
                btnRecPrint.Enabled = false;
                btnRecDel.Enabled = false;
            }
            else
            {
                btnRecPrint.Enabled = true;
                btnRecDel.Enabled = true;
            }

            if (dt.Rows.Count > 0)
            {
                dsRekomendasi.Tables.Clear();
                dsRekomendasi.Tables.Add(dt);
            }
            else
            {

                dsRekomendasi.Tables.Clear();
            }



        }

        private void btnRecSave_Click(object sender, EventArgs e)
        {
            string sql_cnt = "";
            string rec_cnt = "";

            if (lRecRm.Text == "")
            {
                MessageBox.Show("Silahkan load data pasien");
            }
            else if (tRecNo.Text == "")
            {
                MessageBox.Show("No Surat harus diisi");
            }
            else if (dRecDate.Text == "")
            {
                MessageBox.Show("Tanggal Surat harus diisi");
            }
            else if (cRecom01.Checked == false && cRecom02.Checked == false && cRecom03.Checked == false && cRecom04.Checked == false)
            {
                MessageBox.Show("Silahkan pilih rekomendasi");
            }
            else
            {
                sql_cnt = " select count(0) cnt from KLINIK.cs_recommendation where  rm_no = '" + lRecRm.Text + "' ";
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);
                rec_cnt = dt.Rows[0]["cnt"].ToString();
                if (Convert.ToInt32(rec_cnt) > 0)
                {
                    // update data

                    string tmp_recom01 = (cRecom01.Checked == false) ? "N" : "Y";
                    string tmp_recom02 = (cRecom02.Checked == false) ? "N" : "Y";
                    string tmp_recom03 = (cRecom03.Checked == false) ? "N" : "Y";
                    string tmp_recom04 = (cRecom04.Checked == false) ? "N" : "Y";

                    string sql_update = "";

                    sql_update = " update cs_recommendation set letter_no = '" + tRecNo.Text + "', letter_dt = to_date('" + dRecDate.Text + "','yyyy-mm-dd'), recom_01 = '" + tmp_recom01 + "', recom_02 = '" + tmp_recom02 + "', recom_03 = '" + tmp_recom03 + "', recom_04 = '" + tmp_recom04 + "', upd_emp='" + DB.vUserId + "', upd_date = sysdate " +
                                 " where recom_id='" + lRecID.Text + "'  ";

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

                    string tmp_recom01 = (cRecom01.Checked == false) ? "N" : "Y";
                    string tmp_recom02 = (cRecom02.Checked == false) ? "N" : "Y";
                    string tmp_recom03 = (cRecom03.Checked == false) ? "N" : "Y";
                    string tmp_recom04 = (cRecom04.Checked == false) ? "N" : "Y";

                    string sql_insert = "";

                    sql_insert = " insert into cs_recommendation (recom_id, rm_no, insp_date, letter_dt, poli_cd, recom_remark, letter_no, recom_01, recom_02, recom_03, recom_04, visit_no, ins_date, ins_emp)  " +
                                 " values (cs_recom_seq.nextval,'" + lRecRm.Text + "',to_date('" + lRecDate.Text + "','yyyy-mm-dd'),to_date('" + dRecDate.Text + "','yyyy-mm-dd'),'" + lRecPoli.Text + "','Pregnant', '" + tRecNo.Text + "', '" + tmp_recom01 + "', '" + tmp_recom02 + "', '" + tmp_recom03 + "', '" + tmp_recom04 + "','" + lRecQue.Text + "',sysdate,'" + DB.vUserId + "')  ";

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

        private void btnRecDel_Click(object sender, EventArgs e)
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

                sql_delete = " delete from KLINIK.cs_recommendation where recom_id = '" + lRecID.Text + "'  ";

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataPasien();
        }

        private void loadObservasi_Click(object sender, EventArgs e)
        {
            string sql_load = "";
            string s_rm = "", s_que = "", s_date = "", p_rm = "", p_que = "", p_date = "", p_name = "", p_anamnesa = "", p_diagnosa = "", p_nik = "";

            s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();

            sql_load = " select a.patient_no, a.name, c.rm_no, to_char(b.visit_date,'yyyy-mm-dd') visit_date, que01, " +
                       " (select  'Tensi : ' || blood_press || ', Nadi : ' || pulse ||  " +
                       //" ', Suhu : ' || temperature || ', Alergi : ' || allergy || ', Keluhan : ' || anamnesa as anamnesa   " +
                       " ', Suhu : ' || temperature || ', BB : ' || bb || ', TB : ' || tb || ', Alergi : ' || allergy || ', Keluhan : ' || anamnesa as anamnesa   " +
                       " from KLINIK.cs_anamnesa " +
                       " where rm_no=c.rm_no " +
                       " and insp_date=trunc(b.visit_date)  " +
                       " and visit_no=b.que01) anamnesa,  " +
                       " (select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa  " +
                       " from KLINIK.cs_diagnosa a  " +
                       " join KLINIK.cs_diagnosa_item b on (a.item_cd=b.item_cd)  " +
                       " where b.status='A'  " +
                       " and rm_no=c.rm_no  " +
                       " and insp_date=trunc(b.visit_date) " +
                       " and visit_no=b.que01) as diagnosa " +
                       " from KLINIK.cs_patient_info a  " +
                       " join KLINIK.cs_visit b on (a.patient_no = b.patient_no)  " +
                       " join KLINIK.cs_patient c on(b.patient_no = c.patient_no)  " +
                       " where  to_char(b.visit_date, 'yyyy-mm-dd') = '" + s_date + "'  " +
                       " and c.status = 'A'  " +
                       " and b.que01 = '" + s_que + "'  " +
                       " and c.group_patient <> 'COMM'  " +
                       " and c.rm_no = '" + s_rm + "' ";

            if (luObsRoom.Text == "Select Room")
            {
                MessageBox.Show("Silahkan pilih ruangan");
            }
            else
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_load, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);
                p_rm = dt.Rows[0]["rm_no"].ToString();
                p_que = dt.Rows[0]["que01"].ToString();
                p_date = dt.Rows[0]["visit_date"].ToString();

                p_name = dt.Rows[0]["name"].ToString();
                p_nik = dt.Rows[0]["patient_no"].ToString();
                p_anamnesa = dt.Rows[0]["anamnesa"].ToString();
                p_diagnosa = dt.Rows[0]["diagnosa"].ToString();

                lObsRm.Text = p_rm;
                lObsQue.Text = p_que;
                lObsDate.Text = p_date;

                lObsName.Text = p_name;
                lObsNik.Text = p_nik;
                lObsAnam.Text = p_anamnesa;
                lObsDiag.Text = p_diagnosa;


                ObsList();
                int cap = 0, free = 0, cnt = 0;
                cap = Convert.ToInt32(luObsRoom.GetColumnValue("roomQty").ToString());
                cnt = gridView5.RowCount;
                free = cap - cnt;
                lObsCap.Text = luObsRoom.GetColumnValue("roomQty").ToString();
                lObsFre.Text = free.ToString();
                if (free == 0)
                {
                    btnObsAdd.Enabled = false;
                }
                else
                {
                    btnObsAdd.Enabled = true;
                }
                btnObsSave.Enabled = false;
                btnObsCls.Enabled = true;
                btnObsDel.Enabled = true;
            }


        }

        private void luObsRoom_EditValueChanged(object sender, EventArgs e)
        {
            //int cap = 0, free = 0, cnt=0;

            //ObsList();

            //cap = Convert.ToInt32(luObsRoom.GetColumnValue("roomQty").ToString());
            //cnt = gridView5.RowCount;
            //free = cap - cnt;
            //lObsCap.Text = luObsRoom.GetColumnValue("roomQty").ToString();
            //lObsFre.Text = free.ToString();
            //if (free == 0)
            //{
            //    btnObsAdd.Enabled = false;
            //}
            //else
            //{
            //    btnObsAdd.Enabled = true;
            //}
            //btnObsSave.Enabled = false;
            //btnObsCls.Enabled = true;
        }

        private void ObsList()
        {
            string sql_select_room = "";

            sql_select_room = " select a.rm_no, c.que01, to_char(c.visit_date,'yyyy-mm-dd') visit_date, b.obs_id,   " +
                              " b.room_cd, a.patient_no, (select name from KLINIK.cs_patient_info where patient_no = a.patient_no ) nama, " +
                              " to_char(b.obs_start, 'yyyy-mm-dd') obs_date,   " +
                              " to_char(b.obs_start, 'hh24:mi:ss') obs_start, hrs_cnt,  " +
                              " round(24 * (nvl(b.obs_end, sysdate) - b.obs_start), 2) durasi,  " +
                              " case when round(24 * (nvl(b.obs_end, sysdate) - b.obs_start), 2) > hrs_cnt then 'Over' else null end stat,  " +
                              " to_char(b.obs_end, 'hh24:mi:ss') obs_end, b.obs_remark, 'S' action  " +
                              " from KLINIK.cs_patient a  " +
                              " join KLINIK.cs_observation b on (a.rm_no = b.rm_no)  " +
                              " join KLINIK.cs_visit c on(a.patient_no = c.patient_no)  " +
                              " where b.visit_no = c.que01  " +
                              " and a.status = 'A'  " +
                              " and trunc(c.visit_date) = trunc(b.obs_start) " +
                              " and b.obs_end is null  " +
                              " and to_char(b.insp_date, 'yyyy-mm-dd') = '" + lObsDate.Text + "'  " +
                              " and b.room_cd = '" + luObsRoom.GetColumnValue("roomCode").ToString() + "' ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_select_room, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl5.DataSource = null;
            gridView5.Columns.Clear();
            gridControl5.DataSource = dt;

            gridView5.OptionsView.ColumnAutoWidth = true;
            gridView5.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView5.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView5.IndicatorWidth = 30;
            //gridView5.OptionsBehavior.Editable = false;
            gridView5.BestFitColumns();

            gridView5.Columns[0].Caption = "RM No";
            gridView5.Columns[1].Caption = "Que";
            gridView5.Columns[2].Caption = "Date";
            gridView5.Columns[3].Caption = "ID";
            gridView5.Columns[4].Caption = "Room ID";
            gridView5.Columns[5].Caption = "Pasien No";
            gridView5.Columns[6].Caption = "Nama";
            gridView5.Columns[7].Caption = "Tanggal";
            gridView5.Columns[8].Caption = "Jam Mulai";
            gridView5.Columns[9].Caption = "Lama";
            gridView5.Columns[10].Caption = "Durasi";
            gridView5.Columns[11].Caption = "Status";
            gridView5.Columns[12].Caption = "Jam Selesai";
            gridView5.Columns[13].Caption = "Remark";
            gridView5.Columns[14].Caption = "Action";

            gridView5.Columns[0].Visible = false;
            gridView5.Columns[1].Visible = false;
            gridView5.Columns[2].Visible = false;
            gridView5.Columns[3].Visible = false;
            gridView5.Columns[4].Visible = false;


            gridView5.Columns[14].Visible = false;

            RepositoryItemLookUpEdit hrs = new RepositoryItemLookUpEdit();
            hrs.DataSource = listHours;
            hrs.ValueMember = "statCode";
            hrs.DisplayMember = "statName";

            hrs.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            hrs.DropDownRows = listHours.Count;
            hrs.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            hrs.AutoSearchColumnIndex = 1;
            hrs.NullText = "";
            gridView5.Columns[9].ColumnEdit = hrs;

            gridView5.Columns[5].OptionsColumn.ReadOnly = true;
            gridView5.Columns[6].OptionsColumn.ReadOnly = true;
            gridView5.Columns[7].OptionsColumn.ReadOnly = true;
            gridView5.Columns[8].OptionsColumn.ReadOnly = true;
            gridView5.Columns[10].OptionsColumn.ReadOnly = true;
            gridView5.Columns[11].OptionsColumn.ReadOnly = true;
            gridView5.Columns[12].OptionsColumn.ReadOnly = true;
            gridView5.Columns[14].OptionsColumn.ReadOnly = true;
        }

        private void gridView5_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string stat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);

                if (stat == "Over")
                {
                    e.Appearance.BackColor = Color.IndianRed;
                    e.Appearance.BackColor2 = Color.Firebrick;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                    e.HighPriority = true;
                }
            }
        }

        private void gridView5_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView5_RowClick(object sender, RowClickEventArgs e)
        {
            //GridView View = sender as GridView;
            //string s_stat = "";

            //s_stat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[12]);

            //if (s_stat == "")
            //{
            //    btnObsCls.Enabled = true;
            //}
            //else
            //{
            //    btnObsCls.Enabled = false;
            //}

            //btnObsSave.Enabled = true;
        }

        private void btnObsAdd_Click(object sender, EventArgs e)
        {
            gridView5.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView5.AddNewRow();
        }

        private void gridView5_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[14], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[5], lObsNik.Text);
            view.SetRowCellValue(e.RowHandle, view.Columns[6], lObsName.Text);

        }

        private void gridView5_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnObsSave.Enabled = true;
            btnObsCls.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Lama" || e.Column.Caption == "Remark")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[14]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[14], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[14], "U");
                }
            }
        }

        private void btnObsSave_Click(object sender, EventArgs e)
        {
            string lama = "", remark = "", action = "", id = "";
            string sql_cnt = "", obs_cnt = "", sql_update = "";

            for (int i = 0; i < gridView5.DataRowCount; i++)
            {
                id = gridView5.GetRowCellValue(i, gridView5.Columns[3]).ToString();
                lama = gridView5.GetRowCellValue(i, gridView5.Columns[9]).ToString();
                remark = gridView5.GetRowCellValue(i, gridView5.Columns[13]).ToString();
                action = gridView5.GetRowCellValue(i, gridView5.Columns[14]).ToString();

                if (lama == "")
                {
                    MessageBox.Show("Lama Observasi harus diisi");
                }
                else
                {
                    if (action == "I")
                    {
                        sql_cnt = " select count(0) cnt from KLINIK.cs_observation where to_char(insp_date,'yyyy-mm-dd') = '" + lObsDate.Text + "' and visit_no = '" + lObsQue.Text + "' and rm_no = '" + lObsRm.Text + "' " + " and obs_end is null ";
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                        DataTable dt = new DataTable();
                        adOra.Fill(dt);
                        obs_cnt = dt.Rows[0]["cnt"].ToString();
                        if (Convert.ToInt32(obs_cnt) > 0)
                        {
                            MessageBox.Show("Gagal Disimpan, pasien tersebut dalam proses Observasi");
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

                                command.CommandText = " insert into cs_observation (obs_id, rm_no, insp_date, room_cd, bed_no, obs_start, hrs_cnt, obs_remark, visit_no, ins_date, ins_emp) " +
                                                      " values(cs_obs_seq.nextval, '" + lObsRm.Text + "', to_date('" + lObsDate.Text + "', 'yyyy-mm-dd'), '" + luObsRoom.GetColumnValue("roomCode").ToString() + "', 1, sysdate, '" + lama + "', '" + remark + "', '" + lObsQue.Text + "', sysdate, '" + DB.vUserId + "') ";
                                command.ExecuteNonQuery();

                                command.CommandText = " update cs_visit set status = 'OBS', observation='Y', upd_emp = '" + DB.vUserId + "', upd_date = sysdate where empid = '" + lObsNik.Text + "' and to_char(visit_date,'yyyy-mm-dd') = '" + lObsDate.Text + "' and que01 = '" + lObsQue.Text + "' ";
                                command.ExecuteNonQuery();

                                trans.Commit();
                                //MessageBox.Show(sql_insert);
                                //MessageBox.Show("Query Exec : " + sql_insert);
                                //ObsList();
                                ObsList();
                                int cap = 0, free = 0, cnt = 0;
                                cap = Convert.ToInt32(luObsRoom.GetColumnValue("roomQty").ToString());
                                cnt = gridView5.RowCount;
                                free = cap - cnt;
                                lObsCap.Text = luObsRoom.GetColumnValue("roomQty").ToString();
                                lObsFre.Text = free.ToString();
                                if (free == 0)
                                {
                                    btnObsAdd.Enabled = false;
                                }
                                else
                                {
                                    btnObsAdd.Enabled = true;
                                }
                                btnObsSave.Enabled = false;
                                btnObsCls.Enabled = true;
                                btnObsDel.Enabled = true;
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

                        sql_update = sql_update + " update cs_observation" +
                                                  " set hrs_cnt = '" + lama + "', obs_remark = '" + remark + "', ";
                        sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                        sql_update = sql_update + " where obs_id = '" + id + "' ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);
                            ObsList();
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

        private void btnObsCls_Click(object sender, EventArgs e)
        {
            string rm_no = "", que = "", date = "", id = "", nik = "", end_time = "", stat = "", sql_status = "";

            rm_no = gridView5.GetRowCellValue(gridView5.FocusedRowHandle, gridView5.Columns[0]).ToString();
            que = gridView5.GetRowCellValue(gridView5.FocusedRowHandle, gridView5.Columns[1]).ToString();
            date = gridView5.GetRowCellValue(gridView5.FocusedRowHandle, gridView5.Columns[2]).ToString();
            id = gridView5.GetRowCellValue(gridView5.FocusedRowHandle, gridView5.Columns[3]).ToString();
            nik = gridView5.GetRowCellValue(gridView5.FocusedRowHandle, gridView5.Columns[5]).ToString();
            end_time = gridView5.GetRowCellValue(gridView5.FocusedRowHandle, gridView5.Columns[12]).ToString();

            if (end_time != "")
            {
                MessageBox.Show("Data Observasi sudah diclose");
            }
            else
            {
                sql_status = " select decode(time_receipt,null,'OBS','CLS') stat from KLINIK.cs_visit where to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' and patient_no = '" + nik + "' ";
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_status, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);
                stat = dt.Rows[0]["stat"].ToString();

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

                    command.CommandText = " update cs_observation set obs_end = sysdate,  " +
                                         " upd_date = sysdate, upd_emp = '" + DB.vUserId + "'  " +
                                         " where obs_id = '" + id + "' ";
                    command.ExecuteNonQuery();

                    if (stat == "CLS")
                    {
                        command.CommandText = " update cs_visit set status = '" + stat + "', time_observation = sysdate, time_end = sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                    }
                    else
                    {
                        command.CommandText = " update cs_visit set status = '" + stat + "', time_observation = sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                    }

                    command.ExecuteNonQuery();

                    trans.Commit();
                    //MessageBox.Show(sql_insert);
                    //MessageBox.Show("Query Exec : " + sql_insert);
                    ObsList();
                    MessageBox.Show("Data Berhasil diclose.");
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("ERROR: " + ex.Message);
                }

                oraConnectTrans.Close();
            }


        }

        private void LoadDataResep()
        {
            string sql_med_load = "", s_rm = "", s_date = "", s_que = "", sstatus ="", spoli="";

            s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            //if (chOUmum.Checked)
            //    sstatus = lstsobat.Text;
            //else
            sstatus = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[15]).ToString();
            spoli = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();

            sql_med_load = " select a.receipt_id, a.med_cd, b.med_group, a.med_cd, a.formula, type_drink,  " +
                           " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +  " +
                           " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -  " +
                           " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) - " +
                           " klinik.FN_CS_REQ_STOCK(a.insp_date,a.med_cd) stock, " +
                           " a.med_qty, initcap(uom) uom, 'S' action, a.confirm, a.days, a.price, a.qty_day, a.dosis,a.MED_REMARK REMARK " +
                           " from KLINIK.cs_receipt a  " +
                           " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)  " +
                           " JOIN KLINIK.cs_formula D ON (B.med_cd = D.med_cd AND D.FORMULA_ID = A.formula) " +
                           " where b.status = 'A' AND D.MINUS_STOK = 'Y'  AND RACIKAN ='N' " +
                           " and rm_no = '" + s_rm + "'  and ID_VISIT = '" + idvisit + "' " +
                           " and to_char(insp_date, 'yyyy-mm-dd') = '" + s_date + "' AND ATT1 ='" + sstatus + "'  " +
                           " and visit_no = '" + s_que + "' ";

            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_med_load, oraConnect2);
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
            gridView6.Columns[1].Caption = "Kode";
            gridView6.Columns[2].Caption = "Group";
            gridView6.Columns[3].Caption = "Nama Obat";
            gridView6.Columns[4].Caption = "Kode Dosis";
            gridView6.Columns[5].Caption = "Info";
            gridView6.Columns[6].Caption = "Stok";
            gridView6.Columns[7].Caption = "Jumlah";
            gridView6.Columns[8].Caption = "Satuan";
            gridView6.Columns[9].Caption = "Action";
            gridView6.Columns[10].Caption = "Confirm";
            gridView6.Columns[11].Caption = "Jml";
            gridView6.Columns[12].Caption = "Harga";
            gridView6.Columns[13].Caption = "Jumlah per Hari";
            gridView6.Columns[14].Caption = "Dosis";
            gridView6.Columns[15].Caption = "Remark";

            gridView6.Columns[14].VisibleIndex = 5;
            gridView6.Columns[11].VisibleIndex = 6;

            gridView6.Columns[4].MinWidth = 80;
            gridView6.Columns[4].MaxWidth = 80;
            gridView6.Columns[5].MinWidth = 120;
            gridView6.Columns[5].MaxWidth = 120;
            gridView6.Columns[6].MinWidth = 60;
            gridView6.Columns[6].MaxWidth = 60;
            gridView6.Columns[7].MinWidth = 60;
            gridView6.Columns[7].MaxWidth = 60;
            gridView6.Columns[8].MinWidth = 60;
            gridView6.Columns[8].MaxWidth = 60;
            gridView6.Columns[10].MinWidth = 60;
            gridView6.Columns[10].MaxWidth = 60;
            gridView6.Columns[11].MinWidth = 60;
            gridView6.Columns[11].MaxWidth = 60;
            gridView6.Columns[14].MinWidth = 60;
            gridView6.Columns[14].MaxWidth = 60;
            gridView6.Columns[15].MinWidth = 180;
            gridView6.Columns[15].MaxWidth = 180;

            gridView6.Columns[0].Visible = false;
            gridView6.Columns[1].Visible = false;
            gridView6.Columns[2].Visible = false;
            gridView6.Columns[7].Visible = false;
            gridView6.Columns[8].Visible = false;
            gridView6.Columns[9].Visible = false;
            gridView6.Columns[12].Visible = false;
            gridView6.Columns[13].Visible = false;
            //gridView6.Columns[10].Visible = false;

            //gridView6.Columns[3].OptionsColumn.ReadOnly = true;
            gridView6.Columns[2].OptionsColumn.ReadOnly = true;
            gridView6.Columns[6].OptionsColumn.ReadOnly = true;

            gridView6.Columns[8].OptionsColumn.ReadOnly = true;
            gridView6.Columns[9].OptionsColumn.ReadOnly = true;
            gridView6.Columns[10].OptionsColumn.ReadOnly = true;

            //RepositoryItemLookUpEdit medicineLookup = new RepositoryItemLookUpEdit();
            //medicineLookup.DataSource = listMedicine;
            //medicineLookup.ValueMember = "medicineCode";
            //medicineLookup.DisplayMember = "medicineName";

            //medicineLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //medicineLookup.DropDownRows = listMedicine.Count;
            //medicineLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            //medicineLookup.AutoSearchColumnIndex = 1;
            //medicineLookup.NullText = "";
            //gridView6.Columns[3].ColumnEdit = medicineLookup;

            glmed.DataSource = listMedicine;
            glmed.ValueMember = "medicineCode";
            glmed.DisplayMember = "medicineName";
            glmed.PopulateViewColumns();
            glmed.View.Columns["medicineCode"].Width = 35;
            glmed.View.Columns["medicineName"].Width = 200;
            glmed.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glmed.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            //glmed.Appearance.Font.Size = 11;
            glmed.ImmediatePopup = true;
            glmed.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glmed.NullText = "";
            gridView6.Columns[3].ColumnEdit = glmed;

            string sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join cs_medicine b on(a.med_cd=b.med_cd) where 1=1 AND a.POLI_CD  in ('POL0002','POL0003','ALL')  AND RACIKAN ='N' ";
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

            DataListObat(sstatus, spoli);

            glfor.DataSource = listFormula2;
            glfor.ValueMember = "formulaCode";
            glfor.DisplayMember = "formulaName";

            glfor.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glfor.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glfor.ImmediatePopup = true;
            glfor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glfor.NullText = "";
            gridView6.Columns[4].ColumnEdit = glfor;
            //gvRacik.Columns[4].ColumnEdit = glfor;


            medicineInfoLookup.DataSource = listMedicineInfo;
            medicineInfoLookup.ValueMember = "medicineInfoCode";
            medicineInfoLookup.DisplayMember = "medicineInfoName";

            medicineInfoLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            medicineInfoLookup.DropDownRows = listMedicineInfo.Count;
            medicineInfoLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            medicineInfoLookup.AutoSearchColumnIndex = 1;
            medicineInfoLookup.NullText = "";
            gridView6.Columns[5].ColumnEdit = medicineInfoLookup; 

            dosisLookup.DataSource = listDosis;
            dosisLookup.ValueMember = "DosisCode";
            dosisLookup.DisplayMember = "DosisName";
            dosisLookup.NullText = "";
            gridView6.Columns[14].ColumnEdit = dosisLookup; 
             
            btnMedAdd.Enabled = true;
            btnNoReceipt.Enabled = true;

            if (gridView6.RowCount > 0)
            {
                btnMedDel.Enabled = true;
                btnMedCan.Enabled = true;
            }
            else
            {
                btnMedDel.Enabled = false;
                btnMedCan.Enabled = true;
            }
            if (sstatus.ToString().Equals("BPJS"))
                LoadDataResep2();
             
        }

        private void LoadDataResep2()
        {
            string sql_med_load = "", s_rm = "", s_date = "", s_que = "", sstatus = "", spoli = "";

            s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            sstatus = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[15]).ToString();
            spoli = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();

            sql_med_load = " select a.receipt_id, a.med_cd, b.med_group, a.med_cd, A.formula, type_drink,  " +
                           " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +  " +
                           " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -  " +
                           " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) - " +
                           " klinik.FN_CS_REQ_STOCK(a.insp_date,a.med_cd) stock, " +
                           " A.med_qty, initcap(uom) uom, 'S' action, a.confirm, a.days, a.price, a.qty_day, a.dosis " +
                           " from KLINIK.cs_receipt a  " +
                           " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)  JOIN KLINIK.cs_formula D ON (B.med_cd = D.med_cd AND D.FORMULA_ID = A.formula) " +
                           " where b.status = 'A'   and D.MINUS_STOK ='Y'  AND RACIKAN ='N' " +
                           " and rm_no = '" + s_rm + "' and att1 ='UMUM' " +
                           " and to_char(insp_date, 'yyyy-mm-dd') = '" + s_date + "' and GRID_NAME = 'gridView16' " +
                           " and visit_no = '" + s_que + "' and id_visit = " + idvisit + " ";

            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_med_load, oraConnect2);
            DataTable dt2 = new DataTable();
            adOra2.Fill(dt2);

            gridControl16.DataSource = null;
            gridView16.Columns.Clear();
            gridControl16.DataSource = dt2;

            gridView16.OptionsView.ColumnAutoWidth = true;
            gridView16.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView16.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView16.IndicatorWidth = 30;
            gridView16.BestFitColumns();

            gridView16.Columns[0].Caption = "ID";
            gridView16.Columns[1].Caption = "Kode";
            gridView16.Columns[2].Caption = "Group";
            gridView16.Columns[3].Caption = "Nama Obat";
            gridView16.Columns[4].Caption = "Kode Dosis";
            gridView16.Columns[5].Caption = "Info";
            gridView16.Columns[6].Caption = "Stok";
            gridView16.Columns[7].Caption = "Jumlah";
            gridView16.Columns[8].Caption = "Satuan";
            gridView16.Columns[9].Caption = "Action";
            gridView16.Columns[10].Caption = "Confirm";
            gridView16.Columns[11].Caption = "Jml";
            gridView16.Columns[12].Caption = "Harga";
            gridView16.Columns[13].Caption = "Jumlah per Hari";
            gridView16.Columns[14].Caption = "Dosis";

            gridView16.Columns[14].VisibleIndex = 5;
            gridView16.Columns[11].VisibleIndex = 6;

            gridView16.Columns[4].MinWidth = 80;
            gridView16.Columns[4].MaxWidth = 80;
            gridView16.Columns[5].MinWidth = 120;
            gridView16.Columns[5].MaxWidth = 120;
            gridView16.Columns[6].MinWidth = 60;
            gridView16.Columns[6].MaxWidth = 60;
            gridView16.Columns[7].MinWidth = 60;
            gridView16.Columns[7].MaxWidth = 60;
            gridView16.Columns[8].MinWidth = 60;
            gridView16.Columns[8].MaxWidth = 60;
            gridView16.Columns[10].MinWidth = 60;
            gridView16.Columns[10].MaxWidth = 60;
            gridView16.Columns[11].MinWidth = 60;
            gridView16.Columns[11].MaxWidth = 60;
            gridView16.Columns[14].MinWidth = 60;
            gridView16.Columns[14].MaxWidth = 60;

            gridView16.Columns[0].Visible = false;
            gridView16.Columns[1].Visible = false;
            gridView16.Columns[2].Visible = false;
            gridView16.Columns[7].Visible = false;
            gridView16.Columns[8].Visible = false;
            gridView16.Columns[9].Visible = false;
            gridView16.Columns[12].Visible = false;
            gridView16.Columns[13].Visible = false;

            gridView16.Columns[2].OptionsColumn.ReadOnly = true;
            gridView16.Columns[6].OptionsColumn.ReadOnly = true;
            gridView16.Columns[7].OptionsColumn.ReadOnly = true;
            gridView16.Columns[8].OptionsColumn.ReadOnly = true;
            gridView16.Columns[9].OptionsColumn.ReadOnly = true;
            gridView16.Columns[10].OptionsColumn.ReadOnly = true;

            DataListObat("UMUM", spoli);
            RepositoryItemGridLookUpEdit glmedU = new RepositoryItemGridLookUpEdit();
            glmedU.DataSource = listMedicineU;
            glmedU.ValueMember = "medicineCode";
            glmedU.DisplayMember = "medicineName";

            glmedU.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glmedU.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glmedU.ImmediatePopup = true;
            glmedU.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glmedU.NullText = "";
            gridView16.Columns[3].ColumnEdit = glmedU;

            string sql_for = "";
            sql_for = sql_for + Environment.NewLine + "  select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1 and POLI_CD in( 'POL0002', 'POL0003') and att1 = 'UMUM'  AND RACIKAN ='N' ";

            OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
            DataTable dtf = new DataTable();
            adOraf.Fill(dtf);
            listFormulaU.Clear();
            for (int i = 0; i < dtf.Rows.Count; i++)
            {
                listFormulaU.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
            }

            RepositoryItemGridLookUpEdit glfor = new RepositoryItemGridLookUpEdit();
            glfor.DataSource = listFormula2;
            glfor.ValueMember = "formulaCode";
            glfor.DisplayMember = "formulaName";

            glfor.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glfor.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glfor.ImmediatePopup = true;
            glfor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glfor.NullText = "";
            gridView16.Columns[4].ColumnEdit = glfor;

            RepositoryItemLookUpEdit medicineInfoLookup = new RepositoryItemLookUpEdit();
            medicineInfoLookup.DataSource = listMedicineInfo;
            medicineInfoLookup.ValueMember = "medicineInfoCode";
            medicineInfoLookup.DisplayMember = "medicineInfoName";

            medicineInfoLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            medicineInfoLookup.DropDownRows = listMedicineInfo.Count;
            medicineInfoLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            medicineInfoLookup.AutoSearchColumnIndex = 1;
            medicineInfoLookup.NullText = "";
            gridView16.Columns[5].ColumnEdit = medicineInfoLookup;

            RepositoryItemLookUpEdit dosisLookup = new RepositoryItemLookUpEdit();
            dosisLookup.DataSource = listDosis;
            dosisLookup.ValueMember = "DosisCode";
            dosisLookup.DisplayMember = "DosisName";
            dosisLookup.NullText = "";
            gridView16.Columns[14].ColumnEdit = dosisLookup;

        }

        private void DataListObat(string sstatus, string spoli)
        {
            dtGlMed.Clear();
            string sql_med = " "; 
            sql_med = sql_med + Environment.NewLine + " select b.med_cd, initcap(med_name) || decode(att1,'BPJS','',' [None BPJS]')  med_name  ";
            sql_med = sql_med + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1    ";
            sql_med = sql_med + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  AND RACIKAN ='N' ";
            if(sstatus.ToString().Equals("UMUM"))
                sql_med = sql_med + Environment.NewLine + "    and att1 = '" + sstatus + "'  ";
            else
                sql_med = sql_med + Environment.NewLine + "    and upper(att1) in (decode(upper('" + sstatus + "'), 'BPJS', 'BPJS','UMUM') ,'ALL')  ";
            sql_med = sql_med + Environment.NewLine + "    and POLI_CD  in ('POL0002','POL0003')  "; 
            sql_med = sql_med + Environment.NewLine + "  order by med_name  "; 

            OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_med, sqlConnect3);
            DataTable dt3 = new DataTable();
            dtGlMed = dt3;
            adSql3.Fill(dt3);
            if (sstatus.ToString().Equals("UMUM"))
            {
                listMedicineU.Clear();
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    listMedicineU.Add(new MedicineUmum() { medicineCode = dt3.Rows[i]["med_cd"].ToString(), medicineName = dt3.Rows[i]["med_name"].ToString() });
                }
            }
            else
            {
                listMedicine.Clear();
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    listMedicine.Add(new Medicine() { medicineCode = dt3.Rows[i]["med_cd"].ToString(), medicineName = dt3.Rows[i]["med_name"].ToString() });
                }
            }
               
        }
        private void loadResep_Click(object sender, EventArgs e)
        {
            string sql_load = "", sql_resep_luar = "";
            string s_rm = "", s_que = "", s_date = "", p_rm = "", p_que = "", p_date = "", p_name = "", p_anamnesa = "", p_diagnosa = "", p_nik = "", p_que2 = "";
            string p_rp = "", p_pf = "", p_pt = "", p_resep = "";

            if (gridView1.RowCount < 1) return;

            if (idvisit.ToString().Equals(""))
            {
                MessageBox.Show("Silahkan Tentukan Pasien Terlebh Dahulu...!!!");
                return;
            }
            if (gridView1.FocusedRowHandle < 0)
                return;

            s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();

            string s_tatus = gridView1.GetRowCellDisplayText(gridView1.FocusedRowHandle, gridView1.Columns[7]);
            s_stat = gridView1.GetRowCellDisplayText(gridView1.FocusedRowHandle, gridView1.Columns[15]);


            sql_load = sql_load + Environment.NewLine + "select a.patient_no, initcap(a.name) name, c.rm_no, to_char(b.visit_date,'yyyy-mm-dd') visit_date, que01, que02,   ";
            sql_load = sql_load + Environment.NewLine + "(select  'Tensi : ' || blood_press || ', Nadi : ' || pulse ||   ";
            //sql_load = sql_load + Environment.NewLine + "', Suhu : ' || temperature || ', Alergi : ' || allergy || ', Keluhan : ' || anamnesa as anamnesa   ";
            sql_load = sql_load + Environment.NewLine + "', Suhu : ' || temperature || ', BB : ' || bb || ', TB : ' || tb || ', Alergi : ' || allergy || ', Keluhan : ' || anamnesa as anamnesa    ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_anamnesa  ";
            sql_load = sql_load + Environment.NewLine + "where rm_no=c.rm_no  ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)   ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) anamnesa,   ";
            sql_load = sql_load + Environment.NewLine + "(select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa   ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_diagnosa a   ";
            sql_load = sql_load + Environment.NewLine + "join KLINIK.cs_diagnosa_item b on (a.item_cd=b.item_cd)   ";
            sql_load = sql_load + Environment.NewLine + "where b.status='A'   ";
            sql_load = sql_load + Environment.NewLine + "and rm_no=c.rm_no   ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)  ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) as diagnosa,  ";
            sql_load = sql_load + Environment.NewLine + "(select  'Sekarang : ' || disease_now || ', Dahulu : ' || disease_then ||   ";
            sql_load = sql_load + Environment.NewLine + "', Keluarga : ' || disease_family as rp ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_anamnesa  ";
            sql_load = sql_load + Environment.NewLine + "where rm_no=c.rm_no  ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)   ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) rp, ";
            sql_load = sql_load + Environment.NewLine + "(select anamnesa_physical   ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_anamnesa  ";
            sql_load = sql_load + Environment.NewLine + "where rm_no=c.rm_no  ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)   ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) fisik,  ";
            sql_load = sql_load + Environment.NewLine + "(select anamnesa_other  ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_anamnesa  ";
            sql_load = sql_load + Environment.NewLine + "where rm_no=c.rm_no  ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)   ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) lain  ";
            sql_load = sql_load + Environment.NewLine + ", case when b.STATUS = ( select d.TYPE_INS  ";
            sql_load = sql_load + Environment.NewLine + "                           from KLINIK.CS_CALL_LOG d  ";
            sql_load = sql_load + Environment.NewLine + "                          where d.que = b.que01  ";
            sql_load = sql_load + Environment.NewLine + "                            AND TRUNC(d.INS_DATE) = TRUNC(SYSDATE) ";
            sql_load = sql_load + Environment.NewLine + "                            AND TRUNC(d.INS_DATE) = TRUNC(b.VISIT_DATE) ";
            sql_load = sql_load + Environment.NewLine + "                       ) then 'Y'  ";
            sql_load = sql_load + Environment.NewLine + "  when b.STATUS = 'PAY' then 'Y' when b.STATUS = 'CLS' then 'Y' else 'N' end st_close, b.POLI_CD ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_patient_info a   ";
            sql_load = sql_load + Environment.NewLine + "join KLINIK.cs_visit b on (a.patient_no = b.patient_no)   ";
            sql_load = sql_load + Environment.NewLine + "join KLINIK.cs_patient c on(b.patient_no = c.patient_no)   ";
            sql_load = sql_load + Environment.NewLine + "where  to_char(b.visit_date, 'yyyy-mm-dd') = '" + s_date + "'   ";
            sql_load = sql_load + Environment.NewLine + "and c.status = 'A'   ";
            sql_load = sql_load + Environment.NewLine + "and b.que01 = '" + s_que + "'   ";
            sql_load = sql_load + Environment.NewLine + "and c.group_patient = 'COMM'   ";
            sql_load = sql_load + Environment.NewLine + "and c.rm_no = '" + s_rm + "' and id_visit = " + idvisit + "  ";


            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_load, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            if (dt.Rows.Count < 1)
                return;

            p_rm = dt.Rows[0]["rm_no"].ToString();
            p_que = dt.Rows[0]["que01"].ToString();
            p_que2 = dt.Rows[0]["que02"].ToString();
            p_date = dt.Rows[0]["visit_date"].ToString();

            p_name = dt.Rows[0]["name"].ToString();
            p_nik = dt.Rows[0]["patient_no"].ToString();
            p_anamnesa = dt.Rows[0]["anamnesa"].ToString();
            p_rp = dt.Rows[0]["rp"].ToString();
            p_pf = dt.Rows[0]["fisik"].ToString();
            p_pt = dt.Rows[0]["lain"].ToString();
            p_diagnosa = dt.Rows[0]["diagnosa"].ToString();
            p_statuscls = dt.Rows[0]["st_close"].ToString();

            lMedRm.Text = p_rm;
            lMedQue.Text = p_que;
            lMedDate.Text = p_date;
            lMedQue2.Text = p_que2;

            lMedName.Text = p_name;
            lMedNik.Text = p_nik;
            lMedAnam.Text = p_anamnesa;
            lMedRp.Text = p_rp;
            lMedPf.Text = p_pf;
            lMedPt.Text = p_pt;
            lMedDiag.Text = p_diagnosa;

            //DataListObat(s_stat, dt.Rows[0]["POLI_CD"].ToString());
            LoadDataResep();

            if (gridView6.RowCount > 0)
            {
                sql_resep_luar = "";
                sql_resep_luar = sql_resep_luar + Environment.NewLine + "select distinct med_remark  ";
                sql_resep_luar = sql_resep_luar + Environment.NewLine + "from KLINIK.cs_receipt ";
                sql_resep_luar = sql_resep_luar + Environment.NewLine + "where rm_no='" + s_rm + "' ";
                sql_resep_luar = sql_resep_luar + Environment.NewLine + "and visit_no = '" + s_que + "' ";
                sql_resep_luar = sql_resep_luar + Environment.NewLine + "and to_char(insp_date,'yyyy-mm-dd') = '" + s_date + "' ";

                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_resep_luar, oraConnect2);
                DataTable dt2 = new DataTable();
                adOra2.Fill(dt2);

                p_resep = dt2.Rows[0]["med_remark"].ToString();
                mResepLuar.Text = p_resep;
            }

            if (p_statuscls == "Y")
            {
                btnMedDel.Enabled = false;
                btnMedAdd.Enabled = false;
                btnMedCan.Enabled = false;
                btnNoReceipt.Enabled = false;
                btnMedSave.Enabled = false;
                simpleButton3.Enabled = false;
                simpleButton4.Enabled = false;
                simpleButton12.Enabled = false;
                simpleButton13.Enabled = false;
            }
            else
            {
                btnMedDel.Enabled = true;
                btnMedAdd.Enabled = true;
                btnMedCan.Enabled = true;
                btnNoReceipt.Enabled = true;
                btnMedSave.Enabled = true;
                simpleButton3.Enabled = true;
                simpleButton4.Enabled = true;
                simpleButton12.Enabled = true ;
                simpleButton13.Enabled = true;
            }
        }

        private void gridView6_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void btnMedAdd_Click(object sender, EventArgs e)
        {
            gridView6.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView6.AddNewRow();
        }

        private void gridView6_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView; 
            view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
        }

        private void gridView6_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnMedSave.Enabled = true;
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();
            if (a.ToString().Equals(""))
                return;


            if (e.Column.Caption == "Nama Obat" && (a.Substring(0, 2) == "BP" || a.Substring(0, 2) == "UM"))
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();
                string policd = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();
                string sql_medcd = "", sql_med = "", med_cd = "", med_name = "", med_group = "", med_stok = "", med_uom = "", cek_stok = "", sql_for = "";
                s_stat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[15]).ToString();
                sql_medcd = " select " +
                            " klinik.FN_CS_INIT_STOCK(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + a + "') +  " +
                            " klinik.FN_CS_TRX_IN(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + a + "') -  " +
                            " klinik.FN_CS_TRX_OUT(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + a + "') - " +
                            " klinik.FN_CS_REQ_STOCK(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + a + "') stock from dual ";

                OleDbConnection oraConnect0 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_medcd, oraConnect0);
                DataTable dt0 = new DataTable();
                adOra0.Fill(dt0);
                cek_stok = dt0.Rows[0]["stock"].ToString();

                sql_med = " select med_cd, initcap(med_name) med_name, med_group, '" + cek_stok + "' stock, initcap(uom) uom " +
                          //" stock - (select nvl(SUM(med_qty),0) from cs_receipt  " +
                          //"           where TO_CHAR(insp_date, 'yyyy-mm-dd') = '" + lMedDate.Text + "'  " +
                          //"             and confirm = 'N'  " +
                          //"             and med_cd = a.med_cd) stock, uom  " +
                          //" klinik.FN_CS_INIT_STOCK(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'"+ medcd + "') +  " +
                          //" klinik.FN_CS_TRX_IN(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + medcd + "') -  " +
                          //" klinik.FN_CS_TRX_OUT(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + medcd + "') - " +
                          //" klinik.FN_CS_REQ_STOCK(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + medcd + "') stock, uom " +
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

                //if (chOUmum.Checked)
                //{
                //    s_stat = lstsobat.Text;
                //} 
                //else
                //{
                s_stat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[15]).ToString();
                  
                sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1  and  b.med_cd = '" + med_cd + "' and upper(att1) in (decode(upper('" + s_stat + "'), 'BPJS', 'BPJS', 'ASURANSI', 'ASURANSI', 'UMUM') ,'ALL')  and a.POLI_CD in('POL0002','POL0003' )  AND RACIKAN ='N'  ";
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
                if (dtf.Rows.Count == 1)
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], dtf.Rows[0]["formula_id"].ToString());
                else
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], "");

                view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);

                view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[3], med_name);
                    view.SetRowCellValue(e.RowHandle, view.Columns[2], med_group);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                    view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    view.SetRowCellValue(e.RowHandle, view.Columns[7], "0");
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }

                //dataFormula(policd);
            }

            if (e.Column.Caption == "Kode Dosis")
            {
                string medicine_cd = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string formula_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string reg_dt = lMedDate.Text;
                string rm = lMedRm.Text;
                string que = lMedQue.Text;
                string stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                string kode = "", sql_pilihan = "";

                if (stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
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
                        view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                        view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                    }
                    else
                    {
                        MessageBox.Show("Kode Formula tidak valid");
                        return;
                        //LoadDataResep();
                    }
                }


            }

            if (e.Column.Caption == "Jml")
            {
                string sql_for = "", med_price = "", qty = "", tmp_stat = "";
                string for_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string tmp_hari = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                int tot_hari = 0, tot_harga = 0;

                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

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

                tot_hari = Convert.ToInt32(tmp_hari) * Convert.ToInt32(qty);
                tot_harga = Convert.ToInt32(Convert.ToInt32(tmp_hari) * Convert.ToDouble(med_price));

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                    view.SetRowCellValue(e.RowHandle, view.Columns[7], tot_hari.ToString());
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                    view.SetRowCellValue(e.RowHandle, view.Columns[7], tot_hari.ToString());
                }
            }

            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Info" || e.Column.Caption == "Dosis" || e.Column.Caption == "Remark")
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

        private void gridView6_RowCellStyle(object sender, RowCellStyleEventArgs e)
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
                    if (Convert.ToInt32(stok) == 0)
                    {
                        e.Appearance.BackColor = Color.Crimson;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (Convert.ToInt32(stok) <= 20)
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

        private void btnMedSave_Click(object sender, EventArgs e)
        {
            string kode = "", dosis = "", info = "", jumlah = "", id = "", stok = "", con = "", action = "", remark = "";
            string sql_cnt = "", med_cnt = "", sql_update = "", sql_diag = "", diag_cnt = "", harga = "", hari = "", jph = "", info_dosis = "";
            int stsimpan = 0;

            for (int i = 0; i < gridView6.DataRowCount; i++)
            {
                id = gridView6.GetRowCellValue(i, gridView6.Columns[0]).ToString();
                kode = gridView6.GetRowCellValue(i, gridView6.Columns[1]).ToString();
                dosis = gridView6.GetRowCellValue(i, gridView6.Columns[4]).ToString();
                info = gridView6.GetRowCellValue(i, gridView6.Columns[5]).ToString();
                jumlah = gridView6.GetRowCellValue(i, gridView6.Columns[7]).ToString();
                stok = gridView6.GetRowCellValue(i, gridView6.Columns[6]).ToString();
                con = gridView6.GetRowCellValue(i, gridView6.Columns[10]).ToString();
                action = gridView6.GetRowCellValue(i, gridView6.Columns[9]).ToString();
                harga = gridView6.GetRowCellValue(i, gridView6.Columns[12]).ToString();
                hari = gridView6.GetRowCellValue(i, gridView6.Columns[11]).ToString();
                jph = gridView6.GetRowCellValue(i, gridView6.Columns[13]).ToString();
                info_dosis = gridView6.GetRowCellValue(i, gridView6.Columns[14]).ToString();
                remark = gridView6.GetRowCellValue(i, gridView6.Columns[15]).ToString();

                if (con == "Y")
                {
                    //MessageBox.Show("Data tidak bisa dirubah.");
                    labelControl192.Visible = true;
                    labelControl192.Text = "Gagal..Obat Sudah Confirm!!";
                    Blinking(labelControl192, 0);
                    return;
                }
                else if (stok == "0")
                {
                    //MessageBox.Show("Stok obat tidak tersedia.");
                    labelControl192.Visible = true;
                    labelControl192.Text = "Gagal..Obat Kosong!!";
                    Blinking(labelControl192, 0);
                    return;
                }
                else if (jumlah == "" || jumlah == "0")
                {
                    //MessageBox.Show("Jumlah obat harus diisi.");
                    labelControl192.Visible = true;
                    labelControl192.Text = "Gagal..Jumlah Kosong!!";
                    Blinking(labelControl192, 0);
                    return;
                }
                else if (Convert.ToInt32(jumlah) > Convert.ToInt32(stok))
                {
                    //MessageBox.Show("Jumlah melebihi stok");
                    labelControl192.Visible = true;
                    labelControl192.Text = "Gagal..Jumlah > Stok";
                    Blinking(labelControl192, 0);
                    return;
                }
                else if (kode == "")
                {
                    //MessageBox.Show("Kode obat harus diisi.");
                    labelControl192.Visible = true;
                    labelControl192.Text = "Gagal..Tentukan Obat";
                    Blinking(labelControl192, 0);
                    return;
                }
                else if (dosis == "")
                {
                    //MessageBox.Show("Kode Dosis harus diisi.");
                    labelControl192.Visible = true;
                    labelControl192.Text = "Gagal..Tentukan Dosis";
                    Blinking(labelControl192, 0);
                    return;
                }
                //else if (hari == "")
                //{
                //    MessageBox.Show("Jumlah harus diisi."); return;
                //}
                else if (info == "")
                {
                    //MessageBox.Show("Info harus diisi.");
                    labelControl192.Visible = true;
                    labelControl192.Text = "Gagal..Tentukan Info";
                    Blinking(labelControl192, 0);
                    return;
                }
                //else if (info_dosis == "")
                //{
                //    MessageBox.Show("Dosis harus diisi."); return;
                //}
                else
                {
                    int queue = 0;
                    string tmp_queue = "", que = "", cnt = "";
                    string sql_check = " select  nvl(max(to_number(substr(que02,2,3))),0) que from KLINIK.cs_visit where to_char(visit_date,'yyyy-mm-dd')= '" + today + "'  ";
                    string sql_check2 = " select  count(0) cnt from KLINIK.cs_receipt where rm_no = '" + lMedRm.Text + "' and to_char(insp_date,'yyyy-mm-dd')= '" + lMedDate.Text + "' and visit_no = '" + lMedQue.Text + "'  ";

                    try
                    {
                        OleDbConnection oraConnecta = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOraa = new OleDbDataAdapter(sql_check, oraConnecta);
                        DataTable dta = new DataTable();
                        adOraa.Fill(dta);

                        tmp_queue = dta.Rows[0]["que"].ToString();
                        queue = Convert.ToInt32(tmp_queue) + 1;
                        que = queue.ToString();
                        if (queue < 10)
                        {
                            que = que.PadLeft(que.Length + 2, '0');
                        }
                        else if (queue < 100)
                        {
                            que = que.PadLeft(que.Length + 1, '0');
                        }

                        OleDbConnection oraConnectb = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOrab = new OleDbDataAdapter(sql_check2, oraConnectb);
                        DataTable dtb = new DataTable();
                        adOrab.Fill(dtb);
                        cnt = dtb.Rows[0]["cnt"].ToString();

                        if (cnt == "0")
                        {
                            sql_update = "";

                            sql_update = sql_update + " update KLINIK.cs_visit" +
                                                      " set que02 = 'R" + que + "', ";
                            sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                            sql_update = sql_update + " where patient_no = '" + lMedNik.Text + "' and to_char(visit_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and que01 = '" + lMedQue.Text + "' and ID_VISIT =  " + idvisit + " ";

                            try
                            {
                                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                                OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                                oraConnect.Open();
                                cm.ExecuteNonQuery();
                                oraConnect.Close();
                                cm.Dispose();

                                //MessageBox.Show("Query Exec : " + sql_update);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ERROR: " + ex.Message);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }

                    if (action == "I")
                    {
                        sql_diag = " select count(0) cnt from KLINIK.cs_diagnosa where to_char(insp_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and visit_no = '" + lMedQue.Text + "' and rm_no = '" + lMedRm.Text + "' ";
                        OleDbConnection oraConnectd = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOrad = new OleDbDataAdapter(sql_diag, oraConnectd);
                        DataTable dtd = new DataTable();
                        adOrad.Fill(dtd);
                        diag_cnt = dtd.Rows[0]["cnt"].ToString();


                        sql_cnt = " select count(0) cnt from KLINIK.cs_receipt where to_char(insp_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and visit_no = '" + lMedQue.Text + "' and rm_no = '" + lMedRm.Text + "' " + " and med_cd = '" + kode + "' and ID_VISIT =  " + idvisit + " and GRID_NAME ='gridView6' ";
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                        DataTable dt = new DataTable();
                        adOra.Fill(dt);
                        med_cnt = dt.Rows[0]["cnt"].ToString();

                        if (Convert.ToInt32(med_cnt) > 0)
                        {
                            //MessageBox.Show("Gagal Disimpan.");
                        }
                        else if (diag_cnt == "0")
                        {
                            //MessageBox.Show("Gagal Disimpan. Diagnosa belum diinput.");
                            labelControl192.Visible = true;
                            labelControl192.Text = "Gagal..Diagnosa Kosong";
                            Blinking(labelControl192, 0);
                            return;
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

                                command.CommandText = " insert into KLINIK.cs_receipt (receipt_id, rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, visit_no, ins_date, ins_emp,ID_VISIT,GRID_NAME,JENIS_OBAT,MED_REMARK) " +
                                                      " values(cs_receipt_seq.nextval, '" + lMedRm.Text + "', to_date('" + lMedDate.Text + "', 'yyyy-mm-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "', '" + lMedQue.Text + "', sysdate, '" + DB.vUserId + "', " + idvisit + ",'gridView6','NONE', '" + remark + "') ";
                                command.ExecuteNonQuery();

                                //command.CommandText = " update cs_visit set status = 'MED', time_inspection=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + lMedNik.Text + "' and to_char(visit_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and que01 = '" + lMedQue.Text + "' ";
                                //command.ExecuteNonQuery();

                                trans.Commit();
                                //MessageBox.Show(sql_insert);
                                //MessageBox.Show("Query Exec : " + sql_insert);

                                //MessageBox.Show("Data Berhasil disimpan.");
                                stsimpan = 1;
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
                                                  "     price = '" + harga + "', days = '" + hari + "', qty_day = '" + jph + "', dosis = '" + info_dosis + "',";
                        sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate , MED_REMARK = '" + remark + "' ";
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
                            LoadDataResep();
                            //MessageBox.Show("Data Berhasil diupdate");
                            stsimpan = 2;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
            }
            if (stsimpan == 1)
            {
                labelControl192.Visible = true;
                labelControl192.Text = "Save Success";
                Blinking(labelControl192, 1);
            }
            else if (stsimpan == 2)
            {
                labelControl192.Visible = true;
                labelControl192.Text = "Updated Success";
                Blinking(labelControl192, 1);
            }

            chOUmum.Enabled = true;
            LoadDataResep();
        }

        private void btnMedDel_Click(object sender, EventArgs e)
        {
            string sql_delete = "", id = "", confirm = "";

            id = gridView6.GetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns[0]).ToString();
            confirm = gridView6.GetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns[10]).ToString();

            if (confirm == "Y")
            {
                //MessageBox.Show("Data tidak bisa dihapus.");
                labelControl192.Visible = true;
                labelControl192.Text = "Gagal..Obat Sudah Confirm!!";
                Blinking(labelControl192, 0);
                return;
            }
            else
            {
                sql_delete = "";
                sql_delete = sql_delete + " delete from KLINIK.cs_receipt";
                sql_delete = sql_delete + " where receipt_id = '" + id + "' and confirm='N' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_update);
                    LoadDataResep();
                    //MessageBox.Show("Data Berhasil di hapus");
                    labelControl192.Visible = true;
                    labelControl192.Text = "Hapus Berhasil";
                    Blinking(labelControl192, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }

        }

        private void loadSKD_Click(object sender, EventArgs e)
        {
            string sql_load = "";
            string s_rm = "", s_que = "", s_date = "", p_rm = "", p_que = "", p_date = "", p_name = "", p_anamnesa = "", p_diagnosa = "", p_nik = "", p_kk = "";

            s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();

            sql_load = " select a.patient_no, a.name, c.rm_no, to_char(b.visit_date,'yyyy-mm-dd') visit_date, que01, que02,  " +
                       " (select  'Tensi : ' || blood_press || ', Nadi : ' || pulse ||  " +
                       //" ', Suhu : ' || temperature || ', Alergi : ' || allergy || ', Keluhan : ' || anamnesa as anamnesa   " +
                       " ', Suhu : ' || temperature || ', BB : ' || bb || ', TB : ' || tb || ', Alergi : ' || allergy || ', Keluhan : ' || anamnesa as anamnesa   " +
                       " from KLINIK.cs_anamnesa " +
                       " where rm_no=c.rm_no " +
                       " and insp_date=trunc(b.visit_date)  " +
                       " and visit_no=b.que01) anamnesa,  " +
                       " (select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa  " +
                       " from KLINIK.cs_diagnosa a  " +
                       " join cs_diagnosa_item b on (a.item_cd=b.item_cd)  " +
                       " where b.status='A'  " +
                       " and rm_no=c.rm_no  " +
                       " and insp_date=trunc(b.visit_date) " +
                       " and visit_no=b.que01) as diagnosa, decode(b.work_accident,'N','Umum','Kecelakaan kerja') work_accident " +
                       " from KLINIK.cs_patient_info a  " +
                       " join KLINIK.cs_visit b on (a.patient_no = b.patient_no)  " +
                       " join KLINIK.cs_patient c on(b.patient_no = c.patient_no)  " +
                       " where id_visit = '" + idvisit.ToString() + "' " + 
                       " and c.status = 'A'  " + 
                       " and c.rm_no = '" + s_rm + "' ";


            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_load, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);
            p_rm = dt.Rows[0]["rm_no"].ToString();
            p_que = dt.Rows[0]["que01"].ToString();
            p_date = dt.Rows[0]["visit_date"].ToString();

            p_name = dt.Rows[0]["name"].ToString();
            p_nik = dt.Rows[0]["patient_no"].ToString();
            p_anamnesa = dt.Rows[0]["anamnesa"].ToString();
            p_diagnosa = dt.Rows[0]["diagnosa"].ToString();
            p_kk = dt.Rows[0]["work_accident"].ToString();

            lSkdRm.Text = p_rm;
            lSkdQue.Text = p_que;
            lSkdDate.Text = p_date;

            lSkdName.Text = p_name;
            lSkdNik.Text = p_nik;
            lSkdAnam.Text = p_anamnesa;
            lSkdDiag.Text = p_diagnosa;
            lSkdType.Text = p_kk;

            LoadDataSkd();

            if (lSkdID.Text == "")
            {
                skdUPrint.Enabled = false;
                skdUDel.Enabled = false;
                skdKPrint.Enabled = false;
                skdKDel.Enabled = false;
            }
            else
            {
                skdUPrint.Enabled = true;
                skdUDel.Enabled = true;
                skdKPrint.Enabled = true;
                skdKDel.Enabled = true;
            }
        }

        private void LoadDataSkd()
        {
            string sql_umum = "", sql_kk = "", sql_query = "";
            string letter_id = "", letter_dt = "", bgn_rest = "", end_rest = "", cnt_rest = "";
            string letter_id2 = "", letter_no = "", letter_dt2 = "", bgn_limit = "", end_limit = "", limit01 = "", limit02 = "", limit03 = "", limit04 = "", limit05 = "";
            string limit06 = "", limit07 = "", limit08 = "", limit09 = "", limit10 = "", remark_m = "", remark = "";
            string bgn_rest2 = "", end_rest2 = "", return_work = "", control = "";

            sql_umum = " select letter_id, to_char(letter_dt,'yyyy-mm-dd') letter_dt,  " +
                       " to_char(bgn_rest, 'yyyy-mm-dd') bgn_rest,  " +
                       " to_char(end_rest, 'yyyy-mm-dd') end_rest, cnt_rest  " +
                       " from KLINIK.cs_sick_leter  " +
                       " where rm_no = '" + lSkdRm.Text + "'  " +
                       " and to_char(insp_date,'yyyy-mm-dd')= '" + lSkdDate.Text + "'  " +
                       " and visit_no = '" + lSkdQue.Text + "' ";


            sql_kk = " select letter_id, letter_no, to_char(letter_dt,'yyyy-mm-dd') letter_dt,   " +
                       " to_char(bgn_limit, 'yyyy-mm-dd') bgn_limit,  " +
                       " to_char(end_limit, 'yyyy-mm-dd') end_limit,  " +
                       " nvl(limit01, 'N') limit01,  " +
                       " nvl(limit02, 'N') limit02,  " +
                       " nvl(limit03, 'N') limit03, remark_machine,  " +
                       " nvl(limit04, 'N') limit04,  " +
                       " nvl(limit05, 'N') limit05,  " +
                       " nvl(limit06, 'N') limit06,  " +
                       " nvl(limit07, 'N') limit07,  " +
                       " nvl(limit08, 'N') limit08, " +
                       " nvl(limit09, 'N') limit09,  " +
                       " nvl(limit10, 'N') limit10, remark,  " +
                       " to_char(bgn_rest, 'yyyy-mm-dd') bgn_rest,  " +
                       " to_char(end_rest, 'yyyy-mm-dd') end_rest, return_work, " +
                       " to_char(control, 'yyyy-mm-dd') control  " +
                       " from KLINIK.cs_sick_leter  " +
                       " where rm_no = '" + lSkdRm.Text + "'  " +
                       " and to_char(insp_date,'yyyy-mm-dd')= '" + lSkdDate.Text + "'  " +
                       " and visit_no = '" + lSkdQue.Text + "' ";

            if (lSkdType.Text == "Umum")
            {
                sql_query = sql_umum;
            }
            else
            {
                sql_query = sql_kk;
            }

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_query, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            if (dt.Rows.Count > 0)
            {


                if (lSkdType.Text == "Umum")
                {
                    letter_id = dt.Rows[0]["letter_id"].ToString();
                    letter_dt = dt.Rows[0]["letter_dt"].ToString();
                    bgn_rest = dt.Rows[0]["bgn_rest"].ToString();
                    end_rest = dt.Rows[0]["end_rest"].ToString();
                    cnt_rest = dt.Rows[0]["cnt_rest"].ToString();

                    lSkdID.Text = letter_id;
                    dLetterDate.Text = letter_dt;
                    dLetterStart.Text = bgn_rest;
                    dLetterEnd.Text = end_rest;
                    tLetterCnt.Text = cnt_rest;
                }
                else
                {
                    letter_id2 = dt.Rows[0]["letter_id"].ToString();
                    letter_no = dt.Rows[0]["letter_no"].ToString();
                    letter_dt2 = dt.Rows[0]["letter_dt"].ToString();
                    bgn_limit = dt.Rows[0]["bgn_limit"].ToString();
                    end_limit = dt.Rows[0]["end_limit"].ToString();
                    limit01 = dt.Rows[0]["limit01"].ToString();
                    limit02 = dt.Rows[0]["limit02"].ToString();
                    limit03 = dt.Rows[0]["limit03"].ToString();
                    remark_m = dt.Rows[0]["remark_machine"].ToString();
                    limit04 = dt.Rows[0]["limit04"].ToString();
                    limit05 = dt.Rows[0]["limit05"].ToString();
                    limit06 = dt.Rows[0]["limit06"].ToString();
                    limit07 = dt.Rows[0]["limit07"].ToString();
                    limit08 = dt.Rows[0]["limit08"].ToString();
                    limit09 = dt.Rows[0]["limit09"].ToString();
                    limit10 = dt.Rows[0]["limit10"].ToString();
                    remark = dt.Rows[0]["remark"].ToString();
                    bgn_rest2 = dt.Rows[0]["bgn_rest"].ToString();
                    end_rest2 = dt.Rows[0]["end_rest"].ToString();
                    return_work = dt.Rows[0]["return_work"].ToString();
                    control = dt.Rows[0]["control"].ToString();

                    lSkdID.Text = letter_id2;
                    tLetterNo.Text = letter_no;
                    dLetterDate2.Text = letter_dt2;
                    dLetterLimitStart.Text = bgn_limit;
                    dLetterLimitEnd.Text = end_limit;
                    tMachineRemark.Text = remark_m;
                    tLetterRemark.Text = remark;

                    if (return_work == "N") { cLetterReturn.Checked = false; } else { cLetterReturn.Checked = true; }
                    if (dLetterLimitStart.Text == "" && dLetterLimitEnd.Text == "") { cLetterLimit.Checked = false; } else { cLetterLimit.Checked = true; }

                    if (limit01 == "N") { cLimit01.Checked = false; } else { cLimit01.Checked = true; }
                    if (limit02 == "N") { cLimit02.Checked = false; } else { cLimit02.Checked = true; }
                    if (limit03 == "N") { cLimit03.Checked = false; } else { cLimit03.Checked = true; }
                    if (limit04 == "N") { cLimit04.Checked = false; } else { cLimit04.Checked = true; }
                    if (limit05 == "N") { cLimit05.Checked = false; } else { cLimit05.Checked = true; }
                    if (limit06 == "N") { cLimit06.Checked = false; } else { cLimit06.Checked = true; }
                    if (limit07 == "N") { cLimit07.Checked = false; } else { cLimit07.Checked = true; }
                    if (limit08 == "N") { cLimit08.Checked = false; } else { cLimit08.Checked = true; }
                    if (limit09 == "N") { cLimit09.Checked = false; } else { cLimit09.Checked = true; }
                    if (limit10 == "N") { cLimit10.Checked = false; } else { cLimit10.Checked = true; }

                    dRestStart.Text = bgn_rest2;
                    dRestEnd.Text = end_rest2;
                    dLetterControl.Text = control;

                    if (dRestStart.Text == "" && dRestEnd.Text == "") { cLetterRest.Checked = false; } else { cLetterRest.Checked = true; }
                    if (dLetterControl.Text == "") { cLetterControl.Checked = false; } else { cLetterControl.Checked = true; }
                }
            }
            else
            {
                lSkdID.Text = "";
                dLetterDate.Text = today;
                dLetterStart.Text = today;
                dLetterEnd.Text = today;
                tLetterCnt.Text = "";

                tLetterNo.Text = "";
                dLetterDate2.Text = today;
                dLetterLimitStart.Text = today;
                dLetterLimitEnd.Text = today;
                tMachineRemark.Text = "";
                tLetterRemark.Text = "";
                dRestStart.Text = today;
                dRestEnd.Text = today;
                dLetterControl.Text = today;

                cLetterReturn.Checked = false;
                cLetterLimit.Checked = false;
                cLimit01.Checked = false;
                cLimit02.Checked = false;
                cLimit03.Checked = false;
                cLimit04.Checked = false;
                cLimit05.Checked = false;
                cLimit06.Checked = false;
                cLimit07.Checked = false;
                cLimit08.Checked = false;
                cLimit09.Checked = false;
                cLimit10.Checked = false;
                cLetterRest.Checked = false;
                cLetterControl.Checked = false;

            }

        }

        private void lSkdType_TextChanged(object sender, EventArgs e)
        {
            if (lSkdType.Text == "Umum")
            {
                grpSkdUmum.Visible = true;
                grpSkdUmum.Dock = DockStyle.Fill;
                grpSkdKec.Visible = false;
            }
            else
            {
                grpSkdKec.Visible = true;
                grpSkdKec.Dock = DockStyle.Fill;
                grpSkdUmum.Visible = false;
            }
        }

        private void skdUSave_Click(object sender, EventArgs e)
        {
            string sql_cnt = "";
            string skd_cnt = "";

            if (lSkdRm.Text == "")
            {
                MessageBox.Show("Silahkan load data pasien");
            }
            else if (dLetterDate.Text == "")
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
                sql_cnt = " select count(0) cnt from KLINIK.cs_sick_leter where to_char(insp_date,'yyyy-mm-dd') = '" + lSkdDate.Text + "' and visit_no = '" + lSkdQue.Text + "' and rm_no = '" + lSkdRm.Text + "' ";
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);
                skd_cnt = dt.Rows[0]["cnt"].ToString();

                if (Convert.ToInt32(skd_cnt) > 0)
                {
                    // update data

                    string sql_update = "";

                    sql_update = " update cs_sick_leter set letter_dt = to_date('" + dLetterDate.Text + "','yyyy-mm-dd'), bgn_rest = to_date('" + dLetterStart.Text + "','yyyy-mm-dd'), end_rest = to_date('" + dLetterEnd.Text + "','yyyy-mm-dd'), cnt_rest = '" + tLetterCnt.Text + "', upd_emp='" + DB.vUserId + "', upd_date = sysdate " +
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

                    sql_insert = " insert into cs_sick_leter (letter_id, rm_no, insp_date, print_yn, letter_dt, bgn_rest, end_rest, cnt_rest, visit_no, ins_date, ins_emp)  " +
                                 " values (cs_sick_seq.nextval,'" + lSkdRm.Text + "',to_date('" + lSkdDate.Text + "','yyyy-mm-dd'), 'N',to_date('" + dLetterDate.Text + "','yyyy-mm-dd'),to_date('" + dLetterStart.Text + "','yyyy-mm-dd'), to_date('" + dLetterEnd.Text + "','yyyy-mm-dd'),'" + tLetterCnt.Text + "','" + lSkdQue.Text + "',sysdate,'" + DB.vUserId + "')  ";

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

        private void skdKSave_Click(object sender, EventArgs e)
        {
            string sql_cnt = "";
            string skd_cnt = "";

            if (lSkdRm.Text == "")
            {
                MessageBox.Show("Silahkan load data pasien.");
            }
            else if (cLetterLimit.Checked == true && (dLetterLimitStart.Text == "" || dLetterLimitEnd.Text == ""))
            {
                MessageBox.Show("Tanggal keterbataasan harus diisi.");
            }
            else if (cLetterRest.Checked == true && (dRestStart.Text == "" || dRestEnd.Text == ""))
            {
                MessageBox.Show("Tanggal istirahat harus diisi.");
            }
            else if (cLetterControl.Checked == true && dLetterControl.Text == "")
            {
                MessageBox.Show("Tanggal kontrol harus diisi.");
            }
            else if (tLetterNo.Text == "")
            {
                MessageBox.Show("No Surat harus diisi.");
            }
            else if (dLetterDate2.Text == "")
            {
                MessageBox.Show("Tanggal Surat harus diisi.");
            }
            else if (cLetterReturn.Checked == false && cLetterLimit.Checked == false && cLetterRest.Checked == false && cLetterControl.Checked == false)
            {
                MessageBox.Show("Silahkan pilih rekomendasi");
            }
            else
            {
                //MessageBox.Show("Ok");

                sql_cnt = " select count(0) cnt from KLINIK.cs_sick_leter where to_char(insp_date,'yyyy-mm-dd') = '" + lSkdDate.Text + "' and visit_no = '" + lSkdQue.Text + "' and rm_no = '" + lSkdRm.Text + "' ";
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);
                skd_cnt = dt.Rows[0]["cnt"].ToString();

                if (Convert.ToInt32(skd_cnt) > 0)
                {
                    // update data
                    string tmp_return = (cLetterReturn.Checked == false) ? "N" : "Y";
                    string tmp_limit01 = (cLimit01.Checked == false) ? "N" : "Y";
                    string tmp_limit02 = (cLimit02.Checked == false) ? "N" : "Y";
                    string tmp_limit03 = (cLimit03.Checked == false) ? "N" : "Y";
                    string tmp_limit04 = (cLimit04.Checked == false) ? "N" : "Y";
                    string tmp_limit05 = (cLimit05.Checked == false) ? "N" : "Y";
                    string tmp_limit06 = (cLimit06.Checked == false) ? "N" : "Y";
                    string tmp_limit07 = (cLimit07.Checked == false) ? "N" : "Y";
                    string tmp_limit08 = (cLimit08.Checked == false) ? "N" : "Y";
                    string tmp_limit09 = (cLimit09.Checked == false) ? "N" : "Y";
                    string tmp_limit10 = (cLimit10.Checked == false) ? "N" : "Y";













                    string sql_update = "";

                    sql_update = " update cs_sick_leter set letter_no = '" + tLetterNo.Text + "', letter_dt = to_date('" + dLetterDate2.Text + "','yyyy-mm-dd'), " +
                                 " bgn_limit = to_date('" + dLetterLimitStart.Text + "','yyyy-mm-dd'), end_limit = to_date('" + dLetterLimitEnd.Text + "','yyyy-mm-dd'), " +
                                 " limit01 = '" + tmp_limit01 + "', limit02 = '" + tmp_limit02 + "', limit03 = '" + tmp_limit03 + "', limit04 = '" + tmp_limit04 + "', limit05 = '" + tmp_limit05 + "', " +
                                 " limit06 = '" + tmp_limit06 + "', limit07 = '" + tmp_limit07 + "', limit08 = '" + tmp_limit08 + "', limit09 = '" + tmp_limit09 + "', limit10 = '" + tmp_limit10 + "', " +
                                 " bgn_rest= to_date('" + dRestStart.Text + "','yyyy-mm-dd'), end_rest = to_date('" + dRestEnd.Text + "','yyyy-mm-dd'), " +
                                 " remark_machine = '" + tMachineRemark.Text + "', remark = '" + tLetterRemark.Text + "', return_work='" + tmp_return + "', control=to_date('" + dLetterControl.Text + "','yyyy-mm-dd'), " +
                                 " upd_emp='" + DB.vUserId + "', upd_date = sysdate " +
                                 " where letter_id='" + lSkdID.Text + "' ";

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

                    string temp_return = (cLetterReturn.Checked == false) ? "N" : "Y";
                    string temp_limit01 = (cLimit01.Checked == false) ? "N" : "Y";
                    string temp_limit02 = (cLimit02.Checked == false) ? "N" : "Y";
                    string temp_limit03 = (cLimit03.Checked == false) ? "N" : "Y";
                    string temp_limit04 = (cLimit04.Checked == false) ? "N" : "Y";
                    string temp_limit05 = (cLimit05.Checked == false) ? "N" : "Y";
                    string temp_limit06 = (cLimit06.Checked == false) ? "N" : "Y";
                    string temp_limit07 = (cLimit07.Checked == false) ? "N" : "Y";
                    string temp_limit08 = (cLimit08.Checked == false) ? "N" : "Y";
                    string temp_limit09 = (cLimit09.Checked == false) ? "N" : "Y";
                    string temp_limit10 = (cLimit10.Checked == false) ? "N" : "Y";













                    string sql_insert = "";

                    sql_insert = " insert into cs_sick_leter (letter_id, rm_no, insp_date, print_yn, letter_no, letter_dt, return_work, bgn_limit, end_limit, limit01, limit02, limit03, limit04, limit05, limit06, limit07, limit08, limit09, limit10, bgn_rest, end_rest, remark_machine, remark, control, visit_no, ins_date, ins_emp)  " +
                                 " values (cs_sick_seq.nextval,'" + lSkdRm.Text + "',to_date('" + lSkdDate.Text + "','yyyy-mm-dd'), 'N', " +
                                 " '" + tLetterNo.Text + "', to_date('" + dLetterDate2.Text + "','yyyy-mm-dd'), '" + temp_return + "', " +
                                 " to_date('" + dLetterLimitStart.Text + "','yyyy-mm-dd'), to_date('" + dLetterLimitEnd.Text + "','yyyy-mm-dd'), " +
                                 " '" + temp_limit01 + "', '" + temp_limit02 + "', '" + temp_limit03 + "', '" + temp_limit04 + "', '" + temp_limit05 + "', " +
                                 " '" + temp_limit06 + "', '" + temp_limit07 + "', '" + temp_limit08 + "', '" + temp_limit09 + "', '" + temp_limit10 + "', " +
                                 " to_date('" + dRestStart.Text + "','yyyy-mm-dd'), to_date('" + dRestEnd.Text + "','yyyy-mm-dd'), " +
                                 " '" + tMachineRemark.Text + "','" + tLetterRemark.Text + "',to_date('" + dLetterControl.Text + "','yyyy-mm-dd'), " +
                                 " '" + lSkdQue.Text + "',sysdate,'" + DB.vUserId + "')  ";

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

        private void cLetterLimit_CheckedChanged(object sender, EventArgs e)
        {
            if (cLetterLimit.Checked == false)
            {
                dLetterLimitStart.Text = "";
                dLetterLimitEnd.Text = "";
            }
        }

        private void cLimit03_CheckedChanged(object sender, EventArgs e)
        {
            if (cLimit03.Checked == false)
            {
                tMachineRemark.Text = "";
            }
        }

        private void cLimit10_CheckedChanged(object sender, EventArgs e)
        {
            if (cLimit10.Checked == false)
            {
                tLetterRemark.Text = "";
            }
        }

        private void skdKDel_Click(object sender, EventArgs e)
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

                sql_delete = " delete from cs_sick_leter where letter_id = '" + lSkdID.Text + "'  ";

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

        private void cLetterRest_CheckedChanged(object sender, EventArgs e)
        {
            if (cLetterRest.Checked == false)
            {
                dRestStart.Text = "";
                dRestEnd.Text = "";
            }
        }

        private void cLetterControl_CheckedChanged(object sender, EventArgs e)
        {
            if (cLetterControl.Checked == false)
            {
                dLetterControl.Text = "";
            }
        }

        private void btnRefPrint_Click(object sender, EventArgs e)
        {
            ReportRujukan report = new ReportRujukan(dsRujukan);
            report.ShowPreviewDialog();
        }

        private void btnRecPrint_Click(object sender, EventArgs e)
        {
            ReportRekomendasiPr report = new ReportRekomendasiPr(dsRekomendasi);
            report.ShowPreviewDialog();
        }

        private void skdUPrint_Click(object sender, EventArgs e)
        {
            getSkd();
            ReportSkdUmum report = new ReportSkdUmum(dsSkd);
            report.ShowPreviewDialog();
        }

        private void getSkd()
        {
            string sql_skd = "";

            sql_skd = sql_skd + Environment.NewLine + "select a.patient_no, a.name, a.gender, round(((sysdate-birth_date)/30)/12) age, job, null position,  ";
            sql_skd = sql_skd + Environment.NewLine + "(select LISTAGG(item_name, ', ') WITHIN GROUP(ORDER BY type_diagnosa asc) diagnosa  ";
            sql_skd = sql_skd + Environment.NewLine + "from cs_diagnosa a   join cs_diagnosa_item b on (a.item_cd = b.item_cd)  ";
            sql_skd = sql_skd + Environment.NewLine + "where b.status = 'A'  ";
            sql_skd = sql_skd + Environment.NewLine + "and rm_no = c.rm_no  ";
            sql_skd = sql_skd + Environment.NewLine + "and insp_date = trunc(b.visit_date)  ";
            sql_skd = sql_skd + Environment.NewLine + "and visit_no = b.que01) as diagnosa, letter_no,  ";
            sql_skd = sql_skd + Environment.NewLine + "TO_CHAR(visit_date, 'dd Month yyyy', 'nls_date_language = INDONESIAN') visit_date,  ";
            sql_skd = sql_skd + Environment.NewLine + "TO_CHAR(letter_dt, 'dd fmMonth yyyy', 'nls_date_language = INDONESIAN') letter_dt,  ";
            sql_skd = sql_skd + Environment.NewLine + "TO_CHAR(bgn_rest, 'dd Mon yyyy', 'nls_date_language = INDONESIAN') bgn_rest,  ";
            sql_skd = sql_skd + Environment.NewLine + "TO_CHAR(end_rest, 'dd Mon yyyy', 'nls_date_language = INDONESIAN') end_rest, cnt_rest,  ";
            sql_skd = sql_skd + Environment.NewLine + "TO_CHAR(bgn_limit, 'dd Mon yyyy', 'nls_date_language = INDONESIAN') bgn_limit,  ";
            sql_skd = sql_skd + Environment.NewLine + "TO_CHAR(end_limit, 'dd Mon yyyy', 'nls_date_language = INDONESIAN') end_limit,  ";
            sql_skd = sql_skd + Environment.NewLine + "decode(limit01, 'Y','V','') limit01,   ";
            sql_skd = sql_skd + Environment.NewLine + "decode(limit02, 'Y','V','') limit02,   ";
            sql_skd = sql_skd + Environment.NewLine + "decode(limit03, 'Y','V','') limit03, remark_machine,  ";
            sql_skd = sql_skd + Environment.NewLine + "decode(limit04, 'Y','V','') limit04,   ";
            sql_skd = sql_skd + Environment.NewLine + "decode(limit05, 'Y','V','') limit05,   ";
            sql_skd = sql_skd + Environment.NewLine + "decode(limit06, 'Y','V','') limit06,   ";
            sql_skd = sql_skd + Environment.NewLine + "decode(limit07, 'Y','V','') limit07,   ";
            sql_skd = sql_skd + Environment.NewLine + "decode(limit08, 'Y','V','') limit08,   ";
            sql_skd = sql_skd + Environment.NewLine + "decode(limit09, 'Y','V','') limit09,   ";
            sql_skd = sql_skd + Environment.NewLine + "decode(limit10, 'Y','V','') limit10, remark, decode(return_work, 'Y','V','') return_work,  ";
            sql_skd = sql_skd + Environment.NewLine + "TO_CHAR(control, 'dd Month yyyy', 'nls_date_language = INDONESIAN') control, b.purpose,  ";
            sql_skd = sql_skd + Environment.NewLine + "decode (b.purpose,'DOC','dr. ','') || (select distinct klinik.FN_GET_NAME(ins_emp) nama  ";
            sql_skd = sql_skd + Environment.NewLine + " from cs_diagnosa a      ";
            sql_skd = sql_skd + Environment.NewLine + " where rm_no = c.rm_no     ";
            sql_skd = sql_skd + Environment.NewLine + " and insp_date = trunc(visit_date)  ";
            sql_skd = sql_skd + Environment.NewLine + " and visit_no = que01) pic,  ";
            sql_skd = sql_skd + Environment.NewLine + "'Dokter Pemeriksa' as pic_info  ";
            sql_skd = sql_skd + Environment.NewLine + "from cs_patient_info a  ";
            sql_skd = sql_skd + Environment.NewLine + "join cs_visit b on (a.patient_no = b.patient_no)  ";
            sql_skd = sql_skd + Environment.NewLine + "join cs_patient c on(b.patient_no = c.patient_no)  ";
            sql_skd = sql_skd + Environment.NewLine + "join cs_sick_leter d on(c.rm_no = d.rm_no)  ";
            sql_skd = sql_skd + Environment.NewLine + "where b.que01 = d.visit_no  ";
            sql_skd = sql_skd + Environment.NewLine + "and trunc(b.visit_date) = d.insp_date  ";
            sql_skd = sql_skd + Environment.NewLine + "and to_char(b.visit_date, 'yyyy-mm-dd') = '" + lSkdDate.Text + "'  ";
            sql_skd = sql_skd + Environment.NewLine + "and c.status = 'A'   and b.que01 = '" + lSkdQue.Text + "'  ";
            sql_skd = sql_skd + Environment.NewLine + "and c.group_patient <> 'COMM'   and c.rm_no = '" + lSkdRm.Text + "' ";


            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_skd, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            dsSkd.Tables.Clear();
            dsSkd.Tables.Add(dt);
        }

        private void skdKPrint_Click(object sender, EventArgs e)
        {
            getSkd();
            ReportSkdKK report = new ReportSkdKK(dsSkd);
            report.ShowPreviewDialog();
        }

        private void loadMR_Click(object sender, EventArgs e)
        {
            string sql_load = "", sql_mr_load = "", sql_mr_print = "";
            string s_rm = "", s_que = "", s_date = "";
            string p_name = "", p_nik = "", p_line = "", p_rm = "", p_address = "", p_age = "", p_gender = "", p_group = "", p_darah = "";
            string p_gpa = "", p_hpht = "", p_tp = "";

            s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();

            sql_load = " select a.name, a.patient_no, null line, c.rm_no, a.address, gender,   " +
                       " a.birth_place || ', ' || birth_date || ' (' || round(((sysdate-birth_date)/30)/12) || ' tahun)'  as ttl, " +
                       " group_patient, null blood_type, c.info04 as gpa, c.info05 hpht,   " +
                       " TO_CHAR(to_date(c.info07,'yyyy-mm-dd'), 'FmDD Month yyyy', 'nls_date_language = INDONESIAN') as tp  " +
                       " from cs_patient_info a   " +
                       " join cs_visit b on (a.patient_no = b.patient_no)   " +
                       " join cs_patient c on (b.patient_no = c.patient_no)   " +
                       " where 1 = 1   " +
                       " and to_char(b.visit_date, 'yyyy-mm-dd') = '" + s_date + "'   " +
                       " and c.status = 'A'   and b.que01 = '" + s_que + "'   " +
                       " and c.group_patient <> 'COMM'   and c.rm_no = '" + s_rm + "'   ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_load, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            p_name = dt.Rows[0]["name"].ToString();
            p_nik = dt.Rows[0]["patient_no"].ToString();
            p_line = dt.Rows[0]["line"].ToString();
            p_rm = dt.Rows[0]["rm_no"].ToString();
            p_address = dt.Rows[0]["address"].ToString();
            p_age = dt.Rows[0]["ttl"].ToString();
            p_gender = dt.Rows[0]["gender"].ToString();
            p_group = dt.Rows[0]["group_patient"].ToString();
            p_darah = dt.Rows[0]["blood_type"].ToString();
            p_gpa = dt.Rows[0]["gpa"].ToString();
            p_hpht = dt.Rows[0]["hpht"].ToString();
            p_tp = dt.Rows[0]["tp"].ToString();

            lMrName.Text = p_name;
            lMrNik.Text = p_nik;
            lMrNo.Text = p_rm;
            lMrAddr.Text = p_address;
            lMrTtl.Text = p_age;
            lMRGrp.Text = p_group;

            //sql_mr_load = " select visit_no, to_char(b.insp_date,'yyyy-mm-dd') ddate,  " +
            //              " 'Tensi : ' || blood_press || ', Nadi : ' || pulse ||  " +
            //              " ', Suhu : ' || temperature || ', Alergi : ' || allergy ||  " +
            //              " ', Keluhan : ' || anamnesa as anamnesa,  " +
            //              " (select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa  " +
            //              " from cs_diagnosa a  " +
            //              " join cs_diagnosa_item b on (a.item_cd = b.item_cd)  " +
            //              " where b.status = 'A'  " +
            //              " and rm_no = b.rm_no  " +
            //              " and insp_date = b.insp_date  " +
            //              " and visit_no = b.visit_no) diagnosa,  " +
            //              " (select LISTAGG(initcap(med_name), ', ') WITHIN GROUP (ORDER BY med_name asc) resep  " +
            //              " from cs_receipt a  " +
            //              " join cs_medicine b on (a.med_cd = b.med_cd)  " +
            //              " where b.status = 'A'  " +
            //              " and rm_no = b.rm_no  " +
            //              " and insp_date = b.insp_date  " +
            //              " and visit_no = b.visit_no) terapi  " +
            //              " from cs_patient a  " +
            //              " join cs_anamnesa b on (a.rm_no = b.rm_no)  " +
            //              " where a.status = 'A'  " +
            //              " and group_patient <> 'COMM'  " +
            //              " and b.rm_no = '" + s_rm + "' order by b.insp_date, visit_no desc ";

            sql_mr_load = "";
            sql_mr_load = sql_mr_load + Environment.NewLine + "select poli_cd, ddate, anamnesa, diagnosa,  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "terapi, pic ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "from ( ";
            //sql_mr_load = sql_mr_load + Environment.NewLine + "select a.rm_no,visit_no, insp_date, to_char(b.insp_date,'yyyy-mm-dd') ddate, ";


            sql_mr_load = sql_mr_load + Environment.NewLine + "select a.rm_no,visit_no, insp_date,  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "(select distinct poli_name from cs_visit aa  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "join cs_policlinic bb on (aa.poli_cd=bb.poli_cd)  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "where trunc(visit_date)=b.insp_date  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "and visit_no=que01  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "and aa.patient_no=a.patient_no) poli_cd, ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "(select to_char(visit_date,'yyyy-mm-dd hh24:mi:ss') ddate  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "from cs_visit aa ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "join cs_patient bb ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "on aa.patient_no=bb.patient_no ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "where bb.status='A' ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "and to_char(visit_date,'yyyy-mm-dd')=to_char(b.insp_date,'yyyy-mm-dd') ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "and que01=b.visit_no ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "and rm_no=a.rm_no) ddate, ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "'Tensi : ' || blood_press || ',' || ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "'Nadi : ' || pulse || ',' || ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "'Suhu : ' || temperature || ',' || ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "'BB : ' || bb || ',' || ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "'TB : ' || tb || ',' || ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "'Alergi : ' || allergy || ',' || ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "'Keluhan : ' || anamnesa || ',' || ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "'Fisik : ' || anamnesa_physical as anamnesa,  ";




            //sql_mr_load = sql_mr_load + Environment.NewLine + "(select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "(select LISTAGG(item_name || decode(remark,null,null, ' (' || remark || ')'), ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " from cs_diagnosa a   join cs_diagnosa_item b on (a.item_cd = b.item_cd) ";
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
            sql_mr_load = sql_mr_load + Environment.NewLine + " from cs_receipt a  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " join cs_medicine b on (a.med_cd = b.med_cd) ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " where b.status = 'A' ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " and rm_no = b.rm_no  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " and insp_date = b.insp_date ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " and visit_no = b.visit_no) || ', ' || ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "'SKD : ' || (select nvl(cnt_rest,end_rest - (bgn_rest -1)) skd_cnt ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " from cs_sick_leter a ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' || ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "'OBS : ' || (select hrs_cnt hrs_cnt ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " from cs_observation a ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "'Tindakan : ' || (select act_name ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " from cs_action a ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "'Rujukan : ' || (select hos_name || ' / ' || hos_doc ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " from cs_refer a ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "'Rekomendasi : ' || (select recom_remark ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " from cs_recommendation a ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + " and visit_no = b.visit_no )  terapi,  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "klinik.FN_GET_PIC(b.rm_no, insp_date, visit_no) pic  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "from cs_patient a ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "join cs_anamnesa b on (a.rm_no = b.rm_no) ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "where a.status = 'A' ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "and group_patient <> 'COMM' ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "and b.rm_no = '" + s_rm + "') ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "where 1=1 ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "order by ddate desc  ";


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

            gridView7.Columns[0].Caption = "No Antrian";
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

            if (gridView7.RowCount > 0)
            {
                btnMrPrint.Enabled = true;
            }
            else
            {
                btnMrPrint.Enabled = false;
            }

            //sql_mr_print = " select '"+ p_name + "' name, '"+ p_nik + "' nik, '" + p_rm + "' rm, '" + p_address + "' addr, '" + p_age + "' age, '" + p_line + "' dept, " +
            //              " '" + p_gpa + "' gpa, '" + p_hpht + "' hpht, '" + p_tp + "' tp, '" + p_darah + "' darah, " +
            //              " visit_no, to_char(b.insp_date,'yyyy-mm-dd') ddate,  " +
            //              " 'Tensi : ' || blood_press || ', Nadi : ' || pulse ||  " +
            //              " ', Suhu : ' || temperature || ', Alergi : ' || allergy ||  " +
            //              " ', Keluhan : ' || anamnesa as anamnesa,  " +
            //              " (select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa  " +
            //              " from cs_diagnosa a  " +
            //              " join cs_diagnosa_item b on (a.item_cd = b.item_cd)  " +
            //              " where b.status = 'A'  " +
            //              " and rm_no = b.rm_no  " +
            //              " and insp_date = b.insp_date  " +
            //              " and visit_no = b.visit_no) diagnosa,  " +
            //              " (select LISTAGG(initcap(med_name), ', ') WITHIN GROUP (ORDER BY med_name asc) resep  " +
            //              " from cs_receipt a  " +
            //              " join cs_medicine b on (a.med_cd = b.med_cd)  " +
            //              " where b.status = 'A'  " +
            //              " and rm_no = b.rm_no  " +
            //              " and insp_date = b.insp_date  " +
            //              " and visit_no = b.visit_no) terapi  " +
            //              " from cs_patient a  " +
            //              " join cs_anamnesa b on (a.rm_no = b.rm_no)  " +
            //              " where a.status = 'A'  " +
            //              " and group_patient <> 'COMM'  " +
            //              " and b.rm_no = '" + s_rm + "' order by b.insp_date, visit_no desc ";

            sql_mr_print = "";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select '" + p_name + "' name, '" + p_nik + "' nik, '" + p_rm + "' rm, '" + p_address + "' addr, '" + p_age + "' age, '" + p_line + "' dept,  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'" + p_gpa + "' gpa, '" + p_hpht + "' hpht, '" + p_tp + "' tp, '" + p_darah + "' darah, ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "poli_cd, ddate, anamnesa, diagnosa,terapi, pic ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "from ( ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "select a.rm_no,visit_no, insp_date, to_char(b.insp_date,'yyyy-mm-dd') ddate, ";


            sql_mr_print = sql_mr_print + Environment.NewLine + "select a.rm_no,visit_no, insp_date,  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "(select distinct poli_name from cs_visit aa  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "join cs_policlinic bb on (aa.poli_cd=bb.poli_cd)  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "where trunc(visit_date)=b.insp_date  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and visit_no=que01  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and aa.patient_no=a.patient_no) poli_cd,    ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "(select to_char(visit_date,'yyyy-mm-dd hh24:mi:ss') ddate  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "from cs_visit aa ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "join cs_patient bb ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "on aa.patient_no=bb.patient_no ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "where bb.status='A' ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and to_char(visit_date,'yyyy-mm-dd')=to_char(b.insp_date,'yyyy-mm-dd') ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and que01=b.visit_no ";

            sql_mr_print = sql_mr_print + Environment.NewLine + "and rm_no=a.rm_no) ddate, ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Tensi : ' || blood_press || ',' ||   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Nadi : ' || pulse || ',' ||   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Suhu : ' || temperature || ',' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'BB : ' || bb || ',' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'TB : ' || tb || ',' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Alergi : ' || allergy || ',' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Keluhan : ' || anamnesa || ',' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Fisik : ' || anamnesa_physical as anamnesa,  ";




            //sql_mr_print = sql_mr_print + Environment.NewLine + "(select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "(select LISTAGG(item_name || decode(remark,null,null, ' (' || remark || ')'), ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_diagnosa a   join cs_diagnosa_item b on (a.item_cd = b.item_cd) ";
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
            sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_receipt a  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " join cs_medicine b on (a.med_cd = b.med_cd) ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where b.status = 'A' ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and rm_no = b.rm_no  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no) || ', ' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'SKD : ' || (select nvl(cnt_rest,end_rest - (bgn_rest -1)) skd_cnt ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_sick_leter a ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'OBS : ' || (select hrs_cnt hrs_cnt ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_observation a ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Tindakan : ' || (select act_name ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_action a ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Rujukan : ' || (select hos_name || ' / ' || hos_doc ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_refer a ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Rekomendasi : ' || (select recom_remark ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_recommendation a ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no )  terapi,  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "klinik.FN_GET_PIC(b.rm_no, insp_date, visit_no) pic  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "from cs_patient a ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "join cs_anamnesa b on (a.rm_no = b.rm_no) ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "where a.status = 'A' ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and group_patient <> 'COMM' ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and b.rm_no = '" + s_rm + "') ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "where 1=1 ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "order by ddate desc ";

            OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra3 = new OleDbDataAdapter(sql_mr_print, oraConnect3);
            DataTable dt3 = new DataTable();
            adOra3.Fill(dt3);

            if (lMRGrp.Text == "PREG")
            {
                dsMRHamil.Tables.Clear();
                dsMRHamil.Tables.Add(dt3);
            }
            else
            {
                dsMRKb.Tables.Clear();
                dsMRKb.Tables.Add(dt3);
            }

        }

        private void btnMrPrint_Click(object sender, EventArgs e)
        {
            if (lMRGrp.Text == "PREG")
            {
                ReportMRHamil report = new ReportMRHamil(dsMRHamil);
                report.ShowPreviewDialog();
            }
            else
            {
                ReportMRKb report = new ReportMRKb(dsMRKb);
                report.ShowPreviewDialog();
            }
        }

        private void btnSaveAdd_Click(object sender, EventArgs e)
        {
            string sql_update = "";

            sql_update = " update cs_patient set  info01 = '" + tAdd01.Text + "', info02 = '" + tAdd02.Text + "', info03 = '" + tAdd03.Text + "', info04 = '" + tAdd04.Text + "', info05 = '" + tAdd05.Text + "', " +
                         " info06 = '" + tAdd06.Text + "', info07 = '" + tAdd07.Text + "', info08 = '" + tAdd08.Text + "', info09 = '" + tAdd09.Text + "', info10 = '" + tAdd10.Text + "',  " +
                         " upd_emp='" + DB.vUserId + "', upd_date = sysdate  " +
                         " where status = 'A' and patient_no = '" + lAddEmpid.Text + "' and rm_no='" + lAddRM.Text + "'  ";

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

        private void loadSuratHamil_Click(object sender, EventArgs e)
        {
            string sql_load = "";
            string s_rm = "", s_que = "", s_date = "", p_rm = "", p_que = "", p_date = "", p_name = "", p_nik = "", p_minggu = "", p_taksiran = "";
            string p_tipe = "", p_no_surat = "", p_tgl_surat = "", p_leave = "", p_bgn_date = "", p_end_date = "", p_work = "";
            string p_cer_id = "";

            s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();

            sql_load = "";
            sql_load = sql_load + Environment.NewLine + "SELECT x.patient_no, x.NAME, x.line, x.age, x.rm_no, x.poli_cd, x.visit_date,que01, cer_id, ";
            sql_load = sql_load + Environment.NewLine + "       DECODE (cer_type, 'B', 'Melahirkan', 'M', 'Keguguran', null) cer_type, ";
            sql_load = sql_load + Environment.NewLine + "       letter_no, info02, info07, cnt_leave, info09, info10, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (bgn_work, 'yyyy-mm-dd') bgn_work, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (NVL (letter_dt, SYSDATE), 'yyyy-mm-dd') letter_dt, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (letter_dt,'dd fmMonth yyyy','nls_date_language = INDONESIAN') letter_dt2, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (birth_date,'dd fmMonth yyyy','nls_date_language = INDONESIAN') birth_date2, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (TO_DATE (info09, 'yyyy-mm-dd'),'dd fmMonth yyyy','nls_date_language = INDONESIAN') bgn_date2, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (TO_DATE (info10, 'yyyy-mm-dd'),'dd fmMonth yyyy','nls_date_language = INDONESIAN') end_date2, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (bgn_work,'dd fmMonth yyyy','nls_date_language = INDONESIAN') bgn_work2 ";
            sql_load = sql_load + Environment.NewLine + "  FROM (SELECT a.patient_no, a.NAME, null line, a.age, c.rm_no, b.poli_cd, ";
            sql_load = sql_load + Environment.NewLine + "               TO_CHAR (b.visit_date, 'yyyy-mm-dd') visit_date, que01, info02, ";
            sql_load = sql_load + Environment.NewLine + "               info07, info09, info10 ";
            sql_load = sql_load + Environment.NewLine + "          FROM cs_patient_info a JOIN cs_visit b ON (a.patient_no = b.patient_no) ";
            sql_load = sql_load + Environment.NewLine + "               JOIN cs_patient c ON (b.patient_no = c.patient_no) ";
            sql_load = sql_load + Environment.NewLine + "         WHERE c.status = 'A' AND c.group_patient <> 'COMM') x ";
            sql_load = sql_load + Environment.NewLine + "  LEFT JOIN cs_birth_certificate y  ";
            sql_load = sql_load + Environment.NewLine + "  ON ( x.rm_no = y.rm_no AND TO_DATE (x.visit_date, 'yyyy-mm-dd') = y.insp_date AND x.que01 = y.visit_no ) ";
            sql_load = sql_load + Environment.NewLine + "  WHERE 1=1 ";
            //sql_load = sql_load + Environment.NewLine + "  AND  que01 = '" + s_que + "' ";
            //sql_load = sql_load + Environment.NewLine + "  AND  x.visit_date = '" + s_date + "' ";
            sql_load = sql_load + Environment.NewLine + "  AND (cer_id is not null or rownum<=1)";
            sql_load = sql_load + Environment.NewLine + "  AND  x.rm_no = '" + s_rm + "' ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_load, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);


            p_rm = dt.Rows[0]["rm_no"].ToString();
            p_que = dt.Rows[0]["que01"].ToString();
            p_date = dt.Rows[0]["visit_date"].ToString();
            p_cer_id = dt.Rows[0]["cer_id"].ToString();

            p_name = dt.Rows[0]["name"].ToString();
            p_nik = dt.Rows[0]["patient_no"].ToString();
            p_minggu = dt.Rows[0]["info02"].ToString();
            p_taksiran = dt.Rows[0]["info07"].ToString();

            p_tipe = dt.Rows[0]["cer_type"].ToString();
            p_no_surat = dt.Rows[0]["letter_no"].ToString();
            p_tgl_surat = dt.Rows[0]["letter_dt"].ToString();
            p_leave = dt.Rows[0]["cnt_leave"].ToString();
            p_bgn_date = dt.Rows[0]["info09"].ToString();
            p_end_date = dt.Rows[0]["info10"].ToString();
            p_work = dt.Rows[0]["bgn_work"].ToString();

            lHamName.Text = p_name;
            lHamNik.Text = p_nik;
            lHamMinggu.Text = p_minggu;
            lHamTaksiran.Text = p_taksiran;

            luHamTipe.Text = p_tipe;
            tHamNo.Text = p_no_surat;
            dHamDate.Text = p_tgl_surat;
            tHamLeave.Text = p_leave;
            lHamBgn.Text = p_bgn_date;
            lHamEnd.Text = p_end_date;
            dHamWork.Text = p_work;

            lHamRm.Text = p_rm;
            lHamQue.Text = p_que;
            lHamDate.Text = p_date;
            lHamID.Text = p_cer_id;

            luHamTipe.Properties.DataSource = listKetHamil;
            luHamTipe.Properties.ValueMember = "ketHamilCode";
            luHamTipe.Properties.DisplayMember = "ketHamilName";

            luHamTipe.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luHamTipe.Properties.DropDownRows = listKetHamil.Count;
            luHamTipe.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luHamTipe.Properties.AutoSearchColumnIndex = 1;
            luHamTipe.Properties.NullText = "Pilih Tipe Surat";

            if (p_no_surat == "")
            {
                btnHamPrint.Enabled = false;
                btnHamDel.Enabled = false;
            }
            else
            {
                btnHamPrint.Enabled = true;
                btnHamDel.Enabled = true;
            }

            if (dt.Rows.Count > 0)
            {
                dscutiHamil.Tables.Clear();
                dscutiHamil.Tables.Add(dt);
            }
            else
            {
                dscutiHamil.Tables.Clear();
            }

        }

        private void btnHamSave_Click(object sender, EventArgs e)
        {
            string sql_cnt = "";
            string ham_cnt = "", tmp_type = "";

            if (lHamRm.Text == "")
            {
                MessageBox.Show("Silahkan load data pasien");
            }
            else if (lHamMinggu.Text == "")
            {
                MessageBox.Show("Minggu harus diisi");
            }
            else if (lHamTaksiran.Text == "")
            {
                MessageBox.Show("Taksiran harus diisi");
            }
            else if (luHamTipe.Text == "Pilih Tipe Surat")
            {
                MessageBox.Show("Tipe surat harus diisi");
            }
            else if (tHamNo.Text == "")
            {
                MessageBox.Show("No Surat harus diisi");
            }
            else if (dHamDate.Text == "")
            {
                MessageBox.Show("Tanggal Surat harus diisi");
            }
            else if (tHamLeave.Text == "")
            {
                MessageBox.Show("Lama cuti harus diisi");
            }
            else if (lHamBgn.Text == "")
            {
                MessageBox.Show("Mulai cuti harus diisi");
            }
            else if (lHamEnd.Text == "")
            {
                MessageBox.Show("Selesai cuti harus diisi");
            }
            else if (dHamWork.Text == "")
            {
                MessageBox.Show("Tanggal masuk harus diisi");
            }
            else
            {
                sql_cnt = " select count(0) cnt from cs_birth_certificate where  rm_no = '" + lHamRm.Text + "' ";
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);
                ham_cnt = dt.Rows[0]["cnt"].ToString();
                if (Convert.ToInt32(ham_cnt) > 0)
                {
                    // update data

                    string sql_update = "";

                    if (luHamTipe.Text == "Melahirkan")
                    {
                        tmp_type = "B";
                    }
                    else
                    {
                        tmp_type = "M";
                    }

                    sql_update = " update cs_birth_certificate set cer_type = '" + tmp_type + "', letter_no = '" + tHamNo.Text + "', letter_dt = to_date('" + dHamDate.Text + "','yyyy-mm-dd'), cnt_leave = '" + tHamLeave.Text + "', bgn_work = to_date('" + dHamWork.Text + "','yyyy-mm-dd'), upd_emp='" + DB.vUserId + "', upd_date = sysdate " +
                                 " where cer_id='" + lHamID.Text + "'  ";

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

                    if (luHamTipe.Text == "Melahirkan")
                    {
                        tmp_type = "B";
                    }
                    else
                    {
                        tmp_type = "M";
                    }

                    sql_insert = " insert into cs_birth_certificate (cer_id, rm_no, insp_date, letter_dt, letter_no, birth_week, birth_date, cnt_leave, bgn_date, end_date, bgn_work, cer_type, visit_no, ins_date, ins_emp)  " +
                                 " values (cs_birth_cer_seq.nextval, '" + lHamRm.Text + "', to_date('" + lHamDate.Text + "','yyyy-mm-dd'), to_date('" + dHamDate.Text + "','yyyy-mm-dd'), '" + tHamNo.Text + "', '" + lHamMinggu.Text + "', to_date('" + lHamTaksiran.Text + "','yyyy-mm-dd'),'" + tHamLeave.Text + "', to_date('" + lHamBgn.Text + "','yyyy-mm-dd'), to_date('" + lHamEnd.Text + "','yyyy-mm-dd'), to_date('" + dHamWork.Text + "','yyyy-mm-dd'), '" + tmp_type + "', '" + lHamQue.Text + "', sysdate, '" + DB.vUserId + "')  ";

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

        private void btnHamDel_Click(object sender, EventArgs e)
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

                sql_delete = " delete from cs_birth_certificate where cer_id = '" + lHamID.Text + "'  ";

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

        private void btnHamPrint_Click(object sender, EventArgs e)
        {
            ReportCutiHamil report = new ReportCutiHamil(dscutiHamil);
            report.ShowPreviewDialog();
        }

        private void loadMCU_Click(object sender, EventArgs e)
        {
            string sql_load = "", s_rm = "", s_nik = "";
            string p_nik = "", p_name = "", p_dept = "", p_period = "", p_mcu_no = "", p_paket = "";

            s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            //s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();

            sql_load = " select a.patient_no, b.name, null line, c.periode, c.mcu_no, c.paket " +
                       " from cs_patient a " +
                       " join cs_patient_info b on a.patient_no = b.patient_no " +
                       " left join cs_mcu c on a.patient_no = c.patient_no " +
                       " where a.status = 'A' " +
                       " and a.group_patient <> 'COMM' " +
                       " and a.patient_no = '" + s_nik + "' " +
                       " and a.rm_no = '" + s_rm + "' ";


            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_load, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            p_nik = dt.Rows[0]["patient_no"].ToString();
            p_name = dt.Rows[0]["name"].ToString();
            p_dept = dt.Rows[0]["line"].ToString();
            //p_period = dt.Rows[0]["periode"].ToString();
            p_mcu_no = dt.Rows[0]["mcu_no"].ToString();
            p_paket = dt.Rows[0]["paket"].ToString();

            lMcuNik.Text = p_nik;
            lMcuNama.Text = p_name;
            lMcuDept.Text = p_dept;

            //cMcuPeriode.Text = p_period;
            lMcuNo.Text = p_mcu_no;
            lMcuPaket.Text = p_paket;

            LoadDataMCU();
        }

        private void LoadDataMCU()
        {
            string sql_mcu_load = "", s_rm = "", s_nik = "", sql_mcu_load2 = "", sql_mcu_load3 = "", sql_mcu_load4 = "", sql_mcu_load5 = "";

            s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();

            sql_mcu_load = " select periode, ksmfisik, replace(kesimp, chr(10), ' ') kesimp, c.status " +
                           " from cs_patient a " +
                           " left join cs_mcu c on a.patient_no = c.patient_no " +
                           " where a.status = 'A' " +
                           " and a.group_patient <> 'COMM' " +
                           " and c.periode like '%" + cMcuPeriode.Text + "%' " +
                           " and a.patient_no = '" + s_nik + "' " +
                           " and a.rm_no = '" + s_rm + "' " +
                           " order by periode desc ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_mcu_load, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            gridControl8.DataSource = null;
            gridView8.Columns.Clear();
            gridControl8.DataSource = dt;

            gridView8.OptionsView.ColumnAutoWidth = true;
            gridView8.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView8.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView8.IndicatorWidth = 30;
            gridView8.OptionsBehavior.Editable = false;
            gridView8.BestFitColumns();

            gridView8.Columns[0].Caption = "Periode";
            gridView8.Columns[1].Caption = "Ksm Fisik";
            gridView8.Columns[2].Caption = "Kesimpulan";
            gridView8.Columns[3].Caption = "Status";

            RepositoryItemMemoEdit ksmfisik = new RepositoryItemMemoEdit();
            gridControl8.RepositoryItems.Add(ksmfisik);
            gridView8.Columns[1].ColumnEdit = ksmfisik;

            RepositoryItemMemoEdit kesimp = new RepositoryItemMemoEdit();
            gridControl8.RepositoryItems.Add(kesimp);
            gridView8.Columns[2].ColumnEdit = kesimp;

            sql_mcu_load2 = " select periode, riwayat, tb, bb, bmi, tensi " +
                           " from cs_patient a " +
                           " left join cs_mcu c on a.patient_no = c.patient_no " +
                           " where a.status = 'A' " +
                           " and a.group_patient <> 'COMM' " +
                           " and c.periode like '%" + cMcuPeriode.Text + "%' " +
                           " and a.patient_no = '" + s_nik + "' " +
                           " and a.rm_no = '" + s_rm + "' " +
                           " order by periode desc ";

            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_mcu_load2, oraConnect2);
            DataTable dt2 = new DataTable();
            adOra2.Fill(dt2);

            gridControl9.DataSource = null;
            gridView9.Columns.Clear();
            gridControl9.DataSource = dt2;

            gridView9.OptionsView.ColumnAutoWidth = true;
            gridView9.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView9.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView9.IndicatorWidth = 30;
            gridView9.OptionsBehavior.Editable = false;
            gridView9.BestFitColumns();

            gridView9.Columns[0].Caption = "Periode";
            gridView9.Columns[1].Caption = "Riwayat";
            gridView9.Columns[2].Caption = "TB";
            gridView9.Columns[3].Caption = "BB";
            gridView9.Columns[4].Caption = "BMI";
            gridView9.Columns[5].Caption = "Tensi";

            sql_mcu_load3 = " select periode, visuskn, visuskr, butawarna " +
                           " from cs_patient a " +
                           " left join cs_mcu c on a.patient_no = c.patient_no " +
                           " where a.status = 'A' " +
                           " and a.group_patient <> 'COMM' " +
                           " and c.periode like '%" + cMcuPeriode.Text + "%' " +
                           " and a.patient_no = '" + s_nik + "' " +
                           " and a.rm_no = '" + s_rm + "' " +
                           " order by periode desc ";

            OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra3 = new OleDbDataAdapter(sql_mcu_load3, oraConnect3);
            DataTable dt3 = new DataTable();
            adOra3.Fill(dt3);

            gridControl10.DataSource = null;
            gridView10.Columns.Clear();
            gridControl10.DataSource = dt3;

            gridView10.OptionsView.ColumnAutoWidth = true;
            gridView10.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView10.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView10.IndicatorWidth = 30;
            gridView10.OptionsBehavior.Editable = false;
            gridView10.BestFitColumns();

            gridView10.Columns[0].Caption = "Periode";
            gridView10.Columns[1].Caption = "Visus Kanan";
            gridView10.Columns[2].Caption = "Visus Kiri";
            gridView10.Columns[3].Caption = "Buta Warna";

            sql_mcu_load4 = " select periode, labsmua, labhema, labkimia, laburine" +
                           " from cs_patient a " +
                           " left join cs_mcu c on a.patient_no = c.patient_no " +
                           " where a.status = 'A' " +
                           " and a.group_patient <> 'COMM' " +
                           " and c.periode like '%" + cMcuPeriode.Text + "%' " +
                           " and a.patient_no = '" + s_nik + "' " +
                           " and a.rm_no = '" + s_rm + "' " +
                           " order by periode desc ";

            OleDbConnection oraConnect4 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra4 = new OleDbDataAdapter(sql_mcu_load4, oraConnect4);
            DataTable dt4 = new DataTable();
            adOra4.Fill(dt4);

            gridControl11.DataSource = null;
            gridView11.Columns.Clear();
            gridControl11.DataSource = dt4;

            gridView11.OptionsView.ColumnAutoWidth = true;
            gridView11.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView11.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView11.IndicatorWidth = 30;
            gridView11.OptionsBehavior.Editable = false;
            gridView11.BestFitColumns();

            gridView11.Columns[0].Caption = "Periode";
            gridView11.Columns[1].Caption = "Lab Semua";
            gridView11.Columns[2].Caption = "Lab Hema";
            gridView11.Columns[3].Caption = "Lab Kimia";
            gridView11.Columns[4].Caption = "Lab Urin";

            sql_mcu_load5 = " select periode, rontgen, jantung, audio, spiro " +
                           " from cs_patient a " +
                           " left join cs_mcu c on a.patient_no = c.patient_no " +
                           " where a.status = 'A' " +
                           " and a.group_patient <> 'COMM' " +
                           " and c.periode like '%" + cMcuPeriode.Text + "%' " +
                           " and a.patient_no = '" + s_nik + "' " +
                           " and a.rm_no = '" + s_rm + "' " +
                           " order by periode desc ";

            OleDbConnection oraConnect5 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra5 = new OleDbDataAdapter(sql_mcu_load5, oraConnect5);
            DataTable dt5 = new DataTable();
            adOra5.Fill(dt5);

            gridControl12.DataSource = null;
            gridView12.Columns.Clear();
            gridControl12.DataSource = dt5;

            gridView12.OptionsView.ColumnAutoWidth = true;
            gridView12.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView12.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView12.IndicatorWidth = 30;
            gridView12.OptionsBehavior.Editable = false;
            gridView12.BestFitColumns();

            gridView12.Columns[0].Caption = "Periode";
            gridView12.Columns[1].Caption = "Rontgen";
            gridView12.Columns[2].Caption = "Jantung";
            gridView12.Columns[3].Caption = "Audio";
            gridView12.Columns[4].Caption = "Spiro";
        }

        private void cMcuPeriode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                LoadDataMCU();
            }

        }

        private void gridView3_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.Caption == "Tensi" || e.Column.Caption == "Nadi" || e.Column.Caption == "Suhu" || e.Column.Caption == "BB (Kg)" || e.Column.Caption == "TB (Cm)" || e.Column.Caption == "Alergi" || e.Column.Caption == "Keluhan" || e.Column.Caption == "Kehamilan")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void gridView4_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
             
            if (e.Column.Caption == "Diagnosa" || e.Column.Caption == "Tipe" || e.Column.Caption == "Remark")
  
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
             
        }

        private void btnDoInsp_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount < 1) return;


            string s_rm = gridView1.GetRowCellDisplayText(gridView1.FocusedRowHandle, gridView1.Columns[10]);
            string s_tatus = gridView1.GetRowCellDisplayText(gridView1.FocusedRowHandle, gridView1.Columns[7]);
            string s_nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            string s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            string s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            string sql_upd = "";

            if (s_tatus == "First Inspection")
            {
                sql_upd = "";
                sql_upd = sql_upd + " update cs_visit";
                sql_upd = sql_upd + " set status = 'INS', time_reservation = sysdate ";
                sql_upd = sql_upd + " where patient_no = '" + s_nik + "' and to_char(visit_date, 'yyyy-mm-dd') = '" + s_date + "' and que01 = '" + s_que + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_upd, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();
                    //gridControl3.DataSource = null;
                    //gridControl4.DataSource = null;
                    //LoadDataPasien();
                    //MessageBox.Show("Query Exec : " + sql_update);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }

            if (s_rm == "")
            {
                btnAddAnam.Enabled = false;
                btnSaveAnam.Enabled = false;
                //btnCreate.Enabled = true;
            }
            else if (gridView3.RowCount <= 0)
            {
                btnAddAnam.Enabled = true;
                btnSaveAnam.Enabled = false;
                //btnCreate.Enabled = false;
            }
            else if (gridView3.RowCount > 0)
            {
                btnAddAnam.Enabled = false;
                btnSaveAnam.Enabled = true;
                btnCreate.Enabled = false;
            }

            if (gridView4.RowCount <= 0)
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
         
        private void dLetterEnd_TextChanged(object sender, EventArgs e)
        {
            string d_start = "", d_end = "", d_tmp = "";



            d_start = today.Replace("-", "");
            d_tmp = dLetterEnd.Text;
            d_end = d_tmp.Replace("-", "");

            if (Convert.ToInt32(d_end) < Convert.ToInt32(d_start)) 
            { 
                dLetterEnd.Text = today; 
            }
        }

        private void dLetterLimitEnd_TextChanged(object sender, EventArgs e)
        {
            string d_start = "", d_end = "", d_tmp = ""; 

            d_start = today.Replace("-", "");
            d_tmp = dLetterLimitEnd.Text;
            d_end = d_tmp.Replace("-", "");

            if (Convert.ToInt32(d_end) < Convert.ToInt32(d_start)) 
            { 
                dLetterLimitEnd.Text = today; 
            }
        }

        private void dRestEnd_TextChanged(object sender, EventArgs e)
        {
            string d_start = "", d_end = "", d_tmp = "";

            d_start = today.Replace("-", "");
            d_tmp = dRestEnd.Text;
            d_end = d_tmp.Replace("-", "");

            if (Convert.ToInt32(d_end) < Convert.ToInt32(d_start))
            { 
                dRestEnd.Text = today; 
            } 
        }

        private void dLetterControl_TextChanged(object sender, EventArgs e)
        {
            string d_start = "", d_end = "", d_tmp = "";

            d_start = today.Replace("-", "");
            d_tmp = dLetterControl.Text;
            d_end = d_tmp.Replace("-", "");

            if (Convert.ToInt32(d_end) < Convert.ToInt32(d_start))
            { 
                dLetterControl.Text = today; 
            } 
        }

        private void btnObsDel_Click(object sender, EventArgs e)
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

                command.CommandText = " delete from cs_observation where rm_no = '" + lObsRm.Text + "' and to_char(insp_date,'yyyy-mm-dd') = '" + lObsDate.Text + "' and visit_no = '" + lObsQue.Text + "'  ";
                command.ExecuteNonQuery();

                command.CommandText = " update cs_visit set status = 'MED', observation=null, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where empid = '" + lObsNik.Text + "' and to_char(visit_date,'yyyy-mm-dd') = '" + lObsDate.Text + "' and que01 = '" + lObsQue.Text + "' ";
                command.ExecuteNonQuery();

                trans.Commit();

                ObsList();
                int cap = 0, free = 0, cnt = 0;
                cap = Convert.ToInt32(luObsRoom.GetColumnValue("roomQty").ToString());
                cnt = gridView5.RowCount;
                free = cap - cnt;
                lObsCap.Text = luObsRoom.GetColumnValue("roomQty").ToString();
                lObsFre.Text = free.ToString();
                if (free == 0)
                {
                    btnObsAdd.Enabled = false;
                }
                else
                {
                    btnObsAdd.Enabled = true;
                }
                btnObsSave.Enabled = false;
                btnObsCls.Enabled = true;
                btnObsDel.Enabled = true;
                MessageBox.Show("Data Berhasil dicancel");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                MessageBox.Show("ERROR: " + ex.Message);
            }

            oraConnectTrans.Close();
        } 
        private void gridView5_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column.Caption == "Lama")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnCanDiag_Click(object sender, EventArgs e)
        {
            string stat = gridView4.GetRowCellDisplayText(gridView4.FocusedRowHandle, gridView4.Columns[0]);
            if (stat == "I" || stat == "")
            {
                gridView4.DeleteRow(gridView4.FocusedRowHandle);
            }
            
        }

        private void btnMedCan_Click(object sender, EventArgs e)
        {
            string stat = gridView6.GetRowCellDisplayText(gridView6.FocusedRowHandle, gridView6.Columns[3]);
            if (stat == "" || stat == "I")
            {
                gridView6.DeleteRow(gridView6.FocusedRowHandle);
            }
        }

        private void mActName_TextChanged(object sender, EventArgs e)
        {
            lRemainAct.Text = (200 - mActName.Text.Length).ToString();
            if (mActName.Text.Length > 200 || mActRemark.Text.Length > 200)
            {
                btnActSave.Enabled = false;
            }
            else
            {
                btnActSave.Enabled = true;
            }
        }

        private void mActRemark_TextChanged(object sender, EventArgs e)
        {
            lRemainAct2.Text = (200 - mActRemark.Text.Length).ToString();
            if (mActName.Text.Length > 200 || mActRemark.Text.Length > 200)
            {
                btnActSave.Enabled = false;
            }
            else
            {
                btnActSave.Enabled = true;
            }
        }



        private void tAdd05_TextChanged(object sender, EventArgs e)
        {
            string sql_cek = "", p_hpht = "", p_minggu = "";

            if (tAdd05.Text != "")
            {
                sql_cek = " select to_char(to_date('" + tAdd05.Text + "','yyyy-mm-dd') + 280,'yyyy-mm-dd') ddate, round((sysdate-(to_date('" + tAdd05.Text + "','yyyy-mm-dd') + 7))/7,0) dminggu from dual ";


                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cek, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                p_hpht = dt.Rows[0]["ddate"].ToString();
                p_minggu = dt.Rows[0]["dminggu"].ToString();
                tAdd07.Text = p_hpht;
                tAdd02.Text = p_minggu;
            }
        }

        private void btnNoReceipt_Click(object sender, EventArgs e)
        {
            string sql_close = "", s_nik = "", s_que = "", s_date = "";

            s_nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();

            sql_close = "";

            sql_close = sql_close + " update cs_visit" +
                                    " set  VISIT_REMARK ='NONE MEDICINE',";
            sql_close = sql_close + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
            sql_close = sql_close + " where patient_no = '" + s_nik + "' and que01 = '" + s_que + "' and  to_char(visit_date,'yyyy-mm-dd') = '" + s_date + "' and id_visit  = '" + idvisit + "' ";

            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbCommand cm = new OleDbCommand(sql_close, oraConnect);
                oraConnect.Open();
                cm.ExecuteNonQuery();
                oraConnect.Close();
                cm.Dispose();

                //MessageBox.Show("Query Exec : " + sql_update);
                //LoadDataPasien();
                //MessageBox.Show("Data Berhasil diupdate");
                labelControl221.Visible = true;
                labelControl221.Text = "Save Success";
                Blinking(labelControl221, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            } 
        }


        private void loadTind_Click(object sender, EventArgs e)
        {
            string sql_load = "";
            string s_rm = "", s_que = "", s_date = "", p_rm = "", p_que = "", p_date = "", p_name = "", p_anamnesa = "", p_diagnosa = "", p_tipe_pas = "", p_tipe_des = "";
            if (gridView1.RowCount < 1) return;

            if (idvisit.ToString().Equals(""))
            {
                MessageBox.Show("Silahkan Tentukan Pasien Terlebh Dahulu...!!!");
                return;
            }
            if (gridView1.FocusedRowHandle < 0)
                return;

            string s_tatus = gridView1.GetRowCellDisplayText(gridView1.FocusedRowHandle, gridView1.Columns[7]);
            s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();

            sql_load = sql_load + Environment.NewLine + "select a.patient_no, a.name,  ";
            sql_load = sql_load + Environment.NewLine + "c.rm_no, to_char(b.visit_date,'yyyy-mm-dd') visit_date, que01,  ";
            sql_load = sql_load + Environment.NewLine + "(select anamnesa ";
            sql_load = sql_load + Environment.NewLine + "from cs_anamnesa   ";
            sql_load = sql_load + Environment.NewLine + "where rm_no=c.rm_no   ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)  ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) anamnesa,  ";
            sql_load = sql_load + Environment.NewLine + "(select LISTAGG(initcap(item_name), ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa  ";
            sql_load = sql_load + Environment.NewLine + "from cs_diagnosa a  ";
            sql_load = sql_load + Environment.NewLine + "join cs_diagnosa_item b on (a.item_cd=b.item_cd) ";
            sql_load = sql_load + Environment.NewLine + "where b.status='A'  ";
            sql_load = sql_load + Environment.NewLine + "and rm_no=c.rm_no  ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)  ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) as diagnosa,  ";
            sql_load = sql_load + Environment.NewLine + "(select LISTAGG(initcap(med_name), ', ') WITHIN GROUP (ORDER BY med_name asc) resep ";
            sql_load = sql_load + Environment.NewLine + "from cs_receipt a   ";
            sql_load = sql_load + Environment.NewLine + "join cs_medicine b on (a.med_cd=b.med_cd)  ";
            sql_load = sql_load + Environment.NewLine + "where b.status='A'   ";
            sql_load = sql_load + Environment.NewLine + "and rm_no=c.rm_no   ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)  ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) as resep, type_patient, ";
            sql_load = sql_load + Environment.NewLine + "decode(type_patient, 'B','BPJS','A','Asuransi','Umum') as type_desc ,b.id_visit";
            sql_load = sql_load + Environment.NewLine + "from cs_patient_info a   ";
            sql_load = sql_load + Environment.NewLine + "join cs_visit b on (a.patient_no = b.patient_no)  ";
            sql_load = sql_load + Environment.NewLine + "join cs_patient c on (b.patient_no = c.patient_no)  ";
            sql_load = sql_load + Environment.NewLine + "where  to_char(b.visit_date, 'yyyy-mm-dd') = '" + s_date + "'   ";
            sql_load = sql_load + Environment.NewLine + "and c.status = 'A'  ";
            sql_load = sql_load + Environment.NewLine + "and b.que01 = '" + s_que + "'  ";
            sql_load = sql_load + Environment.NewLine + "and b.id_visit = '" + idvisit + "'   ";
            sql_load = sql_load + Environment.NewLine + "and c.rm_no = '" + s_rm + "' ";

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
                p_tipe_pas = dt.Rows[0]["type_patient"].ToString();
                p_tipe_des = dt.Rows[0]["type_desc"].ToString();
                //p_id_visit = dt.Rows[0]["id_visit"].ToString();
            }

            lTindRm.Text = p_rm;
            lTindQue.Text = p_que;
            lTindDate.Text = p_date;

            lTinName.Text = p_name;
            lTinAnam.Text = p_anamnesa;
            lTinDiag.Text = p_diagnosa;
            lTinTipe.Text = p_tipe_pas;
            lTinDesc.Text = p_tipe_des;


            LoadTind();
            LoadAddTind();

            if (s_tatus == "Completed" || s_tatus == "Payment" || s_tatus == "Medicine")
            {
                btnDelTindakan.Enabled = false;
                btnAddTindakan.Enabled = false;
                btnSaveTindakan.Enabled = false;
                btnAddTind.Enabled = false;
                btnDelTind.Enabled = false;
                btnSaveTind.Enabled = false;
            }
            else
            {
                btnDelTindakan.Enabled = true;
                btnAddTindakan.Enabled = true;
                btnSaveTindakan.Enabled = true;
                btnAddTind.Enabled = true;
                btnDelTind.Enabled = true;
                btnSaveTind.Enabled = true;
            }
        }

        private void LoadTind()
        {
            string sql_tind_load = "", s_rm = "", s_date = "", s_que = "";

            s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();


            sql_tind_load = sql_tind_load + Environment.NewLine + "select head_id, rm_no, patient_no, to_char(visit_date,'yyyy-mm-dd') vdate,  ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "visit_no, a.treat_type_id, b.treat_type_id, a.status, remarks, 'S' action, a.pay_status,  decode(insu_flag, 'B','BPJS','A','Asuransi','Umum')  insu_flag ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "from cs_treatment_head a ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "join cs_treatment_type b on (a.treat_type_id=b.treat_type_id) ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "where rm_no='" + s_rm + "' ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "and to_char(visit_date,'yyyy-mm-dd') = '" + s_date + "' ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "and visit_no='" + s_que + "' and id_visit =  '" + idvisit + "' ";

            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_tind_load, oraConnect2);
            DataTable dt2 = new DataTable();
            adOra2.Fill(dt2);

            gridControl13.DataSource = null;
            gridView13.Columns.Clear();
            gridControl13.DataSource = dt2;

            gridView13.OptionsView.ColumnAutoWidth = true;
            gridView13.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView13.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView13.IndicatorWidth = 30;
            //gridView13.OptionsBehavior.Editable = false;
            gridView13.BestFitColumns();

            gridView13.Columns[0].Caption = "ID";
            gridView13.Columns[1].Caption = "Rm No";
            gridView13.Columns[2].Caption = "Pasien No";
            gridView13.Columns[3].Caption = "Tanggal";
            gridView13.Columns[4].Caption = "Visit No";
            gridView13.Columns[5].Caption = "Kode Layanan";
            gridView13.Columns[6].Caption = "Nama Layanan";
            gridView13.Columns[7].Caption = "Status";
            gridView13.Columns[8].Caption = "Remark";
            gridView13.Columns[9].Caption = "Action";
            gridView13.Columns[10].Caption = "Status Bayar";
            gridView13.Columns[11].Caption = "Tipe Pasien";

            gridView13.Columns[5].Width = 60;
            gridView13.Columns[7].Width = 60;

            //gridView13.Columns[9].VisibleIndex = 6;

            gridView13.Columns[0].Visible = false;
            gridView13.Columns[1].Visible = false;
            gridView13.Columns[2].Visible = false;
            gridView13.Columns[3].Visible = false;
            gridView13.Columns[4].Visible = false;
            //gridView13.Columns[5].Visible = false;
            gridView13.Columns[9].Visible = false;
            gridView13.Columns[10].Visible = false;

            gridView13.Columns[5].OptionsColumn.ReadOnly = true;
            //gridView13.Columns[6].OptionsColumn.ReadOnly = true;
            gridView13.Columns[7].OptionsColumn.ReadOnly = true;


            gridView13.Columns[11].VisibleIndex = 2;

            RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
            glLaya.DataSource = listLaya;
            glLaya.ValueMember = "layananCode";
            glLaya.DisplayMember = "layananName";

            glLaya.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glLaya.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glLaya.ImmediatePopup = true;
            glLaya.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glLaya.NullText = "";
            gridView13.Columns[6].ColumnEdit = glLaya;

            RepositoryItemLookUpEdit statLookup = new RepositoryItemLookUpEdit();
            statLookup.DataSource = listLayanan;
            statLookup.ValueMember = "statCode";
            statLookup.DisplayMember = "statName";

            statLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            statLookup.DropDownRows = listLayanan.Count;
            statLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            statLookup.AutoSearchColumnIndex = 1;
            statLookup.NullText = "";
            gridView13.Columns[7].ColumnEdit = statLookup;

            btnAddTind.Enabled = true;

            if (gridView13.RowCount > 0)
            {
                btnDelTind.Enabled = true;
                btnAddTind.Enabled = false;
            }
            else
            {
                btnDelTind.Enabled = false;
                btnAddTind.Enabled = true;
            }
        }


        private void gridView13_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void btnAddTind_Click(object sender, EventArgs e)
        {
            gridView13.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView13.AddNewRow();
            gridView13.UpdateCurrentRow();
        }

        private void gridView13_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            //view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
            //gridView6.Columns[3].OptionsColumn.ReadOnly = false;
            view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[7], "OPN");
            view.SetRowCellValue(e.RowHandle, view.Columns[10], "OPN");
            view.SetRowCellValue(e.RowHandle, view.Columns[6], "TRT01");
            view.SetRowCellValue(e.RowHandle, view.Columns[11], lTinDesc.Text);
            //gridView1.Columns[8].OptionsColumn.AllowFocus = true;
            //view.UpdateCurrentRow();
            btnAddTind.Enabled = false;
        }
         
        private void btnSaveTind_Click(object sender, EventArgs e)
        {
            string date = "", que = "", rm_no = "", pasno = "", nama_laya = "", status = "", remark = "", action = "", stbyr = "", insu_flag = "", pid_visit = "", headid = "", policd = "";
            string sql_cnt = "", diag_cnt = "", sql_update = "";
            int stsimpan = 0;

            if (gridView13.DataRowCount < 1)
                return;

            date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            policd = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();
            pid_visit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[13]).ToString(); 

            for (int i = 0; i < gridView13.DataRowCount; i++)
            {
                nama_laya = gridView13.GetRowCellValue(i, gridView13.Columns[6]).ToString();
                status = gridView13.GetRowCellValue(i, gridView13.Columns[7]).ToString();
                remark = gridView13.GetRowCellValue(i, gridView13.Columns[8]).ToString();
                action = gridView13.GetRowCellValue(i, gridView13.Columns[9]).ToString();
                stbyr = gridView13.GetRowCellValue(i, gridView13.Columns[10]).ToString();
                insu_flag = gridView13.GetRowCellValue(i, gridView13.Columns[11]).ToString();

                if (nama_laya == "")
                {
                    MessageBox.Show("Nama Layanan harus diisi"); return;
                }
                else if (stbyr != "OPN")
                {
                    MessageBox.Show("Data tidak bisa ditambah"); return;
                }
                else
                {
                    if (action == "I")
                    {
                        sql_cnt = " select count(0) cnt, max(head_id) headid from KLINIK.cs_treatment_head where to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and visit_no = '" + que + "' and rm_no = '" + rm_no + "' " + " and status = 'OPN' ";
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                        DataTable dt = new DataTable();
                        adOra.Fill(dt);
                        diag_cnt = dt.Rows[0]["cnt"].ToString();
                        if (Convert.ToInt32(diag_cnt) > 0)
                        {
                            headid = dt.Rows[0]["headid"].ToString();
                            OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                            OleDbCommand command = new OleDbCommand();
                            OleDbTransaction trans = null;

                            command.Connection = oraConnectTrans;
                            oraConnectTrans.Open();

                            try
                            {
                                string sql_seq2 = "", seq_val2 = "", sql_tmp = "", sql_seq = "", seq_val = "";

                                trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                                command.Connection = oraConnectTrans;
                                command.Transaction = trans;
                                //DB.vUserId = "1";

                                //if (nama_laya.ToString().Equals("TRT01"))
                                //{
                                //    command.CommandText = " update KLINIK.cs_visit set status = 'MED', time_inspection=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + pasno + "' and ID_VISIT =" + pid_visit + " "; // and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                                //    command.ExecuteNonQuery();
                                //}
                                //else
                                //{
                                //    sql_seq2 = " select CS_INPATIENT_SEQ.nextval seq from dual ";
                                //    OleDbConnection oraConnects2 = ConnOra.Create_Connect_Ora();
                                //    OleDbDataAdapter adOras2 = new OleDbDataAdapter(sql_seq2, oraConnects2);
                                //    DataTable dts2 = new DataTable();
                                //    adOras2.Fill(dts2);
                                //    seq_val2 = dts2.Rows[0]["seq"].ToString();

                                //    sql_seq = " select CS_TREATMENT_DETAIL_SEQ.nextval seq from dual ";
                                //    OleDbConnection oraConnects = ConnOra.Create_Connect_Ora();
                                //    OleDbDataAdapter adOras = new OleDbDataAdapter(sql_seq, oraConnects);
                                //    DataTable dts = new DataTable();
                                //    adOras.Fill(dts);
                                //    seq_val = dts.Rows[0]["seq"].ToString();


                                //    command.CommandText = " insert into KLINIK.cs_visit_his select a.*,sysdate, '" + DB.vUserId + "' from KLINIK.cs_visit a where ID_VISIT =  '" + pid_visit + "' ";
                                //    command.ExecuteNonQuery();

                                //    command.CommandText = " update KLINIK.cs_visit set POLI_CD = 'POL0004', status = 'INP', inpatient_id = '" + seq_val2 + "' , time_inspection=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + pasno + "' and ID_VISIT =  '" + pid_visit + "'  ";
                                //    command.ExecuteNonQuery();

                                //    command.CommandText = " insert into cs_inpatient (inpatient_id, rm_no,  reg_date, status,   date_in,    ins_date, ins_emp) values ('" + seq_val2 + "', '" + rm_no + "', to_date('" + date.ToString().Substring(0, 10) + "','yyyy-mm-dd'), '" + status + "',   to_date('" + date.ToString().Substring(0, 10) + "','yyyy-mm-dd'),   sysdate, '" + DB.vUserId + "') ";
                                //    command.ExecuteNonQuery();

                                //    //command.CommandText = " insert into KLINIK.cs_treatment_detail  (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME) values ( '" + seq_val + "', '" + head + "', '" + nama_laya + "', to_date('" + ldate.ToString().Substring(0, 10) + "', 'yyyy-mm-dd'), " + qty + ", " + price + ", " + price + ", '" + remarks + "', sysdate, '" + DB.vUserId + "', '" + ljam + "', 'gvMedisPeriksa') ";
                                //    //command.ExecuteNonQuery();

                                //    //command.CommandText = " insert into KLINIK.cs_action (act_id, rm_no, insp_date, visit_dt, visit_no, detail_id, ins_date, ins_emp) values ( CS_ACTION_SEQ.nextval, '" + rm_no + "', to_date('" + date.ToString().Substring(0, 10) + "', 'yyyy-mm-dd'), to_date('" + date.ToString().Substring(0, 10) + "', 'yyyy-mm-dd'), '" + que + "', '" + seq_val + "', sysdate, '" + DB.vUserId + "') ";
                                //    //command.ExecuteNonQuery();  
                                //}

                                sql_tmp = " ";
                                sql_tmp = sql_tmp + "insert into KLINIK.cs_treatment_detail ";
                                sql_tmp = sql_tmp + "select CS_TREATMENT_DETAIL_SEQ.nextval det_id, " + headid + " head_id,  b.treat_item_id, to_date('" + date.ToString().Substring(0, 10) + "', 'yyyy-mm-dd') visit_date, ";
                                sql_tmp = sql_tmp + "     1 treat_qty, 'Initial' remark, sysdate ins_date, '" + DB.vUserId + "' ins_emp, ";
                                sql_tmp = sql_tmp + "  null upd_date, null upd_emp, b.treat_item_price, b.treat_item_price total_price, TO_CHAR(sysdate,'HH24:MI') jam, 'gridView13' GRID_NAME, 0 ID_DOKTER, null att1, null att2, 'Y' F_ACTIVE ";
                                sql_tmp = sql_tmp + "  from KLINIK.cs_treatment_type a ";
                                sql_tmp = sql_tmp + "  join KLINIK.cs_treatment_item b on (a.treat_type_id=b.treat_type_id) ";
                                sql_tmp = sql_tmp + "  join KLINIK.cs_treatment_group c on (b.treat_group_id=c.treat_group_id) ";
                                sql_tmp = sql_tmp + " where 1=1";
                                sql_tmp = sql_tmp + "   and default_st='Y' ";
                                if (!nama_laya.ToString().Equals("TRT01"))
                                    sql_tmp = sql_tmp + "and a.treat_type_id <> 'TRT01' ";
                                else
                                    sql_tmp = sql_tmp + "and a.treat_type_id = 'TRT01' ";
                                sql_tmp = sql_tmp + "and b.treat_group_id = 'TRG07' and b.F_STATUS ='" + insu_flag + "'";

                                command.CommandText = sql_tmp;
                                command.ExecuteNonQuery();

                                trans.Commit();
                                //MessageBox.Show(sql_insert);
                                //MessageBox.Show("Query Exec : " + sql_insert);
                                //MessageBox.Show("Data Berhasil disimpan.");
                                stsimpan = 1;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                MessageBox.Show("ERROR: " + ex.Message);
                            }

                            oraConnectTrans.Close();
                        }
                        else 
                        {
                            string sql_seq = "", seq_val = "", sql_tmp = "";
                            sql_seq = " select CS_TREATMENT_HEAD_SEQ.nextval seq from dual ";
                            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_seq, oraConnect2);
                            DataTable dt2 = new DataTable();
                            adOra2.Fill(dt2);
                            seq_val = dt2.Rows[0]["seq"].ToString();

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
                                //DB.vUserId = "1";
                                if  (insu_flag.ToString().Equals("BPJS"))
                                    insu_flag = "B";
                                else if (insu_flag.ToString().Equals("Asuransi"))
                                    insu_flag = "A";
                                else 
                                    insu_flag = "U";
                                command.CommandText = " insert into KLINIK.cs_treatment_head (head_id, rm_no, patient_no, visit_date, visit_no, treat_type_id, status, remarks, pay_status, insu_flag, ins_date, ins_emp,ID_VISIT) values ('" + seq_val + "', '" + rm_no + "', '" + pasno + "', to_date('" + date + "', 'yyyy-mm-dd'), '" + que + "', '" + nama_laya + "', 'OPN', '" + insu_flag + "', 'OPN', '" + insu_flag + "', sysdate, '" + DB.vUserId + "', '" + pid_visit + "') ";
                                command.ExecuteNonQuery();

                                //if (nama_laya.ToString().Equals("TRT01"))
                                //{
                                //    command.CommandText = " update KLINIK.cs_visit set status = 'MED', time_inspection=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + pasno + "' and ID_VISIT =" + pid_visit + " "; // and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                                //    command.ExecuteNonQuery();
                                //}
                                //else
                                //{
                                //    string sql_seq2 = "", seq_val2 = "";
                                //    sql_seq2 = " select CS_INPATIENT_SEQ.nextval seq from dual ";
                                //    OleDbConnection oraConnects2 = ConnOra.Create_Connect_Ora();
                                //    OleDbDataAdapter adOras2 = new OleDbDataAdapter(sql_seq2, oraConnects2);
                                //    DataTable dts2 = new DataTable();
                                //    adOras2.Fill(dts2);
                                //    seq_val2 = dts2.Rows[0]["seq"].ToString();


                                //    command.CommandText = " insert into KLINIK.cs_visit_his select a.*,sysdate, '" + DB.vUserId + "' from KLINIK.cs_visit a where ID_VISIT =  '" + pid_visit + "' ";
                                //    command.ExecuteNonQuery();

                                //    command.CommandText = " update KLINIK.cs_visit set POLI_CD = 'POL0004', status = 'INP', inpatient_id = '" + seq_val2 + "' , time_inspection=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + pasno + "' and ID_VISIT =  '" + pid_visit + "'  ";
                                //    command.ExecuteNonQuery();

                                //    command.CommandText = " insert into cs_inpatient (inpatient_id, rm_no,  reg_date, status,   date_in,    ins_date, ins_emp) values ('" + seq_val2 + "', '" + rm_no + "', to_date('" + date + "','yyyy-mm-dd'), '" + status + "',   to_date('" + date + "','yyyy-mm-dd'),   sysdate, '" + DB.vUserId + "') ";
                                //    command.ExecuteNonQuery();
                                //}


                                sql_tmp = "";
                                sql_tmp = sql_tmp + "insert into KLINIK.cs_treatment_detail ";
                                sql_tmp = sql_tmp + "select CS_TREATMENT_DETAIL_SEQ.nextval det_id, " + seq_val + " head_id,  b.treat_item_id, to_date('" + date + "', 'yyyy-mm-dd') visit_date, ";
                                sql_tmp = sql_tmp + "1 treat_qty, 'Initial' remark, sysdate ins_date, '" + DB.vUserId + "' ins_emp, ";
                                sql_tmp = sql_tmp + "null upd_date, null upd_emp, b.treat_item_price, b.treat_item_price total_price, TO_CHAR(sysdate,'HH24:MI') jam, 'gridView13' GRID_NAME, 0 ID_DOKTER, null att1, null att2 , 'Y' F_ACTIVE ";
                                sql_tmp = sql_tmp + "from KLINIK.cs_treatment_type a ";
                                sql_tmp = sql_tmp + "join KLINIK.cs_treatment_item b on (a.treat_type_id=b.treat_type_id) ";
                                sql_tmp = sql_tmp + "join KLINIK.cs_treatment_group c on (b.treat_group_id=c.treat_group_id) ";
                                sql_tmp = sql_tmp + "where 1=1";
                                sql_tmp = sql_tmp + "and default_st='Y' ";
                                if (!nama_laya.ToString().Equals("TRT01"))
                                    sql_tmp = sql_tmp + "and a.treat_type_id <> 'TRT01' ";
                                else
                                    sql_tmp = sql_tmp + "and a.treat_type_id = 'TRT01' ";
                                sql_tmp = sql_tmp + "and b.treat_group_id =  'TRG07' AND B.F_STATUS = '" + insu_flag + "'";

                                command.CommandText = sql_tmp;
                                command.ExecuteNonQuery();

                                trans.Commit();
                                //MessageBox.Show(sql_insert);
                                //MessageBox.Show("Query Exec : " + sql_insert);
                                //MessageBox.Show("Data Berhasil disimpan.");
                                stsimpan = 1;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                MessageBox.Show("ERROR: " + ex.Message);
                            }

                            oraConnectTrans.Close();
                        }
                    }
                    else if (action == "U" || action == "S")
                    {
                        sql_update = "";

                        

                        if (insu_flag.ToString().Equals("BPJS"))
                            insu_flag = "B";
                        else
                            insu_flag = "U";

                        if (insu_flag != lTinTipe.Text)
                        {
                            MessageBox.Show("Data Tipe Pasien pada menu reservasi dan tagihan tidak sama");
                            LoadTind();
                            LoadAddTind();
                            return;
                        }

                        sql_update = sql_update + " update KLINIK.cs_treatment_head" +
                                                  " set remarks = '" + remark + "', insu_flag= '" + insu_flag + "', ";
                        sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                        sql_update = sql_update + " where rm_no = '" + rm_no + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and visit_no = '" + que + "' and patient_no = '" + pasno + "' ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            stsimpan = 2;

                            sql_cnt = " select count(0) cnt, max(head_id) headid from KLINIK.cs_treatment_head where to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and visit_no = '" + que + "' and rm_no = '" + rm_no + "' " + " and status = 'OPN' ";
                            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect2);
                            DataTable dt = new DataTable();
                            adOra.Fill(dt);
                            diag_cnt = dt.Rows[0]["cnt"].ToString();
                            if (Convert.ToInt32(diag_cnt) > 0)
                            {
                                headid = dt.Rows[0]["headid"].ToString();
                                OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                                OleDbCommand command = new OleDbCommand();
                                OleDbTransaction trans = null;

                                command.Connection = oraConnectTrans;
                                oraConnectTrans.Open();

                                try
                                {
                                    string sql_seq2 = "", seq_val2 = "", sql_tmp = "", sql_seq = "", seq_val = "";

                                    trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                                    command.Connection = oraConnectTrans;
                                    command.Transaction = trans;
                                    //DB.vUserId = "1";

                                    //if (nama_laya.ToString().Equals("TRT01"))
                                    //{
                                    //    command.CommandText = " update KLINIK.cs_visit set status = 'MED', time_inspection=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + pasno + "' and ID_VISIT =" + pid_visit + " "; // and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                                    //    command.ExecuteNonQuery();
                                    //}
                                    //else
                                    //{
                                    //    sql_seq2 = " select CS_INPATIENT_SEQ.nextval seq from dual ";
                                    //    OleDbConnection oraConnects2 = ConnOra.Create_Connect_Ora();
                                    //    OleDbDataAdapter adOras2 = new OleDbDataAdapter(sql_seq2, oraConnects2);
                                    //    DataTable dts2 = new DataTable();
                                    //    adOras2.Fill(dts2);
                                    //    seq_val2 = dts2.Rows[0]["seq"].ToString();

                                    //    sql_seq = " select CS_TREATMENT_DETAIL_SEQ.nextval seq from dual ";
                                    //    OleDbConnection oraConnects = ConnOra.Create_Connect_Ora();
                                    //    OleDbDataAdapter adOras = new OleDbDataAdapter(sql_seq, oraConnects);
                                    //    DataTable dts = new DataTable();
                                    //    adOras.Fill(dts);
                                    //    seq_val = dts.Rows[0]["seq"].ToString();


                                    //    command.CommandText = " insert into KLINIK.cs_visit_his select a.*,sysdate, '" + DB.vUserId + "' from KLINIK.cs_visit a where ID_VISIT =  '" + pid_visit + "' ";
                                    //    command.ExecuteNonQuery();

                                    //    command.CommandText = " update KLINIK.cs_visit set POLI_CD = 'POL0004', status = 'INP', inpatient_id = '" + seq_val2 + "' , time_inspection=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + pasno + "' and ID_VISIT =  '" + pid_visit + "'  ";
                                    //    command.ExecuteNonQuery();

                                    //    command.CommandText = " insert into cs_inpatient (inpatient_id, rm_no,  reg_date, status,   date_in,    ins_date, ins_emp) values ('" + seq_val2 + "', '" + rm_no + "', to_date('" + date.ToString().Substring(0, 10) + "','yyyy-mm-dd'), '" + status + "',   to_date('" + date.ToString().Substring(0, 10) + "','yyyy-mm-dd'),   sysdate, '" + DB.vUserId + "') ";
                                    //    command.ExecuteNonQuery();

                                    //    //command.CommandText = " insert into KLINIK.cs_treatment_detail  (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME) values ( '" + seq_val + "', '" + head + "', '" + nama_laya + "', to_date('" + ldate.ToString().Substring(0, 10) + "', 'yyyy-mm-dd'), " + qty + ", " + price + ", " + price + ", '" + remarks + "', sysdate, '" + DB.vUserId + "', '" + ljam + "', 'gvMedisPeriksa') ";
                                    //    //command.ExecuteNonQuery();

                                    //    //command.CommandText = " insert into KLINIK.cs_action (act_id, rm_no, insp_date, visit_dt, visit_no, detail_id, ins_date, ins_emp) values ( CS_ACTION_SEQ.nextval, '" + rm_no + "', to_date('" + date.ToString().Substring(0, 10) + "', 'yyyy-mm-dd'), to_date('" + date.ToString().Substring(0, 10) + "', 'yyyy-mm-dd'), '" + que + "', '" + seq_val + "', sysdate, '" + DB.vUserId + "') ";
                                    //    //command.ExecuteNonQuery();  
                                    //}

                                    sql_tmp = " ";
                                    sql_tmp = sql_tmp + "insert into KLINIK.cs_treatment_detail ";
                                    sql_tmp = sql_tmp + "select CS_TREATMENT_DETAIL_SEQ.nextval det_id, " + headid + " head_id,  b.treat_item_id, to_date('" + date.ToString().Substring(0, 10) + "', 'yyyy-mm-dd') visit_date, ";
                                    sql_tmp = sql_tmp + "     1 treat_qty, 'Initial' remark, sysdate ins_date, '" + DB.vUserId + "' ins_emp, ";
                                    sql_tmp = sql_tmp + "  null upd_date, null upd_emp, b.treat_item_price, b.treat_item_price total_price, TO_CHAR(sysdate,'HH24:MI') jam, 'gridView13' GRID_NAME, 0 ID_DOKTER, '" + insu_flag + "' att1, null att2, 'Y' F_ACTIVE ";
                                    sql_tmp = sql_tmp + "  from KLINIK.cs_treatment_type a ";
                                    sql_tmp = sql_tmp + "  join KLINIK.cs_treatment_item b on (a.treat_type_id=b.treat_type_id) ";
                                    sql_tmp = sql_tmp + "  join KLINIK.cs_treatment_group c on (b.treat_group_id=c.treat_group_id) ";
                                    sql_tmp = sql_tmp + " where 1=1";
                                    sql_tmp = sql_tmp + "   and default_st='Y' and b.treat_item_id not in( select TREAT_ITEM_ID from KLINIK.cs_treatment_detail where HEAD_ID = " + headid + " )  ";
                                    if (!nama_laya.ToString().Equals("TRT01"))
                                        sql_tmp = sql_tmp + "and a.treat_type_id <> 'TRT01' ";
                                    else
                                        sql_tmp = sql_tmp + "and a.treat_type_id = 'TRT01' ";
                                    sql_tmp = sql_tmp + "and b.treat_group_id = 'TRG07' AND B.F_STATUS = '" + insu_flag + "' ";

                                    command.CommandText = sql_tmp;
                                    command.ExecuteNonQuery();

                                    trans.Commit();
                                    stsimpan = 1;

                                    //MessageBox.Show(sql_insert);
                                    //MessageBox.Show("Query Exec : " + sql_insert);
                                    //MessageBox.Show("Data Berhasil disimpan.");
                                }
                                catch (Exception ex)
                                {
                                    trans.Rollback();
                                    MessageBox.Show("ERROR: " + ex.Message);
                                }

                                oraConnectTrans.Close();

                            }

                            //MessageBox.Show("Query Exec : " + sql_update);

                            MessageBox.Show("Data Berhasil diupdate");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                    LoadTind();
                    LoadAddTind();
                    simpleButton2.Enabled = true;
                }
            }
            if (stsimpan == 1)
                MessageBox.Show("Data Berhasil disimpan.");
            else if (stsimpan == 2)
                MessageBox.Show("Data Berhasil diupdate");
        }

        private void btnDelTind_Click(object sender, EventArgs e)
        {
            if (gridView13.DataRowCount < 1)
                return;

            if (MessageBox.Show("Anda yakin akan menghapus data?",
                      "Message",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string sql_delete = "", id = "", pasno = "", date = "", que = "", payst = "";

                id = gridView13.GetRowCellValue(gridView13.FocusedRowHandle, gridView13.Columns[0]).ToString();
                date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
                que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                payst = gridView13.GetRowCellValue(gridView13.FocusedRowHandle, gridView13.Columns[10]).ToString();

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

                        command.CommandText = " delete cs_treatment_head where head_id = '" + id + "'  ";
                        command.ExecuteNonQuery();

                        command.CommandText = " delete cs_treatment_detail where head_id = '" + id + "' ";
                        command.ExecuteNonQuery();

                        command.CommandText = " update cs_visit set status = 'INS', time_inspection=null, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + pasno + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                        command.ExecuteNonQuery();

                        trans.Commit();
                        //MessageBox.Show(sql_insert);
                        //MessageBox.Show("Query Exec : " + sql_insert);
                        gridView13.DeleteRow(gridView13.FocusedRowHandle);
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
                LoadTind();
                LoadAddTind();
            }
        }

        private void gridView13_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveTind.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Nama Layanan" || e.Column.Caption == "Status" || e.Column.Caption == "Remark" || e.Column.Caption == "Tipe Pasien")
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

        private void gridView13_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama Layanan" || e.Column.Caption == "Remark")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void LoadAddTind()
        {
            string sql_tind_load = "", s_rm = "", s_date = "", s_que = "";

            s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();

            sql_tind_load = sql_tind_load + Environment.NewLine + "select b.detail_id, c.treat_group_id, b.treat_item_id, b.treat_qty, b.treat_item_price, ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "b.remarks, 'S' action, a.head_id, to_char(b.treat_date,'yyyy-mm-dd') treat_date, a.pay_status ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "from cs_treatment_head a ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "join cs_treatment_detail b on (a.head_id=b.head_id) ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "join cs_treatment_item c on (b.treat_item_id=c.treat_item_id) ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "where rm_no='" + s_rm + "' ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "and to_char(visit_date,'yyyy-mm-dd')='" + s_date + "' ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "and visit_no='" + s_que + "' and id_visit = '" + idvisit + "'  ";
            //sql_tind_load = sql_tind_load + Environment.NewLine + "and a.status='OPN' ";

            sql_tind_load = sql_tind_load + Environment.NewLine + "and (c.treat_type_id is null or c.treat_type_id not in ('TRT02') )";

            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_tind_load, oraConnect2);
            DataTable dt2 = new DataTable();
            adOra2.Fill(dt2);

            gridControl14.DataSource = null;
            gridView14.Columns.Clear();
            gridControl14.DataSource = dt2;

            gridView14.OptionsView.ColumnAutoWidth = true;
            gridView14.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView14.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView14.IndicatorWidth = 30;
            //gridView14.OptionsBehavior.Editable = false;
            gridView14.BestFitColumns();

            gridView14.Columns[0].Caption = "ID";
            gridView14.Columns[1].Caption = "Grup Tindakan";
            gridView14.Columns[2].Caption = "Nama Tindakan";
            gridView14.Columns[3].Caption = "Jumlah";
            gridView14.Columns[4].Caption = "Harga";
            gridView14.Columns[5].Caption = "Remark";
            gridView14.Columns[6].Caption = "Action";
            gridView14.Columns[7].Caption = "Head ID";
            gridView14.Columns[8].Caption = "Tanggal";
            gridView14.Columns[9].Caption = "Status Bayar";

            gridView14.Columns[3].Width = 60;
            gridView14.Columns[4].Width = 80;

            //gridView14.Columns[9].VisibleIndex = 6;

            gridView14.Columns[0].Visible = false;
            gridView14.Columns[4].Visible = false;
            gridView14.Columns[6].Visible = false;
            gridView14.Columns[7].Visible = false;
            gridView14.Columns[8].Visible = false;
            gridView14.Columns[9].Visible = false;

            gridView14.Columns[1].OptionsColumn.ReadOnly = true;
            gridView14.Columns[3].OptionsColumn.ReadOnly = true;
            gridView14.Columns[4].OptionsColumn.ReadOnly = true;
            gridView14.Columns[6].OptionsColumn.ReadOnly = true;

            RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
            glLaya.DataSource = listLaya2;
            glLaya.ValueMember = "layananCode";
            glLaya.DisplayMember = "layananName";

            glLaya.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glLaya.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glLaya.ImmediatePopup = true;
            glLaya.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glLaya.NullText = "";
            gridView14.Columns[2].ColumnEdit = glLaya;

            RepositoryItemLookUpEdit grpLookup = new RepositoryItemLookUpEdit();
            grpLookup.DataSource = listGrpLaya;
            grpLookup.ValueMember = "statCode";
            grpLookup.DisplayMember = "statName";

            grpLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            grpLookup.DropDownRows = listGrpLaya.Count;
            grpLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            grpLookup.AutoSearchColumnIndex = 1;
            grpLookup.NullText = "";
            gridView14.Columns[1].ColumnEdit = grpLookup;

            btnAddTindakan.Enabled = true;

            if (gridView14.RowCount > 0)
            {
                btnDelTindakan.Enabled = true;

            }
            else
            {


                btnDelTindakan.Enabled = false;
            }
        } 
        private void gridView14_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e) 
        {
            if (e.RowHandle >= 0) 
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString(); 
            }

        }

        private void btnAddTindakan_Click(object sender, EventArgs e)
        {
            gridView14.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView14.AddNewRow();
        }

        private void gridView14_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            //view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
            //gridView6.Columns[3].OptionsColumn.ReadOnly = false;
            view.SetRowCellValue(e.RowHandle, view.Columns[6], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[8], today);
            //view.SetRowCellValue(e.RowHandle, view.Columns[6], "TRT02");
            btnAddTindakan.Enabled = false;
        }

        private void btnSaveTindakan_Click(object sender, EventArgs e)
        {
            string date = "", pasno = "", rm_no = "", que = "", nama_laya = "", head = "", detail = "", ldate = "", qty = "", price = "", remarks = "", action = "", stbyr = "";
            string sql_cnt = "", diag_cnt = "", sql_update = "";

            date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();

            for (int i = 0; i < gridView14.DataRowCount; i++)
            {
                detail = gridView14.GetRowCellValue(i, gridView14.Columns[0]).ToString();
                head = gridView14.GetRowCellValue(i, gridView14.Columns[7]).ToString();
                nama_laya = gridView14.GetRowCellValue(i, gridView14.Columns[2]).ToString();
                ldate = gridView14.GetRowCellValue(i, gridView14.Columns[8]).ToString();
                qty = gridView14.GetRowCellValue(i, gridView14.Columns[3]).ToString();
                price = gridView14.GetRowCellValue(i, gridView14.Columns[4]).ToString();
                remarks = gridView14.GetRowCellValue(i, gridView14.Columns[5]).ToString();
                action = gridView14.GetRowCellValue(i, gridView14.Columns[6]).ToString();
                stbyr = gridView14.GetRowCellValue(i, gridView14.Columns[9]).ToString();

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
                        sql_cnt = " select count(0) cnt from cs_treatment_detail where head_id = '" + head + "' and to_char(treat_date,'yyyy-mm-dd') = '" + ldate + "' and treat_item_id = '" + nama_laya + "' ";
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

                                command.CommandText = " insert into cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp) values ( '" + seq_val + "', '" + head + "', '" + nama_laya + "', to_date('" + ldate + "', 'yyyy-mm-dd'), " + qty + ", " + price + ", " + price + ", '" + remarks + "', sysdate, '" + DB.vUserId + "') ";
                                command.ExecuteNonQuery();

                                command.CommandText = " insert into cs_action (act_id, rm_no, insp_date, visit_dt, visit_no, detail_id, ins_date, ins_emp) values ( CS_ACTION_SEQ.nextval, '" + rm_no + "', to_date('" + ldate + "', 'yyyy-mm-dd'), to_date('" + date + "', 'yyyy-mm-dd'), '" + que + "', '" + seq_val + "', sysdate, '" + DB.vUserId + "') ";
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

                        sql_update = sql_update + " update cs_treatment_detail" +
                                                  " set remarks = '" + remarks + "', ";
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

        private void btnDelTindakan_Click(object sender, EventArgs e)
        {
            if (gridView14.DataRowCount < 1)
                return;


            if (MessageBox.Show("Anda yakin akan menghapus data?",
                      "Message",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string sql_delete = "", id = "", payst = "";

                id = gridView14.GetRowCellValue(gridView14.FocusedRowHandle, gridView14.Columns[0]).ToString();
                payst = gridView14.GetRowCellValue(gridView14.FocusedRowHandle, gridView14.Columns[9]).ToString();

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


                        command.CommandText = " delete cs_treatment_detail where detail_id = '" + id + "' ";
                        command.ExecuteNonQuery();

                        command.CommandText = " delete cs_action where detail_id = '" + id + "' ";
                        command.ExecuteNonQuery();

                        trans.Commit();
                        //MessageBox.Show(sql_insert);
                        //MessageBox.Show("Query Exec : " + sql_insert);
                        gridView14.DeleteRow(gridView14.FocusedRowHandle);
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
                //LoadAddTind();
            }
        } 
        private void gridView14_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveTindakan.Enabled = true;
            GridView view = sender as GridView;

            string a = "", tmp_stat = "", date = "", que = "", rm_no = "";

            date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();

            a = view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();
            tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();

            if (e.Column.Caption == "Nama Tindakan" && a != "")
            {
                string sql_ = "", sql_head = "", group_id = "", price = "", head_id = "", stbyr = "";
                sql_ = " select treat_group_id, treat_item_price from cs_treatment_item where treat_item_id = " + a + " ";

                OleDbConnection oraConnect0 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_, oraConnect0);
                DataTable dt0 = new DataTable();
                adOra0.Fill(dt0);
                if (dt0.Rows.Count > 0)
                {
                    group_id = dt0.Rows[0]["treat_group_id"].ToString();
                    price = dt0.Rows[0]["treat_item_price"].ToString();
                }

                sql_head = " select head_id, pay_status from cs_treatment_head where rm_no = '" + rm_no + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and visit_no = '" + que + "' ";

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
                    view.SetRowCellValue(e.RowHandle, view.Columns[3], "1");
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], price);
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], stbyr);
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], "U");
                }

            }

            if (e.Column.Caption == "Remark")
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

        private void gridView14_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama Tindakan" || e.Column.Caption == "Remark")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void chOUmum_CheckedChanged(object sender, EventArgs e)
        {
            if (chOUmum.Checked)
            {
                splitContainer1.Panel2Collapsed = false;
                LoadDataResep2();
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
            }
            //if (gridView1.RowCount > 0)
            //{
            //    string sstatus = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[15]).ToString();

            //    if (chOUmum.Checked)
            //    {
            //        lstsobat.Text = "U";
            //        chOUmum.BackColor = Color.LightGreen;
            //    }
            //    else
            //    {
            //        lstsobat.Text = sstatus;
            //        chOUmum.BackColor = Color.DarkOrange;
            //    }

            //    LoadDataResep();
            //}
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string sql_all = "", gnder = "", p1 = "", p2 = "", teks = "", p_que = "", policd = "", rm_type = "", s_name = "", q_no2 = "", age="", sql_ ="", sql_diag="", diag_cnt ="";
            int stsimpan = 0;



            p_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            gnder = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[3]).ToString();
            s_name = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
            age = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
            policd = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();
            //pasienno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();

            if (MessageBox.Show("Data Tidak Dapat di ubah lagi. Anda yakin akan memproses data?",
                  "Message",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                sql_diag = " select count(0) cnt from KLINIK.cs_diagnosa where to_char(insp_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and ANAMNESA_ID = " + v_anamnesa + " and rm_no = '" + lMedRm.Text + "' ";
                OleDbConnection oraConnectd = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOrad = new OleDbDataAdapter(sql_diag, oraConnectd);
                DataTable dtd = new DataTable();
                adOrad.Fill(dtd);
                diag_cnt = dtd.Rows[0]["cnt"].ToString();

                if (diag_cnt == "0")
                {
                    labelControl223.Visible = true;
                    labelControl223.Text = "Gagal..Diagnosa Belum Di Input.";
                    Blinking(labelControl223, 0);
                    return;
                }

                //sql_ = "";
                //sql_ = " update KLINIK.cs_visit set status = 'MED', time_inspection=sysdate where ID_VISIT =" + idvisit + " "; // and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                //ConnOra.ExeNonQuery(sql_);

                sql_all = "";
                sql_all = sql_all + @" select TYPE_INS, nvl(b.que02,'N') qno2
                                   from KLINIK.CS_CALL_LOG a, KLINIK.cs_visit b
                                  where a.que = b.que01
                                    AND A.QUE = '" + p_que + @"'    
                                    AND TRUNC(a.INS_DATE) = TRUNC(SYSDATE)
                                    AND TRUNC(A.INS_DATE) = TRUNC(VISIT_DATE(+))  ";

                OleDbConnection oraConnect5 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra5 = new OleDbDataAdapter(sql_all, oraConnect5);
                DataTable dt5 = new DataTable();
                adOra5.Fill(dt5);
                if (dt5.Rows.Count > 0)
                {
                    rm_type = dt5.Rows[0]["TYPE_INS"].ToString();
                    q_no2 = dt5.Rows[0]["qno2"].ToString();
                }

                if (rm_type.ToString().Equals("MID") && !q_no2.ToString().Equals("N"))
                {
                    if (gnder.ToString().Equals("P") && Convert.ToInt32(age) > 12 && Convert.ToInt32(age) < 31)
                    {
                        p1 = " Saudari  ";
                    }
                    else if (gnder.ToString().Equals("P") && Convert.ToInt32(age) > 30)
                    {
                        p1 = " Nyonya  ";
                    }
                    else if (gnder.ToString().Equals("L") && Convert.ToInt32(age) > 12 && Convert.ToInt32(age) < 31)
                    {
                        p1 = " Saudara  ";
                    }
                    else if (gnder.ToString().Equals("L") && Convert.ToInt32(age) > 30)
                    {
                        p1 = " Tuan  ";
                    }

                    if (Convert.ToInt32(age) < 13)
                    {
                        p1 = " Anak  ";
                    }


                    p2 = s_name + " ";

                    teks = "Nomor Antrian " + p_que + " " + p1 + p2 + " silahkan menuju ke Farmasi";

                    sql_all = "";
                    sql_all = @"UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'W', type_ins ='MED', stat ='Farmasi', param = '" + teks + "', UPD_ANTRIAN = sysdate  WHERE QUE = '" + p_que + "' AND TRUNC(INS_DATE) = TRUNC(SYSDATE)";

                    ORADB.Execute(ORADB.XE, sql_all);

                    sql_all = "";
                    sql_all = " update KLINIK.cs_visit set status = 'MED', time_inspection=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where ID_VISIT =" + idvisit + " "; // and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                    ORADB.Execute(ORADB.XE, sql_all);

                    stsimpan = 1;
                }
                else if (rm_type.ToString().Equals("MID") && q_no2.ToString().Equals("N"))
                {
                    if (gnder.ToString().Equals("P") && Convert.ToInt32(age) > 12 && Convert.ToInt32(age) < 31)
                    {
                        p1 = " Saudari  ";
                    }
                    else if (gnder.ToString().Equals("P") && Convert.ToInt32(age) > 30)
                    {
                        p1 = " Nyonya  ";
                    }
                    else if (gnder.ToString().Equals("L") && Convert.ToInt32(age) > 12 && Convert.ToInt32(age) < 31)
                    {
                        p1 = " Saudara  ";
                    }
                    else if (gnder.ToString().Equals("L") && Convert.ToInt32(age) > 30)
                    {
                        p1 = " Tuan  ";
                    }

                    if (Convert.ToInt32(age) < 13)
                    {
                        p1 = " Anak  ";
                    }

                    p2 = s_name + " ";

                    teks = "Nomor Antrian " + p_que + " " + p1 + p2 + " silahkan menuju ke Kasir";

                    sql_all = "";
                    sql_all = @"UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'W', type_ins ='PAY', stat ='Kasir', param = '" + teks + "', UPD_ANTRIAN = sysdate WHERE  ID_VISIT =" + idvisit + "  ";

                    ORADB.Execute(ORADB.XE, sql_all);

                    sql_all = "";
                    sql_all = " update KLINIK.cs_visit set status = 'PAY', TIME_END=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where  ID_VISIT =" + idvisit + " "; // and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                    ORADB.Execute(ORADB.XE, sql_all);

                    stsimpan = 1;
                }
                else
                {
                    //MessageBox.Show("Maaf Status Sudah Closed. Data tidak bisa di ubah..!!!");
                    labelControl223.Visible = true;
                    labelControl223.Text = "Gagal..Status Sudah Closed.";
                    Blinking(labelControl223, 0);
                    return;
                }
            }

            if (stsimpan == 1)
            {
                //MessageBox.Show("Data Pemeriksaan Berhasil di Closed.");
                labelControl223.Visible = true;
                labelControl223.Text = "Pemeriksaan Berhasil di Closed.";
                Blinking(labelControl223, 1);
                simpleButton2.Enabled = false;
            }
        }

        private void gridView16_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            sSimpanU.Enabled = true;
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();

            if (e.Column.Caption == "Nama Obat" && (a.Substring(0, 2) == "BP" || a.Substring(0, 2) == "UM"))
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();
                string policd = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();
                string sql_medcd = "", sql_med = "", med_cd = "", med_name = "", med_group = "", med_stok = "", med_uom = "", cek_stok = "", sql_for = "";
                s_stat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[15]).ToString();
                sql_medcd = " select " +
                            " klinik.FN_CS_INIT_STOCK(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + a + "') +  " +
                            " klinik.FN_CS_TRX_IN(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + a + "') -  " +
                            " klinik.FN_CS_TRX_OUT(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + a + "') - " +
                            " klinik.FN_CS_REQ_STOCK(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + a + "') stock from dual ";

                OleDbConnection oraConnect0 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_medcd, oraConnect0);
                DataTable dt0 = new DataTable();
                adOra0.Fill(dt0);
                cek_stok = dt0.Rows[0]["stock"].ToString();

                sql_med = " select med_cd, initcap(med_name) med_name, med_group, '" + cek_stok + "' stock, initcap(uom) uom " +
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
                 
                s_stat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[15]).ToString(); 

                sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1  and  b.med_cd = '" + med_cd + "' and upper(att1) =  'UMUM' and MINUS_STOK ='Y' and a.POLI_CD in ('POL0002','POL0003')  AND RACIKAN ='N' ";
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
                if (dtf.Rows.Count == 1)
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], dtf.Rows[0]["formula_id"].ToString());
                else
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], "");

                view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                //view.SetRowCellValue(e.RowHandle, view.Columns[4], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[3], med_name);
                    view.SetRowCellValue(e.RowHandle, view.Columns[2], med_group);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                    view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    view.SetRowCellValue(e.RowHandle, view.Columns[7], "0");
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }

                //dataFormula(policd);
            }

            if (e.Column.Caption == "Kode Dosis")
            {
                string medicine_cd = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string formula_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string reg_dt = lMedDate.Text;
                string rm = lMedRm.Text;
                string que = lMedQue.Text;
                string stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                string kode = "", sql_pilihan = "";

                if (stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
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
                        view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                        view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                    }
                    else
                    {
                        MessageBox.Show("Kode Formula tidak valid");
                        return;
                        //LoadDataResep();
                    }
                }


            }

            if (e.Column.Caption == "Jml")
            {
                string sql_for = "", med_price = "", qty = "", tmp_stat = "";
                string for_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string tmp_hari = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                int tot_hari = 0, tot_harga = 0;

                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

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

                tot_hari = Convert.ToInt32(tmp_hari) * Convert.ToInt32(qty);
                tot_harga = Convert.ToInt32(Convert.ToInt32(tmp_hari) * Convert.ToDouble(med_price));

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                    view.SetRowCellValue(e.RowHandle, view.Columns[7], tot_hari.ToString());
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                    view.SetRowCellValue(e.RowHandle, view.Columns[7], tot_hari.ToString());
                }
            }

            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Info" || e.Column.Caption == "Dosis" || e.Column.Caption == "Remark")
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

        private void gridView16_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView16_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
        }

        private void gridView16_RowCellStyle(object sender, RowCellStyleEventArgs e)
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
                    if (Convert.ToInt32(stok) == 0)
                    {
                        e.Appearance.BackColor = Color.Crimson;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (Convert.ToInt32(stok) <= 20)
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

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            string stat = gridView16.GetRowCellDisplayText(gridView16.FocusedRowHandle, gridView16.Columns[3]);
            if (stat == "")
            {
                gridView16.DeleteRow(gridView16.FocusedRowHandle);
            }
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            string sql_delete = "", id = "", confirm = "";

            id = gridView16.GetRowCellValue(gridView16.FocusedRowHandle, gridView16.Columns[0]).ToString();
            confirm = gridView16.GetRowCellValue(gridView16.FocusedRowHandle, gridView16.Columns[10]).ToString();

            if (confirm == "Y")
            {
                //MessageBox.Show("Data tidak bisa dihapus.");
                labelControl219.Visible = true;
                labelControl219.Text = "Gagal..Obat Sudah Confirm!!";
                Blinking(labelControl219, 0);
                return;
            }
            else
            {
                sql_delete = "";
                sql_delete = sql_delete + " delete from KLINIK.cs_receipt";
                sql_delete = sql_delete + " where receipt_id = '" + id + "' and confirm='N' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_update);
                    LoadDataResep();
                    labelControl219.Visible = true;
                    labelControl219.Text = "Berhasil di hapus";
                    Blinking(labelControl219, 1);
                    //MessageBox.Show("Data Berhasil di hapus");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }

        private void sTambahU_Click(object sender, EventArgs e)
        {
            gridView16.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView16.AddNewRow();
        }

        private void sSimpanU_Click(object sender, EventArgs e)
        {
            string kode = "", dosis = "", info = "", jumlah = "", id = "", stok = "", con = "", action = "";
            string sql_cnt = "", med_cnt = "", sql_update = "", sql_diag = "", diag_cnt = "", harga = "", hari = "", jph = "", info_dosis = "";
            int stsimpan = 0;

            for (int i = 0; i < gridView16.DataRowCount; i++)
            {
                id = gridView16.GetRowCellValue(i, gridView16.Columns[0]).ToString();
                kode = gridView16.GetRowCellValue(i, gridView16.Columns[1]).ToString();
                dosis = gridView16.GetRowCellValue(i, gridView16.Columns[4]).ToString();
                info = gridView16.GetRowCellValue(i, gridView16.Columns[5]).ToString();
                jumlah = gridView16.GetRowCellValue(i, gridView16.Columns[7]).ToString();
                stok = gridView16.GetRowCellValue(i, gridView16.Columns[6]).ToString();
                con = gridView16.GetRowCellValue(i, gridView16.Columns[10]).ToString();
                action = gridView16.GetRowCellValue(i, gridView16.Columns[9]).ToString();
                harga = gridView16.GetRowCellValue(i, gridView16.Columns[12]).ToString();
                hari = gridView16.GetRowCellValue(i, gridView16.Columns[11]).ToString();
                jph = gridView16.GetRowCellValue(i, gridView16.Columns[13]).ToString();
                info_dosis = gridView16.GetRowCellValue(i, gridView16.Columns[14]).ToString();

                if (con == "Y")
                {
                    labelControl219.Visible = true;
                    labelControl219.Text = "Gagal..Obat Sudah Confirm!!";
                    Blinking(labelControl219, 0);
                    return;
                }
                else if (stok == "0")
                {
                    labelControl219.Visible = true;
                    labelControl219.Text = "Gagal..Obat Kosong!!";
                    Blinking(labelControl219, 0);
                    return;
                }
                else if (jumlah == "" || jumlah == "0")
                {
                    labelControl219.Visible = true;
                    labelControl219.Text = "Gagal..Jumlah Kosong!!";
                    Blinking(labelControl219, 0);
                    return;
                }
                else if (Convert.ToInt32(jumlah) > Convert.ToInt32(stok))
                {
                    labelControl219.Visible = true;
                    labelControl219.Text = "Gagal..Jumlah > Stok";
                    Blinking(labelControl219, 0);
                    return;
                }
                else if (kode == "")
                {
                    labelControl219.Visible = true;
                    labelControl219.Text = "Gagal..Tentukan Obat";
                    Blinking(labelControl219, 0);
                    return;
                }
                else if (dosis == "")
                {
                    labelControl219.Visible = true;
                    labelControl219.Text = "Gagal..Tentukan Dosis";
                    Blinking(labelControl219, 0);
                    return;
                }
                //else if (hari == "")
                //{
                //    MessageBox.Show("Jumlah harus diisi."); return;
                //}
                else if (info == "")
                {
                    labelControl219.Visible = true;
                    labelControl219.Text = "Gagal..Tentukan Info";
                    Blinking(labelControl219, 0);
                    return;
                }
                //else if (info_dosis == "")
                //{
                //    MessageBox.Show("Dosis harus diisi."); return;
                //}
                else
                {
                    int queue = 0;
                    string tmp_queue = "", que = "", cnt = "";
                    string sql_check = " select  nvl(max(to_number(substr(que02,2,3))),0) que from KLINIK.cs_visit where to_char(visit_date,'yyyy-mm-dd')= '" + today + "'  ";
                    string sql_check2 = " select  count(0) cnt from KLINIK.cs_receipt where rm_no = '" + lMedRm.Text + "' and to_char(insp_date,'yyyy-mm-dd')= '" + lMedDate.Text + "' and visit_no = '" + lMedQue.Text + "'  ";

                    try
                    {
                        OleDbConnection oraConnecta = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOraa = new OleDbDataAdapter(sql_check, oraConnecta);
                        DataTable dta = new DataTable();
                        adOraa.Fill(dta);

                        tmp_queue = dta.Rows[0]["que"].ToString();
                        queue = Convert.ToInt32(tmp_queue) + 1;
                        que = queue.ToString();
                        if (queue < 10)
                        {
                            que = que.PadLeft(que.Length + 2, '0');
                        }
                        else if (queue < 100)
                        {
                            que = que.PadLeft(que.Length + 1, '0');
                        }

                        OleDbConnection oraConnectb = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOrab = new OleDbDataAdapter(sql_check2, oraConnectb);
                        DataTable dtb = new DataTable();
                        adOrab.Fill(dtb);
                        cnt = dtb.Rows[0]["cnt"].ToString();

                        if (cnt == "0")
                        {
                            sql_update = "";

                            sql_update = sql_update + " update KLINIK.cs_visit" +
                                                      " set que02 = 'R" + que + "', ";
                            sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                            sql_update = sql_update + " where patient_no = '" + lMedNik.Text + "' and to_char(visit_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and que01 = '" + lMedQue.Text + "' and ID_VISIT =  " + idvisit + " ";

                            try
                            {
                                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                                OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                                oraConnect.Open();
                                cm.ExecuteNonQuery();
                                oraConnect.Close();
                                cm.Dispose();

                                //MessageBox.Show("Query Exec : " + sql_update);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ERROR: " + ex.Message);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }

                    if (action == "I")
                    {
                        sql_diag = " select count(0) cnt from KLINIK.cs_diagnosa where to_char(insp_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and visit_no = '" + lMedQue.Text + "' and rm_no = '" + lMedRm.Text + "' ";
                        OleDbConnection oraConnectd = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOrad = new OleDbDataAdapter(sql_diag, oraConnectd);
                        DataTable dtd = new DataTable();
                        adOrad.Fill(dtd);
                        diag_cnt = dtd.Rows[0]["cnt"].ToString();


                        sql_cnt = " select count(0) cnt from KLINIK.cs_receipt where to_char(insp_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and visit_no = '" + lMedQue.Text + "' and rm_no = '" + lMedRm.Text + "' " + " and med_cd = '" + kode + "' and ID_VISIT =  " + idvisit + " and GRID_NAME ='gridView16' ";
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                        DataTable dt = new DataTable();
                        adOra.Fill(dt);
                        med_cnt = dt.Rows[0]["cnt"].ToString();

                        if (Convert.ToInt32(med_cnt) > 0)
                        {
                            //MessageBox.Show("Gagal Disimpan.");
                        }
                        else if (diag_cnt == "0")
                        {
                            //MessageBox.Show("Gagal Disimpan. Diagnosa belum diinput.");
                            labelControl219.Visible = true;
                            labelControl219.Text = "Gagal..Diagnosa Kosong";
                            Blinking(labelControl219, 0);
                            return;
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

                                command.CommandText = " insert into KLINIK.cs_receipt (receipt_id, rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, visit_no, ins_date, ins_emp,ID_VISIT,GRID_NAME,JENIS_OBAT) " +
                                                      " values(cs_receipt_seq.nextval, '" + lMedRm.Text + "', to_date('" + lMedDate.Text + "', 'yyyy-mm-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "', '" + lMedQue.Text + "', sysdate, '" + DB.vUserId + "', " + idvisit + ",'gridView16','NONE') ";
                                command.ExecuteNonQuery();

                                //command.CommandText = " update cs_visit set status = 'MED', time_inspection=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + lMedNik.Text + "' and to_char(visit_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and que01 = '" + lMedQue.Text + "' ";
                                //command.ExecuteNonQuery();

                                trans.Commit();
                                //MessageBox.Show(sql_insert);
                                //MessageBox.Show("Query Exec : " + sql_insert);

                                //MessageBox.Show("Data Berhasil disimpan.");
                                stsimpan = 1;
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
                                                  "     price = '" + harga + "', days = '" + hari + "', qty_day = '" + jph + "', dosis = '" + info_dosis + "',";
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
                            LoadDataResep();
                            //MessageBox.Show("Data Berhasil diupdate");
                            stsimpan = 2;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
            }
            if (stsimpan == 1)
            {
                labelControl219.Visible = true;
                labelControl219.Text = "Save Success";
                Blinking(labelControl219, 1);
            }
            else if (stsimpan == 2)
            {
                labelControl219.Visible = true;
                labelControl219.Text = "Updated Success";
                Blinking(labelControl219, 1);
            }

            LoadDataResep();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string kode = "", dosis = "", info = "", jumlah = "", id = "", stok = "", con = "", action = "";
            string sql_cnt = "", med_cnt = "", sql_update = "", sql_diag = "", diag_cnt = "", harga = "", hari = "", jph = "", info_dosis = "";
            int stsimpan = 0;

            for (int i = 0; i < gridView16.DataRowCount; i++)
            {
                id = gridView16.GetRowCellValue(i, gridView16.Columns[0]).ToString();
                kode = gridView16.GetRowCellValue(i, gridView16.Columns[1]).ToString();
                dosis = gridView16.GetRowCellValue(i, gridView16.Columns[4]).ToString();
                info = gridView16.GetRowCellValue(i, gridView16.Columns[5]).ToString();
                jumlah = gridView16.GetRowCellValue(i, gridView16.Columns[7]).ToString();
                stok = gridView16.GetRowCellValue(i, gridView16.Columns[6]).ToString();
                con = gridView16.GetRowCellValue(i, gridView16.Columns[10]).ToString();
                action = gridView16.GetRowCellValue(i, gridView16.Columns[9]).ToString();
                harga = gridView16.GetRowCellValue(i, gridView16.Columns[12]).ToString();
                hari = gridView16.GetRowCellValue(i, gridView16.Columns[11]).ToString();
                jph = gridView16.GetRowCellValue(i, gridView16.Columns[13]).ToString();
                info_dosis = gridView16.GetRowCellValue(i, gridView16.Columns[14]).ToString();

                if (con == "Y")
                {
                    MessageBox.Show("Data tidak bisa dirubah."); return;
                }
                else if (stok == "0")
                {
                    MessageBox.Show("Stok obat tidak tersedia."); return;
                }
                else if (jumlah == "" || jumlah == "0")
                {
                    MessageBox.Show("Jumlah obat harus diisi."); return;
                }
                else if (Convert.ToInt32(jumlah) > Convert.ToInt32(stok))
                {
                    MessageBox.Show("Jumlah melebihi stok"); return;
                }
                else if (kode == "")
                {
                    MessageBox.Show("Kode obat harus diisi."); return;
                }
                else if (dosis == "")
                {
                    MessageBox.Show("Kode Dosis harus diisi."); return;
                }
                else if (hari == "")
                {
                    MessageBox.Show("Jumlah harus diisi."); return;
                }
                else if (info == "")
                {
                    MessageBox.Show("Info harus diisi."); return;
                }
                else if (info_dosis == "")
                {
                    MessageBox.Show("Dosis harus diisi."); return;
                }
                else
                {


                    if (action == "I")
                    {
                        sql_diag = " select count(0) cnt from KLINIK.cs_diagnosa where to_char(insp_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and visit_no = '" + lMedQue.Text + "' and rm_no = '" + lMedRm.Text + "' ";
                        OleDbConnection oraConnectd = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOrad = new OleDbDataAdapter(sql_diag, oraConnectd);
                        DataTable dtd = new DataTable();
                        adOrad.Fill(dtd);
                        diag_cnt = dtd.Rows[0]["cnt"].ToString();


                        sql_cnt = " select count(0) cnt from KLINIK.cs_receipt where to_char(insp_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and visit_no = '" + lMedQue.Text + "' and rm_no = '" + lMedRm.Text + "' " + " and med_cd = '" + kode + "' ";
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                        DataTable dt = new DataTable();
                        adOra.Fill(dt);
                        med_cnt = dt.Rows[0]["cnt"].ToString();

                        if (Convert.ToInt32(med_cnt) > 0)
                        {
                            //MessageBox.Show("Gagal Disimpan.");
                        }
                        else if (diag_cnt == "0")
                        {
                            MessageBox.Show("Gagal Disimpan. Diagnosa belum diinput."); return;
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

                                command.CommandText = " insert into KLINIK.cs_receipt (receipt_id, rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, visit_no, ins_date, ins_emp,ID_VISIT) " +
                                                      " values(cs_receipt_seq.nextval, '" + lMedRm.Text + "', to_date('" + lMedDate.Text + "', 'yyyy-mm-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "', '" + lMedQue.Text + "', sysdate, '" + DB.vUserId + "', " + idvisit + ") ";
                                command.ExecuteNonQuery();
                                trans.Commit();
                                stsimpan = 1;
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
                                                  "     price = '" + harga + "', days = '" + hari + "', qty_day = '" + jph + "', dosis = '" + info_dosis + "',";
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
                            LoadDataResep2();
                            //MessageBox.Show("Data Berhasil diupdate");
                            stsimpan = 2;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
            }
            if (stsimpan == 1)
                MessageBox.Show("Data Berhasil disimpan.");
            else if (stsimpan == 2)
                MessageBox.Show("Data Berhasil diupdate");

            LoadDataResep2();
        }

        private void timerStart_Tick(object sender, EventArgs e)
        {
            if (lsOK == 0)
            {
                if (bl_klap == true)
                {
                    _currentLabel.Appearance.ForeColor = Color.Red;
                    _currentLabel.Visible = true;
                    bl_klap = false;
                }
                else
                {
                    bl_klap = true;
                    _currentLabel.Visible = false;
                }
            }
            else
            {
                if (bl_klap == true)
                {
                    _currentLabel.Appearance.ForeColor = Color.ForestGreen;
                    _currentLabel.Visible = true;
                    bl_klap = false;
                }
                else
                {
                    _currentLabel.Visible = false;
                    bl_klap = true;
                }
            }
        }

        private void timerEnd_Tick(object sender, EventArgs e)
        {
            timerStart.Enabled = false;
            timerEnd.Enabled = false;
            _currentLabel.Visible = false;
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView15_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
         
        private void tAdd05_EditValueChanged(object sender, EventArgs e)
        {

        }
         
        private void gridView15_RowClick(object sender, RowClickEventArgs e)
        {
            GridView View = sender as GridView;


            string sql_cek = "", s_id = "", hasil = "", rekom = "", id = "";

            s_id = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);

            sql_cek = " select act_id, act_name, act_remark from cs_action where act_id = '" + s_id + "' ";
            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cek, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                hasil = dt.Rows[0]["act_name"].ToString();
                rekom = dt.Rows[0]["act_remark"].ToString();
                id = dt.Rows[0]["act_id"].ToString();

                mActName.Text = hasil;
                mActRemark.Text = rekom;
                lActID.Text = id;
            }
            else
            {
                mActName.Text = "";
                mActRemark.Text = "";
                lActID.Text = "";
            }
        }

        private void panel25_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tAdd07_TextChanged(object sender, EventArgs e)
        {
            string sql_cek = "", p_start = "", p_end = "";

            if (tAdd05.Text != "")
            {
                sql_cek = " select to_char(to_date('" + tAdd07.Text + "','yyyy-mm-dd') - 45,'yyyy-mm-dd') dst, to_char(to_date('" + tAdd07.Text + "','yyyy-mm-dd') + 45,'yyyy-mm-dd') dend  from dual ";


                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cek, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                p_start = dt.Rows[0]["dst"].ToString();
                p_end = dt.Rows[0]["dend"].ToString();
                tAdd09.Text = p_start;
                tAdd10.Text = p_end;
            }
        }

        private void tAdd06_TextChanged(object sender, EventArgs e)
        {
            string sql_cek = "", p_start = "", p_end = "";
            sql_cek = " select to_char(to_date('" + tAdd06.Text + "','yyyy-mm-dd') - 45,'yyyy-mm-dd') dst, to_char(to_date('" + tAdd06.Text + "','yyyy-mm-dd') + 45,'yyyy-mm-dd') dend  from dual ";


            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cek, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            p_start = dt.Rows[0]["dst"].ToString();
            p_end = dt.Rows[0]["dend"].ToString();
            tAdd09.Text = p_start;
            tAdd10.Text = p_end;
        }

        private void lSkdID_Click(object sender, EventArgs e)
        {

        }

        private void xtraTabControl2_Click(object sender, EventArgs e)
        {
            if (xtraTabControl2.SelectedTabPage.Text == "Terapi / Resep")
            {
                if (tmp_now != rNow.Text || tmp_old != rOld.Text || tmp_fam != rFam.Text || tmp_fisik != pFisik.Text || tmp_add != pAdd.Text)

                {
                    MessageBox.Show("Data Anamnesa belum disimpan. "); 
                }
                loadResep_Click(sender, e);
            }
            if (xtraTabControl2.SelectedTabPage.Text == "Pelayanan")
            {
                if (tmp_now != rNow.Text || tmp_old != rOld.Text || tmp_fam != rFam.Text || tmp_fisik != pFisik.Text || tmp_add != pAdd.Text)
                {
                    MessageBox.Show("Data Anamnesa belum disimpan. ");
                }
                loadTind_Click(sender, e);
            }
        }

        private void btnCall_Click(object sender, EventArgs e)
        {
            string p_que = "";
            string sql_check5 = "", rm_number = "", sql1 = "";
            if (gridView1.RowCount < 1) return;

            p_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();

            sql_check5 = sql_check5 + "select TYPE_INS from KLINIK.CS_CALL_LOG where  QUE = '" + p_que + "' AND TRUNC(INS_DATE) = TRUNC(SYSDATE)";

            OleDbConnection oraConnect5 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra5 = new OleDbDataAdapter(sql_check5, oraConnect5);
            DataTable dt5 = new DataTable();
            adOra5.Fill(dt5);
            if (dt5.Rows.Count > 0)
            {
                rm_number = dt5.Rows[0]["TYPE_INS"].ToString();
            }

            if (rm_number.ToString().Equals("MID"))
            {
                sql1 = " ";
                sql1 = @"UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'N', UPD_ANTRIAN = sysdate WHERE QUE = '" + p_que + "' and TYPE_INS ='MID' AND TRUNC(INS_DATE) = TRUNC(SYSDATE)";

                ORADB.Execute(ORADB.XE, sql1);
            }
            else
            {
                MessageBox.Show("Maaf Pasien sudah di Proses, Tidak Dapat Dipanggil Di Bagian Bidan.");
                return;
            }
        }
        private void Blinking(LabelControl ctrl, int mbOk)
        {
            //lsMSG = Message;
            lsOK = mbOk;
            _currentLabel = ctrl;
            timerStart.Interval = 150;
            timerStart.Enabled = true;
            //timer1.Interval = 2000;
            //timer1.Enabled = true;

            timerEnd.Enabled = true;
            timerEnd.Interval = 3000;
            //timer3.Interval = 4000;
            //timer3.Enabled = true;
        }
    }
}