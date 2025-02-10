using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using System.IO;
using System.Threading;
using System.Web;
using NAudio.Wave;
using System.Net;
using Clinic.Report;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors; 

namespace Clinic
{
    public partial class ReservationRegister : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        Poli poli = new Poli();
        List<Poli> listPoli = new List<Poli>();
        List<PatientType> listPatientType = new List<PatientType>();

        List<Purpose> listPurpose = new List<Purpose>();
        List<Status> listStat = new List<Status>();
        List<Room> listRoom = new List<Room>();
        List<Patient> listPatient = new List<Patient>();
        List<Guarantor> listGuarantor = new List<Guarantor>();
        List<Kehamilan> listKehamilan = new List<Kehamilan>();
        List<WorkAccident> listWorkAccident = new List<WorkAccident>();
        List<Stat> listStat2 = new List<Stat>();
        List<Stat> statIn = new List<Stat>();
        List<Stat> statFrom = new List<Stat>();
        List<Stat> statOut = new List<Stat>();
        List<Stat> statPasien = new List<Stat>();
        List<Layanan> listLayanan = new List<Layanan>();
        List<Layanan> listLaya2 = new List<Layanan>();
        List<Formula> listFormula = new List<Formula>();
        List<Formula2> listFormula2 = new List<Formula2>();
        List<Medicine> listMedicine = new List<Medicine>();
        List<MedicineInfo> listMedicineInfo = new List<MedicineInfo>();
        List<Dosis> listDosis = new List<Dosis>();

        DataSet dsAgree = new DataSet(); DataTable dtGlMed = new DataTable();
        DataSet dsKetRanap = new DataSet();
        DataTable dtJadwalObat; DataTable dtMedis; DataTable dtObat;

        RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
        ObsNotif obsNotif = null;
        RsvNotif rsvNotif = null;

        public string sql_all = ""; 
        public string v_rmnumber = "";
        public string v_ptnumber = "";
        public string s_nik = "", visitid = "", headid = ""; 

        string today = DateTime.Now.ToString("dd/MM/yyyy");
        //string today = "2019-11-27";
        string workingDirectory = "", resourcesDirectory = "";
        string upd_col = "", s_policd = "", pnama_pasien ="";
        int obst = 0, popup_interval = 999900;
        private Control lastSender;

