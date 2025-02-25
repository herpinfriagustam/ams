using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic
{
    public partial class FrmRawatInap : DevExpress.XtraEditors.XtraForm
    {
        private string anamesaID = "", visitid ="", headid ="", RMNO="",pasienno="", type_s ="", inpatient_id ="", fnama ="";
        DataTable dtJadwalObat; DataTable dtStock;
        DataTable dtObatPulang; DataTable datstock = new DataTable();
        DataTable dtCppt; DataTable dtMedis; DataTable dtMedisU; DataTable dtVisitDokter; DataTable dtGlDiag = new DataTable();
        DataTable dtVital; 
        List<Layanan> listLaya2 = new List<Layanan>(); List<Layanan> listLayav = new List<Layanan>(); List<Layanan> listLayaU = new List<Layanan>();
        List<Dokter> listDokter = new List<Dokter>(); List<Dosis> listDosis = new List<Dosis>(); List<Racik> listRacik = new List<Racik>();
        List<Diagnosa> listDiagnosa = new List<Diagnosa>();
        List<Medicine> listMedicine = new List<Medicine>(); List<Medicine> listMedicineP = new List<Medicine>();
        List<MedicineInfo> listMedicineInfo = new List<MedicineInfo>();
        List<Stat> listHours = new List<Stat>();
        List<Formula> listFormula = new List<Formula>(); List<Formula2> listFormulaR = new List<Formula2>();
        List<Formula2> listFormula2 = new List<Formula2>(); List<Formula2> listFormulaU = new List<Formula2>(); 
        List<Room> listRoom = new List<Room>(); 
        List<Medicine> listMedicineU = new List<Medicine>(); List<Medicine> listMedicineRacik = new List<Medicine>();

        List<MedGroup> lMedicine = new List<MedGroup>(); List<MedGroup> lMedicineP = new List<MedGroup>();
        List<MedGroup> lMedicineU = new List<MedGroup>(); List<MedGroup> lMedicineRacik = new List<MedGroup>();

        DataTable dtGlMed = new DataTable(); DataTable dtGlMedP = new DataTable(); DataTable dtGlMedU = new DataTable(); DataTable dtGlMedRacik = new DataTable();
        RepositoryItemGridLookUpEdit glmed = new RepositoryItemGridLookUpEdit();
        RepositoryItemLookUpEdit glmedU = new RepositoryItemLookUpEdit();
        RepositoryItemGridLookUpEdit glmedRacik = new RepositoryItemGridLookUpEdit();
        RepositoryItemLookUpEdit lookLaya = new RepositoryItemLookUpEdit();
        RepositoryItemLookUpEdit lookLayaU = new RepositoryItemLookUpEdit();
        RepositoryItemLookUpEdit lookVisit = new RepositoryItemLookUpEdit();
        RepositoryItemLookUpEdit lookDiagnosa = new RepositoryItemLookUpEdit();
        ConnectDb ConnOra = new ConnectDb();
        KoneksiOra  koneksi = new KoneksiOra();
        RepositoryItemGridLookUpEdit glfor = new RepositoryItemGridLookUpEdit(); RepositoryItemGridLookUpEdit glforR = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit glforU = new RepositoryItemGridLookUpEdit();
        RepositoryItemLookUpEdit medicineInfoLookup = new RepositoryItemLookUpEdit();
        RepositoryItemLookUpEdit dosisLookup = new RepositoryItemLookUpEdit();
        RepositoryItemLookUpEdit racikLookup = new RepositoryItemLookUpEdit();

        RepositoryItemGridLookUpEdit LokDiagGrid = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit LokDiagGridP = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit LokObatGrid = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit LokObatGridU = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit LokObatGridR = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit LokObatGridP = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit LokPelayanan = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit LokPelayananU = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit LokPelayananD = new RepositoryItemGridLookUpEdit();

        string today = DateTime.Now.ToString("yyyy-MM-dd");
        int timer = 0, timer2 = 0,cek_interval = 180;
        private LabelControl _currentLabel;
        string lsMSG = ""; int lsOK = 0; bool bl_klap = true;

        public FrmRawatInap()
        {
            InitializeComponent();
            //this.Location = new Point(0, 0);
            //this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            Scroll2.HorizontalScroll.Visible = true; 
            Scroll2.VerticalScroll.Visible = true;
            InitializeLookupEdit();
            txControlLanjutan.Properties.Mask.Culture = new System.Globalization.CultureInfo("id-ID");
            txControlLanjutan.Properties.Mask.EditMask = "dddd, yyyy-MM-dd HH:mm:ss";
            txControlLanjutan.Properties.Mask.UseMaskAsDisplayFormat = true;

            dtkeluar.Properties.Mask.Culture = new System.Globalization.CultureInfo("id-ID");
            dtkeluar.Properties.Mask.EditMask = "yyyy-MM-dd";
            dtkeluar.Properties.Mask.UseMaskAsDisplayFormat = true;

            dtKeluarx.Properties.Mask.Culture = new System.Globalization.CultureInfo("id-ID");
            dtKeluarx.Properties.Mask.EditMask = "yyyy-MM-dd";
            dtKeluarx.Properties.Mask.UseMaskAsDisplayFormat = true;

            foreach (Control control in scrolPulang.Controls)
            {
                if (control is LabelControl)
                {
                    LabelControl labelControl = (LabelControl)control;
                    labelControl.Padding = new Padding(3, 3, 3, 3);
                }
            }

            //System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("id-ID");
            //System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("id-ID", true);
            //cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd"; 

            foreach (GridColumn column in gvObatUmum.Columns)
            {
                if (Type.GetTypeCode(column.ColumnType) == TypeCode.DateTime)
                {
                    column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    column.DisplayFormat.FormatString = @"yyyy-MM-dd";
                }
            }
        }
        private void InitData()
        {  
            dtGlDiag.Clear();
            string sql_poli = " select '' item_cd, '' item_name from dual union all select item_cd, initcap(item_name) item_name from KLINIK.cs_diagnosa_item where status = 'A' order by 1 ";
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

            //txDiagnosaAkhir.Properties.DataSource = listDiagnosa;
            //txDiagnosaAkhir.Properties.ValueMember = "diagnosaCode";
            //txDiagnosaAkhir.Properties.DisplayMember = "diagnosaName";

            //txDiagnosaAkhir.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //txDiagnosaAkhir.Properties.DropDownRows = listDiagnosa.Count;
            //txDiagnosaAkhir.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            //txDiagnosaAkhir.Properties.AutoSearchColumnIndex = 0;
            //txDiagnosaAkhir.Properties.NullText = "";
            //txDiagnosaAkhir.ItemIndex = -1;

            //ConnOra.LookUpGridFilter(listDiagnosa, gridView4, "diagnosaCode", "diagnosaName", LokDiagGrid, 2);
            ConnOra.LookUpEditFilter(listDiagnosa, mmDokter, "diagnosaCode", "diagnosaName", LokDiagGrid, -1); 
            ConnOra.LookUpEditFilter(listDiagnosa, txDiagnosaAkhir, "diagnosaCode", "diagnosaName", LokDiagGrid, -1);
            //ConnOra.LookUpEditFilter(listDiagnosa, txDiagnosaAkhir, "diagnosaCode", "diagnosaName", LokDiagGrid, -1);



            //mmDokter.Properties.DataSource = listDiagnosa;
            //mmDokter.Properties.ValueMember = "diagnosaCode";
            //mmDokter.Properties.DisplayMember = "diagnosaName";

            //mmDokter.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //mmDokter.Properties.DropDownRows = listDiagnosa.Count;
            //mmDokter.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            //mmDokter.Properties.AutoSearchColumnIndex = 0;
            //mmDokter.Properties.NullText = "";
            //mmDokter.ItemIndex = -1;

            string SQL2 = "";
            SQL2 = SQL2 + Environment.NewLine + "select ID_Dokter, NM_DOKTER Nama_Dokter ";
            SQL2 = SQL2 + Environment.NewLine + "from KLINIK.CS_DOKTER ";
            SQL2 = SQL2 + Environment.NewLine + "where 1=1 AND F_AKTIF ='Y' and NM_DOKTER <> 'System' ";
            //SQL = SQL + Environment.NewLine + "and treat_type_id = 'TRT02'  ";

            OleDbConnection oraConny = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(SQL2, oraConny);
            DataTable dtdok = new DataTable();
            adOra.Fill(dtdok);
            listDokter.Clear();
            for (int i = 0; i < dtdok.Rows.Count; i++)
            {
                listDokter.Add(new Dokter() { ID_Dokter = dtdok.Rows[i]["ID_Dokter"].ToString(), Nama_Dokter = dtdok.Rows[i]["Nama_Dokter"].ToString() });
            }

            ConnOra.LookUpEditFilter(listDokter, txDokterPengirim, "ID_Dokter", "Nama_Dokter", LokDiagGrid, -1);
            ConnOra.LookUpEditFilter(listDokter, txDokterKonsultan, "ID_Dokter", "Nama_Dokter", LokDiagGrid, -1);

            string SQL3 = " ";
            SQL3 = " ";
            SQL3 = SQL3 + Environment.NewLine + "select bed_id, room_name || substr(bed_id,-3) room_name, decode(b.use_yn,'N','1','0') qty ";
            SQL3 = SQL3 + Environment.NewLine + "from cs_room a ";
            SQL3 = SQL3 + Environment.NewLine + "join cs_bed b on (a.room_id=b.room_id) ";
            SQL3 = SQL3 + Environment.NewLine + "join cs_room_class c on (a.class_id=c.class_id) ";
            SQL3 = SQL3 + Environment.NewLine + "where 1=1 "; 

            OleDbConnection sqlConRom = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSqlRom = new OleDbDataAdapter(SQL3, sqlConRom);
            DataTable dtRom = new DataTable();
            adSqlRom.Fill(dtRom);
            listRoom.Clear();
            listRoom.Add(new Room() { roomCode = "", roomName = "Pilih" });
            for (int i = 0; i < dtRom.Rows.Count; i++)
            {
                listRoom.Add(new Room() { roomCode = dtRom.Rows[i]["bed_id"].ToString(), roomName = dtRom.Rows[i]["room_name"].ToString(), roomQty = dtRom.Rows[i]["qty"].ToString() }); 
            }

            //listDiagnosaType.Clear();
            //listDiagnosaType.Add(new DiagnosaType() { diagnosaTypeCode = "P", diagnosaTypeName = "Primary" });
            //listDiagnosaType.Add(new DiagnosaType() { diagnosaTypeCode = "S", diagnosaTypeName = "Secondary" });

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

            ////dtGlMed.Clear();
            ////string sql_med = " select med_cd, initcap(med_name) || ' (BPJS: ' || bpjs_cover || ')' med_name from KLINIK.cs_medicine where status = 'A' and MED_GROUP ='OBAT' order by med_name ";
            ////OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
            ////OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_med, sqlConnect3);
            ////DataTable dt3 = new DataTable();
            ////dtGlMed = dt3;
            ////adSql3.Fill(dt3);
            ////listMedicine.Clear();
            ////for (int i = 0; i < dt3.Rows.Count; i++)
            ////{
            ////    listMedicine.Add(new Medicine() { medicineCode = dt3.Rows[i]["med_cd"].ToString(), medicineName = dt3.Rows[i]["med_name"].ToString() });
            ////}

            //listMedicineInfo.Clear();
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "A", medicineInfoName = "(P.C.) Sesudah Makan" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "B", medicineInfoName = "(A.C.) Sebelum Makan" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "C", medicineInfoName = "(P.R.N.) Bila Perlu" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "D", medicineInfoName = "(G.T.T) Diteteskan" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "E", medicineInfoName = "(U.E) Pemakaian Luar" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "F", medicineInfoName = "(D.C.) Pada Waktu Makan" });

            //listRacik.Clear();
            //listRacik.Add(new Racik() { RacikCode = "R1", RacikName = "Racik 1" });
            //listRacik.Add(new Racik() { RacikCode = "R2", RacikName = "Racik 2" });
            //listRacik.Add(new Racik() { RacikCode = "R3", RacikName = "Racik 3" });
            //listRacik.Add(new Racik() { RacikCode = "R4", RacikName = "Racik 4" });
            //listRacik.Add(new Racik() { RacikCode = "R5", RacikName = "Racik 5" });

            //listFormula.Clear();
            //listFormulaU.Clear();
            string sql_for = "";
            sql_for = sql_for + Environment.NewLine + "  select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1 and POLI_CD ='POL0001' and upper(att1) = decode(upper('" + type_s + "'), 'B', 'BPJS', 'A', 'ASURANSI', 'UMUM')   ";
         
            OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
            DataTable dtf = new DataTable();
            adOraf.Fill(dtf);
            listFormula2.Clear();
            //listFormulaU.Clear();
            for (int i = 0; i < dtf.Rows.Count; i++)
            {
                //listFormula.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                //listFormulaU.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
            }

            string sql_forU = "";
            sql_forU = sql_forU + Environment.NewLine + "  select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1 and POLI_CD ='POL0001' and att1 = 'UMUM'  ";
            //if(sstatus.ToString().Equals("BPJS"))
            //     sql_for = sql_for + Environment.NewLine + "and BPJS_COVER ='Y'";  

            OleDbConnection oraConnectU = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraU = new OleDbDataAdapter(sql_forU, oraConnectU);
            DataTable dtU = new DataTable();
            adOraU.Fill(dtU);
            //listFormula.Clear();
            //listFormula2.Clear();
            listFormulaU.Clear();
            for (int i = 0; i < dtU.Rows.Count; i++)
            {
                //listFormula.Add(new Formula() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                //listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                listFormulaU.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
            }
            //grpSkdUmum.Visible = true;
            //grpSkdUmum.Dock = DockStyle.Fill;
            //grpSkdKec.Visible = false;

            //string sql_period = " select periode from ( select 'a' as s, '' as periode from dual union select distinct 'b' as s, periode from KLINIK.cs_mcu) order by s asc ";
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

            //luObsRoom.Text = "RM001";
            //luObsRoom.Properties.DataSource = listRoom;
            //luObsRoom.Properties.ValueMember = "roomCode";
            //luObsRoom.Properties.DisplayMember = "roomName";

            //luObsRoom.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //luObsRoom.Properties.DropDownRows = listRoom.Count;
            //luObsRoom.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            //luObsRoom.Properties.AutoSearchColumnIndex = 1;
            //luObsRoom.Properties.NullText = "Select Room";

            //cmbPersetujuan.Items.Clear();
            //cmbPersetujuan.Items.Add("");
            //cmbPersetujuan.Items.Add("Setuju");
            //cmbPersetujuan.Items.Add("Tidak Setuju");

            listHours.Clear();
            listHours.Add(new Stat() { statCode = "0.5", statName = "30 Menit" });
            listHours.Add(new Stat() { statCode = "1", statName = "1 Jam" });
            listHours.Add(new Stat() { statCode = "1.5", statName = "1,5 Jam" });
            listHours.Add(new Stat() { statCode = "2", statName = "2 Jam" });
            listHours.Add(new Stat() { statCode = "2.5", statName = "2,5 Jam" });
            listHours.Add(new Stat() { statCode = "3", statName = "3 Jam" });

            //string sql_lay = " select treat_type_id trt_id, initcap(treat_type_name) trt_name from KLINIK.cs_treatment_type where 1=1 and treat_type_id = 'TRT01'  ";
            //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_lay, oraConnectf);
            //DataTable dtf = new DataTable();
            //adOraf.Fill(dtf);
            //listLaya.Clear();
            //for (int i = 0; i < dtf.Rows.Count; i++)
            //{
            //    listLaya.Add(new Layanan() { layananCode = dtf.Rows[i]["trt_id"].ToString(), layananName = dtf.Rows[i]["trt_name"].ToString() });
            //}

            //listLayanan.Clear();
            //listLayanan.Add(new Stat() { statCode = "OPN", statName = "Aktif" });
            //listLayanan.Add(new Stat() { statCode = "CLS", statName = "Selesai" });
            //listLayanan.Add(new Stat() { statCode = "CAN", statName = "Batal" });

            //string SQL = " ";
            //SQL = SQL + Environment.NewLine + " select treat_item_id, initcap(treat_item_name) treat_item_name ";
            //SQL = SQL + Environment.NewLine + "   from KLINIK.cs_treatment_item ";
            //SQL = SQL + Environment.NewLine + "  where 1=1 and treat_type_id = 'TRT01' and treat_group_id in ('TRG01','TRG07')  ";
            ////SQL = SQL + Environment.NewLine + "and (treat_type_id <> 'TRT01' or treat_type_id is null) ";
            ////SQL = SQL + Environment.NewLine + "and treat_group_id not in ('TRG02','TRG03','TRG05') ";

            //OleDbConnection oraConnectly = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adOraly = new OleDbDataAdapter(SQL, oraConnectly);
            //DataTable dtly = new DataTable();
            //adOraly.Fill(dtly);
            //listLaya2.Clear();
            //for (int i = 0; i < dtly.Rows.Count; i++)
            //{
            //    listLaya2.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
            //}
             
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

            string sql_racik = " select code_id, code_name from CS_CODE_DATA where code_class_id = 'MED_RACIK' order by SORT_ORDER ";
            OleDbConnection oraConRck = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraRck = new OleDbDataAdapter(sql_racik, oraConRck);
            DataTable dtRck = new DataTable();
            adOraRck.Fill(dtRck);
            listRacik.Clear();
            for (int i = 0; i < dtRck.Rows.Count; i++)
            {
                listRacik.Add(new Racik() { RacikCode = dtRck.Rows[i]["code_id"].ToString(), RacikName = dtRck.Rows[i]["code_name"].ToString() });
            }

            string sql_minfo = "";
            sql_minfo = " select code_id, code_name from CS_CODE_DATA where code_class_id = 'MED_USE' order by SORT_ORDER ";
            OleDbConnection oraConInfo = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraInfo = new OleDbDataAdapter(sql_minfo, oraConInfo);
            DataTable dtInfo = new DataTable();
            adOraInfo.Fill(dtInfo);
            listMedicineInfo.Clear();
            for (int i = 0; i < dtInfo.Rows.Count; i++)
            {
                listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = dtInfo.Rows[i]["code_id"].ToString(), medicineInfoName = dtInfo.Rows[i]["code_name"].ToString() });
            }  
        }
        #region Main
        private void FrmRawatInap_Load(object sender, EventArgs e)
        {
            loadDataAnamnesa();
            //LoadItemLayanan();
            InitData();
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "FrmRawatInap");
        }
        
        private void LoadItemLayanan()
        {
            string SQL = "";
            SQL = SQL + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) treat_item_name ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_treatment_item ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and treat_type_id = 'TRT02'  AND UPPER(TREAT_ITEM_NAME) NOT LIKE '%VISIT DOKTER%'  and TREAT_GROUP_ID not in('TRG07','TRG16') AND USED_BY IS NULL "; 

            OleDbConnection oraConnectly = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraly = new OleDbDataAdapter(SQL, oraConnectly);
            DataTable dtly = new DataTable();
            adOraly.Fill(dtly);
            listLaya2.Clear();
            for (int i = 0; i < dtly.Rows.Count; i++)
            {
                listLaya2.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
            }

            string SQL1 = "";
            SQL1 = SQL1 + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) treat_item_name ";
            SQL1 = SQL1 + Environment.NewLine + "from KLINIK.cs_treatment_item ";
            SQL1 = SQL1 + Environment.NewLine + "where 1=1 ";
            SQL1 = SQL1 + Environment.NewLine + "and treat_type_id = 'TRT02'  and TREAT_GROUP_ID ='TRG16' AND UPPER(TREAT_ITEM_NAME) NOT LIKE '%BIDAN%'  AND USED_BY IS NULL ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOrav = new OleDbDataAdapter(SQL1, oraConnect);
            DataTable dtlv = new DataTable();
            adOrav.Fill(dtlv);
            listLayav.Clear();
            for (int i = 0; i < dtlv.Rows.Count; i++)
            {
                listLayav.Add(new Layanan() { layananCode = dtlv.Rows[i]["treat_item_id"].ToString(), layananName = dtlv.Rows[i]["treat_item_name"].ToString() });
            }

            //dtGlMed.Clear();
            //string sql_med = " select med_cd, initcap(med_name) || ' (BPJS: ' || bpjs_cover || ')' med_name from KLINIK.cs_medicine where status = 'A'  and MED_GROUP ='OBAT' order by med_name ";
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

            //string SQL2 = "";
            //SQL2 = SQL2 + Environment.NewLine + "select ID_DOKTER, initcap(NM_DOKTER) Nama_Dokter ";
            //SQL2 = SQL2 + Environment.NewLine + "from KLINIK.CS_DOKTER ";
            //SQL2 = SQL2 + Environment.NewLine + "where 1=1  AND F_AKTIF ='Y' and NM_DOKTER <> 'System' ";
            ////SQL = SQL + Environment.NewLine + "and treat_type_id = 'TRT02'  ";

            //OleDbConnection oraConny = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adOra = new OleDbDataAdapter(SQL2, oraConny);
            //DataTable dtdok = new DataTable();
            //adOra.Fill(dtdok);
            //listDokter.Clear();
            //for (int i = 0; i < dtdok.Rows.Count; i++)
            //{
            //    listDokter.Add(new Dokter() { ID_Dokter = dtdok.Rows[i]["ID_DOKTER"].ToString(), Nama_Dokter = dtdok.Rows[i]["Nama_Dokter"].ToString() });
            //}

            //txDokterPengirim.Properties.DataSource = listDokter;
            //txDokterPengirim.Properties.ValueMember = "ID_Dokter";
            //txDokterPengirim.Properties.DisplayMember = "Nama_Dokter";

            //txDokterPengirim.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //txDokterPengirim.Properties.DropDownRows = listDokter.Count;
            //txDokterPengirim.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            //txDokterPengirim.Properties.AutoSearchColumnIndex = 1;
            //txDokterPengirim.Properties.NullText = "";
            //txDokterPengirim.ItemIndex = -1;

            //txDokterKonsultan.Properties.DataSource = listDokter;
            //txDokterKonsultan.Properties.ValueMember = "ID_Dokter";
            //txDokterKonsultan.Properties.DisplayMember = "Nama_Dokter";

            //txDokterKonsultan.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //txDokterKonsultan.Properties.DropDownRows = listDokter.Count;
            //txDokterKonsultan.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            //txDokterKonsultan.Properties.AutoSearchColumnIndex = 1;
            //txDokterKonsultan.Properties.NullText = "";
            //txDokterKonsultan.ItemIndex = -1;


            //string sql_dosis = " select code_id, code_name from CS_CODE_DATA where code_class_id = 'DOSIS' order by SORT_ORDER ";
            //OleDbConnection oraCondsd = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adOrados = new OleDbDataAdapter(sql_dosis, oraCondsd);
            //DataTable dtgsis = new DataTable();
            //adOrados.Fill(dtgsis);
            //listDosis.Clear();
            //for (int i = 0; i < dtgsis.Rows.Count; i++)
            //{
            //    listDosis.Add(new Dosis() { DosisCode = dtgsis.Rows[i]["code_id"].ToString(), DosisName = dtgsis.Rows[i]["code_name"].ToString() });
            //}

            //string SQL3 = "";
            //SQL3 = "";
            //SQL3 = SQL3 + Environment.NewLine + "select bed_id, room_name || substr(bed_id,-3) room_name, decode(b.use_yn,'N','1','0') qty ";
            //SQL3 = SQL3 + Environment.NewLine + "from cs_room a ";
            //SQL3 = SQL3 + Environment.NewLine + "join cs_bed b on (a.room_id=b.room_id) ";
            //SQL3 = SQL3 + Environment.NewLine + "join cs_room_class c on (a.class_id=c.class_id) ";
            //SQL3 = SQL3 + Environment.NewLine + "where 1=1 ";
            ////SQL3 = SQL3 + Environment.NewLine + "and c.class_id=3 ";
            ////SQL3 = SQL3 + Environment.NewLine + "and b.use_yn='N' ";

            //OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adSql = new OleDbDataAdapter(SQL3, sqlConnect);
            //DataTable dt = new DataTable();
            //adSql.Fill(dt);
            //listRoom.Clear();
            //listRoom.Add(new Room() { roomCode = "", roomName = "Pilih" });
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    listRoom.Add(new Room() { roomCode = dt.Rows[i]["bed_id"].ToString(), roomName = dt.Rows[i]["room_name"].ToString(), roomQty = dt.Rows[i]["qty"].ToString() });
            //    //poli.poliCode = dt2.Rows[i]["poli_cd"].ToString();
            //    //poli.poliName = dt2.Rows[i]["poli_name"].ToString();
            //    //listPoli.Add(poli);
            //}

            listMedicineInfo.Clear();
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "A", medicineInfoName = "(P.C.) Sesudah Makan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "B", medicineInfoName = "(A.C.) Sebelum Makan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "C", medicineInfoName = "(P.R.N.) Bila Perlu" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "D", medicineInfoName = "(G.T.T) Diteteskan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "E", medicineInfoName = "(U.E) Pemakaian Luar" });

            //dtGlDiag.Clear();
            //string sql_poli = " select item_cd, initcap(item_name) item_name from KLINIK.cs_diagnosa_item where status = 'A' order by item_name ";
            //OleDbConnection sqlCon1 = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adSqlc2 = new OleDbDataAdapter(sql_poli, sqlCon1);
            //DataTable dtd = new DataTable();
            //adSqlc2.Fill(dtd);
            //dtGlDiag = dtd;
            //listDiagnosa.Clear();
            //for (int i = 0; i < dtd.Rows.Count; i++)
            //{
            //    listDiagnosa.Add(new Diagnosa() { diagnosaCode = dtd.Rows[i]["item_cd"].ToString(), diagnosaName = dtd.Rows[i]["item_name"].ToString() });
            //}

           
            //txDiagnosaAkhir.Properties.DataSource = listDiagnosa;
            //txDiagnosaAkhir.Properties.ValueMember = "diagnosaCode";
            //txDiagnosaAkhir.Properties.DisplayMember = "diagnosaName";

            //txDiagnosaAkhir.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //txDiagnosaAkhir.Properties.DropDownRows = listDiagnosa.Count;
            //txDiagnosaAkhir.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            //txDiagnosaAkhir.Properties.AutoSearchColumnIndex = 0;
            //txDiagnosaAkhir.Properties.NullText = "";
            //txDiagnosaAkhir.ItemIndex = -1;

            //mmDokter.Properties.DataSource = listDiagnosa;
            //mmDokter.Properties.ValueMember = "diagnosaCode";
            //mmDokter.Properties.DisplayMember = "diagnosaName";

            //mmDokter.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //mmDokter.Properties.DropDownRows = listDiagnosa.Count;
            //mmDokter.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            //mmDokter.Properties.AutoSearchColumnIndex = 0;
            //mmDokter.Properties.NullText = "";
            //mmDokter.ItemIndex = -1;

        }
        private void LoadItemLayananType(string type_status)
        {
            string SQL = " ";
            SQL = SQL + Environment.NewLine + "select treat_item_id, initcap(treat_item_name)||case when USED_BY ='NUR' then ' [NUR]' else '' end  treat_item_name ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_treatment_item ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and treat_type_id = 'TRT02'  AND UPPER(TREAT_ITEM_NAME) NOT LIKE '%VISIT DOKTER%'  and TREAT_GROUP_ID not in('TRG03','TRG07','TRG16') ";
            SQL = SQL + Environment.NewLine + "and f_status  = '" + type_status + "'  ";

            OleDbConnection oraConnectly = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraly = new OleDbDataAdapter(SQL, oraConnectly);
            DataTable dtly = new DataTable();
            adOraly.Fill(dtly);
            listLaya2.Clear();
            for (int i = 0; i < dtly.Rows.Count; i++)
            {
                listLaya2.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
            }

            if(type_s.ToString().Equals("B"))
            {
                SQL = " ";
                SQL = SQL + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) ||case when USED_BY ='NUR' then ' [NUR]' else '' end treat_item_name ";
                SQL = SQL + Environment.NewLine + "from KLINIK.cs_treatment_item ";
                SQL = SQL + Environment.NewLine + "where 1=1 ";
                SQL = SQL + Environment.NewLine + "and treat_type_id = 'TRT02'  AND UPPER(TREAT_ITEM_NAME) NOT LIKE '%VISIT DOKTER%'  and TREAT_GROUP_ID not in('TRG03','TRG07','TRG16') ";
                SQL = SQL + Environment.NewLine + "and f_status  = 'U'  ";

                OleDbConnection oraConnectU = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOraU = new OleDbDataAdapter(SQL, oraConnectU);
                DataTable dtU = new DataTable();
                adOraU.Fill(dtU);
                listLayaU.Clear();
                for (int i = 0; i < dtU.Rows.Count; i++)
                {
                    listLayaU.Add(new Layanan() { layananCode = dtU.Rows[i]["treat_item_id"].ToString(), layananName = dtU.Rows[i]["treat_item_name"].ToString() });
                }
            }

            string SQL1 = "";
            SQL1 = SQL1 + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) treat_item_name ";
            SQL1 = SQL1 + Environment.NewLine + "  from KLINIK.cs_treatment_item ";
            SQL1 = SQL1 + Environment.NewLine + " where 1=1 ";
            SQL1 = SQL1 + Environment.NewLine + "   and treat_type_id = 'TRT02'  AND  TREAT_GROUP_ID = 'TRG16'  ";
            SQL1 = SQL1 + Environment.NewLine + "   and f_status  = '" + type_status + "'   ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOrav = new OleDbDataAdapter(SQL1, oraConnect);
            DataTable dtlv = new DataTable();
            adOrav.Fill(dtlv);
            listLayav.Clear();
            for (int i = 0; i < dtlv.Rows.Count; i++)
            {
                listLayav.Add(new Layanan() { layananCode = dtlv.Rows[i]["treat_item_id"].ToString(), layananName = dtlv.Rows[i]["treat_item_name"].ToString() });
            }
             
            //listMedicineInfo.Clear();
            //listMedicineInfo.Add("(P.C.) Sesudah Makan");
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "B", medicineInfoName = "(A.C.) Sebelum Makan" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "C", medicineInfoName = "(P.R.N.) Bila Perlu" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "D", medicineInfoName = "(G.T.T) Diteteskan" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "E", medicineInfoName = "(U.E) Pemakaian Luar" });
        }

        private void DataListObat(string sstatus )
        {
            dtGlMed.Clear(); dtGlMedP.Clear(); dtGlMedRacik.Clear();
            string sql_med = " ", sql_racik = "", sql_medR = "";
            sql_med = sql_med + Environment.NewLine + " select b.med_cd, initcap(med_name) med_name  ";
            sql_med = sql_med + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1    ";
            sql_med = sql_med + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y' and upper(att1) in (decode(upper('" + sstatus + "'), 'B', 'BPJS', 'A', 'ASURANSI', 'UMUM') ,'ALL')  ";
            sql_med = sql_med + Environment.NewLine + "    and POLI_CD = 'POL0001'   "; 
            sql_med = sql_med + Environment.NewLine + "  order by med_name  ";

            OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_med, sqlConnect3);
            DataTable dt3 = new DataTable();
            dtGlMed = dt3;  dtGlMedP = dt3;
            adSql3.Fill(dt3);
            listMedicine.Clear();
            listMedicineP.Clear();
            listMedicineRacik.Clear();
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                listMedicine.Add(new Medicine() { medicineCode = dt3.Rows[i]["med_cd"].ToString(), medicineName = dt3.Rows[i]["med_name"].ToString() });
                listMedicineP.Add(new Medicine() { medicineCode = dt3.Rows[i]["med_cd"].ToString(), medicineName = dt3.Rows[i]["med_name"].ToString() });
                listMedicineRacik.Add(new Medicine() { medicineCode = dt3.Rows[i]["med_cd"].ToString(), medicineName = dt3.Rows[i]["med_name"].ToString() });
            }
            
            //if (sstatus.ToString().Equals("B"))
            //{
                dtGlMedU.Clear();
                sql_med = "";
                sql_med = sql_med + Environment.NewLine + " select b.med_cd, initcap(med_name) med_name  ";
                sql_med = sql_med + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1    ";
                sql_med = sql_med + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y' and att1 ='UMUM'  ";
                sql_med = sql_med + Environment.NewLine + "    and POLI_CD  = 'POL0001'  ";
                sql_med = sql_med + Environment.NewLine + "  order by med_name  ";

                OleDbConnection sqlConnectU = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSqlU = new OleDbDataAdapter(sql_med, sqlConnectU);
                DataTable dtU = new DataTable();
                dtGlMedU = dtU;
                adSqlU.Fill(dtU);
                listMedicineU.Clear();
                for (int i = 0; i < dtU.Rows.Count; i++)
                {
                    listMedicineU.Add(new Medicine() { medicineCode = dtU.Rows[i]["med_cd"].ToString(), medicineName = dtU.Rows[i]["med_name"].ToString() });
                } 

                dtGlMedRacik.Clear();
                sql_medR = "";
                sql_medR = sql_medR + Environment.NewLine + " select b.med_cd, initcap(med_name) || ' (BPJS: Y)' med_name   ";
                sql_medR = sql_medR + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1     ";
                sql_medR = sql_medR + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  and att1 = 'BPJS' ";
                sql_medR = sql_medR + Environment.NewLine + "    and POLI_CD ='POL0001'    ";
                sql_medR = sql_medR + Environment.NewLine + "  UNION ALL ";
                sql_medR = sql_medR + Environment.NewLine + " select b.med_cd, initcap(med_name) || ' (BPJS: N)' med_name   ";
                sql_medR = sql_medR + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1     ";
                sql_medR = sql_medR + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  and att1 in('UMUM','ALL') ";
                sql_medR = sql_medR + Environment.NewLine + "    and POLI_CD = 'POL0001'    ";
                sql_medR = sql_medR + Environment.NewLine + "    and b.med_cd not in ( select b.med_cd  ";
                sql_medR = sql_medR + Environment.NewLine + "                           from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1     ";
                sql_medR = sql_medR + Environment.NewLine + "                            and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  and att1 = 'BPJS' ";
                sql_medR = sql_medR + Environment.NewLine + "                            and POLI_CD ='POL0001'  ";
                sql_medR = sql_medR + Environment.NewLine + "                        ) ";
                sql_medR = sql_medR + Environment.NewLine + "  order by med_name "; 

                OleDbConnection sqlConnectR = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSqlR = new OleDbDataAdapter(sql_medR, sqlConnectR);
                DataTable dtR = new DataTable();
                dtGlMedRacik = dtR;
                adSqlR.Fill(dtR);
                listMedicineRacik.Clear();
                for (int i = 0; i < dtR.Rows.Count; i++)
                {
                    listMedicineRacik.Add(new Medicine() { medicineCode = dtR.Rows[i]["med_cd"].ToString(), medicineName = dtR.Rows[i]["med_name"].ToString() });
                }
            //}
        }

        private void DataListObatGroup(string sstatus)
        {
            dtGlMed.Clear(); dtGlMedP.Clear(); dtGlMedRacik.Clear();
            string sql_med = " ", sql_racik = "", sql_medR = "";
            sql_med = sql_med + Environment.NewLine + " select a.att2 Kategori, b.med_cd Kode_Obat, initcap(med_name) ||' ['||a.FORMULA||']' Nama_Obat  ";
            sql_med = sql_med + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1    ";
            sql_med = sql_med + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y' and upper(att1) in (decode(upper('" + sstatus + "'), 'B', 'BPJS', 'A', 'ASURANSI', 'UMUM') ,'ALL')  ";
            sql_med = sql_med + Environment.NewLine + "    and POLI_CD = 'POL0001'  AND RACIKAN ='N'  ";
            sql_med = sql_med + Environment.NewLine + "  order by a.att2, b.med_name    ";

            OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_med, sqlConnect3);
            DataTable dt3 = new DataTable();
            dtGlMed = dt3; dtGlMedP = dt3;
            adSql3.Fill(dt3);
            lMedicine.Clear();
            lMedicineP.Clear();
            lMedicineRacik.Clear();
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                lMedicine.Add(new MedGroup() { Kategori = dt3.Rows[i]["Kategori"].ToString(), Kode_Obat = dt3.Rows[i]["Kode_Obat"].ToString(), Nama_Obat = dt3.Rows[i]["Nama_Obat"].ToString() });
                lMedicineP.Add(new MedGroup() { Kategori = dt3.Rows[i]["Kategori"].ToString(), Kode_Obat = dt3.Rows[i]["Kode_Obat"].ToString(), Nama_Obat = dt3.Rows[i]["Nama_Obat"].ToString() });
                //lMedicineRacik.Add(new MedGroup() { Kategori = dt3.Rows[i]["Kategori"].ToString(), Kode_Obat = dt3.Rows[i]["Kode_Obat"].ToString(), Nama_Obat = dt3.Rows[i]["Nama_Obat"].ToString() });
            }

            //if (sstatus.ToString().Equals("B"))
            //{
            dtGlMedU.Clear();
            sql_med = "";
            sql_med = sql_med + Environment.NewLine + " select a.att2 Kategori, b.med_cd Kode_Obat, initcap(med_name) ||' ['||a.FORMULA||']' Nama_Obat   ";
            sql_med = sql_med + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1    ";
            sql_med = sql_med + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y' and att1 ='UMUM'  ";
            sql_med = sql_med + Environment.NewLine + "    and POLI_CD  = 'POL0001' AND RACIKAN ='N' ";
            sql_med = sql_med + Environment.NewLine + "  order by a.att2, b.med_name    ";

            OleDbConnection sqlConnectU = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSqlU = new OleDbDataAdapter(sql_med, sqlConnectU);
            DataTable dtU = new DataTable();
            dtGlMedU = dtU;
            adSqlU.Fill(dtU);
            lMedicineU.Clear();
            for (int i = 0; i < dtU.Rows.Count; i++)
            {
                lMedicineU.Add(new MedGroup() { Kategori = dtU.Rows[i]["Kategori"].ToString(), Kode_Obat = dtU.Rows[i]["Kode_Obat"].ToString(), Nama_Obat = dtU.Rows[i]["Nama_Obat"].ToString() });
            }

            dtGlMedRacik.Clear();
            sql_medR = "";
            sql_medR = sql_medR + Environment.NewLine + " select a.att2 Kategori,  b.med_cd Kode_Obat, initcap(med_name) ||' ['||a.FORMULA||']' || decode(att1,'BPJS','',' [None BPJS]') Nama_Obat    ";
            sql_medR = sql_medR + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1     ";
            sql_medR = sql_medR + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  and att1 in('BPJS' ,'UMUM')  ";
            sql_medR = sql_medR + Environment.NewLine + "    and POLI_CD ='POL0001' AND RACIKAN ='Y'   ";
            //sql_medR = sql_medR + Environment.NewLine + "  UNION ALL ";
            //sql_medR = sql_medR + Environment.NewLine + " select a.att2 Kategori,  b.med_cd Kode_Obat, initcap(med_name) ||' ['||a.FORMULA||']' || ' [None BPJS]' Nama_Obat   ";
            //sql_medR = sql_medR + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1     ";
            //sql_medR = sql_medR + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  and att1 in('UMUM','ALL') ";
            //sql_medR = sql_medR + Environment.NewLine + "    and POLI_CD = 'POL0001'  AND RACIKAN ='Y'  ";
            //sql_medR = sql_medR + Environment.NewLine + "    and b.med_cd not in ( select b.med_cd  ";
            //sql_medR = sql_medR + Environment.NewLine + "                           from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1     ";
            //sql_medR = sql_medR + Environment.NewLine + "                            and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  and att1 = 'BPJS' ";
            //sql_medR = sql_medR + Environment.NewLine + "                            and POLI_CD ='POL0001'  ";
            //sql_medR = sql_medR + Environment.NewLine + "                        ) ";
            sql_medR = sql_medR + Environment.NewLine + "  order by 1,3   ";

            OleDbConnection sqlConnectR = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSqlR = new OleDbDataAdapter(sql_medR, sqlConnectR);
            DataTable dtR = new DataTable();
            dtGlMedRacik = dtR;
            adSqlR.Fill(dtR);
            lMedicineRacik.Clear();
            for (int i = 0; i < dtR.Rows.Count; i++)
            {
                lMedicineRacik.Add(new MedGroup() { Kategori = dtR.Rows[i]["Kategori"].ToString(), Kode_Obat = dtR.Rows[i]["Kode_Obat"].ToString(), Nama_Obat = dtR.Rows[i]["Nama_Obat"].ToString() });
            }
            //}
        }
        private void loadDataAnamnesa()
        {
            string sql = @"SELECT  DISTINCT A.ANAMNESA_ID,
                                   A.RM_NO,
                                   B.PATIENT_NO,
                                   to_char(A.INSP_DATE,'yyyy-MM-dd') INSP_DATE,
                                   initcap(C.NAME) NAME,
                                   DECODE(D.TYPE_PATIENT, 'U', 'Umum','B','BPJS','Swasta') GROUP_PATIENT,
                                   case when D.STATUS ='PAY' then 'Pembayaran' ELSE DECODE(d.STATUS,'OPN', 'Proses','NUR', 'Proses','INP', 'Proses', 'REG', 'Registrasi','CLS','Selesai','DON', 'Persiapan Pulang', 'PAY','Pembayaran','Batal') END STATUS,
                                   nvl((select z.name from KLINIK.cs_guarantor z where z.patient_no=c.patient_no and rownum =1 ),  C.FAMILY_HEAD)  FAMILY_HEAD, A.ID_VISIT, E.HEAD_ID, (select ROOM_NAME||' ['||substr(f.room_id,-2)||']' from CS_ROOM g, CS_BED h where g.room_id = h.room_id  and h.BED_ID = f.room_id ) room_id, f.inpatient_id
                              FROM CS_ANAMNESA A, CS_PATIENT B, CS_PATIENT_INFO C, CS_VISIT D, CS_TREATMENT_HEAD E, KLINIK.cs_inpatient F
                              WHERE A.ID_VISIT = D.ID_VISIT AND D.inpatient_id=f.inpatient_id
                                and d.ID_VISIT = E.ID_VISIT
                                AND D.STATUS not in ('CLS','CAN') 
                                AND B.PATIENT_NO = D.PATIENT_NO and anamnesa is not null
                                AND B.PATIENT_NO = C.PATIENT_NO AND d.POLI_CD ='POL0004' and d.plan = 'TRT02' and d.purpose ='DOC' order by  to_char(A.INSP_DATE,'yyyy-MM-dd') desc, initcap(C.NAME) ";

            //grdMain.DataSource = ORADB.SetData(ORADB.XE, sql); 

            grdMain.DataSource = ConnOra.Data_Table_ora(sql);
            gvwMain.BestFitColumns(); 
            //RepositoryItemLookUpEdit roomLookup = new RepositoryItemLookUpEdit();
            //roomLookup.DataSource = listRoom;
            //roomLookup.ValueMember = "roomCode";
            //roomLookup.DisplayMember = "roomName";

            //roomLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //roomLookup.DropDownRows = listRoom.Count;
            //roomLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            //roomLookup.AutoSearchColumnIndex = 1;
            //roomLookup.NullText = "";
            //gvwMain.Columns[10].ColumnEdit = roomLookup;
        }
        private void InitializeLookupEdit()
        {
            var beratbadan = new List<FN.LookupData>
            {
                new FN.LookupData("", ""),
                new FN.LookupData("1", "1-5 Kg"),
                new FN.LookupData("2", "6-10 Kg"),
                new FN.LookupData("3", "11-15 Kg"),
                new FN.LookupData("4", ">15 Kg"),
                new FN.LookupData("5", "Tidak yakin penurunannya")
            };

            lebrtbadan.Properties.DataSource = beratbadan;
            lebrtbadan.Properties.DisplayMember = "Display";
            lebrtbadan.Properties.ValueMember = "Value";
            lebrtbadan.EditValue = "";
        }

        private void getData(string id)
        {
            try
            {
               
                string Sql = "";
                Sql = Sql + Environment.NewLine + "  select to_char(insp_date,'yyyy-mm-dd') as insp_date, '" + fnama + "' as nama, visit_no,  ";
                Sql = Sql + Environment.NewLine + "         blood_press, pulse, temperature, allergy, anamnesa, info_k, 'U' action, rm_no, a.bb, a.tb, disease_now ,ID_VISIT, ";
                Sql = Sql + Environment.NewLine + "         nvl(KELUHAN_UTAMA,disease_now) KELUHAN_UTAMA, 	nvl(PENYAKIT_LALU,DISEASE_THEN) PENYAKIT_LALU,	PERNAH_DIRAWAT,	PERNAH_OPERASI,	nvl(PENYAKIT_KELUARGA,DISEASE_FAMILY) PENYAKIT_KELUARGA,	 ";
                Sql = Sql + Environment.NewLine + "         TERGANTUNG_THD,	RIWAYAT_PEKERJAAN,	RIWAYAT_ALERGI,	RIWAYAT_OBAT,	nvl(TD,blood_press) TD,	nvl(NADI,pulse) NADI,	P,	nvl(SUHU,temperature) SUHU,	KELUHAN,	BATAS_MAKAN,	GIGI_PALSU,	MUAL,	MUNTAH,	nvl(a.BB,b.bb) BB,	nvl(a.TB,b.tb) TB,round((to_number(nvl(a.BB,b.bb),'9,999.9')/(to_number(nvl(a.TB,b.tb),'9,999.9')*	to_number(nvl(a.TB,b.TB),'9,999.9')))* 10000,2) IMT,					 ";
                Sql = Sql + Environment.NewLine + "         nvl(GST_KET,KLINIK.CS_IMT(to_number(nvl(a.BB,b.bb),'9,999.9'), to_number(nvl(a.TB,b.tb),'9,999.9'))) GST_KET,	PENDENGARAN,	PENGLIHATAN,	DEFEKASI,	MIKSI,	KULIT,	SKOR_NORTON,	RESIKO_DEKUBITUS,	LOKASI_LUKA,	PERIKSA_FISIK_LAIN,	FORM_PERIKSA_KHUSUS,	STATUS_PSIKOLOGI,	STATUS_MENTAL,	HUBUNGAN_KELUARGA,	TEMPAT_TINGGAL,	NAMA_KERABAT,	HUB_KERABAT,	TLP_KERABAT,	KEG_AGAMA,	KEG_SPIRITUAL,	HAMBATAN_BELAJAR,	BUTUH_PENERJEMAH,	KEBUTUHAN_EDUKASI,	BERSEDIA_DIKUNJUNGI,	RESIKO_CEDERA,	MENERIMA_INFO, a.VITALRR ";
                Sql = Sql + Environment.NewLine + "   from  cs_anamnesa a, T1_RAWAT_INAP1 b  ";
                Sql = Sql + Environment.NewLine + "  where a.ANAMNESA_ID = b.ANAMESA_ID  "; 
                Sql = Sql + Environment.NewLine + "    and  a.ANAMNESA_ID = " + id + "  ";

                string Sql1 = " ";
                Sql1 = Sql1 + Environment.NewLine + "select to_char(insp_date,'yyyy-mm-dd') as insp_date, '" + fnama + "' as nama, visit_no,  ";
                Sql1 = Sql1 + Environment.NewLine + "         blood_press, pulse, temperature, allergy, anamnesa, info_k, 'U' action, rm_no, a.bb, a.tb, disease_now ,ID_VISIT ";
                Sql1 = Sql1 + Environment.NewLine + "         ,b.* ";
                Sql1 = Sql1 + Environment.NewLine + "   from  cs_anamnesa a, T1_RAWAT_INAP2 b ";
                Sql1 = Sql1 + Environment.NewLine + "  where  a.ANAMNESA_ID = b.ANAMESA_ID ";
                Sql1 = Sql1 + Environment.NewLine + "    and  a.ANAMNESA_ID = " + id + "  ";

                DataTable dt1 = ConnOra.Data_Table_ora(Sql);
                DataTable dt2 = ConnOra.Data_Table_ora(Sql1);
                //DataTable dt1 = ORADB.SetData(ORADB.XE, "select * from T1_RAWAT_INAP1 where anamesa_id = " + id + " ");
                //DataTable dt2 = ORADB.SetData(ORADB.XE, "select * from T1_RAWAT_INAP2 where anamesa_id = " + id + " ");
                if(dt1.Rows.Count > 0)
                {

                    mmKeluhan.Text = FN.rowVal(dt1, "ANAMNESA");
                    //FN.splitVal1(FN.rowVal(dt1, "KELUHAN_UTAMA"), rgSakitLalu, mmKeluhan);
                    FN.splitVal1(FN.rowVal(dt1, "PENYAKIT_LALU"),rgSakitLalu, txSakitLalu);
                    FN.splitVal5(FN.rowVal(dt1, "PERNAH_DIRAWAT"),rgPernahRawat, txDiagnosa, txKapanRawat, txRawatDi);
                    FN.splitVal1(FN.rowVal(dt1, "PERNAH_OPERASI"), rgPrnhOperasi, txJnsOperasi);
                    FN.splitVal4(FN.rowVal(dt1, "PENYAKIT_KELUARGA"),gbRwSakitKlrg, rgRwSktKlrg, txSakitKlrga); 
                    FN.splitVal4(FN.rowVal(dt1, "TERGANTUNG_THD"),gbTergantungThdp, rgKetergantungan, txketergantungan);
                    FN.splitVal1(FN.rowVal(dt1, "RIWAYAT_PEKERJAAN"), rgRiwayatKerja, txRwytKerja);
                    FN.splitVal4(FN.rowVal(dt1, "RIWAYAT_ALERGI"), gbRwAlergi, rgAlergi, txAlergi);
                    FN.splitVal1(FN.rowVal(dt1, "RIWAYAT_OBAT"), rgRiwayatObat, txRiwayatObat);
                    txTd.Text = FN.rowVal(dt1, "TD");
                    txNadi.Text = FN.rowVal(dt1, "NADI");
                    txP.Text = FN.rowVal(dt1, "VITALRR");
                    txSuhu.Text = FN.rowVal(dt1, "SUHU");
                    FN.splitVal1(FN.rowVal(dt1, "KELUHAN"), rgKeluhan, txKeluhan);
                    txBtsMakan.Text = FN.rowVal(dt1, "BATAS_MAKAN");
                    FN.splitVal(FN.rowVal(dt1, "GIGI_PALSU"), rgGigiPalsu);
                    FN.splitVal(FN.rowVal(dt1, "MUAL"), rgMual);
                    FN.splitVal(FN.rowVal(dt1, "MUNTAH"), rgMuntah);
                    txBB.Text = FN.rowVal(dt1, "BB");
                    txTbPb.Text = FN.rowVal(dt1, "TB");
                    //txImt.Text = FN.rowVal(dt1, "IMT");
                    //txGstKet.Text = FN.rowVal(dt1, "GST_KET");
                    FN.splitVal1(FN.rowVal(dt1, "PENDENGARAN"), rgPendengaran, txPdngrDtl);
                    FN.splitVal1(FN.rowVal(dt1, "PENGLIHATAN"), rgPenglihatan, txPnglihtDtl);
                    FN.splitVal1(FN.rowVal(dt1, "DEFEKASI"), rgDefekasi, txDefekasiDtl);
                    FN.splitVal1(FN.rowVal(dt1, "MIKSI"), rgMiksi, txMiksiDtl);
                    FN.splitVal1(FN.rowVal(dt1, "KULIT"), rgKulit, txKulitDtl);
                    txSkorNorton.Text = FN.rowVal(dt1, "SKOR_NORTON");
                    FN.splitVal(FN.rowVal(dt1, "RESIKO_DEKUBITUS"), rbDekubitus);
                    FN.setCheckList(FN.rowVal(dt1, "LOKASI_LUKA"), ckLokasiLuka);
                    txPeriksaFisik.Text = FN.rowVal(dt1, "PERIKSA_FISIK_LAIN");
                    FN.splitVal(FN.rowVal(dt1, "FORM_PERIKSA_KHUSUS"), rgPeriksaKhusus);
                    FN.splitVal2(FN.rowVal(dt1, "STATUS_PSIKOLOGI"),gbStsPsikologi, txStsPsikologi);
                    setStsMental(FN.rowVal(dt1, "STATUS_MENTAL"));
                    FN.splitVal(FN.rowVal(dt1, "HUBUNGAN_KELUARGA"), rgHubKluarga);
                    FN.splitVal1(FN.rowVal(dt1, "TEMPAT_TINGGAL"), rgTmpTinggal, txTpTinggalDtl);
                    txNmKerabat.Text = FN.rowVal(dt1, "NAMA_KERABAT");
                    txHubKerabat.Text = FN.rowVal(dt1, "HUB_KERABAT");
                    txTlpKerabat.Text = FN.rowVal(dt1, "TLP_KERABAT");
                    txkegAgama.Text = FN.rowVal(dt1, "KEG_AGAMA");
                    txkegSpirit.Text = FN.rowVal(dt1, "KEG_SPIRITUAL");
                    FN.splitVal4(FN.rowVal(dt1, "HAMBATAN_BELAJAR"), gbHambatanBljr, rgHmbtanBljr, txHmbtan);
                    FN.splitVal1(FN.rowVal(dt1, "BUTUH_PENERJEMAH"), rgPnrjemah, txPnrjmhDtl);
                    FN.splitVal2(FN.rowVal(dt1, "KEBUTUHAN_EDUKASI"), pnlKbthnEdukasi, txKbthnEdukasi);
                    FN.splitVal4(FN.rowVal(dt1, "BERSEDIA_DIKUNJUNGI"), pnlSedia, rgSedia, txSedia);
                    FN.splitVal(FN.rowVal(dt1, "RESIKO_CEDERA"), rgResikoCedera);
                    FN.splitVal(FN.rowVal(dt1, "MENERIMA_INFO"), rgMnrimaInfo);
                }

                if(dt2.Rows.Count > 0)
                {
                    FN.splitVal1(FN.rowVal(dt2, "MOBILISASI"), rgMobilisasi, txMobilisasiDtl);
                    txAltBantujalan.Text = FN.rowVal(dt2, "ALAT_BANTU_JALAN");
                    FN.splitVal(FN.rowVal(dt2, "NYERI"), rgNyeri);
                    FN.setCheckList(FN.rowVal(dt2, "SKALA_NYERI"), chkSkalaNyeri);
                    FN.splitVal(FN.rowVal(dt2, "TINGKAT_NYERI"), rgTingkatNyeri);
                    txLokasiNyeri.Text = FN.rowVal(dt2, "LOKASI_NYERI");
                    txFrekuensi.Text = FN.rowVal(dt2, "FREKUENSI_NYERI");
                    txDurasiNyeri.Text = FN.rowVal(dt2, "DURASI_NYERI");
                    txScorNyeri.Text = FN.rowVal(dt2, "SKOR_NYERI");
                    FN.splitVal2(FN.rowVal(dt2, "NYERI_HILANG"), gbNyeriHilang, txNyeriHilang);
                    checkTurunBB(FN.rowVal(dt2, "TURUN_BERAT_BADAN"), rgTurunBB, lebrtbadan);
                    FN.splitVal(FN.rowVal(dt2, "KURANG_ASUPAN_MAKAN"), rgAsupanMakan);
                    FN.splitVal4(FN.rowVal(dt2, "DIAGNOSE_KHUSUS"), pnlDiagnoseKhusus, rgDiagnoseKh, txDiagnoseDtl);
                    FN.splitVal1(FN.rowVal(dt2, "LAPOR_TIM_TRGZ"),rgLapor_tr_Gizi, txLaporDtl);
                    mmPerawat.Text = FN.rowVal(dt2, "MSLH_PERAWAT");
                    mmDokter.Text = FN.rowVal(dt2, "MSLH_DOKTER");
                    mmTujuanTerukur.Text = FN.rowVal(dt2, "TUJUAN_TERUKUR");

                    string YN = FN.rowVal(dt2, "SUSUN_RENCANA_PERAWAT");
                    bool chk = YN == "Y" ? chkSusunRencana.Checked = true : chkSusunRencana.Checked=false;
                }
                DataTable dt3 = ConnOra.Data_Table_ora("select * from T1_RESIKO_JATUH_ANAK where anamesa_id = " + anamesaID + " ");
                //DataTable dt3 = ORADB.SetData(ORADB.XE, "select * from T1_RESIKO_JATUH_ANAK where anamesa_id = " + anamesaID + " ");
                if (dt3.Rows.Count > 0)
                {
                    FN.splitVal(FN.rowVal(dt3, "USIA"), rgUsia);
                    FN.splitVal(FN.rowVal(dt3, "JENIS_KELAMIN"), rgJenkel);
                    FN.splitVal(FN.rowVal(dt3, "DIAGNOSIS"), rgDiagnosis);
                    FN.splitVal(FN.rowVal(dt3, "GANGGUAN_FN_KOGNITIF"), rgGangguan);
                    FN.splitVal(FN.rowVal(dt3, "FAKTOR_LINGKUNGAN"), rgFlingkungan);
                    FN.splitVal(FN.rowVal(dt3, "SEDASI_ANESTESI"), rgSedasiAnestesi);
                    FN.splitVal(FN.rowVal(dt3, "PENGGUNAAN_OBAT"), rguseObat);
                    txScoreAnak.Text = FN.rowVal(dt3, "SKOR");
                    txResikoAnak.Text = FN.rowVal(dt3, "RESIKO");
                }
                DataTable dt4 = ConnOra.Data_Table_ora("select * from T1_RESIKO_JATUH_DEWASA where anamesa_id = " + anamesaID + " ");
                //DataTable dt4 = ORADB.SetData(ORADB.XE, "select * from T1_RESIKO_JATUH_DEWASA where anamesa_id = " + anamesaID + " ");
                if (dt4.Rows.Count > 0)
                {
                    FN.splitVal1(FN.rowVal(dt4, "RIWAYAT_JATUH"), rgRiwayatJatuh, txKet1);
                    FN.splitVal1(FN.rowVal(dt4, "MEMILIKI_LBH_PENYAKIT"), rgDiagnosaSekunder, txKet2);
                    FN.splitVal1(FN.rowVal(dt4, "ALAT_BANTU_JALAN"), rgAltBantuJalan, txKet3);
                    FN.splitVal1(FN.rowVal(dt4, "TERPASANG_INFUS"), rgInfus, txket4);
                    FN.splitVal1(FN.rowVal(dt4, "GAYA_BERJALAN"), rgGayaJalan, txKet5);
                    FN.splitVal1(FN.rowVal(dt4, "STATUS_MENTAL"), rgstsMental, txket6);
                    txTotalNilai.Text = FN.rowVal(dt4, "TOTAL_NILAI");
                    txResikoDewasa.Text = FN.rowVal(dt4, "RESIKO");
                    lblTindakan.Text = FN.rowVal(dt4, "TINDAKAN");
                }
                DataTable dt5 = ConnOra.Data_Table_ora("select * from T1_PERENCANAAN_PULANG where anamesa_id = " + anamesaID + " ");
                //DataTable dt5 = ORADB.SetData(ORADB.XE, "select * from T1_PERENCANAAN_PULANG where anamesa_id = " + anamesaID + " ");
                if (dt5.Rows.Count > 0)
                {
                    DateTime dte;
                    if (DateTime.TryParseExact(FN.rowVal(dt5, "tanggal_keluar"), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dte))
                        dtkeluar.EditValue = dte;

                    cbKeadaanPulang.SelectedItem = FN.rowVal(dt5, "keadaan_pulang");
                    txKeadaanDtl.Text = FN.rowVal(dt5, "keadaan_pulang_dtl");
                    cbAlatTerpasang.SelectedItem = FN.rowVal(dt5, "alat_terpasang");
                    txAltTerpsang.Text = FN.rowVal(dt5, "alat_terpasang_dtl");
                    txObatRutin.Text = FN.rowVal(dt5, "obat_dihentikan");
                    txjenisPeriksa.Text = FN.rowVal(dt5, "jenis_periksa");
                    txProcedurePlan.Text = FN.rowVal(dt5, "prosedure_persiapan");
                    txAktivitas.Text = FN.rowVal(dt5, "aktivitas");
                    txPolaMakan.Text = FN.rowVal(dt5, "pola_makan");
                    txPsikologis.Text = FN.rowVal(dt5, "psikologis");
                    txKebiasaanlain.Text = FN.rowVal(dt5, "kebiasaan_lain");
                    FN.splitVal(FN.rowVal(dt5, "perawatan_lanjutan"), rgRawatLanjutan);
                    txProcRawat.Text = FN.rowVal(dt5, "prosedure_perawatan");
                    txdoRawat.Text = FN.rowVal(dt5, "pemberi_perawatan");
                    txWaktu.Text = FN.rowVal(dt5, "waktu_frekuensi_rawat");
                    txUnitKesehatan.Text = FN.rowVal(dt5, "unit_kesehatan");
                    txTindakan.Text = FN.rowVal(dt5, "tindakan_darurat");
                    if (DateTime.TryParseExact(FN.rowVal(dt5, "tgl_kontrol_lanjutan"), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dte))
                    {
                        if (FN.rowVal(dt5, "tgl_kontrol_lanjutan").ToString() == "0001-01-01")
                            txControlLanjutan.EditValue = null;
                        else
                            txControlLanjutan.EditValue = dte;
                    }
                    //txControlLanjutan.Text = FN.rowVal(dt5, "tgl_kontrol_lanjutan");
                    txDokterDituju.Text = FN.rowVal(dt5, "dokter_dituju");
                    setDocPulang(FN.rowVal(dt5, "dokumen_dibawa"));

                    DataTable dtO = ConnOra.Data_Table_ora("select * from T1_OBAT_PULANG where anamesa_id = " + anamesaID + " order by seq");
                    //DataTable dtO = ORADB.SetData(ORADB.XE, "select * from T1_OBAT_PULANG where anamesa_id = " + anamesaID + " order by seq");
                    gcObtPlng.DataSource = dtO;
                }

                DataTable dt6 = ConnOra.Data_Table_ora("select * from T1_RESUME_PULANG where anamesa_id = " + anamesaID + " ");
                //DataTable dt6 = ORADB.SetData(ORADB.XE, "select * from T1_RESUME_PULANG where anamesa_id = " + anamesaID + " ");
                if (dt6.Rows.Count > 0)
                {
                    DateTime dte;
                    if (DateTime.TryParseExact(FN.rowVal(dt6, "tanggal_keluar"), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dte))
                        dtKeluarx.EditValue = dte;

                    //dtKeluarx.EditValue = FN.rowVal(dt6, "tanggal_keluar").ToString().Substring(0,10);
                    txtjam.Text = FN.rowVal(dt6, "JAM_KELUAR").ToString() ;
                    txDokterPengirim.EditValue = FN.rowVal(dt6, "dokter_pengirim");
                    txDokterKonsultan.EditValue = FN.rowVal(dt6, "dokter_konsultan");
                    txDiagnosaAkhir.EditValue = FN.rowVal(dt6, "diagnose_akhir");
                    txAnamesa.Text = FN.rowVal(dt6, "anamesa");
                    mmPeriksaFisik.Text = FN.rowVal(dt6, "periksa_fisik_lab");
                    txPengobatan.Text = FN.rowVal(dt6, "pengobatan_dilakukan");
                    txTindakanDo.Text = FN.rowVal(dt6, "tindakan_dilakukan");
                    txTerapiLanjtan.Text = FN.rowVal(dt6, "terapi_lanjutan");
                    txAnjuran.Text = FN.rowVal(dt6, "anjuran");
                }

                DataTable dt7 = ConnOra.Data_Table_ora("select * from T1_CPPT where anamesa_id = " + anamesaID + " order by to_char(tanggal,'YYYYMMDD')||replace(jam,':','')|| case when ctype ='S' then 1 when ctype ='O' then 2 when ctype ='A' then 3 else 4 end");
                //DataTable dt7 = ORADB.SetData(ORADB.XE, "select * from T1_CPPT where anamesa_id = " + anamesaID+" order by seq");
                gcCppt.DataSource = dt7;

                DataTable dt9 = ConnOra.Data_Table_ora("select * from T1_ASESMEN_GIZI where anamesa_id = " + anamesaID + " ");
                //DataTable dt9 = ORADB.SetData(ORADB.XE, "select * from T1_ASESMEN_GIZI where anamesa_id = " + anamesaID + " ");
                if(dt9.Rows.Count > 0)
                {
                    txBbi.Text = FN.rowVal(dt9, "bbi");
                    txStsGizi.Text = FN.rowVal(dt9, "sts_gizi");
                    txBbDw.Text = FN.rowVal(dt9, "bb_dewasa");
                    txTbDw.Text = FN.rowVal(dt9, "tb_dewasa");
                    txLilaDw.Text = FN.rowVal(dt9, "lila_dewasa");
                    txTgLutut.Text = FN.rowVal(dt9, "tinggi_lutut");
                    txImtDw.Text = FN.rowVal(dt9, "imt_dewasa");
                    txBBU.Text = FN.rowVal(dt9, "bbu");
                    txBBTB.Text = FN.rowVal(dt9, "bbtb");
                    txImtAnk.Text = FN.rowVal(dt9, "imt_anak");
                    txBiokimia.Text = FN.rowVal(dt9, "biokimia");
                    txKlinis.Text = FN.rowVal(dt9, "klinis");
                    txAlergiMkn.Text = FN.rowVal(dt9, "alergi_makan");
                    txPolaMkn.Text = FN.rowVal(dt9, "pola_makan");
                    txNlaiEnergi.Text = FN.rowVal(dt9, "nilai_energi");
                    txPercenEnergi.Text = FN.rowVal(dt9, "percen_energi");
                    txKbthnEnergi.Text = FN.rowVal(dt9, "kbthn_energi");
                    txNilaiProtein.Text = FN.rowVal(dt9, "nilai_protein");
                    txPercenProtein.Text = FN.rowVal(dt9, "percen_protein");
                    txKbthnProtein.Text = FN.rowVal(dt9, "kbthn_protein");
                    txNilaiLemak.Text = FN.rowVal(dt9, "nilai_lemak");
                    txPercenLemak.Text = FN.rowVal(dt9, "percen_lemak");
                    txKbthnLemak.Text = FN.rowVal(dt9, "kbthn_lemak");
                    txNilaiKarbo.Text = FN.rowVal(dt9, "nilai_karbo");
                    txPercenKarbo.Text = FN.rowVal(dt9, "percen_karbo");
                    txKbthnKarbo.Text = FN.rowVal(dt9, "kbthn_karbo");
                    txAsupnKurng.Text = FN.rowVal(dt9, "asupan_kurang");
                    txAsupnLbh.Text = FN.rowVal(dt9, "asupan_lebih");
                    txRiwytPerson.Text = FN.rowVal(dt9, "riwayat_personal");
                    txDiagnosGz.Text = FN.rowVal(dt9, "diagnosa_gizi");
                    txIntrvnsiGz.Text = FN.rowVal(dt9, "intervensi_gizi");
                    txMonitoring.Text = FN.rowVal(dt9, "monitoring");
                }

                //DataTable dt8 = ORADB.SetData(ORADB.XE, "select * from T1_JADWAL_BERI_OBAT where anamesa_id = " + anamesaID + " order by seq");
                //gcJadwalObat.DataSource = dt8;

               

                string SQL = "";
                SQL = "select ROWNUM SEQ, b.detail_id, c.treat_group_id, b.treat_item_id, c.TREAT_ITEM_NAME, b.treat_qty, b.treat_item_price,  " +
                            "       b.remarks, 'S' action, a.head_id, b.treat_date  TANGGAL, TREAT_JAM JAM, a.pay_status ,a.ID_VISIT " +
                            "  from KLINIK.cs_treatment_head a  " +
                            "  join KLINIK.cs_treatment_detail b on (a.head_id=b.head_id)  " +
                            "  join KLINIK.cs_treatment_item c on (b.treat_item_id=c.treat_item_id and c.F_STATUS = '" + type_s + "' and c.USED_BY is null )  " +
                            " where ID_VISIT = '" + visitid + "'   and b.ID_DOKTER is  null AND B.F_ACTIVE ='Y' and c.F_STATUS = '" + type_s + "' and b.GRID_NAME = 'gvMedis' order by  b.treat_date desc, c.TREAT_ITEM_NAME";
                            //"   and a.status = 'OPN'  ";

                dtMedis = ConnOra.Data_Table_ora(SQL);   
                gridMedis.DataSource = dtMedis;

                ConnOra.LookUpGridFilter(listLaya2, gvMedis, "layananCode", "layananName", LokPelayanan, 3);
                ////RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
                //lookLaya.DataSource = listLaya2;
                //lookLaya.ValueMember = "layananCode";
                //lookLaya.DisplayMember = "layananName"; 
                //lookLaya.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //lookLaya.AutoSearchColumnIndex = 1;
                //lookLaya.ImmediatePopup = true;
                //lookLaya.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                //lookLaya.NullText = "";
                //gvMedis.Columns[3].ColumnEdit = lookLaya;

                if(type_s.ToString().Equals("B"))
                {
                    string SQLU = "";
                    SQLU = "select ROWNUM SEQ, b.detail_id, c.treat_group_id, b.treat_item_id, c.TREAT_ITEM_NAME, b.treat_qty, b.treat_item_price,  " +
                                "       b.remarks, 'S' action, a.head_id, b.treat_date  TANGGAL, TREAT_JAM JAM, a.pay_status ,a.ID_VISIT " +
                                "  from KLINIK.cs_treatment_head a  " +
                                "  join KLINIK.cs_treatment_detail b on (a.head_id=b.head_id)  " +
                                "  join KLINIK.cs_treatment_item c on (b.treat_item_id=c.treat_item_id AND c.F_STATUS = 'U' and c.USED_BY is null )  " +
                                " where ID_VISIT = '" + visitid + "'   and b.ID_DOKTER is  null AND B.F_ACTIVE ='Y' and c.F_STATUS = 'U' and b.GRID_NAME = 'gvMedisU' order by  b.treat_date desc, c.TREAT_ITEM_NAME";
                    //"   and a.status = 'OPN'  ";

                    dtMedisU = ConnOra.Data_Table_ora(SQLU);
                    gridMedisU.DataSource = dtMedisU;

                    ConnOra.LookUpGridFilter(listLayaU, gvMedisU, "layananCode", "layananName", LokPelayananU, 3);

                    ////RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
                    //lookLayaU.DataSource = listLayaU;
                    //lookLayaU.ValueMember = "layananCode";
                    //lookLayaU.DisplayMember = "layananName";
                    //lookLayaU.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                    //lookLayaU.AutoSearchColumnIndex = 1;
                    //lookLayaU.ImmediatePopup = true;
                    //lookLayaU.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                    //lookLayaU.NullText = "";
                    //gvMedisU.Columns[3].ColumnEdit = lookLayaU;
                }
                
                //gvMedis.Columns[10].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                //gvMedis.Columns[10].DisplayFormat.FormatString = "yyyy-MM-dd";

                string SQL2 = " Select ROWNUM SEQ, b.detail_id, c.treat_group_id, b.treat_item_id, c.TREAT_ITEM_NAME, b.treat_qty, b.treat_item_price,  " +
                              "        b.remarks, 'S' action, a.head_id, b.treat_date  TANGGAL, TREAT_JAM JAM, a.pay_status, a.ID_VISIT, b.ID_DOKTER " +
                              "  from KLINIK.cs_treatment_head a  " +
                              "  join KLINIK.cs_treatment_detail b on (a.head_id=b.head_id)  " +
                              "  join KLINIK.cs_treatment_item c on (b.treat_item_id=c.treat_item_id)  " +
                              "  join KLINIK.CS_DOKTER d on (b.ID_DOKTER = d.ID_DOKTER)  " +
                              " where ID_VISIT = '" + visitid + "'  and b.ID_DOKTER is not null AND B.F_ACTIVE ='Y' order by  b.treat_date desc, c.TREAT_ITEM_NAME ";
                //"   and a.status = 'OPN'  ";

                dtVisitDokter = ConnOra.Data_Table_ora(SQL2); //ORADB.SetData(ORADB.XE, SQL2);
                gridVisitDoc.DataSource = dtVisitDokter;

                RepositoryItemGridLookUpEdit glvisit = new RepositoryItemGridLookUpEdit();
                glvisit.DataSource = listDokter;
                glvisit.ValueMember = "ID_Dokter";
                glvisit.DisplayMember = "Nama_Dokter";

                glvisit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glvisit.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                glvisit.ImmediatePopup = true;
                glvisit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glvisit.NullText = "";
                gvVisitDoc.Columns[4].ColumnEdit = glvisit;

                ConnOra.LookUpGridFilter(listLayav, gvVisitDoc, "layananCode", "layananName", LokPelayananD, 3);

                //RepositoryItemGridLookUpEdit glLayav = new RepositoryItemGridLookUpEdit();
                //glLayav.DataSource = listLayav;
                //glLayav.ValueMember = "layananCode";
                //glLayav.DisplayMember = "layananName";

                //glLayav.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //glLayav.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                //glLayav.ImmediatePopup = true;
                //glLayav.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                //glLayav.NullText = "";
                //gvVisitDoc.Columns[3].ColumnEdit = glLayav;

                LoadDataResep();

                //gvVisitDoc.Columns[10].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                //gvVisitDoc.Columns[10].DisplayFormat.FormatString = "yyyy-MM-dd";

                dtVital = ConnOra.Data_Table_ora("select * from T1_GRAFIK_VITAL where anamesa_id = " + anamesaID + " "); //ORADB.SetData(ORADB.XE, "select * from T1_GRAFIK_VITAL where anamesa_id = " + anamesaID+" ");
                gcVt.DataSource = dtVital;
                setChart();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Info");
            }
        }

        private bool updateData()
        {
            try
            {
                //Ambil semua data pada inputan TAB-1
                Dictionary<string, string> A1_fields = new Dictionary<string, string>
                {
                    { "keluhan_utama", mmKeluhan.Text?.ToString()},
                    { "penyakit_lalu", FN.getVal(gbRwPenyakitlalu) },
                    { "pernah_dirawat",FN.getVal(gbPernahDirawat) },
                    { "pernah_operasi",FN.getVal(gbPernahOperasi) },
                    { "penyakit_keluarga", FN.getVal(gbRwSakitKlrg, 3) },
                    { "tergantung_thd", FN.getVal(gbTergantungThdp, 3) },
                    { "riwayat_pekerjaan", FN.getVal(gbRwkerja) },
                    { "riwayat_alergi", FN.getVal(gbRwAlergi, 3) },
                    { "riwayat_obat", FN.getVal(gbRwObat) },
                    { "TD", txTd.Text?.ToString() },
                    { "nadi", txNadi.Text?.ToString() },
                    { "p", txP.Text?.ToString() },
                    { "suhu", txSuhu.Text?.ToString() },
                    { "keluhan", FN.getVal(pnKeluhan) },
                    { "batas_makan", txBtsMakan.Text?.ToString() },
                    { "gigi_palsu",FN.radioVal(rgGigiPalsu)},
                    { "mual", FN.radioVal(rgMual) },
                    { "muntah", FN.radioVal(rgMuntah)},
                    { "BB", txBB.Text?.ToString() },
                    { "TB", txTbPb.Text?.ToString() },
                    { "imt", txImt.Text?.ToString() },
                    { "gst_ket", txGstKet.Text?.ToString() },
                    { "pendengaran", FN.getVal(pnlPendengaran) },
                    { "penglihatan", FN.getVal(pnlPenglihatan) },
                    { "defekasi", FN.getVal(pnlDefekasi) },
                    { "miksi", FN.getVal(pnlMiksi) },
                    { "kulit", FN.getVal(pnlKulit) },
                    { "skor_norton", txSkorNorton.Text?.ToString() },
                    { "resiko_dekubitus", FN.radioVal(rbDekubitus) },
                    { "lokasi_luka", FN.chkListOf(ckLokasiLuka) },
                    { "periksa_fisik_lain", txPeriksaFisik.Text?.ToString() },
                    { "form_periksa_khusus", FN.radioVal(rgPeriksaKhusus)},
                    { "status_psikologi", FN.getVal(gbStsPsikologi, 5) },
                    { "status_mental", getStsMental() },
                    { "hubungan_keluarga",FN.radioVal(rgHubKluarga)},
                    { "tempat_tinggal", FN.getVal(pnlTempatTinggal)},
                    { "nama_kerabat", txNmKerabat.Text?.ToString() },
                    { "hub_kerabat", txHubKerabat.Text?.ToString() },
                    { "tlp_kerabat", txTlpKerabat.Text?.ToString() },
                    { "keg_agama", txkegAgama.Text?.ToString() },
                    { "keg_spiritual", txkegSpirit.Text?.ToString() },
                    { "hambatan_belajar", FN.getVal(gbHambatanBljr, 2) },
                    { "butuh_penerjemah", FN.getVal(pnlButuhPnrjmh) },
                    { "kebutuhan_edukasi", FN.getVal(pnlKbthnEdukasi, 5) },
                    { "bersedia_dikunjungi", FN.getVal(pnlSedia, 2) },
                    { "resiko_cedera", FN.radioVal(rgResikoCedera)},
                    { "menerima_info",FN.radioVal(rgMnrimaInfo)}
                };
                MD.UpdateData(ORADB.XE, "T1_RAWAT_INAP1", "anamesa_id = " + anamesaID + " ", A1_fields);

                //Ambil semua data pada inputan TAB-2
                Dictionary<string, string> A2_fields = new Dictionary<string, string>
                {
                    { "mobilisasi", FN.getVal(gbStsFungsi) },
                    { "alat_bantu_jalan", txAltBantujalan.Text?.ToString() },
                    { "nyeri", FN.radioVal(rgNyeri) },
                    { "skala_nyeri", FN.chkListOf(chkSkalaNyeri) },
                    { "tingkat_nyeri",FN.radioVal(rgTingkatNyeri)  },
                    { "lokasi_nyeri", txLokasiNyeri.Text?.ToString() },
                    { "frekuensi_nyeri", txFrekuensi.Text?.ToString() },
                    { "durasi_nyeri",txDurasiNyeri.Text?.ToString() },
                    { "skor_nyeri", txScorNyeri.Text?.ToString() },
                    { "nyeri_hilang", FN.getVal(gbNyeriHilang) },
                    { "turun_berat_badan", FN.getVal(pnlBeratBadan, 4) },
                    { "kurang_asupan_makan", FN.radioVal(rgAsupanMakan) },
                    { "skor_trgz", "8" },
                    { "diagnose_khusus", FN.getVal(pnlDiagnoseKhusus, 3) },
                    { "lapor_tim_trgz", FN.getVal(pnlLaporTim) },
                    { "mslh_perawat", mmPerawat.Text?.ToString() },
                    { "mslh_dokter", mmDokter.EditValue?.ToString() },
                    { "tujuan_terukur", mmTujuanTerukur.Text?.ToString() },
                    { "susun_rencana_perawat", chkSusunRencana.Checked?"Y":"N" }
                };
                MD.UpdateData(ORADB.XE, "T1_RAWAT_INAP2", "anamesa_id = " + anamesaID + " ", A2_fields);


                Dictionary<string, string> planingPulangData = new Dictionary<string, string>
                {
                    { "tanggal_keluar", dtkeluar.DateTime.ToString("yyyy-MM-dd") },
                    { "keadaan_pulang", cbKeadaanPulang.Text?.ToString()},
                    { "keadaan_pulang_dtl", txKeadaanDtl.Text?.ToString() },
                    { "alat_terpasang", cbAlatTerpasang.Text?.ToString() },
                    { "alat_terpasang_dtl", txAltTerpsang.Text?.ToString() },
                    { "obat_dihentikan", txObatRutin.Text?.ToString() },
                    { "jenis_periksa", txjenisPeriksa.Text?.ToString() },
                    { "prosedure_persiapan", txProcedurePlan.Text?.ToString() },
                    { "aktivitas", txAktivitas.Text?.ToString() },
                    { "pola_makan", txPolaMakan.Text?.ToString() },
                    { "psikologis", txPsikologis.Text?.ToString() },
                    { "kebiasaan_lain", txKebiasaanlain.Text?.ToString() },
                    { "perawatan_lanjutan", FN.radioVal(rgRawatLanjutan) },
                    { "prosedure_perawatan", txProcRawat.Text?.ToString() },
                    { "pemberi_perawatan", txdoRawat.Text?.ToString() },
                    { "waktu_frekuensi_rawat", txWaktu.Text?.ToString() },
                    { "unit_kesehatan", txUnitKesehatan.Text?.ToString() },
                    { "tindakan_darurat", txTindakan.Text?.ToString() },
                    { "tgl_kontrol_lanjutan",  txControlLanjutan.DateTime.ToString("yyyy-MM-dd")   },
                    { "dokter_dituju", txDokterDituju.Text?.ToString() },
                    { "dokumen_dibawa", getDocPulang() }
                };
                bool save = MD.UpdateData(ORADB.XE, "T1_PERENCANAAN_PULANG", "anamesa_id = " + anamesaID + " ", planingPulangData);
                if (save)
                {
                    //if(gvObtPlng.RowCount > 0)
                    //{
                    //    DataTable dt = ORADB.SetData(ORADB.XE, "select * from T1_OBAT_PULANG where anamesa_id = " + anamesaID+" ");
                    //    if(dt != null && dt.Rows.Count > 0)
                    //    {
                    //        ORADB.Execute(ORADB.XE, " delete from T1_OBAT_PULANG where anamesa_id = " + anamesaID + "  ");
                    //    }

                    //    string sql = "insert all ";
                    //    for (int i =0; i< gvObtPlng.RowCount; i++)
                    //    {
                    //        sql = sql + " INTO T1_OBAT_PULANG (anamesa_id, seq, nama_obat, dosis, waktu_beri, cara) values ( ";
                    //        sql = sql + " "+anamesaID+" ,";
                    //        sql = sql + " '"+FN.strVal(gvObtPlng, i, "SEQ") +"' ,";
                    //        sql = sql + " '"+FN.strVal(gvObtPlng, i, "NAMA_OBAT") +"' ,";
                    //        sql = sql + " '"+FN.strVal(gvObtPlng, i, "DOSIS") +"' ,";
                    //        sql = sql + " '"+FN.strVal(gvObtPlng, i, "WAKTU_BERI") +"' ,";
                    //        sql = sql + " '"+FN.strVal(gvObtPlng, i, "CARA") +"' ) ";
                    //    }
                    //    sql = sql + " select * from dual";
                    //    ORADB.Execute(ORADB.XE, sql);
                    //}
                }


                Dictionary<string, string> resumePulangData = new Dictionary<string, string>
                {
                    { "tanggal_keluar", dtKeluarx.DateTime.ToString("yyyy-MM-dd")},
                    { "dokter_pengirim", txDokterPengirim.EditValue.ToString() },
                    { "dokter_konsultan", txDokterKonsultan.EditValue.ToString() },
                    { "diagnose_akhir", txDiagnosaAkhir.EditValue.ToString() },
                    { "anamesa",txAnamesa.Text?.ToString() },
                    { "periksa_fisik_lab", mmPeriksaFisik.Text?.ToString() },
                    { "pengobatan_dilakukan", txPengobatan.Text?.ToString() },
                    { "tindakan_dilakukan", txTindakanDo.Text?.ToString() },
                    { "terapi_lanjutan", txTerapiLanjtan.Text?.ToString() },
                    { "anjuran", txAnjuran.Text?.ToString() },
                    { "JAM_KELUAR", txtjam.Text?.ToString() } 
                };
                MD.UpdateData(ORADB.XE, "T1_RESUME_PULANG", "anamesa_id = " + anamesaID + " ", resumePulangData);

                string tgl_out = dtKeluarx.DateTime.ToString("yyyy-MM-dd") + " " + txtjam.Text.ToString() + ":00";

                Dictionary<string, string> TglPulangData = new Dictionary<string, string>
                {
                    { "date_out", tgl_out},
                    { "STATUS", "PAY" },
                    { "upd_date",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "upd_emp", DB.vUserId } 
                };
                MD.UpdateData(ORADB.XE, "cs_inpatient", "inpatient_id = " + inpatient_id + " ", TglPulangData);

                
                DataTable dtDiagnos = ConnOra.Data_Table_ora("select * from KLINIK.cs_diagnosa where ANAMNESA_ID = " + anamesaID + " and TYPE_DIAGNOSA = 'E' "); 
                if (dtDiagnos.Rows.Count > 0)
                {
                    Dictionary<string, string> DiagnosaPulang = new Dictionary<string, string>
                    {
                        { "rm_no", RMNO },
                        { "insp_date",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                        { "item_cd", txDiagnosaAkhir.EditValue.ToString() }, 
                        { "remark", txAnjuran.Text.ToString()  },
                        { "NOTED", txTerapiLanjtan.Text.ToString()  },
                        { "UPD_DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                        { "UPD_EMP", DB.vUserId } 
                    };
                    MD.UpdateData(ORADB.XE, "cs_diagnosa", "ANAMNESA_ID = " + anamesaID + " and type_diagnosa ='E' ", DiagnosaPulang); 
                }
                else
                {
                    string sql = @"INSERT INTO KLINIK.cs_diagnosa ( RM_NO, INSP_DATE, ITEM_CD, TYPE_DIAGNOSA, REMARK, 
                                      INS_DATE, INS_EMP, VISIT_NO, NOTED,   ANAMNESA_ID ) VALUES ( 
                                    '" + RMNO + @"',
                                    '" + tgl_out + @"',
                                    '" + txDiagnosaAkhir.EditValue.ToString() + @"',
                                    'E',
                                    '" + txAnjuran.Text.ToString() + @"',
                                    '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"',
                                    '" + DB.vUserId + @"',
                                    '" + visitid + @"',
                                    '" + txTerapiLanjtan.Text.ToString() + @"', 
                                    " + anamesaID + @") ";
                    save = ORADB.Execute(ORADB.XE, sql);
                }

                Dictionary<string, string> asesmenGiziValues = new Dictionary<string, string>
                {
                    { "bbi", txBbi.Text?.ToString() },
                    { "sts_gizi", txStsGizi.Text?.ToString() },
                    { "bb_dewasa", txBbDw.Text?.ToString() },
                    { "tb_dewasa", txTbDw.Text?.ToString() },
                    { "lila_dewasa", txLilaDw.Text?.ToString() },
                    { "tinggi_lutut", txTgLutut.Text?.ToString() },
                    { "imt_dewasa", txImtDw.Text?.ToString() },
                    { "bbu", txBBU.Text?.ToString() },
                    { "bbtb", txBBTB.Text?.ToString() },
                    { "imt_anak", txImtAnk.Text?.ToString() },
                    { "biokimia", txBiokimia.Text?.ToString() },
                    { "klinis", txKlinis.Text?.ToString() },
                    { "alergi_makan", txAlergiMkn.Text?.ToString() },
                    { "pola_makan", txPolaMkn.Text?.ToString() },
                    { "nilai_energi", txNlaiEnergi.Text?.ToString() },
                    { "percen_energi", txPercenEnergi.Text?.ToString() },
                    { "kbthn_energi", txKbthnEnergi.Text?.ToString() },
                    { "nilai_protein", txNilaiProtein.Text?.ToString() },
                    { "percen_protein", txPercenProtein.Text?.ToString() },
                    { "kbthn_protein", txKbthnProtein.Text?.ToString() },
                    { "nilai_lemak", txNilaiLemak.Text?.ToString() },
                    { "percen_lemak", txPercenLemak.Text?.ToString() },
                    { "kbthn_lemak", txKbthnLemak.Text?.ToString() },
                    { "nilai_karbo", txNilaiKarbo.Text?.ToString() },
                    { "percen_karbo", txPercenKarbo.Text?.ToString() },
                    { "kbthn_karbo", txKbthnKarbo.Text?.ToString() },
                    { "asupan_kurang", txAsupnKurng.Text?.ToString() },
                    { "asupan_lebih", txAsupnLbh.Text?.ToString() },
                    { "riwayat_personal", txRiwytPerson.Text?.ToString() },
                    { "diagnosa_gizi", txDiagnosGz.Text?.ToString() },
                    { "intervensi_gizi", txIntrvnsiGz.Text?.ToString() },
                    { "monitoring", txMonitoring.Text?.ToString() }
                };
                MD.UpdateData(ORADB.XE, "T1_ASESMEN_GIZI", "anamesa_id = " + anamesaID + " ", asesmenGiziValues);

            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Failed to save");
                return false;
            }
            return true;
        }
        #endregion


        #region Controls Actions

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            bool save = updateData();
            if (save)
            {
                MessageBox.Show("Data berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                FN.errosMsg("Data gagal disimpan!", "Error");
            }
        }

        //Event untuk mengatur saat radioGroup atau CheckBox dipilih
        private Control lastSender;
        private void EnableTextEdit(object sender, EventArgs e)
        {
            Control parentControl = null;

            if (sender is RadioGroup)
            {
                RadioGroup radioGroup = (RadioGroup)sender;
                lastSender = radioGroup;
                parentControl = radioGroup.Parent;
                if (radioGroup.EditValue != null && radioGroup.EditValue?.ToString() == "1") {
                    if (parentControl != null) FN.EnableControls(parentControl, true, lastSender);
                }
                else{
                    if (parentControl != null) FN.EnableControls(parentControl, false, lastSender);
                }
            }
            else if (sender is CheckEdit)
            {
                CheckEdit checkEdit = (CheckEdit)sender;
                lastSender = checkEdit;
                parentControl = checkEdit.Parent;
                if (checkEdit.Checked){
                    if (parentControl != null) FN.EnableControls(parentControl, true, lastSender);
                }
                else{
                    if (parentControl != null) FN.EnableControls(parentControl, false, lastSender);
                }
            }
        }


        private void rgKatgriPasien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(rgKatgriPasien.SelectedIndex == 0)
            {
                pnlRJDewasa.Visible = false;
                pnlRJAnak.Visible = true;
            }
            else
            {
                pnlRJAnak.Visible = false;
                pnlRJDewasa.Visible = true;
            }
        }

        private void cbKeadaanPulang_SelectedValueChanged(object sender, EventArgs e)
        {
            string val = cbKeadaanPulang.SelectedIndex.ToString();
            if (val == "0" || val == "1" || val == "2") txKeadaanDtl.Enabled = false;
            else txKeadaanDtl.Enabled = true;

            if (val == "3") lblKeadaan.Text = "Rujuk ke";
            if (val == "4") lblKeadaan.Text = "Alasan";
            if (val == "5") lblKeadaan.Text = "Lainnya";
        }

        private void cbAlatTerpasang_SelectedValueChanged(object sender, EventArgs e)
        {
            string val = cbAlatTerpasang.Text?.ToString();
            if (val == "Lainnya") txAltTerpsang.Enabled = true;
            else txAltTerpsang.Enabled = false;

        }


        private void btnAddJadwalObat_Click(object sender, EventArgs e)
        {
            //if (dtJadwalObat == null) return;

            //DataRow newRow = dtJadwalObat.NewRow();

            //newRow["SEQ"] = ((gvJadwalObat.RowCount) + 1).ToString();
            //dtJadwalObat.Rows.Add(newRow);

            //gcJadwalObat.DataSource = dtJadwalObat;

            gvJadwalObat.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gvJadwalObat.AddNewRow();
        }

        private void btnAddCppt_Click(object sender, EventArgs e)
        {
            //if (dtCppt == null) return;


            //DataRow newRow = dtCppt.NewRow();

            //newRow["SEQ"] = ((gvCppt.RowCount) + 1).ToString();
            //newRow["TANGGAL"] = DateTime.Now;
            //newRow["JAM"] = DateTime.Now.ToString("HH:mm");
            gvCppt.OptionsBehavior.ImmediateUpdateRowPosition = false;
            gvCppt.AddNewRow();
            //dtCppt.Rows.Add(newRow);

            //gcCppt.DataSource = dtCppt;
        }

        private void addObat_Click(object sender, EventArgs e)
        {
            //if (dtObatPulang == null) return;

            //DataRow newRow = dtObatPulang.NewRow();

            //newRow["SEQ"] = ((gvObtPlng.RowCount) + 1).ToString();
            //dtObatPulang.Rows.Add(newRow);

            //gcObtPlng.DataSource = dtObatPulang;

            gvObtPlng.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gvObtPlng.AddNewRow();

        }
        #endregion




        private string getStsMental()
        {
            string a = "";string b = "";string c = "";
            if (ckStsMental1.Checked) a = ckStsMental1.Text?.ToString();
            if (chkStsMental2.Checked) b = chkStsMental2.Text?.ToString();
            if (chkStasMental3.Checked) c = chkStasMental3.Text?.ToString();

            return a + "::" + b + "=>" + txStsMental2.Text?.ToString() + "::" + c + "=>" + txStsMental3.Text?.ToString();
        }

        private void setStsMental(string dt)
        {
            string[] val = dt.Split(new string[] { "::" }, StringSplitOptions.None);
            if(val.Length > 1)
            {
                if (val[0].ToString() != "")
                    ckStsMental1.Checked = true;
                else return;

                if (val[1].ToString() != "=>")
                {
                    string[] aa = val[1].ToString().Split(new string[] { "=>" }, StringSplitOptions.None);
                    txStsMental2.Text = aa[1];
                    chkStsMental2.Checked = true;
                }

                if (val[2].ToString() != "=>")
                {
                    string[] aa = val[2].ToString().Split(new string[] { "=>" }, StringSplitOptions.None);
                    txStsMental3.Text = aa[1];
                    chkStasMental3.Checked = true;
                }
            } 
        }

        private string getDocPulang()
        {
            string a = "";string b = "";string c = "";string d = "";
            string e = "";string f = "";string g = "";string h = "";
            if (chkDoc1.Checked) a = chkDoc1.Text?.ToString();
            if (chkDoc2.Checked) b = chkDoc2.Text?.ToString();
            if (chkDoc3.Checked) c = chkDoc3.Text?.ToString();
            if (chkDoc4.Checked) d = chkDoc4.Text?.ToString();
            if (chkDoc5.Checked) e = chkDoc5.Text?.ToString();
            if (chkDoc6.Checked) f = chkDoc6.Text?.ToString();
            if (chkDoc7.Checked) g = chkDoc7.Text?.ToString();
            if (chkDoc8.Checked) h = chkDoc8.Text?.ToString();

           string x = a + "=>"+txDoc1.Text?.ToString()+"::"+b+"=>"+txDoc2.Text.ToString()+"=>"+txDoc3.Text.ToString()+"::";
           string y = c + "=>" + txDoc4.Text.ToString() + "::" + d + "=>" + txDoc5.Text.ToString() + "::" + e + "=>" + txDoc6.Text?.ToString()+"::";
           string z = f+"::"+g+"::"+ h;

           return x + y + z;
        }

        private void setDocPulang(string dt)
        {
            string[] val = dt.Split(new string[] { "::" }, StringSplitOptions.None);
            if(val.Length > 1)
            {
                if (val[0].ToString() == "")
                    return;

                if (val[0].ToString() != "=>")
                {
                    string[] aa = val[0].ToString().Split(new string[] { "=>" }, StringSplitOptions.None);
                    txDoc1.Text = aa[1];
                    chkDoc1.Checked = true;
                }

                if (val[1].ToString() != "=>=>")
                {
                    string[] aa = val[1].ToString().Split(new string[] { "=>" }, StringSplitOptions.None);
                    chkDoc2.Checked = true;
                    txDoc2.Text = aa[1];
                    txDoc3.Text = aa[2];
                }

                if (val[2].ToString() != "=>")
                {
                    string[] aa = val[2].ToString().Split(new string[] { "=>" }, StringSplitOptions.None);
                    txDoc4.Text = aa[1];
                    chkDoc3.Checked = true;
                }
                if (val[3].ToString() != "=>")
                {
                    string[] aa = val[3].ToString().Split(new string[] { "=>" }, StringSplitOptions.None);
                    txDoc5.Text = aa[1];
                    chkDoc4.Checked = true;
                }
                if (val[4].ToString() != "=>")
                {
                    string[] aa = val[4].ToString().Split(new string[] { "=>" }, StringSplitOptions.None);
                    txDoc6.Text = aa[1];
                    chkDoc5.Checked = true;
                }
                if (val[5].ToString() != "")
                {
                    chkDoc6.Checked = true;
                }
                if (val[6].ToString() != "")
                {
                    chkDoc7.Checked = true;
                }
                if (val[7].ToString() != "")
                {
                    chkDoc8.Checked = true;
                }
            }
        }

        

        private void checkTurunBB(string dt, RadioGroup rg, LookUpEdit le)
        {
            string[] val = dt.Split(new string[] { "::" }, StringSplitOptions.None);
            if(val.Length == 3)
            {
                rg.SelectedIndex = Convert.ToInt32(val[0]);
                if(val[2] != "")
                {
                    le.EditValue = val[2];
                }
            }
        }

        private void btnInputData_Click(object sender, EventArgs e)
        {
            
        }


        private void btnSaveX_Click(object sender, EventArgs e)
        {
            bool save = false;
            if(rgKatgriPasien.SelectedIndex == 0)
            {
                DataTable dt = ConnOra.Data_Table_ora("select * from T1_RESIKO_JATUH_ANAK where anamesa_id = " + anamesaID + " ");
                //ORADB.SetData(ORADB.XE, "select * from T1_RESIKO_JATUH_ANAK where anamesa_id = " + anamesaID + " ");
                if(dt.Rows.Count > 0)
                {
                    Dictionary<string, string> resiko_jatuh_anak = new Dictionary<string, string>
                    {
                        { "usia", FN.radioVal(rgUsia) },
                        { "jenis_kelamin", FN.radioVal(rgJenkel) },
                        { "diagnosis", FN.radioVal(rgDiagnosis) },
                        { "gangguan_fn_kognitif", FN.radioVal(rgGangguan) },
                        { "faktor_lingkungan", FN.radioVal(rgFlingkungan) },
                        { "sedasi_anestesi", FN.radioVal(rgSedasiAnestesi) },
                        { "penggunaan_obat", FN.radioVal(rguseObat) },
                        { "skor", txScoreAnak.Text?.ToString() },
                        { "resiko", txResikoAnak.Text?.ToString() }
                    };
                    save = MD.UpdateData(ORADB.XE, "T1_RESIKO_JATUH_ANAK", "anamesa_id = " + anamesaID + " ", resiko_jatuh_anak);
                }
                else
                {
                    string sql = @"INSERT INTO T1_RESIKO_JATUH_ANAK (id, anamesa_id, usia, jenis_kelamin, diagnosis,  gangguan_fn_kognitif, faktor_lingkungan,
	                                sedasi_anestesi,penggunaan_obat,skor,resiko) VALUES (
                                    resiko_jatuh_seq.NEXTVAL,
                                    " + anamesaID + @",
                                    '"+ FN.radioVal(rgUsia) + @"',
                                    '"+ FN.radioVal(rgJenkel) + @"',
                                    '"+ FN.radioVal(rgDiagnosis) + @"',
                                    '"+ FN.radioVal(rgGangguan) + @"',
                                    '"+ FN.radioVal(rgFlingkungan) + @"',
                                    '"+ FN.radioVal(rgSedasiAnestesi) + @"',
                                    '"+ FN.radioVal(rguseObat) + @"',
                                    "+ txScoreAnak.Text?.ToString()+@",
                                    '"+ txResikoAnak.Text?.ToString() + @"' )";
                   save = ORADB.Execute(ORADB.XE, sql);
                }

            }else
            {
                DataTable dt = ConnOra.Data_Table_ora("select * from T1_RESIKO_JATUH_DEWASA where anamesa_id = " + anamesaID + " ");
                //ORADB.SetData(ORADB.XE, "select * from T1_RESIKO_JATUH_DEWASA where anamesa_id = " + anamesaID + " ");
                if (dt.Rows.Count > 0)
                {
                    Dictionary<string, string> resiko_jatuh_dewasa = new Dictionary<string, string>
                    {
                        { "riwayat_jatuh", FN.joinVal(rgRiwayatJatuh, txKet1) },
                        { "memiliki_lbh_penyakit", FN.joinVal(rgDiagnosaSekunder, txKet2) },
                        { "alat_bantu_jalan", FN.joinVal(rgAltBantuJalan, txKet3) },
                        { "terpasang_infus", FN.joinVal(rgInfus, txket4) },
                        { "gaya_berjalan", FN.joinVal(rgGayaJalan, txKet5) },
                        { "status_mental", FN.joinVal(rgstsMental, txket6) },
                        { "total_nilai", txTotalNilai.Text?.ToString() },
                        { "resiko", txResikoDewasa.Text?.ToString() },
                        { "tindakan", lblTindakan.Text?.ToString() }
                    };
                   save = MD.UpdateData(ORADB.XE, "T1_RESIKO_JATUH_DEWASA", "anamesa_id = " + anamesaID + " ", resiko_jatuh_dewasa);
                }
                else
                {
                    string sql = @"INSERT INTO T1_RESIKO_JATUH_DEWASA (id, anamesa_id, riwayat_jatuh, memiliki_lbh_penyakit, alat_bantu_jalan, terpasang_infus, 
                                    gaya_berjalan, status_mental, total_nilai, resiko, tindakan) VALUES (
                                     resiko_jatuh_seq.NEXTVAL,
                                    " + anamesaID + @",
                                    '"+ FN.joinVal(rgRiwayatJatuh, txKet1) + @"',
                                    '"+ FN.joinVal(rgDiagnosaSekunder, txKet2) + @"',
                                    '"+ FN.joinVal(rgAltBantuJalan, txKet3) + @"',
                                    '"+ FN.joinVal(rgInfus, txket4) + @"',
                                    '"+ FN.joinVal(rgGayaJalan, txKet5) + @"',
                                    '"+ FN.joinVal(rgstsMental, txket6) + @"',
                                    "+ txTotalNilai.Text?.ToString() + @",
                                    '"+ txResikoDewasa.Text?.ToString() + @"',
                                    '"+ lblTindakan.Text?.ToString() + @"') ";
                    save = ORADB.Execute(ORADB.XE, sql);
                }

            }

            if (save)
            {
                MessageBox.Show("Formulir Resiko Jatuh berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                FN.errosMsg("Simpan data gagal!", "Error");
            }

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
                string sql_delete = "";
                 
                sql_delete = "";

                sql_delete = sql_delete + " update T1_CPPT set f_active = 'N', UPDATED_BY = '" + DB.vUserId + "',UPDATED_DATE = SYSDATE  ";
                sql_delete = sql_delete + " where anamesa_id = " + anamesaID + " and seq = '" + FN.strVal(gvCppt, gvCppt.FocusedRowHandle, "SEQ") + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);
                    gvCppt.DeleteRow(gvCppt.FocusedRowHandle);
                    MessageBox.Show("Data Berhasil dihapus");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }

        private void btnSaveCppt_Click(object sender, EventArgs e)
        {
            try
            {
                if(gvCppt.RowCount > 0)
                {
                    DataTable dt = ConnOra.Data_Table_ora("select * from T1_CPPT where anamesa_id =" + anamesaID + " ");
                    //ORADB.SetData(ORADB.XE, "select * from T1_CPPT where anamesa_id =" + anamesaID + " ");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ORADB.Execute(ORADB.XE, "delete from T1_CPPT where anamesa_id = " + anamesaID + " ");
                    }

                    string sql = "insert all ";
                    for (int i = 0; i < gvCppt.RowCount; i++)
                    {
                        string dte = ""; int ii = i + 1;
                        object tgl = gvCppt.GetRowCellValue(i,"TANGGAL");
                        if (tgl != null && tgl is DateTime)
                        {
                            DateTime selectedDateTime = (DateTime)tgl;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime selectedDateTime = DateTime.Now;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }

                        sql = sql + " into T1_CPPT (anamesa_id, tanggal, jam, kode_ppa, CTYPE, hasil_asesmen, instruksi, nama_terang, seq,CREATED_BY,CREATED_DATE) values ( ";
                        sql = sql + " " + anamesaID + " ,";
                        sql = sql + " TO_DATE('"+ dte + "', 'yyyy-MM-dd') ,";
                        sql = sql + " '" + FN.strVal(gvCppt, i, "JAM") + "' ,";
                        sql = sql + " '" + FN.strVal(gvCppt, i, "KODE_PPA") + "' ,";
                        sql = sql + " '" + FN.strVal(gvCppt, i, "CTYPE") + "' ,";
                        sql = sql + " '" + FN.strVal(gvCppt, i, "HASIL_ASESMEN") + "' ,";
                        sql = sql + " '" + FN.strVal(gvCppt, i, "INSTRUKSI") + "' ,";
                        sql = sql + " '" + FN.strVal(gvCppt, i, "NAMA_TERANG") + "' ,";
                        sql = sql + "  " + ii + " , '" + DB.vUserId + "',sysdate) ";   //FN.strVal(gvCppt, i, "SEQ")
                    }
                    sql = sql + " select * from dual";
                    bool save = ORADB.Execute(ORADB.XE, sql);
                    if (save)
                    {
                        MessageBox.Show("Data CPPT Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    DataTable dt7 = ConnOra.Data_Table_ora("select * from T1_CPPT where anamesa_id = " + anamesaID + " order by to_char(tanggal,'YYYYMMDD')||replace(jam,':','')|| case when ctype ='S' then 1 when ctype ='O' then 2 when ctype ='A' then 3 else 4 end");
                    //DataTable dt7 = ORADB.SetData(ORADB.XE, "select * from T1_CPPT where anamesa_id = " + anamesaID+" order by seq");
                    gcCppt.DataSource = dt7;
                }

            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
        }

        private void btnSimpanObat_Click(object sender, EventArgs e)
        {

            string r_id = "", kode = "", dosis = "", info = "", jumlah = "", id = "", stok = "", con = "", action = "", RECEIPT_ID="";
            string sql_cnt = "", med_cnt = "", sql_update = "", sql_diag = "", diag_cnt = "", harga = "", hari = "", jph = "", info_dosis = "";
            string today = DateTime.Now.ToString("yyyy-MM-dd"); 

            try
            {
                if (gvJadwalObat.RowCount > 0)
                {   
                    bool save = false; int ssave = 0;
                    for (int i = 0; i < gvJadwalObat.RowCount; i++)
                    {
                        r_id = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[0]).ToString();
                        kode = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[3]).ToString();
                        dosis = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[4]).ToString();
                        info = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[5]).ToString();
                        jumlah = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[7]).ToString();
                        stok = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[6]).ToString();
                        con = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[10]).ToString();
                        action = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[9]).ToString();
                        harga = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[12]).ToString();
                        hari = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[11]).ToString();
                        jph = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[13]).ToString();
                        info_dosis = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[14]).ToString();

                        if(jumlah.ToString().Equals("0") || jumlah.ToString().Equals(""))
                        {
                            MessageBox.Show("Jumlah Obat Tidak Boleh Kosong...!!!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        string dte = "", sql = " ";
                        object tgl = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[15]);
                        if (tgl != null && tgl is DateTime)
                        {
                            DateTime selectedDateTime = (DateTime)tgl;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime selectedDateTime = DateTime.Now;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }

                        //DataTable dt = ConnOra.Data_Table_ora("Select RECEIPT_ID from KLINIK.cs_receipt where ID_VISIT = " + visitid + "  ");
                        //if (dt != null && dt.Rows.Count > 0)
                        //{
                        //    RECEIPT_ID = dt.Rows[0]["RECEIPT_ID"].ToString();

                            //id = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[0]).ToString();
                        if(r_id.ToString().Equals(""))
                        {
                            sql = "";
                            sql = sql + " Insert into KLINIK.cs_receipt (rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, INS_JAM, ID_VISIT, ins_date, ins_emp, GRID_NAME,JENIS_OBAT) ";
                            sql = sql + " values(  '" + RMNO + "', to_date('" + dte + "', 'yyyy-MM-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', ";
                            sql = sql + "   '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "',   '" + FN.strVal(gvJadwalObat, i, "INS_JAM") + "' , '" + visitid + "',sysdate, '" + DB.vUserId + "' , 'gvJadwalObat','NONE' ) ";
                            ssave = 2;
                            ORADB.Execute(ORADB.XE, sql);
                           
                        }
                        else
                        {
                            ssave = 1;
                            if (con.ToString().Equals("N")) // receipt_id,  cs_receipt_seq.nextval, "+ RECEIPT_ID.ToString() +", 
                            {
                                sql = "";
                                sql = sql + " Update  KLINIK.cs_receipt ";
                                sql = sql + "    set  insp_date = to_date('" + dte + "', 'yyyy-MM-dd'),  INS_JAM = '" + FN.strVal(gvJadwalObat, i, "INS_JAM") + "' , med_qty = '" + jumlah + "', dosis =  '" + info_dosis + "' , UPD_DATE = sysdate, UPD_EMP = '" + DB.vUserId + "'  ";
                                sql = sql + "  where  RECEIPT_ID =  '" + r_id + "' and GRID_NAME =  'gvJadwalObat' ";

                                ORADB.Execute(ORADB.XE, sql);
                                ssave = 3;
                            } 
                        }  
                    }
                    //sql = sql + " select * from dual";
                    //bool save = ORADB.Execute(ORADB.XE, sql);
                    if (ssave ==  1)
                    {
                        lblobatS.Visible = true;
                        //MessageBox.Show("Jadwal Pemberian Obat Tidak Dapat Diganti, Karena Sudah Confirm!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        lblobatS.Text = "Gagal..Obat Sudah Confirm!!";
                        Blinking(lblobatS,0);
                        //simpansukses(lblobatS, "N");
                    } 
                    else if (ssave == 2)
                    {
                        lblobatS.Visible = true;
                        //MessageBox.Show("Jadwal Pemberian Obat Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblobatS.Text = "Simpan Obat Berhasil"; Blinking(lblobatS,1);
                        //simpansukses(lblobatS, "Y");
                        LoadDataResep();
                    } 
                    else if (ssave == 3)
                    {
                        lblobatS.Visible = true;
                        //MessageBox.Show("Jadwal Pemberian Obat Berhasil di ubah!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblobatS.Text = "Ubah Data Berhasil"; Blinking(lblobatS,1);
                        //simpansukses(lblobatS, "Y");
                        LoadDataResep();
                    } 
                }
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }


            //try
            //{
            //    if (gvJadwalObat.RowCount > 0)
            //    {
            //        DataTable dt = ORADB.SetData(ORADB.XE, "select * from T1_JADWAL_BERI_OBAT where anamesa_id =" + anamesaID + " ");
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            ORADB.Execute(ORADB.XE, "delete from T1_JADWAL_BERI_OBAT where anamesa_id = " + anamesaID + " ");
            //        }

            //        string sql = "insert all ";
            //        for (int i = 0; i < gvJadwalObat.RowCount; i++)
            //        {
            //            string dte = "";
            //            object tgl = gvJadwalObat.GetRowCellValue(i, "TANGGAL");
            //            if (tgl != null && tgl is DateTime)
            //            {
            //                DateTime selectedDateTime = (DateTime)tgl;
            //                dte = selectedDateTime.ToString("yyyy-MM-dd");
            //            }
            //            else
            //            {
            //                DateTime selectedDateTime = DateTime.Now;
            //                dte = selectedDateTime.ToString("yyyy-MM-dd");
            //            }

            //            sql = sql + " into T1_JADWAL_BERI_OBAT (anamesa_id, seq, jenis_obat, nama_obat, dosis, tanggal, jam1, jam2, jam3, jam4, EXTRA, ttd) values ( ";
            //            sql = sql + " " + anamesaID + " ,";
            //            sql = sql + " " + FN.strVal(gvJadwalObat, i, "SEQ") + " ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JENIS_OBAT") + "' ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "NAMA_OBAT") + "' ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "DOSIS") + "' ,";
            //            sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd') ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JAM1") + "' ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JAM2") + "' ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JAM3") + "' ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JAM4") + "' ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "EXTRA") + "' ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "TTD") + "' ) ";
            //        }
            //        sql = sql + " select * from dual";
            //        bool save = ORADB.Execute(ORADB.XE, sql);
            //        if (save)
            //        {
            //            MessageBox.Show("Jadwal Pemberian obat berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }

            //}
            //catch(Exception ex)
            //{
            //    FN.errosMsg(ex.Message, "Error");
            //}
        }

        private void txLain2_CheckedChanged(object sender, EventArgs e)
        {
            if (txLain2.Checked) txAlergi.Enabled = true;
            else txAlergi.Enabled = false;
        }

        private void chkEtc7_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEtc7.Checked) txNyeriHilang.Enabled = true;
            else
            {
                txNyeriHilang.Enabled = false;
                txNyeriHilang.Text = "";
            }
        }

        private void getScoreDewasa(object sender, EventArgs e)
        {
            int totalNilai = 0;

            if (rgRiwayatJatuh.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgRiwayatJatuh.EditValue);
            if (rgDiagnosaSekunder.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgDiagnosaSekunder.EditValue);
            if (rgAltBantuJalan.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgAltBantuJalan.EditValue);
            if (rgInfus.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgInfus.EditValue);
            if (rgGayaJalan.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgGayaJalan.EditValue);
            if (rgstsMental.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgstsMental.EditValue);
            
            txTotalNilai.Text = totalNilai.ToString();

            if (totalNilai < 25)
            {
                txResikoDewasa.Text = "TIDAK BERESIKO";
                lblTindakan.Text = "Perawatan Dasar";
            }
            else if (totalNilai < 51)
            {
                txResikoDewasa.Text = "RESIKO RENDAH";
                lblTindakan.Text = @"Pelaksanaan Intervensi Pencegahan Jatuh Standar";
            }
            else if (totalNilai >= 51)
            {
                txResikoDewasa.Text = "RESIKO TINGGI";
                lblTindakan.Text = @"Pelaksanaan Intervensi Pencegahan Jatuh Resiko Tinggi";
            }
        }

        private void getScoreAnak(object sender, EventArgs e)
        {
            int totalNilai = 0;

            if (rgUsia.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgUsia.EditValue);
            if (rgJenkel.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgJenkel.EditValue);
            if (rgDiagnosis.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgDiagnosis.EditValue);
            if (rgGangguan.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgGangguan.EditValue);
            if (rgFlingkungan.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgFlingkungan.EditValue);
            if (rgSedasiAnestesi.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgSedasiAnestesi.EditValue);
            if (rguseObat.SelectedIndex != -1) totalNilai += Convert.ToInt32(rguseObat.EditValue);

            txScoreAnak.Text = totalNilai.ToString();

            if (totalNilai < 12){
                txResikoAnak.Text = "RESIKO RENDAH";
            }
            else if (totalNilai >= 12){
                txResikoAnak.Text = "RESIKO TINGGI";
            }
        }

        private void getScoreScriningGizi(object sender, EventArgs e)
        {
            if (rgTurunBB.SelectedIndex == 2)
                lebrtbadan.Enabled = true;
            else
            {
                lebrtbadan.Enabled = false;
                lebrtbadan.SelectedText = "";
            }


            int totalNilai = 0;
            if(rgTurunBB.SelectedIndex == 2){
                totalNilai += Convert.ToInt32(lebrtbadan.EditValue.ToString()==""?"0": lebrtbadan.EditValue);
            }
            else{
                if (rgTurunBB.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgTurunBB.EditValue);
            }
            
            if (rgAsupanMakan.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgAsupanMakan.EditValue);
            txScoreScrining.Text = totalNilai.ToString();
        }

        private void chkStsMental2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStsMental2.Checked)
                txStsMental2.Enabled = true;
            else
            {
                txStsMental2.Enabled = false;
                txStsMental2.Text = "";
            }


        }

        private void chkStasMental3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStasMental3.Checked)
                txStsMental3.Enabled = true;
            else
            {
                txStsMental3.Enabled = false;
                txStsMental3.Text = "";
            }
        }

        private void chkEtc12_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEtc12.Checked)
                txKbthnEdukasi.Enabled = true;
            else
            {
                txKbthnEdukasi.Enabled = false;
                txKbthnEdukasi.Text = "";
            }
        }

        private void chkSkalaNyeri_SelectedIndexChanged(object sender, EventArgs e)
        {
            int totalNilai = 0;
            int jumlahCheckboxDipilih = 0;

            for (int i = 0; i < chkSkalaNyeri.Items.Count; i++)
            {
                if (chkSkalaNyeri.GetItemChecked(i))
                {
                    int nilaiCheckbox = Convert.ToInt32(chkSkalaNyeri.Items[i]);
                    totalNilai += nilaiCheckbox;
                    jumlahCheckboxDipilih++;
                }
            }

            if (jumlahCheckboxDipilih > 0)
            {
                double rataRata = (double)totalNilai / jumlahCheckboxDipilih;
                txScorNyeri.Text = rataRata.ToString("0.##");
            }
            else
            {
                txScorNyeri.Text = "0";
            }
        }

        private void chkEtc14_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEtc14.Checked)
                txDiagnoseDtl.Enabled = true;
            else
            {
                txDiagnoseDtl.Enabled = false;
                txDiagnoseDtl.Text = "";
            }
        }

        private void setChart()
        {
            string sqlx = @"SELECT '[' || TO_CHAR (TANGGAL, 'yyyy-MM-dd') || ']' AS TANGGAL,
                                       SUBSTR (TENSI, 1, INSTR (TENSI, '/') - 1) AS SIS,
                                       SUBSTR (TENSI, INSTR (TENSI, '/') + 1) AS DIA,
                                       SUHU
                                  FROM T1_GRAFIK_VITAL WHERE ANAMESA_ID = " + anamesaID+"";
            DataTable dtChart = ConnOra.Data_Table_ora(sqlx); //ORADB.SetData(ORADB.XE, sqlx);
            createChart(dtSelect(dtChart, "TANGGAL", "SIS"), chrVital, 0);
            createChart(dtSelect(dtChart, "TANGGAL", "DIA"), chrVital, 1);
            createChart(dtSelect(dtChart, "TANGGAL", "SUHU"), chrVital, 2);
        }

        private void btnAddVt_Click(object sender, EventArgs e)
        {
            if (dtVital == null) return;

            DataRow newRow = dtVital.NewRow();

            newRow["SEQ"] = ((gvVt.RowCount) + 1).ToString();
            dtVital.Rows.Add(newRow);

            gcVt.DataSource = dtVital;
        }

        private void btnSaveV_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ConnOra.Data_Table_ora("select * from T1_GRAFIK_VITAL where anamesa_id = " + anamesaID + " ");
                //ORADB.SetData(ORADB.XE, "select * from T1_GRAFIK_VITAL where anamesa_id = " + anamesaID+" ");
                if (dt != null && dt.Rows.Count > 0)
                {
                    ORADB.Execute(ORADB.XE, "delete from T1_GRAFIK_VITAL where anamesa_id = " + anamesaID+" ");
                }

                string sql = "insert all ";
                for (int i = 0; i < gvVt.RowCount; i++)
                {
                    string dte = "";
                    object tgl = gvVt.GetRowCellValue(i,"TANGGAL");
                    if (tgl != null && tgl is DateTime)
                    {
                        DateTime selectedDateTime = (DateTime)tgl;
                        dte = selectedDateTime.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        DateTime selectedDateTime = DateTime.Now;
                        dte = selectedDateTime.ToString("yyyy-MM-dd");
                    }

                    sql = sql + " into T1_GRAFIK_VITAL (anamesa_id, tanggal, suhu, tensi, seq) values ( ";
                    sql = sql + " "+anamesaID+", ";
                    sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd'), ";
                    sql = sql + " '" + gvVt.GetRowCellDisplayText(i, "SUHU") + "', ";
                    sql = sql + " '" + gvVt.GetRowCellDisplayText(i, "TENSI") + "', ";
                    sql = sql + " " + gvVt.GetRowCellDisplayText(i, "SEQ") + " ) ";
                }

                sql = sql + " select * from dual ";
                bool save = ORADB.Execute(ORADB.XE, sql);
                if (save)
                {
                    MessageBox.Show("Berhasil disimpan", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    setChart();
                }
            }
            catch
            {

            }
        }

        private void createChart(DataTable dt, ChartControl chart, int sr)
        {
            int srsCount = chart.Series.Count;
            if (dt != null)
            {
                try
                {
                    if (srsCount > 0)
                    {
                        Series srs = chart.Series[sr];
                        addSeries(dt, srs);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                chart.Series[sr].Points.Clear();
            }
        }

        private void addSeries(DataTable dt, Series sr)
        {
            sr.Points.Clear();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    string lbl = dt.Rows[i][0].ToString();
                    string qty = dt.Rows[i][1].ToString();
                    sr.Points.Add(new SeriesPoint(lbl, Convert.ToDouble(qty)));
                }
                //sr.Label.TextPattern = "{V}"; /* "{A}:{V}~{VP:P0}";*/
            }
        }

        private DataTable dtSelect(DataTable dt, params string[] column)
        {
            if (dt.Rows.Count > 0)
            {
                DataView view = new DataView(dt);
                DataTable selected = view.ToTable("DGM", false, column);
                return selected;
            }
            else return null;

        }

        private void gcVt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                gvVt.DeleteRow(gvVt.FocusedRowHandle);
            }
        }

        private void gcJadwalObat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                gvJadwalObat.DeleteRow(gvJadwalObat.FocusedRowHandle);
            }
        }

        private void gcCppt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                gvCppt.DeleteRow(gvCppt.FocusedRowHandle);
            }
        }

        private void gcObtPlng_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                gvObtPlng.DeleteRow(gvObtPlng.FocusedRowHandle);
            }
        }

        private void chkDoc1_CheckedChanged(object sender, EventArgs e)
        {
            txDoc1.Enabled = chkDoc1.Checked;
            if (chkDoc1.Checked == false)
                txDoc1.Text = "";
        }

        private void chkDoc2_CheckedChanged(object sender, EventArgs e)
        {
            txDoc2.Enabled = chkDoc2.Checked;
            txDoc3.Enabled = chkDoc2.Checked;

            if (chkDoc2.Checked == false)
            {
                txDoc2.Text = "";
                txDoc3.Text = "";
            }

        }

        private void chkDoc3_CheckedChanged(object sender, EventArgs e)
        {
            txDoc4.Enabled = chkDoc3.Checked;
            if (chkDoc3.Checked == false)
                txDoc4.Text = "";
        }

        private void chkDoc4_CheckedChanged(object sender, EventArgs e)
        {
            txDoc5.Enabled = chkDoc4.Checked;
            if (chkDoc4.Checked == false)
                txDoc5.Text = "";
        }

        private void chkDoc5_CheckedChanged(object sender, EventArgs e)
        {
            txDoc6.Enabled = chkDoc5.Checked;
            if (chkDoc5.Checked == false)
                txDoc6.Text = "";
        }

        private void chkEtc5_CheckedChanged(object sender, EventArgs e)
        {
            txStsPsikologi.Enabled = chkEtc5.Checked;
            if (chkEtc5.Checked == false)
                txStsPsikologi.Text = "";
        }

        private void grdMain_DoubleClick(object sender, EventArgs e)
        {
          
            //if (gvwMain.RowCount < 1)
            //    return;

            //anamesaID = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "ANAMNESA_ID");

            //if (anamesaID.ToString().Equals("") || anamesaID.ToString().Equals("0"))
            //    return;

            //mainTab.Enabled = true;
            //FN.ResetInput(mainTab);


            //visitid = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "ID_VISIT");
            //headid = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "HEAD_ID");
            //RMNO = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "RM_NO");
            //pasienno = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "PATIENT_NO");
            //type_s = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "GROUP_PATIENT");
            //inpatient_id = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "INPATIENT_ID");
            //fnama = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "NAME");
            //labelControl106.Text = RMNO;
            //labelControl107.Text = fnama; 

            //if (type_s.ToString().Equals("Umum"))
            //{
            //    type_s = "U";
            //    panelControl3.Visible = false; 
            //    splitContainerControl7.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2; // Hanya Panel 2 yang terlihat
            //    splitContainerControl8.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            //} 
            //else if ( type_s.ToString().Equals("Asuransi"))
            //{
            //    type_s = "A";
            //    panelControl3.Visible = false;
            //    splitContainerControl7.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2; // Hanya Panel 2 yang terlihat
            //    splitContainerControl8.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            //}
            //else
            //{
            //    panelControl3.Visible = true;
            //    splitContainerControl7.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both; // Hanya Panel 2 yang terlihat
            //    splitContainerControl8.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
            //    type_s = "B";
            //}
            ////LoadItemLayanan();
            
            //DataListObat(type_s);

            //// dtJadwalObat = ORADB.SetData(ORADB.XE, "select * from T1_JADWAL_BERI_OBAT where anamesa_id =" + anamesaID + "");
            //dtCppt = ConnOra.Data_Table_ora("SELECT * FROM ( select a.*,  case when ctype = 'S' then tanggal || 1 when ctype = 'O' then tanggal || 2 when ctype = 'A' then tanggal || 3 when ctype = 'P' then tanggal || 4 END SSORT from T1_CPPT a where anamesa_id = " + anamesaID + " ) ORDER BY SSORT   ");
            //dtObatPulang = ConnOra.Data_Table_ora("select * from T1_OBAT_PULANG where anamesa_id =" + anamesaID + " ");
            //dtVital = ConnOra.Data_Table_ora("select * from T1_GRAFIK_VITAL where anamesa_id =" + anamesaID + " ");
            ////ORADB.SetData(ORADB.XE, "select * from T1_CPPT where anamesa_id =" + anamesaID + " "); 
            ////ORADB.SetData(ORADB.XE,  "select * from T1_OBAT_PULANG where anamesa_id =" + anamesaID + " ");  
            ////ORADB.SetData(ORADB.XE, "select * from T1_GRAFIK_VITAL where anamesa_id =" + anamesaID + " ");

            //try
            //{
            //    if (ConnOra.Data_Table_ora("select * from T1_RAWAT_INAP1 where anamesa_id =" + anamesaID + " ").Rows.Count > 0)
            //    //if (ORADB.SetData(ORADB.XE, "select * from T1_RAWAT_INAP1 where anamesa_id = " + anamesaID + "").Rows.Count > 0)
            //    {
            //        getData(anamesaID);
            //    }
            //    else
            //    {
            //        string newId = ORADB.getData(ORADB.XE, "select rawat_inap_seq.NEXTVAL new_id from dual ", "NEW_ID");
            //        string newId2 = ORADB.getData(ORADB.XE, "select resiko_jatuh_seq.NEXTVAL new_id from dual ", "NEW_ID");
            //        List<string> sql = new List<string>();
            //        sql.Add("insert into T1_RAWAT_INAP1 (id, anamesa_id) values (" + newId + "," + anamesaID + ")");
            //        sql.Add("insert into T1_RAWAT_INAP2 (id, anamesa_id) values (" + newId + "," + anamesaID + ")");
            //        sql.Add("insert into T1_PERENCANAAN_PULANG (id, anamesa_id) values (" + newId + "," + anamesaID + ")");
            //        sql.Add("insert into T1_RESUME_PULANG (id, anamesa_id) values (" + newId + "," + anamesaID + ")");
            //        sql.Add("insert into T1_ASESMEN_GIZI (anamesa_id) values (" + anamesaID + ")");
            //        ORADB.DbTrans(ORADB.XE, sql);
            //    }
            //    btnInputData.Enabled = false;
            //    LoadItemLayananType(type_s);
            //}
            //catch (Exception ex)
            //{
            //    FN.errosMsg(ex.Message, "Error");
            //}
        }

        private void gvwMain_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

      

        private void bdelmedis_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                  "Message",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_treatment_head where HEAD_ID = '" + FN.strVal(gvMedis, gvMedis.FocusedRowHandle, "HEAD_ID") + "' and STATUS ='OPN' and PAY_STATUS ='OPN' ");

                if (dt != null && dt.Rows.Count > 0)
                {
                    string sql_delete = "";

                    sql_delete = "";
                    sql_delete = sql_delete + " update  KLINIK.cs_treatment_detail   set f_active = 'N', UPD_EMP = '" + DB.vUserId + "', UPD_DATE = SYSDATE  ";
                    sql_delete = sql_delete + "  where DETAIL_ID= '" + FN.strVal(gvMedis, gvMedis.FocusedRowHandle, "DETAIL_ID") + "' AND  f_active = 'Y'  ";

                    try
                    {
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                        oraConnect.Open();
                        cm.ExecuteNonQuery();
                        oraConnect.Close();
                        cm.Dispose();

                        //MessageBox.Show("Query Exec : " + sql_delete);
                        gvMedis.DeleteRow(gvMedis.FocusedRowHandle);
                        //MessageBox.Show("Data Berhasil dihapus");
                        labelControl103.Visible = true;
                        labelControl103.Text = "Berhasil Dihapus";
                        Blinking(labelControl103, 1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }
                else
                {
                    labelControl103.Visible = true;
                    labelControl103.Text = "Gagal..Status Closed.";
                    Blinking(labelControl103, 0);
                    //MessageBox.Show("Maaf Data Close Tidak Dapat dihapus...! ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                } 
            }
        } 

        private void bdelnone_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                 "Message",
                  MessageBoxButtons.YesNo,
                  MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_treatment_head where HEAD_ID = '" + FN.strVal(gvVisitDoc, gvVisitDoc.FocusedRowHandle, "HEAD_ID") + "' and STATUS ='OPN' and PAY_STATUS ='OPN' ");

                if (dt != null && dt.Rows.Count > 0)
                {
                    string sql_delete = "";

                    sql_delete = "";
                    sql_delete = sql_delete + " update  KLINIK.cs_treatment_detail   set f_active = 'N', UPD_EMP = '" + DB.vUserId + "', UPD_DATE = SYSDATE  ";
                    sql_delete = sql_delete + "  where DETAIL_ID= '" + FN.strVal(gvVisitDoc, gvVisitDoc.FocusedRowHandle, "DETAIL_ID") + "' AND  f_active = 'Y'  ";

                    try
                    {
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                        oraConnect.Open();
                        cm.ExecuteNonQuery();
                        oraConnect.Close();
                        cm.Dispose();

                        //MessageBox.Show("Query Exec : " + sql_delete);
                        gvVisitDoc.DeleteRow(gvVisitDoc.FocusedRowHandle);
                        //MessageBox.Show("Data Berhasil dihapus");
                        labelControl104.Visible = true;
                        labelControl104.Text = "Visit Berhasil Dihapus";
                        Blinking(labelControl104, 1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }
                else
                {
                    //MessageBox.Show("Maaf Data Close Tidak Dapat dihapus...! ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelControl104.Visible = true;
                    labelControl104.Text = "Gagal..Pasien Closed.";
                    Blinking(labelControl104, 0);
                }
            }
        }

        private void bsavenone_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvVisitDoc.RowCount > 0)
                {
                    DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_treatment_detail where HEAD_ID = '" + headid + "'  ");
                    //ORADB.SetData(ORADB.XE, "Select * from KLINIK.cs_treatment_detail where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvVisitDoc' ");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //    ORADB.Execute(ORADB.XE, " insert into KLINIK.cs_treatment_detail_del select a.*, sysdate, '" + DB.vUserId + "' as emp from KLINIK.cs_treatment_detail a  where  HEAD_ID = '" + headid + "'  and GRID_NAME = 'gvVisitDoc' ");
                        //    ORADB.Execute(ORADB.XE, " Delete from KLINIK.cs_treatment_detail  where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvVisitDoc' ");
                        //}

                        string sql = ""; bool save = false;
                        for (int i = 0; i < gvVisitDoc.RowCount; i++)
                        {
                            string dte = "", detailid = "", spay = "";
                            object tgl = gvVisitDoc.GetRowCellValue(i, "TANGGAL"); 
                            detailid = FN.strVal(gvVisitDoc, i, "DETAIL_ID");
                            spay = FN.strVal(gvVisitDoc, i, "PAY_STATUS");

                            if (tgl != null && tgl is DateTime)
                            {
                                DateTime selectedDateTime = (DateTime)tgl;
                                dte = selectedDateTime.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                DateTime selectedDateTime = DateTime.Now;
                                dte = selectedDateTime.ToString("yyyy-MM-dd");
                            }

                            if (detailid.ToString().Equals(""))
                            {
                                sql = "";
                                sql = sql + " insert into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME, ID_DOKTER) values ( ";
                                sql = sql + " CS_TREATMENT_DETAIL_SEQ.nextval ,'" + FN.strVal(gvVisitDoc, i, "HEAD_ID") + "','" + FN.strVal(gvVisitDoc, i, "TREAT_ITEM_ID") + "'  ,";
                                sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd'), '" + FN.strVal(gvVisitDoc, i, "TREAT_QTY") + "', '" + FN.strVal(gvVisitDoc, i, "TREAT_ITEM_PRICE") + "', " + Convert.ToInt32(FN.strVal(gvVisitDoc, i, "TREAT_QTY")) * Convert.ToInt32(FN.strVal(gvVisitDoc, i, "TREAT_ITEM_PRICE")) + ", ";
                                sql = sql + " '" + FN.strVal(gvVisitDoc, i, "REMARKS") + "' ,  sysdate, '" + DB.vUserId + "', '" + FN.strVal(gvVisitDoc, i, "JAM") + "' , 'gvVisitDoc' , '" + FN.strVal(gvVisitDoc, i, "ID_DOKTER") + "' )";
                            }
                            else
                            {
                                sql = "";
                                sql = sql + " update KLINIK.cs_treatment_detail  set treat_date =  TO_DATE('" + dte + "', 'yyyy-MM-dd'), TREAT_JAM = '" + FN.strVal(gvVisitDoc, i, "JAM") + "', ";
                                sql = sql + "        remarks   = '" + FN.strVal(gvVisitDoc, i, "REMARKS") + "', UPD_DATE = sysdate, UPD_EMP = '" + DB.vUserId + "'  ";
                                sql = sql + "  where detail_id   = " + detailid + " ";
                            }
                            save = ORADB.Execute(ORADB.XE, sql);

                            //                    command.CommandText = " insert into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp) values
                            //  ( '" + seq_val + "', '" + head + "', '" + nama_laya + "', to_date('" + ldate + "', 'yyyy-MM-dd'), " + qty + ", " + item_price + ", " + price + ", '" + remarks + "', sysdate, '" + DB.vUserId + "') ";
                            //                    command.ExecuteNonQuery();

                          
                        }
                        //sql = sql + " select * from dual";
                        //bool save = ORADB.Execute(ORADB.XE, sql);
                        if (save)
                        {
                            //MessageBox.Show("Data Kunjungan Dokter Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            labelControl104.Visible = true;
                            labelControl104.Text = "Visit Berhasil Disimpan";
                            Blinking(labelControl104, 1);
                            //return;
                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
        }

        private void gvVisitDoc_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            string a = "", tmp_stat = "", date = "", que = "", rm_no = "", no_visit = "";
               
            if (e.Column.Caption == "Pelayanan Visit")
            {
                a = view.GetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_ID"]).ToString();
                no_visit = view.GetRowCellValue(e.RowHandle, view.Columns["ID_VISIT"]).ToString();
                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();

                string sql_ = "", sql_head = "", group_id = "", price = "", head_id = "", stbyr = "";
                sql_ = " select treat_group_id, treat_item_price from KLINIK.cs_treatment_item where treat_item_id = " + a + " ";

                DataTable dt0 = ConnOra.Data_Table_ora(sql_);

                //OleDbConnection oraConnect0 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_, oraConnect0);
                //DataTable dt0 = new DataTable();
                //adOra0.Fill(dt0);
                if (dt0.Rows.Count > 0)
                {
                    group_id = dt0.Rows[0]["TREAT_GROUP_ID"].ToString();
                    price = dt0.Rows[0]["TREAT_ITEM_PRICE"].ToString();
                }

                sql_head = " select head_id, pay_status from KLINIK.cs_treatment_head where ID_VISIT = '" + visitid + "'  ";
                DataTable dt1 = ConnOra.Data_Table_ora(sql_head);
                //OleDbConnection oraConnect1 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra1 = new OleDbDataAdapter(sql_head, oraConnect1);
                //DataTable dt1 = new DataTable();
                //adOra1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    head_id = dt1.Rows[0]["HEAD_ID"].ToString();
                    stbyr = dt1.Rows[0]["PAY_STATUS"].ToString();
                }

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns["HEAD_ID"], head_id);
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_GROUP_ID"], group_id);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[2], a);
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_QTY"], "1");
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_PRICE"], price);
                    view.SetRowCellValue(e.RowHandle, view.Columns["PAY_STATUS"], stbyr);
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
                }

            }

            if (e.Column.Caption == "Note Visit")
            {
                string tmp_stat2 = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                if (tmp_stat2 == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
                }
            }
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gvJadwalObat_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I"); 
            view.SetRowCellValue(e.RowHandle, view.Columns[15], DateTime.Now);
            view.SetRowCellValue(e.RowHandle, view.Columns[16], DateTime.Now.ToString("HH:mm"));
            view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
        }

        private void gvJadwalObat_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Kode Dosis" || e.Column.Caption == "Dosis" || e.Column.Caption == "Info" || e.Column.Caption == "Qty" || e.Column.Caption == "Tanggal" || e.Column.Caption == "Jam")
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

        private void gvJadwalObat_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                   "Message",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_receipt where ID_VISIT = '" + visitid + "' AND  f_active = 'Y' and receipt_id = '" + FN.strVal(gvJadwalObat, gvJadwalObat.FocusedRowHandle, "RECEIPT_ID") + "' and CONFIRM ='Y'"); 

                if (dt != null && dt.Rows.Count > 0)
                {
                    //MessageBox.Show("Maaf Data Confirm Tidak Dapat dihapus...! ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lblobatS.Visible = true;
                    lblobatS.Text = "Gagal..Obat Sudah Confirm";
                    Blinking(lblobatS, 0);
                }
                else
                {
                    string sql_delete = "";

                    sql_delete = "";
                    sql_delete = sql_delete + " update KLINIK.cs_receipt  set f_active = 'N', UPD_EMP = '" + DB.vUserId + "',UPD_DATE = SYSDATE  ";
                    sql_delete = sql_delete + "  where ID_VISIT = '" + visitid + "' and receipt_id = '" + FN.strVal(gvJadwalObat, gvJadwalObat.FocusedRowHandle, "RECEIPT_ID") + "' AND  f_active = 'Y' and CONFIRM ='N' and GRID_NAME='gvJadwalObat'   ";

                    try
                    {
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                        oraConnect.Open();
                        cm.ExecuteNonQuery();
                        oraConnect.Close();
                        cm.Dispose();

                        //MessageBox.Show("Query Exec : " + sql_delete);
                        gvJadwalObat.DeleteRow(gvJadwalObat.FocusedRowHandle);
                        //MessageBox.Show("Data Berhasil dihapus");
                        lblobatS.Visible = true;
                        //MessageBox.Show("Jadwal Pemberian Obat Tidak Dapat Diganti, Karena Sudah Confirm!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        lblobatS.Text = "Hapus Obat Berhasil";
                        Blinking(lblobatS, 1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }
                    
            }
        }

       
        private void simpleButton7_Click(object sender, EventArgs e)
        {

            string r_id ="", kode = "", dosis = "", info = "", jumlah = "", id = "", stok = "", con = "", action = "", info_cara ="" ;
            string sql_cnt = "", med_cnt = "", sql_update = "", sql_diag = "", diag_cnt = "", harga = "", hari = "", jph = "", info_dosis = "";
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                if (gvObtPlng.RowCount > 0)
                { 
                    string sql = " "; bool save = false; int ssave = 0;
                    for (int i = 0; i < gvObtPlng.RowCount; i++)
                    {
                        r_id = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[0]).ToString();
                        kode = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[1]).ToString();
                        dosis = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[4]).ToString();
                        info = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[5]).ToString();
                        jumlah = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[7]).ToString();
                        stok = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[6]).ToString();
                        con = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[10]).ToString();
                        action = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[9]).ToString();
                        harga = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[12]).ToString();
                        hari = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[11]).ToString();
                        jph = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[13]).ToString();
                        info_dosis = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[14]).ToString();
                        info_cara = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[21]).ToString();

                        string dte = "";
                        object tgl = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[15]);
                        if (tgl != null && tgl is DateTime)
                        {
                            DateTime selectedDateTime = (DateTime)tgl;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime selectedDateTime = DateTime.Now;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }

                        if (r_id.ToString().Equals(""))
                        {
                            sql = "";
                            sql = sql + " insert into KLINIK.cs_receipt ( rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, INS_JAM, ID_VISIT, ins_date, ins_emp, GRID_NAME, FLAG_PULANG,JENIS_OBAT) ";
                            sql = sql + " values('" + RMNO + "', to_date('" + dte + "', 'yyyy-MM-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', ";
                            sql = sql + "   '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "',   '" + FN.strVal(gvObtPlng, i, "INS_JAM") + "' , '" + visitid + "',sysdate, '" + DB.vUserId + "' , 'gvObtPlng', 'Y','NONE' ) ";
                             
                            ssave = 2;
                            ORADB.Execute(ORADB.XE, sql);

                        }
                        else
                        {
                            ssave = 1;
                            if (con.ToString().Equals("N")) // receipt_id,  cs_receipt_seq.nextval, "+ RECEIPT_ID.ToString() +", 
                            {
                                sql = "";
                                sql = sql + " Update  KLINIK.cs_receipt ";
                                sql = sql + "    set  insp_date = to_date('" + dte + "', 'yyyy-MM-dd'),  INS_JAM = '" + FN.strVal(gvObtPlng, i, "INS_JAM") + "' , med_qty = '" + jumlah + "', dosis =  '" + info_dosis + "' ";
                                sql = sql + "  where  RECEIPT_ID =  '" + r_id + "' and GRID_NAME =  'gvObtPlng' ";

                                ORADB.Execute(ORADB.XE, sql);
                                ssave = 3;
                            }
                        } 
                    }
                    //sql = sql + " select * from dual";
                    //bool save = ORADB.Execute(ORADB.XE, sql);
                     
                    if (ssave == 1)
                    {
                        //MessageBox.Show("Jadwal Pemberian Obat Pulang Tidak Dapat Diganti, Karena Sudah Confirm!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                         
                        labelControl102.Visible = true; 
                        labelControl102.Text = "Gagal..Obat Sudah Confirm!!";
                        Blinking(labelControl102, 0);
                    }
                    else if (ssave == 2)
                    {
                        //MessageBox.Show("Jadwal Pemberian Obat Pulang Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        labelControl102.Visible = true;
                        labelControl102.Text = "Simpan Obat Berhasil"; Blinking(labelControl102, 1);
                        LoadDataResep();
                    }
                    else if (ssave == 3)
                    {
                        labelControl102.Visible = true;
                        labelControl102.Text = "Ubah Data Berhasil"; Blinking(labelControl102, 1);
                        LoadDataResep();
                    }
                }
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
             
        }

        private void gvObtPlng_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[15], DateTime.Now);
            view.SetRowCellValue(e.RowHandle, view.Columns[16], DateTime.Now.ToString("HH:mm"));
            view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
        }

        private void gvObtPlng_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvObtPlng_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Kode Dosis" || e.Column.Caption == "Dosis" || e.Column.Caption == "Info" || e.Column.Caption == "Qty" || e.Column.Caption == "Tanggal" || e.Column.Caption == "Jam")
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

        private void gvObtPlng_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();

            if (a.ToString().Equals(""))
                return;

            string dte = "";

            DateTime selectedDateTime = DateTime.Now;
            dte = selectedDateTime.ToString("yyyy-MM-dd");

            if (e.Column.Caption == "Nama Obat" && (a.Substring(0, 2) == "BP" || a.Substring(0, 2) == "UM"))
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                string sql_medcd = " ", sql_med = "", med_cd = "", med_name = "", med_group = "", med_stok = "", med_uom = "", cek_stok = "", sql_for = "";
                 
                sql_medcd = " select " +
                            " max(klinik.FN_CS_INIT_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') +  " +
                            " klinik.FN_CS_TRX_IN(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') -  " +
                            " klinik.FN_CS_TRX_OUT(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') - " +
                            " klinik.FN_CS_REQ_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "')) stock from dual ";

                datstock = ConnOra.Data_Table_ora(sql_medcd);

                if (datstock.Rows.Count > 0)
                    cek_stok = datstock.Rows[0]["stock"].ToString();
                else
                    cek_stok = "0"; 

                sql_med = " select med_cd, initcap(med_name) med_name, med_group, '" + cek_stok + "' stock, initcap(uom) uom " + 
                          " from KLINIK.cs_medicine a  " +
                          " where status = 'A'  " +
                          " and med_cd = '" + a + "' ";

                DataTable dt = ConnOra.Data_Table_ora(sql_med);

                //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra = new OleDbDataAdapter(sql_med, oraConnect);
                //DataTable dt = new DataTable();
                //adOra.Fill(dt);

                med_cd = dt.Rows[0]["med_cd"].ToString();
                med_name = dt.Rows[0]["med_name"].ToString();
                med_group = dt.Rows[0]["med_group"].ToString();
                med_stok = dt.Rows[0]["stock"].ToString();
                med_uom = dt.Rows[0]["uom"].ToString();

                sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1  and MINUS_STOK ='Y'  and  b.med_cd = '" + med_cd + "' and POLI_CD = 'POL0001' and ATT1 = decode('" + type_s + "','B','BPJS','U','UMUM','ASURANSI')";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for);

                //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
                //DataTable dtf = new DataTable();
                //adOraf.Fill(dtf);
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

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");  
                    view.SetRowCellValue(e.RowHandle, view.Columns[2], med_group);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "U"); 
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok); 
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
                view.SetRowCellValue(e.RowHandle, view.Columns[7], 1); 
                view.SetRowCellValue(e.RowHandle, view.Columns[14], "1x1");
            }

            if (e.Column.Caption == "Formula")
            {
                string medicine_cd = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string formula_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string reg_dt = dte.ToString();
                string rm = dte.ToString();
                string que = dte.ToString();
                string stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                string kode = "", sql_pilihan = "";
 
                sql_pilihan = " select med_cd from KLINIK.cs_formula where formula_id = '" + formula_cd + "' ";
                DataTable dtf2 = ConnOra.Data_Table_ora(sql_pilihan); 

                if (dtf2.Rows.Count > 0)
                {
                    kode = dtf2.Rows[0]["med_cd"].ToString();

                }
                else
                {
                    kode = "";
                }

                if (kode == medicine_cd)
                {
                    //view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                    //view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                }
                else
                {
                    MessageBox.Show("Kode Formula tidak valid");
                    return;
                    //LoadDataResep();
                } 
            }

            if (e.Column.Caption == "Qty")
            {
                string sql_for = "", med_price = "", qty = "", tmp_stat = "";
                string for_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string tmp_hari = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                string cstock = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                int tot_hari = 0, tot_harga = 0, istock = 0;

                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                sql_for = " select med_price, qty from KLINIK.cs_formula where formula_id = '" + for_cd + "' and MINUS_STOK ='Y' ";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for);
                  
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
                    tmp_hari = "1";
                }

                tot_hari = Convert.ToInt32(tmp_hari); //Convert.ToInt32(tmp_hari) * Convert.ToInt32(qty);
                tot_harga = Convert.ToInt32(med_price); //Convert.ToInt32(tmp_hari) *

                if (!cstock.ToString().Equals(""))
                {
                    istock = Convert.ToInt32(cstock);
                    if (istock - Convert.ToInt32(qty) < 0)
                    {
                        MessageBox.Show("Stok Obat Kosong. Tidak dapat dipilih..!!!");
                        view.DeleteRow(view.FocusedRowHandle);
                        return;
                    }
                    else
                    {
                        if (tmp_stat == "I")
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                            view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                        }
                        else
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                            view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                        }
                    }
                }
            }
        }

        private void txTbPb_TextChanged(object sender, EventArgs e)
        {

            //double p_tbb = Convert.ToDouble(txBB.Text);
            //double p_tpb = Convert.ToDouble(txTbPb.Text);
            double p_imt = 0;

            if (txBB.Text.ToString().Length > 0 && txTbPb.Text.ToString().Length > 0)
            {
                double p_tbb = Convert.ToDouble(txBB.Text.ToString());
                double p_tpb = Convert.ToDouble(txTbPb.Text.ToString());
                //double p_imt = 0;

                if (p_tbb.ToString().Equals("") || txBB.Text == string.Empty)
                    return;
                else if (p_tbb.ToString().Equals("") || txTbPb.Text == string.Empty)
                    return;
                else if (p_tbb < 0)
                    return;
                else if (p_tpb < 0)
                    return;
                else
                    p_imt = Math.Round(((p_tbb / (p_tpb * p_tpb)) * 10000), 2); //.ToString("0.00");

                txImt.Text = p_imt.ToString();
                if (Convert.ToDouble(p_imt) < 18.5)
                    txGstKet.Text = "Berat Badan Kurang";
                else if (Convert.ToDouble(p_imt) >= 18.5 && Convert.ToDouble(p_imt) < 23)
                    txGstKet.Text = "Berat Badan Normal";
                else if (Convert.ToDouble(p_imt) >= 23 && Convert.ToDouble(p_imt) < 25)
                    txGstKet.Text = "Kelebihan Berat Badan";
                else if (Convert.ToDouble(p_imt) >= 25 && Convert.ToDouble(p_imt) < 30)
                    txGstKet.Text = "Obesitas 1";
                else if (Convert.ToDouble(p_imt) >= 30)
                    txGstKet.Text = "Obesitas 2";
                else
                    txGstKet.Text = "Tidak Terklasifikasi";
            }


            //if (txBB.Text == "" || txBB.Text == string.Empty)
            //    return;
            //else if (txTbPb.Text == "" || txTbPb.Text == string.Empty)
            //    return;
            //else if (Convert.ToDouble(txBB.Text) < 0)
            //    return;
            //else if (Convert.ToDouble(txTbPb.Text) < 0)
            //    return; 
            //else
            //    txImt.Text = ((Convert.ToDouble(txBB.Text) / (Convert.ToDouble(txTbPb.Text) * Convert.ToDouble(txTbPb.Text))) * 10000).ToString("0.00");

            //if (Convert.ToDouble(txImt.Text) < 18.5)
            //    txGstKet.Text = "Berat Badan Kurang";
            //else if (Convert.ToDouble(txImt.Text) >= 18.5 && Convert.ToDouble(txImt.Text) < 23)
            //    txGstKet.Text = "Berat Badan Normal";
            //else if (Convert.ToDouble(txImt.Text) >= 23 && Convert.ToDouble(txImt.Text) < 25)
            //    txGstKet.Text = "Kelebihan Berat Badan";
            //else if (Convert.ToDouble(txImt.Text) >= 25 && Convert.ToDouble(txImt.Text) < 30)
            //    txGstKet.Text = "Obesitas 1";
            //else if (Convert.ToDouble(txImt.Text) >= 30)
            //    txGstKet.Text = "Obesitas 2";
            //else
            //    txGstKet.Text = "Tidak Terklasifikasi";

        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                   "Message",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_receipt where ID_VISIT = '" + visitid + "' AND  f_active = 'Y' and receipt_id = '" + FN.strVal(gvObtPlng, gvObtPlng.FocusedRowHandle, "RECEIPT_ID") + "' and CONFIRM ='Y' and GRID_NAME='gvObtPlng'");

                if (dt != null && dt.Rows.Count > 0)
                {
                    MessageBox.Show("Maaf Data Confirm Tidak Dapat dihapus...! ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string sql_delete = "";

                    sql_delete = "";
                    sql_delete = sql_delete + " update KLINIK.cs_receipt  set f_active = 'N', UPD_EMP = '" + DB.vUserId + "',UPD_DATE = SYSDATE  ";
                    sql_delete = sql_delete + "  where ID_VISIT = '" + visitid + "' and receipt_id = '" + FN.strVal(gvObtPlng, gvObtPlng.FocusedRowHandle, "RECEIPT_ID") + "' AND  f_active = 'Y' and CONFIRM ='N' and GRID_NAME='gvObtPlng'   ";

                    try
                    {
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                        oraConnect.Open();
                        cm.ExecuteNonQuery();
                        oraConnect.Close();
                        cm.Dispose();

                        //MessageBox.Show("Query Exec : " + sql_delete);
                        gvObtPlng.DeleteRow(gvObtPlng.FocusedRowHandle);
                        MessageBox.Show("Data Berhasil dihapus");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }

            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            loadDataAnamnesa();
            LoadItemLayanan();
        } 
        private void stsDefault()
        {
            if (chDefault.Checked)
            {
                rgSakitLalu.SelectedIndex = 0;
                rgPernahRawat.SelectedIndex = 0;
                rgPrnhOperasi.SelectedIndex = 0;
                rgRwSktKlrg.SelectedIndex = 0;
                rgKetergantungan.SelectedIndex = 0;
                rgRiwayatKerja.SelectedIndex = 0;
                rgAlergi.SelectedIndex = 0;
                rgRiwayatObat.SelectedIndex = 0;
                rgKeluhan.SelectedIndex = 0;
                rgGigiPalsu.SelectedIndex = 2;
                rgMual.SelectedIndex = 0;
                rgMuntah.SelectedIndex = 0;
                rgPendengaran.SelectedIndex = 0;
                rgPenglihatan.SelectedIndex = 0;
                rgDefekasi.SelectedIndex = 0;
                rgMiksi.SelectedIndex = 0;
                rgKulit.SelectedIndex = 0;
                rbDekubitus.SelectedIndex = 0;
                rgPeriksaKhusus.SelectedIndex = 1;
                checkBox36.CheckState = CheckState.Checked;
                ckStsMental1.CheckState = CheckState.Checked;
                rgHubKluarga.SelectedIndex = 0;
                rgTmpTinggal.SelectedIndex = 0;
                rgHmbtanBljr.SelectedIndex = 0;
                rgPnrjemah.SelectedIndex = 0;
                checkBox7.CheckState = CheckState.Checked;
                rgSedia.SelectedIndex = 1;
                rgResikoCedera.SelectedIndex = 0;
                rgMnrimaInfo.SelectedIndex = 0;
                rgMobilisasi.SelectedIndex = 0;
                rgNyeri.SelectedIndex = 1;
                rgTurunBB.SelectedIndex = 0;
                rgAsupanMakan.SelectedIndex = 0;
                rgDiagnoseKh.SelectedIndex = 0;
                rgLapor_tr_Gizi.SelectedIndex = 1; 
            }
        }

        private void gvObatUmum_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();

            if (a.ToString().Equals(""))
                return;

            string dte = "";

            DateTime selectedDateTime = DateTime.Now;
            dte = selectedDateTime.ToString("yyyy-MM-dd");

            if (e.Column.Caption == "Nama Obat" && (a.Substring(0, 2) == "BP" || a.Substring(0, 2) == "UM"))
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                string sql_medcd = " ", sql_med = "", med_cd = "", med_name = "", med_group = "", med_stok = "", med_uom = "", cek_stok = "", sql_for = "";

                sql_medcd = " select " +
                            " max(klinik.FN_CS_INIT_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') +  " +
                            " klinik.FN_CS_TRX_IN(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') -  " +
                            " klinik.FN_CS_TRX_OUT(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') - " +
                            " klinik.FN_CS_REQ_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "')) stock from dual ";

                datstock = ConnOra.Data_Table_ora(sql_medcd);

                if (datstock.Rows.Count > 0)
                    cek_stok = datstock.Rows[0]["stock"].ToString();
                else
                    cek_stok = "0";

                sql_med = " select med_cd, initcap(med_name) med_name, med_group, '" + cek_stok + "' stock, initcap(uom) uom " +
                          " from KLINIK.cs_medicine a  " +
                          " where status = 'A'  " +
                          " and med_cd = '" + a + "' ";

                DataTable dt = ConnOra.Data_Table_ora(sql_med);
                 
                med_cd = dt.Rows[0]["med_cd"].ToString();
                med_name = dt.Rows[0]["med_name"].ToString();
                med_group = dt.Rows[0]["med_group"].ToString();
                med_stok = dt.Rows[0]["stock"].ToString();
                med_uom = dt.Rows[0]["uom"].ToString();

                sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1  and  b.med_cd = '" + med_cd + "' and POLI_CD ='POL0001' and att1  =   'UMUM'";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for); 
                listFormulaU.Clear(); 
                for (int i = 0; i < dtf.Rows.Count; i++)
                {
                    listFormulaU.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                }
                if (dtf.Rows.Count == 1)
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], dtf.Rows[0]["formula_id"].ToString());
                else
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], "");

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
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
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[7], "0");
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }

                view.SetRowCellValue(e.RowHandle, view.Columns[7], 1);
                //view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                //view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                //view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[14], "1x1");

            }

            if (e.Column.Caption == "Formula")
            {
                string medicine_cd = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string formula_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string reg_dt = dte.ToString();
                string rm = dte.ToString();
                string que = dte.ToString();
                string stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                string kode = "", sql_pilihan = "";

                //if (stat == "I")
                //{
                //    //view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                //    view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                //    view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                //    view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                //}
                //else
                //{
                    sql_pilihan = " select med_cd from KLINIK.cs_formula where formula_id = '" + formula_cd + "' ";
                    DataTable dtf = ConnOra.Data_Table_ora(sql_pilihan); 
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
                        //view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                        //view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                        //view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                        //view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                    }
                    else
                    {
                        MessageBox.Show("Kode Formula tidak valid");
                        return; 
                    }
                //}
            }

            if (e.Column.Caption == "Qty")
            {
                string sql_for = "", med_price = "", qty = "", tmp_stat = "";
                string for_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string tmp_hari = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                string cstock = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                int tot_hari = 0, tot_harga = 0, istock = 0;

                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                sql_for = " select med_price, qty from KLINIK.cs_formula where formula_id = '" + for_cd + "' ";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for);

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
                    tmp_hari = "1";
                }

                tot_hari = Convert.ToInt32(tmp_hari); //Convert.ToInt32(tmp_hari) * Convert.ToInt32(qty);
                tot_harga = Convert.ToInt32(med_price); //Convert.ToInt32(tmp_hari) *

                if (!cstock.ToString().Equals(""))
                {
                    istock = Convert.ToInt32(cstock);
                    if (istock - Convert.ToInt32(qty) < 0)
                    {
                        MessageBox.Show("Stok Obat Kosong. Tidak dapat dipilih..!!!");
                        view.DeleteRow(view.FocusedRowHandle);
                        return;
                    }
                    else
                    {
                        if (tmp_stat == "I")
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                            view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                        }
                        else
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                            view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                        }
                    }
                }
            }
        }

        private void gvObatUmum_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvObatUmum_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[15], DateTime.Now);
            view.SetRowCellValue(e.RowHandle, view.Columns[16], DateTime.Now.ToString("HH:mm"));
            view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
        }

        private void gvObatUmum_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Kode Dosis" || e.Column.Caption == "Dosis" || e.Column.Caption == "Info" || e.Column.Caption == "Qty" || e.Column.Caption == "Tanggal" || e.Column.Caption == "Jam")
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

        private void simpleButton3_Click(object sender, EventArgs e)
        {

            string r_id = "", kode = "", dosis = "", info = "", jumlah = "", id = "", stok = "", con = "", action = "", info_cara = "";
            string sql_cnt = "", med_cnt = "", sql_update = "", sql_diag = "", diag_cnt = "", harga = "", hari = "", jph = "", info_dosis = "";
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                if (gvObatUmum.RowCount > 0)
                {
                    bool save = false; int ssave = 0;
                    for (int i = 0; i < gvObatUmum.RowCount; i++)
                    {
                        r_id = gvObatUmum.GetRowCellValue(i, gvObatUmum.Columns[0]).ToString();
                        kode = gvObatUmum.GetRowCellValue(i, gvObatUmum.Columns[1]).ToString();
                        dosis = gvObatUmum.GetRowCellValue(i, gvObatUmum.Columns[4]).ToString();
                        info = gvObatUmum.GetRowCellValue(i, gvObatUmum.Columns[5]).ToString();
                        jumlah = gvObatUmum.GetRowCellValue(i, gvObatUmum.Columns[7]).ToString();
                        stok = gvObatUmum.GetRowCellValue(i, gvObatUmum.Columns[6]).ToString();
                        con = gvObatUmum.GetRowCellValue(i, gvObatUmum.Columns[10]).ToString();
                        action = gvObatUmum.GetRowCellValue(i, gvObatUmum.Columns[9]).ToString();
                        harga = gvObatUmum.GetRowCellValue(i, gvObatUmum.Columns[12]).ToString();
                        hari = gvObatUmum.GetRowCellValue(i, gvObatUmum.Columns[11]).ToString();
                        jph = gvObatUmum.GetRowCellValue(i, gvObatUmum.Columns[13]).ToString();
                        info_dosis = gvObatUmum.GetRowCellValue(i, gvObatUmum.Columns[14]).ToString();
                        //info_cara = gvObatUmum.GetRowCellValue(i, gvObatUmum.Columns[21]).ToString();

                        string dte = "", sql = "";
                        object tgl = gvObatUmum.GetRowCellValue(i, gvObatUmum.Columns[15]);
                        if (tgl != null && tgl is DateTime)
                        {
                            DateTime selectedDateTime = (DateTime)tgl;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime selectedDateTime = DateTime.Now;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        if(!action.ToString().Equals("S"))
                        {
                            if (r_id.ToString().Equals(""))
                            {
                                sql = " ";
                                sql = sql + " insert into KLINIK.cs_receipt ( rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, INS_JAM, ID_VISIT, ins_date, ins_emp, GRID_NAME,JENIS_OBAT) ";
                                sql = sql + " values( '" + RMNO + "', to_date('" + dte + "', 'yyyy-MM-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', ";
                                sql = sql + "   '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "',   '" + FN.strVal(gvObatUmum, i, "INS_JAM") + "' , '" + visitid + "',sysdate, '" + DB.vUserId + "' , 'gvObatUmum', 'NONE' ) ";

                                ssave = 2;
                                ORADB.Execute(ORADB.XE, sql);

                            }
                            else
                            {
                                ssave = 1;
                                if (con.ToString().Equals("N"))
                                {
                                    sql = " ";
                                    sql = sql + " Update  KLINIK.cs_receipt ";
                                    sql = sql + "    set  insp_date = to_date('" + dte + "', 'yyyy-MM-dd'),  INS_JAM = '" + FN.strVal(gvObatUmum, i, "INS_JAM") + "' , med_qty = '" + jumlah + "', dosis =  '" + info_dosis + "', UPD_EMP = '" + DB.vUserId + "',UPD_DATE = SYSDATE  ";
                                    sql = sql + "  where  RECEIPT_ID =  '" + r_id + "' and GRID_NAME =  'gvObatUmum' ";

                                    ORADB.Execute(ORADB.XE, sql);
                                    ssave = 3;
                                }
                            }
                        } 
                    } 

                    if (ssave == 1)
                    {
                        //MessageBox.Show("Jadwal Pemberian Obat Umum Tidak Dapat Diganti, Karena Sudah Confirm!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        labelControl33.Visible = true;
                        labelControl33.Text = "Gagal..Obat Sudah Confirm!!";
                        Blinking(labelControl33, 0);
                    }
                    else if (ssave == 2)
                    {
                        //MessageBox.Show("Jadwal Pemberian Obat Umum Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        labelControl33.Visible = true;
                        labelControl33.Text = "Simpan Obat Berhasil";
                        Blinking(labelControl33, 1);
                        LoadDataResep();
                    }
                    else if (ssave == 3)
                    {
                        //MessageBox.Show("Jadwal Pemberian Obat Umum Berhasil di ubah!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        labelControl33.Visible = true;
                        labelControl33.Text = "Ubah Data Berhasil";
                        Blinking(labelControl33, 1);
                        LoadDataResep();
                    } 
                }
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            gvObatUmum.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gvObatUmum.AddNewRow();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                   "Message",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_receipt where ID_VISIT = '" + visitid + "' AND  f_active = 'Y' and receipt_id = '" + FN.strVal(gvObatUmum, gvObatUmum.FocusedRowHandle, "RECEIPT_ID") + "' and CONFIRM ='Y'");

                if (dt != null && dt.Rows.Count > 0)
                {
                    //MessageBox.Show("Maaf Data Confirm Tidak Dapat dihapus...! ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelControl33.Visible = true;
                    //MessageBox.Show("Jadwal Pemberian Obat Tidak Dapat Diganti, Karena Sudah Confirm!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelControl33.Text = "Gagal..Obat Sudah Confirm!!!";
                    Blinking(labelControl33, 0);
                }
                else
                {
                    string sql_delete = "";

                    sql_delete = "";
                    sql_delete = sql_delete + " update KLINIK.cs_receipt  set f_active = 'N', UPD_EMP = '" + DB.vUserId + "',UPD_DATE = SYSDATE  ";
                    sql_delete = sql_delete + "  where ID_VISIT = '" + visitid + "' and receipt_id = '" + FN.strVal(gvObatUmum, gvObatUmum.FocusedRowHandle, "RECEIPT_ID") + "' AND  f_active = 'Y' and CONFIRM ='N' and GRID_NAME='gvObatUmum'   ";

                    try
                    {
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                        oraConnect.Open();
                        cm.ExecuteNonQuery();
                        oraConnect.Close();
                        cm.Dispose();

                        //MessageBox.Show("Query Exec : " + sql_delete);
                        gvObatUmum.DeleteRow(gvObatUmum.FocusedRowHandle);
                        //MessageBox.Show("Data Berhasil dihapus");
                        labelControl33.Visible = true;
                        //MessageBox.Show("Jadwal Pemberian Obat Tidak Dapat Diganti, Karena Sudah Confirm!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        labelControl33.Text = "Hapus Obat Berhasil";
                        Blinking(labelControl33, 1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }

            }
        }

        private void gvMedis_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvVisitDoc_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvwMain_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
                string kk1 = View.GetRowCellValue(e.RowHandle, View.Columns[10]).ToString();
                string pur = View.GetRowCellDisplayText(e.RowHandle, View.Columns[5]);

                if (pur == "Proses")
                {
                    e.Appearance.BackColor = Color.FromArgb(75, Color.LightSalmon);
                    e.Appearance.BackColor2 = Color.FromArgb(75, Color.DodgerBlue);
                }
                else if (pur == "Pembayaran")
                {
                    e.Appearance.BackColor = Color.FromArgb(175, Color.CadetBlue);
                    e.Appearance.BackColor2 = Color.FromArgb(75, Color.LightBlue);
                }
                else if (pur == "Selesai")
                {
                    e.Appearance.BackColor = Color.FromArgb(175, Color.DarkGray);
                    e.Appearance.BackColor2 = Color.FromArgb(75, Color.DarkGoldenrod);
                }
            } 
        }

        private void gvJadwalObat_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            //gvJadwalObat.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(gvJadwalObat_RowUpdated);
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            stsDefault();
        }
         
        private void gvJadwalObat_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();

            if (a.ToString().Equals(""))
                return;

            string dte = "";
            
            DateTime selectedDateTime = DateTime.Now;
            dte = selectedDateTime.ToString("yyyy-MM-dd");

            if (e.Column.Caption == "Nama Obat" && (a.Substring(0, 2) == "BP" || a.Substring(0, 2) == "UM"))
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                string sql_medcd = " ", sql_med = "", med_cd = "", med_name = "", med_group = "", med_stok = "", med_uom = "", cek_stok = "", sql_for = "";

                //dtStock = null;
                sql_medcd = " select " +
                            " max(klinik.FN_CS_INIT_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') +  " +
                            " klinik.FN_CS_TRX_IN(to_date('" + dte.ToString()  + "','yyyy-mm-dd'),'" + a + "') -  " +
                            " klinik.FN_CS_TRX_OUT(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') - " +
                            " klinik.FN_CS_REQ_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "')) stock from dual ";

                datstock = ConnOra.Data_Table_ora(sql_medcd);

                if (datstock.Rows.Count > 0)
                    cek_stok = datstock.Rows[0]["stock"].ToString();
                else
                    cek_stok = "0";
                //cb_ada_tindakan.Checked = functionChk(dataTable3.Rows[0]["ada_tindakan"].ToString(), "Ya");


                //    OleDbConnection oraConn = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_medcd, oraConn);
                ////dtStock.Clear();
                //adOra0.Fill(dtStock);
                //cek_stok = dtStock.Rows[0]["stock"].ToString();

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

                DataTable dt = ConnOra.Data_Table_ora(sql_med);

                //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra = new OleDbDataAdapter(sql_med, oraConnect);
                //DataTable dt = new DataTable();
                //adOra.Fill(dt);

                med_cd = dt.Rows[0]["med_cd"].ToString();
                med_name = dt.Rows[0]["med_name"].ToString();
                med_group = dt.Rows[0]["med_group"].ToString();
                med_stok = dt.Rows[0]["stock"].ToString();
                med_uom = dt.Rows[0]["uom"].ToString();

                sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1  and MINUS_STOK ='Y'  and  b.med_cd = '" + med_cd + "' and POLI_CD = 'POL0001' and ATT1 = decode('" +type_s + "','B','BPJS','U','UMUM','ASURANSI')";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for);

                //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
                //DataTable dtf = new DataTable();
                //adOraf.Fill(dtf);
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
                
               
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
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
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[7], "0");
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
                view.SetRowCellValue(e.RowHandle, view.Columns[7], 1);
                //view.SetRowCellValue(e.RowHandle, view.Columns[4], "");
                //view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                //view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                //view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[14], "1x1");

            }

            if (e.Column.Caption == "Formula")
            {
                string medicine_cd = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();
                string formula_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string reg_dt = dte.ToString();
                string rm = dte.ToString();
                string que = dte.ToString();
                string stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                string kode = "", sql_pilihan = "";

                //if (stat == "I")
                //{
                //    //view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                //    view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                //    view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                //    view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                //}
                //else
                //{
                    sql_pilihan = " select med_cd from KLINIK.cs_formula where formula_id = '" + formula_cd + "' and MINUS_STOK ='Y' ";
                    DataTable dtf2 = ConnOra.Data_Table_ora(sql_pilihan);

                    //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                    //OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_pilihan, oraConnectf);
                    //DataTable dtf = new DataTable();
                    //adOraf.Fill(dtf);

                    if (dtf2.Rows.Count > 0)
                    {
                        kode = dtf2.Rows[0]["med_cd"].ToString();

                    }
                    else
                    {
                        kode = "";
                    }

                    if (kode == medicine_cd)
                    {
                        //view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                        //view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                        //view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                        //view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                    }
                    else
                    {
                        MessageBox.Show("Kode Formula tidak valid");
                        return;
                        //LoadDataResep();
                    }
                //}


            }

            if (e.Column.Caption == "Qty")
            {
                string sql_for = "", med_price = "", qty = "", tmp_stat = "";
                string for_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string tmp_hari = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                string cstock = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                int tot_hari = 0, tot_harga = 0, istock = 0;

                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                sql_for = " select med_price, qty from KLINIK.cs_formula where formula_id = '" + for_cd + "' and MINUS_STOK ='Y' ";
                DataTable dtf3 = ConnOra.Data_Table_ora(sql_for);

                if (dtf3.Rows.Count > 0)
                {
                    med_price = dtf3.Rows[0]["med_price"].ToString();
                    qty = dtf3.Rows[0]["qty"].ToString();
                }
                else
                {
                    med_price = "0";
                    qty = "0";
                }

                if (tmp_hari == "")
                {
                    tmp_hari = "1";
                }

                tot_hari = Convert.ToInt32(tmp_hari); //Convert.ToInt32(tmp_hari) * Convert.ToInt32(qty);
                tot_harga = Convert.ToInt32(med_price); //Convert.ToInt32(tmp_hari) *

                if (!cstock.ToString().Equals(""))
                {
                    istock = Convert.ToInt32(cstock);
                    if (istock - Convert.ToInt32(qty) < 0)
                    {
                        MessageBox.Show("Stok Obat Kosong. Tidak dapat dipilih..!!!");
                        view.DeleteRow(view.FocusedRowHandle);
                        return;
                    }
                    else
                    {
                        if (tmp_stat == "I")
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                            view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                        }
                        else
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                            view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                        }
                    }
                }
            }

            //if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Info" || e.Column.Caption == "Dosis")
            //{
            //    string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();

            //    if (tmp_stat == "I")
            //    {
            //        view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
            //    }
            //    else
            //    {
            //        view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
            //    }
            //}
        }

        private void gvMedis_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            string a = "", tmp_stat = "", date = "", que = "", rm_no = "", no_visit = "";
            string dte = "";
            //date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            //que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            //rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString(); 

            DateTime selectedDateTime = DateTime.Now;
            dte = selectedDateTime.ToString("yyyy-MM-dd");

            if (e.Column.Caption == "Nama Pelayanan" )
            {
                a = view.GetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_ID"]).ToString();
                no_visit = view.GetRowCellValue(e.RowHandle, view.Columns["ID_VISIT"]).ToString();
                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();

                string sql_ = "", sql_head = "", group_id = "", price = "", head_id = "", stbyr = "";
                sql_ = " select treat_group_id, treat_item_price from KLINIK.cs_treatment_item where treat_item_id = " + a + " ";
                DataTable dt0 = ConnOra.Data_Table_ora(sql_);

                //OleDbConnection oraConnect0 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_, oraConnect0);
                //DataTable dt0 = new DataTable();
                //adOra0.Fill(dt0);
                if (dt0.Rows.Count > 0)
                {
                    group_id = dt0.Rows[0]["TREAT_GROUP_ID"].ToString();
                    price = dt0.Rows[0]["TREAT_ITEM_PRICE"].ToString();
                }

                sql_head = " select head_id, pay_status from KLINIK.cs_treatment_head where ID_VISIT = '" + visitid + "'  ";
                DataTable dt1 = ConnOra.Data_Table_ora(sql_head);
                //OleDbConnection oraConnect1 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra1 = new OleDbDataAdapter(sql_head, oraConnect1);
                //DataTable dt1 = new DataTable();
                //adOra1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    head_id = dt1.Rows[0]["HEAD_ID"].ToString();
                    stbyr = dt1.Rows[0]["PAY_STATUS"].ToString();
                }

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns["HEAD_ID"], head_id);
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_GROUP_ID"], group_id);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[2], a);
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_QTY"], "1");
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_PRICE"], price);
                    view.SetRowCellValue(e.RowHandle, view.Columns["PAY_STATUS"], stbyr);
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
                } 
            }

            if (e.Column.Caption == "Remark")
            {
                string tmp_stat2 = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                if (tmp_stat2 == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
                }
            }
        }

        

        private void dtDataPelayanan()
        {
            string sql_tind_load = "";

            sql_tind_load = sql_tind_load + Environment.NewLine + "select b.detail_id, c.treat_group_id, b.treat_item_id, b.treat_qty, b.total_price, ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "b.remarks, 'S' action, a.head_id, to_char(b.treat_date,'yyyy-MM-dd') treat_date, a.pay_status, b.treat_item_price ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "from KLINIK.cs_treatment_head a ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "join KLINIK.cs_treatment_detail b on (a.head_id=b.head_id) ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "join KLINIK.cs_treatment_item c on (b.treat_item_id=c.treat_item_id) ";
            //sql_tind_load = sql_tind_load + Environment.NewLine + "where rm_no='" + pub_rm_no + "' ";
            //sql_tind_load = sql_tind_load + Environment.NewLine + "and to_char(visit_date,'yyyy-MM-dd')='" + pub_reg_date + "' ";
            //sql_tind_load = sql_tind_load + Environment.NewLine + "and visit_no='" + pub_que + "' ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "and a.status='OPN' ";
            DataTable dtPmedis = ConnOra.Data_Table_ora(sql_tind_load);
            //DataTable dtPmedis = ORADB.SetData(ORADB.XE, sql_tind_load);
            gridMedis.DataSource = null;
            gvMedis.Columns.Clear();
            gridMedis.DataSource = dtPmedis;

            gvMedis.OptionsView.ColumnAutoWidth = true;
            gvMedis.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gvMedis.Appearance.HeaderPanel.FontSizeDelta = 0;
            gvMedis.IndicatorWidth = 30; 
            gvMedis.BestFitColumns();

        }


        private void bsavemedis_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvMedis.RowCount > 0)
                {
                    //DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_treatment_detail where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvMedis' and F_ACTIVE ='Y' ");
                    //ORADB.SetData(ORADB.XE, "Select * from KLINIK.cs_treatment_detail where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvMedis' ");
                    //if (dt != null && dt.Rows.Count > 0)
                    //{
                    //    ORADB.Execute(ORADB.XE, " insert into KLINIK.cs_treatment_detail_del select a.*, sysdate, '" + DB.vUserId + "' as emp from KLINIK.cs_treatment_detail a  where  HEAD_ID = '" + headid + "'  and GRID_NAME = 'gvMedis' and F_ACTIVE ='Y' ");
                    //    ORADB.Execute(ORADB.XE, " Delete from KLINIK.cs_treatment_detail  where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvMedis' and F_ACTIVE ='Y' ");
                    //}

                    string sql = " "; bool save = false ; //insert all 
                    for (int i = 0; i < gvMedis.RowCount; i++)
                    {
                        string dte = "",detailid ="", spay ="";
                        object tgl = gvMedis.GetRowCellValue(i, "TANGGAL");
                        detailid =  FN.strVal(gvMedis, i, "DETAIL_ID") ;
                        spay = FN.strVal(gvMedis, i, "PAY_STATUS");
                        if (tgl != null && tgl is DateTime)
                        {
                            DateTime selectedDateTime = (DateTime)tgl;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime selectedDateTime = DateTime.Now;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }

                        if(detailid.ToString().Equals(""))
                        {
                            sql = "";
                            sql = sql + " insert into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME) values ( ";
                            sql = sql + " CS_TREATMENT_DETAIL_SEQ.nextval ,'" + FN.strVal(gvMedis, i, "HEAD_ID") + "','" + FN.strVal(gvMedis, i, "TREAT_ITEM_ID") + "'  ,";
                            sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd'), '" + FN.strVal(gvMedis, i, "TREAT_QTY") + "', '" + FN.strVal(gvMedis, i, "TREAT_ITEM_PRICE") + "', " + Convert.ToInt32(FN.strVal(gvMedis, i, "TREAT_QTY")) * Convert.ToInt32(FN.strVal(gvMedis, i, "TREAT_ITEM_PRICE")) + ", ";
                            sql = sql + " '" + FN.strVal(gvMedis, i, "REMARKS") + "' ,  sysdate, '" + DB.vUserId + "', '" + FN.strVal(gvMedis, i, "JAM") + "' , 'gvMedis' )";
                        } 
                        else
                        {
                            sql = "";
                            sql = sql + " update KLINIK.cs_treatment_detail  set treat_date =  TO_DATE('" + dte + "', 'yyyy-MM-dd'), TREAT_JAM = '" + FN.strVal(gvMedis, i, "JAM") + "', ";
                            sql = sql + "        remarks   = '" + FN.strVal(gvMedis, i, "REMARKS") + "', UPD_DATE = sysdate, UPD_EMP = '" + DB.vUserId  + "'  ";
                            sql = sql + "  where detail_id   = " + detailid + " ";
                        }
                        save = ORADB.Execute(ORADB.XE, sql);
                    }
                    //sql = sql + " select * from dual";
                    //bool save = ORADB.Execute(ORADB.XE, sql);
                    if (save)
                    {
                        //MessageBox.Show("Data Pelayanan Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        labelControl103.Visible = true;
                        labelControl103.Text = "Berhasil Disimpan";
                        Blinking(labelControl103, 1);
                    } 
                } 
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            } 
        }

        private void gvObatUmum_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            //gvObatUmum.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(gvObatUmum_RowUpdated);
        }

        private void gridHRacik_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            gridHRacik.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(gridHRacik_RowUpdated);
        }

        private void gvRacik_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            //gvRacik.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(gvRacik_RowUpdated);
        }

        private void gvRacik_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView; DataTable dtf = null;
           string a = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();

            if (a.ToString().Equals(""))
                return;

            string dte = "";

            DateTime selectedDateTime = DateTime.Now;
            dte = selectedDateTime.ToString("yyyy-MM-dd");

            if (e.Column.Caption == "Nama Obat" && (a.Substring(0, 2) == "BP" || a.Substring(0, 2) == "UM"))
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                string sql_medcd = " ", sql_med = "", med_cd = "", med_name = "", med_group = "", med_stok = "", med_uom = "", cek_stok = "", sql_for = "";

                //dtStock = null;
                sql_medcd = " select " +
                            " max(klinik.FN_CS_INIT_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') +  " +
                            " klinik.FN_CS_TRX_IN(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') -  " +
                            " klinik.FN_CS_TRX_OUT(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') - " +
                            " klinik.FN_CS_REQ_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "')) stock from dual ";

                datstock = ConnOra.Data_Table_ora(sql_medcd);

                if (datstock.Rows.Count > 0)
                    cek_stok = datstock.Rows[0]["stock"].ToString();
                else
                    cek_stok = "0";
                //cb_ada_tindakan.Checked = functionChk(dataTable3.Rows[0]["ada_tindakan"].ToString(), "Ya");


                //    OleDbConnection oraConn = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_medcd, oraConn);
                ////dtStock.Clear();
                //adOra0.Fill(dtStock);
                //cek_stok = dtStock.Rows[0]["stock"].ToString();

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

                DataTable dt = ConnOra.Data_Table_ora(sql_med);

                //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra = new OleDbDataAdapter(sql_med, oraConnect);
                //DataTable dt = new DataTable();
                //adOra.Fill(dt);

                med_cd = dt.Rows[0]["med_cd"].ToString();
                med_name = dt.Rows[0]["med_name"].ToString();
                med_group = dt.Rows[0]["med_group"].ToString();
                med_stok = dt.Rows[0]["stock"].ToString();
                med_uom = dt.Rows[0]["uom"].ToString();

                if(type_s.ToString().Equals("B"))
                {
                    //sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1  and MINUS_STOK ='Y'  and  b.med_cd = '" + med_cd + "' and POLI_CD = 'POL0001' and ATT1 = decode('" + type_s + "','B','BPJS','U','UMUM','ASURANSI')";
                    string Sql = " ";
                    Sql = Sql + Environment.NewLine + " select formula_id, initcap(formula) formula, initcap(b.med_name) || decode(att1,'BPJS','',' [None BPJS]') med_name, a.med_cd ";
                    Sql = Sql + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1      ";
                    Sql = Sql + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'    ";
                    Sql = Sql + Environment.NewLine + "    and POLI_CD ='POL0001'    and  b.med_cd = '" + med_cd + "' AND RACIKAN ='Y' ";
                    //Sql = Sql + Environment.NewLine + "  UNION ALL  ";
                    //Sql = Sql + Environment.NewLine + " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name,a.med_cd ";
                    //Sql = Sql + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1      ";
                    //Sql = Sql + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  and att1 ='UMUM'  ";
                    //Sql = Sql + Environment.NewLine + "    and POLI_CD = 'POL0001'    and  b.med_cd = '" + med_cd + "'  ";
                    //Sql = Sql + Environment.NewLine + "    and b.med_cd not in ( select b.med_cd   ";
                    //Sql = Sql + Environment.NewLine + "                           from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1      ";
                    //Sql = Sql + Environment.NewLine + "                            and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  and att1 = 'BPJS'  ";
                    //Sql = Sql + Environment.NewLine + "                            and POLI_CD ='POL0001'   ";
                    //Sql = Sql + Environment.NewLine + "                        )  ";
                    Sql = Sql + Environment.NewLine + "  order by 2,3  ";

                    dtf = ConnOra.Data_Table_ora(Sql); 

                    listFormulaR.Clear(); 
                    for (int i = 0; i < dtf.Rows.Count; i++)
                    {
                        listFormulaR.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                    }
                }
                else
                {
                    sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name)|| decode(att1,'BPJS','',' [None BPJS]') med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1  and MINUS_STOK ='Y'  and  b.med_cd = '" + med_cd + "' and POLI_CD = 'POL0001' AND RACIKAN ='Y'  ";
                    dtf = ConnOra.Data_Table_ora(sql_for);

                    listFormulaR.Clear();
                    for (int i = 0; i < dtf.Rows.Count; i++)
                    {
                        listFormulaR.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                    }
                }
               
                if (dtf.Rows.Count == 1)
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], dtf.Rows[0]["formula_id"].ToString());
                else
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], "");


                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
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
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[7], "0");
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
                view.SetRowCellValue(e.RowHandle, view.Columns[7], 1);
                //view.SetRowCellValue(e.RowHandle, view.Columns[4], "");
                //view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                //view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                //view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[14], "1x1");

            }

            if (e.Column.Caption == "Formula")
            {
                string medicine_cd = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();
                string formula_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string reg_dt = dte.ToString();
                string rm = dte.ToString();
                string que = dte.ToString();
                string stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                string kode = "", sql_pilihan = "";
                 
                sql_pilihan = " select med_cd from KLINIK.cs_formula where formula_id = '" + formula_cd + "' and MINUS_STOK ='Y' ";
                DataTable dtf2 = ConnOra.Data_Table_ora(sql_pilihan);
                 
                if (dtf2.Rows.Count > 0)
                {
                    kode = dtf2.Rows[0]["med_cd"].ToString();

                }
                else
                {
                    kode = "";
                }

                if (kode == medicine_cd)
                {
                    //view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                    //view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                }
                else
                {
                    MessageBox.Show("Kode Formula tidak valid");
                    return;
                    //LoadDataResep();
                }
                //}


            }

            if (e.Column.Caption == "Qty")
            {
                string sql_for = "", med_price = "", qty = "", tmp_stat = "";
                string for_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string tmp_hari = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                string cstock = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                int tot_hari = 0, tot_harga = 0, istock = 0;

                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                sql_for = " select med_price, qty from KLINIK.cs_formula where formula_id = '" + for_cd + "' and MINUS_STOK ='Y' ";
                DataTable dtf3 = ConnOra.Data_Table_ora(sql_for);

                if (dtf3.Rows.Count > 0)
                {
                    med_price = dtf3.Rows[0]["med_price"].ToString();
                    qty = dtf3.Rows[0]["qty"].ToString();
                }
                else
                {
                    med_price = "0";
                    qty = "0";
                }

                if (tmp_hari == "")
                {
                    tmp_hari = "1";
                }

                tot_hari = Convert.ToInt32(tmp_hari); //Convert.ToInt32(tmp_hari) * Convert.ToInt32(qty);
                tot_harga = Convert.ToInt32(med_price); //Convert.ToInt32(tmp_hari) *

                if (!cstock.ToString().Equals(""))
                {
                    istock = Convert.ToInt32(cstock);
                    if (istock - Convert.ToInt32(qty) < 0)
                    {
                        MessageBox.Show("Stok Obat Kosong. Tidak dapat dipilih..!!!");
                        view.DeleteRow(view.FocusedRowHandle);
                        return;
                    }
                    else
                    {
                        if (tmp_stat == "I")
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                            view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                        }
                        else
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                            view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                        }
                    }
                }
            } 
        }

        private void gvRacik_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvRacik_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
            //view.SetRowCellValue(e.RowHandle, view.Columns[15], DateTime.Now);
            //view.SetRowCellValue(e.RowHandle, view.Columns[16], DateTime.Now.ToString("HH:MM"));
            view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
        }

        private void gvRacik_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Kode Dosis" || e.Column.Caption == "Dosis"  || e.Column.Caption == "Qty"  )
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

        private void gridHRacik_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //btnMedSave.Enabled = true;
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();

            if (e.Column.Caption == "Dosis")
            {
                gridHRacik.SetRowCellValue(gridHRacik.FocusedRowHandle, view.Columns[3], "A");
            }
        }

        private void gridHRacik_RowClick(object sender, RowClickEventArgs e)
        {
            GridView View = sender as GridView;

            if (View.RowCount < 1)
                return; 

            string idracikan = "";

            idracikan = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            LoadResepRacikan(idracikan);
            gridHRacik.Columns[1].ColumnEdit = racikLookup;
            gridHRacik.Columns[2].ColumnEdit = dosisLookup;
            gridHRacik.Columns[3].ColumnEdit = medicineInfoLookup;
        }

        private void LoadResepRacikan(string idracikan)
        {
            string s_rm = "", s_date = "", s_que = "", sstatus = "", spoli = "";
             
            string sql_med = "";
            sql_med = " select a.receipt_id, a.med_cd, b.med_group, a.med_cd, A.formula, type_drink,  " +
                           " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +  " +
                           " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -  " +
                           " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) - " +
                           " klinik.FN_CS_REQ_STOCK(a.insp_date,a.med_cd) stock, " +
                           " A.med_qty, initcap(uom) uom, 'S' action, a.confirm, a.days, a.price, a.qty_day, a.dosis " +
                           " from KLINIK.cs_receipt a  " +
                           " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)  JOIN KLINIK.cs_formula D ON (B.med_cd = D.med_cd AND D.FORMULA_ID = A.formula) " +
                           " where b.status = 'A'   and D.MINUS_STOK ='Y'  and a.ATT1_RECIEPT is not null and a.JENIS_OBAT ='RACIK' " +
                           " and rm_no = '" + RMNO + "' and a.GRID_NAME ='gvRacik'  and a.ATT1_RECIEPT = '" + idracikan + "'" + // and upper(att1) in (upper('" + sstatus + "'),  'ALL')  " + 
                           " and id_visit = " + visitid + " and d.racikan ='Y'  order by  a.med_cd  ";
              
            DataTable dtRacik = ConnOra.Data_Table_ora(sql_med);

            gdRacik.DataSource = null;
            gdRacik.DataSource = dtRacik;

            gvRacik.OptionsView.ColumnAutoWidth = true;
            gvRacik.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gvRacik.Appearance.HeaderPanel.FontSizeDelta = 0;
            gvRacik.IndicatorWidth = 33;
            //gvRacik.OptionsLayout.Reset();
            //gdRacik.MainView.RestoreLayoutFromXml("");
            gvRacik.BestFitColumns();

            gvRacik.Columns[6].OptionsColumn.ReadOnly = true;
            gvRacik.Columns[10].OptionsColumn.ReadOnly = true;

            ////gvRacik.Columns[15].VisibleIndex = 0;
            ////gvRacik.Columns[16].VisibleIndex = 1;
            //gvRacik.Columns[3].VisibleIndex = 2;
            //gvRacik.Columns[14].VisibleIndex = 3;
            //gvRacik.Columns[7].VisibleIndex = 4;

            //gvRacik.Columns[0].Caption = "ID";
            //gvRacik.Columns[1].Caption = "Kode";
            //gvRacik.Columns[2].Caption = "Group";
            //gvRacik.Columns[3].Caption = "Nama Obat";
            //gvRacik.Columns[4].Caption = "Formula";
            //gvRacik.Columns[5].Caption = "Info";
            //gvRacik.Columns[6].Caption = "Stok";
            //gvRacik.Columns[7].Caption = "Jumlah";
            //gvRacik.Columns[8].Caption = "Satuan";
            //gvRacik.Columns[9].Caption = "Action";
            //gvRacik.Columns[10].Caption = "Confirm";
            //gvRacik.Columns[11].Caption = "Jml";
            //gvRacik.Columns[12].Caption = "Harga";
            //gvRacik.Columns[13].Caption = "Jumlah per Hari";
            //gvRacik.Columns[14].Caption = "Dosis";
            //gvRacik.Columns[15].Caption = "Remark";
            //gvRacik.Columns[16].Caption = "Tanggal";
            //gvRacik.Columns[17].Caption = "Jam";

            ////gvRacik.Columns[3].Width = 200;
            ////gvRacik.Columns[4].Width = 95;
            ////gvRacik.Columns[5].Width = 150;
            ////gvRacik.Columns[6].Width = 65;
            ////gvRacik.Columns[7].Width = 60;
            ////gvRacik.Columns[8].Width = 65;
            ////gvRacik.Columns[10].Width = 65;
            ////gvRacik.Columns[11].Width = 60;
            ////gvRacik.Columns[14].Width = 55;

            //gvRacik.Columns[0].Visible = false;
            //gvRacik.Columns[1].Visible = false;
            ////gvRacik.Columns[2].Visible = false;
            ////gvRacik.Columns[5].Visible = false;
            ////gvRacik.Columns[7].Visible = false;
            ////gvRacik.Columns[8].Visible = false;
            //gvRacik.Columns[9].Visible = false;
            //gvRacik.Columns[12].Visible = false;
            //gvRacik.Columns[13].Visible = false;
            //gvRacik.Columns[14].Visible = false;
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            gridHRacik.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridHRacik.AddNewRow();
            simpleButton9.Enabled = true;
            simpleButton10.Enabled = true;
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (gridHRacik.RowCount < 1)
                return;

            string stat = gridHRacik.GetRowCellDisplayText(gridHRacik.FocusedRowHandle, gridHRacik.Columns[6]);
            if (stat == "I")
            {
                gridHRacik.DeleteRow(gridHRacik.FocusedRowHandle);
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            if (gridHRacik.RowCount < 1)
                return;

            string stat = gridHRacik.GetRowCellDisplayText(gridHRacik.FocusedRowHandle, gridHRacik.Columns[6]);
            if (stat == "I")
            {
                //RowClickEventArgs Ee = RowClickEventArgs(gridHRacik.FocusedRowHandle);
                gridHRacik.SetRowCellValue(gridHRacik.FocusedRowHandle, gridHRacik.Columns[6], "W");
                gridHRacik.SelectRow(gridHRacik.FocusedRowHandle);
                gridHRacik.SelectRows(gridHRacik.FocusedRowHandle, gridHRacik.FocusedRowHandle);
                gridHRacik.Appearance.FocusedRow.BackColor = Color.GreenYellow;
                gridHRacik.Appearance.FocusedRow.ForeColor = Color.DarkGreen; // for demonstration purposes only
                gridHRacik.Appearance.SelectedRow.Assign(gridHRacik.Appearance.FocusedRow);
                gridHRacik.Appearance.FocusedCell.Assign(gridHRacik.Appearance.FocusedRow);
                SetFocusedAppearance(gridHRacik,true);
                labelControl35.Visible = true;
                //MessageBox.Show("Jadwal Pemberian Obat Berhasil di ubah!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                labelControl35.Text = "Racikan Di Proses"; Blinking(labelControl35, 1);
                LoadResepRacikan("");
                //gridHRacik.FocusedColumn("ID", "ID");
                //gridHRacik.Raise("Click", new EventArgs());
                //gridHRacik_RowClick(sender, gridHRacik.FocusedRowHandle);
                //gridHRacik.P            
            }
        }
        private void SetFocusedAppearance(GridView grdviw, bool isFocused)
        {
            grdviw.OptionsSelection.EnableAppearanceFocusedRow = isFocused;
            grdviw.OptionsSelection.EnableAppearanceFocusedCell = isFocused;
        }

        private void sAddRacik_Click(object sender, EventArgs e)
        {
            gvRacik.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gvRacik.AddNewRow();
            sHapusRacik.Enabled = true;
            sSimpanRacik.Enabled = true;
        }

        private void sHapusRacik_Click(object sender, EventArgs e)
        {
            if (gvRacik.RowCount < 1)
                return;

            string stat = gvRacik.GetRowCellDisplayText(gvRacik.FocusedRowHandle, gvRacik.Columns[6]);
            if (stat == "I" || stat == "" )
            {
                gvRacik.DeleteRow(gvRacik.FocusedRowHandle);
                labelControl34.Visible = true;
                labelControl34.Text = "Obat Racikan Berhasil Dihapus"; Blinking(labelControl34, 1);
            }
        }

        private void sSimpanRacik_Click(object sender, EventArgs e)
        {
            string kode = "", dosis = "", info = "", jumlah = "", id = "", stok = "", con = "", action = "";
            string sql_cnt = "", med_cnt = "", sql_update = "", sjam = "", stanggal = "", harga = "", hari = "", jph = "", info_dosis = "";
            int stsimpan = 0;
            string jnsracik = "", dosisH = "", info_dosisH = "", jumlahH = "", remarkH = "";

            jnsracik = gridHRacik.GetRowCellValue(gridHRacik.FocusedRowHandle, gridHRacik.Columns[1]).ToString();
            dosisH = gridHRacik.GetRowCellValue(gridHRacik.FocusedRowHandle, gridHRacik.Columns[2]).ToString();
            info_dosisH = gridHRacik.GetRowCellValue(gridHRacik.FocusedRowHandle, gridHRacik.Columns[3]).ToString();
            jumlahH = gridHRacik.GetRowCellValue(gridHRacik.FocusedRowHandle, gridHRacik.Columns[4]).ToString();
            remarkH = gridHRacik.GetRowCellValue(gridHRacik.FocusedRowHandle, gridHRacik.Columns[5]).ToString();
            stanggal = gridHRacik.GetRowCellValue(gridHRacik.FocusedRowHandle, gridHRacik.Columns[7]).ToString();
            sjam  = gridHRacik.GetRowCellValue(gridHRacik.FocusedRowHandle, gridHRacik.Columns[8]).ToString();
            for (int i = 0; i < gvRacik.DataRowCount; i++)
            {
                id = gvRacik.GetRowCellValue(i, gvRacik.Columns[0]).ToString();
                kode = gvRacik.GetRowCellValue(i, gvRacik.Columns[1]).ToString();
                dosis = gvRacik.GetRowCellValue(i, gvRacik.Columns[4]).ToString();
                info = gvRacik.GetRowCellValue(i, gvRacik.Columns[5]).ToString();
                jumlah = gvRacik.GetRowCellValue(i, gvRacik.Columns[7]).ToString();
                stok = gvRacik.GetRowCellValue(i, gvRacik.Columns[6]).ToString();
                con = gvRacik.GetRowCellValue(i, gvRacik.Columns[10]).ToString();
                action = gvRacik.GetRowCellValue(i, gvRacik.Columns[9]).ToString();
                harga = gvRacik.GetRowCellValue(i, gvRacik.Columns[12]).ToString();
                hari = gvRacik.GetRowCellValue(i, gvRacik.Columns[11]).ToString();
                jph = gvRacik.GetRowCellValue(i, gvRacik.Columns[13]).ToString();
                info_dosis = gvRacik.GetRowCellValue(i, gvRacik.Columns[14]).ToString();

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
                else
                { 
                    string dte = "", sql = " ";
                    object tgl = gridHRacik.GetRowCellValue(gridHRacik.FocusedRowHandle, gridHRacik.Columns[7]);
                    if (tgl != null && tgl is DateTime)
                    {
                        DateTime selectedDateTime = (DateTime)tgl;
                        dte = selectedDateTime.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        DateTime selectedDateTime = DateTime.Now;
                        dte = selectedDateTime.ToString("yyyy-MM-dd");
                    }

                    if (action == "I")
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
                             
                                command.CommandText = " insert into KLINIK.cs_receipt (receipt_id, rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis,  ins_date, ins_emp,ID_VISIT, JENIS_OBAT, ATT1_RECIEPT, ATT2_RECIEPT, ATT3_RECIEPT, GRID_NAME,INS_JAM ) " +
                                                      " values(cs_receipt_seq.nextval, '" + RMNO + "', to_date('" + dte + "', 'yyyy-mm-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', '" + info_dosisH + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + dosisH + "',   sysdate, '" + DB.vUserId + "', " + visitid + ",'RACIK', '" + jnsracik + "','" + remarkH + "', " + jumlahH + ", 'gvRacik','" + sjam + "' ) ";
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
                    else if (action == "U")
                    {
                        sql_update = "";

                        sql_update = sql_update + " update KLINIK.cs_receipt" +
                                                  " set med_cd = '" + kode + "', formula = '" + dosis + "', med_qty = '" + jumlah + "', type_drink = '" + info + "', " +
                                                  "     price = '" + harga + "', days = '" + hari + "', qty_day = '" + jph + "', dosis = '" + info_dosis + "',";
                        sql_update = sql_update + "     insp_date = to_date('" + dte + "', 'yyyy-MM-dd'),  INS_JAM = '" + sjam  + "' , upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
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
                labelControl34.Visible = true;
                labelControl34.Text = "Racikan Berhasil Dibuat"; Blinking(labelControl34, 1);
            } 
            else if (stsimpan == 2)
            {
                labelControl34.Visible = true;
                labelControl34.Text = "Racikan Berhasil Diubah"; Blinking(labelControl34, 1);
            }
            LoadDataResep();
        }

        private void gridHRacik_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[3], "A"); 
            view.SetFocusedRowCellValue(view.Columns[6], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[7], DateTime.Now);
            view.SetRowCellValue(e.RowHandle, view.Columns[8], DateTime.Now.ToString("HH:mm"));
        }
        private void baddmedis_Click(object sender, EventArgs e)
        {
            if (dtMedis == null) return;

            DataRow newRow = dtMedis.NewRow();

            newRow["SEQ"] = ((gvMedis.RowCount) + 1).ToString();
            newRow["HEAD_ID"] = headid;
            newRow["ID_VISIT"] = visitid;
            newRow["ACTION"] = "I";
            newRow["TANGGAL"] = DateTime.Now;
            newRow["JAM"] = DateTime.Now.ToString("HH:mm");
            dtMedis.Rows.Add(newRow);

            gridMedis.DataSource = dtMedis;
        }

        private void baddnone_Click(object sender, EventArgs e)
        {
            if (dtVisitDokter == null) return;

            DataRow newRow = dtVisitDokter.NewRow();

            newRow["SEQ"] = ((gvVisitDoc.RowCount) + 1).ToString();
            newRow["HEAD_ID"] = headid;
            newRow["ID_VISIT"] = visitid;
            newRow["ACTION"] = "I";
            newRow["TANGGAL"] = DateTime.Now;
            newRow["JAM"] = DateTime.Now.ToString("HH:MM");
            dtVisitDokter.Rows.Add(newRow);

            gridVisitDoc.DataSource = dtVisitDokter;
        }
        private void gvMedis_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
             
            view.SetFocusedRowCellValue(view.Columns[8], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[11], DateTime.Now);
            view.SetRowCellValue(e.RowHandle, view.Columns[12], DateTime.Now.ToString("HH:mm")); 
        }

        private void gvMedis_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            //gvMedis.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(gvMedis_RowUpdated);
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            if (dtMedisU == null) return;

            DataRow newRow = dtMedisU.NewRow();

            newRow["SEQ"] = ((gvMedisU.RowCount) + 1).ToString();
            newRow["HEAD_ID"] = headid;
            newRow["ID_VISIT"] = visitid;
            newRow["ACTION"] = "I";
            newRow["TANGGAL"] = DateTime.Now;
            newRow["JAM"] = DateTime.Now.ToString("HH:mm");
            dtMedisU.Rows.Add(newRow);

            gridMedisU.DataSource = dtMedisU;
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                 "Message",
                  MessageBoxButtons.YesNo,
                  MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_treatment_head where HEAD_ID = '" + FN.strVal(gvMedis, gvMedis.FocusedRowHandle, "HEAD_ID") + "' and STATUS ='OPN' and PAY_STATUS ='OPN' ");

                if (dt != null && dt.Rows.Count > 0)
                {
                    string sql_delete = "";

                    sql_delete = "";
                    sql_delete = sql_delete + " update  KLINIK.cs_treatment_detail   set f_active = 'N', UPD_EMP = '" + DB.vUserId + "', UPD_DATE = SYSDATE  ";
                    sql_delete = sql_delete + "  where DETAIL_ID= '" + FN.strVal(gvMedis, gvMedis.FocusedRowHandle, "DETAIL_ID") + "' AND  f_active = 'Y'  ";

                    try
                    {
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                        oraConnect.Open();
                        cm.ExecuteNonQuery();
                        oraConnect.Close();
                        cm.Dispose();

                        //MessageBox.Show("Query Exec : " + sql_delete);
                        gvMedis.DeleteRow(gvMedis.FocusedRowHandle);
                        //MessageBox.Show("Data Berhasil dihapus");
                        labelControl105.Visible = true;
                        labelControl105.Text = "Berhasil Dihapus";
                        Blinking(labelControl105, 1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }
                else
                {
                    labelControl105.Visible = true;
                    labelControl105.Text = "Gagal..Status Closed.";
                    Blinking(labelControl105, 0);
                    //MessageBox.Show("Maaf Data Close Tidak Dapat dihapus...! ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvMedisU.RowCount > 0)
                {
                    //DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_treatment_detail where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvMedis' and F_ACTIVE ='Y' ");
                    //ORADB.SetData(ORADB.XE, "Select * from KLINIK.cs_treatment_detail where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvMedis' ");
                    //if (dt != null && dt.Rows.Count > 0)
                    //{
                    //    ORADB.Execute(ORADB.XE, " insert into KLINIK.cs_treatment_detail_del select a.*, sysdate, '" + DB.vUserId + "' as emp from KLINIK.cs_treatment_detail a  where  HEAD_ID = '" + headid + "'  and GRID_NAME = 'gvMedis' and F_ACTIVE ='Y' ");
                    //    ORADB.Execute(ORADB.XE, " Delete from KLINIK.cs_treatment_detail  where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvMedis' and F_ACTIVE ='Y' ");
                    //}

                    string sql = " "; bool save = false; //insert all 
                    for (int i = 0; i < gvMedisU.RowCount; i++)
                    {
                        string dte = "", detailid = "", spay = "";
                        object tgl = gvMedisU.GetRowCellValue(i, "TANGGAL");
                        detailid = FN.strVal(gvMedisU, i, "DETAIL_ID");
                        spay = FN.strVal(gvMedisU, i, "PAY_STATUS");
                        if (tgl != null && tgl is DateTime)
                        {
                            DateTime selectedDateTime = (DateTime)tgl;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime selectedDateTime = DateTime.Now;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }

                        if (detailid.ToString().Equals(""))
                        {
                            sql = "";
                            sql = sql + " insert into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME) values ( ";
                            sql = sql + " CS_TREATMENT_DETAIL_SEQ.nextval ,'" + FN.strVal(gvMedisU, i, "HEAD_ID") + "','" + FN.strVal(gvMedisU, i, "TREAT_ITEM_ID") + "'  ,";
                            sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd'), '" + FN.strVal(gvMedisU, i, "TREAT_QTY") + "', '" + FN.strVal(gvMedisU, i, "TREAT_ITEM_PRICE") + "', " + Convert.ToInt32(FN.strVal(gvMedisU, i, "TREAT_QTY")) * Convert.ToInt32(FN.strVal(gvMedisU, i, "TREAT_ITEM_PRICE")) + ", ";
                            sql = sql + " '" + FN.strVal(gvMedisU, i, "REMARKS") + "' ,  sysdate, '" + DB.vUserId + "', '" + FN.strVal(gvMedisU, i, "JAM") + "' , 'gvMedisU' )";
                        }
                        else
                        {
                            sql = "";
                            sql = sql + " update KLINIK.cs_treatment_detail  set treat_date =  TO_DATE('" + dte + "', 'yyyy-MM-dd'), TREAT_JAM = '" + FN.strVal(gvMedisU, i, "JAM") + "', ";
                            sql = sql + "        remarks   = '" + FN.strVal(gvMedisU, i, "REMARKS") + "', UPD_DATE = sysdate, UPD_EMP = '" + DB.vUserId + "'  ";
                            sql = sql + "  where detail_id   = " + detailid + " ";
                        }
                        save = ORADB.Execute(ORADB.XE, sql);
                    }
                    //sql = sql + " select * from dual";
                    //bool save = ORADB.Execute(ORADB.XE, sql);
                    if (save)
                    {
                        //MessageBox.Show("Data Pelayanan Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        labelControl105.Visible = true;
                        labelControl105.Text = "Berhasil Disimpan";
                        Blinking(labelControl105, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
        }

        private void gvMedisU_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvMedisU_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            string a = "", tmp_stat = "", date = "", que = "", rm_no = "", no_visit = "";
            string dte = "";
            //date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            //que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            //rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString(); 

            DateTime selectedDateTime = DateTime.Now;
            dte = selectedDateTime.ToString("yyyy-MM-dd");

            if (e.Column.Caption == "Nama Pelayanan")
            {
                a = view.GetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_ID"]).ToString();
                no_visit = view.GetRowCellValue(e.RowHandle, view.Columns["ID_VISIT"]).ToString();
                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();

                string sql_ = "", sql_head = "", group_id = "", price = "", head_id = "", stbyr = "";
                sql_ = " select treat_group_id, treat_item_price from KLINIK.cs_treatment_item where treat_item_id = " + a + " ";
                DataTable dt0 = ConnOra.Data_Table_ora(sql_);

                //OleDbConnection oraConnect0 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_, oraConnect0);
                //DataTable dt0 = new DataTable();
                //adOra0.Fill(dt0);
                if (dt0.Rows.Count > 0)
                {
                    group_id = dt0.Rows[0]["TREAT_GROUP_ID"].ToString();
                    price = dt0.Rows[0]["TREAT_ITEM_PRICE"].ToString();
                }

                sql_head = " select head_id, pay_status from KLINIK.cs_treatment_head where ID_VISIT = '" + visitid + "'  ";
                DataTable dt1 = ConnOra.Data_Table_ora(sql_head);
                //OleDbConnection oraConnect1 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra1 = new OleDbDataAdapter(sql_head, oraConnect1);
                //DataTable dt1 = new DataTable();
                //adOra1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    head_id = dt1.Rows[0]["HEAD_ID"].ToString();
                    stbyr = dt1.Rows[0]["PAY_STATUS"].ToString();
                }

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns["HEAD_ID"], head_id);
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_GROUP_ID"], group_id);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[2], a);
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_QTY"], "1");
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_PRICE"], price);
                    view.SetRowCellValue(e.RowHandle, view.Columns["PAY_STATUS"], stbyr);
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
                }
            }

            if (e.Column.Caption == "Remark")
            {
                string tmp_stat2 = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                if (tmp_stat2 == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
                }
            }
        }

        private void gvMedisU_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            //gvMedisU.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(gvMedisU_RowUpdated);
        }

        private void gvCppt_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns[1], DateTime.Now); // DateTime.Now;
            //newRow["JAM"] = DateTime.Now.ToString("HH:mm");
            view.SetRowCellValue(e.RowHandle, view.Columns[2], DateTime.Now.ToString("HH:mm"));
        }

        private void gvCppt_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            //gvCppt.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(gvCppt_RowUpdated);
        }

        private void gvwMain_DoubleClick(object sender, EventArgs e)
        {
            if (gvwMain.RowCount < 1)
                return;

            anamesaID = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "ANAMNESA_ID");

            if (anamesaID.ToString().Equals("") || anamesaID.ToString().Equals("0"))
                return;

            mainTab.Enabled = true;
            FN.ResetInput(mainTab);


            visitid = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "ID_VISIT");
            headid = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "HEAD_ID");
            RMNO = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "RM_NO");
            pasienno = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "PATIENT_NO");
            type_s = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "GROUP_PATIENT");
            inpatient_id = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "INPATIENT_ID");
            fnama = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "NAME");
            labelControl106.Text = RMNO;
            labelControl107.Text = fnama;

            if (type_s.ToString().Equals("Umum"))
            {
                type_s = "U";
                panelControl3.Visible = false;
                splitContainerControl7.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2; // Hanya Panel 2 yang terlihat
                splitContainerControl8.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            }
            else if (type_s.ToString().Equals("Asuransi"))
            {
                type_s = "A";
                panelControl3.Visible = false;
                splitContainerControl7.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2; // Hanya Panel 2 yang terlihat
                splitContainerControl8.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            }
            else
            {
                panelControl3.Visible = true;
                splitContainerControl7.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both; // Hanya Panel 2 yang terlihat
                splitContainerControl8.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
                type_s = "B";
            }
            //LoadItemLayanan();

            //DataListObat(type_s);
            DataListObatGroup(type_s);
            // dtJadwalObat = ORADB.SetData(ORADB.XE, "select * from T1_JADWAL_BERI_OBAT where anamesa_id =" + anamesaID + "");
            dtCppt = ConnOra.Data_Table_ora("SELECT * FROM ( select a.*,  case when ctype = 'S' then tanggal || 1 when ctype = 'O' then tanggal || 2 when ctype = 'A' then tanggal || 3 when ctype = 'P' then tanggal || 4 END SSORT from T1_CPPT a where anamesa_id = " + anamesaID + " ) ORDER BY SSORT   ");
            dtObatPulang = ConnOra.Data_Table_ora("select * from T1_OBAT_PULANG where anamesa_id =" + anamesaID + " ");
            dtVital = ConnOra.Data_Table_ora("select * from T1_GRAFIK_VITAL where anamesa_id =" + anamesaID + " ");
            //ORADB.SetData(ORADB.XE, "select * from T1_CPPT where anamesa_id =" + anamesaID + " "); 
            //ORADB.SetData(ORADB.XE,  "select * from T1_OBAT_PULANG where anamesa_id =" + anamesaID + " ");  
            //ORADB.SetData(ORADB.XE, "select * from T1_GRAFIK_VITAL where anamesa_id =" + anamesaID + " ");

            try
            {
                if (ConnOra.Data_Table_ora("select * from T1_RAWAT_INAP1 where anamesa_id =" + anamesaID + " ").Rows.Count > 0)
                //if (ORADB.SetData(ORADB.XE, "select * from T1_RAWAT_INAP1 where anamesa_id = " + anamesaID + "").Rows.Count > 0)
                {
                    getData(anamesaID);
                }
                else
                {
                    string newId = ORADB.getData(ORADB.XE, "select rawat_inap_seq.NEXTVAL new_id from dual ", "NEW_ID");
                    string newId2 = ORADB.getData(ORADB.XE, "select resiko_jatuh_seq.NEXTVAL new_id from dual ", "NEW_ID");
                    List<string> sql = new List<string>();
                    sql.Add("insert into T1_RAWAT_INAP1 (id, anamesa_id) values (" + newId + "," + anamesaID + ")");
                    sql.Add("insert into T1_RAWAT_INAP2 (id, anamesa_id) values (" + newId + "," + anamesaID + ")");
                    sql.Add("insert into T1_PERENCANAAN_PULANG (id, anamesa_id) values (" + newId + "," + anamesaID + ")");
                    sql.Add("insert into T1_RESUME_PULANG (id, anamesa_id) values (" + newId + "," + anamesaID + ")");
                    sql.Add("insert into T1_ASESMEN_GIZI (anamesa_id) values (" + anamesaID + ")");
                    ORADB.DbTrans(ORADB.XE, sql);
                    getData(anamesaID);
                }
                btnInputData.Enabled = false;
                LoadItemLayananType(type_s);
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
        }

        private void gridHRacik_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
         
        private void LoadDataResep()
        {
            string sql_med_load = "",  s_que = "", s_que2 = "";
             
            sql_med_load = " select a.receipt_id, a.med_cd, b.med_group, a.med_cd, A.formula, type_drink,  " +
                           " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +  " +
                           " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -  " +
                           " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) - " +
                           " klinik.FN_CS_REQ_STOCK(a.insp_date,a.med_cd) stock, " +
                           " A.med_qty, initcap(uom) uom, 'S' action, a.confirm, a.days, a.price, a.qty_day, a.dosis,a.MED_REMARK REMARK , a.insp_date, a.INS_JAM	 " +
                           " from KLINIK.cs_receipt a  " +
                           " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)  JOIN KLINIK.cs_formula D ON (B.med_cd = D.med_cd AND D.FORMULA_ID = A.formula) " +
                           " where b.status = 'A' and D.MINUS_STOK ='Y' and a.ATT1_RECIEPT is null and a.JENIS_OBAT ='NONE' " +
                           " and rm_no = '" + RMNO + "'  " +
                           " and ID_VISIT = '" + visitid + "' and GRID_NAME = 'gvJadwalObat' and a.f_active ='Y'  and d.racikan ='N' order by  a.insp_date desc, a.INS_JAM desc,  a.med_cd ";

            DataTable dt2 =  ConnOra.Data_Table_ora(sql_med_load);
             
            gcJadwalObat.DataSource = null;
            gcJadwalObat.DataSource = dt2;

            gvJadwalObat.OptionsView.ColumnAutoWidth = true;
            gvJadwalObat.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gvJadwalObat.Appearance.HeaderPanel.FontSizeDelta = 0;
            gvJadwalObat.IndicatorWidth = 33;
            gvJadwalObat.BestFitColumns();

            gvJadwalObat.Columns[6].OptionsColumn.ReadOnly = true;
            gvJadwalObat.Columns[10].OptionsColumn.ReadOnly = true;

            gvJadwalObat.Columns[15].VisibleIndex = 0;
            gvJadwalObat.Columns[16].VisibleIndex = 1;
            gvJadwalObat.Columns[1].VisibleIndex = 2;
            gvJadwalObat.Columns[14].VisibleIndex = 3;
            gvJadwalObat.Columns[7].VisibleIndex = 4; 

            gvJadwalObat.Columns[3].Width = 200;
            gvJadwalObat.Columns[4].Width = 95;
            gvJadwalObat.Columns[5].Width = 150;
            gvJadwalObat.Columns[6].Width = 65;
            gvJadwalObat.Columns[7].Width = 60;
            gvJadwalObat.Columns[8].Width = 65;
            gvJadwalObat.Columns[10].Width = 65;
            gvJadwalObat.Columns[11].Width = 60;
            gvJadwalObat.Columns[14].Width = 55;
            gvJadwalObat.Columns[15].Width = 100;
            gvJadwalObat.Columns[16].Width = 70;
            gvJadwalObat.Columns[17].Width = 55;
            s_que = "";
            s_que = " select a.receipt_id, a.med_cd, b.med_group, a.med_cd med_cd1, a.formula, type_drink,  " +
                           " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +  " +
                           " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -  " +
                           " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) - " +
                           " klinik.FN_CS_REQ_STOCK(a.insp_date,a.med_cd) stock, " +
                           " a.med_qty, initcap(uom) uom, 'S' action, a.confirm, a.days, a.price, a.qty_day, a.dosis, a.insp_date, a.INS_JAM, a.cara" +
                           " from KLINIK.cs_receipt a  " +
                           " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)   JOIN KLINIK.cs_formula D ON (B.med_cd = D.med_cd AND D.FORMULA_ID = A.formula)  " +
                           " where b.status = 'A'  " +
                           " and rm_no = '" + RMNO + "'  and d.racikan ='N' " +
                           " and ID_VISIT = '" + visitid + "' and GRID_NAME = 'gvObtPlng' and a.f_active ='Y' order by a.insp_date desc, a.INS_JAM desc, a.med_cd ";

            DataTable dt = ConnOra.Data_Table_ora(s_que);
             
            gcObtPlng.DataSource = null; 
            gcObtPlng.DataSource = dt;

            gvObtPlng.OptionsView.ColumnAutoWidth = true;
            gvObtPlng.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gvObtPlng.Appearance.HeaderPanel.FontSizeDelta = 0;
            gvObtPlng.IndicatorWidth = 35;
            //gvJadwalObat.OptionsBehavior.Editable = false;
            gvObtPlng.BestFitColumns();
            gvObtPlng.Columns[6].OptionsColumn.ReadOnly = true;
            gvObtPlng.Columns[10].OptionsColumn.ReadOnly = true;

            //if (type_s == "B")
            //{
            s_que2 = "";
            s_que2 = " select a.receipt_id, a.med_cd, b.med_group, a.med_cd med_cd1, a.formula, type_drink,  " +
                               " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +  " +
                               " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -  " +
                               " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) - " +
                               " klinik.FN_CS_REQ_STOCK(a.insp_date,a.med_cd) stock, " +
                               " a.med_qty, initcap(uom) uom, 'S' action, a.confirm, a.days, a.price, a.qty_day, a.dosis, a.insp_date, a.INS_JAM, a.cara" +
                               " from KLINIK.cs_receipt a  " +
                               " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)  JOIN KLINIK.cs_formula D ON (B.med_cd = D.med_cd AND D.FORMULA_ID = A.formula)  " +
                               " where b.status = 'A'  " +
                               " and rm_no = '" + RMNO + "' and d.racikan ='N'  " +
                               " and ID_VISIT = '" + visitid + "' and GRID_NAME = 'gvObatUmum' and a.f_active ='Y'  order by a.insp_date desc, a.INS_JAM desc, a.med_cd  ";

                DataTable dtUmum = ConnOra.Data_Table_ora(s_que2);

                gObatUmum.DataSource = null;
                gObatUmum.DataSource = dtUmum;

                gvObatUmum.OptionsView.ColumnAutoWidth = true;
                gvObatUmum.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gvObatUmum.Appearance.HeaderPanel.FontSizeDelta = 0;
                gvObatUmum.IndicatorWidth = 33;
                gvObatUmum.BestFitColumns();

                gvObatUmum.Columns[6].OptionsColumn.ReadOnly = true;
                gvObatUmum.Columns[10].OptionsColumn.ReadOnly = true;

                gvObatUmum.Columns[15].VisibleIndex = 0;
                gvObatUmum.Columns[16].VisibleIndex = 1;
                gvObatUmum.Columns[1].VisibleIndex = 2;
                gvObatUmum.Columns[14].VisibleIndex = 3;
                gvObatUmum.Columns[7].VisibleIndex = 4;

                gvObatUmum.Columns[3].Width = 200;
                gvObatUmum.Columns[4].Width = 95;
                gvObatUmum.Columns[5].Width = 150;
                gvObatUmum.Columns[6].Width = 65;
                gvObatUmum.Columns[7].Width = 60;
                gvObatUmum.Columns[8].Width = 65;
                gvObatUmum.Columns[10].Width = 65;
                gvObatUmum.Columns[11].Width = 60;
                gvObatUmum.Columns[14].Width = 55;
                gvObatUmum.Columns[15].Width = 110;
                gvObatUmum.Columns[16].Width = 70;
                gvObatUmum.Columns[17].Width = 55;

            //}

            string idracik = "", sql_racik2 ="";
            sql_racik2 = " select distinct a.ATT1_RECIEPT CODE_ID, a.ATT1_RECIEPT RACIKAN, a.DOSIS, type_drink,a.ATT3_RECIEPT jumlah, a.ATT2_RECIEPT REMARK_RACIK, 'S' action , a.insp_date, a.INS_JAM " +
                          " from KLINIK.cs_receipt a  " +
                          " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)  JOIN KLINIK.cs_formula D ON (B.med_cd = D.med_cd AND D.FORMULA_ID = A.formula) " +
                          " join KLINIK.CS_CODE_DATA c on (a.ATT1_RECIEPT = c.CODE_ID and c.CODE_CLASS_ID = 'MED_RACIK' )  " +
                          " where b.status = 'A' and D.MINUS_STOK ='Y' AND a.JENIS_OBAT = 'RACIK' " +
                          "   and rm_no = '" + RMNO + "' AND RACIKAN ='Y' " + 
                          "   and id_visit = " + visitid + "  order by a.insp_date desc,   a.INS_JAM desc, a.ATT1_RECIEPT   ";

            OleDbConnection oraconR2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraR2 = new OleDbDataAdapter(sql_racik2, oraconR2);
            DataTable dtR2 = new DataTable();
            adOraR2.Fill(dtR2);

            if (dtR2.Rows.Count > 0)
            {
                idracik = dtR2.Rows[0]["CODE_ID"].ToString();
                
                ////if (sstatus.ToString().Equals("BPJS"))
                //gvRacik.Columns[3].ColumnEdit = glmedRacik;
                ////else
                //    //gvRacik.Columns[3].ColumnEdit = glmed;
                //gvRacik.Columns[4].ColumnEdit = glfor;
                //gvRacik.Columns[5].ColumnEdit = medicineInfoLookup;
                //gvRacik.Columns[14].ColumnEdit = dosisLookup;
            }
            LoadResepRacikan(idracik);

            gridRacik.DataSource = null;
            gridHRacik.Columns.Clear();
            gridRacik.DataSource = dtR2; 

            gridHRacik.OptionsView.ColumnAutoWidth = true;
            gridHRacik.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridHRacik.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridHRacik.IndicatorWidth = 30;
            //gridView17.OptionsBehavior.Editable = true;
            gridHRacik.BestFitColumns();

            gridHRacik.Columns[0].Caption = "ID";
            gridHRacik.Columns[1].Caption = "Racikan";
            gridHRacik.Columns[2].Caption = "Dosis";
            gridHRacik.Columns[3].Caption = "Info";
            gridHRacik.Columns[4].Caption = "Jumlah";
            gridHRacik.Columns[5].Caption = "Remark";
            gridHRacik.Columns[6].Caption = "Action";
            gridHRacik.Columns[7].Caption = "Tanggal";
            gridHRacik.Columns[8].Caption = "Jam";

            gridHRacik.Columns[0].Visible = true;
            gridHRacik.Columns[0].OptionsColumn.AllowEdit = false;
            gridHRacik.Columns[0].OptionsColumn.ReadOnly = true;
            gridHRacik.Columns[6].Visible = false;

            gridHRacik.Columns[0].VisibleIndex = 0;
            gridHRacik.Columns[7].VisibleIndex = 1;
            gridHRacik.Columns[8].VisibleIndex = 2;
            gridHRacik.Columns[1].VisibleIndex = 3;
            gridHRacik.Columns[2].VisibleIndex = 4;
            gridHRacik.Columns[4].VisibleIndex = 5;
            gridHRacik.Columns[3].VisibleIndex = 6;
            gridHRacik.Columns[5].VisibleIndex = 7;

            gridHRacik.Columns[0].MinWidth = 35;
            gridHRacik.Columns[0].MaxWidth = 35;
            gridHRacik.Columns[1].MinWidth = 80;
            gridHRacik.Columns[1].MaxWidth = 80;
            gridHRacik.Columns[2].MinWidth = 55;
            gridHRacik.Columns[2].MaxWidth = 55;
            gridHRacik.Columns[3].MinWidth = 135;
            gridHRacik.Columns[3].MaxWidth = 135;
            gridHRacik.Columns[4].MinWidth = 55;
            gridHRacik.Columns[4].MaxWidth = 55; 

            racikLookup.DataSource = listRacik;
            racikLookup.ValueMember = "RacikCode";
            racikLookup.DisplayMember = "RacikName";
            racikLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            racikLookup.AutoSearchColumnIndex = 0;
            racikLookup.ImmediatePopup = true;
            racikLookup.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            racikLookup.NullText = ""; 
            gridHRacik.Columns[1].ColumnEdit = racikLookup;

            //ConnOra.LookUpGridFilter(listMedicine, gvJadwalObat, "medicineCode", "medicineName", LokObatGrid, 1);
            //ConnOra.LookUpGridFilter(listMedicine, gvObtPlng, "medicineCode", "medicineName", LokObatGridP, 1);
            //ConnOra.LookUpGridFilter(listMedicineU, gvObatUmum, "medicineCode", "medicineName", LokObatGridU, 1);
            //ConnOra.LookUpGridFilter(listMedicineRacik, gvRacik, "medicineCode", "medicineName", LokObatGridR, 1);

            ConnOra.LookUpGroupGridFilter(lMedicine, gvJadwalObat, "Kategori", "Kode_Obat", "Nama_Obat", LokObatGrid, 1);
            ConnOra.LookUpGroupGridFilter(lMedicine, gvObtPlng, "Kategori", "Kode_Obat", "Nama_Obat", LokObatGridP, 1);
            ConnOra.LookUpGroupGridFilter(lMedicineU, gvObatUmum, "Kategori", "Kode_Obat", "Nama_Obat", LokObatGridU, 1);
            ConnOra.LookUpGroupGridFilter(lMedicineRacik, gvRacik, "Kategori", "Kode_Obat", "Nama_Obat", LokObatGridR, 1);

            ////LoadResepRacikan(visitid);

            ////RepositoryItemGridLookUpEdit glmed = new RepositoryItemGridLookUpEdit();
            //glmed.DataSource = listMedicine;
            //glmed.ValueMember = "medicineCode";
            //glmed.DisplayMember = "medicineName";
            //glmed.PopulateViewColumns();
            //glmed.View.Columns["medicineCode"].Width = 35;
            //glmed.View.Columns["medicineName"].Width = 200;
            //glmed.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //glmed.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains; 
            ////glmed.Appearance.Font.Size = 11;
            //glmed.ImmediatePopup = true;
            //glmed.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            //glmed.NullText = "";
            //gvJadwalObat.Columns[1].ColumnEdit = glmed;
            //gvObtPlng.Columns[1].ColumnEdit = glmed; 

            //glmedU.DataSource = listMedicineU;
            //glmedU.ValueMember = "medicineCode";
            //glmedU.DisplayMember = "medicineName"; 
            //glmedU.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //glmedU.AutoSearchColumnIndex = 0;
            //glmedU.ImmediatePopup = true;
            //glmedU.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            //glmedU.NullText = ""; 
            //gvObatUmum.Columns[1].ColumnEdit = glmedU; 

            //glmedRacik.DataSource = listMedicineRacik;
            //glmedRacik.ValueMember = "medicineCode";
            //glmedRacik.DisplayMember = "medicineName";
            //glmedRacik.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //glmedRacik.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            //glmedRacik.ImmediatePopup = true;
            //glmedRacik.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            //glmedRacik.NullText = "";
            //gvRacik.Columns[1].ColumnEdit = glmedRacik;

            string sql_for = "";
            sql_for = sql_for + Environment.NewLine + "  select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1 and POLI_CD ='POL0001' and MINUS_STOK ='Y'  and upper(att1) = decode(upper('" + type_s + "'), 'B', 'BPJS', 'A', 'ASURANSI', 'UMUM')   AND RACIKAN ='N' ";
            //if(sstatus.ToString().Equals("BPJS"))
            //     sql_for = sql_for + Environment.NewLine + "and BPJS_COVER ='Y'";  

            OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
            DataTable dtf = new DataTable();
            adOraf.Fill(dtf);
            //listFormula.Clear();
            listFormula2.Clear();
            //listFormulaU.Clear();
            for (int i = 0; i < dtf.Rows.Count; i++)
            {
                //listFormula.Add(new Formula() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                listFormulaR.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
            } 

            //if(type_s.ToString().Equals("B"))
            //{
               
                string Sql = " ";
                Sql = Sql + Environment.NewLine + " select formula_id, initcap(formula) formula, initcap(b.med_name) || decode(att1,'BPJS','',' [None BPJS]') med_name ";
                Sql = Sql + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1      ";
                Sql = Sql + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'    ";
                Sql = Sql + Environment.NewLine + "    and POLI_CD ='POL0001'  AND RACIKAN ='Y'   ";
                //Sql = Sql + Environment.NewLine + "  UNION ALL  ";
                //Sql = Sql + Environment.NewLine + " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name  ";
                //Sql = Sql + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1      ";
                //Sql = Sql + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  and att1 ='UMUM'  ";
                //Sql = Sql + Environment.NewLine + "    and POLI_CD = 'POL0001'     ";
                //Sql = Sql + Environment.NewLine + "    and b.med_cd not in ( select b.med_cd   ";
                //Sql = Sql + Environment.NewLine + "                           from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1      ";
                //Sql = Sql + Environment.NewLine + "                            and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  and att1 = 'BPJS'  ";
                //Sql = Sql + Environment.NewLine + "                            and POLI_CD ='POL0001'   ";
                //Sql = Sql + Environment.NewLine + "                        )  ";
                Sql = Sql + Environment.NewLine + "  order by 1,3  ";


                OleDbConnection oraConnectR = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOraR = new OleDbDataAdapter(Sql, oraConnectR);
                DataTable dtR = new DataTable();
                adOraR.Fill(dtR); 
                listFormulaR.Clear(); 
                for (int i = 0; i < dtR.Rows.Count; i++)
                {
                    listFormulaR.Add(new Formula2() { formulaCode = dtR.Rows[i]["formula_id"].ToString(), formulaName = dtR.Rows[i]["formula"].ToString(), medicineName = dtR.Rows[i]["med_name"].ToString() });
                }
            //}

            glfor.DataSource = listFormula2;
            glfor.ValueMember = "formulaCode";
            glfor.DisplayMember = "formulaName";

            glfor.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glfor.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glfor.ImmediatePopup = true;
            glfor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glfor.NullText = "";
            gvJadwalObat.Columns[4].ColumnEdit = glfor;
            gvObtPlng.Columns[4].ColumnEdit = glfor;

            glforR.DataSource = listFormulaR;
            glforR.ValueMember = "formulaCode";
            glforR.DisplayMember = "formulaName";

            glforR.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glforR.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glforR.ImmediatePopup = true;
            glforR.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glforR.NullText = ""; 
            gvRacik.Columns[4].ColumnEdit = glforR;

            glforU.DataSource = listFormulaU;
            glforU.ValueMember = "formulaCode";
            glforU.DisplayMember = "formulaName";

            glforU.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glforU.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glforU.ImmediatePopup = true;
            glforU.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glforU.NullText = "";
            gvObatUmum.Columns[4].ColumnEdit = glforU;

            RepositoryItemTextEdit rpjam = new RepositoryItemTextEdit();
            rpjam.Mask.EditMask = "90:00";
            rpjam.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            gvJadwalObat.Columns[16].ColumnEdit = rpjam;
            gvObtPlng.Columns[16].ColumnEdit = rpjam;
            gvObatUmum.Columns[16].ColumnEdit = rpjam;
            gridHRacik.Columns[8].ColumnEdit = rpjam;
            gvMedis.Columns[2].ColumnEdit = rpjam;
            gvMedisU.Columns[2].ColumnEdit = rpjam;

            RepositoryItemDateEdit rptanggal = new RepositoryItemDateEdit();
            rptanggal.DisplayFormat.FormatString = "yyyy-MM-dd";
            rptanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            rptanggal.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            rptanggal.Mask.EditMask = "yyyy-MM-dd";
            rptanggal.Mask.UseMaskAsDisplayFormat = true;
            gvJadwalObat.Columns[15].ColumnEdit = rptanggal;
            gvObtPlng.Columns[15].ColumnEdit = rptanggal;
            gvCppt.Columns[1].ColumnEdit = rptanggal;
            gvObatUmum.Columns[15].ColumnEdit = rptanggal;
            gridHRacik.Columns[7].ColumnEdit = rptanggal;
            gvMedis.Columns[1].ColumnEdit = rptanggal;
            gvMedisU.Columns[1].ColumnEdit = rptanggal;

            //RepositoryItemLookUpEdit medicineInfoLookup = new RepositoryItemLookUpEdit();
            medicineInfoLookup.DataSource = listMedicineInfo;
            medicineInfoLookup.ValueMember = "medicineInfoCode";
            medicineInfoLookup.DisplayMember = "medicineInfoName";

            medicineInfoLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            medicineInfoLookup.DropDownRows = listMedicineInfo.Count;
            medicineInfoLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            medicineInfoLookup.AutoSearchColumnIndex = 0;
            medicineInfoLookup.NullText = "";
            gvJadwalObat.Columns[5].ColumnEdit = medicineInfoLookup;
            gvObtPlng.Columns[5].ColumnEdit = medicineInfoLookup;
            gvObatUmum.Columns[5].ColumnEdit = medicineInfoLookup;            
            gridHRacik.Columns[3].ColumnEdit = medicineInfoLookup;
            gvRacik.Columns[5].ColumnEdit = medicineInfoLookup;
            //RepositoryItemLookUpEdit medicineInfoLookup = new RepositoryItemLookUpEdit();
            //medicineInfoLookup.DataSource = listMedicineInfo;
            //medicineInfoLookup.ValueMember = "medicineInfoCode";
            //medicineInfoLookup.DisplayMember = "medicineInfoName";

            //medicineInfoLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //medicineInfoLookup.DropDownRows = listMedicineInfo.Count;
            //medicineInfoLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            //medicineInfoLookup.AutoSearchColumnIndex = 1;
            //medicineInfoLookup.NullText = "";
            //gvJadwalObat.Columns[5].ColumnEdit = medicineInfoLookup;

            //RepositoryItemLookUpEdit dosisLookup = new RepositoryItemLookUpEdit();
            dosisLookup.DataSource = listDosis;
            dosisLookup.ValueMember = "DosisCode";
            dosisLookup.DisplayMember = "DosisName";
            dosisLookup.NullText = "";
            gvJadwalObat.Columns[14].ColumnEdit = dosisLookup;
            gvObtPlng.Columns[14].ColumnEdit = dosisLookup;
            gvObatUmum.Columns[14].ColumnEdit = dosisLookup;
            gridHRacik.Columns[2].ColumnEdit = dosisLookup;
            gvRacik.Columns[14].ColumnEdit = dosisLookup;
            //btnMedAdd.Enabled = true;
            //btnNoReceipt.Enabled = true;

            //if (gvJadwalObat.RowCount > 0)
            //{
            //    btnMedDel.Enabled = true;
            //    btnMedCan.Enabled = true;
            //}
            //else
            //{
            //    btnMedDel.Enabled = false;
            //    btnMedCan.Enabled = true;
            //}
        }

        private void timerCek_Tick(object sender, EventArgs e)
        {
            timer++;

            if (timer == cek_interval)
            {
                timer = 0;
                timerCek.Stop();
                timerCek.Start();

                //lblobatS.Visible = false ;
            }
        }

        private void  simpansukses(LabelControl lbl, string stssimpan)
        {
            timerCek.Start();
            if(stssimpan.ToString().Equals("Y"))
                SoftBlink(lbl, Color.ForestGreen, Color.LimeGreen, 1600, false);
            else
                SoftBlink(lbl, Color.LightPink, Color.Red, 1600, false);

            //lbl.Visible = false;
        }
        private async void SoftBlink(Control ctrl, Color c1, Color c2, short CycleTime_ms, bool BkClr)
        { 
            var sw = new Stopwatch(); sw.Start();
            short halfCycle = (short)Math.Round(CycleTime_ms * 0.5);
            while (true)
            {
                await Task.Delay(1);
                var n = sw.ElapsedMilliseconds % CycleTime_ms;
                var per = (double)Math.Abs(n - halfCycle) / halfCycle;
                var red = (short)Math.Round((c2.R - c1.R) * per) + c1.R;
                var grn = (short)Math.Round((c2.G - c1.G) * per) + c1.G;
                var blw = (short)Math.Round((c2.B - c1.B) * per) + c1.B;
                var clr = Color.FromArgb(red, grn, blw);
                if (BkClr) ctrl.BackColor = clr; else ctrl.ForeColor = clr; 
            } 
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            //timer1.Enabled = false;
            //timer3.Enabled = false;
            //_currentLabel.Visible = false;
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
    }
}
