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
using System.Windows.Forms;
using System.Data.OleDb;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using System.Diagnostics;
using System.Web;
using NAudio.Wave;
using System.Media;
using System.IO;
using System.Net;

namespace Clinic
{
    public partial class PrescriptionList : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<PatientType> listPatientType = new List<PatientType>();
        List<Status> listStat = new List<Status>();
        List<Status> listStat2 = new List<Status>();
        List<MedicineInfo> listMedicineInfo = new List<MedicineInfo>();
        List<Formula2> listFormula2 = new List<Formula2>();
        List<Medicine> listMedBpjs = new List<Medicine>();
        DataTable dtKir; 
        public string   v_name = "", visitid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        int timer = 0, timer2 = 0, cek_interval = 180;
        private LabelControl _currentLabel;
        string lsMSG = ""; int lsOK = 0; bool bl_klap = true;

        public PrescriptionList()
        {
            InitializeComponent();
        }

        private void initData()
        {

            listPatientType.Clear();
            listPatientType.Add(new PatientType() { patientTypeCode = "B", patientTypeName = "BPJS" });
            listPatientType.Add(new PatientType() { patientTypeCode = "U", patientTypeName = "Umum" });
            listPatientType.Add(new PatientType() { patientTypeCode = "A", patientTypeName = "Asuransi" });

            listStat.Clear();
            listStat.Add(new Status() { statusCode = "PRE", statusName = "Preparation" });
            listStat.Add(new Status() { statusCode = "RSV", statusName = "Registrasi" });
            listStat.Add(new Status() { statusCode = "NUR", statusName = "Pemeriksaan Awal" });
            listStat.Add(new Status() { statusCode = "INS", statusName = "Pemeriksaan" });
            listStat.Add(new Status() { statusCode = "INP", statusName = "Rawat Inap" });
            listStat.Add(new Status() { statusCode = "OBS", statusName = "Observasi" });
            listStat.Add(new Status() { statusCode = "MED", statusName = "Obat" });
            listStat.Add(new Status() { statusCode = "PAY", statusName = "Pembayaran" });
            listStat.Add(new Status() { statusCode = "DON", statusName = "Sudah Bayar" });
            listStat.Add(new Status() { statusCode = "CLS", statusName = "Selesai" });
            listStat.Add(new Status() { statusCode = "CAN", statusName = "Batal" });

            listStat2.Clear();
            listStat2.Add(new Status() { statusCode = "", statusName = "All" });
            listStat2.Add(new Status() { statusCode = "INP", statusName = "Rawat Inap" });
            listStat2.Add(new Status() { statusCode = "MED", statusName = "Obat" });
            listStat2.Add(new Status() { statusCode = "PAY", statusName = "Pembayaran" });
            listStat2.Add(new Status() { statusCode = "DON", statusName = "Sudah Bayar" });
            //listStat2.Add(new Status() { statusCode = "CLS", statusName = "Selesai" });

            comboBox1.Items.Clear();
            comboBox1.Items.Add("");
            comboBox1.Items.Add("Rawat Jalan");
            comboBox1.Items.Add("Rawat Inap");
            comboBox1.Items.Add("Lain-Lain");
            comboBox1.SelectedIndex = 1;


            luStatus.Properties.DataSource = listStat2;
            luStatus.Properties.ValueMember = "statusCode";
            luStatus.Properties.DisplayMember = "statusCode";

            luStatus.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luStatus.Properties.DropDownRows = listStat2.Count;
            luStatus.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luStatus.Properties.AutoSearchColumnIndex = 1;
            luStatus.Properties.NullText = "";
            luStatus.ItemIndex = 0;

            string sql_for1 = " select CODE_ID, initcap(CODE_NAME) medname from CS_CODE_DATA  where 1=1 and CODE_CLASS_ID = 'MED_USE' order by SORT_ORDER ";
            OleDbConnection oraConnectm = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOram = new OleDbDataAdapter(sql_for1, oraConnectm);
            DataTable dtm = new DataTable();
            adOram.Fill(dtm);

            listMedicineInfo.Clear();
            for (int i = 0; i < dtm.Rows.Count; i++)
            {
                listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = dtm.Rows[i]["CODE_ID"].ToString(), medicineInfoName = dtm.Rows[i]["medname"].ToString()  });
            }

            //listMedicineInfo.Clear();
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "A", medicineInfoName = "(P.C.) Sesudah Makan" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "B", medicineInfoName = "(A.C.) Sebelum Makan" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "C", medicineInfoName = "(P.R.N.) Bila Perlu" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "D", medicineInfoName = "(G.T.T) Diteteskan" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "E", medicineInfoName = "(U.E) Pemakaian Luar" });

            dResDate.Text = today;

            //tableLayoutPanel3.RowStyles[4] = new RowStyle(SizeType.Absolute, 0);
            //tableLayoutPanel3.RowStyles[5] = new RowStyle(SizeType.Absolute, 0);


            string sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from cs_formula a join cs_medicine b on(a.med_cd=b.med_cd) where 1=1 ";
            OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
            DataTable dtf = new DataTable();
            adOraf.Fill(dtf); 

            listFormula2.Clear();
            for (int i = 0; i < dtf.Rows.Count; i++)
            {
                listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
            }

            string sql_bpjs_cov = " select code_id, initcap(code_name) code_name from cs_code_data where code_class_id ='YES_NO'  and status = 'A' order by sort_order ";
            OleDbConnection sqlConnectBpjs = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSqlBpjs = new OleDbDataAdapter(sql_bpjs_cov, sqlConnectBpjs);
            DataTable dtBpjs = new DataTable();
            adSqlBpjs.Fill(dtBpjs);
            listMedBpjs.Clear();
            for (int i = 0; i < dtBpjs.Rows.Count; i++)
            {
                listMedBpjs.Add(new Medicine() { medicineCode = dtBpjs.Rows[i]["code_id"].ToString(), medicineName = dtBpjs.Rows[i]["code_name"].ToString() });

            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
            //LoadDataLimit();
            btnCancel.Enabled = false;
            gridControl2.DataSource = null;
            mActName.Text = "";
        }

        private void PrescriptionList_Load(object sender, EventArgs e)
        {
            initData();
            LoadData();
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "PrescriptionList");
            //LoadDataLimit();
            //SoftBlink(labelControl6, Color.LightPink, Color.Red, 1600, false);
        }

        private void LoadData()
        {
            string sql_search, tmp_month="";

            sql_search = " ";
            //sql_search = sql_search + " select que02, a.empid, b.name, b.dept, gender, type_patient,  status,   " +
            //                          " case  when observation = 'Y' then 'Yes' else 'No' end as observation, visit_remark, 'S' action,  " +
            //                          " to_char(visit_date,'yyyy-MM-dd') visit_date, que01 " +
            //                          " from cs_visit a join cs_employees b on a.empid = b.empid " +
            //                          " where 1 = 1  " +
            //                          " and to_char(visit_date,'yyyy-MM-dd')= '" + dResDate.Text + "'  " +
            //                          " and status in ('OBS','MED','CLS') " +
            //                          " and status like '%" + luStatus.Text + "%' " +
            //                          " order by que02 ";

            tmp_month = today;
            tmp_month = tmp_month.Substring(3,7);

            string SQL = " ";
            SQL = SQL + Environment.NewLine + "  ";
            SQL = SQL + Environment.NewLine + "  SELECT distinct que02, patient_no, initcap(name) name, address, gender, ";
            SQL = SQL + Environment.NewLine + "         poli_name, status, observation, confirm, ";
            SQL = SQL + Environment.NewLine + "         action, visit_date, que01, 'Y' editable, type_patient, id_visit, PLAN, age ";
            SQL = SQL + Environment.NewLine + "    FROM ( ";
            SQL = SQL + Environment.NewLine + "          SELECT que02, a.patient_no, b.name, address, gender, poli_name, a.status, ";
            SQL = SQL + Environment.NewLine + "                 CASE WHEN observation = 'Y' THEN 'Yes' ELSE 'No' END AS observation, ";
            SQL = SQL + Environment.NewLine + "                 CASE WHEN (SELECT COUNT (0) ";
            SQL = SQL + Environment.NewLine + "                            FROM cs_patient x ";
            SQL = SQL + Environment.NewLine + "                            JOIN cs_receipt y ON (x.rm_no = y.rm_no) ";
            SQL = SQL + Environment.NewLine + "                           WHERE x.status = 'A' ";
            SQL = SQL + Environment.NewLine + "                             AND patient_no = a.patient_no "; 
            SQL = SQL + Environment.NewLine + "                             AND y.id_visit = a.id_visit ";
            SQL = SQL + Environment.NewLine + "                             AND confirm = 'N') > 0 ";
            SQL = SQL + Environment.NewLine + "                    THEN 'N' ELSE 'Y' ";
            SQL = SQL + Environment.NewLine + "                 END AS confirm, 'S' action, ";
            SQL = SQL + Environment.NewLine + "                 TO_CHAR (visit_date, 'yyyy-MM-dd') visit_date, ";
            SQL = SQL + Environment.NewLine + "                 que01, a.type_patient, a.id_visit, a.PLAN ,round(((sysdate-b.birth_date)/30)/12) age   ";
            SQL = SQL + Environment.NewLine + "            FROM cs_visit a ";
            SQL = SQL + Environment.NewLine + "                 JOIN cs_patient_info b ON a.patient_no = b.patient_no ";
            SQL = SQL + Environment.NewLine + "                 JOIN cs_policlinic c ON a.poli_cd = c.poli_cd join cs_receipt d ON a.id_visit = d.id_visit  ";
            SQL = SQL + Environment.NewLine + "           WHERE     1 = 1 ";
            SQL = SQL + Environment.NewLine + "             AND c.status = 'A'  and d.F_ACTIVE ='Y' ";
            SQL = SQL + Environment.NewLine + "             AND a.status LIKE '%" + luStatus.Text + "%' ";
            if (comboBox1.Text.ToString().Equals("Rawat Jalan"))
                SQL = SQL + Environment.NewLine + "           AND trunc (a.visit_date) = trunc(sysdate) ";
            if (chkclosed.Checked)
            {
                SQL = SQL + Environment.NewLine + "     and a.status in ('INP','MED','PAY','DON','CLS') ";
                SQL = SQL + Environment.NewLine + "     and trunc(a.TIME_END) = trunc(sysdate) ";
            }
            else
            {
                SQL = SQL + Environment.NewLine + "     and a.status in ('INP','NUR','MED','PAY','DON') and a.id_visit not in ( select id_visit from cs_visit where status='NUR' and plan ='TRT01') ";
            }
            SQL = SQL + Environment.NewLine + "   group by que02, a.patient_no, b.name, address, gender, poli_name, a.status,  CASE WHEN observation = 'Y' THEN 'Yes' ELSE 'No' END , TO_CHAR (visit_date, 'yyyy-MM-dd') , que01, a.type_patient, a.id_visit, a.PLAN ,round(((sysdate-b.birth_date)/30)/12)  ";
            SQL = SQL + Environment.NewLine + " union all  ";
            SQL = SQL + Environment.NewLine + "select '-' que02,'-' patient_no, initcap(name) NAME, ADDRS address, c.GENDER, 'Lain-lain' poli_name, decode(STAT_PAY,'N','PAY','Y','DON','CLS')  status, 'No' observation, a.confirm,  ";
            SQL = SQL + Environment.NewLine + "       'S' action, TO_CHAR (REGIS_DATE, 'yyyy-MM-dd') visit_date,  '-' que01,  'U' type_patient, c.KIR_ID id_visit, 'TRT01' PLAN , round(((sysdate-c.birth_date)/30)/12) age   ";
            SQL = SQL + Environment.NewLine + "  FROM KLINIK.cs_receipt a  ";
            SQL = SQL + Environment.NewLine + "       JOIN KLINIK.CS_KIR c ON (a.ATT3_RECIEPT = c.KIR_ID)  ";
            SQL = SQL + Environment.NewLine + "       JOIN KLINIK.cs_medicine b ON (a.med_cd = b.med_cd)  ";
            SQL = SQL + Environment.NewLine + "       JOIN KLINIK.cs_formula D  ";
            SQL = SQL + Environment.NewLine + "          ON (B.med_cd = D.med_cd AND D.FORMULA_ID = A.formula)  ";
            SQL = SQL + Environment.NewLine + " WHERE     b.status = 'A'  ";
            SQL = SQL + Environment.NewLine + "       AND D.MINUS_STOK = 'Y'  ";
            SQL = SQL + Environment.NewLine + "       AND BPJS_COVER = 'N'  ";
            if (chkclosed.Checked)
            {
                SQL = SQL + Environment.NewLine + "        AND STAT_PAY ='X' ";
            }
            else
            {
                SQL = SQL + Environment.NewLine + "       AND STAT_PAY not in('C','X') ";
            }
            SQL = SQL + Environment.NewLine + "          ) x ";
            SQL = SQL + Environment.NewLine + "   WHERE 1 = 1 ";
            if (comboBox1.Text.ToString().Equals("Lain-Lain"))
                SQL = SQL + Environment.NewLine + "   and poli_name = 'Lain-lain' ";
            else if (comboBox1.Text.ToString().Equals("Rawat Inap"))
                SQL = SQL + Environment.NewLine + "   and poli_name = 'Rawat Inap' ";
            else if (comboBox1.Text.ToString().Equals("Rawat Jalan"))
                SQL = SQL + Environment.NewLine + "   and poli_name not in ( 'Rawat Inap','Lain-lain' ) ";
            SQL = SQL + Environment.NewLine + "ORDER BY visit_date, que02 "; ;

             
            //sql_search = sql_search + Environment.NewLine + "select que02, patient_no, name, address, gender, poli_name,  status, "; 
            //sql_search = sql_search + Environment.NewLine + "observation, confirm, action, visit_date, que01, ";
            ////sql_search = sql_search + Environment.NewLine + "case when to_date(visit_date,'yyyy-MM-dd') < trunc(sysdate) then 'N' ";
            ////sql_search = sql_search + Environment.NewLine + "else 'Y' end editable,  ";
            //sql_search = sql_search + Environment.NewLine + "'Y' editable, ";
            //sql_search = sql_search + Environment.NewLine + "type_patient, id_visit  ";
            //sql_search = sql_search + Environment.NewLine + "from ( ";
            //sql_search = sql_search + Environment.NewLine + "select que02, a.patient_no, b.name, address, gender, poli_name,  a.status, ";
            //sql_search = sql_search + Environment.NewLine + "case  when observation = 'Y' then 'Yes' else 'No' end as observation, ";
            //sql_search = sql_search + Environment.NewLine + "case when (select count(0)  ";
            //sql_search = sql_search + Environment.NewLine + "from cs_patient x ";
            //sql_search = sql_search + Environment.NewLine + "join cs_receipt y on (x.rm_no=y.rm_no) ";
            //sql_search = sql_search + Environment.NewLine + "where x.status='A' ";
            //sql_search = sql_search + Environment.NewLine + "and patient_no=a.patient_no ";
            //sql_search = sql_search + Environment.NewLine + "and visit_no=a.que01 ";
            //sql_search = sql_search + Environment.NewLine + "and insp_date=trunc(visit_date) ";
            //sql_search = sql_search + Environment.NewLine + "and confirm='N') > 0 then 'N' else 'Y' end as confirm,  ";
            //sql_search = sql_search + Environment.NewLine + "'S' action, to_char(visit_date,'yyyy-MM-dd') visit_date, que01, a.type_patient, a.id_visit ";
            //sql_search = sql_search + Environment.NewLine + "from cs_visit a  ";
            //sql_search = sql_search + Environment.NewLine + "join cs_patient_info b on a.patient_no = b.patient_no   ";
            //sql_search = sql_search + Environment.NewLine + "join cs_policlinic c on a.poli_cd=c.poli_cd ";
            //sql_search = sql_search + Environment.NewLine + "where 1 = 1 ";
            //sql_search = sql_search + Environment.NewLine + "and c.status='A' ";
            //sql_search = sql_search + Environment.NewLine + "and to_char(visit_date,'yyyy-MM-dd')= '" + dResDate.Text + "' ";
            //sql_search = sql_search + Environment.NewLine + "and a.status in ('OBS','MED','CLS','PAY','DON') ";
            //sql_search = sql_search + Environment.NewLine + "and a.status like '%" + luStatus.Text + "%' ";
            //sql_search = sql_search + Environment.NewLine + "union  ";
            //sql_search = sql_search + Environment.NewLine + "select a.que02, a.patient_no, name, address, gender, poli_name, a.status, 'No' obs,  ";
            //sql_search = sql_search + Environment.NewLine + "case when (select count(0)   ";
            //sql_search = sql_search + Environment.NewLine + "from cs_patient x  ";
            //sql_search = sql_search + Environment.NewLine + "join cs_receipt y on (x.rm_no=y.rm_no)  ";
            //sql_search = sql_search + Environment.NewLine + "where x.status='A'  ";
            //sql_search = sql_search + Environment.NewLine + "and patient_no=a.patient_no  ";
            //sql_search = sql_search + Environment.NewLine + "and visit_no=a.que01  ";
            //sql_search = sql_search + Environment.NewLine + "and visit_dt=trunc(visit_date)  ";
            //sql_search = sql_search + Environment.NewLine + "and confirm='N') > 0 then 'N' else 'Y' end as confirm, ";
            //sql_search = sql_search + Environment.NewLine + "'S' action, to_char(c.insp_date,'yyyy-MM-dd') visit_date, que01, a.type_patient, a.id_visit   ";
            //sql_search = sql_search + Environment.NewLine + "from cs_visit a ";
            //sql_search = sql_search + Environment.NewLine + "join cs_inpatient b on (a.inpatient_id=b.inpatient_id) ";
            //sql_search = sql_search + Environment.NewLine + "join cs_receipt c on (b.rm_no=c.rm_no and b.reg_date=c.visit_dt and a.que01=c.visit_no) ";
            //sql_search = sql_search + Environment.NewLine + "join cs_patient_info d on (a.patient_no=d.patient_no) ";
            //sql_search = sql_search + Environment.NewLine + "join cs_policlinic e on (a.poli_cd=e.poli_cd) ";
            //sql_search = sql_search + Environment.NewLine + "where 1=1 ";
            //sql_search = sql_search + Environment.NewLine + "and b.status='OPN' ";
            //sql_search = sql_search + Environment.NewLine + "and to_char(c.insp_date,'yyyy-MM-dd') = '" + dResDate.Text + "' ";
            //sql_search = sql_search + Environment.NewLine + "union  ";
            //sql_search = sql_search + Environment.NewLine + "select que02, a.patient_no, b.name, address, gender, poli_name,  a.status,  ";
            //sql_search = sql_search + Environment.NewLine + "case  when observation = 'Y' then 'Yes' else 'No' end as observation,   ";
            //sql_search = sql_search + Environment.NewLine + "'N' as confirm, 'S' action, to_char(visit_date,'yyyy-MM-dd') visit_date, que01, a.type_patient , a.id_visit ";
            //sql_search = sql_search + Environment.NewLine + "from cs_visit a   ";
            //sql_search = sql_search + Environment.NewLine + "join cs_patient_info b on a.patient_no = b.patient_no    ";
            //sql_search = sql_search + Environment.NewLine + "join cs_policlinic c on a.poli_cd=c.poli_cd  ";
            //sql_search = sql_search + Environment.NewLine + "join (select b.patient_no, insp_date, visit_no, count(0)  ";
            //sql_search = sql_search + Environment.NewLine + "from cs_receipt a ";
            //sql_search = sql_search + Environment.NewLine + "join cs_patient b on (a.rm_no=b.rm_no) ";
            //sql_search = sql_search + Environment.NewLine + "where to_char(insp_date,'MM/yyyy')=  '" + tmp_month + "'   ";
            //sql_search = sql_search + Environment.NewLine + "and confirm='N' ";
            //sql_search = sql_search + Environment.NewLine + "and b.status='A' ";
            //sql_search = sql_search + Environment.NewLine + "group by b.patient_no, insp_date, visit_no) d  ";
            //sql_search = sql_search + Environment.NewLine + "on (a.patient_no=d.patient_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no) ";
            //sql_search = sql_search + Environment.NewLine + "where 1 = 1  ";
            //sql_search = sql_search + Environment.NewLine + "and c.status='A'  ";
            //sql_search = sql_search + Environment.NewLine + "and to_char(visit_date,'MM/yyyy')=  '" + tmp_month + "' ";
            //sql_search = sql_search + Environment.NewLine + "and a.status in ('OBS','MED','CLS','PAY','DON')  ";
            //sql_search = sql_search + Environment.NewLine + ") x ";
            //sql_search = sql_search + Environment.NewLine + "where 1=1 ";
            //sql_search = sql_search + Environment.NewLine + "order by visit_date,que02 ";

             
            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                btnSave.Enabled = false;

                //gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 30;
                gridView1.OptionsBehavior.Editable = false;
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
                //gridView1.Columns[1].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[2].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[3].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[4].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[5].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[6].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[7].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[8].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[9].OptionsColumn.ReadOnly = true;
                gridView1.Columns[4].Visible = false;
                gridView1.Columns[9].Visible = false;
                //gridView1.Columns[10].Visible = false;
                gridView1.Columns[11].Visible = true ;

                gridView1.Columns[0].Caption = "Antrian";
                gridView1.Columns[1].Caption = "Pasien No";
                gridView1.Columns[2].Caption = "Nama";
                gridView1.Columns[3].Caption = "Alamat";
                gridView1.Columns[4].Caption = "Jenis Kelamin";
                gridView1.Columns[5].Caption = "Poli";
                gridView1.Columns[6].Caption = "Status";
                gridView1.Columns[7].Caption = "Obs";
                gridView1.Columns[8].Caption = "Confirm";
                gridView1.Columns[9].Caption = "Action";
                gridView1.Columns[10].Caption = "Tanggal";
                gridView1.Columns[11].Caption = "Antrian";
                gridView1.Columns[12].Caption = "Editable";
                gridView1.Columns[13].Caption = "Pasien";
                gridView1.Columns[14].Caption = "visitid";
                gridView1.Columns[15].Caption = "splan";
                gridView1.Columns[16].Caption = "age";
                gridView1.Columns[16].Visible = false;
                //PRE, RSV, NUR, INS, OBS, MED, CLS, CAN

                RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
                statusLookup.DataSource = listStat;
                statusLookup.ValueMember = "statusCode";
                statusLookup.DisplayMember = "statusName";

                statusLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup.DropDownRows = listStat.Count;
                statusLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup.AutoSearchColumnIndex = 1;
                statusLookup.NullText = "";
                gridView1.Columns[6].ColumnEdit = statusLookup;

                RepositoryItemLookUpEdit patientLookup = new RepositoryItemLookUpEdit();
                patientLookup.DataSource = listPatientType;
                patientLookup.ValueMember = "patientTypeCode";
                patientLookup.DisplayMember = "patientTypeName";

                patientLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                patientLookup.DropDownRows = listPatientType.Count;
                patientLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                patientLookup.AutoSearchColumnIndex = 1;
                patientLookup.NullText = "";
                gridView1.Columns[13].ColumnEdit = patientLookup;

                //gridView1.BestFitColumns();
                gridView1.Columns[0].Visible = false;
                gridView1.Columns[9].Visible = false;
                gridView1.Columns[3].Visible = false;
                gridView1.Columns[7].Visible = false;
                gridView1.Columns[12].Visible = false;
                gridView1.Columns[10].VisibleIndex = 0;
                gridView1.Columns[13].VisibleIndex = 5;
                gridView1.Columns[14].Visible = false;
                gridView1.Columns[15].Visible = false;

                gridView1.Columns[0].Width = 50;
                gridView1.Columns[1].Width = 60;
                gridView1.Columns[6].Width = 80;
                gridView1.Columns[8].Width = 60;
                gridView1.Columns[10].Width = 70;
                gridView1.BestFitColumns();

                gridControl3.DataSource = null;
                gridView3.Columns.Clear();
                gridControl3.DataSource = null;
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
            if (gridView1.RowCount < 1)
                return;

            GridView View = sender as GridView;
            string s_nik = "", s_que = "", s_date = "", sql_his = "", s_check="", s_cnt="", s_nama = "", s_action = "", act_cnt = "", act_name ="", s_act="", s_edit="";
            string s_tipe_pas = "", s_confirm = "", s_stat="", s_poli ="";
            
            s_nik = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
            if (s_nik.ToString().Equals(""))
            {
                gridControl2.DataSource = null;
                gridView2.Columns.Clear();
                gridControl2.DataSource = null;
                gridControl3.DataSource = null;
                gridView3.Columns.Clear();
                gridControl3.DataSource = null;
                return;
            }
               
            s_nama = View.GetRowCellDisplayText(e.RowHandle, View.Columns[2]);
            s_date = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
            s_que = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);
            s_edit = View.GetRowCellDisplayText(e.RowHandle, View.Columns[12]);
            s_tipe_pas = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[13]).ToString();
            s_confirm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            s_stat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[6]).ToString();
            visitid = View.GetRowCellDisplayText(e.RowHandle, View.Columns[14]);
            s_poli = View.GetRowCellDisplayText(e.RowHandle, View.Columns[5]);
            lNamaPasien.Text = s_nama;
            sql_his = "";
            if (s_poli.ToString().Equals("Lain-lain"))
            {
                sql_his = sql_his + Environment.NewLine + " select distinct initcap(med_name) med_name, a.formula, dosis,  ";
                sql_his = sql_his + Environment.NewLine + " 'N' bpjs,  ";
                sql_his = sql_his + Environment.NewLine + "  nvl(f.TRANS_QTY,a.med_qty)  med_qty,   ";
                sql_his = sql_his + Environment.NewLine + " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +   ";
                sql_his = sql_his + Environment.NewLine + " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -   ";
                sql_his = sql_his + Environment.NewLine + " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) stok,  ";
                sql_his = sql_his + Environment.NewLine + " confirm, a.receipt_id,a.med_cd , e.MINUS_STOK ,f.TRANS_ID   ";
                sql_his = sql_his + Environment.NewLine + " from cs_receipt a JOIN KLINIK.CS_KIR d ON (a.ATT3_RECIEPT = d.KIR_ID)   ";
                sql_his = sql_his + Environment.NewLine + "  join cs_medicine c on(a.med_cd = c.med_cd) JOIN KLINIK.CS_FORMULA e on(a.FORMULA = e.FORMULA_ID and a.med_cd = e.med_cd   )  ";
                sql_his = sql_his + Environment.NewLine + "  left JOIN cs_medicine_trans f on(a.med_cd = f.med_cd and a.receipt_id = f.receipt_id)   ";
                sql_his = sql_his + Environment.NewLine + " where  c.status = 'A' AND c.BPJS_COVER = 'N' AND e.MINUS_STOK = 'Y'   and KIR_ID = " + visitid + "   ";
                sql_his = sql_his + Environment.NewLine + " order by 1  "; 
            }
            else
            {
                sql_his = sql_his + Environment.NewLine + " select distinct initcap(med_name) med_name, a.formula, dosis, ";
                if (s_confirm == "N")
                {
                    if (s_tipe_pas == "U")
                    {
                        sql_his = sql_his + Environment.NewLine + " 'N' bpjs, ";
                    }
                    else
                    {
                        sql_his = sql_his + Environment.NewLine + " c.bpjs_cover bpjs, ";
                    }
                }
                else
                {
                    sql_his = sql_his + Environment.NewLine + " nvl(decode(insu_cover,0,'Y','N'),c.BPJS_COVER) bpjs,  ";
                    //sql_his = sql_his + Environment.NewLine + "  (select decode(insu_cover,0,'Y','N')  from cs_medicine_trans where receipt_id = a.receipt_id and a.MED_CD = MED_CD) bpjs, ";
                }

                //sql_his = sql_his + Environment.NewLine + "  nvl((select TRANS_QTY from cs_medicine_trans where receipt_id = a.receipt_id and a.MED_CD = MED_CD),a.med_qty)  med_qty,  ";
                sql_his = sql_his + Environment.NewLine + "  nvl(f.TRANS_QTY,a.med_qty)  med_qty,  ";
                sql_his = sql_his + Environment.NewLine + " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +  ";
                sql_his = sql_his + Environment.NewLine + " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -  ";
                sql_his = sql_his + Environment.NewLine + " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) stok, ";
                sql_his = sql_his + Environment.NewLine + " confirm, a.receipt_id,a.med_cd , e.MINUS_STOK ,f.TRANS_ID, nvl(f.TRANS_REMARK, a.TYPE_DRINK) CARA ,a.MED_REMARK REMARK ";
                sql_his = sql_his + Environment.NewLine + " from cs_receipt a ";
                sql_his = sql_his + Environment.NewLine + " join cs_patient b on (a.rm_no = b.rm_no) ";
                sql_his = sql_his + Environment.NewLine + "  join cs_medicine c on(a.med_cd = c.med_cd) JOIN KLINIK.CS_FORMULA e on(a.FORMULA = e.FORMULA_ID and a.med_cd = e.med_cd) ";
                sql_his = sql_his + Environment.NewLine + "  left JOIN cs_medicine_trans f on(a.med_cd = f.med_cd and a.receipt_id = f.receipt_id)  ";
                sql_his = sql_his + Environment.NewLine + "    join cs_code_data g on (a.TYPE_DRINK = g.CODE_ID and g.CODE_CLASS_ID = 'MED_USE') ";
                sql_his = sql_his + Environment.NewLine + " where b.status = 'A' ";
                sql_his = sql_his + Environment.NewLine + " and c.status = 'A' and a.jenis_obat ='NONE' ";
                sql_his = sql_his + Environment.NewLine + " and b.patient_no = '" + s_nik + "' and id_visit = " + visitid + " ";
                //if (s_poli.ToString().Equals("Rawat Inap"))
                //{
                //    sql_his = sql_his + Environment.NewLine + "  AND A.GRID_NAME ='gvObtPlng' ";
                //}
                sql_his = sql_his + Environment.NewLine + " order by confirm, 1 ";
            }
            

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_his, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl2.DataSource = null;
            gridView2.Columns.Clear();
            gridControl2.DataSource = dt;

            gridControl3.DataSource = null;
            gridView3.Columns.Clear();
            gridControl3.DataSource = null;

            gridView2.OptionsView.ColumnAutoWidth = true;
            gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView2.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView2.IndicatorWidth = 30;
            //gridView2.OptionsBehavior.Editable = false;
            gridView2.BestFitColumns();

            gridView2.Columns[0].Caption = "Nama Obat";
            gridView2.Columns[0].Width = 140;
            gridView2.Columns[1].Caption = "Kode Dosis";
            gridView2.Columns[2].Caption = "Dosis";
            gridView2.Columns[3].Caption = "BPJS";
            gridView2.Columns[4].Caption = "Jumlah";
            gridView2.Columns[5].Caption = "Stok";
            gridView2.Columns[6].Caption = "Confirm";
            gridView2.Columns[7].Caption = "ID";
            gridView2.Columns[8].Caption = "MED_CD";
            gridView2.Columns[9].Caption = "MSTOCK";
            gridView2.Columns[10].Caption = "CARA";
            if (!s_poli.ToString().Equals("Lain-lain"))
            {
                gridView2.Columns[11].Width = 100;
                gridView2.Columns[12].Caption = "REMARK";
                gridView2.Columns[12].Width = 100;

                RepositoryItemGridLookUpEdit glMedInfo = new RepositoryItemGridLookUpEdit();
                glMedInfo.DataSource = listMedicineInfo;
                glMedInfo.ValueMember = "medicineInfoCode";
                glMedInfo.DisplayMember = "medicineInfoName";

                glMedInfo.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glMedInfo.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                glMedInfo.ImmediatePopup = true;
                glMedInfo.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glMedInfo.NullText = "";
                gridView2.Columns[11].ColumnEdit = glMedInfo;
            }

            
            //gridView2.Columns[7].VisibleIndex = 0;
            gridView2.Columns[0].OptionsColumn.ReadOnly = true;
            gridView2.Columns[1].OptionsColumn.ReadOnly = true;
            gridView2.Columns[2].OptionsColumn.ReadOnly = true;
            gridView2.Columns[4].OptionsColumn.ReadOnly = false;
            gridView2.Columns[5].OptionsColumn.ReadOnly = true;
            gridView2.Columns[6].OptionsColumn.ReadOnly = true;
            gridView2.Columns[7].Visible = false;
            gridView2.Columns[8].Visible = false;
            gridView2.Columns[9].Visible = false;
            gridView2.Columns[10].Visible = false;
            if (s_tipe_pas == "U")
            {
                gridView2.Columns[3].OptionsColumn.ReadOnly = true;
            }
            else
            {
                gridView2.Columns[3].OptionsColumn.ReadOnly = false;
            }

            if (s_confirm == "Y")
            {
                gridView2.Columns[3].OptionsColumn.ReadOnly = true;
            }
            else
            {
                gridView2.Columns[3].OptionsColumn.ReadOnly = false;
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
            gridView2.Columns[1].ColumnEdit = glfor; 

            RepositoryItemLookUpEdit bpjsLookup = new RepositoryItemLookUpEdit();
            bpjsLookup.DataSource = listMedBpjs;
            bpjsLookup.ValueMember = "medicineCode";
            bpjsLookup.DisplayMember = "medicineName";

            bpjsLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            bpjsLookup.DropDownRows = listMedBpjs.Count;
            bpjsLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            bpjsLookup.AutoSearchColumnIndex = 1;
            bpjsLookup.NullText = "";
            gridView2.Columns[3].ColumnEdit = bpjsLookup;
            if (!s_poli.ToString().Equals("Lain-lain"))
            {
                ObatRacikan(visitid, s_nik);
                labelControl6.Visible = true;
                simpleButton1.Visible = true;
                gridControl3.Visible = true;
            }
            else
            {
                labelControl6.Visible = false;
                simpleButton1.Visible = false;
                gridControl3.Visible = false;
            }
            s_check = " select count(0) cnt " +
                      " from cs_receipt a " +
                      " join cs_patient b on (a.rm_no = b.rm_no) " +
                      " where b.status = 'A' " +
                      " and a.confirm = 'N' " +
                      " and b.patient_no = '" + s_nik + "'  and a.id_visit = " + visitid + " ";
                      //" and to_char(insp_date, 'yyyy-MM-dd') =  '" + s_date + "' " +
                      //" and visit_no = '" + s_que + "' ";

            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(s_check, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);
            s_cnt = dt2.Rows[0]["cnt"].ToString();

            s_action = " select count(0) cnt " +
                       " from cs_action a " +
                       " join cs_patient b on (a.rm_no = b.rm_no) " +
                       " where b.status = 'A' " +
                       " and b.patient_no = '" + s_nik + "' "  +
                       " and to_char(insp_date, 'yyyy-MM-dd') =  '" + s_date + "' " +
                       " and visit_no = '" + s_que + "' ";

            OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql3 = new OleDbDataAdapter(s_action, sqlConnect3);
            DataTable dt3 = new DataTable();
            adSql3.Fill(dt3);
            act_cnt = dt3.Rows[0]["cnt"].ToString();

            if (!visitid.ToString().Equals("0") && (!s_poli.ToString().Equals("Lain-lain")))
            { 
                //s_act = " select act_name " +
                //        " from cs_action a " +
                //        " join cs_patient b on (a.rm_no = b.rm_no) " +
                //        " where b.status = 'A' " +
                //        " and b.patient_no = '" + s_nik + "' " +
                //        " and to_char(insp_date, 'yyyy-MM-dd') = '" + s_date + "' " +
                //        " and visit_no = '" + s_que + "' ";

                string SQL  = "select  REPLACE('Diagnosa : '||(select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa " +
                                "           from KLINIK.cs_diagnosa a    " +
                                "            join KLINIK.cs_diagnosa_item b on (a.item_cd=b.item_cd)    " +
                                "          where b.status='A'    " +
                                "            and rm_no=c.rm_no    " +
                                "            and insp_date=trunc(b.visit_date)   " +
                                "            and visit_no=b.que01)  || chr(13) || 'Anamnesa : '|| " +
                                "        (select  'Tensi : ' || blood_press || ', Nadi : ' || pulse ||    " +
                                "                 ', Suhu : ' || temperature || ', BB : ' || bb || ', TB : ' || tb || ', Alergi : ' || allergy || ', Keluhan : ' || anamnesa   " +
                                "                 || CHR (13) ||'Alergi ' || ALERGI_MKN || ', Alergi ' || ALERGI_OBAT || ', Kolesterol : '||CHOLESTEROL || ', Gula : '||BLOOD_SUGAR || ', Asam Urat : '||URIC_ACID as anamnesa     " +
                                "        from KLINIK.cs_anamnesa d, KLINIK.cs_anamnesa_dtl e " +
                                "        where d.ANAMNESA_ID = e.ANAMNESA_ID " +
                                "          and d.rm_no=c.rm_no   " +
                                "          and d.insp_date=trunc(b.visit_date)    " +
                                "          and d.visit_no=b.que01)  " +
                                "          ,'::',':') as anamnesa " +
                                "from KLINIK.cs_patient_info a    " +
                                "join KLINIK.cs_visit b on (a.patient_no = b.patient_no)    " +
                                "join KLINIK.cs_patient c on(b.patient_no = c.patient_no)    " +
                                "where b.id_visit = " + visitid + " " + //to_char(b.visit_date, 'yyyy-MM-dd') =  '" + s_date + "'    " +
                                "  and c.status = 'A'    " +
                                //"  and b.que01 =  '" + s_que + "'     " +
                                //"  and c.group_patient = 'COMM'    " +
                                "  and c.patient_no = '" + s_nik + "' ";


                OleDbConnection sqlConnect4 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql4 = new OleDbDataAdapter(SQL, sqlConnect4);
                DataTable dt4 = new DataTable();
                adSql4.Fill(dt4);
                act_name = dt4.Rows[0]["anamnesa"].ToString(); 
                mActName.Text = dt4.Rows[0]["anamnesa"].ToString();
                mActName.Text = act_name; 
            }
            else
            {
                mActName.Text = "";
            }

            if (Convert.ToInt32(s_cnt) > 0 || (s_poli.ToString().Equals("Lain-lain")) && (s_confirm.ToString().Equals("N")))
            {
                btnConfirm.Enabled = true;
                simpleButton1.Enabled = true;
                if (s_stat == "CLS")
                    btnCancel.Enabled = false;
                else
                    btnCancel.Enabled = true;
            }
            else
            {
                btnConfirm.Enabled = false;
                simpleButton1.Enabled = false;
                if (s_stat == "CLS")
                    btnCancel.Enabled = false;
                else
                    btnCancel.Enabled = true;
            }

            if (s_stat == "DON")
            {
                btnCompltd.Enabled = true;
            }
            else
            {
                btnCompltd.Enabled = false;
            }
        }
        private void ObatRacikan(string idvisit, string idpasien)
        {
            string Sql = "";
            Sql = Sql + Environment.NewLine + "  select distinct initcap(med_name) med_name, a.formula, dosis,  ";
            Sql = Sql + Environment.NewLine + "         nvl(decode(insu_cover,0,'Y','N'),c.BPJS_COVER) bpjs,  ";
            Sql = Sql + Environment.NewLine + "         nvl(f.TRANS_QTY,a.med_qty)  med_qty,   ";
            Sql = Sql + Environment.NewLine + "         klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +   ";
            Sql = Sql + Environment.NewLine + "         klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -   ";
            Sql = Sql + Environment.NewLine + "         klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) stok,  ";
            Sql = Sql + Environment.NewLine + "         confirm, a.receipt_id,a.med_cd , e.MINUS_STOK ,f.TRANS_ID ,d.CODE_ID ID_RACIK,d.CODE_NAME RACIKAN,ATT3_RECIEPT QTY,ATT2_RECIEPT NOTE, a.TYPE_DRINK CARA,a.MED_REMARK REMARK  ";
            Sql = Sql + Environment.NewLine + "    from cs_receipt a  ";
            Sql = Sql + Environment.NewLine + "    join cs_patient b on (a.rm_no = b.rm_no)  ";
            Sql = Sql + Environment.NewLine + "    join cs_medicine c on(a.med_cd = c.med_cd) JOIN KLINIK.CS_FORMULA e on(a.FORMULA = e.FORMULA_ID and a.med_cd = e.med_cd)  ";
            Sql = Sql + Environment.NewLine + "    left JOIN cs_medicine_trans f on(a.med_cd = f.med_cd and a.receipt_id = f.receipt_id)   ";
            Sql = Sql + Environment.NewLine + "    join cs_code_data d on (a.ATT1_RECIEPT = d.CODE_ID and d.CODE_CLASS_ID = 'MED_RACIK') ";
            Sql = Sql + Environment.NewLine + "    join cs_code_data g on (a.TYPE_DRINK = g.CODE_ID and g.CODE_CLASS_ID = 'MED_USE') ";
            Sql = Sql + Environment.NewLine + " where b.status = 'A'  ";
            Sql = Sql + Environment.NewLine + "   and c.status = 'A' and a.jenis_obat = 'RACIK' ";
            Sql = Sql + Environment.NewLine + "   and b.patient_no = '" + idpasien + "' and id_visit = " + idvisit + "  ";
            Sql = Sql + Environment.NewLine + " order by d.CODE_ID, initcap(med_name) ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(Sql, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl3.DataSource = null;
            gridView3.Columns.Clear();
            gridControl3.DataSource = dt;

            gridView3.OptionsView.ColumnAutoWidth = true;
            gridView3.OptionsView.AllowCellMerge = true ;
            gridView3.OptionsBehavior.Editable= false;
            gridView3.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView3.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView3.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView3.IndicatorWidth = 30; 
            gridView3.BestFitColumns();

            gridView3.Columns[11].Caption = "ID";
            gridView3.Columns[12].Caption = "RACIKAN";
            gridView3.Columns[2].Caption = "Dosis";
            gridView3.Columns[13].Caption = "QTY";
            gridView3.Columns[14].Caption = "NOTE";
            gridView3.Columns[15].Caption = "CARA";
            gridView3.Columns[0].Caption = "Nama Obat";
            gridView3.Columns[1].Caption = "Satuan";            
            gridView3.Columns[3].Caption = "BPJS";
            gridView3.Columns[4].Caption = "Jumlah";
            gridView3.Columns[5].Caption = "Stok";
            gridView3.Columns[6].Caption = "Cnfrm";
            gridView3.Columns[7].Caption = "ID";
            gridView3.Columns[8].Caption = "MED_CD";
            gridView3.Columns[9].Caption = "MSTOCK";
            gridView3.Columns[10].Caption = "TRANS_ID";
            gridView3.Columns[16].Caption = "REMARK";

            gridView3.Columns[11].VisibleIndex =0;
            gridView3.Columns[12].VisibleIndex = 1;
            gridView3.Columns[2].VisibleIndex = 2;
            gridView3.Columns[13].VisibleIndex = 3;
            gridView3.Columns[14].VisibleIndex =4;
            gridView3.Columns[15].VisibleIndex = 5;
            gridView3.Columns[0].VisibleIndex = 6;
            gridView3.Columns[1].VisibleIndex = 7;
            gridView3.Columns[3].VisibleIndex = 8;
            gridView3.Columns[4].VisibleIndex = 9;
            gridView3.Columns[5].VisibleIndex = 10;
            gridView3.Columns[6].VisibleIndex = 11;
            gridView3.Columns[7].VisibleIndex = 12;
            gridView3.Columns[8].VisibleIndex = 13;
            gridView3.Columns[9].VisibleIndex = 14;
            gridView3.Columns[11].VisibleIndex = 10;
            gridView3.Columns[16].VisibleIndex = 15;

            gridView3.Columns[11].Width = 30;
            gridView3.Columns[12].Width = 50;
            gridView3.Columns[2].Width = 35;
            gridView3.Columns[13].Width =30;
            gridView3.Columns[14].Width = 100;
            gridView3.Columns[15].Width = 100;
            gridView3.Columns[0].Width = 150;
            gridView3.Columns[1].Width = 40;
            gridView3.Columns[3].Width = 8;
            gridView3.Columns[4].Width = 40;
            gridView3.Columns[5].Width = 10;
            gridView3.Columns[6].Width = 40;
            gridView3.Columns[7].Width = 12;
            gridView3.Columns[8].Width = 13;
            gridView3.Columns[9].Width = 14;
            gridView3.Columns[10].Width = 15;
            gridView3.Columns[11].Width = 55;

            //gridView3.Columns[7].VisibleIndex = 0;
            gridView3.Columns[0].OptionsColumn.ReadOnly = true;
            gridView3.Columns[1].OptionsColumn.ReadOnly = true;
            gridView3.Columns[2].OptionsColumn.ReadOnly = true;
            gridView3.Columns[4].OptionsColumn.ReadOnly = false;
            gridView3.Columns[5].OptionsColumn.ReadOnly = true;
            gridView3.Columns[6].OptionsColumn.ReadOnly = true;
            gridView3.Columns[3].Visible = false;
            gridView3.Columns[5].Visible = false;
            gridView3.Columns[7].Visible = false;
            gridView3.Columns[8].Visible = false;
            gridView3.Columns[9].Visible = false;
            gridView3.Columns[10].Visible = false;
            gridView3.Columns[11].Visible = true;
            gridView3.Columns[12].Visible = true;
            gridView3.Columns[13].Visible = true;
            gridView3.Columns[14].Visible = true;

            gridView3.Columns[11].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            gridView3.Columns[12].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            gridView3.Columns[1].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            gridView3.Columns[13].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            gridView3.Columns[14].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            gridView3.Columns[15].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;

            gridView3.Columns[0].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False ;
            gridView3.Columns[1].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            gridView3.Columns[4].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            gridView3.Columns[6].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;

            gridView3.CellMerge += gridView3_CellMerge;

            gridView3.BestFitColumns();

            //if (s_tipe_pas == "U")
            //{
            //    gridView3.Columns[3].OptionsColumn.ReadOnly = true;
            //}
            //else
            //{
            //    gridView3.Columns[3].OptionsColumn.ReadOnly = false;
            //}

            //if (s_confirm == "Y")
            //{
            //    gridView3.Columns[3].OptionsColumn.ReadOnly = true;
            //}
            //else
            //{
            //    gridView3.Columns[3].OptionsColumn.ReadOnly = false;
            //}

            RepositoryItemGridLookUpEdit glfor = new RepositoryItemGridLookUpEdit();
            glfor.DataSource = listFormula2;
            glfor.ValueMember = "formulaCode";
            glfor.DisplayMember = "formulaName";

            glfor.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glfor.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glfor.ImmediatePopup = true;
            glfor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glfor.NullText = "";
            gridView3.Columns[1].ColumnEdit = glfor;

            RepositoryItemLookUpEdit bpjsLookup = new RepositoryItemLookUpEdit();
            bpjsLookup.DataSource = listMedBpjs;
            bpjsLookup.ValueMember = "medicineCode";
            bpjsLookup.DisplayMember = "medicineName";

            bpjsLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            bpjsLookup.DropDownRows = listMedBpjs.Count;
            bpjsLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            bpjsLookup.AutoSearchColumnIndex = 1;
            bpjsLookup.NullText = "";
            gridView3.Columns[3].ColumnEdit = bpjsLookup;

            RepositoryItemLookUpEdit lookmedinfo = new RepositoryItemLookUpEdit();
            lookmedinfo.DataSource = listMedicineInfo;
            lookmedinfo.ValueMember = "medicineInfoCode";
            lookmedinfo.DisplayMember = "medicineInfoName";

            lookmedinfo.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            lookmedinfo.DropDownRows = listMedBpjs.Count;
            lookmedinfo.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            lookmedinfo.AutoSearchColumnIndex = 1;
            lookmedinfo.NullText = "";
            gridView3.Columns[15].ColumnEdit = lookmedinfo;

        }
        private void LoadDataLimit()
        {
            string SQL = "", limit = "";

            if (Convert.ToInt32(txtLimitStok.Text) <= 0)
            {
                limit = "5";
                txtLimitStok.Text = "5";
            }
            else
            {
                limit = txtLimitStok.Text;
            }

            SQL = SQL + Environment.NewLine + "select LISTAGG(med_name, '; ') WITHIN GROUP (ORDER BY med_name ASC) med_name, stok from (   ";
            SQL = SQL + Environment.NewLine + "select initcap(med_name) med_name, ";
            SQL = SQL + Environment.NewLine + "klinik.FN_CS_INIT_STOCK(sysdate,med_cd) +  ";
            SQL = SQL + Environment.NewLine + "klinik.FN_CS_TRX_IN(sysdate,med_cd) -   ";
            SQL = SQL + Environment.NewLine + "klinik.FN_CS_TRX_OUT(sysdate,med_cd) -   ";
            SQL = SQL + Environment.NewLine + "klinik.FN_CS_REQ_STOCK(sysdate,med_cd) as stok ";
            SQL = SQL + Environment.NewLine + "from cs_medicine  ";
            SQL = SQL + Environment.NewLine + "where 1=1  ";
            SQL = SQL + Environment.NewLine + "and status = 'A' ) a  ";
            SQL = SQL + Environment.NewLine + "where 1=1   ";
            SQL = SQL + Environment.NewLine + "and stok <= " + limit + " ";
            SQL = SQL + Environment.NewLine + "group by stok ";
            SQL = SQL + Environment.NewLine + "order by stok   ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql2 = new OleDbDataAdapter(SQL, sqlConnect2);
                DataTable dt2 = new DataTable();
                adSql2.Fill(dt2);

                gridControl3.DataSource = null;
                gridView3.Columns.Clear();
                gridControl3.DataSource = dt2;

                //gridView3.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView3.OptionsView.ColumnAutoWidth = true;
                gridView3.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView3.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView3.IndicatorWidth = 40;
                gridView3.OptionsBehavior.Editable = false;
                gridView3.BestFitColumns();

                gridView3.Columns[0].Caption = "Nama Obat";
                gridView3.Columns[1].Caption = "Stok Saat Ini";

                gridView3.Columns[1].MinWidth = 80;
                gridView3.Columns[1].MaxWidth = 80;

                gridView3.Columns[0].OptionsColumn.AllowEdit = false;
                gridView3.Columns[1].OptionsColumn.AllowEdit = false;

                RepositoryItemMemoEdit nmObat = new RepositoryItemMemoEdit();
                nmObat.WordWrap = true;
                gridView3.Columns[0].ColumnEdit = nmObat;

                gridView3.BestFitColumns();
                gridView3.Columns[0].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                if (dt2.Rows.Count > 0)
                {
                    tableLayoutPanel3.RowStyles[4] = new RowStyle(SizeType.Absolute, 30);
                    tableLayoutPanel3.RowStyles[5] = new RowStyle(SizeType.Absolute, 200);
                }
                else
                {
                    tableLayoutPanel3.RowStyles[4] = new RowStyle(SizeType.Absolute, 0);
                    tableLayoutPanel3.RowStyles[5] = new RowStyle(SizeType.Absolute, 0);
                }

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
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

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount < 0) return;

            string s_nik = "", s_que = "", s_date = "", sql_his = "", s_rm = "", s_edit = "", sql_cek = "", payst = "",s_stat="", tdrink = "";
            string s_tipe = "", splan ="";
            string sql_all = "", gnder = "", p1 = "", p2 = "", teks = "", p_que = "", policd = "", rm_type = "", s_name = "", q_no2 = "";

            s_nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            s_stat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[6]).ToString();
            policd = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[11]).ToString();
            s_edit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[12]).ToString();
            s_tipe = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[13]).ToString();
            visitid = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();

            if (policd.ToString().Equals("Lain-lain"))
            {
                sql_cek = "";
                sql_cek = sql_cek + Environment.NewLine + "select STAT_PAY status  ";
                sql_cek = sql_cek + Environment.NewLine + "from KLINIK.CS_KIR ";
                sql_cek = sql_cek + Environment.NewLine + "where KIR_ID=  " + visitid + "";

                dtKir = ConnOra.Data_Table_ora(sql_cek);

                if (dtKir.Rows.Count > 0)
                {
                    payst = dtKir.Rows[0]["status"].ToString();

                    if (payst == "Y")
                    {
                        MessageBox.Show("Data tidak bisa dirubah. Sudah melakukan pembayaran..!!!");
                        return;
                    }

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

                        command.CommandText = " update cs_receipt set confirm = 'Y', CONFIRM_BY = '" + DB.vUserId + "', CONFIRM_DATE = sysdate " +
                                              " where ATT3_RECIEPT = " + visitid + " and confirm = 'N' ";

                        command.ExecuteNonQuery();

                        for (int i = 0; i < gridView2.RowCount; i++)
                        {
                            string temp_id = "", temp_code = "", temp_q = "", temp_confrm = "";

                            temp_code = gridView2.GetRowCellValue(i, gridView2.Columns[8]).ToString();
                            temp_q = gridView2.GetRowCellValue(i, gridView2.Columns[4]).ToString();
                            temp_id = gridView2.GetRowCellValue(i, gridView2.Columns[7]).ToString();
                            temp_confrm = gridView2.GetRowCellValue(i, gridView2.Columns[6]).ToString();
                            //tdrink = gridView2.GetRowCellValue(i, gridView2.Columns[11]).ToString();
                            try
                            {
                                if (temp_confrm.ToString().Equals("N"))
                                {
                                    command.CommandText = " insert into cs_medicine_trans (trans_id, med_cd, trans_type, trans_date, trans_qty, receipt_id, insu_cover, ins_date, ins_emp) values " + //, TRANS_REMARK
                                                " (klinik.cs_medtrans_seq.nextval,'" + temp_code + "','OUT',to_date('" + s_date + "','yyyy-MM-dd'),'" + temp_q + "','" + temp_id + "', 1, sysdate,'" + DB.vUserId + "') "; //,'" + tdrink + "'

                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ERROR: " + ex.Message);
                            }
                        }

                        trans.Commit();

                        LoadData();

                        labelControl5.Visible = true;
                        labelControl5.Text = "Konfirmasi Berhasil";
                        Blinking(labelControl5, 1);

                        //MessageBox.Show("Data Obat Berhasil di Konfirmasi.");
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MessageBox.Show("ERROR: " + ex.Message);
                    }

                    oraConnectTrans.Close();

                }
            }
            else
            {
                if (policd.ToString().Equals("Rawat Inap"))
                {
                    sql_cek = "";
                    sql_cek = sql_cek + Environment.NewLine + "select status  ";
                    sql_cek = sql_cek + Environment.NewLine + "from cs_treatment_head ";
                    sql_cek = sql_cek + Environment.NewLine + "where patient_no='" + s_nik + "' and id_visit = " + visitid + ""; 

                    OleDbConnection sqlConnectc = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSqlc = new OleDbDataAdapter(sql_cek, sqlConnectc);
                    DataTable dtc = new DataTable();
                    adSqlc.Fill(dtc);

                    if (dtc.Rows.Count > 0)
                    {
                        payst = dtc.Rows[0]["status"].ToString();
                    }

                    if (payst == "CLS")
                    {
                        MessageBox.Show("Data tidak bisa dirubah.");
                        return;
                    }

                    if (s_edit == "N")
                    {
                        MessageBox.Show("Data tidak bisa dirubah. Silahkan melakukan adjusment.");
                        return;
                    }

                    sql_his = " select a.rm_no, b.patient_no, a.visit_no, med_cd, med_qty, receipt_id " +
                              " from cs_receipt a " +
                              " join cs_patient b on (a.rm_no = b.rm_no) " +
                              " where b.status = 'A' " +
                              " and a.confirm = 'N' " +
                              " and b.patient_no = '" + s_nik + "' and id_visit = " + visitid + " ";
                    //" and to_char(insp_date, 'yyyy-MM-dd') = '" + s_date + "' " +
                    //" and visit_no = '" + s_que + "' ";

                    OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSql = new OleDbDataAdapter(sql_his, sqlConnect);
                    DataTable dt = new DataTable();
                    adSql.Fill(dt);


                    if (dt.Rows.Count > 0)
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

                            if (payst == "OPN")
                            {

                            }
                            else
                            {
                                command.CommandText = " update cs_visit set time_receipt = sysdate, time_end = sysdate, status = 'PAY',  " +
                                                  " upd_date = sysdate, upd_emp = '" + DB.vUserId + "'  " +
                                                  " where patient_no = '" + s_nik + "'  and id_visit = " + visitid + " "; // and to_char(visit_date,'yyyy-MM-dd') = '" + s_date + "' " +
                                                                                                                          //" and que01 = '" + s_que + "' ";
                                command.ExecuteNonQuery();
                            }

                            s_rm = dt.Rows[0]["rm_no"].ToString();
                            command.CommandText = " update cs_receipt set confirm = 'Y', upd_emp = '" + DB.vUserId + "', upd_date = sysdate " +
                                                  " where rm_no = '" + s_rm + "' and id_visit = " + visitid + " and confirm = 'N' ";

                            command.ExecuteNonQuery();

                            //for (int i = 0; i < dt.Rows.Count; i++)
                            //{
                            //    string temp_cd="", temp_qty = "", temp_id = "";
                            //    //listDiagnosa.Add(new Diagnosa() { diagnosaCode = dt.Rows[i]["item_cd"].ToString(), diagnosaName = dt.Rows[i]["item_name"].ToString() });
                            //    temp_cd = dt.Rows[i]["med_cd"].ToString();
                            //    temp_qty = dt.Rows[i]["med_qty"].ToString();
                            //    temp_id = dt.Rows[i]["receipt_id"].ToString();

                            //    // Ini di ganti 2024.03.31                    
                            //    //command.CommandText = " insert into cs_medicine_trans (trans_id, med_cd, trans_type, trans_date, trans_qty, receipt_id, insu_cover, ins_date, ins_emp) values " +
                            //    //                      " (klinik.cs_medtrans_seq.nextval,'" + temp_cd + "','OUT',to_date('" + s_date + "','yyyy-MM-dd'),'" + temp_qty + "','" + temp_id + "', null, sysdate,'" + DB.vUserId + "') ";

                            //    //command.ExecuteNonQuery();
                            //}


                            for (int i = 0; i < gridView2.RowCount; i++)
                            {
                                // view.GetRowCellValue(e.RowHandle, view.Columns[14]).ToString();
                                string temp_bpjs = "", temp_id = "", temp_cover = "", temp_code = "", temp_q = "", temp_confrm = "";

                                temp_code = gridView2.GetRowCellValue(i, gridView2.Columns[8]).ToString();
                                temp_q = gridView2.GetRowCellValue(i, gridView2.Columns[4]).ToString();
                                temp_id = gridView2.GetRowCellValue(i, gridView2.Columns[7]).ToString();
                                temp_bpjs = gridView2.GetRowCellValue(i, gridView2.Columns[3]).ToString();
                                temp_confrm = gridView2.GetRowCellValue(i, gridView2.Columns[6]).ToString();
                                tdrink = gridView2.GetRowCellValue(i, gridView2.Columns[11]).ToString();
                                if (temp_bpjs == "Y")
                                {
                                    temp_cover = "0";
                                }
                                else
                                {
                                    temp_cover = "1";
                                }

                                //if (s_tipe == "B")
                                //    temp_cover = "0";
                                //else
                                //    temp_cover = "1"; 

                                //sql_update = ""; 
                                //sql_update = sql_update + " update cs_medicine_trans" +
                                //                          " set insu_cover = " + temp_cover + ", ";
                                //sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                                //sql_update = sql_update + " where receipt_id = '" + temp_id + "' ";

                                try
                                {
                                    if (temp_confrm.ToString().Equals("N"))
                                    {
                                        command.CommandText = " insert into cs_medicine_trans (trans_id, med_cd, trans_type, trans_date, trans_qty, receipt_id, insu_cover, ins_date, ins_emp, TRANS_REMARK) values " +
                                                    " (klinik.cs_medtrans_seq.nextval,'" + temp_code + "','OUT',to_date('" + s_date + "','yyyy-MM-dd'),'" + temp_q + "','" + temp_id + "', " + temp_cover + ", sysdate,'" + DB.vUserId + "' ,'" + tdrink + "') ";

                                        command.ExecuteNonQuery();
                                    }
                                    //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                                    //OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                                    //oraConnect.Open();
                                    //cm.ExecuteNonQuery();
                                    //oraConnect.Close();
                                    //cm.Dispose();

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("ERROR: " + ex.Message);
                                }
                            }

                            splan = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[15]).ToString();

                            if (splan.ToString().Equals("TRT01"))
                            {
                                string callid = "", age = "";
                                p_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                                gnder = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
                                s_name = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
                                age = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[16]).ToString();

                                sql_all = "";
                                sql_all = sql_all + @" select a.CALL_ID, TYPE_INS, a.que
                                        from KLINIK.CS_CALL_LOG a, KLINIK.cs_visit b
                                        where a.que = b.que01
                                        AND b.que02 = '" + p_que + @"'    
                                        AND b.id_visit = '" + visitid + @"'    
                                        AND TRUNC(A.INS_DATE) = TRUNC(VISIT_DATE)  ";

                                OleDbConnection oraConnect5 = ConnOra.Create_Connect_Ora();
                                OleDbDataAdapter adOra5 = new OleDbDataAdapter(sql_all, oraConnect5);
                                DataTable dt5 = new DataTable();
                                adOra5.Fill(dt5);
                                if (dt5.Rows.Count > 0)
                                {
                                    callid = dt5.Rows[0]["CALL_ID"].ToString();
                                    rm_type = dt5.Rows[0]["TYPE_INS"].ToString();
                                    q_no2 = dt5.Rows[0]["que"].ToString();
                                }

                                //{
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

                                teks = "Nomor Antrian " + q_no2 + " " + p1 + p2 + " Silahkan Menuju Ke Kasir";

                                sql_all = "";
                                sql_all = @"UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'W', type_ins ='PAY', stat ='Kasir', param = '" + teks + "' WHERE CALL_ID = " + callid + "";

                                ORADB.Execute(ORADB.XE, sql_all);
                            }

                            trans.Commit();

                            LoadData();
                            //MessageBox.Show(sql_insert);
                            //MessageBox.Show("Query Exec : " + sql_insert);

                            labelControl5.Visible = true;
                            labelControl5.Text = "Konfirmasi Berhasil";
                            Blinking(labelControl5, 1);
                            //MessageBox.Show("Data Berhasil di Konfirmasi.");
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            MessageBox.Show("ERROR: " + ex.Message);
                        }

                        oraConnectTrans.Close();
                    }

                }
                else
                {

                    sql_cek = "";
                    sql_cek = sql_cek + Environment.NewLine + "select status  ";
                    sql_cek = sql_cek + Environment.NewLine + "from cs_treatment_head ";
                    sql_cek = sql_cek + Environment.NewLine + "where patient_no='" + s_nik + "' and id_visit = " + visitid + "";
                    //sql_cek = sql_cek + Environment.NewLine + "and to_char(visit_date,'yyyy-MM-dd')='" + s_date + "' ";
                    //sql_cek = sql_cek + Environment.NewLine + "and visit_no='" + s_que + "' ";

                    OleDbConnection sqlConnectc = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSqlc = new OleDbDataAdapter(sql_cek, sqlConnectc);
                    DataTable dtc = new DataTable();
                    adSqlc.Fill(dtc);

                    if (dtc.Rows.Count > 0)
                    {
                        payst = dtc.Rows[0]["status"].ToString();
                    }

                    if (payst == "CLS")
                    {
                        MessageBox.Show("Data tidak bisa dirubah.");
                        return;
                    }

                    if (s_edit == "N")
                    {
                        MessageBox.Show("Data tidak bisa dirubah. Silahkan melakukan adjusment.");
                        return;
                    }

                    sql_his = " select a.rm_no, b.patient_no, a.visit_no, med_cd, med_qty, receipt_id " +
                              " from cs_receipt a " +
                              " join cs_patient b on (a.rm_no = b.rm_no) " +
                              " where b.status = 'A' " +
                              " and a.confirm = 'N' " +
                              " and b.patient_no = '" + s_nik + "' and id_visit = " + visitid + " ";
                    //" and to_char(insp_date, 'yyyy-MM-dd') = '" + s_date + "' " +
                    //" and visit_no = '" + s_que + "' ";

                    OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSql = new OleDbDataAdapter(sql_his, sqlConnect);
                    DataTable dt = new DataTable();
                    adSql.Fill(dt);


                    if (dt.Rows.Count > 0)
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

                            if (s_stat == "INP")
                            {

                            }
                            else
                            {
                                command.CommandText = " update cs_visit set time_receipt = sysdate, time_end = sysdate, status = 'PAY',  " +
                                                  " upd_date = sysdate, upd_emp = '" + DB.vUserId + "'  " +
                                                  " where patient_no = '" + s_nik + "'  and id_visit = " + visitid + " "; // and to_char(visit_date,'yyyy-MM-dd') = '" + s_date + "' " +
                                                                                                                          //" and que01 = '" + s_que + "' ";
                                command.ExecuteNonQuery();
                            }

                            s_rm = dt.Rows[0]["rm_no"].ToString();
                            command.CommandText = " update cs_receipt set confirm = 'Y', upd_emp = '" + DB.vUserId + "', upd_date = sysdate " +
                                                  " where rm_no = '" + s_rm + "' and id_visit = " + visitid + " and confirm = 'N' ";

                            command.ExecuteNonQuery();

                            //for (int i = 0; i < dt.Rows.Count; i++)
                            //{
                            //    string temp_cd="", temp_qty = "", temp_id = "";
                            //    //listDiagnosa.Add(new Diagnosa() { diagnosaCode = dt.Rows[i]["item_cd"].ToString(), diagnosaName = dt.Rows[i]["item_name"].ToString() });
                            //    temp_cd = dt.Rows[i]["med_cd"].ToString();
                            //    temp_qty = dt.Rows[i]["med_qty"].ToString();
                            //    temp_id = dt.Rows[i]["receipt_id"].ToString();

                            //    // Ini di ganti 2024.03.31                    
                            //    //command.CommandText = " insert into cs_medicine_trans (trans_id, med_cd, trans_type, trans_date, trans_qty, receipt_id, insu_cover, ins_date, ins_emp) values " +
                            //    //                      " (klinik.cs_medtrans_seq.nextval,'" + temp_cd + "','OUT',to_date('" + s_date + "','yyyy-MM-dd'),'" + temp_qty + "','" + temp_id + "', null, sysdate,'" + DB.vUserId + "') ";

                            //    //command.ExecuteNonQuery();
                            //}


                            for (int i = 0; i < gridView2.RowCount; i++)
                            {
                                // view.GetRowCellValue(e.RowHandle, view.Columns[14]).ToString();
                                string temp_bpjs = "", temp_id = "", temp_cover = "", temp_code = "", temp_q = "", temp_confrm = "";

                                temp_code = gridView2.GetRowCellValue(i, gridView2.Columns[8]).ToString();
                                temp_q = gridView2.GetRowCellValue(i, gridView2.Columns[4]).ToString();
                                temp_id = gridView2.GetRowCellValue(i, gridView2.Columns[7]).ToString();
                                temp_bpjs = gridView2.GetRowCellValue(i, gridView2.Columns[3]).ToString();
                                temp_confrm = gridView2.GetRowCellValue(i, gridView2.Columns[6]).ToString();
                                tdrink = gridView2.GetRowCellValue(i, gridView2.Columns[11]).ToString();
                                if (temp_bpjs == "Y")
                                {
                                    temp_cover = "0";
                                }
                                else
                                {
                                    temp_cover = "1";
                                }

                                //if (s_tipe == "B")
                                //    temp_cover = "0";
                                //else
                                //    temp_cover = "1"; 

                                //sql_update = ""; 
                                //sql_update = sql_update + " update cs_medicine_trans" +
                                //                          " set insu_cover = " + temp_cover + ", ";
                                //sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                                //sql_update = sql_update + " where receipt_id = '" + temp_id + "' ";

                                try
                                {
                                    if (temp_confrm.ToString().Equals("N"))
                                    {
                                        command.CommandText = " insert into cs_medicine_trans (trans_id, med_cd, trans_type, trans_date, trans_qty, receipt_id, insu_cover, ins_date, ins_emp, TRANS_REMARK) values " +
                                                    " (klinik.cs_medtrans_seq.nextval,'" + temp_code + "','OUT',to_date('" + s_date + "','yyyy-MM-dd'),'" + temp_q + "','" + temp_id + "', " + temp_cover + ", sysdate,'" + DB.vUserId + "' ,'" + tdrink + "') ";

                                        command.ExecuteNonQuery();
                                    }
                                    //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                                    //OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                                    //oraConnect.Open();
                                    //cm.ExecuteNonQuery();
                                    //oraConnect.Close();
                                    //cm.Dispose();

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("ERROR: " + ex.Message);
                                }
                            }

                            splan = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[15]).ToString();

                            if (splan.ToString().Equals("TRT01"))
                            {
                                string callid = "", age = "";
                                p_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                                gnder = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
                                s_name = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
                                age = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[16]).ToString();

                                sql_all = "";
                                sql_all = sql_all + @" select a.CALL_ID, TYPE_INS, a.que
                                        from KLINIK.CS_CALL_LOG a, KLINIK.cs_visit b
                                        where a.que = b.que01
                                        AND b.que02 = '" + p_que + @"'    
                                        AND b.id_visit = '" + visitid + @"'    
                                        AND TRUNC(A.INS_DATE) = TRUNC(VISIT_DATE)  ";

                                OleDbConnection oraConnect5 = ConnOra.Create_Connect_Ora();
                                OleDbDataAdapter adOra5 = new OleDbDataAdapter(sql_all, oraConnect5);
                                DataTable dt5 = new DataTable();
                                adOra5.Fill(dt5);
                                if (dt5.Rows.Count > 0)
                                {
                                    callid = dt5.Rows[0]["CALL_ID"].ToString();
                                    rm_type = dt5.Rows[0]["TYPE_INS"].ToString();
                                    q_no2 = dt5.Rows[0]["que"].ToString();
                                }

                                //{
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

                                teks = "Nomor Antrian " + q_no2 + " " + p1 + p2 + " Silahkan Menuju Ke Kasir";

                                sql_all = "";
                                sql_all = @"UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'W', type_ins ='PAY', stat ='Kasir', param = '" + teks + "' WHERE CALL_ID = " + callid + "";

                                ORADB.Execute(ORADB.XE, sql_all);
                            }

                            trans.Commit();

                            LoadData();
                            //MessageBox.Show(sql_insert);
                            //MessageBox.Show("Query Exec : " + sql_insert);

                            labelControl5.Visible = true;
                            labelControl5.Text = "Konfirmasi Berhasil";
                            Blinking(labelControl5, 1);
                            //MessageBox.Show("Data Berhasil di Konfirmasi.");
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
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string s_nik = "", s_que = "", s_date = "", sql_his = "", s_rm = "", sql_cek = "", payst = "",s_stat="";

            s_nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[11]).ToString();
            s_stat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[6]).ToString();

            sql_cek = sql_cek + Environment.NewLine + "select status  ";
            sql_cek = sql_cek + Environment.NewLine + "from cs_treatment_head ";
            sql_cek = sql_cek + Environment.NewLine + "where patient_no='" + s_nik + "' ";
            sql_cek = sql_cek + Environment.NewLine + "and to_char(visit_date,'yyyy-MM-dd')='" + s_date + "' ";
            sql_cek = sql_cek + Environment.NewLine + "and visit_no='" + s_que + "' and id_visit = " + visitid + " ";

            OleDbConnection sqlConnectc = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSqlc = new OleDbDataAdapter(sql_cek, sqlConnectc);
            DataTable dtc = new DataTable();
            adSqlc.Fill(dtc);

            if (dtc.Rows.Count > 0)
            {
                payst = dtc.Rows[0]["status"].ToString();
            }

            if (payst == "CLS")
            {
                MessageBox.Show("Data tidak bisa dirubah.");
                return;
            }

            if (MessageBox.Show("Anda yakin akan melakukan proses cancel?",
                      "Message",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                sql_his = " select a.rm_no, b.patient_no, a.visit_no, med_cd, med_qty, receipt_id " +
                      " from cs_receipt a " +
                      " join cs_patient b on (a.rm_no = b.rm_no) " +
                      " where b.status = 'A' " +
                      " and a.confirm = 'Y' " +
                      " and b.patient_no = '" + s_nik + "' " +
                      " and to_char(insp_date, 'yyyy-MM-dd') = '" + s_date + "' " +
                      " and visit_no = '" + s_que + "' and id_visit = " + visitid + "  ";

                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_his, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);


                if (dt.Rows.Count > 0)
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

                        if (s_stat == "INP")
                        {

                        }
                        else
                        {
                            command.CommandText = " update cs_visit set time_receipt = null, time_end = sysdate, status = 'MED',  " +
                                              " upd_date = sysdate, upd_emp = '" + DB.vUserId + "'  " +
                                              " where patient_no = '" + s_nik + "' and to_char(visit_date,'yyyy-MM-dd') = '" + s_date + "' " +
                                              " and que01 = '" + s_que + "' and id_visit = " + visitid + "  ";
                            command.ExecuteNonQuery();
                        }

                        s_rm = dt.Rows[0]["rm_no"].ToString();
                        command.CommandText = " update cs_receipt set confirm = 'N', upd_emp = '" + DB.vUserId + "', upd_date = sysdate " +
                                              " where rm_no = '" + s_rm + "' and to_char(insp_date,'yyyy-MM-dd') = '" + s_date + "' and visit_no = '" + s_que + "' and confirm = 'Y' and id_visit = " + visitid + "  ";

                        command.ExecuteNonQuery();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string temp_cd = "", temp_qty = "", temp_id = "";
                            //listDiagnosa.Add(new Diagnosa() { diagnosaCode = dt.Rows[i]["item_cd"].ToString(), diagnosaName = dt.Rows[i]["item_name"].ToString() });
                            temp_id = dt.Rows[i]["receipt_id"].ToString();

                            command.CommandText = " delete from cs_medicine_trans where receipt_id = '" + temp_id + "' ";

                            command.ExecuteNonQuery();
                        }

                        trans.Commit();
                        LoadData();
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

        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.Column.Caption == "Confirm")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);
                if (kk == "Y")
                {
                    //e.Appearance.BackColor = Color.FromArgb(150, Color.Blue);
                    //e.Appearance.BackColor2 = Color.FromArgb(150, Color.Blue);

                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.Caption == "Stok")
            {
                string stok = View.GetRowCellDisplayText(e.RowHandle, View.Columns[5]);

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
                    else if (Convert.ToInt32(stok) > 20)
                    {
                        e.Appearance.BackColor = Color.FromArgb(150, Color.Green);
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }

            }

            if (e.Column.Caption == "BPJS")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[3]);
                if (kk == "Yes")
                {
                    //e.Appearance.BackColor = Color.FromArgb(150, Color.Blue);
                    //e.Appearance.BackColor2 = Color.FromArgb(150, Color.Blue);

                    e.Appearance.BackColor = Color.FromArgb(150, Color.Green);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.Column.Caption == "Confirm")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);
                if (kk == "N")
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Y")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.Caption == "Status")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);
                if (kk == "Obat" || kk == "Sudah Bayar")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Green);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.Green);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Pembayaran")
                {
                    e.Appearance.BackColor = Color.MediumPurple;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Rawat Inap")
                {
                    e.Appearance.BackColor = Color.Orange;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

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

        private void gridView3_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView3_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Stok Saat Ini")
            {
                string stok = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;

                if (stok != "")
                {
                    if (Convert.ToInt32(stok) <= 0)
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnCompltd_Click(object sender, EventArgs e)
        {
            string s_nik = "", s_que = "", s_date = "", sql_his = "", s_rm = "", s_edit = "", sql_cek = "", payst = "", s_stat = "" ,smstock ="", spoli ="";
            string s_tipe = "", sql_update="", p_kirid ="";

            if (gridView1.RowCount < 1)
                return;

            s_nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            s_stat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[6]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[11]).ToString();
            s_edit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[12]).ToString();
            s_tipe = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[13]).ToString();
            smstock = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            spoli = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();
            p_kirid = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();

            if (spoli.ToString().Equals("Lain-lain"))
            {
                sql_update = "";
                sql_update = sql_update + " update cs_kir set  STAT_PAY  = 'X', UPD_DATE = sysdate, UPD_EMP =  '" + DB.vUserId + "' ";
                sql_update = sql_update + " where  KIR_ID = " + p_kirid + " and STAT_PAY = 'Y' ";

            }
            else
            {
                sql_update = "";
                sql_update = " update cs_visit set time_end = sysdate, status = 'CLS',  " +
                             " upd_date = sysdate, upd_emp = '" + DB.vUserId + "'  " +
                             " where patient_no = '" + s_nik + "' " +
                             " and id_visit = '" + visitid + "' ";
            } 

            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                oraConnect.Open();
                cm.ExecuteNonQuery();
                oraConnect.Close();
                cm.Dispose();

                LoadData();

                MessageBox.Show("Data Berhasil disimpan.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }

            
        }

        private void btnCall_Click(object sender, EventArgs e)
        { 
            string sql_check5 = "", rm_number = "", p_que = "", visitid = "", sql1 = "", p_que2 ="" , fstat ="", fcallid = "";

            if (gridView1.RowCount < 1)
                return;

            p_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            visitid = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();

            sql_check5 = "";
            sql_check5 = sql_check5 + @" select TYPE_INS, a.que, STATUS, A.CALL_ID
                                           from KLINIK.CS_CALL_LOG a, KLINIK.cs_visit b
                                          where a.que = b.que01
                                            AND b.que02 = '" + p_que + @"'   
                                            AND TRUNC(A.INS_DATE) = TRUNC(VISIT_DATE) and id_visit = '" + visitid + @"'     ";


            //sql_check5 = sql_check5 + "select TYPE_INS from KLINIK.CS_CALL_LOG where  QUE = '" + p_que + "' AND TRUNC(INS_DATE) = TRUNC(SYSDATE)";

            OleDbConnection oraConnect5 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra5 = new OleDbDataAdapter(sql_check5, oraConnect5);
            DataTable dt5 = new DataTable();
            adOra5.Fill(dt5);
            if (dt5.Rows.Count > 0)
            {
                rm_number = dt5.Rows[0]["TYPE_INS"].ToString();
                p_que2 = dt5.Rows[0]["que"].ToString();
                fstat = dt5.Rows[0]["STATUS"].ToString();
                fcallid = dt5.Rows[0]["CALL_ID"].ToString();
            }

            if (rm_number.ToString().Equals("MED") && !fstat.ToString().Equals("CLS"))
            {
                sql1 = " ";
                sql1 = @"UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'N', STAT = 'Closed' WHERE CALL_ID = " + fcallid + "  ";

                ORADB.Execute(ORADB.XE, sql1); 
            }
            else if(fstat.ToString().Equals("CLS"))
            {
                MessageBox.Show("Maaf Pasien sudah Closed, Tidak Dapat Dipanggil Ulang.");
                return;
            }
            else
            {
                MessageBox.Show("Maaf Pasien sudah di Proses, Tidak Dapat Dipanggil Di Bagian Farmasi.");
                return;
            }



            //if (p_que != "")
            //{
            //    p1 = p_que.Substring(0, 1);
            //    p2 = p_que.Substring(1, 1);
            //    p3 = p_que.Substring(2, 1);
            //    p4 = p_que.Substring(3, 1);
            //}


            //if (s_gender == "Perempuan")
            //{
            //    p1 = "Ibu";
            //}
            //else
            //{
            //    p1 = "Bapak";
            //}

            //p2 = s_name;

            //teks = p1 + p2 + " silahkan menuju ke loket obat";

            //loading.ShowWaitForm();
            //try
            //{
            //    sql_insert = "";
            //    sql_insert = sql_insert + " insert into cs_call_log (call_id, que, type_ins, stat, param, flag, ins_emp, ins_date) ";
            //    sql_insert = sql_insert + " values (cs_call_log_seq.nextval,'" + p_que + "','DOC','Medicine','" + teks + "','N','" + DB.vUserId + "',sysdate)";

            //    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            //    OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect);
            //    oraConnect.Open();
            //    cm.ExecuteNonQuery();
            //    oraConnect.Close();
            //    cm.Dispose();

            //    SoundPlayer player = new SoundPlayer(p_dir + "suara_antrian1" + fname);
            //    player.PlaySync();
            //    urltts = string.Format("https://translate.googleapis.com/translate_tts?ie=UTF-8&q={0}&tl={1}&total=1&idx=0&textlen={2}&client=gtx", HttpUtility.UrlEncode(teks, Encoding.GetEncoding("utf-8")), "id" + "-gb&q=", teks.Length);
            //    PlayMp3FromUrl(urltts);
            //    SoundPlayer player2 = new SoundPlayer(p_dir + "suara_antrian2" + fname);
            //    player2.PlaySync();

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

        private void gridView3_CellMerge(object sender, CellMergeEventArgs e)
        {
            //gridView3.Columns[11].Caption = "ID";
            //gridView3.Columns[12].Caption = "RACIKAN";
            //gridView3.Columns[2].Caption = "Dosis";
            //gridView3.Columns[13].Caption = "QTY";
            //gridView3.Columns[14].Caption = "NOTE";
            //gridView3.Columns[15].Caption = "CARA";

            if (e.Column.FieldName == "ID")
            {
                GridView view = sender as GridView;
                string strval1 = gridView3.GetRowCellValue(e.RowHandle1, "ID_RACIK").ToString();
                string strval2 = gridView3.GetRowCellValue(e.RowHandle2, "ID_RACIK").ToString(); 

                e.Merge = (strval1 == strval2  );
                e.Handled = true;
            }
            if (e.Column.FieldName == "RACIKAN")
            {
                GridView view = sender as GridView;
                string strval1 = gridView3.GetRowCellValue(e.RowHandle1, "ID_RACIK").ToString();
                string strval2 = gridView3.GetRowCellValue(e.RowHandle2, "ID_RACIK").ToString();

                string strval3 = gridView3.GetRowCellValue(e.RowHandle1, "RACIKAN").ToString();
                string strval4 = gridView3.GetRowCellValue(e.RowHandle2, "RACIKAN").ToString(); 

                e.Merge = (strval1 == strval2 && strval3 == strval4 );
                e.Handled = true;
            }
            if (e.Column.FieldName == "CARA")
            {
                GridView view = sender as GridView;
                string strval1 = gridView3.GetRowCellValue(e.RowHandle1, "ID_RACIK").ToString();
                string strval2 = gridView3.GetRowCellValue(e.RowHandle2, "ID_RACIK").ToString();

                string strval3 = gridView3.GetRowCellValue(e.RowHandle1, "RACIKAN").ToString();
                string strval4 = gridView3.GetRowCellValue(e.RowHandle2, "RACIKAN").ToString();

                string strval5 = gridView3.GetRowCellValue(e.RowHandle1, "CARA").ToString();
                string strval6 = gridView3.GetRowCellValue(e.RowHandle2, "CARA").ToString();

                e.Merge = (strval1 == strval2 && strval3 == strval4 && strval5 == strval6);
                e.Handled = true;
            }

            if (e.Column.FieldName == "NOTE")
            {
                GridView view = sender as GridView;
                string strval1 = gridView3.GetRowCellValue(e.RowHandle1, "ID_RACIK").ToString();
                string strval2 = gridView3.GetRowCellValue(e.RowHandle2, "ID_RACIK").ToString();

                string strval3 = gridView3.GetRowCellValue(e.RowHandle1, "RACIKAN").ToString();
                string strval4 = gridView3.GetRowCellValue(e.RowHandle2, "RACIKAN").ToString();

                string strval5 = gridView3.GetRowCellValue(e.RowHandle1, "NOTE").ToString();
                string strval6 = gridView3.GetRowCellValue(e.RowHandle2, "NOTE").ToString();

                e.Merge = (strval1 == strval2 && strval3 == strval4 && strval5 == strval6);
                e.Handled = true;
            }
            if (e.Column.FieldName == "REMARK")
            {
                GridView view = sender as GridView;
                string strval1 = gridView3.GetRowCellValue(e.RowHandle1, "ID_RACIK").ToString();
                string strval2 = gridView3.GetRowCellValue(e.RowHandle2, "ID_RACIK").ToString();

                string strval3 = gridView3.GetRowCellValue(e.RowHandle1, "RACIKAN").ToString();
                string strval4 = gridView3.GetRowCellValue(e.RowHandle2, "RACIKAN").ToString();

                string strval5 = gridView3.GetRowCellValue(e.RowHandle1, "REMARK").ToString();
                string strval6 = gridView3.GetRowCellValue(e.RowHandle2, "REMARK").ToString();

                e.Merge = (strval1 == strval2 && strval3 == strval4 && strval5 == strval6);
                e.Handled = true;
            }
            //if (e.Column.FieldName == "Dosis")
            //{
            //    GridView view = sender as GridView;
            //    string strval1 = bandFreedom.GetRowCellValue(e.RowHandle1, "C_LINE").ToString();
            //    string strval2 = bandFreedom.GetRowCellValue(e.RowHandle2, "C_LINE").ToString();

            //    string strval3 = bandFreedom.GetRowCellValue(e.RowHandle1, "MACHINO").ToString();
            //    string strval4 = bandFreedom.GetRowCellValue(e.RowHandle2, "MACHINO").ToString();

            //    string strval5 = bandFreedom.GetRowCellValue(e.RowHandle1, "COLORI").ToString();
            //    string strval6 = bandFreedom.GetRowCellValue(e.RowHandle2, "COLORI").ToString();

            //    string strval9 = bandFreedom.GetRowCellValue(e.RowHandle1, "C_MODEL").ToString();
            //    string strval10 = bandFreedom.GetRowCellValue(e.RowHandle2, "C_MODEL").ToString();

            //    e.Merge = (strval1 == strval2 && strval3 == strval4 && strval5 == strval6 && strval9 == strval10);
            //    e.Handled = true;
            //}
            //if (e.Column.FieldName == "C_STYLE")
            //{
            //    GridView view = sender as GridView;
            //    string strval1 = bandFreedom.GetRowCellValue(e.RowHandle1, "C_LINE").ToString();
            //    string strval2 = bandFreedom.GetRowCellValue(e.RowHandle2, "C_LINE").ToString();

            //    string strval3 = bandFreedom.GetRowCellValue(e.RowHandle1, "MACHINO").ToString();
            //    string strval4 = bandFreedom.GetRowCellValue(e.RowHandle2, "MACHINO").ToString();

            //    string strval5 = bandFreedom.GetRowCellValue(e.RowHandle1, "COLORI").ToString();
            //    string strval6 = bandFreedom.GetRowCellValue(e.RowHandle2, "COLORI").ToString();

            //    string strval7 = bandFreedom.GetRowCellValue(e.RowHandle1, "C_MODEL").ToString();
            //    string strval8 = bandFreedom.GetRowCellValue(e.RowHandle2, "C_MODEL").ToString();

            //    string strval9 = bandFreedom.GetRowCellValue(e.RowHandle1, "C_STYLE").ToString();
            //    string strval10 = bandFreedom.GetRowCellValue(e.RowHandle2, "C_STYLE").ToString();

            //    e.Merge = (strval1 == strval2 && strval3 == strval4 && strval5 == strval6 && strval7 == strval8 && strval9 == strval10);
            //    e.Handled = true;
            //}
            //string value1 = gridView1.GetRowCellValue(e.RowHandle1, e.Column).ToString();
            //string value2 = gridView1.GetRowCellValue(e.RowHandle2, e.Column).ToString();

            //// Tentukan kondisi penggabungan berdasarkan nilai yang ada di dua sel
            //if (value1 == value2)
            //{
            //    e.Merge = true; // Gabungkan sel jika nilainya sama
            //}
            //else
            //{
            //    e.Merge = false; // Jangan gabungkan jika nilainya berbeda
            //}
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string s_nik = "", s_que = "", s_date = "", sql_his = "", s_rm = "", s_edit = "", sql_cek = "", payst = "", s_stat = "";
            string s_tipe = "", splan = "";
            string sql_all = "", gnder = "", p1 = "", p2 = "", teks = "", p_que = "", policd = "", rm_type = "", s_name = "", q_no2 = "";

            s_nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            s_stat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[6]).ToString();
            policd = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[11]).ToString();
            s_edit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[12]).ToString();
            s_tipe = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[13]).ToString();
            visitid = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();

            if (policd.ToString().Equals("Lain-lain"))
            {
                
            }
            else
            {

                sql_cek = "";
                sql_cek = sql_cek + Environment.NewLine + "select status  ";
                sql_cek = sql_cek + Environment.NewLine + "from cs_treatment_head ";
                sql_cek = sql_cek + Environment.NewLine + "where patient_no='" + s_nik + "' and id_visit = " + visitid + ""; 

                OleDbConnection sqlConnectc = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSqlc = new OleDbDataAdapter(sql_cek, sqlConnectc);
                DataTable dtc = new DataTable();
                adSqlc.Fill(dtc);

                if (dtc.Rows.Count > 0)
                {
                    payst = dtc.Rows[0]["status"].ToString();
                }

                if (payst == "CLS")
                {
                    MessageBox.Show("Data tidak bisa dirubah.");
                    return;
                }

                if (s_edit == "N")
                {
                    MessageBox.Show("Data tidak bisa dirubah. Silahkan melakukan adjusment.");
                    return;
                }

                sql_his = " select a.rm_no, b.patient_no, a.visit_no, med_cd, med_qty, receipt_id " +
                          " from cs_receipt a " +
                          " join cs_patient b on (a.rm_no = b.rm_no) " +
                          " where b.status = 'A' " +
                          " and a.confirm = 'N' " +
                          " and b.patient_no = '" + s_nik + "' and id_visit = " + visitid + " "; 

                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_his, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);  

                if (dt.Rows.Count > 0)
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

                        if (s_stat == "INP")
                        {

                        }
                        else
                        {
                            command.CommandText = " update cs_visit set time_receipt = sysdate, time_end = sysdate, status = 'PAY',  " +
                                              " upd_date = sysdate, upd_emp = '" + DB.vUserId + "'  " +
                                              " where patient_no = '" + s_nik + "'  and id_visit = " + visitid + " "; // and to_char(visit_date,'yyyy-MM-dd') = '" + s_date + "' " +
                                                                                                                      //" and que01 = '" + s_que + "' ";
                            command.ExecuteNonQuery();
                        }

                        s_rm = dt.Rows[0]["rm_no"].ToString();
                        command.CommandText = " update cs_receipt set confirm = 'Y', upd_emp = '" + DB.vUserId + "', upd_date = sysdate " +
                                              " where rm_no = '" + s_rm + "' and id_visit = " + visitid + " and confirm = 'N' ";

                        command.ExecuteNonQuery();
                          
                        for (int i = 0; i < gridView2.RowCount; i++)
                        { 
                            string temp_bpjs = "", temp_id = "", temp_cover = "", temp_code = "", temp_q = "", temp_confrm = "";

                            temp_code = gridView2.GetRowCellValue(i, gridView2.Columns[8]).ToString();
                            temp_q = gridView2.GetRowCellValue(i, gridView2.Columns[4]).ToString();
                            temp_id = gridView2.GetRowCellValue(i, gridView2.Columns[7]).ToString();
                            temp_bpjs = gridView2.GetRowCellValue(i, gridView2.Columns[3]).ToString();
                            temp_confrm = gridView2.GetRowCellValue(i, gridView2.Columns[6]).ToString();

                            if (temp_bpjs == "Y")
                            {
                                temp_cover = "0";
                            }
                            else
                            {
                                temp_cover = "1";
                            } 

                            try
                            {
                                if (temp_confrm.ToString().Equals("N"))
                                {
                                    command.CommandText = " insert into cs_medicine_trans (trans_id, med_cd, trans_type, trans_date, trans_qty, receipt_id, insu_cover, ins_date, ins_emp) values " +
                                                " (klinik.cs_medtrans_seq.nextval,'" + temp_code + "','OUT',to_date('" + s_date + "','yyyy-MM-dd'),'" + temp_q + "','" + temp_id + "', " + temp_cover + ", sysdate,'" + DB.vUserId + "') ";

                                    command.ExecuteNonQuery();
                                } 

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ERROR: " + ex.Message);
                            }
                        }
                         

                        trans.Commit();

                        ObatRacikan(visitid, s_nik);

                        int focusedRowHandle = gridView1.FocusedRowHandle;

                        if (focusedRowHandle >= 0) // Pastikan ada baris yang dipilih
                        {
                            var mouseArgs = new DevExpress.Utils.DXMouseEventArgs(
                                                                    System.Windows.Forms.MouseButtons.Left, // Tombol mouse (kiri)
                                                                    1,                                     // Jumlah klik
                                                                    0,                                     // Posisi X
                                                                    0,                                     // Posisi Y
                                                                    0                                      // Delta scroll (biasanya 0)
                                                                );

                            var rowClickArgs = new DevExpress.XtraGrid.Views.Grid.RowClickEventArgs(
                                                  mouseArgs, focusedRowHandle 
                                              );

                            // Panggil event RowClick
                            gridView1_RowClick(gridView1, rowClickArgs);
                            //gridView1_RowClick(gridView1, gridView1.FocusedRowHandle);
                        }
                        //gridView1_RowClick(sender,)
                        labelControl164.Visible = true;
                        labelControl164.Text = "Konfirmasi Racikan Berhasil";
                        Blinking(labelControl164, 1);

                        //MessageBox.Show("Data Obat Racikan Berhasil di Konfirmasi.");
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {

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
    }
}