        public ReservationRegister()
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
            btnSaveAnam.Enabled = false;
            btnAddAnam.Enabled = false;
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "ReservationRegister");
            //workingDirectory = Environment.CurrentDirectory;
            //resourcesDirectory = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\";
            initData();
            LoadData();
            //tableLayoutPanel1.RowStyles[4] = new RowStyle(SizeType.Absolute, 0);

            timerObs.Start();
            btnCreate.Enabled = false;
            btnSaveAnam.Enabled = false;
            btnAddAnam.Enabled = false;
            //workingDirectory = Environment.CurrentDirectory;
            //resourcesDirectory = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\";
            //initData();
            //LoadData();
            //tableLayoutPanel1.RowStyles[4] = new RowStyle(SizeType.Absolute, 0);
            //tableLayoutPanel1.RowStyles[5] = new RowStyle(SizeType.Absolute, 0);

        }

        private void initData()
        {
            string sql_poli = " select poli_cd, poli_name from cs_policlinic where status = 'A' and poli_cd not in ('POL0004','POL0008','POL0009') ";
            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_poli, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);
            listPoli.Clear();
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                listPoli.Add(new Poli() { poliCode = dt2.Rows[i]["poli_cd"].ToString(), poliName = dt2.Rows[i]["poli_name"].ToString() });
                //poli.poliCode = dt2.Rows[i]["poli_cd"].ToString();
                //poli.poliName = dt2.Rows[i]["poli_name"].ToString();
                //listPoli.Add(poli);
            } 

            listPatientType.Clear();
            listPatientType.Add(new PatientType() { patientTypeCode = "B", patientTypeName = "BPJS" });
            listPatientType.Add(new PatientType() { patientTypeCode = "U", patientTypeName = "Umum" });
            listPatientType.Add(new PatientType() { patientTypeCode = "A", patientTypeName = "Asuransi" });

            listWorkAccident.Clear();
            listWorkAccident.Add(new WorkAccident() { workAccidentCode = "Y", workAccidentName = "Yes" });
            listWorkAccident.Add(new WorkAccident() { workAccidentCode = "N", workAccidentName = "No" });

            listPurpose.Clear();
            listPurpose.Add(new Purpose() { purposeCode = "DOC", purposeName = "Dokter" });
            listPurpose.Add(new Purpose() { purposeCode = "MID", purposeName = "Bidan" });
            listPurpose.Add(new Purpose() { purposeCode = "ETC", purposeName = "Lain2" });

            listStat.Clear();
            listStat.Add(new Status() { statusCode = "PRE", statusName = "Preparation" });
            listStat.Add(new Status() { statusCode = "RSV", statusName = "Reservation" });
            listStat.Add(new Status() { statusCode = "NUR", statusName = "First Inspection" });
            listStat.Add(new Status() { statusCode = "INS", statusName = "Inspection" });
            //listStat.Add(new Status() { statusCode = "OBS", statusName = "Observation" });
            //listStat.Add(new Status() { statusCode = "MED", statusName = "Medicine" });
            //listStat.Add(new Status() { statusCode = "CLS", statusName = "Completed" });
            //listStat.Add(new Status() { statusCode = "HOL", statusName = "Hold" });
            listStat.Add(new Status() { statusCode = "CAN", statusName = "Cancel" });

            listKehamilan.Clear();
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K1", kehamilanName = "K1" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K2", kehamilanName = "K2" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K3", kehamilanName = "K3" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K4", kehamilanName = "K4" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K5", kehamilanName = "K5" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K6", kehamilanName = "K6" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K7", kehamilanName = "K7" });

            listStat2.Clear();
            listStat2.Add(new Stat() { statCode = "A", statName = "Active" });
            listStat2.Add(new Stat() { statCode = "I", statName = "Inactive" });

            string SQL2 = "";
            SQL2 = "";
            SQL2 = SQL2 + Environment.NewLine + "select patient_no, initcap(name) name from cs_patient_info where STATUS ='A' order by 2";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(SQL2, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            listPatient.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listPatient.Add(new Patient() { patientCode = dt.Rows[i]["patient_no"].ToString(), patientName = dt.Rows[i]["name"].ToString() });

            }

            listLayanan.Clear();
            string sql_laya = " select treat_item_id, treat_item_name from KLINIK.cs_treatment_item where treat_group_id = 'TRG08' order by 2 ";
            OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_laya, sqlConnect3);
            DataTable dt3 = new DataTable();
            adSql3.Fill(dt3);

            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                listLayanan.Add(new Layanan() { layananCode = dt3.Rows[i]["treat_item_id"].ToString(), layananName = dt3.Rows[i]["treat_item_name"].ToString() });
            }

            //dtGlMed.Clear();
            //string sql_med = " select med_cd, initcap(med_name) || ' (BPJS: ' || bpjs_cover || ')' med_name from KLINIK.cs_medicine where status = 'A' and MED_GROUP ='OBAT' order by med_name ";
            //OleDbConnection sqlConnect4 = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adSql4 = new OleDbDataAdapter(sql_med, sqlConnect4);
            //DataTable dt4 = new DataTable();
            //dtGlMed = dt4;
            //adSql4.Fill(dt4);
            //listMedicine.Clear();
            //for (int i = 0; i < dt4.Rows.Count; i++)
            //{
            //    listMedicine.Add(new Medicine() { medicineCode = dt4.Rows[i]["med_cd"].ToString(), medicineName = dt4.Rows[i]["med_name"].ToString() });
            //}

            listMedicineInfo.Clear();
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "A", medicineInfoName = "(P.C.) Sesudah Makan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "B", medicineInfoName = "(A.C.) Sebelum Makan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "C", medicineInfoName = "(P.R.N.) Bila Perlu" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "D", medicineInfoName = "(G.T.T) Diteteskan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "E", medicineInfoName = "(U.E) Pemakaian Luar" });

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

            subclear();
        }

        void subclear()
        {
            radioGroup1.SelectedIndex = 0; radioGroup2.SelectedIndex = 1; radioGroup3.SelectedIndex = 1;  radioGroup4.SelectedIndex = 0; radioGroup5.SelectedIndex = 0;
            radioGroup6.SelectedIndex = 0; radioGroup7.SelectedIndex = 0; radioGroup8.SelectedIndex = 1;  radioGroup9.SelectedIndex = 0; radioGroup10.SelectedIndex = 0;
            radioGroup11.SelectedIndex = 0;  radioGroup12.SelectedIndex = 0; radioGroup13.SelectedIndex = 0;  radioGroup14.SelectedIndex = 0; radioGroup15.SelectedIndex = 0;
            radioGroup16.SelectedIndex = 0;  radioGroup17.SelectedIndex = 0; rgNyeri.SelectedIndex = 1; rgTingkatNyeri.SelectedIndex = -1; radioGroup18.SelectedIndex = 0;
            radioGroup19.SelectedIndex = 0; radioGroup20.SelectedIndex = 0; radioGroup21.SelectedIndex = 0; radioGroup22.SelectedIndex = 1; radioGroup23.SelectedIndex = 1;
            radioGroup24.SelectedIndex = 1; radioGroup25.SelectedIndex = 1; radioGroup26.SelectedIndex = 1; radioGroup27.SelectedIndex = 0; radioGroup28.SelectedIndex = 0;
            radioGroup30.SelectedIndex = 0; radioGroup31.SelectedIndex = 0; radioGroup32.SelectedIndex = 0; radioGroup33.SelectedIndex = 0; radioGroup34.SelectedIndex = 0;
            radioGroup35.SelectedIndex = 0; radioGroup36.SelectedIndex = 0; radioGroup37.SelectedIndex = 1; radioGroup38.SelectedIndex = 1; radioGroup39.SelectedIndex = 1;
            txtsdr.Text = ""; textBox2.Text = ""; textBox3.Text = ""; textBox4.Text = ""; textBox5.Text = ""; textBox6.Text = ""; textBox7.Text = ""; txScorNyeri.Text = "";
            txt_menjalar.Text = ""; txt_srnnyeri.Text = ""; txt_beritahu.Text = ""; txt_hsl_s.Text = ""; txt_saran.Text = ""; txt_bb.Text = ""; txt_pbtb.Text = "";
            txt_h_sk.Text = ""; txt_ssaran.Text = ""; txt_h_skrining.Text = ""; txt_saran4.Text = ""; txt_p_penunjang.Text = "";
            //chkSkalaNyeri.SelectedIndex = -1 ;
            for (int i = 0; i < chkSkalaNyeri.Items.Count; i++)
            {
                chkSkalaNyeri.SetItemChecked(i, false);
            }
            txt_rekammds.Text = "";
        }

        private void LoadData()
        {
            string sql_search;

            sql_search = " ";
            sql_search = sql_search + Environment.NewLine + " select * from  ( ";
            sql_search = sql_search + Environment.NewLine + " SELECT  que01, a.patient_no, a.patient_no pasno, a.plan, decode(b.gender,'P','Perempuan','Laki-Laki') gender,  ";
            sql_search = sql_search + Environment.NewLine + "         round(((sysdate-b.birth_date)/30)/12) age,   ";
            sql_search = sql_search + Environment.NewLine + "         a.poli_cd, type_patient, work_accident, purpose, a.status, 'S' action,  ";
            sql_search = sql_search + Environment.NewLine + "         CASE WHEN observation = 'Y' THEN 'Yes' ELSE 'No'  ";
            sql_search = sql_search + Environment.NewLine + "         END AS observation, visit_remark, D.rm_no, "; 
            sql_search = sql_search + Environment.NewLine + "         DECODE (c.poli_group, 'PREG', 'Ibu Hamil', 'FAMP', 'KB', 'Umum' ) AS type_mr,  ";
            sql_search = sql_search + Environment.NewLine + "         a.poli_cd policd, round((nvl(start_hold,sysdate)-A.visit_date) * 24 * 60) wait_time , visit_remark resmark, a.ID_VISIT, e.ANAMNESA_ID, F.HEAD_ID, F.PAY_STATUS, b.NID NIK";
            sql_search = sql_search + Environment.NewLine + "         ,(select  max(z.CALL_ID) from CS_CALL_LOG z where a.que01 = z.QUE and to_char(z.INS_DATE,'yyyy-mm-dd')=to_char(sysdate,'yyyy-mm-dd') and TRUNC(A.visit_date) =  TRUNC(z.INS_DATE)  )CALL_ID ,case when a.status = 'REG' then 1 else 2 end stat  ";
            sql_search = sql_search + Environment.NewLine + "    FROM cs_visit a JOIN cs_patient_info b ON a.patient_no = b.patient_no  ";
            sql_search = sql_search + Environment.NewLine + "         join cs_patient D ON a.patient_no = D.patient_no  LEFT JOIN cs_policlinic c ON (a.poli_cd = c.poli_cd AND c.status = 'A') LEFT JOIN CS_ANAMNESA e ON (a.ID_VISIT = e.ID_VISIT) ";
            sql_search = sql_search + Environment.NewLine + "         LEFT JOIN KLINIK.cs_treatment_head F ON  (a.ID_VISIT = F.ID_VISIT) ";
            sql_search = sql_search + Environment.NewLine + "   WHERE 1 = 1  ";
            sql_search = sql_search + Environment.NewLine + "     AND TRUNC(A.visit_date) = TRUNC(sysdate)   ";
            sql_search = sql_search + Environment.NewLine + "     AND a.poli_cd not in ('POL0004')  ";
            sql_search = sql_search + Environment.NewLine + "     AND a.status IN ('PRE', 'RSV', 'NUR', 'INS', 'OBS', 'HOL','CAN')  "; 
            sql_search = sql_search + Environment.NewLine + "   UNION ALL ";
            sql_search = sql_search + Environment.NewLine + "   select QUE que01, '' patient_no, '' pasno, '' plan, '' gender,   ";
            sql_search = sql_search + Environment.NewLine + "         0 age,    ";
            sql_search = sql_search + Environment.NewLine + "         a.poli_cd, STYPE type_patient, 'N' work_accident,POLI_PIC purpose, 'REG' status, 'S' action,     ";
            sql_search = sql_search + Environment.NewLine + "         'No'   AS observation, '' visit_remark, '' rm_no,  ";
            sql_search = sql_search + Environment.NewLine + "         DECODE (TYPE_INS, 'REG', 'Pendaftaran', 'Umum' ) AS type_mr,   ";
            sql_search = sql_search + Environment.NewLine + "         a.poli_cd, round((nvl(a.INS_DATE,sysdate)-a.INS_DATE) * 24 * 60) wait_time , '' visit_remark, 0 ID_VISIT, 0 ANAMNESA_ID, 0 HEAD_ID, '' PAY_STATUS, '' NIK , a.CALL_ID ,1 stat ";
            sql_search = sql_search + Environment.NewLine + "   from  CS_CALL_LOG  a, cs_policlinic b ";
            sql_search = sql_search + Environment.NewLine + "   where a.POLI_CD = b.POLI_CD and FLAG <>'X' ";
            sql_search = sql_search + Environment.NewLine + "     and to_char(a.INS_DATE,'yyyy-mm-dd')=to_char(sysdate,'yyyy-mm-dd')  ";
            sql_search = sql_search + Environment.NewLine + "     and QUE not in (select NVL(que01,'N') from  cs_visit b where to_char(visit_date,'yyyy-mm-dd')= to_char(sysdate, 'yyyy-mm-dd') and b.STATUS NOT IN('CAN') ) ";
            sql_search = sql_search + Environment.NewLine + "   ) a order by stat||CALL_ID ,que01   ";
            
            //loading.ShowWaitForm();
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
                gridView1.IndicatorWidth = 30;
                //gridView1.OptionsBehavior.Editable = false;
                gridView1.BestFitColumns();
                //gridView1.OptionsSelection.MultiSelect = true;
                //gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView1.VisibleColumns[0].Width = 20;
                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[1].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[2].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[3].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[4].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[5].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                gridView1.Columns[1].OptionsColumn.AllowEdit = false;
                gridView1.Columns[2].OptionsColumn.AllowEdit = false;
                gridView1.Columns[3].OptionsColumn.AllowEdit = false;
                gridView1.Columns[4].OptionsColumn.AllowEdit = false;
                gridView1.Columns[5].OptionsColumn.AllowEdit = false;
                gridView1.Columns[6].OptionsColumn.AllowEdit = true ; 
                gridView1.Columns[8].OptionsColumn.AllowEdit = false;
                gridView1.Columns[9].OptionsColumn.AllowEdit = false;
                gridView1.Columns[11].OptionsColumn.AllowEdit = false;
                gridView1.Columns[12].OptionsColumn.AllowEdit = false;
                gridView1.Columns[13].OptionsColumn.AllowEdit = false;
                gridView1.Columns[14].OptionsColumn.AllowEdit = false;
                gridView1.Columns[15].OptionsColumn.AllowEdit = false;
                gridView1.Columns[16].OptionsColumn.AllowEdit = false;
                gridView1.Columns[17].OptionsColumn.AllowEdit = false;
                gridView1.Columns[18].OptionsColumn.AllowEdit = false;
                gridView1.Columns[19].OptionsColumn.AllowEdit = false;
                gridView1.Columns[22].OptionsColumn.AllowEdit = false;
                gridView1.Columns[23].OptionsColumn.AllowEdit = false;
                gridView1.Columns[24].Visible = false;
                gridView1.Columns[25].Visible = false;
                gridView1.Columns[0].OptionsColumn.AllowEdit = false;

                gridView1.Columns[0].Caption = "Antrian"; 
                gridView1.Columns[1].Caption = "Pasien No";
                gridView1.Columns[2].Caption = "Nama";
                gridView1.Columns[3].Caption = "Plan";
                gridView1.Columns[4].Caption = "Jenis Kelamin";
                gridView1.Columns[5].Caption = "Umur";
                gridView1.Columns[6].Caption = "Poli";
                gridView1.Columns[7].Caption = "Pasien";
                gridView1.Columns[8].Caption = "KK";
                gridView1.Columns[9].Caption = "Berobat";
                gridView1.Columns[10].Caption = "Status";
                gridView1.Columns[11].Caption = "Action";
                gridView1.Columns[12].Caption = "Observation";
                gridView1.Columns[13].Caption = "Remark";
                gridView1.Columns[14].Caption = "Medical Record";
                gridView1.Columns[15].Caption = "Type";
                gridView1.Columns[16].Caption = "Poli Cd";
                gridView1.Columns[17].Caption = "W.T.";
                gridView1.Columns[18].Caption = "Layanan";
                gridView1.Columns[19].Caption = "Visit ID";
                gridView1.Columns[20].Caption = "HEAD ID";
                gridView1.Columns[21].Caption = "PAY STS";
                gridView1.Columns[23].Caption = "NIK";

                //gridView1.Columns[6].MinWidth = 70;
                gridView1.Columns[6].MinWidth = 70;
                //gridView1.Columns[7].MinWidth = 70;
                gridView1.Columns[7].MinWidth = 70;
                //gridView1.Columns[10].MinWidth = 80;
                gridView1.Columns[10].MinWidth = 80;
                gridView1.Columns[17].Width = 50;

                gridView1.Columns[17].VisibleIndex = 6;

                //PRE, RSV, NUR, INS, OBS, MED, CLS, CAN

                RepositoryItemGridLookUpEdit glPatient = new RepositoryItemGridLookUpEdit();
                //glPatient.DataSource = listPatient;
                //glPatient.ValueMember = "patientCode";
                //glPatient.DisplayMember = "patientName";

                //glPatient.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //glPatient.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                //glPatient.ImmediatePopup = true;
                //glPatient.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                //glPatient.NullText = "";
                //gridView1.Columns[2].ColumnEdit = glPatient;

                ConnOra.LookUpGridFilter(listPatient, gridView1, "patientCode", "patientName", glPatient, 2);
                glPatient.ImmediatePopup = true;
                glPatient.PopupFilterMode = PopupFilterMode.Contains;

                RepositoryItemLookUpEdit poliLookup = new RepositoryItemLookUpEdit();
                poliLookup.DataSource = listPoli;
                poliLookup.ValueMember = "poliCode";
                poliLookup.DisplayMember = "poliName";

                poliLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                poliLookup.DropDownRows = listPoli.Count;
                poliLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                poliLookup.AutoSearchColumnIndex = 1;
                poliLookup.NullText = "";
                gridView1.Columns[6].ColumnEdit = poliLookup;

                RepositoryItemLookUpEdit patientLookup = new RepositoryItemLookUpEdit();
                patientLookup.DataSource = listPatientType;
                patientLookup.ValueMember = "patientTypeCode";
                patientLookup.DisplayMember = "patientTypeName";

                patientLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                patientLookup.DropDownRows = listPatientType.Count;
                patientLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                patientLookup.AutoSearchColumnIndex = 1;
                patientLookup.NullText = "";
                gridView1.Columns[7].ColumnEdit = patientLookup;

                RepositoryItemLookUpEdit workAccLookup = new RepositoryItemLookUpEdit();
                workAccLookup.DataSource = listWorkAccident;
                workAccLookup.ValueMember = "workAccidentCode";
                workAccLookup.DisplayMember = "workAccidentName";

                workAccLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                workAccLookup.DropDownRows = listWorkAccident.Count;
                workAccLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                workAccLookup.AutoSearchColumnIndex = 1;
                workAccLookup.NullText = "";
                gridView1.Columns[8].ColumnEdit = workAccLookup;

                RepositoryItemLookUpEdit purposeLookup = new RepositoryItemLookUpEdit();
                purposeLookup.DataSource = listPurpose;
                purposeLookup.ValueMember = "purposeCode";
                purposeLookup.DisplayMember = "purposeName";

                purposeLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                purposeLookup.DropDownRows = listPurpose.Count;
                purposeLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                purposeLookup.AutoSearchColumnIndex = 1;
                purposeLookup.NullText = "";
                gridView1.Columns[9].ColumnEdit = purposeLookup;

                //RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
                statusLookup.DataSource = listStat;
                statusLookup.ValueMember = "statusCode";
                statusLookup.DisplayMember = "statusName";

                statusLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup.DropDownRows = listStat.Count;
                statusLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup.AutoSearchColumnIndex = 1;
                statusLookup.NullText = "";
                gridView1.Columns[10].ColumnEdit = statusLookup;
                gridView1.BestFitColumns();

                //RepositoryItemLookUpEdit kLookup = new RepositoryItemLookUpEdit();
                //kLookup.DataSource = listLayanan;
                //kLookup.ValueMember = "layananCode";
                //kLookup.DisplayMember = "layananName";

                //kLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //kLookup.DropDownRows = listLayanan.Count;
                //kLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                //kLookup.AutoSearchColumnIndex = 1;
                //kLookup.NullText = "";
                //gridView1.Columns[18].ColumnEdit = replayanan;

                gridControl3.DataSource = null;
                gridView3.Columns.Clear();

                gridMedisPeriksa.DataSource = null;
                //gvMedisPeriksa.Columns.Clear(); 


                //RepositoryItemButtonEdit riButtonEdit = new RepositoryItemButtonEdit();
                //gridControl1.RepositoryItems.Add(riButtonEdit);
                //gridView1.Columns[17].ColumnEdit = riButtonEdit;

                gridView1.Columns[3].Visible = false;
                gridView1.Columns[8].Visible = false;
                gridView1.Columns[11].Visible = false;
                gridView1.Columns[12].Visible = false;
                gridView1.Columns[13].Visible = false;
                gridView1.Columns[15].Visible = false;
                gridView1.Columns[16].Visible = false;
                gridView1.Columns[17].Visible = false;
                gridView1.Columns[18].Visible = true;
                gridView1.Columns[20].Visible = false;
                gridView1.Columns[21].Visible = false;
                gridView1.Columns[2].OptionsColumn.AllowEdit = true;
                subclear();

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        //private void gridView1_RowClick(object sender, RowClickEventArgs e)
        //{
        //    //GridView View = sender as GridView;
        //    //string s_rm = "", s_que = "", s_poli = "", s_group = "", s_rmno = "", group = "", s_nama = "", s_berobat = "";

        //    //s_rm = View.GetRowCellDisplayText(e.RowHandle, View.Columns[14]);
        //    //s_que = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
        //    //s_nik = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
        //    //s_nama = View.GetRowCellDisplayText(e.RowHandle, View.Columns[2]);
        //    //s_poli = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);
        //    //s_berobat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);
        //    //s_rmno = View.GetRowCellDisplayText(e.RowHandle, View.Columns[14]);
        //    //s_group = View.GetRowCellDisplayText(e.RowHandle, View.Columns[15]);
        //    //s_policd = View.GetRowCellDisplayText(e.RowHandle, View.Columns[16]);

        //    //v_rmnumber = View.GetRowCellDisplayText(e.RowHandle, View.Columns[14]);
        //    //visitid = View.GetRowCellDisplayText(e.RowHandle, View.Columns[19]);
        //    //txt_rekammds.Text = v_rmnumber;

        //    //pnama_pasien = s_nama;

        //    //if (s_poli == "Poli Ibu Hamil")
        //    //{
        //    //    tableLayoutPanel6.RowStyles[0] = new RowStyle(SizeType.Percent, 12);
        //    //    tableLayoutPanel6.RowStyles[1] = new RowStyle(SizeType.Percent, 38);
        //    //    tableLayoutPanel6.RowStyles[2] = new RowStyle(SizeType.Percent, 12);
        //    //    tableLayoutPanel6.RowStyles[3] = new RowStyle(SizeType.Percent, 38);
        //    //}
        //    //else
        //    //{
        //    //    tableLayoutPanel6.RowStyles[0] = new RowStyle(SizeType.Percent, 10);
        //    //    tableLayoutPanel6.RowStyles[1] = new RowStyle(SizeType.Percent, 90);
        //    //    tableLayoutPanel6.RowStyles[2] = new RowStyle(SizeType.Percent, 0);
        //    //    tableLayoutPanel6.RowStyles[3] = new RowStyle(SizeType.Percent, 0);
        //    //}

        //    //if (s_rm == "")
        //    //{
        //    //    if (s_berobat == "Dokter")
        //    //    {
        //    //        btnCreate.Enabled = false;
        //    //    }
        //    //    else
        //    //    {
        //    //        btnCreate.Enabled = true;
        //    //    }

        //    //    btnSaveAnam.Enabled = false;
        //    //}
        //    //else
        //    //{
        //    //    btnCreate.Enabled = false;
        //    //}

        //    //string sql_addinfo = "", sql_info = "", p_col = "";

        //    //sql_addinfo = " select info_cd, description from cs_add_info where status = 'A' and poli_cd = '" + s_poli + "' ";

        //    //OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
        //    //OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_addinfo, sqlConnect2);
        //    //DataTable dt2 = new DataTable();
        //    //adSql2.Fill(dt2);

        //    //for (int i = 0; i < dt2.Rows.Count; i++)
        //    //{
        //    //    p_col = p_col + ", " + dt2.Rows[i]["info_cd"].ToString();
        //    //}

        //    ////if (s_group == "Umum")
        //    ////{
        //    //group = "COMM";
        //    ////}
        //    ////else if (s_group == "KB")
        //    ////{
        //    ////    group = "FAMP";
        //    ////}
        //    ////else
        //    ////{
        //    ////    group = "PREG";
        //    ////}

        //    //sql_info = " ";
        //    //sql_info = sql_info + " select  patient_no, group_patient, decode(group_patient,'PREG','Ibu Hamil','FAMP','KB','Umum') group_patient_nm, '" + s_nama + "' as nama, 'U' as a, status, rm_no ";
        //    //sql_info = sql_info + p_col;
        //    //sql_info = sql_info + " from cs_patient where status='A' and group_patient='" + group + "' and patient_no='" + s_nik + "' ";

        //    //OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
        //    //OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_info, sqlConnect3);
        //    //DataTable dt3 = new DataTable();
        //    //adSql3.Fill(dt3);

        //    //gridControl6.DataSource = null;
        //    //gridView6.Columns.Clear();
        //    //gridControl6.DataSource = dt3;

        //    ////gridView6.OptionsView.ColumnAutoWidth = true;
        //    //gridView6.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
        //    //gridView6.Appearance.HeaderPanel.FontSizeDelta = 0;
        //    ////gridView6.BestFitColumns();
        //    //int ii = 0;


        //    //gridView6.Columns[0].Caption = "Pasien No";
        //    //gridView6.Columns[1].Caption = "Type Record";
        //    //gridView6.Columns[2].Caption = "Type Record";
        //    //gridView6.Columns[3].Caption = "Nama";
        //    //gridView6.Columns[4].Caption = "Action";
        //    //gridView6.Columns[5].Caption = "Status";
        //    //gridView6.Columns[6].Caption = "Medical Record";

        //    //for (int i = 0; i < dt2.Rows.Count; i++)
        //    //{
        //    //    ii = i + 7;
        //    //    gridView6.Columns[ii].Caption = dt2.Rows[i]["description"].ToString();
        //    //}
        //    //RepositoryItemLookUpEdit statLookup = new RepositoryItemLookUpEdit();
        //    //statLookup.DataSource = listStat2;
        //    //statLookup.ValueMember = "statCode";
        //    //statLookup.DisplayMember = "statName";

        //    //statLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
        //    //statLookup.DropDownRows = listStat2.Count;
        //    //statLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
        //    //statLookup.AutoSearchColumnIndex = 1;
        //    //statLookup.NullText = "";
        //    //gridView6.Columns[5].ColumnEdit = statLookup;

        //    //gridView6.Columns[0].OptionsColumn.ReadOnly = true;
        //    //gridView6.Columns[1].OptionsColumn.ReadOnly = true;
        //    //gridView6.Columns[2].OptionsColumn.ReadOnly = true;
        //    //gridView6.Columns[3].OptionsColumn.ReadOnly = true;
        //    //gridView6.Columns[4].OptionsColumn.ReadOnly = true;

        //    //gridView6.Columns[1].Visible = false;
        //    //gridView6.Columns[4].Visible = false;
        //    //gridView6.Columns[6].Visible = false;

        //    //gridView6.BestFitColumns();

        //    //if (gridView6.RowCount > 0)
        //    //{
        //    //    btnSaveAdd.Enabled = true;
        //    //}
        //    //else
        //    //{
        //    //    btnSaveAdd.Enabled = false;
        //    //}

        //    //if (!visitid.ToString().Equals(""))
        //    //{
        //    //    string sql_anam = "";
        //    //    sql_anam = " select to_date(to_char(insp_date,'dd/MM/yyyy'),'dd/MM/yyyy') as insp_date, '" + s_nama + "' as nama, visit_no, " +
        //    //               " blood_press, pulse, temperature, allergy, anamnesa, info_k, 'S' action, rm_no, bb, tb, " +
        //    //               " cholesterol, blood_sugar, uric_acid, VITALHR, VITALRR, disease_now, disease_then, disease_family, anamnesa_physical, anamnesa_other, ANAMNESA_ID" +
        //    //               " from cs_anamnesa where ID_VISIT =  " + visitid + "  ";

        //    //    OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
        //    //    OleDbDataAdapter adSql = new OleDbDataAdapter(sql_anam, sqlConnect);
        //    //    DataTable dt = new DataTable();
        //    //    adSql.Fill(dt);

        //    //    gridControl2.DataSource = null;
        //    //    gridView2.Columns.Clear();
        //    //    gridControl2.DataSource = dt;



        //    //    if (dt.Rows.Count > 0)
        //    //    {

        //    //        v_ptnumber = dt.Rows[0]["ANAMNESA_ID"].ToString();
        //    //        dtJadwalObat = ORADB.SetData(ORADB.XE, "select * from T1_JADWAL_BERI_OBAT where anamesa_id =" + v_ptnumber + " AND F_AKTIF ='Y'");
        //    //        gcJadwalObat.DataSource = dtJadwalObat;
        //    //    }
        //    //    else
        //    //    {
        //    //        v_ptnumber = "";
        //    //        if (gcJadwalObat.DataSource != null)
        //    //        {
        //    //            dtJadwalObat.Rows.Clear();
        //    //        }
        //    //        //if (!v_ptnumber.ToString().Equals(""))

        //    //        //dtJadwalObat.Columns.Clear();
        //    //        //dtJadwalObat.Reset();
        //    //        gcJadwalObat.DataSource = null;
        //    //        return;
        //    //    }



        //    //    //gridView2.OptionsView.ColumnAutoWidth = true;
        //    //    gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
        //    //    gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
        //    //    //gridView2.BestFitColumns();
        //    //    gridView2.FixedLineWidth = 3;
        //    //    gridView2.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
        //    //    gridView2.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
        //    //    gridView2.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

        //    //    gridView2.Columns[0].Caption = "Tanggal";
        //    //    gridView2.Columns[1].Caption = "Nama";
        //    //    gridView2.Columns[2].Caption = "Antrian";
        //    //    gridView2.Columns[3].Caption = "Tensi";
        //    //    gridView2.Columns[4].Caption = "Nadi";
        //    //    gridView2.Columns[5].Caption = "Suhu";
        //    //    gridView2.Columns[6].Caption = "Alergi";
        //    //    gridView2.Columns[7].Caption = "Keluhan Utama";
        //    //    gridView2.Columns[8].Caption = "Kehamilan";
        //    //    gridView2.Columns[9].Caption = "Action";
        //    //    gridView2.Columns[10].Caption = "Medical Record";
        //    //    gridView2.Columns[11].Caption = "BB (Kg)";
        //    //    gridView2.Columns[12].Caption = "TB (Cm)";
        //    //    gridView2.Columns[13].Caption = "Kolesterol (Mg)";
        //    //    gridView2.Columns[14].Caption = "Gula Darah (Mg)";
        //    //    gridView2.Columns[15].Caption = "Asam Urat (Mg)";
        //    //    gridView2.Columns[16].Caption = "HR (x/m)";
        //    //    gridView2.Columns[17].Caption = "RR (x/m)";
        //    //    gridView2.Columns[18].Caption = "R.Sekarang";
        //    //    gridView2.Columns[19].Caption = "R.Dulu";
        //    //    gridView2.Columns[20].Caption = "R.Keluarga";
        //    //    gridView2.Columns[21].Caption = "Pem.Fisik";
        //    //    gridView2.Columns[22].Caption = "Pem.Lain";

        //    //    RepositoryItemLookUpEdit kLookup = new RepositoryItemLookUpEdit();
        //    //    kLookup.DataSource = listKehamilan;
        //    //    kLookup.ValueMember = "kehamilanCode";
        //    //    kLookup.DisplayMember = "kehamilanName";

        //    //    kLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
        //    //    kLookup.DropDownRows = listKehamilan.Count;
        //    //    kLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
        //    //    kLookup.AutoSearchColumnIndex = 1;
        //    //    kLookup.NullText = "";
        //    //    gridView2.Columns[8].ColumnEdit = kLookup;


        //    //    if (s_poli == "Poli Ibu Hamil")
        //    //    {
        //    //        gridView2.Columns[8].Visible = true;
        //    //    }
        //    //    else
        //    //    {
        //    //        gridView2.Columns[8].Visible = false;
        //    //    }

        //    //    //gridView2.Columns[8].Visible = false;
        //    //    gridView2.Columns[9].Visible = false;
        //    //    gridView2.Columns[10].Visible = false;
        //    //    gridView2.Columns[16].Visible = false;
        //    //    gridView2.Columns[23].Visible = false;
        //    //    gridView2.Columns[11].VisibleIndex = 6;
        //    //    gridView2.Columns[12].VisibleIndex = 7;
        //    //    gridView2.BestFitColumns();

        //    //    if (gridView2.RowCount > 0)
        //    //    {
        //    //        btnSaveAnam.Enabled = true;
        //    //        //btnAddAnam.Enabled = false;
        //    //    }
        //    //    else
        //    //    {
        //    //        btnSaveAnam.Enabled = false;
        //    //        //btnAddAnam.Enabled = true;
        //    //    }

        //    //    if (s_rm != "")
        //    //    {
        //    //        btnAddAnam.Enabled = true;
        //    //    }
        //    //    else
        //    //    {
        //    //        btnAddAnam.Enabled = false;
        //    //    }


        //    //    LoadDataResep();
        //    //    ListDataLayanan(visitid);

        //    //    string sql_cek_hold = "", temp_shold = "", temp_ehold = "";

        //    //    sql_cek_hold = " select to_char(start_hold,'yyyy-mm-dd') s_hold, to_char(end_hold,'yyyy-mm-dd') e_hold from cs_visit where patient_no = '" + s_nik + "' and trunc(visit_date) =  trunc(to_date('" + today + "','dd/MM/yyyy'))  and que01 = '" + s_que + "' ";

        //    //    OleDbConnection sqlConnect4 = ConnOra.Create_Connect_Ora();
        //    //    OleDbDataAdapter adSql4 = new OleDbDataAdapter(sql_cek_hold, sqlConnect4);
        //    //    DataTable dt4 = new DataTable();
        //    //    adSql4.Fill(dt4);

        //    //    if (dt4.Rows.Count > 0)
        //    //    {
        //    //        temp_shold = dt4.Rows[0]["s_hold"].ToString();
        //    //        temp_ehold = dt4.Rows[0]["e_hold"].ToString();

        //    //        if (temp_shold == "" && temp_ehold == "")
        //    //        {
        //    //            btnTunda.Enabled = true;
        //    //            btnLanjut.Enabled = false;
        //    //        }
        //    //        else if (temp_shold != "" && temp_ehold == "")
        //    //        {
        //    //            btnTunda.Enabled = false;
        //    //            btnLanjut.Enabled = true;
        //    //        }
        //    //        else if (temp_shold != "" && temp_ehold != "")
        //    //        {
        //    //            btnTunda.Enabled = false;
        //    //            btnLanjut.Enabled = false;
        //    //        }
        //    //        else
        //    //        {
        //    //            btnTunda.Enabled = false;
        //    //            btnLanjut.Enabled = false;
        //    //        }
        //    //    }

        //    //    if (dt.Rows.Count > 0)
        //    //    {
        //    //        DataTable dt1 = ORADB.SetData(ORADB.XE, "select a.*,b.*,c.*,d.*,round((bb/(tb*tb))* 10000,2) imt  from klinik.cs_anamnesa a, klinik.cs_anamnesa_dtl b, klinik.cs_visit c, klinik.CS_PATIENT_INFO d where A.ANAMNESA_ID = b.anamnesa_id   and b.PATIENT_NO = c.PATIENT_NO and b.PATIENT_NO = d.PATIENT_NO and trunc(VISIT_DATE) = trunc(to_date('" + today + "','dd/MM/yyyy')) and a.ANAMNESA_ID = " + v_ptnumber + " ");
        //    //        if (dt1.Rows.Count > 0)
        //    //        {
        //    //            //mmKeluhan.Text = FN.rowVal(dt1, "KELUHAN_UTAMA");
        //    //            FN.splitVal(FN.rowVal(dt1, "C_MSK_RS"), radioGroup16);
        //    //            FN.splitVal(FN.rowVal(dt1, "SDR_KANDUNG"), radioGroup3);
        //    //            FN.splitVal(FN.rowVal(dt1, "SDR_TIRI"), radioGroup2);
        //    //            FN.splitVal1(FN.rowVal(dt1, "TGL_BERSAMA"), radioGroup4, textBox3);
        //    //            FN.splitVal(FN.rowVal(dt1, "SBICARA"), radioGroup5);
        //    //            FN.splitVal(FN.rowVal(dt1, "SKOMUNIKASI"), radioGroup6);
        //    //            FN.splitVal(FN.rowVal(dt1, "SEMOSI"), radioGroup7);
        //    //            FN.splitVal(FN.rowVal(dt1, "RJIWA"), radioGroup8);
        //    //            FN.splitVal(FN.rowVal(dt1, "KSPIRITUAL"), radioGroup9);
        //    //            FN.splitVal(FN.rowVal(dt1, "RTRAUMA"), radioGroup10);
        //    //            FN.splitVal(FN.rowVal(dt1, "APERASAAN"), radioGroup13);
        //    //            FN.splitVal(FN.rowVal(dt1, "INWAWANCARA"), radioGroup1);
        //    //            FN.splitVal(FN.rowVal(dt1, "MSPIRITUAL"), radioGroup11);
        //    //            FN.splitVal(FN.rowVal(dt1, "KSPIRITUAL"), radioGroup12);
        //    //            FN.splitVal(FN.rowVal(dt1, "JOB"), radioGroup14);
        //    //            FN.splitVal(FN.rowVal(dt1, "STAT_KAWIN"), radioGroup15);
        //    //            FN.splitVal(FN.rowVal(dt1, "JNS_PELAYANAN"), radioGroup17);
        //    //            FN.setCheckList(FN.rowVal(dt1, "SKALA_NYERI"), chkSkalaNyeri);
        //    //            txScorNyeri.Text = FN.rowVal(dt1, "SCORE_NYERI");
        //    //            FN.splitVal(FN.rowVal(dt1, "TINGKAT_NYERI"), rgTingkatNyeri);
        //    //            FN.splitVal(FN.rowVal(dt1, "KUALITAS_NYERI"), radioGroup18);
        //    //            FN.splitVal(FN.rowVal(dt1, "MENJALAR"), radioGroup19);
        //    //            FN.splitVal(FN.rowVal(dt1, "FREKUENSI_NYERI"), radioGroup20);
        //    //            FN.splitVal(FN.rowVal(dt1, "PENGARUH_NYERI"), radioGroup28);
        //    //            FN.splitVal(FN.rowVal(dt1, "PSEMPOYONGAN"), radioGroup22);
        //    //            FN.splitVal(FN.rowVal(dt1, "PPENOPANG"), radioGroup23);
        //    //            FN.splitVal(FN.rowVal(dt1, "HRESIKO"), radioGroup27);
        //    //            FN.splitVal(FN.rowVal(dt1, "BERITAHU_DOKTER"), radioGroup21);
        //    //            FN.splitVal(FN.rowVal(dt1, "SG_KURUS"), radioGroup24);
        //    //            FN.splitVal(FN.rowVal(dt1, "SG_TURUNBB"), radioGroup25);
        //    //            FN.splitVal(FN.rowVal(dt1, "SG_ASUPAN"), radioGroup26);
        //    //            FN.splitVal(FN.rowVal(dt1, "AFS_PENGLIHATAN"), radioGroup30);
        //    //            FN.splitVal(FN.rowVal(dt1, "AFS_PENCIUMAN"), radioGroup31);
        //    //            FN.splitVal(FN.rowVal(dt1, "AFS_PENDENGARAN"), radioGroup32);
        //    //            FN.splitVal(FN.rowVal(dt1, "AFS_KOGNITIF1"), radioGroup33);
        //    //            FN.splitVal(FN.rowVal(dt1, "AFS_KOGNITIF2"), radioGroup34);
        //    //            FN.splitVal(FN.rowVal(dt1, "AFS_MOTOR_SHRI"), radioGroup35);
        //    //            FN.splitVal(FN.rowVal(dt1, "AFS_MOTOR_JALAN"), radioGroup36);
        //    //            FN.splitVal(FN.rowVal(dt1, "DPS_HOME_CARE"), radioGroup37);
        //    //            FN.splitVal(FN.rowVal(dt1, "DPS_IMPLAN"), radioGroup38);
        //    //            FN.splitVal(FN.rowVal(dt1, "DPS_PULANG"), radioGroup39);
        //    //            FN.setCheckList(FN.rowVal(dt1, "ALERGI_MKN"), chkSkalaNyeri);
        //    //            FN.splitVal2(FN.rowVal(dt1, "ALERGI_MKN"), gbMakan, txMakanan);
        //    //            FN.splitVal2(FN.rowVal(dt1, "ALERGI_OBAT"), gbObat, txtaobat);

        //    //            txt_bb.Text = FN.rowVal(dt1, "BB");
        //    //            txt_pbtb.Text = FN.rowVal(dt1, "TB");
        //    //            txt_imt.Text = FN.rowVal(dt1, "IMT");
        //    //        }
        //    //    }
        //    //}

        //}

        void datapasienisi(string v_ptnumber)
        {
            DataTable dt1 = ORADB.SetData(ORADB.XE, "select a.*,b.*,c.*,d.*,round((bb/(tb*tb))* 10000,2) imt from klinik.cs_anamnesa a, klinik.cs_anamnesa_dtl b, klinik.cs_visit c, klinik.CS_PATIENT_INFO d where A.ANAMNESA_ID = b.anamnesa_id   and b.PATIENT_NO = c.PATIENT_NO and b.PATIENT_NO = d.PATIENT_NO and trunc(VISIT_DATE) = trunc(to_date('" + today + "','dd/MM/yyyy')) and a.ANAMNESA_ID = " + v_ptnumber + " ");
            if (dt1.Rows.Count > 0)
            {
                //mmKeluhan.Text = FN.rowVal(dt1, "KELUHAN_UTAMA");
                FN.splitVal(FN.rowVal(dt1, "C_MSK_RS"), radioGroup16);
                FN.splitVal(FN.rowVal(dt1, "SDR_KANDUNG"), radioGroup3);
                FN.splitVal(FN.rowVal(dt1, "SDR_TIRI"), radioGroup2);
                FN.splitVal1(FN.rowVal(dt1, "TGL_BERSAMA"), radioGroup4, textBox3);
                FN.splitVal(FN.rowVal(dt1, "SBICARA"), radioGroup5);
                FN.splitVal(FN.rowVal(dt1, "SKOMUNIKASI"), radioGroup6);
                FN.splitVal(FN.rowVal(dt1, "SEMOSI"), radioGroup7);
                FN.splitVal(FN.rowVal(dt1, "RJIWA"), radioGroup8);
                FN.splitVal(FN.rowVal(dt1, "KSPIRITUAL"), radioGroup9);
                FN.splitVal(FN.rowVal(dt1, "RTRAUMA"), radioGroup10);
                FN.splitVal(FN.rowVal(dt1, "APERASAAN"), radioGroup13);
                FN.splitVal(FN.rowVal(dt1, "INWAWANCARA"), radioGroup1);
                FN.splitVal(FN.rowVal(dt1, "MSPIRITUAL"), radioGroup11);
                FN.splitVal(FN.rowVal(dt1, "KSPIRITUAL"), radioGroup12);
                FN.splitVal(FN.rowVal(dt1, "JOB"), radioGroup14);
                FN.splitVal(FN.rowVal(dt1, "STAT_KAWIN"), radioGroup15);
                FN.splitVal(FN.rowVal(dt1, "JNS_PELAYANAN"), radioGroup17);
                FN.setCheckList(FN.rowVal(dt1, "SKALA_NYERI"), chkSkalaNyeri);
                txScorNyeri.Text = FN.rowVal(dt1, "SCORE_NYERI");
                FN.splitVal(FN.rowVal(dt1, "TINGKAT_NYERI"), rgTingkatNyeri);
                FN.splitVal(FN.rowVal(dt1, "KUALITAS_NYERI"), radioGroup18);
                FN.splitVal(FN.rowVal(dt1, "MENJALAR"), radioGroup19);
                FN.splitVal(FN.rowVal(dt1, "FREKUENSI_NYERI"), radioGroup20);
                FN.splitVal(FN.rowVal(dt1, "PENGARUH_NYERI"), radioGroup28);
                FN.splitVal(FN.rowVal(dt1, "PSEMPOYONGAN"), radioGroup22);
                FN.splitVal(FN.rowVal(dt1, "PPENOPANG"), radioGroup23);
                FN.splitVal(FN.rowVal(dt1, "HRESIKO"), radioGroup27);
                FN.splitVal(FN.rowVal(dt1, "BERITAHU_DOKTER"), radioGroup21);
                FN.splitVal(FN.rowVal(dt1, "SG_KURUS"), radioGroup24);
                FN.splitVal(FN.rowVal(dt1, "SG_TURUNBB"), radioGroup25);
                FN.splitVal(FN.rowVal(dt1, "SG_ASUPAN"), radioGroup26);
                FN.splitVal(FN.rowVal(dt1, "AFS_PENGLIHATAN"), radioGroup30);
                FN.splitVal(FN.rowVal(dt1, "AFS_PENCIUMAN"), radioGroup31);
                FN.splitVal(FN.rowVal(dt1, "AFS_PENDENGARAN"), radioGroup32);
                FN.splitVal(FN.rowVal(dt1, "AFS_KOGNITIF1"), radioGroup33);
                FN.splitVal(FN.rowVal(dt1, "AFS_KOGNITIF2"), radioGroup34);
                FN.splitVal(FN.rowVal(dt1, "AFS_MOTOR_SHRI"), radioGroup35);
                FN.splitVal(FN.rowVal(dt1, "AFS_MOTOR_JALAN"), radioGroup36);
                FN.splitVal(FN.rowVal(dt1, "DPS_HOME_CARE"), radioGroup37);
                FN.splitVal(FN.rowVal(dt1, "DPS_IMPLAN"), radioGroup38);
                FN.splitVal(FN.rowVal(dt1, "DPS_PULANG"), radioGroup39);
                FN.splitVal2(FN.rowVal(dt1, "ALERGI_MKN"), gbMakan, txMakanan);
                FN.splitVal2(FN.rowVal(dt1, "ALERGI_OBAT"), gbObat, txtaobat);

                txt_bb.Text = FN.rowVal(dt1, "BB");
                txt_pbtb.Text = FN.rowVal(dt1, "TB");
                txt_imt.Text = FN.rowVal(dt1, "IMT");
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
            initData();
            LoadData();
            gridControl2.DataSource = null;
            btnAddAnam.Enabled = false;
            btnSaveAnam.Enabled = false;
            btnTunda.Enabled = false;
            btnLanjut.Enabled = false;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string fname = ".wav", p_que = "", p1 = "", p2 = "", p3 = "", p4 = "", policd = "", s_gender = "", s_name = "", urltts = "", teks = "";
            string sql_check5 = "", rm_number ="", sql_cnt="", pasienno="";
            int visit, queue, tmp_visit_no = 0;
            //p_dir = resourcesDirectory;
            //p_dir = "C:\\KLINIK\\";

            if (gridView1.RowCount < 1)
                return;

            p_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            //s_gender = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
            //s_name = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
            policd = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[6]).ToString();
            //pasienno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();

            sql_check5 = sql_check5 + "select TYPE_INS from KLINIK.CS_CALL_LOG where  QUE = '" + p_que + "' AND TRUNC(INS_DATE) = TRUNC(SYSDATE)";

            OleDbConnection oraConnect5 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra5 = new OleDbDataAdapter(sql_check5, oraConnect5);
            DataTable dt5 = new DataTable();
            adOra5.Fill(dt5);
            if (dt5.Rows.Count > 0)
            {
                rm_number = dt5.Rows[0]["TYPE_INS"].ToString(); 
            }

            if(rm_number.ToString().Equals("REG"))
            {
                string sql = @"UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'N', UPD_ANTRIAN =sysdate WHERE QUE = '" + p_que + "' AND TRUNC(INS_DATE) = TRUNC(SYSDATE)";

                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbCommand cm = new OleDbCommand(sql, oraConnect);
                oraConnect.Open();
                cm.ExecuteNonQuery();
                oraConnect.Close();
                cm.Dispose();
            }
            else
            {
                MessageBox.Show("Maaf Pasien sudah di Proses, Tidak Dapat Dipanggil Di Pendaftaran.");
                return;
            }



            //p1 = p_que.Substring(0, 1);
            //p2 = p_que.Substring(1, 1);
            //p3 = p_que.Substring(2, 1);
            //p4 = p_que.Substring(3, 1);

            //if (s_gender == "Perempuan")
            //{
            //    p1 = "Ibu ";
            //}
            //else
            //{
            //    p1 = "Bapak ";
            //}

            //p2 = s_name;

            //teks = p1 + p2 + " silahkan menuju ke konter pendaftaran";

            //loading.ShowWaitForm();
            //try
            //{  
            //    string stpoli = "";
            //    if (policd.ToString().Equals("POL0002"))
            //        stpoli = "PREG";
            //    else
            //        stpoli = "COMM"; 

            //    sql_check5 = sql_check5 + " select  rm_no from cs_patient_info a, cs_patient b where a.PATIENT_NO = b.PATIENT_NO and a.PATIENT_NO = '" + pasienno.ToString() + "' and b.GROUP_PATIENT = '" + stpoli + "' ";

            //    try
            //    {
            //        OleDbConnection oraConnect5 = ConnOra.Create_Connect_Ora();
            //        OleDbDataAdapter adOra5 = new OleDbDataAdapter(sql_check5, oraConnect5);
            //        DataTable dt5 = new DataTable();
            //        adOra5.Fill(dt5);
            //        if (dt5.Rows.Count > 0)
            //        {
            //            rm_number = dt5.Rows[0]["rm_no"].ToString();
            //        }
            //        else
            //        {
            //            rm_number = "";
            //            MessageBox.Show("Anda belum terdaftar untuk Poli ini. Silahkan ke bagian pendaftaran dahulu.");
            //            return;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("ERROR: " + ex.Message);
            //        loading.CloseWaitForm();
            //    }


            //    sql_cnt = " select to_char(sysdate,'yymm') || LPAD(CS_VISIT_SEQ.NEXTVAL, 4, '0') vno from dual ";
            //    OleDbConnection oraConnect4 = ConnOra.Create_Connect_Ora();
            //    OleDbDataAdapter adOra4 = new OleDbDataAdapter(sql_cnt, oraConnect4);
            //    DataTable dt4 = new DataTable();
            //    adOra4.Fill(dt4);
            //    tmp_visit_no = Convert.ToInt32(dt4.Rows[0]["vno"].ToString());




            //    //SoundPlayer player = new SoundPlayer(p_dir + "antrian" + fname);
            //    //SoundPlayer player2 = new SoundPlayer(p_dir + p1 + fname);
            //    //SoundPlayer player3 = new SoundPlayer(p_dir + "_" + p2 + fname);
            //    //SoundPlayer player4 = new SoundPlayer(p_dir + "_" + p3 + fname);
            //    //SoundPlayer player5 = new SoundPlayer(p_dir + "_" + p4 + fname);
            //    //SoundPlayer player6 = new SoundPlayer(p_dir + "IN" + fname);
            //    //player.PlaySync();
            //    ////Thread.Sleep(2000);
            //    //player2.PlaySync();
            //    ////Thread.Sleep(900);
            //    //player3.PlaySync();
            //    ////Thread.Sleep(900);
            //    //player4.PlaySync();
            //    ////Thread.Sleep(900);
            //    //player5.PlaySync();
            //    //Thread.Sleep(900);
            //    //player6.PlaySync();
            //    //Thread.Sleep(2000);

            //    loading.CloseWaitForm();
            //}
            //catch (Exception ex)
            //{
            //    loading.CloseWaitForm();
            //    MessageBox.Show("ERROR: " + ex.Message);
            //}


        }

        public static void PlayMp3FromUrl(string url)
        {
            using (Stream ms = new MemoryStream())
            {
                using (Stream stream = WebRequest.Create(url)
                    .GetResponse().GetResponseStream())
                {
                    byte[] buffer = new byte[32768];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                }

                ms.Position = 0;
                using (WaveStream blockAlignedStream =
                    new BlockAlignReductionStream(
                        WaveFormatConversionStream.CreatePcmStream(
                            new Mp3FileReader(ms))))
                {
                    using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        waveOut.Init(blockAlignedStream);
                        waveOut.Play();
                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
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
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string pur = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);
                string stat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
                if (stat == "CAN" )
                {
                    //e.Appearance.BackColor = Color.FromArgb(40, Color.DodgerBlue);
                    e.Appearance.BackColor = Color.Maroon;
                    e.Appearance.BackColor2 = Color.MediumOrchid;
                    e.Appearance.ForeColor = Color.White;
                    //e.Appearance.Font = new Font("Arial", 9, FontStyle.Bold);
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                    //e.HighPriority = true;
                }

                //if (stat == "Inspection" && pur == "Bidan")
                //{
                //    e.Appearance.BackColor = Color.LightCoral;
                //    //e.Appearance.BackColor2 = Color.White;
                //    e.Appearance.ForeColor = Color.White;
                //    e.Appearance.FontStyleDelta = FontStyle.Bold;
                //    e.HighPriority = true;
                //}
            }
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.Column.Caption == "Pasien")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[7]);
                if (kk == "BPJS")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.ForestGreen);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.ForestGreen);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else
                {
                    e.Appearance.BackColor = Color.OldLace;
                    e.Appearance.ForeColor = Color.Black;
                }
            }

            if (e.Column.Caption == "KK")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);
                if (kk == "Yes")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.OrangeRed);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else
                {
                    e.Appearance.BackColor = Color.OldLace;
                    e.Appearance.ForeColor = Color.Black;
                }
            }

            if (e.Column.Caption == "W.T.")
            {
                string wt = View.GetRowCellDisplayText(e.RowHandle, View.Columns[17]);

                if (wt != "")
                {
                    if (Convert.ToInt64(wt) >= 60)
                    {
                        e.Appearance.BackColor = Color.Red;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (Convert.ToInt64(wt) >= 40 && Convert.ToInt64(wt) < 60)
                    {
                        e.Appearance.BackColor = Color.Orange;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else
                    {
                        //e.Appearance.BackColor = Color.OldLace;
                        //e.Appearance.ForeColor = Color.Black;
                    }
                }

            }

            if (e.Column.Caption == "Status")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
                string pur = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);

                if (kk == "Inspection" && pur == "Dokter")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.BackColor2 = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Inspection" && pur == "Bidan")
                {
                    e.Appearance.BackColor = Color.LightCoral;
                    e.Appearance.BackColor2 = Color.LightCoral;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "First Inspection" && pur == "Dokter")
                {
                    e.Appearance.BackColor = Color.FromArgb(75, Color.DodgerBlue);
                    e.Appearance.BackColor2 = Color.FromArgb(75, Color.DodgerBlue);
                }
                else if (kk == "First Inspection" && pur == "Bidan")
                {
                    e.Appearance.BackColor = Color.FromArgb(75, Color.LightCoral);
                    e.Appearance.BackColor2 = Color.FromArgb(75, Color.LightCoral);
                }
                else if (kk == "Reservation" && pur == "Dokter")
                {
                    e.Appearance.BackColor = Color.FromArgb(50, Color.DodgerBlue);
                    e.Appearance.BackColor2 = Color.FromArgb(50, Color.DodgerBlue);
                }
                else if (kk == "Reservation" && pur == "Bidan")
                {
                    e.Appearance.BackColor = Color.FromArgb(50, Color.LightCoral);
                    e.Appearance.BackColor2 = Color.FromArgb(50, Color.LightCoral);
                }
                else if (kk == "Observation")
                {
                    e.Appearance.BackColor = Color.MediumPurple;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Hold")
                {
                    e.Appearance.BackColor = Color.SlateGray;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Cancel")
                {
                    e.Appearance.BackColor = Color.Maroon;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Preparation")
                {
                    e.Appearance.BackColor = Color.OldLace;
                    e.Appearance.ForeColor = Color.Black;
                }
            }

            //if (e.Column.Caption == "Poli")
            //{
            //    e.Appearance.BackColor = Color.OldLace;
            //    e.Appearance.ForeColor = Color.Black;
            //    //e.Appearance.FontStyleDelta = FontStyle.Bold;
            //}

            //if (e.Column.Caption == "Berobat")
            //{
            //    e.Appearance.BackColor = Color.OldLace;
            //    e.Appearance.ForeColor = Color.Black;
            //    //e.Appearance.FontStyleDelta = FontStyle.Bold;
            //}

            if (e.Column.Caption == "Rencana")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
                //e.Appearance.FontStyleDelta = FontStyle.Bold;
            }

        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.Column.Caption == "Nama" || e.Column.Caption == "ID" || e.Column.Caption == "Action")
            {

            }
            else
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void gridView3a_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.Column.Caption == "Nama" || e.Column.Caption == "Tanggal" || e.Column.Caption == "Antrian" || e.Column.Caption == "Action")
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
            gridView1.Columns[2].OptionsColumn.ReadOnly = false;
            //gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gridView1.Columns[2].OptionsColumn.AllowEdit = true;
            gridView1.AddNewRow();
            subclear();
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns[6], "POL0001");
            view.SetRowCellValue(e.RowHandle, view.Columns[18], "UGD");
            view.SetRowCellValue(e.RowHandle, view.Columns[10], "RSV");
            view.SetRowCellValue(e.RowHandle, view.Columns[9], "DOC");
            view.SetRowCellValue(e.RowHandle, view.Columns[11], "I");
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.Caption == "Nama")
            {
                string p_empid = e.Value.ToString();
                string empid = "", name = "", dept = "", gender = "", age = "", pnik="", que="";
                string sql_emp = " select patient_no, initcap(name) name, null dept, decode(gender,'P','Perempuan','Laki-Laki')  gender, round(((sysdate-birth_date)/30)/12) age, nid NIK from KLINIK.cs_patient_info where 1 = 1 and patient_no = '" + p_empid + "' and STATUS ='A' ";

                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_emp, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    name = dt.Rows[0]["patient_no"].ToString();
                    dept = dt.Rows[0]["dept"].ToString();
                    gender = dt.Rows[0]["gender"].ToString();
                    age = dt.Rows[0]["age"].ToString();
                    pnik = dt.Rows[0]["NIK"].ToString();
                }
                else
                {
                    empid = ""; dept = ""; gender = ""; age = "";
                    view.SetColumnError(gridView1.Columns[1], "Employees Not Found");
                }

                que = gridView1.GetRowCellValue(e.RowHandle, gridView1.Columns[0]).ToString();
                empid = gridView1.GetRowCellValue(e.RowHandle, gridView1.Columns[19]).ToString();
                if (que.ToString().Equals("") && empid.ToString().Equals(""))
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "I");
                else if (!que.ToString().Equals("") && empid.ToString().Equals("0"))
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "I");
                else
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "U");

                view.SetRowCellValue(e.RowHandle, view.Columns[1], name);
                view.SetRowCellValue(e.RowHandle, view.Columns[3], dept);
                view.SetRowCellValue(e.RowHandle, view.Columns[4], gender);
                view.SetRowCellValue(e.RowHandle, view.Columns[5], age);
                view.SetRowCellValue(e.RowHandle, view.Columns[23], pnik);

                //view.SetRowCellValue(e.RowHandle, view.Columns[7], "U");
                view.SetRowCellValue(e.RowHandle, view.Columns[8], "N");
                view.SetRowCellValue(e.RowHandle, view.Columns[10], "RSV");
                

                string tmp_nik = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string tmp_grp = "";
                string tmp_poli = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                string tmp_rm = "", sql = ""; 
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                tmp_grp = "COMM";

                sql = " select rm_no from cs_patient where patient_no = '" + tmp_nik + "' and group_patient = '" + tmp_grp + "' and status = 'A'  ";
                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql, oraConnect2);
                DataTable dt2 = new DataTable();
                adOra2.Fill(dt2);
                if (dt2.Rows.Count > 0)
                {
                    tmp_rm = dt2.Rows[0]["rm_no"].ToString();
                }
                else
                {
                    tmp_rm = "";
                }

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[14], tmp_rm);
                }

                simpleButton2.Enabled = true;
            }
             

            if (e.Column.Caption == "Poli" || e.Column.Caption == "Pasien" || e.Column.Caption == "Status")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                string tmp_poli = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                string tmp_poli2 = view.GetRowCellValue(e.RowHandle, view.Columns[16]).ToString();

                if (tmp_poli != tmp_poli2 && (!visitid.ToString().Equals("0")&&!visitid.ToString().Equals("")))
                //if ((!tmp_poli.ToString().Equals("POL0002") && !tmp_poli.ToString().Equals("POL0003")))
                {
                    if ((!tmp_poli.ToString().Equals("POL0002") && !tmp_poli.ToString().Equals("POL0003")))
                    {
                        MessageBox.Show("Hanya Poli Kebidanan & Poli KB yang dapat di Ganti..!!");
                        //gridView1.SetRowCellValue(e.RowHandle, gridView1.Columns[6], tmp_poli);
                        LoadData();
                        return;
                    }
                }

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "I");
                    simpleButton2.Enabled = true;
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "U");
                    simpleButton2.Enabled = true;
                }
            }

        }

        private void btnAddAnam_Click(object sender, EventArgs e)
        {
            gridView2.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView2.AddNewRow();
            btnAddAnam.Enabled = false;
            btnSaveAnam.Enabled = true;
              
        }

        private void gridView2_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            string tmp_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            string tmp_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();
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



        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.Caption == "Tensi" || e.Column.Caption == "Nadi" || e.Column.Caption == "Suhu" || e.Column.Caption == "BB (Kg)" || e.Column.Caption == "TB (Cm)" || e.Column.Caption == "Alergi" || e.Column.Caption == "Keluhan Utama" || e.Column.Caption == "Riwayat" || e.Column.Caption == "HR (x/m)" || e.Column.Caption == "RR (x/m)")                 
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
         
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string que = "", nik = "", nama = "", poli = "", poli2 ="", pasien = "", gnder ="", workA = "", purpose = "", status = "", action = "", cek = "", remark = "", idvisit = "", age="";
            string sql_check = "", sql_cnt = "", sql_insert = "", sql_update = "", c_que = "", tmp_queue = "", visit_cnt = "", rm = "", teks="", p1 ="", p2="", ppoli ="";
            int queue = 0, visit = 0, tmp_visit_no = 0, ssimpan=0;
            string no_antrian = "", SCode = "";
            cek = "";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                que = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                nik = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                nama = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                gnder = gridView1.GetRowCellValue(i, gridView1.Columns[4]).ToString();
                poli = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                ppoli = gridView1.GetRowCellDisplayText(i, gridView1.Columns[6]).ToString();
                pasien = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();
                workA = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();
                purpose = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                status = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                action = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
                idvisit = gridView1.GetRowCellValue(i, gridView1.Columns[19]).ToString();
                rm = gridView1.GetRowCellValue(i, gridView1.Columns[14]).ToString();
                age = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
                poli2 = gridView1.GetRowCellValue(i, gridView1.Columns[16]).ToString();

                string nme = gridView1.GetRowCellDisplayText(i, gridView1.Columns[2]).ToString();
                if (poli == "POL0007")
                    remark = gridView1.GetRowCellValue(i, gridView1.Columns[18]).ToString();
                else
                    remark = gridView1.GetRowCellValue(i, gridView1.Columns[13]).ToString();

                if (action == "I"  )
                {
                    if (nama == "")
                    {
                        MessageBox.Show("Data pasien tidak ditemukan");
                        return;
                    }
                    else if (purpose == "" || purpose == "REG")
                    {
                        MessageBox.Show("Tujuan Berobat harus diisi");
                        return;
                    }
                    else if (age == "")
                    {
                        MessageBox.Show("Tanggal Lahir Belum di input. Pasien tidak dapat di proses..!!!");
                        return;
                    }
                    else if (poli == "")
                    {
                        MessageBox.Show("Silahkan Tentukan Poli yang Anda Tuju..!!"); return;
                    }
                    else
                    {
                        if (que.ToString().Equals(""))
                        {
                            if (pasien.ToString().Equals("B"))
                                SCode = "008";
                            else if (pasien.ToString().Equals("A"))
                                SCode = "019";
                            else
                                SCode = "020";

                            sql_cnt = "";
                            sql_cnt = " select  KLINIK.CS_GET_ANTRIAN_POLI('POL0000',  decode('" + pasien + "','B','BPJS','U','UMUM','A','ASURANSI') , '" + SCode + "') as que from dual ";
                            OleDbConnection oraConNo = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOraNo = new OleDbDataAdapter(sql_cnt, oraConNo);
                            DataTable dtNo = new DataTable();
                            adOraNo.Fill(dtNo);
                            no_antrian = dtNo.Rows[0]["que"].ToString();


                            if (gnder.ToString().Equals("Perempuan") && Convert.ToInt32(age) > 12 && Convert.ToInt32(age) < 31)
                            {
                                p1 = " Saudari  ";
                            }
                            else if (gnder.ToString().Equals("Perempuan") && Convert.ToInt32(age) > 30)
                            {
                                p1 = " Nyonya  ";
                            }
                            else if (gnder.ToString().Equals("Laki-Laki") && Convert.ToInt32(age) > 12 && Convert.ToInt32(age) < 31)
                            {
                                p1 = " Saudara  ";
                            }
                            else if (gnder.ToString().Equals("Laki-Laki") && Convert.ToInt32(age) > 30)
                            {
                                p1 = " Tuan  ";
                            }

                            if (Convert.ToInt32(age) < 13)
                            {
                                p1 = " Anak  ";
                            }

                            p2 = nme + " ";

                            teks = "Nomor Antrian " + no_antrian + " " + p1 + p2 + " Pasien UGD";

                            sql_insert = "";
                            sql_insert = sql_insert + " insert into cs_call_log (call_id, que, type_ins, stat, param, flag, ins_emp, ins_date, POLI_CD, STYPE) ";
                            sql_insert = sql_insert + " values (cs_call_log_seq.nextval, '" + no_antrian + "','UGD','Perawat','" + teks + "','X','Admin',sysdate, '" + poli + "', '" + pasien + "'  )";

                            ORADB.Execute(ORADB.XE, sql_insert); 
                            remark = "UGD";
                        }
                        else
                        {
                            no_antrian = que;
                        }
                        
                        sql_cnt = " select count(patient_no) cnt from cs_visit where patient_no = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd')= to_char(sysdate,'yyyy-mm-dd') and status not in ('CLS','CAN') and POLI_CD ='" + poli + "' ";
                        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_cnt, oraConnect2);
                        DataTable dt2 = new DataTable();
                        adOra2.Fill(dt2);
                        visit_cnt = dt2.Rows[0]["cnt"].ToString();
                        if (Convert.ToInt32(visit_cnt) > 0)
                        {
                           MessageBox.Show("Pasien sudah terdaftar pada " + ppoli + " di hari ini, silahkan Cancel/Closed status Pasien untuk pendaftaran Ulang."); 
                            return;
                        }
                        else
                        {

                            sql_cnt = " select to_char(sysdate,'yymm') || LPAD(CS_VISIT_SEQ.NEXTVAL, 4, '0') vno from dual ";
                            OleDbConnection oraConnect4 = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOra4 = new OleDbDataAdapter(sql_cnt, oraConnect4);
                            DataTable dt4 = new DataTable();
                            adOra4.Fill(dt4);
                            tmp_visit_no = Convert.ToInt32(dt4.Rows[0]["vno"].ToString());


                            OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                            OleDbCommand command = new OleDbCommand();
                            OleDbTransaction trans = null;

                            command.Connection = oraConnectTrans;
                            oraConnectTrans.Open();

                            visit = Convert.ToInt32(visit_cnt) + 1;

                            //cek = cek + sql_insert;
                            try
                            {
                                trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                                command.Connection = oraConnectTrans;
                                command.Transaction = trans;

                                command.CommandText = " insert into KLINIK.cs_visit (patient_no, visit_date, status, poli_cd, type_patient, work_accident, purpose, visit_remark, visit_cnt, que01, plan, ins_date, ins_emp,ID_VISIT) values ('" + nik + "',sysdate, '" + status + "', '" + poli + "', '" + pasien + "','" + workA + "', '" + purpose + "', '" + remark + "', '" + Convert.ToString(visit) + "', '" + no_antrian + "' , 'TRT01', sysdate, '" + DB.vUserId + "', " + tmp_visit_no + ") ";
                                command.ExecuteNonQuery();
                                 
                                string sql_anamnesa_id = " select cs_anamnesa_seq.nextval cnt from dual";
                                OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
                                OleDbDataAdapter adOra3 = new OleDbDataAdapter(sql_anamnesa_id, oraConnect2);
                                DataTable dt3 = new DataTable();
                                adOra3.Fill(dt3);
                                int anamnesa_id = Convert.ToInt32(dt3.Rows[0]["cnt"].ToString());

                                command.CommandText = " insert into KLINIK.cs_anamnesa (anamnesa_id, rm_no, insp_date, visit_no, ins_date, ins_emp, ID_VISIT) values(" + anamnesa_id + ", '" + rm + "', trunc(sysdate), '" + no_antrian + "', sysdate, '" + DB.vUserId + "', " + tmp_visit_no + ") ";
                                command.ExecuteNonQuery();

                                command.CommandText = @"insert into KLINIK.CS_ANAMNESA_DTL(
		                                                PATIENT_NO,ANAMNESA_ID, C_MSK_RS, SDR_KANDUNG, SDR_TIRI, TGL_BERSAMA,
		                                                SBICARA, SKOMUNIKASI, SEMOSI, RJIWA, RTRAUMA, APERASAAN , INWAWANCARA,
		                                                KSPIRITUAL, MSPIRITUAL, KIBADAH, NYERI_SIFAT, SKALA_NYERI, TINGKAT_NYERI, SCORE_NYERI, KUALITAS_NYERI, MENJALAR,
		                                                FREKUENSI_NYERI , PENGARUH_NYERI, SARAN_NYERI, PSEMPOYONGAN, PPENOPANG , HRESIKO,
		                                                BERITAHU_DOKTER , HSKRINING_RESIKO, HRESIKO_SARAN, SG_KURUS, SG_TURUNBB, SG_ASUPAN,
		                                                SG_HASIL, SG_SARAN, AFS_PENGLIHATAN , AFS_PENCIUMAN, AFS_PENDENGARAN , AFS_KOGNITIF1,
		                                                AFS_KOGNITIF2, AFS_MOTOR_SHRI, AFS_MOTOR_JALAN , DPS_HOME_CARE, DPS_IMPLAN, DPS_PULANG,
		                                                DPS_HASIL, DPS_SARAN, DPS_PENUNJANG, INS_DATE, INS_EMP
	                                                ) values (
	                                                '" + nik + "' , " + anamnesa_id + ",   '" + FN.radioVal(radioGroup16) + "', '" + FN.radioVal(radioGroup3) + "', '" + FN.radioVal(radioGroup2) + "', '" + FN.radioVal(radioGroup4) + @"',
	                                                '" + FN.radioVal(radioGroup5) + "',  '" + FN.radioVal(radioGroup6) + "',   '" + FN.radioVal(radioGroup7) + "', '" + FN.radioVal(radioGroup8) + "', '" + FN.radioVal(radioGroup10) + "','" + FN.radioVal(radioGroup13) + "', '" + FN.radioVal(radioGroup1) + @"',
	                                                '" + FN.radioVal(radioGroup9) + "', '" + FN.radioVal(radioGroup11) + "', '" + FN.radioVal(radioGroup12) + "', '" + FN.radioVal(rgNyeri) + "', '" + FN.chkListOf(chkSkalaNyeri) + "', '" + FN.radioVal(rgTingkatNyeri) + "', '" + txScorNyeri.Text  + "', '" + FN.radioVal(radioGroup18) + "','" + FN.radioVal(radioGroup19) + @"',
	                                                '" + FN.radioVal(radioGroup20) + "', '" + FN.radioVal(radioGroup28) + "', '" + txt_srnnyeri.Text + "', '" + FN.radioVal(radioGroup22) + "', '" + FN.radioVal(radioGroup23) + "',  '" + FN.radioVal(radioGroup27) + @"',
	                                                '" + FN.radioVal(radioGroup21) + "', '" + txt_hsl_s.Text + "', '" + txt_saran.Text + "', '" + FN.radioVal(radioGroup24) + "',  '" + FN.radioVal(radioGroup25) + "',  '" + FN.radioVal(radioGroup26) + @"',
	                                                '" + txt_h_sk.Text + "', '" + txt_ssaran.Text + "', '" + FN.radioVal(radioGroup30) + "', '" + FN.radioVal(radioGroup31) + "', '" + FN.radioVal(radioGroup32) + "', '" + FN.radioVal(radioGroup33) + @"',
	                                                '" + FN.radioVal(radioGroup34) + "', '" + FN.radioVal(radioGroup35) + "', '" + FN.radioVal(radioGroup36) + "', '" + FN.radioVal(radioGroup37) + "',  '" + FN.radioVal(radioGroup38) + "',  '" + FN.radioVal(radioGroup39) + @"',
	                                                '" + txt_h_skrining.Text + "', '" + txt_saran4.Text + "', '" + txt_p_penunjang.Text + "', sysdate,'" + DB.vUserId + "' ) ";
                                command.ExecuteNonQuery();
                                //}

                                if(!remark.ToString ().Equals("UGD"))
                                {
                                    if (gnder.ToString().Equals("Perempuan") && Convert.ToInt32(age) > 12 && Convert.ToInt32(age) < 31)
                                    {
                                        p1 = " Saudari  ";
                                    }
                                    else if (gnder.ToString().Equals("Perempuan") && Convert.ToInt32(age) > 30)
                                    {
                                        p1 = " Nyonya  ";
                                    }
                                    else if (gnder.ToString().Equals("Laki-Laki") && Convert.ToInt32(age) > 12 && Convert.ToInt32(age) < 31)
                                    {
                                        p1 = " Saudara  ";
                                    }
                                    else if (gnder.ToString().Equals("Laki-Laki") && Convert.ToInt32(age) > 30)
                                    {
                                        p1 = " Tuan  ";
                                    }

                                    if (Convert.ToInt32(age) < 13)
                                    {
                                        p1 = " Anak  ";
                                    }

                                    p2 = nme + " ";

                                    teks = "Nomor Antrian " + que + " " + p1 + p2 + " Silahkan Menuju Ke Bagian Perawat";

                                    command.CommandText = @"UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'W', type_ins ='PWT', stat ='Perawat', param = '" + teks + "', UPD_ANTRIAN =sysdate WHERE QUE = '" + que + "' AND TRUNC(INS_DATE) = TRUNC(SYSDATE)";
                                    command.ExecuteNonQuery();
                                }
                                 

                                trans.Commit();
                                ssimpan = 1;
                                //MessageBox.Show(sql_insert);
                                //MessageBox.Show("Query Exec : " + sql);
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
                }
                else if (action == "U")
                {
                    if(idvisit.ToString().Equals("0"))
                    {
                        sql_update = @"UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'X', param = 'BATAL', UPD_ANTRIAN =sysdate WHERE QUE = '" + que + "' AND TRUNC(INS_DATE) = TRUNC(SYSDATE)";
                        ORADB.Execute(ORADB.XE, sql_update);
                        LoadData();
                        return;
                    }

                    if (nama == "")
                    {
                        MessageBox.Show("Data pasien tidak ditemukan");
                        return;
                    }
                    else if (purpose == "" || purpose == "REG")
                    {
                        MessageBox.Show("Tujuan Berobat harus diisi");
                        return;
                    }
                    else if (poli == "")
                    {
                        MessageBox.Show("Silahkan Tentukan Poli yang Anda Tuju..!!"); return;
                    }
                     

                    string sql_head = "", typehead ="" ;

                    sql_head = " select INSU_FLAG from KLINIK.cs_treatment_head where patient_no = '" + nik + "' and ID_VISIT = '" + idvisit + "' ";

                    OleDbConnection sqlConHead = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSqlHead = new OleDbDataAdapter(sql_head, sqlConHead);
                    DataTable dtHead = new DataTable();
                    adSqlHead.Fill(dtHead);

                    if (dtHead.Rows.Count > 0)
                    {
                        typehead = dtHead.Rows[0]["INSU_FLAG"].ToString();
                        if(typehead != pasien)
                        {
                            MessageBox.Show("Pasien sudah dilakukan proses Pelayanan, Perubahan Type tidak dapat di proses."); 
                            return;
                        }
                    }
                    
                    if(poli != poli2)
                    {
                        if (poli.ToString().Equals("POL0002"))
                            SCode = "006";
                        else
                            SCode = "009";

                        sql_cnt = ""; 
                        sql_cnt = " select  KLINIK.CS_GET_ANTRIAN_POLI('" + poli + "',  decode('" + pasien + "','B','BPJS','U','UMUM','A','ASURANSI') , '" + SCode + "') as que from dual ";
                        OleDbConnection oraConNo = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOraNo = new OleDbDataAdapter(sql_cnt, oraConNo);
                        DataTable dtNo = new DataTable();
                        adOraNo.Fill(dtNo);
                        no_antrian = dtNo.Rows[0]["que"].ToString();

                         
                        if (gnder.ToString().Equals("Perempuan") && Convert.ToInt32(age) > 12 && Convert.ToInt32(age) < 31)
                        {
                            p1 = " Saudari  ";
                        }
                        else if (gnder.ToString().Equals("Perempuan") && Convert.ToInt32(age) > 30)
                        {
                            p1 = " Nyonya  ";
                        }
                        else if (gnder.ToString().Equals("Laki-Laki") && Convert.ToInt32(age) > 12 && Convert.ToInt32(age) < 31)
                        {
                            p1 = " Saudara  ";
                        }
                        else if (gnder.ToString().Equals("Laki-Laki") && Convert.ToInt32(age) > 30)
                        {
                            p1 = " Tuan  ";
                        }

                        if (Convert.ToInt32(age) < 13)
                        {
                            p1 = " Anak  ";
                        }

                        p2 = nme + " ";

                        teks = "Nomor Antrian " + no_antrian + " " + p1 + p2 + " Silahkan Menuju Ke Bagian Perawat";
                         
                        sql_insert = "";
                        sql_insert = sql_insert + " insert into cs_call_log (call_id, que, type_ins, stat, param, flag, ins_emp, ins_date, POLI_CD, STYPE) ";
                        sql_insert = sql_insert + " values (cs_call_log_seq.nextval, '" + no_antrian + "','PWT','Perawat','" + teks + "','W','Admin',sysdate, '" + poli + "', '" + pasien + "'  )";

                        ORADB.Execute(ORADB.XE, sql_insert);

                        sql_update = @"UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'X', param = 'GANTI POLI', UPD_ANTRIAN =sysdate WHERE QUE = '" + que + "' AND TRUNC(INS_DATE) = TRUNC(SYSDATE)";
                        ORADB.Execute(ORADB.XE, sql_update);

                    }

                    sql_update = ""; 
                    sql_update = sql_update + Environment.NewLine + "update cs_visit ";
                    sql_update = sql_update + Environment.NewLine + "set type_patient = '" + pasien + "', patient_no  = '" + nik + "', ";
                    sql_update = sql_update + Environment.NewLine + "work_accident = '" + workA + "', purpose = '" + purpose + "', visit_remark = '" + remark + "', status = '" + status + "', ";
                    if (poli != poli2)
                    {
                        sql_update = sql_update + Environment.NewLine + "   poli_cd = '" + poli + "', QUE01 = '" + no_antrian + "',   ";
                    }
                    if (status == "INS")
                    {
                        sql_update = sql_update + Environment.NewLine + " time_reservation = sysdate, ";
                    }
                    sql_update = sql_update + Environment.NewLine + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                    sql_update = sql_update + Environment.NewLine + " where   ID_VISIT = '" + idvisit + "'";

                    cek = cek + sql_update;

                    try
                    {
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                        oraConnect.Open();
                        cm.ExecuteNonQuery();
                        oraConnect.Close();
                        cm.Dispose();
                        ssimpan = 2;
                        //MessageBox.Show("Query Exec : " + sql_update);

                        //MessageBox.Show("Data Berhasil diupdate");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }
            }
            if(ssimpan ==1)
                MessageBox.Show("Data Berhasil disimpan.");
            else if(ssimpan==2)
                MessageBox.Show("Data Berhasil diupdate.");

            //richTextBox1.Text = cek;
            //MessageBox.Show(action);
            LoadData();
        }


        private void btnSaveAnam_Click(object sender, EventArgs e)
        {
            string date = "", que = "", tensi = "", nadi = "", suhu = "", alergi = "", keluhan = "", action = "", rm_no = "", nik = "", infok = "", bb = "", tb = "";
            string chol = "", bsugar = "", uacid = "", r_now = "", r_then = "", r_fam = "", anam_physical = "", anam_other = "", vhr = "", vrr = "";
            string sql_update2 = "", sql_cnt = "", stat_rsv = "", sql_update = "", anam_cnt = "";

            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                date = Convert.ToDateTime(gridView2.GetRowCellValue(i, gridView2.Columns[0]).ToString()).ToString("dd/MM/yyyy");
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
                bb = gridView2.GetRowCellValue(i, gridView2.Columns[11]).ToString();
                tb = gridView2.GetRowCellValue(i, gridView2.Columns[12]).ToString();
                chol = gridView2.GetRowCellValue(i, gridView2.Columns[13]).ToString();
                bsugar = gridView2.GetRowCellValue(i, gridView2.Columns[14]).ToString();
                uacid = gridView2.GetRowCellValue(i, gridView2.Columns[15]).ToString();
                vhr = gridView2.GetRowCellValue(i, gridView2.Columns[16]).ToString();
                vrr = gridView2.GetRowCellValue(i, gridView2.Columns[17]).ToString();
                r_now = gridView2.GetRowCellValue(i, gridView2.Columns[18]).ToString();
                r_then = gridView2.GetRowCellValue(i, gridView2.Columns[19]).ToString();
                r_fam = gridView2.GetRowCellValue(i, gridView2.Columns[20]).ToString();
                anam_physical = gridView2.GetRowCellValue(i, gridView2.Columns[21]).ToString();
                anam_other = gridView2.GetRowCellValue(i, gridView2.Columns[22]).ToString();
                stat_rsv = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();

                if (tensi == "")
                {
                    MessageBox.Show("Tensi harus diisi");
                }
                else if (nadi == "")
                {
                    MessageBox.Show("Nadi harus diisi");
                }
                else if (bb == "")
                {
                    MessageBox.Show("BB harus diisi");
                }
                else if (tb == "")
                {
                    MessageBox.Show("TB harus diisi");
                }
                else if (keluhan == "")
                {
                    MessageBox.Show("Keluhan harus diisi");
                }
                else
                {
                    if (action == "I")
                    {
                        sql_cnt = " select count(0) cnt from cs_anamnesa where trunc(insp_date) = trunc(to_date('" + today + "','dd/MM/yyyy')) and visit_no = '" + que + "' and rm_no = '" + rm_no + "' ";
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


                            //sql_insert = " insert into cs_anamnesa (anamnesa_id, rm_no, insp_date, blood_press, pulse, temperature, allergy, anamnesa, visit_no, ins_date, ins_emp) values (cs_anamnesa_seq.nextval, '" + rm_no + "', to_date('" + date + "','yyyy-mm-dd'), '" + tensi + "', '" + nadi + "','" + suhu + "', '" + alergi + "', '" + keluhan + "', '" + que + "', sysdate, '" + DB.vUserId + "') ";

                            try
                            {
                                //OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
                                //OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect3);
                                //oraConnect3.Open();
                                //cm.ExecuteNonQuery();
                                //oraConnect3.Close();
                                //cm.Dispose(); 
                                chol = gridView2.GetRowCellValue(i, gridView2.Columns[13]).ToString();
                                bsugar = gridView2.GetRowCellValue(i, gridView2.Columns[14]).ToString();
                                uacid = gridView2.GetRowCellValue(i, gridView2.Columns[15]).ToString();
                                vhr = gridView2.GetRowCellValue(i, gridView2.Columns[16]).ToString();
                                vrr = gridView2.GetRowCellValue(i, gridView2.Columns[17]).ToString();
                                r_now = gridView2.GetRowCellValue(i, gridView2.Columns[18]).ToString();
                                r_then = gridView2.GetRowCellValue(i, gridView2.Columns[19]).ToString();
                                r_fam = gridView2.GetRowCellValue(i, gridView2.Columns[20]).ToString();
                                anam_physical = gridView2.GetRowCellValue(i, gridView2.Columns[21]).ToString();
                                anam_other = gridView2.GetRowCellValue(i, gridView2.Columns[22]).ToString();

                                trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                                command.Connection = oraConnectTrans;
                                command.Transaction = trans;

                                string SQL = "";
                                SQL = SQL + Environment.NewLine + "insert into cs_anamnesa ";
                                SQL = SQL + Environment.NewLine + "( ";
                                SQL = SQL + Environment.NewLine + "anamnesa_id, rm_no, insp_date, blood_press, pulse, temperature, allergy, anamnesa, visit_no, info_k, bb, tb, ";
                                SQL = SQL + Environment.NewLine + "cholesterol, blood_sugar, uric_acid, disease_now, disease_then, disease_family, anamnesa_physical, anamnesa_other, VITALHR,VITALRR,";
                                SQL = SQL + Environment.NewLine + "ins_date, ins_emp ";
                                SQL = SQL + Environment.NewLine + ") ";
                                SQL = SQL + Environment.NewLine + "values  ";
                                SQL = SQL + Environment.NewLine + "( ";
                                SQL = SQL + Environment.NewLine + "cs_anamnesa_seq.nextval, '" + rm_no + "', to_date('" + date + "', 'yyyy-mm-dd'), '" + tensi + "', '" + nadi + "', '" + suhu;
                                SQL = SQL + Environment.NewLine + "', '" + alergi + "', '" + keluhan + "', '" + que + "', '" + infok + "','" + bb + "','" + tb;
                                SQL = SQL + Environment.NewLine + "', '" + chol + "', '" + bsugar + "', '" + uacid + "', '" + r_now + "','" + r_then + "','" + r_fam;
                                SQL = SQL + Environment.NewLine + "', '" + anam_physical + "', '" + anam_other + "', '" + tensi + "', '" + vrr;
                                SQL = SQL + Environment.NewLine + "', sysdate, '" + DB.vUserId + "'  ";
                                SQL = SQL + Environment.NewLine + ") ";

                                command.CommandText = SQL;
                                command.ExecuteNonQuery();

                                command.CommandText = " update cs_visit set status = 'NUR', time_reservation=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + nik + "' and trunc(visit_date) = trunc(to_date('" + date + "','dd/MM/yyyy')) and que01 = '" + que + "' ";
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

                        sql_update = sql_update + " update cs_anamnesa" +
                                     " set blood_press = '" + tensi + "', pulse = '" + nadi + "', bb = '" + bb + "', tb = '" + tb + "', " +
                                     " temperature = '" + suhu + "', allergy = '" + alergi + "', anamnesa = '" + keluhan + "', info_k = '" + infok + "', VITALHR = '" + tensi + "',VITALRR = '" + vrr + "', " +
                                     " cholesterol = '" + chol + "', blood_sugar = '" + bsugar + "', uric_acid = '" + uacid + "', disease_now = '" + r_now + "',  " +
                                     " disease_then = '" + r_then + "', disease_family = '" + r_fam + "', anamnesa_physical = '" + anam_physical + "', anamnesa_other = '" + anam_other + "',  ";
                        sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                        sql_update = sql_update + " where  ANAMNESA_ID = " + v_ptnumber + "";

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
                        //PRE and RSV
                        if (stat_rsv == "PRE" || stat_rsv == "RSV")
                        {
                            sql_update2 = "";

                            sql_update2 = " update cs_visit set status = 'NUR', time_reservation=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + s_nik + "'  and trunc(visit_date) = trunc(to_date('" + date.Substring(0,10) + "','dd/MM/yyyy')) and que01 = '" + que + "' ";

                            try
                            {
                                OleDbConnection oraConnectb = ConnOra.Create_Connect_Ora();
                                OleDbCommand cmb = new OleDbCommand(sql_update2, oraConnectb);
                                oraConnectb.Open();
                                cmb.ExecuteNonQuery();
                                oraConnectb.Close();
                                cmb.Dispose();

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

            }
            //LoadData();
            datapasienisi(v_ptnumber);
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (!txt_rekammds.Text.ToString().Equals("-") || !txt_rekammds.Text.ToString().Equals(""))
            {
                //command.CommandText = @"insert into KLINIK.CS_ANAMNESA_DTL(
                //                                      PATIENT_NO,ANAMNESA_ID, C_MSK_RS, SDR_KANDUNG, SDR_TIRI, TGL_BERSAMA,
                //                                      SBICARA, SKOMUNIKASI, SEMOSI, RJIWA, RTRAUMA, APERASAAN , INWAWANCARA,
                //                                      KSPIRITUAL, MSPIRITUAL, KIBADAH, NYERI_SIFAT, SCORE_NYERI, KUALITAS_NYERI, MENJALAR,
                //                                      FREKUENSI_NYERI , PENGARUH_NYERI, SARAN_NYERI, PSEMPOYONGAN, PPENOPANG , HRESIKO,
                //                                      BERITAHU_DOKTER , HSKRINING_RESIKO, HRESIKO_SARAN, SG_KURUS, SG_TURUNBB, SG_ASUPAN,
                //                                      SG_HASIL, SG_SARAN, AFS_PENGLIHATAN , AFS_PENCIUMAN, AFS_PENDENGARAN , AFS_KOGNITIF1,
                //                                      AFS_KOGNITIF2, AFS_MOTOR_SHRI, AFS_MOTOR_JALAN , DPS_HOME_CARE, DPS_IMPLAN, DPS_PULANG,
                //                                      DPS_HASIL, DPS_SARAN, DPS_PENUNJANG, INS_DATE, INS_EMP
                //                                     ) values (
                //                                     '" + nik + "' , " + anamnesa_id + ",   '" + FN.radioVal(radioGroup16) + "', '" + FN.radioVal(radioGroup3) + "', '" + FN.radioVal(radioGroup2) + "', '" + FN.radioVal(radioGroup4) + @"',
                //                                     '" + FN.radioVal(radioGroup5) + "',  '" + FN.radioVal(radioGroup6) + "',   '" + FN.radioVal(radioGroup7) + "', '" + FN.radioVal(radioGroup8) + "', '" + FN.radioVal(radioGroup10) + "','" + FN.radioVal(radioGroup13) + "', '" + FN.radioVal(radioGroup1) + @"',
                //                                     '" + FN.radioVal(radioGroup9) + "', '" + FN.radioVal(radioGroup11) + "', '" + FN.radioVal(radioGroup12) + "', '" + FN.radioVal(radioGroup29) + "', '" + txt_score.Text + "', '" + FN.radioVal(radioGroup18) + "','" + FN.radioVal(radioGroup19) + @"',
                //                                     '" + FN.radioVal(radioGroup20) + "', '" + FN.radioVal(radioGroup28) + "', '" + txt_srnnyeri.Text + "', '" + FN.radioVal(radioGroup22) + "', '" + FN.radioVal(radioGroup23) + "',  '" + FN.radioVal(radioGroup27) + @"',
                //                                     '" + FN.radioVal(radioGroup21) + "', '" + txt_hsl_s.Text + "', '" + txt_saran.Text + "', '" + FN.radioVal(radioGroup24) + "',  '" + FN.radioVal(radioGroup25) + "',  '" + FN.radioVal(radioGroup26) + @"',
                //                                     '" + txt_h_sk.Text + "', '" + txt_ssaran.Text + "', '" + FN.radioVal(radioGroup30) + "', '" + FN.radioVal(radioGroup31) + "', '" + FN.radioVal(radioGroup32) + "', '" + FN.radioVal(radioGroup33) + @"',
                //                                     '" + FN.radioVal(radioGroup34) + "', '" + FN.radioVal(radioGroup35) + "', '" + FN.radioVal(radioGroup36) + "', '" + FN.radioVal(radioGroup37) + "',  '" + FN.radioVal(radioGroup38) + "',  '" + FN.radioVal(radioGroup39) + @"',
                //                                     '" + txt_h_skrining.Text + "', '" + txt_saran4.Text + "', '" + txt_p_penunjang.Text + "', sysdate,'1' ) ";
                try
                {
                    sql_all = "";
                    sql_all = sql_all + " update KLINIK.CS_ANAMNESA_DTL " +
                                    " set C_MSK_RS = '" + FN.radioVal(radioGroup16) + "', SDR_KANDUNG = '" + FN.radioVal(radioGroup3) + "', SDR_TIRI ='" + FN.radioVal(radioGroup2) + "', TGL_BERSAMA = '" + FN.joinVal(radioGroup4, textBox3)   + "',  " +
                                    "     SBICARA = '" + FN.radioVal(radioGroup5) + "',  SKOMUNIKASI = '" + FN.radioVal(radioGroup6) + "',  SEMOSI = '" + FN.radioVal(radioGroup7) + "', RJIWA = '" + FN.radioVal(radioGroup8) + "', RTRAUMA = '" + FN.radioVal(radioGroup10) + "', " +
                                    "     APERASAAN = '" + FN.radioVal(radioGroup13) + "', INWAWANCARA = '" + FN.radioVal(radioGroup1) + "', " +
                                    "     KSPIRITUAL = '" + FN.radioVal(radioGroup9) + "', MSPIRITUAL = '" + FN.radioVal(radioGroup11) + "', KIBADAH = '" + FN.radioVal(radioGroup12) + "' ";
                    sql_all = sql_all + " where ANAMNESA_ID = " + v_ptnumber + " ";

                    ORADB.Execute(ORADB.XE, sql_all); 

                    sql_all = " ";
                    sql_all = sql_all + " update KLINIK.CS_PATIENT_INFO " +
                                        " set JOB  = '" + FN.radioVal(radioGroup14) + "' , STAT_KAWIN	  ='" + FN.radioVal(radioGroup15) + "'";
                    sql_all = sql_all + " where PATIENT_NO = '" + s_nik + "' ";

                    ORADB.Execute(ORADB.XE, sql_all); 

                    sql_all = "";
                    sql_all = sql_all + " update KLINIK.CS_ANAMNESA " +
                                        " set MASUK_RS = '" + FN.radioVal(radioGroup16) + "', JNS_PELAYANAN  = '" + FN.radioVal(radioGroup17) + "', VITALTERATUR  = '" + FN.radioVal(radioGroup40) + "' ";
                    sql_all = sql_all + " where ANAMNESA_ID = " + v_ptnumber + " ";

                    ORADB.Execute(ORADB.XE, sql_all); 

                    MessageBox.Show("Data Berhasil di Update");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }

            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (!txt_rekammds.Text.ToString().Equals("-") || !txt_rekammds.Text.ToString().Equals(""))
            {
                //command.CommandText = @"insert into KLINIK.CS_ANAMNESA_DTL(
                //                                      PATIENT_NO,ANAMNESA_ID, C_MSK_RS, SDR_KANDUNG, SDR_TIRI, TGL_BERSAMA,
                //                                      SBICARA, SKOMUNIKASI, SEMOSI, RJIWA, RTRAUMA, APERASAAN , INWAWANCARA,
                //                                      KSPIRITUAL, MSPIRITUAL, KIBADAH, NYERI_SIFAT, SCORE_NYERI, KUALITAS_NYERI, MENJALAR,
                //                                      FREKUENSI_NYERI , PENGARUH_NYERI, SARAN_NYERI, PSEMPOYONGAN, PPENOPANG , HRESIKO,
                //                                      BERITAHU_DOKTER , HSKRINING_RESIKO, HRESIKO_SARAN, SG_KURUS, SG_TURUNBB, SG_ASUPAN,
                //                                      SG_HASIL, SG_SARAN, AFS_PENGLIHATAN , AFS_PENCIUMAN, AFS_PENDENGARAN , AFS_KOGNITIF1,
                //                                      AFS_KOGNITIF2, AFS_MOTOR_SHRI, AFS_MOTOR_JALAN , DPS_HOME_CARE, DPS_IMPLAN, DPS_PULANG,
                //                                      DPS_HASIL, DPS_SARAN, DPS_PENUNJANG, INS_DATE, INS_EMP
                //                                     ) values (
                //                                     '" + nik + "' , " + anamnesa_id + ",   '" + FN.radioVal(radioGroup16) + "', '" + FN.radioVal(radioGroup3) + "', '" + FN.radioVal(radioGroup2) + "', '" + FN.radioVal(radioGroup4) + @"',
                //                                     '" + FN.radioVal(radioGroup5) + "',  '" + FN.radioVal(radioGroup6) + "',   '" + FN.radioVal(radioGroup7) + "', '" + FN.radioVal(radioGroup8) + "', '" + FN.radioVal(radioGroup10) + "','" + FN.radioVal(radioGroup13) + "', '" + FN.radioVal(radioGroup1) + @"',
                //                                     '" + FN.radioVal(radioGroup9) + "', '" + FN.radioVal(radioGroup11) + "', '" + FN.radioVal(radioGroup12) + "', '" + FN.radioVal(radioGroup29) + "', '" + txt_score.Text + "', '" + FN.radioVal(radioGroup18) + "','" + FN.radioVal(radioGroup19) + @"',
                //                                     '" + FN.radioVal(radioGroup20) + "', '" + FN.radioVal(radioGroup28) + "', '" + txt_srnnyeri.Text + "', '" + FN.radioVal(radioGroup22) + "', '" + FN.radioVal(radioGroup23) + "',  '" + FN.radioVal(radioGroup27) + @"',
                //                                     '" + FN.radioVal(radioGroup21) + "', '" + txt_hsl_s.Text + "', '" + txt_saran.Text + "', '" + FN.radioVal(radioGroup24) + "',  '" + FN.radioVal(radioGroup25) + "',  '" + FN.radioVal(radioGroup26) + @"',
                //                                     '" + txt_h_sk.Text + "', '" + txt_ssaran.Text + "', '" + FN.radioVal(radioGroup30) + "', '" + FN.radioVal(radioGroup31) + "', '" + FN.radioVal(radioGroup32) + "', '" + FN.radioVal(radioGroup33) + @"',
                //                                     '" + FN.radioVal(radioGroup34) + "', '" + FN.radioVal(radioGroup35) + "', '" + FN.radioVal(radioGroup36) + "', '" + FN.radioVal(radioGroup37) + "',  '" + FN.radioVal(radioGroup38) + "',  '" + FN.radioVal(radioGroup39) + @"',
                //                                     '" + txt_h_skrining.Text + "', '" + txt_saran4.Text + "', '" + txt_p_penunjang.Text + "', sysdate,'1' ) ";
 
                try
                {
                    sql_all = "";
                    sql_all = sql_all + " update KLINIK.CS_ANAMNESA_DTL " +
                                    " set NYERI_SIFAT = '" + FN.radioVal(rgNyeri) + "', SKALA_NYERI = '" + FN.chkListOf(chkSkalaNyeri) + "', TINGKAT_NYERI ='" + FN.radioVal(rgTingkatNyeri) + "', SCORE_NYERI = '" + txScorNyeri.Text + "',  " +
                                    "     KUALITAS_NYERI = '" + FN.radioVal(radioGroup18) + "', MENJALAR = '" + FN.radioVal(radioGroup19) + "', FREKUENSI_NYERI ='" + FN.radioVal(radioGroup20) + "', PENGARUH_NYERI = '" + FN.radioVal(radioGroup28) + "',  " +
                                    "     PSEMPOYONGAN = '" + FN.radioVal(radioGroup22) + "',  PPENOPANG = '" + FN.radioVal(radioGroup23) + "',  HRESIKO = '" + FN.radioVal(radioGroup27) + "', BERITAHU_DOKTER = '" + FN.radioVal(radioGroup21) + "',  " +
                                    "     SG_KURUS = '" + FN.radioVal(radioGroup24) + "', SG_TURUNBB = '" + FN.radioVal(radioGroup25) + "',  SG_ASUPAN = '" + FN.radioVal(radioGroup26) + "', " +
                                    "     SG_HASIL = '" + txt_h_sk.Text  + "', SG_SARAN = '" + txt_ssaran.Text + "' ,ALERGI_MKN = '" + FN.getVal(gbMakan) + "' , ALERGI_OBAT = '" + FN.getVal(gbObat) + "' ";
                    sql_all = sql_all + " where ANAMNESA_ID = " + v_ptnumber + " ";

                    ORADB.Execute(ORADB.XE, sql_all);

                    //sql_all = " ";
                    //sql_all = sql_all + " update KLINIK.CS_PATIENT_INFO " +
                    //                    " set JOB  = '" + FN.radioVal(radioGroup14) + "' , STAT_KAWIN	  ='" + FN.radioVal(radioGroup15) + "'";
                    //sql_all = sql_all + " where PATIENT_NO = '" + s_nik + "' ";

                    //ORADB.Execute(ORADB.XE, sql_all);

                    //sql_all = "";
                    //sql_all = sql_all + " update KLINIK.CS_ANAMNESA " +
                    //                    " set MASUK_RS = '" + FN.radioVal(radioGroup16) + "', JNS_PELAYANAN  = '" + FN.radioVal(radioGroup17) + "' ";
                    //sql_all = sql_all + " where ANAMNESA_ID = " + v_ptnumber + " ";

                    //ORADB.Execute(ORADB.XE, sql_all);

                    MessageBox.Show("Data Berhasil di Update");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }

            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            if (!txt_rekammds.Text.ToString().Equals("-") || !txt_rekammds.Text.ToString().Equals("") || !v_ptnumber.ToString().Equals(""))
            { 
                sql_all = " ";
                sql_all = sql_all + " update KLINIK.CS_ANAMNESA_DTL " +
                                " set AFS_PENGLIHATAN = '" + FN.radioVal(radioGroup30) + "', AFS_PENCIUMAN = '" + FN.radioVal(radioGroup31) + "', AFS_PENDENGARAN ='" + FN.radioVal(radioGroup32) + "', AFS_KOGNITIF1 = '" + FN.radioVal(radioGroup33) + "',  " +
                                "     AFS_KOGNITIF2 = '" + FN.radioVal(radioGroup34) + "',  AFS_MOTOR_SHRI = '" + FN.radioVal(radioGroup35) + "',  AFS_MOTOR_JALAN = '" + FN.radioVal(radioGroup36) + "', DPS_HOME_CARE = '" + FN.radioVal(radioGroup37) + "',  " +
                                "     DPS_IMPLAN = '" + FN.radioVal(radioGroup38) + "', DPS_PULANG = '" + FN.radioVal(radioGroup39) + "',  DPS_HASIL = '" + txt_h_skrining.Text + "', " +
                                "     DPS_SARAN = '" + txt_saran4.Text + "', DPS_PENUNJANG = '" + txt_p_penunjang.Text + "'  ";
                sql_all = sql_all + " where ANAMNESA_ID = " + v_ptnumber + " ";

                try
                { 
                    ORADB.Execute(ORADB.XE, sql_all);
                    
                    MessageBox.Show("Data Berhasil di Update");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }

            }
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_cnt = "";
            string rm_no = "", nik = "", grp = "", poli = "", cd1 = "", cd2 = "", cd3 = "", rm_cnt = "";

            Thread.Sleep(1000);

            nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            poli = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[6]).ToString();

            if (poli == "POL0002")
            {
                grp = "PREG";
            }
            else if (poli == "POL0003")
            {
                grp = "FAMP";
            }
            else
            {
                grp = "COMM";
            }

            cd1 = grp.Substring(0, 1);
            cd2 = DateTime.Now.ToString("yyMMdd");
            cd3 = nik.Substring(1);

            rm_no = cd1 + cd2 + cd3;

            sql_cnt = " select count(0) cnt from cs_patient where rm_no = '" + rm_no + "' and group_patient = '" + grp + "' and status = 'A' ";
            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_cnt, oraConnect2);
            DataTable dt2 = new DataTable();
            adOra2.Fill(dt2);
            rm_cnt = dt2.Rows[0]["cnt"].ToString();
            if (Convert.ToInt32(rm_cnt) > 0)
            {

            }
            else
            {
                sql_insert = " insert into cs_patient (rm_no, patient_no, group_patient, status, ins_date, ins_emp) values ('" + rm_no + "', '" + nik + "', '" + grp + "', 'A', sysdate, '" + DB.vUserId + "') ";
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
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14], rm_no  );
                    //LoadData();
                    btnCreate.Enabled = false;
                    MessageBox.Show("Data Medical Number Berhasil dibuat.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }

        private void btnLanjut_Click(object sender, EventArgs e)
        {
            string p_nik = "", p_que = "", p_status = "", sql_update = "";

            p_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            p_nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();

            string sql_tmp_status = "";

            sql_tmp_status = " select tmp_status from cs_visit where patient_no = '" + p_nik + "' and trunc(visit_date) = trunc(to_date('" + today + "','dd/MM/yyyy')) and que01 = '" + p_que + "' ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_tmp_status, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                p_status = dt.Rows[0]["tmp_status"].ToString();

                sql_update = "";

                sql_update = sql_update + " update cs_visit " +
                                          " set status = '" + p_status + "', end_hold = sysdate, ";
                sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                sql_update = sql_update + " where que01 = '" + p_que + "' and patient_no = '" + p_nik + "' and trunc(visit_date) = trunc(to_date('" + today + "','dd/MM/yyyy'))  ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_update);

                    MessageBox.Show("Data Berhasil dilanjut");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
                LoadData();
            }
        }

        private void btnTunda_Click(object sender, EventArgs e)
        {
            string p_nik = "", p_que = "", p_status = "", sql_update = "";

            p_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            p_nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            p_status = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();

            sql_update = "";

            sql_update = sql_update + " update cs_visit " +
                                      " set status = 'HOL', tmp_status = '" + p_status + "', start_hold = sysdate, ";
            sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
            sql_update = sql_update + " where que01 = '" + p_que + "' and patient_no = '" + p_nik + "' and trunc(visit_date) = trunc(to_date('" + today + "','dd/MM/yyyy')) ";

            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                oraConnect.Open();
                cm.ExecuteNonQuery();
                oraConnect.Close();
                cm.Dispose();

                //MessageBox.Show("Query Exec : " + sql_update);

                MessageBox.Show("Data Berhasil ditunda");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
            LoadData();
        }

        private void btnNotif_Click(object sender, EventArgs e)
        {
            if (obsNotif == null || obsNotif.Text == "")
            {

                string SQL = "";
                SQL = SQL + Environment.NewLine + "select obs_id ";
                SQL = SQL + Environment.NewLine + "from ( ";
                SQL = SQL + Environment.NewLine + "select b.rm_no, b.insp_date, b.visit_no, b.obs_id, d.room_name, ";
                SQL = SQL + Environment.NewLine + "(select name from cs_patient_info where patient_no = a.patient_no ) nama,   ";
                SQL = SQL + Environment.NewLine + "hrs_cnt,  round(24 * (nvl(b.obs_end, sysdate) - b.obs_start), 2) durasi,   ";
                SQL = SQL + Environment.NewLine + "case when round(24 * (nvl(b.obs_end, sysdate) - b.obs_start), 2) > hrs_cnt and obs_end is null then 'Waktu habis'   ";
                SQL = SQL + Environment.NewLine + "when round(24 * (nvl(b.obs_end, sysdate) - b.obs_start), 2) <= hrs_cnt and obs_end is null then 'Proses' else 'Selesai' end stat ";
                SQL = SQL + Environment.NewLine + "from cs_patient a   ";
                SQL = SQL + Environment.NewLine + "join cs_observation b on (a.rm_no = b.rm_no)    ";
                SQL = SQL + Environment.NewLine + "JOIN cs_room d on (b.room_cd=d.room_id)   ";
                SQL = SQL + Environment.NewLine + "and a.status = 'A' AND d.status = 'A'   ";
                //SQL = SQL + Environment.NewLine + "and to_char(b.insp_date, 'yyyy-mm-dd') = to_char(sysdate,'yyyy-mm-dd')) ";
                SQL = SQL + Environment.NewLine + "and b.insp_date <= trunc(sysdate)) ";
                SQL = SQL + Environment.NewLine + "where stat='Waktu habis' ";


                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    obsNotif = new ObsNotif();
                    obsNotif.Show();
                    obsNotif.v_empid = DB.vUserId;
                }

            }
            else if (CheckOpened(obsNotif.Text))
            {
                //obsNotif.WindowState = FormWindowState.Maximized;
                obsNotif.Show();
                //obsNotif.ShowDialog();
                obsNotif.Focus();
            }

            if (rsvNotif == null || rsvNotif.Text == "")
            {

                string SQL = "";

                SQL = SQL + Environment.NewLine + "select patient_no from cs_visit ";
                SQL = SQL + Environment.NewLine + "where trunc(visit_date)<= trunc(sysdate-1) ";
                SQL = SQL + Environment.NewLine + "and status in ('PRE','RSV','NUR','INS','MED','OBS','HOL') ";

                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    rsvNotif = new RsvNotif();
                    rsvNotif.Show();
                    rsvNotif.v_empid = DB.vUserId;
                }

            }
            else if (CheckOpened(rsvNotif.Text))
            {
                //rsvNotif.WindowState = FormWindowState.Maximized;
                rsvNotif.Show();
                //rsvNotif.ShowDialog();
                rsvNotif.Focus();
            }
        }

        private void timerObs_Tick(object sender, EventArgs e)
        {
            obst++;

            if (obst == popup_interval)
            {
                obst = 0;
                timerObs.Stop();
                timerObs.Start();

                if (obsNotif == null || obsNotif.Text == "")
                {

                    string SQL = "";
                    SQL = SQL + Environment.NewLine + "select obs_id ";
                    SQL = SQL + Environment.NewLine + "from ( ";
                    SQL = SQL + Environment.NewLine + "select b.rm_no, b.insp_date, b.visit_no, b.obs_id, d.room_name, ";
                    SQL = SQL + Environment.NewLine + "(select name from cs_patient_info where patient_no = a.patient_no ) nama,   ";
                    SQL = SQL + Environment.NewLine + "hrs_cnt,  round(24 * (nvl(b.obs_end, sysdate) - b.obs_start), 2) durasi,   ";
                    SQL = SQL + Environment.NewLine + "case when round(24 * (nvl(b.obs_end, sysdate) - b.obs_start), 2) > hrs_cnt and obs_end is null then 'Waktu habis'   ";
                    SQL = SQL + Environment.NewLine + "when round(24 * (nvl(b.obs_end, sysdate) - b.obs_start), 2) <= hrs_cnt and obs_end is null then 'Proses' else 'Selesai' end stat ";
                    SQL = SQL + Environment.NewLine + "from cs_patient a   ";
                    SQL = SQL + Environment.NewLine + "join cs_observation b on (a.rm_no = b.rm_no)    ";
                    SQL = SQL + Environment.NewLine + "JOIN cs_room d on (b.room_cd=d.room_id)   ";
                    SQL = SQL + Environment.NewLine + "and a.status = 'A' AND d.status = 'A'   ";
                    //SQL = SQL + Environment.NewLine + "and to_char(b.insp_date, 'yyyy-mm-dd') = to_char(sysdate,'yyyy-mm-dd')) ";
                    SQL = SQL + Environment.NewLine + "and b.insp_date <= trunc(sysdate)) ";
                    SQL = SQL + Environment.NewLine + "where stat='Waktu habis' ";


                    OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                    DataTable dt = new DataTable();
                    adSql.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        obsNotif = new ObsNotif();
                        obsNotif.Show();
                        obsNotif.v_empid = DB.vUserId;
                    }

                }
                else if (CheckOpened(obsNotif.Text))
                {
                    //obsNotif.WindowState = FormWindowState.Maximized;
                    obsNotif.Show();
                    //obsNotif.ShowDialog();
                    obsNotif.Focus();
                }

                if (rsvNotif == null || rsvNotif.Text == "")
                {

                    string SQL = "";

                    SQL = SQL + Environment.NewLine + "select patient_no from cs_visit ";
                    SQL = SQL + Environment.NewLine + "where trunc(visit_date)<= trunc(sysdate-1) ";
                    SQL = SQL + Environment.NewLine + "and status in ('PRE','RSV','NUR','INS','MED','OBS','HOL') ";

                    OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                    DataTable dt = new DataTable();
                    adSql.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        rsvNotif = new RsvNotif();
                        rsvNotif.Show();
                        rsvNotif.v_empid = DB.vUserId;
                    }

                }
                else if (CheckOpened(rsvNotif.Text))
                {
                    //rsvNotif.WindowState = FormWindowState.Maximized;
                    rsvNotif.Show();
                    //rsvNotif.ShowDialog();
                    rsvNotif.Focus();
                }

            }
        }

        private void xtraTabPage1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSaveAdd_Click(object sender, EventArgs e)
        {
            string val = "", stat = "", nik = "", rm_no = "";
            string sql_addinfo = "";

            sql_addinfo = " select info_cd, description from cs_add_info where status = 'A' and poli_cd = '" + s_policd + "' ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_addinfo, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            int iii = 0;
            //for (int i = 0; i < gridView3.DataRowCount; i++)
            //{
            //    nik = gridView3.GetRowCellValue(i, gridView3.Columns[0]).ToString();
            //    rm_no = gridView3.GetRowCellValue(i, gridView3.Columns[6]).ToString();
            //    stat = gridView3.GetRowCellValue(i, gridView3.Columns[5]).ToString();

            //    upd_col = upd_col + " update cs_patient set status = '" + stat + "' ";
            //    for (int ii = 0; ii < dt.Rows.Count; ii++)
            //    {
            //        iii = ii + 7;
            //        val = gridView3.GetRowCellValue(i, gridView3.Columns[iii]).ToString();

            //        upd_col = upd_col + ", " + dt.Rows[ii]["info_cd"].ToString() + " = '" + val + "' ";
            //    }
            //    upd_col = upd_col + " , upd_date=sysdate, upd_emp='" + DB.vUserId + "' ";
            //    upd_col = upd_col + " where patient_no='" + nik + "' and rm_no='" + rm_no + "' ";


            //    try
            //    {
            //        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            //        OleDbCommand cm = new OleDbCommand(upd_col, oraConnect);
            //        oraConnect.Open();
            //        cm.ExecuteNonQuery();
            //        oraConnect.Close();
            //        cm.Dispose();

            //        //MessageBox.Show("Query Exec : " + sql_update);

            //        //MessageBox.Show("Update Success");
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("ERROR: " + ex.Message);
            //    }

            //    upd_col = "";
            //}
        }

       

        private void btnCetak_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string p_pasno = "", p_date = "";

            if (gridView1.RowCount > 0)
            {
                p_pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
                p_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();

                SQL = "";
                SQL = SQL + Environment.NewLine + "select c.name, round(((sysdate-c.birth_date)/30)/12) age, c.job, c.address,  ";
                SQL = SQL + Environment.NewLine + "d.name p_name, round(((sysdate-d.birth_date)/30)/12) p_age,  ";
                SQL = SQL + Environment.NewLine + "d.job p_job, d.relation , d.address p_address,  ";
                SQL = SQL + Environment.NewLine + "to_char(visit_date,'fmdd Month yyyy', 'nls_date_language = INDONESIAN') as ddate ";
                SQL = SQL + Environment.NewLine + "from cs_inpatient a  ";
                SQL = SQL + Environment.NewLine + "join cs_visit b on (a.inpatient_id=b.inpatient_id)  ";
                SQL = SQL + Environment.NewLine + "join cs_patient_info c on (b.patient_no=c.patient_no)  ";
                SQL = SQL + Environment.NewLine + "join cs_guarantor d on (a.gr_no=d.gr_no)  ";
                SQL = SQL + Environment.NewLine + "where 1=1 ";
                SQL = SQL + Environment.NewLine + "and a.status not in ('CAN') ";
                SQL = SQL + Environment.NewLine + "and b.patient_no = '" + p_pasno + "'  ";
                SQL = SQL + Environment.NewLine + "and to_char(visit_date,'yyyy-mm-dd') = '" + p_date + "'  ";

                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dsAgree.Tables.Clear();
                    dsAgree.Tables.Add(dt);

                    ReportAgreement report = new ReportAgreement(dsAgree);
                    report.ShowPreviewDialog();
                }
                else
                {

                } 
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

        private void btnr_obat_p_Click(object sender, EventArgs e)
        {
            if (dtJadwalObat == null) return;

            DataRow newRow = dtJadwalObat.NewRow();

            newRow["SEQ"] = ((gvJadwalObat.RowCount) + 1).ToString();
            dtJadwalObat.Rows.Add(newRow);

            gcJadwalObat.DataSource = dtJadwalObat;
        }

        private void bsave_obat_p_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvJadwalObat.RowCount > 0)
                {
                    DataTable dt = ORADB.SetData(ORADB.XE, "select * from T1_JADWAL_BERI_OBAT where anamesa_id =" + v_ptnumber + " ");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ORADB.Execute(ORADB.XE, "delete from T1_JADWAL_BERI_OBAT where anamesa_id = " + v_ptnumber + " ");
                    }

                    string sql = "insert all ";
                    for (int i = 0; i < gvJadwalObat.RowCount; i++)
                    {
                        string dte = "";
                        object tgl = gvJadwalObat.GetRowCellValue(i, "TANGGAL");
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

                        sql = sql + " into T1_JADWAL_BERI_OBAT (anamesa_id, seq,  nama_obat,  tanggal,  ttd, INS_BY, INS_DATE) values ( ";
                        sql = sql + " " + v_ptnumber + " ,";
                        sql = sql + " " + FN.strVal(gvJadwalObat, i, "SEQ") + " ,";
                        //sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JENIS_OBAT") + "' ,";
                        sql = sql + " '" + FN.strVal(gvJadwalObat, i, "NAMA_OBAT") + "' ,";
                        //sql = sql + " '" + FN.strVal(gvJadwalObat, i, "DOSIS") + "' ,";
                        sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd') ,";
                        //sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JAM1") + "' ,";
                        //sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JAM2") + "' ,";
                        //sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JAM3") + "' ,";
                        //sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JAM4") + "' ,";
                        //sql = sql + " '" + FN.strVal(gvJadwalObat, i, "EXTRA") + "' ,";
                        sql = sql + " '" + FN.strVal(gvJadwalObat, i, "TTD") + "', '" + DB.vUserId + "', SYSDATE  ) ";
                    }
                    sql = sql + " select * from dual";
                    bool save = ORADB.Execute(ORADB.XE, sql);
                    if (save)
                    {
                        //MessageBox.Show("Jadwal Pemberian obat berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
        }

        private void simpleButton9_Click(object sender, EventArgs e)
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

                id = gvJadwalObat.GetRowCellValue(gvJadwalObat.FocusedRowHandle, gvJadwalObat.Columns[0]).ToString();

                sql_delete = "";

                sql_delete = sql_delete + " update T1_JADWAL_BERI_OBAT set f_aktif = 'N' , DEL_BY = '" + DB.vUserId + "', DEL_DATE = SYSDATE ";
                sql_delete = sql_delete + " where  anamesa_id = " + v_ptnumber + " and SEQ = " + id + " ";

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
                    MessageBox.Show("Data Berhasil dihapus");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }
         
        private void checkBox34_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox34.Checked) txMakanan.Enabled = true;
            else txMakanan.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) txtaobat.Enabled = true;
            else txtaobat.Enabled = false;
        }
        FrmTindakan FrmTindakan = null;
        ppPendaftaran ppPendaftaran = null;
        private void replayanan_DoubleClick(object sender, EventArgs e)
        {
            ppPendaftaran = new ppPendaftaran();
            ppPendaftaran.p_poli = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[6]).ToString(); 
            ppPendaftaran.prekam_medis = txt_rekammds.Text;
            ppPendaftaran.ShowDialog();
            ppPendaftaran.Focus();
        }

        private void gridView3_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvMedisPeriksa_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            gridView3.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView3.AddNewRow();
        }
        private void gridView3_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            string    s_idvisit = "", s_anam = "",  s_queu = "";
            s_idvisit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[19]).ToString();
            s_queu = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_anam = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[20]).ToString();

            view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[15], s_idvisit);
            view.SetRowCellValue(e.RowHandle, view.Columns[16], s_anam);
            view.SetRowCellValue(e.RowHandle, view.Columns[17], s_queu);
        }
        private void LoadDataResep()
        {
            string sql_med_load = "", s_rm = "", s_date = "", s_idvisit = "";

            s_rm = txt_rekammds.Text;
            s_idvisit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[19]).ToString();
            //s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();

            sql_med_load = " select a.receipt_id, a.med_cd, b.med_group, a.med_cd, A.formula, type_drink,  " +
                           " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +  " +
                           " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -  " +
                           " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) - " +
                           " klinik.FN_CS_REQ_STOCK(a.insp_date,a.med_cd) stock, " +
                           " A.med_qty, initcap(uom) uom, 'S' action, a.confirm, a.days, a.price, a.qty_day, a.dosis, a.ID_VISIT , c.ANAMNESA_ID, a.VISIT_NO " +
                           " from KLINIK.cs_receipt a   " +
                           " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)  left join CS_ANAMNESA c on (a.ID_VISIT = c.ID_VISIT) JOIN KLINIK.cs_formula D ON (B.med_cd = D.med_cd AND D.FORMULA_ID = A.formula) " +
                           " where b.status = 'A'  and D.MINUS_STOK ='N' " +
                           " and a.rm_no = '" + s_rm + "'  " +
                           " and a.ID_VISIT = '" + s_idvisit + "'  " ;

            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_med_load, oraConnect2);
            DataTable dt2 = new DataTable();
            adOra2.Fill(dt2);

            gridControl3.DataSource = null;
            gridView3.Columns.Clear();
            gridControl3.DataSource = dt2;

            gridView3.OptionsView.ColumnAutoWidth = true;
            gridView3.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView3.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView3.IndicatorWidth = 30;
            //gridView3.OptionsBehavior.Editable = false;
            gridView3.BestFitColumns();

            gridView3.Columns[0].Caption = "ID";
            gridView3.Columns[1].Caption = "Kode";
            gridView3.Columns[2].Caption = "Group";
            gridView3.Columns[3].Caption = "Nama Obat";
            gridView3.Columns[4].Caption = "Kode Dosis";
            gridView3.Columns[5].Caption = "Info";
            gridView3.Columns[6].Caption = "Stok";
            gridView3.Columns[7].Caption = "Jumlah";
            gridView3.Columns[8].Caption = "Satuan";
            gridView3.Columns[9].Caption = "Action";
            gridView3.Columns[10].Caption = "Confirm";
            gridView3.Columns[11].Caption = "Jml";
            gridView3.Columns[12].Caption = "Harga";
            gridView3.Columns[13].Caption = "Jumlah per Hari";
            gridView3.Columns[14].Caption = "Dosis";
            gridView3.Columns[15].Caption = "ID_VISIT";
            gridView3.Columns[16].Caption = "ANAMNESA_ID";
            gridView3.Columns[17].Caption = "VISIT_NO";

            gridView3.Columns[14].VisibleIndex = 5;
            gridView3.Columns[11].VisibleIndex = 6;

            gridView3.Columns[4].MinWidth = 80;
            gridView3.Columns[4].MaxWidth = 80;
            gridView3.Columns[5].MinWidth = 120;
            gridView3.Columns[5].MaxWidth = 120;
            gridView3.Columns[6].MinWidth = 60;
            gridView3.Columns[6].MaxWidth = 60;
            gridView3.Columns[7].MinWidth = 60;
            gridView3.Columns[7].MaxWidth = 60;
            gridView3.Columns[8].MinWidth = 60;
            gridView3.Columns[8].MaxWidth = 60;
            gridView3.Columns[10].MinWidth = 60;
            gridView3.Columns[10].MaxWidth = 60;
            gridView3.Columns[11].MinWidth = 60;
            gridView3.Columns[11].MaxWidth = 60;
            gridView3.Columns[14].MinWidth = 60;
            gridView3.Columns[14].MaxWidth = 60;

            gridView3.Columns[0].Visible = false;
            gridView3.Columns[1].Visible = false;
            gridView3.Columns[2].Visible = false;
            gridView3.Columns[7].Visible = false;
            gridView3.Columns[8].Visible = false;
            gridView3.Columns[9].Visible = false;
            gridView3.Columns[12].Visible = false;
            gridView3.Columns[13].Visible = false;
            gridView3.Columns[15].Visible = false; gridView3.Columns[16].Visible = false; gridView3.Columns[17].Visible = false;

            //gridView3.Columns[3].OptionsColumn.ReadOnly = true;
            gridView3.Columns[2].OptionsColumn.ReadOnly = true;
            gridView3.Columns[6].OptionsColumn.ReadOnly = true;
            gridView3.Columns[7].OptionsColumn.ReadOnly = true;
            gridView3.Columns[8].OptionsColumn.ReadOnly = true;
            gridView3.Columns[9].OptionsColumn.ReadOnly = true;
            gridView3.Columns[10].OptionsColumn.ReadOnly = true;


            string sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1 ";
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

            string sql_med = " ";
            sql_med = sql_med + Environment.NewLine + " select a.med_cd, initcap(med_name) || ' (BPJS: ' || bpjs_cover || ')' med_name  ";
            sql_med = sql_med + Environment.NewLine + "  from KLINIK.cs_medicine a, CS_FORMULA b ";
            sql_med = sql_med + Environment.NewLine + " where a.MED_CD = b.MED_CD ";
            sql_med = sql_med + Environment.NewLine + "   and a.status = 'A'   ";
            sql_med = sql_med + Environment.NewLine + "   and b.MINUS_STOK ='N'  ";
            sql_med = sql_med + Environment.NewLine + " order by med_name "; 


            //string sql_med = " select med_cd, initcap(med_name) || ' (BPJS: ' || bpjs_cover || ')' med_name from KLINIK.cs_medicine where status = 'A' and MED_GROUP ='OBAT' order by med_name ";
            OleDbConnection sqlConnect4 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql4 = new OleDbDataAdapter(sql_med, sqlConnect4);
            DataTable dt4 = new DataTable();
            dtGlMed = dt4;
            adSql4.Fill(dt4);
            listMedicine.Clear();
            for (int i = 0; i < dt4.Rows.Count; i++)
            {
                listMedicine.Add(new Medicine() { medicineCode = dt4.Rows[i]["med_cd"].ToString(), medicineName = dt4.Rows[i]["med_name"].ToString() });
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
            gridView3.Columns[3].ColumnEdit = glmed;

            RepositoryItemGridLookUpEdit glfor = new RepositoryItemGridLookUpEdit();
            glfor.DataSource = listFormula2;
            glfor.ValueMember = "formulaCode";
            glfor.DisplayMember = "formulaName";

            glfor.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glfor.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glfor.ImmediatePopup = true;
            glfor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glfor.NullText = "";
            gridView3.Columns[4].ColumnEdit = glfor;

            RepositoryItemLookUpEdit medicineInfoLookup = new RepositoryItemLookUpEdit();
            medicineInfoLookup.DataSource = listMedicineInfo;
            medicineInfoLookup.ValueMember = "medicineInfoCode";
            medicineInfoLookup.DisplayMember = "medicineInfoName";

            medicineInfoLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            medicineInfoLookup.DropDownRows = listMedicineInfo.Count;
            medicineInfoLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            medicineInfoLookup.AutoSearchColumnIndex = 1;
            medicineInfoLookup.NullText = "";
            gridView3.Columns[5].ColumnEdit = medicineInfoLookup;

            RepositoryItemLookUpEdit dosisLookup = new RepositoryItemLookUpEdit();
            dosisLookup.DataSource = listDosis;
            dosisLookup.ValueMember = "DosisCode";
            dosisLookup.DisplayMember = "DosisName";
            dosisLookup.NullText = "";
            gridView3.Columns[14].ColumnEdit = dosisLookup;

            simpleButton11.Enabled = true; 

            if (gridView3.RowCount > 0)
            {
                simpleButton15.Enabled = true; 
            }
            else
            {
                simpleButton15.Enabled = false; 
            }
        }

        void ListDataLayanan(string idvisit)
        {
            string policd = "";
            policd = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[6]).ToString();

            string SQL = " ";
            SQL = SQL + Environment.NewLine + " select ROWNUM SEQ, b.detail_id, c.treat_group_id, b.treat_item_id, c.TREAT_ITEM_NAME, b.treat_qty, b.treat_item_price,  ";
            SQL = SQL + Environment.NewLine + "         b.remarks, 'S' action, a.head_id, b.treat_date  TANGGAL, TREAT_JAM JAM, a.pay_status ,a.ID_VISIT ";
            SQL = SQL + Environment.NewLine + "    from KLINIK.cs_treatment_head a  ";
            SQL = SQL + Environment.NewLine + "    join KLINIK.cs_treatment_detail b on (a.head_id=b.head_id)  ";
            SQL = SQL + Environment.NewLine + "    join KLINIK.cs_treatment_item c on (b.treat_item_id=c.treat_item_id)  ";
            SQL = SQL + Environment.NewLine + "    JOIN KLINIK.cs_visit d ON (a.ID_VISIT = d.ID_VISIT)  ";
            SQL = SQL + Environment.NewLine + "   where a.ID_VISIT = '" + idvisit + "'   and b.ID_DOKTER is  null "; 
            if (policd.ToString().Equals("POL0001"))
                SQL = SQL + Environment.NewLine + "  AND D.POLI_CD = 'POL0001' AND c.treat_group_id ='TRG02' ";

            //"   and a.status = 'OPN'  ";

            dtMedis = ORADB.SetData(ORADB.XE, SQL);
            gridMedisPeriksa.DataSource = dtMedis;
            gvMedisPeriksa.IndicatorWidth = 30;
            gvMedisPeriksa.Columns[0].Visible = false;
            //gvMedisPeriksa.Columns[1].Visible = false; gvMedisPeriksa.Columns[2].Visible = false;
            //gvMedisPeriksa.Columns[5].Visible = false; gvMedisPeriksa.Columns[8].Visible = false; gvMedisPeriksa.Columns[9].Visible = false;
            if (dtMedis.Rows.Count > 0)
                btnDelTindakan.Enabled = true;

            SQL = " ";
            SQL = SQL + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) treat_item_name ";
            SQL = SQL + Environment.NewLine + "  from KLINIK.cs_treatment_item ";
            SQL = SQL + Environment.NewLine + " where 1=1 ";
            SQL = SQL + Environment.NewLine + "   and treat_type_id = 'TRT01'  AND treat_group_id = 'TRG02'  AND UPPER(TREAT_ITEM_NAME) NOT LIKE '%VISIT DOKTER%' ";

            OleDbConnection oraConnectly = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraly = new OleDbDataAdapter(SQL, oraConnectly);
            DataTable dtly = new DataTable();
            adOraly.Fill(dtly);
            listLaya2.Clear();
            for (int i = 0; i < dtly.Rows.Count; i++)
            {
                listLaya2.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
            }

            RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
            glLaya.DataSource = listLaya2;
            glLaya.ValueMember = "layananCode";
            glLaya.DisplayMember = "layananName";

            glLaya.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glLaya.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glLaya.ImmediatePopup = true;
            glLaya.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glLaya.NullText = "";
            gvMedisPeriksa.Columns[3].ColumnEdit = glLaya;
        }

        private void gridView3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            simpleButton11.Enabled = true;
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();

            if (e.Column.Caption == "Nama Obat"  )
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();
                string sql_medcd = "", sql_med = "", med_cd = "", med_name = "", med_group = "", med_stok = "", med_uom = "", cek_stok = "", sql_for = "";

                sql_medcd = " select " +
                            " klinik.FN_CS_INIT_STOCK(to_date('2024-05-03','yyyy-mm-dd'),'" + a + "') +  " +
                            " klinik.FN_CS_TRX_IN(to_date('2024-05-03','yyyy-mm-dd'),'" + a + "') -  " +
                            " klinik.FN_CS_TRX_OUT(to_date('2024-05-03','yyyy-mm-dd'),'" + a + "') - " +
                            " klinik.FN_CS_REQ_STOCK(to_date('2024-05-03','yyyy-mm-dd'),'" + a + "') stock from dual ";

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

                sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1  and  b.med_cd = '" + med_cd + "' and a.MINUS_STOK ='N' ";
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

                view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[4], "");
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
            }

            if (e.Column.Caption == "Kode Dosis")
            {
                string medicine_cd = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string formula_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                //string reg_dt = lMedDate.Text;
                //string rm = lMedRm.Text;
                //string que = lMedQue.Text;
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

                tot_hari = Convert.ToInt16(tmp_hari) * Convert.ToInt16(qty);
                tot_harga = Convert.ToInt16(tmp_hari) * Convert.ToInt16(med_price);

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

            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Info" || e.Column.Caption == "Dosis")
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

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            string kode = "", dosis = "", info = "", jumlah = "", id = "", stok = "", con = "", action = "", id_visit ="", lMedNik ="", anamnesaid = "" , visitno = "";
            string sql_cnt = "", med_cnt = "", sql_update = "", sql_diag = "", diag_cnt = "", harga = "", hari = "", jph = "", info_dosis = "";

            lMedNik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            if (txt_rekammds.Text.ToString().Equals(""))
            {
                MessageBox.Show("Silahkan Tentukan Pasien. No Rekam Medis tidak boleh kosong...!!");
                return;
            }
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                id = gridView3.GetRowCellValue(i, gridView3.Columns[0]).ToString();
                kode = gridView3.GetRowCellValue(i, gridView3.Columns[1]).ToString();
                dosis = gridView3.GetRowCellValue(i, gridView3.Columns[4]).ToString();
                info = gridView3.GetRowCellValue(i, gridView3.Columns[5]).ToString();
                jumlah = gridView3.GetRowCellValue(i, gridView3.Columns[7]).ToString();
                stok = gridView3.GetRowCellValue(i, gridView3.Columns[6]).ToString();
                con = gridView3.GetRowCellValue(i, gridView3.Columns[10]).ToString();
                action = gridView3.GetRowCellValue(i, gridView3.Columns[9]).ToString();
                harga = gridView3.GetRowCellValue(i, gridView3.Columns[12]).ToString();
                hari = gridView3.GetRowCellValue(i, gridView3.Columns[11]).ToString();
                jph = gridView3.GetRowCellValue(i, gridView3.Columns[13]).ToString();
                info_dosis = gridView3.GetRowCellValue(i, gridView3.Columns[14]).ToString();
                id_visit = gridView3.GetRowCellValue(i, gridView3.Columns[15]).ToString();
                anamnesaid = gridView3.GetRowCellValue(i, gridView3.Columns[16]).ToString();
                visitno = gridView3.GetRowCellValue(i, gridView3.Columns[17]).ToString();

                if (con == "Y")
                {
                    MessageBox.Show("Data tidak bisa dirubah.");
                }
                else if (stok == "0")
                {
                    MessageBox.Show("Stok obat tidak tersedia.");
                }
                else if (jumlah == "" || jumlah == "0")
                {
                    MessageBox.Show("Jumlah obat harus diisi.");
                }
                //else if (Convert.ToInt16(jumlah) > Convert.ToInt16(stok))
                //{
                //    MessageBox.Show("Jumlah melebihi stok");
                //}
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
                    int queue = 0;
                    string tmp_queue = "", que = "", cnt = "";
                    string sql_check = " select  nvl(max(to_number(substr(que02,2,3))),0) que from KLINIK.cs_visit where to_char(visit_date,'yyyy-mm-dd')= '" + today + "'  ";
                    string sql_check2 = " select  count(0) cnt from KLINIK.cs_receipt where rm_no = '" + txt_rekammds.Text + "' and ID_VISIT = '" + id_visit + "'    ";

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
                            sql_update = sql_update + " where patient_no = '" + lMedNik + "' and ID_VISIT = '" + id_visit + "'  ";

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
                        sql_diag = " select count(0) cnt from KLINIK.cs_diagnosa where ANAMNESA_ID = '" + anamnesaid + "' ";
                        OleDbConnection oraConnectd = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOrad = new OleDbDataAdapter(sql_diag, oraConnectd);
                        DataTable dtd = new DataTable();
                        adOrad.Fill(dtd);
                        diag_cnt = dtd.Rows[0]["cnt"].ToString();


                        sql_cnt = " select count(0) cnt from KLINIK.cs_receipt where rm_no = '" + txt_rekammds.Text + "' and ID_VISIT = '" + id_visit + "'  and med_cd = '" + kode + "' ";
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                        DataTable dt = new DataTable();
                        adOra.Fill(dt);
                        med_cnt = dt.Rows[0]["cnt"].ToString();

                        if (Convert.ToInt32(med_cnt) > 0)
                        {
                            //MessageBox.Show("Gagal Disimpan.");
                        }
                        //else if (diag_cnt == "0")
                        //{
                        //    MessageBox.Show("Gagal Disimpan. Diagnosa belum diinput.");
                        //}
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
                                                      " values(cs_receipt_seq.nextval, '" + txt_rekammds.Text + "', to_date(to_char(sysdate,'YYYY-MM-DD'),'YYYY-MM-DD'), '" + kode + "', '" + dosis + "', '" + jumlah + "', '" + info + "', 'Y', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "', '" + visitno + "', sysdate, '" + DB.vUserId + "', " + id_visit + ") ";
                                command.ExecuteNonQuery();

                                //command.CommandText = " update cs_visit set status = 'MED', time_inspection=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + lMedNik.Text + "' and to_char(visit_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and que01 = '" + lMedQue.Text + "' ";
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
                            MessageBox.Show("Data Berhasil diupdate");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
            }
            LoadDataResep();
        }

        private void gvMedisPeriksa_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            string a = "", tmp_stat = "", date = "", que = "", rm_no = "", no_visit = "";

            //date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            //que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            //rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString(); 

            if (e.Column.Caption == "Nama Pelayanan")
            {
                a = view.GetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_ID"]).ToString();
                no_visit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[19]).ToString();
                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();

                string sql_ = "", sql_head = "", group_id = "", price = "", head_id = "", stbyr = "";
                sql_ = " select treat_group_id, treat_item_price from KLINIK.cs_treatment_item where treat_item_id = " + a + " ";

                OleDbConnection oraConnect0 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_, oraConnect0);
                DataTable dt0 = new DataTable();
                adOra0.Fill(dt0);
                if (dt0.Rows.Count > 0)
                {
                    group_id = dt0.Rows[0]["TREAT_GROUP_ID"].ToString();
                    price = dt0.Rows[0]["TREAT_ITEM_PRICE"].ToString();
                }

                sql_head = " select head_id, pay_status from KLINIK.cs_treatment_head where ID_VISIT = '" + no_visit + "'  ";

                OleDbConnection oraConnect1 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra1 = new OleDbDataAdapter(sql_head, oraConnect1);
                DataTable dt1 = new DataTable();
                adOra1.Fill(dt1);
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

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            //gridView1.DoubleClick += GridView1_DoubleClick;
            GridView View = sender as GridView;
            string s_rm = "", s_que = "", s_poli = "", s_group = "", s_rmno = "", group = "", s_nama = "", s_berobat = "";

            s_rm = View.GetRowCellDisplayText(gridView1.FocusedRowHandle, View.Columns[14]);
            s_que = View.GetRowCellDisplayText(gridView1.FocusedRowHandle, View.Columns[0]);
            s_nik = View.GetRowCellDisplayText(gridView1.FocusedRowHandle, View.Columns[1]);
            s_nama = View.GetRowCellDisplayText(gridView1.FocusedRowHandle, View.Columns[2]);
            s_poli = View.GetRowCellDisplayText(gridView1.FocusedRowHandle, View.Columns[6]);
            s_berobat = View.GetRowCellDisplayText(gridView1.FocusedRowHandle, View.Columns[9]);
            s_rmno = View.GetRowCellDisplayText(gridView1.FocusedRowHandle, View.Columns[14]);
            s_group = View.GetRowCellDisplayText(gridView1.FocusedRowHandle, View.Columns[15]);
            s_policd = View.GetRowCellDisplayText(gridView1.FocusedRowHandle, View.Columns[16]);

            v_rmnumber = View.GetRowCellDisplayText(gridView1.FocusedRowHandle, View.Columns[14]);
            visitid = View.GetRowCellDisplayText(gridView1.FocusedRowHandle, View.Columns[19]);
            txt_rekammds.Text = v_rmnumber;

            pnama_pasien = s_nama;

            if (s_poli == "Poli Ibu Hamil")
            {
                tableLayoutPanel6.RowStyles[0] = new RowStyle(SizeType.Percent, 12);
                tableLayoutPanel6.RowStyles[1] = new RowStyle(SizeType.Percent, 38);
                tableLayoutPanel6.RowStyles[2] = new RowStyle(SizeType.Percent, 12);
                tableLayoutPanel6.RowStyles[3] = new RowStyle(SizeType.Percent, 38);
            }
            else
            {
                tableLayoutPanel6.RowStyles[0] = new RowStyle(SizeType.Percent, 10);
                tableLayoutPanel6.RowStyles[1] = new RowStyle(SizeType.Percent, 90);
                tableLayoutPanel6.RowStyles[2] = new RowStyle(SizeType.Percent, 0);
                tableLayoutPanel6.RowStyles[3] = new RowStyle(SizeType.Percent, 0);
            }

            if (s_rm == "")
            {
                if (s_berobat == "Dokter")
                {
                    btnCreate.Enabled = false;
                }
                else
                {
                    btnCreate.Enabled = true;
                }

                btnSaveAnam.Enabled = false;
            }
            else
            {
                btnCreate.Enabled = false;
            }

            string sql_addinfo = "", sql_info = "", p_col = "";

            sql_addinfo = " select info_cd, description from cs_add_info where status = 'A' and poli_cd = '" + s_poli + "' ";

            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_addinfo, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                p_col = p_col + ", " + dt2.Rows[i]["info_cd"].ToString();
            }

            //if (s_group == "Umum")
            //{
            group = "COMM";
            //}
            //else if (s_group == "KB")
            //{
            //    group = "FAMP";
            //}
            //else
            //{
            //    group = "PREG";
            //}

            sql_info = " ";
            sql_info = sql_info + " select  patient_no, group_patient, decode(group_patient,'PREG','Ibu Hamil','FAMP','KB','Umum') group_patient_nm, '" + s_nama + "' as nama, 'U' as a, status, rm_no ";
            sql_info = sql_info + p_col;
            sql_info = sql_info + " from cs_patient where status='A' and group_patient='" + group + "' and patient_no='" + s_nik + "' ";

            OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_info, sqlConnect3);
            DataTable dt3 = new DataTable();
            adSql3.Fill(dt3);

            gridControl6.DataSource = null;
            gridView6.Columns.Clear();
            gridControl6.DataSource = dt3;

            //gridView6.OptionsView.ColumnAutoWidth = true;
            gridView6.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView6.Appearance.HeaderPanel.FontSizeDelta = 0;
            //gridView6.BestFitColumns();
            int ii = 0;


            gridView6.Columns[0].Caption = "Pasien No";
            gridView6.Columns[1].Caption = "Type Record";
            gridView6.Columns[2].Caption = "Type Record";
            gridView6.Columns[3].Caption = "Nama";
            gridView6.Columns[4].Caption = "Action";
            gridView6.Columns[5].Caption = "Status";
            gridView6.Columns[6].Caption = "Medical Record";

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                ii = i + 7;
                gridView6.Columns[ii].Caption = dt2.Rows[i]["description"].ToString();
            }
            RepositoryItemLookUpEdit statLookup = new RepositoryItemLookUpEdit();
            statLookup.DataSource = listStat2;
            statLookup.ValueMember = "statCode";
            statLookup.DisplayMember = "statName";

            statLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            statLookup.DropDownRows = listStat2.Count;
            statLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            statLookup.AutoSearchColumnIndex = 1;
            statLookup.NullText = "";
            gridView6.Columns[5].ColumnEdit = statLookup;

            gridView6.Columns[0].OptionsColumn.ReadOnly = true;
            gridView6.Columns[1].OptionsColumn.ReadOnly = true;
            gridView6.Columns[2].OptionsColumn.ReadOnly = true;
            gridView6.Columns[3].OptionsColumn.ReadOnly = true;
            gridView6.Columns[4].OptionsColumn.ReadOnly = true;

            gridView6.Columns[1].Visible = false;
            gridView6.Columns[4].Visible = false;
            gridView6.Columns[6].Visible = false;

            gridView6.BestFitColumns();

            if (gridView6.RowCount > 0)
            {
                btnSaveAdd.Enabled = true;
            }
            else
            {
                btnSaveAdd.Enabled = false;
            }

            if (!visitid.ToString().Equals(""))
            {
                string sql_anam = "";
                sql_anam = " select to_date(to_char(insp_date,'dd/MM/yyyy'),'dd/MM/yyyy') as insp_date, '" + s_nama + "' as nama, visit_no, " +
                           " blood_press, pulse, temperature, allergy, anamnesa, info_k, 'S' action, rm_no, bb, tb, " +
                           " cholesterol, blood_sugar, uric_acid, VITALHR, VITALRR, disease_now, disease_then, disease_family, anamnesa_physical, anamnesa_other, ANAMNESA_ID" +
                           " from cs_anamnesa where ID_VISIT =  " + visitid + "  ";

                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_anam, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl2.DataSource = null;
                gridView2.Columns.Clear();
                gridControl2.DataSource = dt;



                if (dt.Rows.Count > 0)
                {

                    v_ptnumber = dt.Rows[0]["ANAMNESA_ID"].ToString();
                    dtJadwalObat = ORADB.SetData(ORADB.XE, "select * from T1_JADWAL_BERI_OBAT where anamesa_id =" + v_ptnumber + " AND F_AKTIF ='Y'");
                    gcJadwalObat.DataSource = dtJadwalObat;
                }
                else
                {
                    v_ptnumber = "";
                    if (gcJadwalObat.DataSource != null)
                    {
                        dtJadwalObat.Rows.Clear();
                    }
                    //if (!v_ptnumber.ToString().Equals(""))

                    //dtJadwalObat.Columns.Clear();
                    //dtJadwalObat.Reset();
                    gcJadwalObat.DataSource = null;
                    return;
                }



                //gridView2.OptionsView.ColumnAutoWidth = true;
                gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
                //gridView2.BestFitColumns();
                gridView2.FixedLineWidth = 3;
                gridView2.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView2.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView2.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

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
                gridView2.Columns[11].Caption = "BB (Kg)";
                gridView2.Columns[12].Caption = "TB (Cm)";
                gridView2.Columns[13].Caption = "Kolesterol (Mg)";
                gridView2.Columns[14].Caption = "Gula Darah (Mg)";
                gridView2.Columns[15].Caption = "Asam Urat (Mg)";
                gridView2.Columns[16].Caption = "HR (x/m)";
                gridView2.Columns[17].Caption = "RR (x/m)";
                gridView2.Columns[18].Caption = "R.Sekarang";
                gridView2.Columns[19].Caption = "R.Dulu";
                gridView2.Columns[20].Caption = "R.Keluarga";
                gridView2.Columns[21].Caption = "Pem.Fisik";
                gridView2.Columns[22].Caption = "Pem.Lain";

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
                gridView2.Columns[10].Visible = false;
                gridView2.Columns[16].Visible = false;
                gridView2.Columns[23].Visible = false;
                gridView2.Columns[11].VisibleIndex = 6;
                gridView2.Columns[12].VisibleIndex = 7;
                gridView2.BestFitColumns();

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


                LoadDataResep();
                ListDataLayanan(visitid);

                string sql_cek_hold = "", temp_shold = "", temp_ehold = "";

                sql_cek_hold = " select to_char(start_hold,'yyyy-mm-dd') s_hold, to_char(end_hold,'yyyy-mm-dd') e_hold from cs_visit where patient_no = '" + s_nik + "' and trunc(visit_date) =  trunc(to_date('" + today + "','dd/MM/yyyy'))  and que01 = '" + s_que + "' ";

                OleDbConnection sqlConnect4 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql4 = new OleDbDataAdapter(sql_cek_hold, sqlConnect4);
                DataTable dt4 = new DataTable();
                adSql4.Fill(dt4);

                if (dt4.Rows.Count > 0)
                {
                    temp_shold = dt4.Rows[0]["s_hold"].ToString();
                    temp_ehold = dt4.Rows[0]["e_hold"].ToString();

                    if (temp_shold == "" && temp_ehold == "")
                    {
                        btnTunda.Enabled = true;
                        btnLanjut.Enabled = false;
                    }
                    else if (temp_shold != "" && temp_ehold == "")
                    {
                        btnTunda.Enabled = false;
                        btnLanjut.Enabled = true;
                    }
                    else if (temp_shold != "" && temp_ehold != "")
                    {
                        btnTunda.Enabled = false;
                        btnLanjut.Enabled = false;
                    }
                    else
                    {
                        btnTunda.Enabled = false;
                        btnLanjut.Enabled = false;
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    DataTable dt1 = ORADB.SetData(ORADB.XE, "select a.*,b.*,c.*,d.*,round((bb/(tb*tb))* 10000,2) imt  from klinik.cs_anamnesa a, klinik.cs_anamnesa_dtl b, klinik.cs_visit c, klinik.CS_PATIENT_INFO d where A.ANAMNESA_ID = b.anamnesa_id   and b.PATIENT_NO = c.PATIENT_NO and b.PATIENT_NO = d.PATIENT_NO and trunc(VISIT_DATE) = trunc(to_date('" + today + "','dd/MM/yyyy')) and a.ANAMNESA_ID = " + v_ptnumber + " ");
                    if (dt1.Rows.Count > 0)
                    {
                        //mmKeluhan.Text = FN.rowVal(dt1, "KELUHAN_UTAMA");
                        FN.splitVal(FN.rowVal(dt1, "C_MSK_RS"), radioGroup16);
                        FN.splitVal(FN.rowVal(dt1, "SDR_KANDUNG"), radioGroup3);
                        FN.splitVal(FN.rowVal(dt1, "SDR_TIRI"), radioGroup2);
                        FN.splitVal1(FN.rowVal(dt1, "TGL_BERSAMA"), radioGroup4, textBox3);
                        FN.splitVal(FN.rowVal(dt1, "SBICARA"), radioGroup5);
                        FN.splitVal(FN.rowVal(dt1, "SKOMUNIKASI"), radioGroup6);
                        FN.splitVal(FN.rowVal(dt1, "SEMOSI"), radioGroup7);
                        FN.splitVal(FN.rowVal(dt1, "RJIWA"), radioGroup8);
                        FN.splitVal(FN.rowVal(dt1, "KSPIRITUAL"), radioGroup9);
                        FN.splitVal(FN.rowVal(dt1, "RTRAUMA"), radioGroup10);
                        FN.splitVal(FN.rowVal(dt1, "APERASAAN"), radioGroup13);
                        FN.splitVal(FN.rowVal(dt1, "INWAWANCARA"), radioGroup1);
                        FN.splitVal(FN.rowVal(dt1, "MSPIRITUAL"), radioGroup11);
                        FN.splitVal(FN.rowVal(dt1, "KSPIRITUAL"), radioGroup12);
                        FN.splitVal(FN.rowVal(dt1, "JOB"), radioGroup14);
                        FN.splitVal(FN.rowVal(dt1, "STAT_KAWIN"), radioGroup15);
                        FN.splitVal(FN.rowVal(dt1, "JNS_PELAYANAN"), radioGroup17);
                        FN.setCheckList(FN.rowVal(dt1, "SKALA_NYERI"), chkSkalaNyeri);
                        txScorNyeri.Text = FN.rowVal(dt1, "SCORE_NYERI");
                        FN.splitVal(FN.rowVal(dt1, "TINGKAT_NYERI"), rgTingkatNyeri);
                        FN.splitVal(FN.rowVal(dt1, "KUALITAS_NYERI"), radioGroup18);
                        FN.splitVal(FN.rowVal(dt1, "MENJALAR"), radioGroup19);
                        FN.splitVal(FN.rowVal(dt1, "FREKUENSI_NYERI"), radioGroup20);
                        FN.splitVal(FN.rowVal(dt1, "PENGARUH_NYERI"), radioGroup28);
                        FN.splitVal(FN.rowVal(dt1, "PSEMPOYONGAN"), radioGroup22);
                        FN.splitVal(FN.rowVal(dt1, "PPENOPANG"), radioGroup23);
                        FN.splitVal(FN.rowVal(dt1, "HRESIKO"), radioGroup27);
                        FN.splitVal(FN.rowVal(dt1, "BERITAHU_DOKTER"), radioGroup21);
                        FN.splitVal(FN.rowVal(dt1, "SG_KURUS"), radioGroup24);
                        FN.splitVal(FN.rowVal(dt1, "SG_TURUNBB"), radioGroup25);
                        FN.splitVal(FN.rowVal(dt1, "SG_ASUPAN"), radioGroup26);
                        FN.splitVal(FN.rowVal(dt1, "AFS_PENGLIHATAN"), radioGroup30);
                        FN.splitVal(FN.rowVal(dt1, "AFS_PENCIUMAN"), radioGroup31);
                        FN.splitVal(FN.rowVal(dt1, "AFS_PENDENGARAN"), radioGroup32);
                        FN.splitVal(FN.rowVal(dt1, "AFS_KOGNITIF1"), radioGroup33);
                        FN.splitVal(FN.rowVal(dt1, "AFS_KOGNITIF2"), radioGroup34);
                        FN.splitVal(FN.rowVal(dt1, "AFS_MOTOR_SHRI"), radioGroup35);
                        FN.splitVal(FN.rowVal(dt1, "AFS_MOTOR_JALAN"), radioGroup36);
                        FN.splitVal(FN.rowVal(dt1, "DPS_HOME_CARE"), radioGroup37);
                        FN.splitVal(FN.rowVal(dt1, "DPS_IMPLAN"), radioGroup38);
                        FN.splitVal(FN.rowVal(dt1, "DPS_PULANG"), radioGroup39);
                        FN.setCheckList(FN.rowVal(dt1, "ALERGI_MKN"), chkSkalaNyeri);
                        FN.splitVal2(FN.rowVal(dt1, "ALERGI_MKN"), gbMakan, txMakanan);
                        FN.splitVal2(FN.rowVal(dt1, "ALERGI_OBAT"), gbObat, txtaobat);

                        txt_bb.Text = FN.rowVal(dt1, "BB");
                        txt_pbtb.Text = FN.rowVal(dt1, "TB");
                        txt_imt.Text = FN.rowVal(dt1, "IMT");
                    }
                }
            }
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                     "Message",
                      MessageBoxButtons.YesNo,
                      MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string  id = "", payst = "", s_idvisit = "";

                id = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns[0]).ToString();
                payst = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
                s_idvisit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[19]).ToString();

                OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                OleDbCommand command = new OleDbCommand();
                OleDbTransaction trans = null;

                command.Connection = oraConnectTrans;
                oraConnectTrans.Open();

                try
                {
                    if (payst != "CLS")
                    {
                        trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                        command.Connection = oraConnectTrans;
                        command.Transaction = trans; 

                        command.CommandText = " delete KLINIK.cs_receipt where receipt_id = '" + id + "' ";
                        command.ExecuteNonQuery();
                         
                        trans.Commit(); 
                        gridView3.DeleteRow(gridView3.FocusedRowHandle);
                        MessageBox.Show("Data Berhasil di Hapus.");
                    }
                    else
                    {
                        MessageBox.Show("Data Tidak Dapat Dihapus. Karena status sudah bayar");
                    }

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("ERROR: " + ex.Message);
                }
                oraConnectTrans.Close();
                LoadDataResep();
            }
        }

        private void btnDelTindakan_Click(object sender, EventArgs e)
        {
            string stbyr = "";
            if (gvMedisPeriksa.RowCount > 0)
            {
                stbyr = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[22]).ToString();

                if (MessageBox.Show("Anda yakin akan menghapus data?",
                    "Message",
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Information) == DialogResult.No)
                {

                }
                else
                {
                    string id = "", payst = "", s_idvisit = "";

                    id = gvMedisPeriksa.GetRowCellValue(gvMedisPeriksa.FocusedRowHandle, gvMedisPeriksa.Columns[6]).ToString();
                    payst = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
                    s_idvisit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[19]).ToString();

                    OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                    OleDbCommand command = new OleDbCommand();
                    OleDbTransaction trans = null;

                    command.Connection = oraConnectTrans;
                    oraConnectTrans.Open();

                    try
                    {
                        if (payst != "CLS")
                        {
                            trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                            command.Connection = oraConnectTrans;
                            command.Transaction = trans; 

                            command.CommandText = " delete KLINIK.cs_treatment_detail where detail_id = '" + id + "' ";
                            command.ExecuteNonQuery();

                            command.CommandText = " delete KLINIK.cs_action where detail_id = '" + id + "' ";
                            command.ExecuteNonQuery();

                            trans.Commit();
                            gvMedisPeriksa.DeleteRow(gvMedisPeriksa.FocusedRowHandle);
                            MessageBox.Show("Data Berhasil di Hapus.");
                        }
                        else
                        {
                            MessageBox.Show("Data Tidak Dapat Dihapus. Karena status sudah bayar");
                        }

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                    oraConnectTrans.Close();
                    ListDataLayanan(s_idvisit);
                }
            }
        }

        private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.Caption == "Poli")
            { 
                string tmp_poli = gridView1.GetRowCellValue(e.RowHandle, gridView1.Columns[16]).ToString(); 
                if ((!tmp_poli.ToString().Equals("POL0002") && !tmp_poli.ToString().Equals("POL0003")))
                {
                    MessageBox.Show("Hanya Poli Kebidanan & Poli KB yang dapat di Ganti..!!");
                    gridView1.SetRowCellValue(e.RowHandle, gridView1.Columns[6], tmp_poli);
                    LoadData();
                    return;
                } 
            }
            if (e.Column.Caption == "Pasien")
            {
                string tque = gridView1.GetRowCellValue(e.RowHandle, gridView1.Columns[0]).ToString();
                string tidvis1 = gridView1.GetRowCellValue(e.RowHandle, gridView1.Columns[19]).ToString();
                if (!tque.ToString().Equals("") && (!tidvis1.ToString().Equals("0") && !tidvis1.ToString().Equals("")) )
                {
                    MessageBox.Show("Status Type Pasien Tidak Dapat di Ganti...!!");
                    LoadData();
                    return;
                }
            }
            if (e.Column.Caption == "Nama")
            {
                string tidvis = gridView1.GetRowCellValue(e.RowHandle, gridView1.Columns[19]).ToString();
                if (!tidvis.ToString().Equals("0") && !tidvis.ToString().Equals(""))
                {
                    MessageBox.Show("Pasien Tidak Dapat di Ganti Setelah di Registrasi...!!");
                    LoadData();
                    return;
                }
            }
        }
  

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            if (dtMedis == null) return;

            DataRow newRow = dtMedis.NewRow();

            newRow["SEQ"] = ((gvMedisPeriksa.RowCount) + 1).ToString();
            //newRow["HEAD_ID"] = headid;
            newRow["ID_VISIT"] = visitid;
            newRow["ACTION"] = "I";
            dtMedis.Rows.Add(newRow);

            gridMedisPeriksa.DataSource = dtMedis;
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (gvMedisPeriksa.RowCount > 0)
            //    {
            //        DataTable dt = ORADB.SetData(ORADB.XE, "Select * from KLINIK.cs_treatment_detail where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvMedis' ");
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            ORADB.Execute(ORADB.XE, " insert into KLINIK.cs_treatment_detail_del select a.*, sysdate, '" + DB.vUserId + "' as emp from KLINIK.cs_treatment_detail a  where  HEAD_ID = '" + headid + "'  and GRID_NAME = 'gvMedis' ");
            //            ORADB.Execute(ORADB.XE, " Delete from KLINIK.cs_treatment_detail  where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvMedis' ");
            //        } 
            //        string sql = "insert all ";
            //        for (int i = 0; i < gvMedisPeriksa.RowCount; i++)
            //        {
            //            string dte = "";
            //            object tgl = gvMedisPeriksa.GetRowCellValue(i, "TANGGAL");
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
            //            sql = sql + " into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME) values ( ";
            //            sql = sql + " CS_TREATMENT_DETAIL_SEQ.nextval ,'" + FN.strVal(gvMedisPeriksa, i, "HEAD_ID") + "','" + FN.strVal(gvMedisPeriksa, i, "TREAT_ITEM_ID") + "'  ,";
            //            sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd'), '" + FN.strVal(gvMedisPeriksa, i, "TREAT_QTY") + "', '" + FN.strVal(gvMedisPeriksa, i, "TREAT_ITEM_PRICE") + "', " + Convert.ToInt32(FN.strVal(gvMedisPeriksa, i, "TREAT_QTY")) * Convert.ToInt32(FN.strVal(gvMedisPeriksa, i, "TREAT_ITEM_PRICE")) + ", ";
            //            sql = sql + " '" + FN.strVal(gvMedisPeriksa, i, "REMARKS") + "' ,  sysdate, '" + DB.vUserId + "', '" + FN.strVal(gvMedisPeriksa, i, "JAM") + "' , 'gvMedis' )";
            //        }
            //        sql = sql + " select * from dual";
            //        bool save = ORADB.Execute(ORADB.XE, sql);
            //        if (save)
            //        {
            //            MessageBox.Show("Formulir CPPT disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    FN.errosMsg(ex.Message, "Error");
            //}


            try
            {
                if (gvMedisPeriksa.RowCount > 0)
                {
                    //DataTable dt = ORADB.SetData(ORADB.XE, "Select * from KLINIK.cs_treatment_detail where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvMedisPeriksa' ");
                    //if (dt != null && dt.Rows.Count > 0)
                    //{
                    //    ORADB.Execute(ORADB.XE, " insert into KLINIK.cs_treatment_detail_del select a.*, sysdate, '" + DB.vUserId + "' as emp from KLINIK.cs_treatment_detail a  where  HEAD_ID = '" + headid + "'  and GRID_NAME = 'gvMedisPeriksa' ");
                    //    ORADB.Execute(ORADB.XE, " Delete from KLINIK.cs_treatment_detail  where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvMedisPeriksa' ");
                    //}

                    //string sql = "insert all ";
                    //for (int i = 0; i < gvMedisPeriksa.RowCount; i++)
                    //{
                    //    string dte = "";
                    //    object tgl = gvMedisPeriksa.GetRowCellValue(i, "TANGGAL");
                    //    if (tgl != null && tgl is DateTime)
                    //    {
                    //        DateTime selectedDateTime = (DateTime)tgl;
                    //        dte = selectedDateTime.ToString("dd/MM/yyyy");
                    //    }
                    //    else
                    //    {
                    //        DateTime selectedDateTime = DateTime.Now;
                    //        dte = selectedDateTime.ToString("dd/MM/yyyy");
                    //    }

                    //    //                    command.CommandText = " insert into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp) values
                    //    //  ( '" + seq_val + "', '" + head + "', '" + nama_laya + "', to_date('" + ldate + "', 'dd/MM/yyyy'), " + qty + ", " + item_price + ", " + price + ", '" + remarks + "', sysdate, '" + DB.vUserId + "') ";
                    //    //                    command.ExecuteNonQuery();

                    //    sql = sql + " into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME) values ( ";
                    //    sql = sql + " CS_TREATMENT_DETAIL_SEQ.nextval ,'" + FN.strVal(gvMedisPeriksa, i, "HEAD_ID") + "','" + FN.strVal(gvMedisPeriksa, i, "TREAT_ITEM_ID") + "'  ,";
                    //    sql = sql + " TO_DATE('" + dte + "', 'dd/MM/yyyy'), '" + FN.strVal(gvMedisPeriksa, i, "TREAT_QTY") + "', '" + FN.strVal(gvMedisPeriksa, i, "TREAT_ITEM_PRICE") + "', " + Convert.ToInt32(FN.strVal(gvMedisPeriksa, i, "TREAT_QTY")) * Convert.ToInt32(FN.strVal(gvMedisPeriksa, i, "TREAT_ITEM_PRICE")) + ", ";
                    //    sql = sql + " '" + FN.strVal(gvMedisPeriksa, i, "REMARKS") + "' ,  sysdate, '" + DB.vUserId + "', '" + FN.strVal(gvMedisPeriksa, i, "JAM") + "' , 'gvMedisPeriksa' )";
                    //}
                    //sql = sql + " select * from dual";
                    //bool save = ORADB.Execute(ORADB.XE, sql);
                    //if (save)
                    //{
                    //    MessageBox.Show("Formulir CPPT disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}

                    if (txt_rekammds.Text.ToString().Equals(""))
                    {
                        MessageBox.Show("Silahkan Tentukan Pasien. No Rekam Medis tidak boleh kosong...!!");
                        return;
                    }


                    string date = "", que = "", rm_no = "", pasno = "", nama_laya = "", status = "", remark = "", action = "", stbyr = "", insu_flag = "", pid_visit = "", headid ="", tplan = "";
                    string sql_cnt = "", diag_cnt = "", sql_update = "";

                    
                    que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                    rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();
                    pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                    insu_flag = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[7]).ToString();
                    headid = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[20]).ToString();
                    stbyr = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[22]).ToString();
                    tplan = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[3]).ToString();

                    if (!insu_flag.ToString().Equals("BPJS"))
                        insu_flag = "U";
                    else
                        insu_flag = "B";

                    status = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
                    pid_visit = visitid;

                    for (int i = 0; i < gvMedisPeriksa.DataRowCount; i++)
                    {
                        nama_laya = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[3]).ToString();
                        date = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[1]).ToString();
                        //status = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[7]).ToString();
                        remark = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[5]).ToString();
                        action = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[10]).ToString();
                        //stbyr = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[12]).ToString();
                        //insu_flag = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[11]).ToString();

                        if (nama_laya == "")
                        {
                            MessageBox.Show("Nama Layanan harus diisi");
                        }
                        //else if (stbyr != "OPN")
                        //{
                        //    MessageBox.Show("Data tidak bisa ditambah");
                        //}
                        else
                        {
                            if (action == "I")
                            {
                                string head = "", detail = "", ldate = "", ljam = "", qty = "", price = "", remarks = "";

                                sql_cnt = " select count(0) cnt, max(HEAD_ID) HEAD_ID from KLINIK.cs_treatment_head where ID_VISIT = " + pid_visit + "  and status = 'OPN' ";
                                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                                DataTable dt = new DataTable();
                                adOra.Fill(dt);
                                diag_cnt = dt.Rows[0]["cnt"].ToString();

                                detail = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[0]).ToString();
                                nama_laya = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[3]).ToString();
                                ldate = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[1]).ToString();
                                ljam = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[2]).ToString();
                                qty = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[4]).ToString();
                                price = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[9]).ToString();
                                remarks = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[5]).ToString();
                                action = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[10]).ToString();
                                stbyr = gvMedisPeriksa.GetRowCellValue(i, gvMedisPeriksa.Columns[12]).ToString();

                                if (Convert.ToInt32(diag_cnt) > 0)
                                { 
                                    
                                    head = dt.Rows[0]["HEAD_ID"].ToString(); 

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
                                            sql_cnt = " select count(0) cnt from KLINIK.cs_treatment_detail where head_id = '" + head + "' and to_char(treat_date,'yyyy-mm-dd') = '" + ldate + "' and treat_item_id = '" + nama_laya + "' ";
                                            OleDbConnection oraConnect7 = ConnOra.Create_Connect_Ora();
                                            OleDbDataAdapter adOra7 = new OleDbDataAdapter(sql_cnt, oraConnect7);
                                            DataTable dt7 = new DataTable();
                                            adOra7.Fill(dt7);
                                            diag_cnt = dt7.Rows[0]["cnt"].ToString();
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

                                                    command.CommandText = " insert into KLINIK.cs_treatment_detail  (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME) values ( '" + seq_val + "', '" + head + "', '" + nama_laya + "', to_date('" + ldate.ToString().Substring(0, 10) + "', 'yyyy-mm-dd'), " + qty + ", " + price + ", " + price + ", '" + remarks + "', sysdate, '" + DB.vUserId + "', '" + ljam + "', 'gvMedisPeriksa') ";
                                                    command.ExecuteNonQuery();

                                                    command.CommandText = " insert into KLINIK.cs_action (act_id, rm_no, insp_date, visit_dt, visit_no, detail_id, ins_date, ins_emp) values ( CS_ACTION_SEQ.nextval, '" + rm_no + "', to_date('" + ldate.ToString().Substring(0, 10) + "', 'yyyy-mm-dd'), to_date('" + date.ToString().Substring(0, 10) + "', 'yyyy-mm-dd'), '" + que + "', '" + seq_val + "', sysdate, '" + DB.vUserId + "') ";
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
                                                                        " set remarks = '" + remarks + "', ";
                                            sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                                            sql_update = sql_update + " where detail_id = '" + detail + "' ";

                                            try
                                            {
                                                OleDbConnection oraConnect8 = ConnOra.Create_Connect_Ora();
                                                OleDbCommand cm8 = new OleDbCommand(sql_update, oraConnect8);
                                                oraConnect8.Open();
                                                cm8.ExecuteNonQuery();
                                                oraConnect8.Close();
                                                cm8.Dispose();

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
                                else
                                {
                                    string sql_seq = "", seq_val = "", sql_tmp = "", sql_dtl = "";
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
                                        command.CommandText = " insert into KLINIK.cs_treatment_head (head_id, rm_no, patient_no, visit_date, visit_no, treat_type_id, status, remarks, pay_status, insu_flag, ins_date, ins_emp,ID_VISIT) values ('" + seq_val + "', '" + rm_no + "', '" + pasno + "', to_date('" + date.ToString().Substring(0,10) + "', 'yyyy-mm-dd'), '" + que + "', '" + tplan + "', 'OPN', '" + remark + "', 'OPN', '" + insu_flag + "', sysdate, '" + DB.vUserId + "', '" + pid_visit + "') ";
                                        command.ExecuteNonQuery();

                                        //string sql_seq = "", seq_val = "", sql_tmp = "";
                                        sql_seq = "";
                                        sql_seq = " select CS_TREATMENT_DETAIL_SEQ.nextval seq from dual ";
                                        OleDbConnection oraConnects = ConnOra.Create_Connect_Ora();
                                        OleDbDataAdapter adOras = new OleDbDataAdapter(sql_seq, oraConnects);
                                        DataTable dts = new DataTable();
                                        adOras.Fill(dts);
                                        sql_dtl = dts.Rows[0]["seq"].ToString();

                                        //OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                                        //OleDbCommand command = new OleDbCommand();
                                        //OleDbTransaction trans = null;

                                        //command.Connection = oraConnectTrans;
                                        //oraConnectTrans.Open();

                                        try
                                        {
                                            //trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                                            //command.Connection = oraConnectTrans;
                                            //command.Transaction = trans;

                                            command.CommandText = " insert into KLINIK.cs_treatment_detail  (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME) values ( '" + sql_dtl + "', '" + seq_val + "', '" + nama_laya + "', to_date('" + date.ToString().Substring(0, 10) + "', 'yyyy-mm-dd'), " + qty + ", " + price + ", " + price + ", '" + remarks + "', sysdate, '" + DB.vUserId + "', '" + ljam + "', 'gvMedisPeriksa') ";
                                            command.ExecuteNonQuery();


                                            //sql_tmp = "";
                                            //sql_tmp = sql_tmp + "insert into KLINIK.cs_treatment_detail ";
                                            //sql_tmp = sql_tmp + "select  " + sql_dtl + "  det_id, " + seq_val + " head_id,  b.treat_item_id, to_date('" + date.ToString().Substring(0, 10) + "', 'yyyy-mm-dd') visit_date, ";
                                            //sql_tmp = sql_tmp + "1 treat_qty, 'Initial' remark, sysdate ins_date, '" + DB.vUserId + "' ins_emp, ";
                                            //sql_tmp = sql_tmp + "null upd_date, null upd_emp, b.treat_item_price, b.treat_item_price total_price, TO_CHAR(sysdate,'HH24:MI') jam, 'gvMedisPeriksa' GRID_NAME, '" + DB.vUserId + "' ID_DOKTER, null att1, null att2 ";
                                            //sql_tmp = sql_tmp + "from KLINIK.cs_treatment_type a ";
                                            //sql_tmp = sql_tmp + "join KLINIK.cs_treatment_item b on (a.treat_type_id=b.treat_type_id) ";
                                            //sql_tmp = sql_tmp + "join KLINIK.cs_treatment_group c on (b.treat_group_id=c.treat_group_id) ";
                                            //sql_tmp = sql_tmp + "where 1=1";
                                            //sql_tmp = sql_tmp + "and b.TREAT_ITEM_ID = '" + nama_laya + "' ";

                                            //command.CommandText = sql_tmp;
                                            //command.ExecuteNonQuery();

                                            command.CommandText = " insert into KLINIK.cs_action (act_id, rm_no, insp_date, visit_dt, visit_no, detail_id, ins_date, ins_emp) values ( CS_ACTION_SEQ.nextval, '" + rm_no + "', to_date('" + date.ToString().Substring(0, 10) + "', 'yyyy-mm-dd'), to_date('" + date.ToString().Substring(0, 10) + "', 'yyyy-mm-dd'), '" + que + "', '" + sql_dtl + "', sysdate, '" + DB.vUserId + "') ";
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


                                        //if (!nama_laya.ToString().Equals("TRT01"))
                                        //{
                                        //    //command.CommandText = " update KLINIK.cs_visit set status = 'MED', time_inspection=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + pasno + "' and ID_VISIT =" + pid_visit + " "; // and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                                        //    //command.ExecuteNonQuery();
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

                                        //    command.CommandText = " insert into cs_inpatient (inpatient_id, rm_no,  reg_date, status,   date_in,    ins_date, ins_emp) values ('" + seq_val2 + "', '" + rm_no + "', to_date('" + date.ToString().Substring(0, 10) + "','yyyy-mm-dd'), '" + status + "',   to_date('" + date.ToString().Substring(0, 10) + "','yyyy-mm-dd'),   sysdate, '" + DB.vUserId + "') ";
                                        //    command.ExecuteNonQuery();
                                        //}


                                        //sql_tmp = "";
                                        //sql_tmp = sql_tmp + "insert into KLINIK.cs_treatment_detail ";
                                        //sql_tmp = sql_tmp + "select CS_TREATMENT_DETAIL_SEQ.nextval det_id, " + seq_val + " head_id,  b.treat_item_id, to_date('" + date.ToString().Substring(0, 10) + "', 'yyyy-mm-dd') visit_date, ";
                                        //sql_tmp = sql_tmp + "1 treat_qty, 'Initial' remark, sysdate ins_date, '" + DB.vUserId + "' ins_emp, ";
                                        //sql_tmp = sql_tmp + "null upd_date, null upd_emp, b.treat_item_price, b.treat_item_price total_price, TO_CHAR(sysdate,'HH24:MI') jam, 'gvMedisPeriksa' GRID_NAME, '" + DB.vUserId + "' ID_DOKTER, null att1, null att2 ";
                                        //sql_tmp = sql_tmp + "from KLINIK.cs_treatment_type a ";
                                        //sql_tmp = sql_tmp + "join KLINIK.cs_treatment_item b on (a.treat_type_id=b.treat_type_id) ";
                                        //sql_tmp = sql_tmp + "join KLINIK.cs_treatment_group c on (b.treat_group_id=c.treat_group_id) ";
                                        //sql_tmp = sql_tmp + "where 1=1";  
                                        //sql_tmp = sql_tmp + "and b.TREAT_ITEM_ID = '" + nama_laya + "' ";

                                        //command.CommandText = sql_tmp;
                                        //command.ExecuteNonQuery();

                                        //trans.Commit();
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

                                //if (insu_flag != lTinTipe.Text)
                                //{
                                //    MessageBox.Show("Data Tipe Pasien pada menu reservasi dan tagihan tidak sama");
                                //    LoadTind();
                                //    LoadAddTind();
                                //    return;
                                //}

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

                                    //MessageBox.Show("Query Exec : " + sql_update);

                                    MessageBox.Show("Data Berhasil diupdate");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("ERROR: " + ex.Message);
                                }
                            }
                            //LoadTind();
                            //LoadAddTind();

                        }
                    }

                    ListDataLayanan(pid_visit);
                }
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            FrmTindakan = new FrmTindakan();
            FrmTindakan.p_anamnesa_id = v_ptnumber;
            FrmTindakan.prekam_medis = txt_rekammds.Text;
            FrmTindakan.pnama = pnama_pasien;
            //FrmTindakan.MdiParent = this;
            //ReportForm.DB.vUserId = userEmpid;
            FrmTindakan.ShowDialog();
            FrmTindakan.Focus();
        }

        private void btnCetak2_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string p_pasno = "", p_date = "";

            if (gridView1.RowCount > 0)
            {
                p_pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
                p_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();

                SQL = "";
                SQL = SQL + Environment.NewLine + "select c.name, round(((sysdate-c.birth_date)/30)/12) age, c.address,   ";
                SQL = SQL + Environment.NewLine + "to_char(date_in,'fmdd Month yyyy', 'nls_date_language = INDONESIAN') bgndt, ";
                SQL = SQL + Environment.NewLine + "to_char(date_out,'fmdd Month yyyy', 'nls_date_language = INDONESIAN') enddt, ";
                SQL = SQL + Environment.NewLine + "a.letter_no, e.item_name d_name, c.company, c.company_addr, ";
                SQL = SQL + Environment.NewLine + "to_char(sysdate,'fmdd Month yyyy', 'nls_date_language = INDONESIAN') as ddate  ";
                SQL = SQL + Environment.NewLine + "from cs_inpatient a   ";
                SQL = SQL + Environment.NewLine + "join cs_visit b on (a.inpatient_id=b.inpatient_id)   ";
                SQL = SQL + Environment.NewLine + "join cs_patient_info c on (b.patient_no=c.patient_no)   ";
                SQL = SQL + Environment.NewLine + "join cs_diagnosa d on (trunc(b.visit_date)=d.visit_dt and b.que01=d.visit_no)  ";
                SQL = SQL + Environment.NewLine + "join cs_diagnosa_item e on (d.item_cd=e.item_cd) ";
                SQL = SQL + Environment.NewLine + "where 1=1  ";
                SQL = SQL + Environment.NewLine + "and a.status not in ('CAN')  ";
                SQL = SQL + Environment.NewLine + "and b.patient_no = '" + p_pasno + "'  ";
                SQL = SQL + Environment.NewLine + "and to_char(visit_date,'yyyy-mm-dd') = '" + p_date + "'  ";

                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dsKetRanap.Tables.Clear();
                    dsKetRanap.Tables.Add(dt);

                    ReportKetRanap report2 = new ReportKetRanap(dsKetRanap);
                    report2.ShowPreviewDialog();
                }
                else
                {
                    MessageBox.Show("Data diagnosa harus diisi");
                } 
            }
        }

        private bool CheckOpened(string name)
        {
            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                if (frm.Text == name)
                {
                    return true;
                }
            }
            return false;
        }

        #region Controls Actions
         
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
        #endregion
    }
}
