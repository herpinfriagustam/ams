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
using System.Web;
using NAudio.Wave;
using System.Net;
using Clinic.Report;
using DevExpress.XtraReports.UI;

namespace Clinic
{
    public partial class ReservationMngt6 : DevExpress.XtraEditors.XtraForm
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

        DataSet dsAgree = new DataSet();
        DataSet dsKetRanap = new DataSet();

        RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
        ObsNotif obsNotif = null;
        RsvNotif rsvNotif = null;

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string workingDirectory = "", resourcesDirectory = "";
        string upd_col = "", s_policd = "";
        int obst = 0, popup_interval = 999900;

        public ReservationMngt6()
        {
            InitializeComponent();
        }

        private void ReservationInput_Load(object sender, EventArgs e)
        {
            btnSaveAnam.Enabled = false;
            btnAddAnam.Enabled = false;
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
            initData();
            LoadData();
            //tableLayoutPanel1.RowStyles[4] = new RowStyle(SizeType.Absolute, 0);
            //tableLayoutPanel1.RowStyles[5] = new RowStyle(SizeType.Absolute, 0);

        }

        private void initData()
        {
            string sql_poli = " select poli_cd, poli_name from cs_policlinic where status = 'A' and poli_cd not in ('POL0004') ";
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
            listPatientType.Add(new PatientType() { patientTypeCode = "P", patientTypeName = "Perusahaan" });

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
            listStat.Add(new Status() { statusCode = "OBS", statusName = "Observation" });
            //listStat.Add(new Status() { statusCode = "MED", statusName = "Medicine" });
            //listStat.Add(new Status() { statusCode = "CLS", statusName = "Completed" });
            listStat.Add(new Status() { statusCode = "HOL", statusName = "Hold" });
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
            SQL2 = SQL2 + Environment.NewLine + "select patient_no, name from cs_patient_info ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(SQL2, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            listPatient.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listPatient.Add(new Patient() { patientCode = dt.Rows[i]["patient_no"].ToString(), patientName = dt.Rows[i]["name"].ToString() });

            }
        }

        private void LoadData()
        {
            string sql_search;

            sql_search = " ";

            sql_search = sql_search + Environment.NewLine + "SELECT   que01, a.patient_no, a.patient_no pasno, null dept, b.gender,  ";
            sql_search = sql_search + Environment.NewLine + "         round(((sysdate-b.birth_date)/30)/12) age,   ";
            sql_search = sql_search + Environment.NewLine + "         a.poli_cd, type_patient, work_accident, purpose, a.status, 'S' action,  ";
            sql_search = sql_search + Environment.NewLine + "         CASE WHEN observation = 'Y' THEN 'Yes' ELSE 'No'  ";
            sql_search = sql_search + Environment.NewLine + "         END AS observation, visit_remark,  ";
            sql_search = sql_search + Environment.NewLine + "         (SELECT MAX (rm_no)  ";
            sql_search = sql_search + Environment.NewLine + "            FROM cs_patient  ";
            sql_search = sql_search + Environment.NewLine + "           WHERE status = 'A'  ";
            sql_search = sql_search + Environment.NewLine + "             AND group_patient = c.poli_group  ";
            sql_search = sql_search + Environment.NewLine + "             AND patient_no = a.patient_no) AS rm_no,  ";
            sql_search = sql_search + Environment.NewLine + "         DECODE (c.poli_group, 'PREG', 'Ibu Hamil', 'FAMP', 'KB', 'Umum' ) AS type_mr,  ";
            sql_search = sql_search + Environment.NewLine + "         a.poli_cd, round((nvl(start_hold,sysdate)-visit_date) * 24 * 60) wait_time  ";
            sql_search = sql_search + Environment.NewLine + "    FROM cs_visit a JOIN cs_patient_info b ON a.patient_no = b.patient_no  ";
            sql_search = sql_search + Environment.NewLine + "         LEFT JOIN cs_policlinic c ON (a.poli_cd = c.poli_cd AND c.status = 'A')  ";
            sql_search = sql_search + Environment.NewLine + "   WHERE 1 = 1  ";
            sql_search = sql_search + Environment.NewLine + "     AND TO_CHAR (visit_date, 'yyyy-mm-dd') = '" + today + "'  ";
            sql_search = sql_search + Environment.NewLine + "     AND a.poli_cd not in ('POL0004')  ";
            sql_search = sql_search + Environment.NewLine + "     AND a.status IN ('PRE', 'RSV', 'NUR', 'INS', 'OBS', 'HOL')  ";
            sql_search = sql_search + Environment.NewLine + "ORDER BY a.ins_date  ";

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
                gridView1.Columns[11].OptionsColumn.AllowEdit = false;
                gridView1.Columns[12].OptionsColumn.AllowEdit = false;
                gridView1.Columns[13].OptionsColumn.AllowEdit = false;
                gridView1.Columns[14].OptionsColumn.AllowEdit = false;
                gridView1.Columns[15].OptionsColumn.AllowEdit = false;
                gridView1.Columns[16].OptionsColumn.AllowEdit = false;
                gridView1.Columns[17].OptionsColumn.AllowEdit = false;
                gridView1.Columns[0].OptionsColumn.AllowEdit = false;

                gridView1.Columns[0].Caption = "Antrian";
                gridView1.Columns[1].Caption = "Pasien No";
                gridView1.Columns[2].Caption = "Nama";
                gridView1.Columns[3].Caption = "Dept";
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

                gridView1.Columns[6].MinWidth = 70;
                gridView1.Columns[6].MinWidth = 70;
                gridView1.Columns[7].MinWidth = 70;
                gridView1.Columns[7].MinWidth = 70;
                gridView1.Columns[10].MinWidth = 80;
                gridView1.Columns[10].MinWidth = 80;
                gridView1.Columns[17].Width = 50;

                gridView1.Columns[17].VisibleIndex = 6;

                //PRE, RSV, NUR, INS, OBS, MED, CLS, CAN

                RepositoryItemGridLookUpEdit glPatient = new RepositoryItemGridLookUpEdit();
                glPatient.DataSource = listPatient;
                glPatient.ValueMember = "patientCode";
                glPatient.DisplayMember = "patientName";

                glPatient.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glPatient.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                glPatient.ImmediatePopup = true;
                glPatient.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glPatient.NullText = "";
                gridView1.Columns[2].ColumnEdit = glPatient;

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

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            GridView View = sender as GridView;
            string s_rm = "", s_que = "", s_poli = "", s_group = "", s_rmno = "", group = "", s_nik = "", s_nama = "", s_berobat = "";

            s_rm = View.GetRowCellDisplayText(e.RowHandle, View.Columns[14]);
            s_que = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            s_nik = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
            s_nama = View.GetRowCellDisplayText(e.RowHandle, View.Columns[2]);
            s_poli = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);
            s_berobat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);
            s_rmno = View.GetRowCellDisplayText(e.RowHandle, View.Columns[14]);
            s_group = View.GetRowCellDisplayText(e.RowHandle, View.Columns[15]);
            s_policd = View.GetRowCellDisplayText(e.RowHandle, View.Columns[16]);

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

            string sql_anam = "";
            sql_anam = " select to_char(insp_date,'yyyy-mm-dd') as insp_date, '" + s_nama + "' as nama, visit_no, " +
                       " blood_press, pulse, temperature, allergy, anamnesa, info_k, 'S' action, rm_no, bb, tb, " +
                       " cholesterol, blood_sugar, uric_acid, disease_now, disease_then, disease_family, anamnesa_physical, anamnesa_other " +
                       " from cs_anamnesa where rm_no = '" + s_rm + "' and to_char(insp_date,'yyyy-mm-dd') = '" + today + "' and visit_no = '" + s_que + "' ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_anam, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl2.DataSource = null;
            gridView2.Columns.Clear();
            gridControl2.DataSource = dt;

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
            gridView2.Columns[16].Caption = "R.Sekarang";
            gridView2.Columns[17].Caption = "R.Dulu";
            gridView2.Columns[18].Caption = "R.Keluarga";
            gridView2.Columns[19].Caption = "Pem.Fisik";
            gridView2.Columns[20].Caption = "Pem.Lain";

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

            string sql_addinfo = "", sql_info = "", p_col = "";

            sql_addinfo = " select info_cd, description from cs_add_info where status = 'A' and poli_cd = '" + s_policd + "' ";

            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_addinfo, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                p_col = p_col + ", " + dt2.Rows[i]["info_cd"].ToString();
            }

            if (s_group == "Umum")
            {
                group = "COMM";
            }
            else if (s_group == "KB")
            {
                group = "FAMP";
            }
            else
            {
                group = "PREG";
            }

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



            string sql_cek_hold = "", temp_shold = "", temp_ehold = "";

            sql_cek_hold = " select to_char(start_hold,'yyyy-mm-dd') s_hold, to_char(end_hold,'yyyy-mm-dd') e_hold from cs_visit where patient_no = '" + s_nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + today + "' and que01 = '" + s_que + "' ";

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
            gridControl2.DataSource = null;
            btnAddAnam.Enabled = false;
            btnSaveAnam.Enabled = false;
            btnTunda.Enabled = false;
            btnLanjut.Enabled = false;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string fname = ".wav", p_que = "", p1 = "", p2 = "", p3 = "", p4 = "", p_dir = "", s_gender = "", s_name = "", urltts = "", teks = "";

            //p_dir = resourcesDirectory;
            p_dir = "C:\\KLINIK\\";

            p_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_gender = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
            s_name = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();

            p1 = p_que.Substring(0, 1);
            p2 = p_que.Substring(1, 1);
            p3 = p_que.Substring(2, 1);
            p4 = p_que.Substring(3, 1);

            if (s_gender == "Perempuan")
            {
                p1 = "Ibu ";
            }
            else
            {
                p1 = "Bapak ";
            }

            p2 = s_name;

            teks = p1 + p2 + " silahkan menuju ke konter perawat";

            loading.ShowWaitForm();
            try
            {
                urltts = string.Format("https://translate.googleapis.com/translate_tts?ie=UTF-8&q={0}&tl={1}&total=1&idx=0&textlen={2}&client=gtx", HttpUtility.UrlEncode(teks, Encoding.GetEncoding("utf-8")), "id" + "-gb&q=", teks.Length);
                PlayMp3FromUrl(urltts);

                //SoundPlayer player = new SoundPlayer(p_dir + "antrian" + fname);
                //SoundPlayer player2 = new SoundPlayer(p_dir + p1 + fname);
                //SoundPlayer player3 = new SoundPlayer(p_dir + "_" + p2 + fname);
                //SoundPlayer player4 = new SoundPlayer(p_dir + "_" + p3 + fname);
                //SoundPlayer player5 = new SoundPlayer(p_dir + "_" + p4 + fname);
                //SoundPlayer player6 = new SoundPlayer(p_dir + "IN" + fname);
                //player.PlaySync();
                ////Thread.Sleep(2000);
                //player2.PlaySync();
                ////Thread.Sleep(900);
                //player3.PlaySync();
                ////Thread.Sleep(900);
                //player4.PlaySync();
                ////Thread.Sleep(900);
                //player5.PlaySync();
                //Thread.Sleep(900);
                //player6.PlaySync();
                //Thread.Sleep(2000);

                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }


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
                    if (Convert.ToInt16(wt) >= 60)
                    {
                        e.Appearance.BackColor = Color.Red;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (Convert.ToInt16(wt) >= 40 && Convert.ToInt16(wt) < 60)
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
                else if (kk == "Preparation")
                {
                    e.Appearance.BackColor = Color.OldLace;
                    e.Appearance.ForeColor = Color.Black;
                }
            }

            if (e.Column.Caption == "Poli")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
                //e.Appearance.FontStyleDelta = FontStyle.Bold;
            }

            if (e.Column.Caption == "Berobat")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
                //e.Appearance.FontStyleDelta = FontStyle.Bold;
            }

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
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns[10], "RSV");
            view.SetRowCellValue(e.RowHandle, view.Columns[9], "DOC");
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.Caption == "Nama")
            {
                string p_empid = e.Value.ToString();
                string empid = "", name = "", dept = "", gender = "", age = "";
                string sql_emp = " select patient_no, name, null dept, gender, round(((sysdate-birth_date)/30)/12) age from cs_patient_info where 1 = 1 and patient_no = '" + p_empid + "' ";

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
                }
                else
                {
                    empid = ""; dept = ""; gender = ""; age = "";
                    view.SetColumnError(gridView1.Columns[1], "Employees Not Found");
                }

                view.SetRowCellValue(e.RowHandle, view.Columns[1], name);
                view.SetRowCellValue(e.RowHandle, view.Columns[3], dept);
                view.SetRowCellValue(e.RowHandle, view.Columns[4], gender);
                view.SetRowCellValue(e.RowHandle, view.Columns[5], age);

                view.SetRowCellValue(e.RowHandle, view.Columns[7], "U");
                view.SetRowCellValue(e.RowHandle, view.Columns[8], "N");
                view.SetRowCellValue(e.RowHandle, view.Columns[10], "PRE");
                view.SetRowCellValue(e.RowHandle, view.Columns[11], "I");
            }

            if (e.Column.Caption == "Poli")
            {
                string tmp_nik = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string tmp_grp = "";
                string tmp_poli = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                string tmp_rm = "", sql = "", sql2 = "", purpose = "", sql3 = "", rmk = "";
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();

                if (tmp_poli == "POL0001" || tmp_poli == "POL0000")
                {
                    tmp_grp = "COMM";
                }
                else if (tmp_poli == "POL0002")
                {
                    tmp_grp = "PREG";
                }
                else if (tmp_poli == "POL0003")
                {
                    tmp_grp = "FAMP";
                }
                else
                {
                    tmp_grp = "COMM";
                }

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

                sql2 = " select poli_pic from cs_policlinic where poli_cd = '" + tmp_poli + "'  and status = 'A'  ";
                OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra3 = new OleDbDataAdapter(sql2, oraConnect3);
                DataTable dt3 = new DataTable();
                adOra3.Fill(dt3);

                if (dt3.Rows.Count > 0)
                {
                    purpose = dt3.Rows[0]["poli_pic"].ToString();
                }
                else
                {
                    purpose = "";
                }

                //if (tmp_poli == "POL0001" || tmp_poli == "POL0000" || tmp_poli == "POL0004")
                //{
                //    view.SetRowCellValue(e.RowHandle, view.Columns[9], "DOC");
                //}
                //else
                //{
                //    view.SetRowCellValue(e.RowHandle, view.Columns[9], "MID");
                //}

                sql3 = " select attr_06 from cs_code_data where code_class_id = 'RESV_ITEM' and attr_03 = '" + tmp_poli + "'  and status = 'A'  ";
                OleDbConnection oraConnect4 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra4 = new OleDbDataAdapter(sql3, oraConnect4);
                DataTable dt4 = new DataTable();
                adOra4.Fill(dt4);

                if (dt4.Rows.Count > 0)
                {
                    rmk = dt4.Rows[0]["attr_06"].ToString();
                }
                else
                {
                    rmk = "";
                }

                view.SetRowCellValue(e.RowHandle, view.Columns[9], purpose);
                view.SetRowCellValue(e.RowHandle, view.Columns[10], "RSV");
                view.SetRowCellValue(e.RowHandle, view.Columns[13], rmk);
            }

            if (e.Column.Caption == "Poli" || e.Column.Caption == "Pasien" || e.Column.Caption == "KK" || e.Column.Caption == "Berobat" || e.Column.Caption == "Status" || e.Column.Caption == "Remark" || e.Column.Caption == "Rencana")
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

            if (e.Column.Caption == "Tensi" || e.Column.Caption == "Nadi" || e.Column.Caption == "Suhu" || e.Column.Caption == "BB (Kg)" || e.Column.Caption == "TB (Cm)" || e.Column.Caption == "Alergi" || e.Column.Caption == "Keluhan Utama" || e.Column.Caption == "Riwayat")



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
            string que = "", nik = "", nama = "", poli = "", pasien = "", workA = "", purpose = "", status = "", action = "", cek = "", remark = "";
            string sql_check = "", sql_cnt = "", sql_insert = "", sql_update = "", c_que = "", tmp_queue = "", visit_cnt = "", rm = "";
            int queue = 0, visit = 0;
            cek = "";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                que = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                nik = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                nama = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                poli = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                pasien = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();
                workA = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();
                purpose = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                status = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                action = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
                remark = gridView1.GetRowCellValue(i, gridView1.Columns[13]).ToString();
                rm = gridView1.GetRowCellValue(i, gridView1.Columns[14]).ToString();

                if (action == "I")
                {
                    if (nama == "")
                    {
                        MessageBox.Show("Data pasien tidak ditemukan");
                    }
                    else if (purpose == "")
                    {
                        MessageBox.Show("Tujuan Berobat harus diisi");
                    }
                    else
                    {
                        if (purpose == "DOC")
                        {
                            c_que = "D";
                        }
                        else if (purpose == "MID")
                        {
                            c_que = "M";
                        }
                        else
                        {
                            c_que = "E";
                        }

                        sql_check = " select  nvl(max(to_number(substr(que01,2,3))),0) que from cs_visit where to_char(visit_date,'yyyy-mm-dd')= to_char(sysdate,'yyyy-mm-dd') and purpose = '" + purpose + "' ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_check, oraConnect);
                            DataTable dt = new DataTable();
                            adOra.Fill(dt);

                            tmp_queue = dt.Rows[0]["que"].ToString();
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

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }

                        sql_cnt = " select count(patient_no) cnt from cs_visit where patient_no = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd')= to_char(sysdate,'yyyy-mm-dd') and status not in ('CLS','CAN') ";
                        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_cnt, oraConnect2);
                        DataTable dt2 = new DataTable();
                        adOra2.Fill(dt2);
                        visit_cnt = dt2.Rows[0]["cnt"].ToString();
                        if (Convert.ToInt32(visit_cnt) > 0)
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

                            visit = Convert.ToInt32(visit_cnt) + 1;

                            //cek = cek + sql_insert;
                            try
                            {
                                trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                                command.Connection = oraConnectTrans;
                                command.Transaction = trans;

                                command.CommandText = " insert into cs_visit (patient_no, visit_date, status, poli_cd, type_patient, work_accident, purpose, visit_remark, visit_cnt, que01, plan, ins_date, ins_emp) values ('" + nik + "',sysdate, '" + status + "', '" + poli + "', '" + pasien + "','" + workA + "', '" + purpose + "', '" + remark + "', '" + Convert.ToString(visit) + "', '" + c_que + que + "' , 'TRT02', sysdate, '" + v_empid + "') ";
                                command.ExecuteNonQuery();

                                if (poli == "POL0002" || poli == "POL0003")
                                {

                                }
                                else
                                {
                                    command.CommandText = " insert into cs_anamnesa (anamnesa_id, rm_no, insp_date, visit_no, ins_date, ins_emp) values(cs_anamnesa_seq.nextval, '" + rm + "', trunc(sysdate), '" + c_que + que + "', sysdate, '" + v_empid + "') ";
                                    command.ExecuteNonQuery();
                                }

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

                    string tmp_stat = "", tmp_shold = "";

                    string sql_tmp_status = "";

                    sql_tmp_status = " select status, to_char(start_hold,'yyyy-mm-dd') s_hold from cs_visit where patient_no = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + today + "' and que01 = '" + que + "' ";

                    OleDbConnection sqlConnecta = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSqla = new OleDbDataAdapter(sql_tmp_status, sqlConnecta);
                    DataTable dta = new DataTable();
                    adSqla.Fill(dta);

                    if (dta.Rows.Count > 0)
                    {
                        tmp_stat = dta.Rows[0]["status"].ToString();
                        tmp_shold = dta.Rows[0]["s_hold"].ToString();

                        if (tmp_stat == "HOL")
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[10], "HOL");
                            MessageBox.Show("Untuk merubah status Hold, silahkan klik tombol lanjut");
                            LoadData();
                            return;
                        }
                        //else
                        //{
                        //    view.SetRowCellValue(e.RowHandle, view.Columns[10], tmp_stat2);
                        //}
                    }

                    sql_update = "";

                    sql_update = sql_update + " update cs_visit " +
                                 " set poli_cd = '" + poli + "', type_patient = '" + pasien + "', " +
                                 " work_accident = '" + workA + "', purpose = '" + purpose + "', visit_remark = '" + remark + "', status = '" + status + "', ";
                    if (status == "INS")
                    {
                        sql_update = sql_update + " time_reservation = sysdate, ";
                    }
                    sql_update = sql_update + " upd_emp = '" + v_empid + "', upd_date = sysdate ";
                    sql_update = sql_update + " where que01 = '" + que + "' and patient_no = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + today + "' ";

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
            LoadData();
        }


        private void btnSaveAnam_Click(object sender, EventArgs e)
        {
            string date = "", que = "", tensi = "", nadi = "", suhu = "", alergi = "", keluhan = "", action = "", rm_no = "", nik = "", infok = "", bb = "", tb = "";
            string chol = "", bsugar = "", uacid = "", r_now = "", r_then = "", r_fam = "", anam_physical = "", anam_other = "";
            string sql_update2 = "", sql_cnt = "", stat_rsv = "", sql_update = "", anam_cnt = "";

            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                date = gridView2.GetRowCellValue(i, gridView2.Columns[0]).ToString();
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
                r_now = gridView2.GetRowCellValue(i, gridView2.Columns[16]).ToString();
                r_then = gridView2.GetRowCellValue(i, gridView2.Columns[17]).ToString();
                r_fam = gridView2.GetRowCellValue(i, gridView2.Columns[18]).ToString();
                anam_physical = gridView2.GetRowCellValue(i, gridView2.Columns[19]).ToString();
                anam_other = gridView2.GetRowCellValue(i, gridView2.Columns[20]).ToString();
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
                        sql_cnt = " select count(0) cnt from cs_anamnesa where to_char(insp_date,'yyyy-mm-dd') = '" + today + "' and visit_no = '" + que + "' and rm_no = '" + rm_no + "' ";
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


                            //sql_insert = " insert into cs_anamnesa (anamnesa_id, rm_no, insp_date, blood_press, pulse, temperature, allergy, anamnesa, visit_no, ins_date, ins_emp) values (cs_anamnesa_seq.nextval, '" + rm_no + "', to_date('" + date + "','yyyy-mm-dd'), '" + tensi + "', '" + nadi + "','" + suhu + "', '" + alergi + "', '" + keluhan + "', '" + que + "', sysdate, '" + v_empid + "') ";

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
                                r_now = gridView2.GetRowCellValue(i, gridView2.Columns[16]).ToString();
                                r_then = gridView2.GetRowCellValue(i, gridView2.Columns[17]).ToString();
                                r_fam = gridView2.GetRowCellValue(i, gridView2.Columns[18]).ToString();
                                anam_physical = gridView2.GetRowCellValue(i, gridView2.Columns[19]).ToString();
                                anam_other = gridView2.GetRowCellValue(i, gridView2.Columns[20]).ToString();

                                trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                                command.Connection = oraConnectTrans;
                                command.Transaction = trans;

                                string SQL = "";
                                SQL = SQL + Environment.NewLine + "insert into cs_anamnesa ";
                                SQL = SQL + Environment.NewLine + "( ";
                                SQL = SQL + Environment.NewLine + "anamnesa_id, rm_no, insp_date, blood_press, pulse, temperature, allergy, anamnesa, visit_no, info_k, bb, tb, ";
                                SQL = SQL + Environment.NewLine + "cholesterol, blood_sugar, uric_acid, disease_now, disease_then, disease_family, anamnesa_physical, anamnesa_other, ";
                                SQL = SQL + Environment.NewLine + "ins_date, ins_emp ";
                                SQL = SQL + Environment.NewLine + ") ";
                                SQL = SQL + Environment.NewLine + "values  ";
                                SQL = SQL + Environment.NewLine + "( ";
                                SQL = SQL + Environment.NewLine + "cs_anamnesa_seq.nextval, '" + rm_no + "', to_date('" + date + "', 'yyyy-mm-dd'), '" + tensi + "', '" + nadi + "', '" + suhu;
                                SQL = SQL + Environment.NewLine + "', '" + alergi + "', '" + keluhan + "', '" + que + "', '" + infok + "','" + bb + "','" + tb;
                                SQL = SQL + Environment.NewLine + "', '" + chol + "', '" + bsugar + "', '" + uacid + "', '" + r_now + "','" + r_then + "','" + r_fam;
                                SQL = SQL + Environment.NewLine + "', '" + anam_physical + "', '" + anam_other;
                                SQL = SQL + Environment.NewLine + "', sysdate, '" + v_empid + "'  ";
                                SQL = SQL + Environment.NewLine + ") ";

                                command.CommandText = SQL;
                                command.ExecuteNonQuery();

                                command.CommandText = " update cs_visit set status = 'NUR', time_reservation=sysdate, upd_emp = '" + v_empid + "', upd_date = sysdate where patient_no = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
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
                                     " temperature = '" + suhu + "', allergy = '" + alergi + "', anamnesa = '" + keluhan + "', info_k = '" + infok + "',  " +
                                     " cholesterol = '" + chol + "', blood_sugar = '" + bsugar + "', uric_acid = '" + uacid + "', disease_now = '" + r_now + "',  " +
                                     " disease_then = '" + r_then + "', disease_family = '" + r_fam + "', anamnesa_physical = '" + anam_physical + "', anamnesa_other = '" + anam_other + "',  ";
                        sql_update = sql_update + " upd_emp = '" + v_empid + "', upd_date = sysdate ";
                        sql_update = sql_update + " where rm_no = '" + rm_no + "' and to_char(insp_date,'yyyy-mm-dd') = '" + date + "' and visit_no = '" + que + "' ";

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

                            sql_update2 = " update cs_visit set status = 'NUR', time_reservation=sysdate, upd_emp = '" + v_empid + "', upd_date = sysdate where patient_no = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";

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
            LoadData();
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
                sql_insert = " insert into cs_patient (rm_no, patient_no, group_patient, status, ins_date, ins_emp) values ('" + rm_no + "', '" + nik + "', '" + grp + "', 'A', sysdate, '" + v_empid + "') ";
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
                    btnCreate.Enabled = false;
                    MessageBox.Show("Data Berhasil disimpan.");
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

            sql_tmp_status = " select tmp_status from cs_visit where patient_no = '" + p_nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + today + "' and que01 = '" + p_que + "' ";

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
                sql_update = sql_update + " upd_emp = '" + v_empid + "', upd_date = sysdate ";
                sql_update = sql_update + " where que01 = '" + p_que + "' and patient_no = '" + p_nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + today + "' ";

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
            sql_update = sql_update + " upd_emp = '" + v_empid + "', upd_date = sysdate ";
            sql_update = sql_update + " where que01 = '" + p_que + "' and patient_no = '" + p_nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + today + "' ";

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
                    obsNotif.v_empid = v_empid;
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
                    rsvNotif.v_empid = v_empid;
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
                        obsNotif.v_empid = v_empid;
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
                        rsvNotif.v_empid = v_empid;
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
            //    upd_col = upd_col + " , upd_date=sysdate, upd_emp='" + v_empid + "' ";
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


    }
}
