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
using System.Globalization;
using Clinic.Report;
using DevExpress.XtraReports.UI;

namespace Clinic
{
    public partial class BillList : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<PatientType> listPatientType = new List<PatientType>();
        List<Status> listStat = new List<Status>();
        List<Status> listStat2 = new List<Status>();
        List<Status> listStat3 = new List<Status>();
        List<Status> listStat4 = new List<Status>();
        List<MedicineInfo> listMedicineInfo = new List<MedicineInfo>();
        List<Layanan> listLaya = new List<Layanan>();
        List<Stat> listGrpLaya = new List<Stat>();
        Terbilang terbilang = new Terbilang();
        DataSet dsBillRj = new DataSet();
        DataSet dsKwitansi = new DataSet();

        public string    v_name = "",  idvisit = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        int totPay = 0, totPayment = 0, totBill = 0, totcover = 0, ttlcover = 0, ttlsisa = 0;
        int totSelisih = 0;
        //string today = "2019-11-27";

        public BillList()
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
            listStat.Add(new Status() { statusCode = "PRE", statusName = "Registrasi" });
            listStat.Add(new Status() { statusCode = "RSV", statusName = "Reservasi" });
            listStat.Add(new Status() { statusCode = "NUR", statusName = "Pemeriksaan Awal" });
            listStat.Add(new Status() { statusCode = "INP", statusName = "Rawat Inap" });
            listStat.Add(new Status() { statusCode = "INS", statusName = "Pemeriksaan" });
            listStat.Add(new Status() { statusCode = "MED", statusName = "Obat" });
            listStat.Add(new Status() { statusCode = "PAY", statusName = "Pembayaran" });
            listStat.Add(new Status() { statusCode = "CLS", statusName = "Selesai" });
            listStat.Add(new Status() { statusCode = "DON", statusName = "Sudah Bayar" });
            listStat.Add(new Status() { statusCode = "CAN", statusName = "Batal" });

            listStat2.Clear();
            listStat2.Add(new Status() { statusCode = "", statusName = "All" });
            listStat2.Add(new Status() { statusCode = "INP", statusName = "Rawat Inap" });
            listStat2.Add(new Status() { statusCode = "INS", statusName = "Pemeriksaan" });
            listStat2.Add(new Status() { statusCode = "MED", statusName = "Obat" });
            listStat2.Add(new Status() { statusCode = "PAY", statusName = "Pembayaran" });
            listStat2.Add(new Status() { statusCode = "CLS", statusName = "Selesai" });

            listStat3.Clear();
            listStat3.Add(new Status() { statusCode = "OPN", statusName = "Belum Bayar" });
            listStat3.Add(new Status() { statusCode = "CLS", statusName = "Selesai" });
            listStat3.Add(new Status() { statusCode = "ADJ", statusName = "Adjusment" });
            listStat3.Add(new Status() { statusCode = "CAN", statusName = "Batal" });


            luStatus.Properties.DataSource = listStat2;
            luStatus.Properties.ValueMember = "statusCode";
            luStatus.Properties.DisplayMember = "statusCode";

            luStatus.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luStatus.Properties.DropDownRows = listStat2.Count;
            luStatus.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luStatus.Properties.AutoSearchColumnIndex = 1;
            luStatus.Properties.NullText = "";
            luStatus.ItemIndex = 0;

            listMedicineInfo.Clear();
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "A", medicineInfoName = "(P.C.) Sesudah Makan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "B", medicineInfoName = "(A.C.) Sebelum Makan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "C", medicineInfoName = "(P.R.N.) Bila Perlu" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "D", medicineInfoName = "(G.T.T) Diteteskan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "E", medicineInfoName = "(U.E) Pemakaian Luar" });

            dResDate.Text = today;

            tableLayoutPanel3.RowStyles[7] = new RowStyle(SizeType.Absolute, 0);
            tableLayoutPanel3.RowStyles[8] = new RowStyle(SizeType.Absolute, 0);
            tableLayoutPanel3.RowStyles[9] = new RowStyle(SizeType.Absolute, 0);

            string sql_lay = " select treat_type_id trt_id, initcap(treat_type_name) trt_name from KLINIK.cs_treatment_type where 1=1  ";
            OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_lay, oraConnectf);
            DataTable dtf = new DataTable();
            adOraf.Fill(dtf);
            listLaya.Clear();
            for (int i = 0; i < dtf.Rows.Count; i++)
            {
                listLaya.Add(new Layanan() { layananCode = dtf.Rows[i]["trt_id"].ToString(), layananName = dtf.Rows[i]["trt_name"].ToString() });
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

            listStat4.Clear();
            listStat4.Add(new Status() { statusCode = "", statusName = "All" });
            listStat4.Add(new Status() { statusCode = "B", statusName = "BPJS" });
            listStat4.Add(new Status() { statusCode = "U", statusName = "Umum" });
            listStat4.Add(new Status() { statusCode = "A", statusName = "Asuransi" });

            luTipe.Properties.DataSource = listStat4;
            luTipe.Properties.ValueMember = "statusCode";
            luTipe.Properties.DisplayMember = "statusCode";

            luTipe.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luTipe.Properties.DropDownRows = listStat4.Count;
            luTipe.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luTipe.Properties.AutoSearchColumnIndex = 1;
            luTipe.Properties.NullText = "";
            luTipe.ItemIndex = 0;

            comboBox1.Items.Clear();
            comboBox1.Items.Add("Rawat Jalan");
            comboBox1.Items.Add("Rawat Inap");
            comboBox1.Items.Add("Lain-Lain");
            comboBox1.SelectedIndex = 0;

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
            LoadDataLimit(); 
            btnCancel.Enabled = false;
            gridControl2.DataSource = null;
            lTotalPay.Text = "0";
            lTotalPayment.Text = "0";
            txtselisih.Text = "0";
            tDiskon.Text = "0";
            l_diskon.Text = "0";
        }

        private void PrescriptionList_Load(object sender, EventArgs e)
        {
            initData();
            LoadData();
            LoadDataLimit();
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "BillList");
            SoftBlink(labelControl6, Color.LightPink, Color.Red, 1600, false);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text.ToString().Equals("Lain-Lain"))
            {
                gridControl1.Visible = false;
                gridControl4.Visible = true;
                LoadData();
                btnCancel.Enabled = false;
                gridControl2.DataSource = null;
                lTotalPay.Text = "0";
                lTotalPayment.Text = "0";
            }
            else
            {
                gridControl1.Visible = true;
                gridControl4.Visible = false;
                LoadData();
                LoadDataLimit();
                btnCancel.Enabled = false;
                gridControl2.DataSource = null;
                lTotalPay.Text = "0";
                lTotalPayment.Text = "0";
            } 
        }

        private void LoadData()
        {
            string sql_search, tmp_month="", Sql = "";

            if (!comboBox1.Text.ToString().Equals("Lain-Lain"))
            { 
                sql_search = " ";
                //sql_search = sql_search + " select que02, a.empid, b.name, b.dept, gender, type_patient,  status,   " +
                //                          " case  when observation = 'Y' then 'Yes' else 'No' end as observation, visit_remark, 'S' action,  " +
                //                          " to_char(visit_date,'yyyy-mm-dd') visit_date, que01 " +
                //                          " from cs_visit a join cs_employees b on a.empid = b.empid " +
                //                          " where 1 = 1  " +
                //                          " and to_char(visit_date,'yyyy-mm-dd')= '" + dResDate.Text + "'  " +
                //                          " and status in ('OBS','MED','CLS') " +
                //                          " and status like '%" + luStatus.Text + "%' " +
                //                          " order by que02 ";

                tmp_month = dResDate.Text;
                tmp_month = tmp_month.Substring(0,7);

                sql_search = sql_search + Environment.NewLine + "select head_id, a.patient_no, a.rm_no, a.visit_no, to_char(a.visit_date,'yyyy-mm-dd') visit_date, ";
                sql_search = sql_search + Environment.NewLine + "     initcap(b.name)  name, b.address, treat_type_id, c.status, pay_status,  ";
                sql_search = sql_search + Environment.NewLine + "     insu_flag, decode(c.TYPE_PATIENT,'U','-',insu_no) insu_no, decode(c.TYPE_PATIENT,'U','-',insu_class)   insu_class, a.remarks, 'S' action,   ";
                sql_search = sql_search + Environment.NewLine + "     type_patient tipe, nvl(disc,0) disc, nvl(total_pay,0) total_pay, type_patient tipe1, insu_flag tipe2, c.id_visit, b.gender, d.POLI_NAME POLI,c.poli_cd,C.INPATIENT_ID, e.ANAMNESA_ID,round(((sysdate-b.birth_date)/30)/12) age    ";
                sql_search = sql_search + Environment.NewLine + "from KLINIK.cs_treatment_head a ";
                sql_search = sql_search + Environment.NewLine + "join KLINIK.cs_patient_info b on (a.patient_no=b.patient_no) ";
                sql_search = sql_search + Environment.NewLine + "join KLINIK.cs_visit c ON (a.id_visit = c.id_visit) join  cs_policlinic D ON (c.poli_cd = d.poli_cd) join cs_anamnesa e on(c.ID_VISIT = e.ID_VISIT) ";// on (a.patient_no=c.patient_no and a.visit_date=trunc(c.visit_date) and a.visit_no=c.que01) ";
                sql_search = sql_search + Environment.NewLine + "where 1=1 ";

                if (comboBox1.Text == "Rawat Jalan")
                {
                    sql_search = sql_search + Environment.NewLine + "  and treat_type_id <> 'TRT02'";
                    sql_search = sql_search + Environment.NewLine + "  and trunc(a.visit_date) > trunc(sysdate-1) "; // '" + dResDate.Text + "' ";
                    sql_search = sql_search + Environment.NewLine + "  and pay_status not in ('CAN') ";
                }
                else
                {
                    sql_search = sql_search + Environment.NewLine + "  and treat_type_id = 'TRT02'";
                    //sql_search = sql_search + Environment.NewLine + "  and to_char(a.visit_date,'yyyy-mm-dd') = '" + dResDate.Text + "' ";
                    sql_search = sql_search + Environment.NewLine + "  and pay_status not in ('CAN') ";
                } 
            
                sql_search = sql_search + Environment.NewLine + "  and c.status like '%" + luStatus.Text + "%' ";
                sql_search = sql_search + Environment.NewLine + "order by visit_date, a.visit_no ";

            
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

                    //btnSave.Enabled = false;

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

                    gridView1.Columns[0].Caption = "Head ID";
                    gridView1.Columns[1].Caption = "Pasien No";
                    gridView1.Columns[2].Caption = "RM No";
                    gridView1.Columns[3].Caption = "No Antrian";
                    gridView1.Columns[4].Caption = "Tanggal";
                    gridView1.Columns[5].Caption = "Nama";
                    gridView1.Columns[6].Caption = "Alamat";
                    gridView1.Columns[7].Caption = "Pelayanan";
                    gridView1.Columns[8].Caption = "Pemeriksaan";
                    gridView1.Columns[9].Caption = "Pembayaran";
                    gridView1.Columns[10].Caption = "Tipe";
                    gridView1.Columns[11].Caption = "No Asuransi";
                    gridView1.Columns[12].Caption = "Kelas";
                    gridView1.Columns[13].Caption = "Remarks";
                    gridView1.Columns[14].Caption = "Action";
                    gridView1.Columns[15].Caption = "Tipe Pasien";
                    gridView1.Columns[16].Caption = "Diskon";
                    gridView1.Columns[17].Caption = "Tot Bayar";
                    gridView1.Columns[18].Caption = "Tipe Pas";
                    gridView1.Columns[19].Caption = "Insu Flag";
                    gridView1.Columns[20].Caption = "visitid";
                    gridView1.Columns[21].Caption = "gender";
                    gridView1.Columns[22].Caption = "Poli";
                    gridView1.Columns[23].Caption = "PoliCD";
                    gridView1.Columns[24].Caption = "INPATIENT_ID";
                    gridView1.Columns[25].Caption = "ANAMNESA_ID";
                    gridView1.Columns[26].Caption = "AGE";
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
                    gridView1.Columns[8].ColumnEdit = statusLookup;
                    //gridView1.BestFitColumns();

                    RepositoryItemLookUpEdit statusLookup2 = new RepositoryItemLookUpEdit();
                    statusLookup2.DataSource = listStat3;
                    statusLookup2.ValueMember = "statusCode";
                    statusLookup2.DisplayMember = "statusName";

                    statusLookup2.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                    statusLookup2.DropDownRows = listStat3.Count;
                    statusLookup2.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                    statusLookup2.AutoSearchColumnIndex = 1;
                    statusLookup2.NullText = "";
                    gridView1.Columns[9].ColumnEdit = statusLookup2;

                    RepositoryItemLookUpEdit statusLookup3 = new RepositoryItemLookUpEdit();
                    statusLookup3.DataSource = listPatientType;
                    statusLookup3.ValueMember = "patientTypeCode";
                    statusLookup3.DisplayMember = "patientTypeName";

                    statusLookup3.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                    statusLookup3.DropDownRows = listPatientType.Count;
                    statusLookup3.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                    statusLookup3.AutoSearchColumnIndex = 1;
                    statusLookup3.NullText = "";
                    gridView1.Columns[15].ColumnEdit = statusLookup3;

                    RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
                    glLaya.DataSource = listLaya;
                    glLaya.ValueMember = "layananCode";
                    glLaya.DisplayMember = "layananName";

                    glLaya.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                    glLaya.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                    glLaya.ImmediatePopup = true;
                    glLaya.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                    glLaya.NullText = "";
                    gridView1.Columns[7].ColumnEdit = glLaya;

                    gridView1.Columns[0].Visible = false;
                    gridView1.Columns[1].Visible = true;
                    gridView1.Columns[2].Visible = true;
                    gridView1.Columns[3].Visible = true;
                    gridView1.Columns[6].Visible = false;
                    gridView1.Columns[13].Visible = false;
                    gridView1.Columns[10].Visible = false;
                    gridView1.Columns[11].Visible = false;
                    gridView1.Columns[12].Visible = false;
                    gridView1.Columns[14].Visible = false;
                    gridView1.Columns[16].Visible = false;
                    gridView1.Columns[17].Visible = false;
                    gridView1.Columns[18].Visible = false;
                    gridView1.Columns[19].Visible = false;
                    gridView1.Columns[20].Visible = false;
                    gridView1.Columns[21].Visible = false;
                    gridView1.Columns[23].Visible = false;
                    gridView1.Columns[24].Visible = false;
                    gridView1.Columns[25].Visible = false;
                    gridView1.Columns[26].Visible = false;
                    //gridView1.Columns[10].VisibleIndex = 0;

                    gridView1.Columns[4].Width = 50;
                    gridView1.Columns[7].Width = 50;
                    gridView1.Columns[8].Width = 50;
                    gridView1.Columns[9].Width = 50;
                    gridView1.Columns[10].Width = 50;
                    gridView1.Columns[12].Width = 40;
                    gridView1.Columns[22].Width = 70;

                    gridView1.BestFitColumns();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
            else
            {
                Sql = " ";
                Sql = Sql + Environment.NewLine + "select kir_id, regis_date regis_date, nid, name, gender, f_type, decode(STAT_PAY,'N','Belum Bayar','Selesai') STAT_PAY, PRICE,TOTAL_PAY, TTLDISC ";
                Sql = Sql + Environment.NewLine + "  from cs_kir  ";
                Sql = Sql + Environment.NewLine + " where 1=1  and trunc(regis_date) = trunc(sysdate) ";
                Sql = Sql + Environment.NewLine + "   and STAT_PAY = decode('" + luStatus.Text + "','CLS','Y','N')   ";
                Sql = Sql + Environment.NewLine + " order by regis_date, name  ";

                try
                {
                    OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSql2 = new OleDbDataAdapter(Sql, sqlConnect);
                    DataTable dt2 = new DataTable();
                    adSql2.Fill(dt2);

                    gridControl4.DataSource = null;
                    gridView4.Columns.Clear();
                    gridControl4.DataSource = dt2;

                    gridView4.OptionsView.ColumnAutoWidth = true;
                    gridView4.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                    gridView4.Appearance.HeaderPanel.FontSizeDelta = 0;
                    gridView4.IndicatorWidth = 30;
                    gridView4.OptionsBehavior.Editable = false;
                    gridView4.BestFitColumns();

                    gridView4.Columns[0].Caption = "ID";
                    gridView4.Columns[1].Caption = "Register";
                    gridView4.Columns[2].Caption = "NIK KTP";
                    gridView4.Columns[3].Caption = "Nama";
                    gridView4.Columns[4].Caption = "JK";
                    gridView4.Columns[5].Caption = "Tipe";
                    gridView4.Columns[6].Caption = "Status";
                    gridView4.Columns[7].Visible = false;
                    gridView4.Columns[8].Visible = false;
                    gridView4.Columns[9].Visible = false;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                } 

            }
        }
        private void gridView4_RowClick(object sender, RowClickEventArgs e)
        {
            GridView View = sender as GridView;
            string s_kir_id = "", s_pay = "", s_price = "",s_disc ="", s_tot = "";

            if (gridView4.RowCount < 1)
                return;

            s_kir_id = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            s_pay = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);
            s_price = View.GetRowCellDisplayText(e.RowHandle, View.Columns[7]);
            s_tot  = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);
            s_disc = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);

            string Sql = " ";
            Sql = Sql + Environment.NewLine + "select kir_id TREAT_ITEM_ID, 'Pelayanan '||F_TYPE  TREAT_GROUP_ID, TREAT_ITEM_NAME, to_char(regis_date,'yyyy-MM-dd') TREAT_DATE, 1 TREAT_QTY, treat_item_price total_price, F_TYPE REMARKS, 'N' INSU, 0 RECEIPT_ID  ";
            Sql = Sql + Environment.NewLine + "from cs_kir a, cs_treatment_item b ";
            Sql = Sql + Environment.NewLine + "where 1=1  ";
            Sql = Sql + Environment.NewLine + "  and a.ID_ITEM_LAYANAN = b.treat_item_id "; 
            Sql = Sql + Environment.NewLine + "  and kir_id =" + s_kir_id + " ";           
            Sql = Sql + Environment.NewLine + "union all ";
            Sql = Sql + Environment.NewLine + "select  0 treat_item_id,  'TRG05' treat_group_id,  ";
            Sql = Sql + Environment.NewLine + "        initcap(med_name)  ||' ['||d.FORMULA||']' med_name,   ";
            Sql = Sql + Environment.NewLine + "        to_char(a.insp_date,'yyyy-MM-dd') insp_date,  a.med_qty, a.price,   ";
            Sql = Sql + Environment.NewLine + "        confirm  remarks, 'N' insu , a.RECEIPT_ID    ";
            Sql = Sql + Environment.NewLine + "  FROM KLINIK.cs_receipt a ";
            Sql = Sql + Environment.NewLine + "       JOIN KLINIK.CS_KIR c ON (a.ATT3_RECIEPT = c.KIR_ID) ";
            Sql = Sql + Environment.NewLine + "       JOIN KLINIK.cs_medicine b ON (a.med_cd = b.med_cd) ";
            Sql = Sql + Environment.NewLine + "       JOIN KLINIK.cs_formula D ON (B.med_cd = D.med_cd AND D.FORMULA_ID = A.formula) ";
            Sql = Sql + Environment.NewLine + " WHERE     b.status = 'A' ";
            Sql = Sql + Environment.NewLine + "       AND D.MINUS_STOK = 'Y' ";
            Sql = Sql + Environment.NewLine + "       AND BPJS_COVER = 'N' ";
            Sql = Sql + Environment.NewLine + "       AND c.KIR_ID = " + s_kir_id + " ";
            Sql = Sql + Environment.NewLine + "order by 2, 3   ";

            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(Sql, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);

            lTotalPay.Text = "0";
            totPay = 0;
            tDiskon.Text = s_disc.ToString();
            lTotalPayment.Text = "0";
            totBill = 0;
            txt_cover.Text = "0";
            totPayment = 0;
            ttlcover = 0; ttlsisa = 0; 

            if (dt2.Rows.Count > 0)
            {
                totPay = 0; // Convert.ToInt32(dt2.Rows[0]["total_price"].ToString());  
                string tmp = "" ;

                for (int i = 0; i < dt2.Rows.Count; i++)
                { 
                    tmp = dt2.Rows[i]["total_price"].ToString(); 
                    totPay = totPay + Convert.ToInt32(tmp);
                }
                lTotalPay.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", Convert.ToInt32(totPay));
            }


            if (s_disc != "0" && s_disc != "")
            {
                tDiskon.Text = s_disc.ToString();
                totBill = totPay;
                totPay = totPay - (totPay / Convert.ToInt32(s_disc));
                lTotalPayment.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", Convert.ToInt32(totPay));
                totPayment = Convert.ToInt32(totBill - (totBill / Convert.ToInt32(tDiskon.Text)));
                l_diskon.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", (totPay - Convert.ToInt32(s_tot)));
            }
            else
            {
                tDiskon.Text = "0"; l_diskon.Text = "0";
            }
            lTotalPayment.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", Convert.ToInt32(totPay));


            gridControl2.DataSource = null;
            gridView2.Columns.Clear();
            gridControl2.DataSource = dt2;

            gridView2.OptionsView.ColumnAutoWidth = true;
            gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView2.IndicatorWidth = 30;
            gridView2.OptionsBehavior.Editable = false;
            gridView2.BestFitColumns();

            gridView2.Columns[0].Caption = "Item ID";
            gridView2.Columns[1].Caption = "Group";
            gridView2.Columns[2].Caption = "Layanan";
            gridView2.Columns[3].Caption = "Tanggal";
            gridView2.Columns[4].Caption = "Jumlah";
            gridView2.Columns[5].Caption = "Harga";
            gridView2.Columns[6].Caption = "Remarks";
            gridView2.Columns[7].Caption = "Insu";
            gridView2.Columns[8].Caption = "receipt_id";
            //gridView2.Columns[7].VisibleIndex = 0;

            gridView2.Columns[0].Visible = false;
            gridView2.Columns[7].Visible = false;
            gridView2.Columns[8].Visible = false;

            chTangguh.CheckState = CheckState.Unchecked;
            chTangguh.Enabled = false; 

            if (s_pay == "PAY" || s_pay == "Selesai" )
            {
                btnPayment.Enabled = false;
                btnConfirm.Enabled = false;
                btnCancel.Enabled = true;
                simpleButton2.Enabled = true;
            }
            else
            {
                btnPayment.Enabled = true;
                btnConfirm.Enabled = true;
                btnCancel.Enabled = false;
                simpleButton2.Enabled = false;
            }  
            cktransfer();
        }
        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            
            GridView View = sender as GridView;
            string s_head = "", s_que = "", s_date = "", sql_his = "", s_check="", s_cnt="", s_pasno = "", s_action = "", act_cnt = "", act_name ="", s_act="", s_edit="",s_laya="";
            string s_rmno = "", s_tipe = "", s_insuno = "", s_kelas = "", s_stbyr = "", s_disc="", s_tot="", s_tipe1 = "", s_tipe2 = "", sstatus ="", sremark ="" ;

            if (gridView1.RowCount < 1)
                return;

            s_head = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            s_pasno = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
            s_rmno = View.GetRowCellDisplayText(e.RowHandle, View.Columns[2]);
            s_que = View.GetRowCellDisplayText(e.RowHandle, View.Columns[3]);
            s_date = View.GetRowCellDisplayText(e.RowHandle, View.Columns[4]); 
            s_laya = View.GetRowCellDisplayText(e.RowHandle, View.Columns[7]);
            sstatus = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);
            s_stbyr = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);
            s_tipe = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
            s_insuno = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);
            s_kelas = View.GetRowCellDisplayText(e.RowHandle, View.Columns[12]);
            s_disc = View.GetRowCellDisplayText(e.RowHandle, View.Columns[16]);
            s_tot = View.GetRowCellDisplayText(e.RowHandle, View.Columns[17]);
            s_tipe1 = View.GetRowCellDisplayText(e.RowHandle, View.Columns[18]);
            s_tipe2 = View.GetRowCellDisplayText(e.RowHandle, View.Columns[19]);
            idvisit = View.GetRowCellDisplayText(e.RowHandle, View.Columns[20]); 

            luTipe.EditValue = s_tipe;
            lInsuNo.Text = s_insuno;
            lKelas.Text = s_kelas;
            lTreatType.Text = s_laya;
           

            if (luTipe.GetColumnValue("statusCode").ToString() == "")
            {
                btnPayment.Enabled = false;
                btnPrint.Enabled = false;
            }
            else
            {
                btnPayment.Enabled = true;
                btnPrint.Enabled = true;
            }

            if (s_stbyr == "Belum Bayar")
            {
                tDiskon.Enabled = true;
            }
            else
            {
                tDiskon.Enabled = false;
            }


            if (s_tipe1 != s_tipe2)
            {
                btnPayment.Enabled = false;
                btnPrint.Enabled = false;
                MessageBox.Show("Data Tipe Pasien pada menu reservasi dan tagihan tidak sama");
            }
            else
            {
                btnPayment.Enabled = true;
                btnPrint.Enabled = true;
            }
            if (comboBox1.Text == "Rawat Jalan")
            {
                sql_his = " ";
                sql_his = sql_his + Environment.NewLine + " select a.treat_item_id, a.treat_group_id, a.treat_item_name,  ";
                sql_his = sql_his + Environment.NewLine + " to_char(treat_date,'yyyy-mm-dd') treat_date, treat_qty, ";
                sql_his = sql_his + Environment.NewLine + " b.total_price, b.remarks, decode(INSU_FLAG,'U','N','Y') insu,0  receipt_id ";
                sql_his = sql_his + Environment.NewLine + " from KLINIK.cs_treatment_item a ";
                sql_his = sql_his + Environment.NewLine + " join KLINIK.cs_treatment_detail b on (a.treat_item_id=b.treat_item_id) join KLINIK.cs_treatment_head c on (b.head_id=c.head_id) ";
                sql_his = sql_his + Environment.NewLine + " where b.head_id='" + s_head + "' ";
                sql_his = sql_his + Environment.NewLine + " union all";
                sql_his = sql_his + Environment.NewLine + " select 0 treat_item_id, 'TRG05' treat_group_id,   ";
                sql_his = sql_his + Environment.NewLine + "        case when a.jenis_obat = 'RACIK' then F.CODE_NAME ||' [Qty:'||A.ATT3_RECIEPT||'] - ' ||initcap(med_name)  ||' ['||e.FORMULA||']' else initcap(med_name)  ||' ['||e.FORMULA||']'  END med_name, ";
                if (s_tipe2 == "U")
                {
                    sql_his = sql_his + Environment.NewLine + " to_char(a.insp_date,'yyyy-mm-dd') insp_date,  nvl(d.TRANS_QTY,a.med_qty)  med_qty, nvl(d.TRANS_QTY,a.med_qty)*MED_PRICE price, ";
                }
                else
                {
                    sql_his = sql_his + Environment.NewLine + " to_char(a.insp_date,'yyyy-mm-dd') insp_date,   nvl(d.TRANS_QTY,a.med_qty) med_qty, price  price, ";
                }

                sql_his = sql_his + Environment.NewLine + " confirm  remarks, ";
                if (s_tipe2 == "U")
                {
                    sql_his = sql_his + Environment.NewLine + " 'N' insu  ";
                }
                else
                {
                    sql_his = sql_his + Environment.NewLine + " decode(d.insu_cover,0,'Y','N') insu  ";
                }
                sql_his = sql_his + Environment.NewLine + "      ,A.receipt_id  ";
                sql_his = sql_his + Environment.NewLine + " from KLINIK.cs_receipt a  ";
                sql_his = sql_his + Environment.NewLine + " join KLINIK.cs_patient b on (a.rm_no = b.rm_no)  ";
                sql_his = sql_his + Environment.NewLine + " join KLINIK.cs_medicine c on(a.med_cd = c.med_cd)  ";
                sql_his = sql_his + Environment.NewLine + " LEFT join KLINIK.cs_medicine_trans d on(a.receipt_id = d.receipt_id)  ";
                sql_his = sql_his + Environment.NewLine + " JOIN KLINIK.CS_FORMULA e on(a.FORMULA = e.FORMULA_ID) ";
                sql_his = sql_his + Environment.NewLine + " LEFT join cs_code_data f on (a.ATT1_RECIEPT = f.CODE_ID and f.CODE_CLASS_ID = 'MED_RACIK')  ";
                if (comboBox1.Text == "Rawat Jalan")
                {

                }
                else
                {
                    sql_his = sql_his + Environment.NewLine + " join KLINIK.cs_inpatient e on (a.rm_no=e.rm_no and a.visit_dt=e.reg_date)   ";
                }
                sql_his = sql_his + Environment.NewLine + " where b.status = 'A'  ";
                sql_his = sql_his + Environment.NewLine + " and c.status = 'A'  ";
                sql_his = sql_his + Environment.NewLine + " and b.patient_no = '" + s_pasno + "'  ";
                if (comboBox1.Text == "Rawat Jalan")
                {
                    sql_his = sql_his + Environment.NewLine + " and to_char(insp_date, 'yyyy-mm-dd') = '" + s_date + "'  ";
                }

                sql_his = sql_his + Environment.NewLine + " and a.id_visit = " + idvisit + " ";

            }
            else
            {
                sql_his = " ";
                sql_his = sql_his + Environment.NewLine + "select  a.treat_item_id, a.treat_group_id, a.treat_item_name,    ";
                sql_his = sql_his + Environment.NewLine + "        to_char(treat_date,'yyyy-mm-dd') treat_date, treat_qty,   ";
                sql_his = sql_his + Environment.NewLine + "        b.total_price, b.remarks, decode(INSU_FLAG,'U','N','Y') insu,0  receipt_id   ";
                sql_his = sql_his + Environment.NewLine + "   from KLINIK.cs_treatment_item a   ";
                sql_his = sql_his + Environment.NewLine + "   join KLINIK.cs_treatment_detail b on (a.treat_item_id=b.treat_item_id) join KLINIK.cs_treatment_head c on (b.head_id=c.head_id)  "; 
                sql_his = sql_his + Environment.NewLine + " where b.head_id= '" + s_head + "' and c.id_visit =" + idvisit + "  and b.F_ACTIVE ='Y'  ";
                sql_his = sql_his + Environment.NewLine + " union all  ";
                sql_his = sql_his + Environment.NewLine + " select 0 treat_item_id, 'TRG05' treat_group_id,  ";
                sql_his = sql_his + Environment.NewLine + " initcap(med_name)  ||' ['||e.FORMULA||']' med_name,  ";
                sql_his = sql_his + Environment.NewLine + " to_char(a.insp_date,'yyyy-mm-dd') insp_date,  a.med_qty, price,  ";
                sql_his = sql_his + Environment.NewLine + " confirm  remarks,  ";
                sql_his = sql_his + Environment.NewLine + " BPJS_COVER insu , a.RECEIPT_ID  ";
                sql_his = sql_his + Environment.NewLine + " from KLINIK.cs_visit z   ";
                sql_his = sql_his + Environment.NewLine + " join KLINIK.cs_receipt a on (a.id_visit = z.id_visit)    ";
                sql_his = sql_his + Environment.NewLine + " join KLINIK.cs_patient b on (a.rm_no = b.rm_no)   ";
                sql_his = sql_his + Environment.NewLine + " join KLINIK.cs_medicine c on(a.med_cd = c.med_cd)   ";
                sql_his = sql_his + Environment.NewLine + " LEFT join KLINIK.cs_medicine_trans d on(a.receipt_id = d.receipt_id and a.med_cd = d.med_cd)   ";
                sql_his = sql_his + Environment.NewLine + " JOIN KLINIK.CS_FORMULA e on(a.FORMULA = e.FORMULA_ID  )  ";
                sql_his = sql_his + Environment.NewLine + " join KLINIK.cs_inpatient f on ( z.inpatient_id=f.inpatient_id and f.status in('OPN','REG','CLS'))    ";
                sql_his = sql_his + Environment.NewLine + "where 1=1 ";
                sql_his = sql_his + Environment.NewLine + "  and b.status = 'A' and a.F_ACTIVE ='Y'  ";
                sql_his = sql_his + Environment.NewLine + "  and b.patient_no = '" + s_pasno + "'  ";
                sql_his = sql_his + Environment.NewLine + "  and a.id_visit =" + idvisit + " ";
                sql_his = sql_his + Environment.NewLine + "  UNION ALL "; 
                sql_his = sql_his + Environment.NewLine + "  select g.ROOM_HIS_ID treat_item_id, 'TRG04' treat_group_id,    ";
                sql_his = sql_his + Environment.NewLine + "      c.ROOM_NAME  ||' ['||F.BED_ID||']'  med_name,   ";
                sql_his = sql_his + Environment.NewLine + "      to_char(a.VISIT_DATE,'yyyy-mm-dd') insp_date, "; 
                sql_his = sql_his + Environment.NewLine + "       case when to_char(case when a.STATUS ='CLS' then TIME_END else sysdate end,'HH24MISS') < '120000' then   ";
                sql_his = sql_his + Environment.NewLine + "            case when ((((sysdate - TIME_RESERVATION)*24) ) - floor(((sysdate - TIME_RESERVATION)*24))) > 0.5 then floor((((sysdate - TIME_RESERVATION)*24)/24 ) +1) ";
                sql_his = sql_his + Environment.NewLine + "              else floor(((sysdate - TIME_RESERVATION)*24)/24 ) end   ";
                sql_his = sql_his + Environment.NewLine + "       else CEIL(TO_NUMBER(case when a.STATUS ='CLS' then TIME_END else sysdate end-TIME_RESERVATION))   ";
                sql_his = sql_his + Environment.NewLine + "        end  med_qty,   ";
                sql_his = sql_his + Environment.NewLine + "       case when to_char(case when a.STATUS ='CLS' then TIME_END else sysdate end,'HH24MISS') < '120000' then  ";
                sql_his = sql_his + Environment.NewLine + "            case when ((((sysdate - TIME_RESERVATION)*24) ) - floor(((sysdate - TIME_RESERVATION)*24))) > 0.5 then floor((((sysdate - TIME_RESERVATION)*24)/24 ) +1) ";
                sql_his = sql_his + Environment.NewLine + "              else floor(((sysdate - TIME_RESERVATION)*24)/24 ) end * ROOM_PRICE --*CEIL(TO_NUMBER((case when a.STATUS ='CLS' then TIME_END else sysdate end-1)-VISIT_DATE))  ";
                sql_his = sql_his + Environment.NewLine + "        else ROOM_PRICE*CEIL(TO_NUMBER(case when a.STATUS ='CLS' then TIME_END else sysdate end-TIME_RESERVATION))  ";
                sql_his = sql_his + Environment.NewLine + "       end   price,  "; 
                //sql_his = sql_his + Environment.NewLine + "      case when to_char(case when a.STATUS ='CLS' then TIME_END else sysdate end,'HH24MISS') < '120000' then  CEIL(TO_NUMBER((case when a.STATUS ='CLS' then TIME_END else sysdate end-1)-VISIT_DATE)) ";
                //sql_his = sql_his + Environment.NewLine + "        else CEIL(TO_NUMBER(case when a.STATUS ='CLS' then TIME_END else sysdate end-VISIT_DATE))  ";
                //sql_his = sql_his + Environment.NewLine + "        end  med_qty,  ";
                //sql_his = sql_his + Environment.NewLine + "        case when to_char(case when a.STATUS ='CLS' then TIME_END else sysdate end,'HH24MISS') < '120000' then CLASS_PRICE*CEIL(TO_NUMBER((case when a.STATUS ='CLS' then TIME_END else sysdate end-1)-VISIT_DATE)) ";
                //sql_his = sql_his + Environment.NewLine + "        else CLASS_PRICE*CEIL(TO_NUMBER(case when a.STATUS ='CLS' then TIME_END else sysdate end-VISIT_DATE))  ";
                //sql_his = sql_his + Environment.NewLine + "        end   price,   ";
                sql_his = sql_his + Environment.NewLine + "      'Ruangan' remarks,   ";
                sql_his = sql_his + Environment.NewLine + "      decode(TYPE_PATIENT,'B','Y','N') insu ,0  receipt_id    "; 
                sql_his = sql_his + Environment.NewLine + " from KLINIK.cs_visit a    ";
                sql_his = sql_his + Environment.NewLine + " join KLINIK.cs_patient b on (a.patient_no = b.patient_no)    ";
                sql_his = sql_his + Environment.NewLine + " join KLINIK.cs_inpatient e on (a.INPATIENT_ID = e.INPATIENT_ID  and e.status in('OPN','REG','CLS') )    ";
                sql_his = sql_his + Environment.NewLine + " JOIN KLINIK.cs_bed F on(F.BED_ID = e.ROOM_ID   ) ";
                sql_his = sql_his + Environment.NewLine + " JOIN KLINIK.CS_ROOM c on(c.ROOM_ID = F.ROOM_ID  ) ";
                sql_his = sql_his + Environment.NewLine + " JOIN KLINIK.CS_ROOM_CLASS d on(c.CLASS_ID = d.CLASS_ID  )    "; 
                sql_his = sql_his + Environment.NewLine + " LEFT JOIN (select max(ROOM_HIS_ID) ROOM_HIS_ID ,INPATIENT_ID, max(ROOM_ID) ROOM_ID from KLINIK.CS_ROOM_HIS  ";
                sql_his = sql_his + Environment.NewLine + "  group by INPATIENT_ID)  g on (e.INPATIENT_ID = g.INPATIENT_ID and F.BED_ID = g.ROOM_ID )  "; 
                sql_his = sql_his + Environment.NewLine + "where 1=1 ";
                sql_his = sql_his + Environment.NewLine + "  and b.status = 'A'    ";
                sql_his = sql_his + Environment.NewLine + "  and b.patient_no ='" + s_pasno + "'  ";
                sql_his = sql_his + Environment.NewLine + "  and a.id_visit = " + idvisit + " "; 

            }
            sql_his = sql_his + Environment.NewLine + " order by 2,3  ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_his, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            
            lTotalPay.Text = "0";
            totPay = 0;
            tDiskon.Text = s_disc.ToString();
            l_diskon.Text = "0";
            lTotalPayment.Text = "0";
            totBill = 0;
            txt_cover.Text = "0";
            totPayment = 0;
            ttlcover = 0; ttlsisa = 0;


            if (dt.Rows.Count > 0)
            {
                string tmp = "", sinsu = "" ;
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sinsu = dt.Rows[i]["insu"].ToString();
                    tmp = dt.Rows[i]["total_price"].ToString();
                    sremark = dt.Rows[i]["remarks"].ToString();
                    if (comboBox1.Text == "Rawat Inap" && sremark == "Ruangan")
                    {
                        if(tmp == "")
                        {
                            MessageBox.Show("Pasien Belum Melakukan Pemeriksaan..!!!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                        }
                    }
                    if (s_tipe.ToString().Equals("B"))
                    {
                        if (sinsu.ToString().Equals("Y"))
                        {
                            ttlcover = ttlcover + Convert.ToInt32(tmp);
                        }
                        else
                        {
                            ttlsisa = ttlsisa + Convert.ToInt32(tmp);
                        }
                    }
                    else
                    {
                        ttlcover = ttlcover + Convert.ToInt32(tmp);
                        ttlsisa = 0;
                    }
                    
                    totPay = totPay+ Convert.ToInt32(tmp);
                } 
            }

            

            if (s_tipe2.ToString().Equals("B"))
            {
                if (ttlsisa.ToString().Equals("0"))
                {
                    //totPay = Convert.ToInt32(ttlcover);
                    tDiskon.Text = "0";
                    tDiskon.Enabled = false;
                    tDiskon.BackColor = Color.LightGray;
                }   
                else
                {
                    //totPay = Convert.ToInt32(ttlsisa);
                    tDiskon.Enabled = true;
                    tDiskon.BackColor = Color.SandyBrown;
                }
                txtselisih.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", ttlsisa);
            }
            else if (s_tipe2.ToString().Equals("U"))
            {
                //totPay = totPay + Convert.ToInt32(totPay);
                tDiskon.Enabled = true;
                tDiskon.BackColor = Color.SandyBrown;
                if(s_stbyr.ToString().Equals("Selesai"))
                {
                    txtselisih.Text = "0";
                }
                else
                {
                    txtselisih.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", totPay);
                } 
            }
            else  
            {
                //totPay = totPay + Convert.ToInt32(totPay);
                tDiskon.Enabled = true;
                tDiskon.BackColor = Color.SandyBrown;
                txtselisih.Text = "0";
            }


            lTotalPay.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", totPay);
           

            if (s_disc == "0")
            {
                tDiskon.Text = "0"; 
                

                if ( s_tipe2 == "B")
                {
                    txt_cover.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", ttlcover);
                    txtselisih.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", ttlsisa);

                    lTotalPayment.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", ttlsisa);
                    totBill = ttlsisa;
                    totPayment = ttlsisa;

                }
                else if (s_tipe2 == "P" )
                {
                    txt_cover.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", totPay);
                    txtselisih.Text = "0";

                    lTotalPayment.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", totPay);
                    totBill = totPay;
                    totPayment = totPay;
                }
                else
                {
                    txt_cover.Text = "0";
                    txtselisih.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", totPay);

                    lTotalPayment.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", totPay);
                    totBill = totPay;
                    totPayment = totPay;
                } 
            }
            else
            {
                tDiskon.Text = s_disc.ToString();
                if (s_stbyr.ToString().Equals("Selesai"))
                {
                    totPayment = Convert.ToInt32(totPay - ((Convert.ToDecimal(tDiskon.Text)/100) * totPay));
                    lTotalPayment.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", Convert.ToInt32(totPayment));
                }
                else
                {
                    totPayment = ttlsisa;
                    lTotalPayment.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", Convert.ToInt32(ttlsisa));
                } 
                totBill = ttlsisa;
                
                if (s_tipe2 != "U")
                { 
                    txt_cover.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", (totPay- totBill));                    
                    chTangguh.CheckState = CheckState.Checked;
                }
                else
                {
                    txt_cover.Text = "0";
                    l_diskon.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", (totPay - Convert.ToInt32(totPayment)));
                    chTangguh.CheckState = CheckState.Unchecked;
                }
            }

            if (s_tipe2 == "U" || s_tipe2 == "B")
            {
                chTangguh.CheckState = CheckState.Unchecked;
                chTangguh.Enabled = false;
            }
            else
            {
                chTangguh.Enabled = true ;
            }

            gridControl2.DataSource = null;
            gridView2.Columns.Clear();
            gridControl2.DataSource = dt;

            gridView2.OptionsView.ColumnAutoWidth = true;
            gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView2.IndicatorWidth = 30;
            gridView2.OptionsBehavior.Editable = false;
            gridView2.BestFitColumns();

            gridView2.Columns[0].Caption = "Item ID";
            gridView2.Columns[1].Caption = "Group";
            gridView2.Columns[2].Caption = "Layanan";
            gridView2.Columns[3].Caption = "Tanggal";
            gridView2.Columns[4].Caption = "Jumlah";
            gridView2.Columns[5].Caption = "Harga";
            gridView2.Columns[6].Caption = "Remarks";
            gridView2.Columns[7].Caption = "Insu";
            gridView2.Columns[8].Caption = "receipt_id";
            //gridView2.Columns[7].VisibleIndex = 0;

            gridView2.Columns[0].Visible = false;
            gridView2.Columns[7].Visible = true ;
            gridView2.Columns[8].Visible = false;
            RepositoryItemLookUpEdit grpLookup = new RepositoryItemLookUpEdit();
            grpLookup.DataSource = listGrpLaya;
            grpLookup.ValueMember = "statCode";
            grpLookup.DisplayMember = "statName";

            grpLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            grpLookup.DropDownRows = listGrpLaya.Count;
            grpLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            grpLookup.AutoSearchColumnIndex = 1;
            grpLookup.NullText = "";
            gridView2.Columns[1].ColumnEdit = grpLookup;

            gridView2.Columns[5].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum; 

            gridView2.Columns[5].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView2.Columns[5].DisplayFormat.FormatString = "#,#";

            if (s_stbyr == "CLS")
            {
                btnConfirm.Enabled = false;
                btnCancel.Enabled = true;
            }
            else
            {
                btnConfirm.Enabled = true;
                btnCancel.Enabled = false;
            }

            if (s_stbyr == "Belum Bayar")
            {
                btnPayment.Enabled = true;
                btnPrint.Enabled = false;
            }
            else
            {
                btnPayment.Enabled = false;
                btnPrint.Enabled = true;
            }
            if (sstatus.ToString().Equals("Sudah Bayar") && s_stbyr.ToString().Equals("Selesai"))
                simpleButton2.Enabled = true;
            else
                simpleButton2.Enabled = false;

            LoadDataLimit();
            cktransfer();

        }

        private void txt_cover_EditValueChanged(object sender, EventArgs e)
        {
            decimal p_cvr = 0, p_sisa = 0, p_paying = 0 ;
            decimal p_bill = 0, p_dis = 0, p_pay = 0, p_temp = 0;
            string s_tipe = "";
            if (!txt_cover.Text.Equals("") || !txt_cover.Text.Equals("0"))
            {
                p_cvr = decimal.Parse(txt_cover.Text.Replace(".", ""));
                p_paying = decimal.Parse(lTotalPay.Text.Replace(".", ""));
                if (p_paying > 0 && p_cvr > 0)
                {
                    if (p_paying > p_cvr )
                    {
                        p_sisa = p_paying - p_cvr;
                        txtselisih.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", p_sisa);

                        totSelisih = Convert.ToInt32(p_sisa);

                        if (!tDiskon.Text.Equals("") || !tDiskon.Text.Equals("0"))
                        {
                            p_bill = decimal.Parse(totPay.ToString());
                            p_dis = decimal.Parse(tDiskon.Text);
                            p_pay = decimal.Parse(totPay.ToString());

                            s_tipe = gridView1.GetRowCellDisplayText(gridView1.FocusedRowHandle, gridView1.Columns[19]);

                            if (p_bill > 0 && p_dis > 0)
                            {
                                p_temp = p_dis * p_sisa / 100;
                                p_pay = p_sisa - p_temp;
                                totPayment = Convert.ToInt32(p_pay);
                                totcover = Convert.ToInt32(p_sisa);
                                //lTotalPayment.Text = p_pay.ToString();
                                lTotalPayment.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", p_pay);
                                l_diskon.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", p_temp);
                                //txt_cover.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", totcover);
                                if (s_tipe.ToString().Equals("B"))
                                {
                                    //txt_cover.Text = "0";
                                    chTangguh.CheckState = CheckState.Unchecked;
                                }
                                else
                                {
                                    //txt_cover.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", p_pay);
                                    chTangguh.CheckState = CheckState.Checked;
                                }
                            }
                            else
                            {
                                if (p_sisa > 0)
                                {
                                    totPayment = Convert.ToInt32(p_sisa);
                                    totcover = Convert.ToInt32(p_sisa);
                                    lTotalPayment.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", p_sisa);
                                }
                                else
                                {
                                    totPayment = totPay;
                                    totcover = totPayment;
                                    lTotalPayment.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", totPayment);
                                }
                                
                                //txt_cover.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", totcover);
                                l_diskon.Text = "0";
                                if (s_tipe.ToString().Equals("B"))
                                {
                                    //txt_cover.Text = "0";
                                    chTangguh.CheckState = CheckState.Unchecked;
                                }
                                else
                                {
                                    //txt_cover.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", totPayment);
                                    chTangguh.CheckState = CheckState.Checked;
                                }
                            }
                        }
                    }
                    else
                    {
                        txtselisih.Text = "0";
                        lTotalPayment.Text = "0";
                        l_diskon.Text = "0";
                    }
                }
            }
        }

        private void tDiskon_EditValueChanged(object sender, EventArgs e)
        {
            decimal p_bill = 0, p_dis = 0, p_pay = 0, p_temp = 0;
            string s_tipe = "";
            if (!tDiskon.Text.Equals("") && !tDiskon.Text.Equals("0"))
            {
               

                s_tipe = gridView1.GetRowCellDisplayText(gridView1.FocusedRowHandle, gridView1.Columns[19]);
                if(s_tipe.ToString().Equals("U"))
                    p_bill = decimal.Parse(totPay.ToString().Replace(".", ""));
                else
                    p_bill = decimal.Parse(totSelisih.ToString().Replace(".", ""));
                p_dis = decimal.Parse(tDiskon.Text.Replace(".", ""));
                p_pay = decimal.Parse(totSelisih.ToString().Replace(".", ""));

                if (p_bill > 0 && p_dis > 0)
                {
                    p_temp = p_dis * p_bill / 100;
                    p_pay = p_bill - p_temp;
                    totPayment = Convert.ToInt32(p_pay);
                    totcover = Convert.ToInt32(p_pay);
                    //lTotalPayment.Text = p_pay.ToString();
                    lTotalPayment.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", p_pay);
                    l_diskon.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", p_temp);
                    //txt_cover.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", totcover);
                    chTangguh.CheckState = CheckState.Unchecked;
                    if (s_tipe.ToString().Equals("B"))
                    {
                        //txt_cover.Text = "0";
                        chTangguh.Enabled = false;
                    }
                    else
                    {
                        chTangguh.Enabled = true ; 
                    } 
                }
                else
                {
                    totPayment = totPay;
                    totcover = totPayment;
                    lTotalPayment.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", totPayment);
                    //txt_cover.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", totcover);
                    l_diskon.Text = "0";
                    chTangguh.CheckState = CheckState.Unchecked;
                    if (s_tipe.ToString().Equals("B"))
                    {
                        //txt_cover.Text = "0";
                        chTangguh.Enabled = false;
                    }
                    else
                    {
                        //txt_cover.Text = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:N0}", totPayment);
                        chTangguh.Enabled = true;
                    } 
                }
                chTangguh.CheckState = CheckState.Unchecked;
                chTangguh.Enabled = false ;
            }
        }

        private void LoadDataLimit()
        {
            string SQL = "", limit = "", s_head= "", s_pasno="", s_rmno="", s_date="", s_que;

            if (gridView1.RowCount > 0)
            {
                s_head = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                s_pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                s_rmno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
                s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
                s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[3]).ToString();
                idvisit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[20]).ToString();
            }
            else
            {
                return;
            }


            if (Convert.ToInt16(txtLimitStok.Text) <= 0)
            {
                limit = "5";
                txtLimitStok.Text = "5";
            }
            else
            {
                limit = txtLimitStok.Text;
            }

            SQL = SQL + Environment.NewLine + "select grp, med_name, med_qty, price from ( ";
            SQL = SQL + Environment.NewLine + "select 'TRG05' grp, INITCAP(med_name) med_name, med_qty, price ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_receipt a ";
            SQL = SQL + Environment.NewLine + "join KLINIK.cs_medicine b on (a.med_cd=b.med_cd) ";
            SQL = SQL + Environment.NewLine + "where 1=1   ";
            if(!idvisit.ToString().Equals("") && !idvisit.ToString().Equals("0"))
                SQL = SQL + Environment.NewLine + "  and id_visit = " + idvisit + "  ";
            SQL = SQL + Environment.NewLine + "  and confirm='N' ";
            //SQL = SQL + Environment.NewLine + "union ";
            //SQL = SQL + Environment.NewLine + "select 'TRG02' grp, 'nama kamar' nm, sysdate-(sysdate-3) cnt_day,  ";
            //SQL = SQL + Environment.NewLine + "to_number(sysdate-(sysdate-3)) * 175000 harga ";
            //SQL = SQL + Environment.NewLine + "from cs_visit a ";
            //SQL = SQL + Environment.NewLine + "left join cs_inpatient b on (a.inpatient_id=b.inpatient_id) ";
            //SQL = SQL + Environment.NewLine + "left join cs_room c on (b.room_id=c.room_id) ";
            //SQL = SQL + Environment.NewLine + "left join cs_room_class d on (c.class_id=d.class_id) ";
            //SQL = SQL + Environment.NewLine + "where patient_no='" + s_pasno + "' ";
            //SQL = SQL + Environment.NewLine + "and to_char(a.visit_date,'yyyy-mm-dd')='" + s_date + "' ";
            //SQL = SQL + Environment.NewLine + "and que01='" + s_que + "' ";
            SQL = SQL + Environment.NewLine + ")";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "order by 1,2 ";


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

                gridView3.Columns[0].Caption = "Group";
                gridView3.Columns[1].Caption = "Nama Layanan";
                gridView3.Columns[2].Caption = "Jumlah";
                gridView3.Columns[3].Caption = "Harga";

                gridView3.Columns[2].MinWidth = 60;
                gridView3.Columns[2].MaxWidth = 60;

                RepositoryItemLookUpEdit grpLookup = new RepositoryItemLookUpEdit();
                grpLookup.DataSource = listGrpLaya;
                grpLookup.ValueMember = "statCode";
                grpLookup.DisplayMember = "statName";

                grpLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                grpLookup.DropDownRows = listGrpLaya.Count;
                grpLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                grpLookup.AutoSearchColumnIndex = 1;
                grpLookup.NullText = "";
                gridView3.Columns[0].ColumnEdit = grpLookup;

                gridView3.Columns[0].OptionsColumn.AllowEdit = false;
                gridView3.Columns[1].OptionsColumn.AllowEdit = false;
                gridView3.Columns[2].OptionsColumn.AllowEdit = false;
                gridView3.Columns[3].OptionsColumn.AllowEdit = false;

                //RepositoryItemMemoEdit nmObat = new RepositoryItemMemoEdit();
                //nmObat.WordWrap = true;
                //gridView3.Columns[0].ColumnEdit = nmObat;

                gridView3.BestFitColumns();
                gridView3.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                //if (dt2.Rows.Count > 0)
                //{
                //    //tableLayoutPanel3.RowStyles[3] = new RowStyle(SizeType.Absolute, 30);
                //    tableLayoutPanel3.RowStyles[8] = new RowStyle(SizeType.Absolute, 30);
                //    tableLayoutPanel3.RowStyles[9] = new RowStyle(SizeType.Absolute, 200);
                //}
                //else
                //{
                //    //tableLayoutPanel3.RowStyles[3] = new RowStyle(SizeType.Absolute, 0);
                //    tableLayoutPanel3.RowStyles[8] = new RowStyle(SizeType.Absolute, 30);
                //    tableLayoutPanel3.RowStyles[9] = new RowStyle(SizeType.Absolute, 30);
                //}

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
            string s_nik = "", s_que = "", s_date = "", sql_his = "", s_rm = "", s_edit = "";

            s_nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[11]).ToString();
            s_edit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[12]).ToString();

            if (s_edit == "N")
            {
                MessageBox.Show("Data titak bisa dirubah. Silahkan melakukan adjusment.");
                return;
            }

            sql_his = " select a.rm_no, b.patient_no, a.visit_no, med_cd, med_qty, receipt_id " +
                      " from KLINIK.cs_receipt a " +
                      " join KLINIK.cs_patient b on (a.rm_no = b.rm_no) " +
                      " where b.status = 'A' " +
                      " and a.confirm = 'N' " +
                      " and b.patient_no = '" + s_nik + "' " +
                      " and to_char(insp_date, 'yyyy-mm-dd') = '" + s_date + "' " +
                      " and visit_no = '" + s_que + "' ";

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

                    command.CommandText = " update KLINIK.cs_visit set time_receipt = sysdate, time_end = sysdate, status = 'PAY',  " +
                                          " upd_date = sysdate, upd_emp = '" + DB.vUserId + "'  " +
                                          " where patient_no = '" + s_nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + s_date + "' " +
                                          " and que01 = '" + s_que + "' ";
                    command.ExecuteNonQuery();

                    s_rm = dt.Rows[0]["rm_no"].ToString();
                    command.CommandText = " update KLINIK.cs_receipt set confirm = 'Y', upd_emp = '" + DB.vUserId + "', upd_date = sysdate " + 
                                          " where rm_no = '" + s_rm + "' and to_char(insp_date,'yyyy-mm-dd') = '" + s_date + "' and visit_no = '" + s_que + "' and confirm = 'N' ";

                    command.ExecuteNonQuery();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string temp_cd="", temp_qty = "", temp_id = "";
                        //listDiagnosa.Add(new Diagnosa() { diagnosaCode = dt.Rows[i]["item_cd"].ToString(), diagnosaName = dt.Rows[i]["item_name"].ToString() });
                        temp_cd = dt.Rows[i]["med_cd"].ToString();
                        temp_qty = dt.Rows[i]["med_qty"].ToString();
                        temp_id = dt.Rows[i]["receipt_id"].ToString();

                        command.CommandText = " insert into KLINIK.cs_medicine_trans (trans_id, med_cd, trans_type, trans_date, trans_qty, receipt_id, ins_date, ins_emp) values " +
                                              " (klinik.cs_medtrans_seq.nextval,'" + temp_cd + "','OUT',to_date('" + s_date + "','yyyy-mm-dd'),'" + temp_qty + "','" + temp_id + "',sysdate,'" + DB.vUserId + "') ";

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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string s_nik = "", s_que = "", s_date = "", sql_his = "", s_rm = "";

            s_nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[11]).ToString();

            if (MessageBox.Show("Anda yakin akan melakukan proses cancel?",
                      "Message",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                pnlCancel.Visible = true;
                gridControl1.Enabled = false;
                gridControl2.Enabled = false;
                btnPayment.Enabled = false;
                btnPrint.Enabled = false; 
            }

        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.Column.Caption == "Confirm")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);
                if (kk == "N")
                {
                    //e.Appearance.BackColor = Color.FromArgb(150, Color.Blue);
                    //e.Appearance.BackColor2 = Color.FromArgb(150, Color.Blue);

                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.Caption == "Stok")
            {
                string stok = View.GetRowCellDisplayText(e.RowHandle, View.Columns[5]);

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
                    else if (Convert.ToInt16(stok) > 20)
                    {
                        e.Appearance.BackColor = Color.FromArgb(150, Color.Green);
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }

            }
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Pembayaran")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);
                if (kk == "Belum Bayar")
                {
                    e.Appearance.BackColor = Color.Orange;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Selesai")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Cancel" || kk == "Adjusment")
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.Caption == "Pemeriksaan")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);
                if (kk == "Medicine")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Green);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.Green);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Payment")
                {
                    e.Appearance.BackColor = Color.MediumPurple;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.Caption == "Tipe Pasien")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[15]);

                if (kk == "Umum")
                {
                    
                }
                else if (kk == "BPJS" || kk == "Perusahaan")
                {
                    e.Appearance.BackColor = Color.ForestGreen;
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
                    if (Convert.ToInt16(stok) <= 0)
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string s_head = "";
            s_head = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();

            if (luTipe.Text == "")
            {
                MessageBox.Show("Pilih tipe pasien");
                return;
            }

            if (lInsuNo.Text == "" || lInsuNo.Text == "-")
            {
                MessageBox.Show("No Asuransi belum diinput");
                return;
            }

            string sql_update = "";

            sql_update = " update KLINIK.cs_treatment_head set insu_flag = '" + luTipe.GetColumnValue("statusCode").ToString() + "' where head_id = '" + s_head + "' and pay_status = 'OPN'  ";

            try
            {
                OleDbConnection oraConnect4 = ConnOra.Create_Connect_Ora();
                OleDbCommand cm4 = new OleDbCommand(sql_update, oraConnect4);
                oraConnect4.Open();
                cm4.ExecuteNonQuery();
                oraConnect4.Close();
                cm4.Dispose();

                //MessageBox.Show("Query Exec : " + sql_delete);

                MessageBox.Show("Data Berhasil dirubah");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }


        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string SQL = "", SQL2 = "", limit = "", s_head = "", s_pasno = "", s_rmno = "", s_date = "", s_que;
            string p_name = "", p_age = "", p_phone = "", p_address = "", p_rm = "", p_date = "", p_tipe = "", tot = "", s_insu="", s_stat_pay="";
            string Sql = "";

            tot = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:C2}", totPay);

            s_head = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            s_rmno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
            s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
            s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[3]).ToString();
            s_insu = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[19]).ToString();
            s_stat_pay = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[9]).ToString();

            if (gridView3.RowCount > 0)
            {
                MessageBox.Show("Obat belum dikonfirmasi. Silahkan menghubungi bagian Farmasi.");
                return;
            }

            if (s_stat_pay != "CLS")
            {
                MessageBox.Show("Silahkan Konfirmasi Pembayaran.");
                return;
            }

            if (comboBox1.Text == "Rawat Jalan")
            {
                Sql = "";
                Sql = Sql + Environment.NewLine + " select a.name, round((sysdate-a.birth_date)/30/12) age, a.phone,    ";
                Sql = Sql + Environment.NewLine + "        a.address, d.rm_no, TO_CHAR(c.visit_date, 'fmdd Month yyyy', 'nls_date_language = INDONESIAN') tgl,     ";
                Sql = Sql + Environment.NewLine + "        decode (insu_flag,'B','BPJS','P','Perusahaan','Umum') insu_flag ,  ";
                Sql = Sql + Environment.NewLine + "        TREAT_TYPE_NAME typests,d.id_visit ,d.TOTAL_BILL  ";
                Sql = Sql + Environment.NewLine + "        ,'Rp. '|| to_char ( case when insu_flag = 'B' then decode(total_pay,0,TOTAL_COVERED,total_pay)  ";
                Sql = Sql + Environment.NewLine + "              else total_pay end, '999,999,999') ttlPay   ";
                Sql = Sql + Environment.NewLine + "        , replace(TERBILANG_INDO(case when insu_flag = 'B' then decode(total_pay,0,TOTAL_COVERED,total_pay) else total_pay end),'-',' ')||'Rupiah' TTL   ";
                Sql = Sql + Environment.NewLine + " from KLINIK.cs_patient_info a     ";
                Sql = Sql + Environment.NewLine + " join KLINIK.cs_visit c on (a.patient_no=c.patient_no)    ";
                Sql = Sql + Environment.NewLine + " join KLINIK.cs_treatment_head d on (c.id_visit = d.id_visit )    ";
                Sql = Sql + Environment.NewLine + " join KLINIK.CS_TREATMENT_TYPE e on (d.TREAT_TYPE_ID=e.TREAT_TYPE_ID)    ";
                Sql = Sql + Environment.NewLine + " where 1=1    ";
                Sql = Sql + Environment.NewLine + " and c.id_visit=  " + idvisit + "  "; 
            }
            else
            {
                Sql = "";
                Sql = Sql + Environment.NewLine + " select a.name, round((sysdate-a.birth_date)/30/12) age, a.phone,   ";
                Sql = Sql + Environment.NewLine + "        a.address, b.rm_no, TO_CHAR(c.visit_date, 'fmdd Month yyyy', 'nls_date_language = INDONESIAN') tgl,    ";
                Sql = Sql + Environment.NewLine + "        decode (insu_flag,'B','BPJS','P','Perusahaan','Umum') insu_flag , ";
                Sql = Sql + Environment.NewLine + "        TREAT_TYPE_NAME typests,d.id_visit ,d.TOTAL_BILL ";
                Sql = Sql + Environment.NewLine + "        ,'Rp. '|| to_char ( case when insu_flag = 'B' then decode(total_pay,0,TOTAL_COVERED,total_pay) ";
                Sql = Sql + Environment.NewLine + "              else total_pay end, '999,999,999') ttlPay  ";
                Sql = Sql + Environment.NewLine + "        , replace(TERBILANG_INDO(case when insu_flag = 'B' then decode(total_pay,0,TOTAL_COVERED,total_pay) else total_pay end),'-',' ')||' Rupiah' TTL  ";
                Sql = Sql + Environment.NewLine + " from KLINIK.cs_patient_info a   ";
                Sql = Sql + Environment.NewLine + " join KLINIK.cs_patient b on (a.patient_no=b.patient_no)   ";
                Sql = Sql + Environment.NewLine + " join KLINIK.cs_visit c on (a.patient_no=c.patient_no)   ";
                Sql = Sql + Environment.NewLine + " join KLINIK.cs_treatment_head d on (c.id_visit = d.id_visit )   ";
                Sql = Sql + Environment.NewLine + " join KLINIK.CS_TREATMENT_TYPE e on (d.TREAT_TYPE_ID=e.TREAT_TYPE_ID)  ";
                Sql = Sql + Environment.NewLine + " join KLINIK.cs_inpatient f on (f.INPATIENT_ID = c.INPATIENT_ID   )     ";
                Sql = Sql + Environment.NewLine + " join KLINIK.CS_GUARANTOR g on (f.GR_NO = g.GR_NO)  ";
                Sql = Sql + Environment.NewLine + " where 1=1   ";
                Sql = Sql + Environment.NewLine + " and c.id_visit= " + idvisit + "  ";
            }
           

            DataTable dataBayar =   ConnOra.Data_Table_ora(Sql);
            if (dataBayar.Rows.Count > 0)
            {
                //dataBayar.Columns.Add("TTL", typeof(string));
                //DataRow rwttl = dataBayar.NewRow();
                //rwttl["TTL"] = terbilang.CariIndexAngka(Convert.ToInt64(dataBayar.Rows[0]["ttlPay"].ToString())).TrimEnd().ToString();
                //dataBayar.Rows.Add(rwttl);

                //p_name = dataBayar.Rows[0]["TTL"].ToString();
                dsKwitansi.Tables.Clear(); 
                dsKwitansi.Tables.Add(dataBayar);

                ReportKwitansi report = new ReportKwitansi(dsKwitansi);
                report.ShowPreviewDialog();
            }

            SQL = " ";
            SQL = SQL + Environment.NewLine + " select name, round((sysdate-birth_date)/30/12) age, phone,  ";
            SQL = SQL + Environment.NewLine + "        address, b.rm_no, TO_CHAR(c.visit_date, 'fmdd Month yyyy', 'nls_date_language = INDONESIAN') tgl,   ";
            SQL = SQL + Environment.NewLine + "        decode (insu_flag,'B','BPJS','P','Perusahaan','Umum') insu_flag  ";
            SQL = SQL + Environment.NewLine + "   from KLINIK.cs_patient_info a  ";
            SQL = SQL + Environment.NewLine + "   join KLINIK.cs_patient b on (a.patient_no=b.patient_no)  ";
            SQL = SQL + Environment.NewLine + "   join KLINIK.cs_visit c on (a.patient_no=c.patient_no)  ";
            SQL = SQL + Environment.NewLine + "   join KLINIK.cs_treatment_head d on (b.rm_no=d.rm_no and trunc(c.visit_date)=d.visit_date and c.que01=d.visit_no)  ";
            SQL = SQL + Environment.NewLine + "  where 1=1  ";
            SQL = SQL + Environment.NewLine + "    and c.id_visit=" + idvisit + "  ";
            //SQL = SQL + Environment.NewLine + " and to_char(c.visit_date,'yyyy-mm-dd')='"+ s_date + "'  ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                p_name = dt.Rows[0]["name"].ToString();
                p_age = dt.Rows[0]["age"].ToString();
                p_phone = dt.Rows[0]["phone"].ToString();
                p_address = dt.Rows[0]["address"].ToString();
                p_rm = dt.Rows[0]["rm_no"].ToString();
                p_date = dt.Rows[0]["tgl"].ToString();
                p_tipe = dt.Rows[0]["insu_flag"].ToString();
            }

            SQL2 = "";
            SQL2 = SQL2 + Environment.NewLine + " select name, age, phone, address, rm, tgl, tipe, ";
            SQL2 = SQL2 + Environment.NewLine + " treat_group_order, treat_group_name, ";
            SQL2 = SQL2 + Environment.NewLine + " case when treat_group_order is not null then null else a end a, ";
            SQL2 = SQL2 + Environment.NewLine + " case when treat_group_order is not null then null else b end b, ";
            SQL2 = SQL2 + Environment.NewLine + " case when treat_group_order is not null then null else c end c, ord, ";
            SQL2 = SQL2 + Environment.NewLine + " case when treat_group_name in ('Total Tagihan','Diskon (%)','Jumlah biaya yang harus dibayar') then null ";
            SQL2 = SQL2 + Environment.NewLine + " else rownum end nno from ( ";
            SQL2 = SQL2 + Environment.NewLine + " select name, age, phone, address, rm, tgl, tipe,  ";
            SQL2 = SQL2 + Environment.NewLine + " treat_group_order, treat_group_name, a, b, c, ord from (   ";
            SQL2 = SQL2 + Environment.NewLine + " select '" + p_name + "' name, '" + p_age + "' age, '" + p_phone + "' phone, '" + p_address + "' address, '" + p_rm + "' rm, '" + p_date + "' tgl, '" + p_tipe + "' tipe,  ";
            SQL2 = SQL2 + Environment.NewLine + " treat_group_order, treat_group_name, a, b, a*b c, ord from (  ";
            SQL2 = SQL2 + Environment.NewLine + " select treat_group_order, treat_group_name, 0 a, 0 b, treat_group_order * 10 ord from cs_treatment_group  ";
            SQL2 = SQL2 + Environment.NewLine + " where treat_group_id in (select c.treat_group_id  ";
            SQL2 = SQL2 + Environment.NewLine + " from KLINIK.cs_treatment_item a   ";
            SQL2 = SQL2 + Environment.NewLine + " join KLINIK.cs_treatment_detail b on (a.treat_item_id=b.treat_item_id)   ";
            SQL2 = SQL2 + Environment.NewLine + " join KLINIK.cs_treatment_group c on (a.treat_group_id=c.treat_group_id)  ";
            SQL2 = SQL2 + Environment.NewLine + " where head_id='" + s_head + "' )  ";
            SQL2 = SQL2 + Environment.NewLine + " union all ";
            SQL2 = SQL2 + Environment.NewLine + " select i, treat_item_name, sum(treat_qty) treat_qty,  ";
            SQL2 = SQL2 + Environment.NewLine + " treat_item_price, ord from ( ";
            SQL2 = SQL2 + Environment.NewLine + " select null i, a.treat_item_name, treat_qty,   ";
            SQL2 = SQL2 + Environment.NewLine + " a.treat_item_price, (c.treat_group_order * 10) + 1 ord   ";
            SQL2 = SQL2 + Environment.NewLine + " from KLINIK.cs_treatment_item a   ";
            SQL2 = SQL2 + Environment.NewLine + " join KLINIK.cs_treatment_detail b on (a.treat_item_id=b.treat_item_id)   ";
            SQL2 = SQL2 + Environment.NewLine + " join KLINIK.cs_treatment_group c on (a.treat_group_id=c.treat_group_id)  ";
            SQL2 = SQL2 + Environment.NewLine + " where head_id='" + s_head + "'   ";
            SQL2 = SQL2 + Environment.NewLine + " union all ";
            SQL2 = SQL2 + Environment.NewLine + " select 5 i, 'Obat-obatan dan Alkes' a, 0 b, 0 c, 50 d from dual  ";
            SQL2 = SQL2 + Environment.NewLine + " union all ";
            if (s_insu == "U")
            {
                SQL2 = SQL2 + Environment.NewLine + " select null i, initcap(med_name) med_name, 1 med_qty, price,   ";

            }
            else
            {
                SQL2 = SQL2 + Environment.NewLine + " select null i, initcap(med_name) med_name, 1 med_qty, price * d.insu_cover  price,   ";
            }
            
            SQL2 = SQL2 + Environment.NewLine + " 50 + 1  remarks ";
            SQL2 = SQL2 + Environment.NewLine + " from KLINIK.cs_receipt a    ";
            SQL2 = SQL2 + Environment.NewLine + " join KLINIK.cs_patient b on (a.rm_no = b.rm_no)   ";
            SQL2 = SQL2 + Environment.NewLine + " join KLINIK.cs_medicine c on(a.med_cd = c.med_cd)    ";
            SQL2 = SQL2 + Environment.NewLine + " join KLINIK.cs_medicine_trans d on(a.receipt_id = d.receipt_id)   ";
            SQL2 = SQL2 + Environment.NewLine + " where b.status = 'A'    ";
            SQL2 = SQL2 + Environment.NewLine + " and c.status = 'A'    ";
            SQL2 = SQL2 + Environment.NewLine + " and b.patient_no = '" + s_pasno + "'    ";
            if (comboBox1.Text == "Rawat Jalan")
            {
                SQL2 = SQL2 + Environment.NewLine + " and to_char(insp_date, 'yyyy-mm-dd') = '" + s_date + "'    ";
            }
            else
            {
                SQL2 = SQL2 + Environment.NewLine + " and to_char(visit_dt, 'yyyy-mm-dd') = '" + s_date + "'    ";
            }
                
            SQL2 = SQL2 + Environment.NewLine + " and visit_no = '" + s_que + "' )  ";
            SQL2 = SQL2 + Environment.NewLine + " group by i, treat_item_name, treat_item_price, ord) ";
            SQL2 = SQL2 + Environment.NewLine + " where 1=1  ";
            SQL2 = SQL2 + Environment.NewLine + " union ";
            SQL2 = SQL2 + Environment.NewLine + " select null a1, null a2, null a3, null a4, null a5, null a6, null a7, null aa,   ";
            SQL2 = SQL2 + Environment.NewLine + " 'Total Tagihan' bb, null a, null b, total_bill c, 997 ord ";
            SQL2 = SQL2 + Environment.NewLine + " from KLINIK.cs_treatment_head  a ";
            SQL2 = SQL2 + Environment.NewLine + " where head_id='" + s_head + "'  ";
            SQL2 = SQL2 + Environment.NewLine + " union ";
            SQL2 = SQL2 + Environment.NewLine + " select null a1, null a2, null a3, null a4, null a5, null a6, null a7, null aa,  ";
            SQL2 = SQL2 + Environment.NewLine + " 'Diskon (%)' bb, disc a, null b, total_bill * (case when disc is null then 1 else disc end)/100 c, 998 ord  ";
            SQL2 = SQL2 + Environment.NewLine + " from KLINIK.cs_treatment_head ";
            SQL2 = SQL2 + Environment.NewLine + " where head_id='" + s_head + "' ";
            SQL2 = SQL2 + Environment.NewLine + " union ";
            SQL2 = SQL2 + Environment.NewLine + " select null a1, null a2, null a3, null a4, null a5, '"+ p_date + "' a6, null a7, ";
            SQL2 = SQL2 + Environment.NewLine + " null aa, 'Jumlah biaya yang harus dibayar' bb, ";
            SQL2 = SQL2 + Environment.NewLine + " null a, null b, to_number('" + lTotalPayment.Text.Replace(".","") + "') c, 999 ord from dual ";
            SQL2 = SQL2 + Environment.NewLine + " ) ";
            SQL2 = SQL2 + Environment.NewLine + " where 1=1  ";
            SQL2 = SQL2 + Environment.NewLine + " order by ord asc) x ";

            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra2 = new OleDbDataAdapter(SQL2, oraConnect2);
            DataTable dt2 = new DataTable();
            adOra2.Fill(dt2);

            if (dt2.Rows.Count > 0)
            {
                dsBillRj.Tables.Clear();
                dsBillRj.Tables.Add(dt2);

                ReportBill report = new ReportBill(dsBillRj);
                report.ShowPreviewDialog();
            } 
        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }
        private void cktransfer()
        {
            if(rdTunai.Checked == true )
            {
                panel6.Enabled = false;
                panel7.Enabled = false;
            }
            else
            {
                panel6.Enabled = true;
                panel7.Enabled = true;
            }
            if(rdEDC.Checked == true )
            { 
                panel7.Enabled = false;
                cbbank.Enabled = false; 
            }
            else
            { 
                panel7.Enabled = true;
                cbbank.Enabled = true;
            }
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            cktransfer();
        }

        private void gridView4_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

       

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            cktransfer();
        }

        private void rdTunai_CheckedChanged(object sender, EventArgs e)
        {
            cktransfer();
        }

        private void rdTransfer_CheckedChanged(object sender, EventArgs e)
        {
            cktransfer();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if(simpleButton2.Text =="Cancel")
            {
                pnlCancel.Visible = false;
                gridControl1.Enabled = false;
                gridControl2.Enabled = false;
                btnPayment.Enabled = false;
                btnPrint.Enabled = false;
                labelControl11.Visible = true;
                textBox1.Visible = true;
                textBox1.Text = "";
                simpleButton1.Visible = true;
                simpleButton1.Enabled = true;
                simpleButton2.Text = "Batal";
            }else if (simpleButton2.Text == "Batal")
            {
                pnlCancel.Visible = true;
                gridControl1.Enabled = true;
                gridControl2.Enabled = true;
                //btnPayment.Enabled = true;
                btnPrint.Enabled = true;
                labelControl11.Visible = false;
                textBox1.Visible = false;
                textBox1.Text = "";
                simpleButton1.Enabled = false;
                simpleButton1.Visible = false; 
                simpleButton2.Text = "Cancel";
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string s_nik = "", s_que = "", s_date = "", sql_his = "", sql_user = "", id_visit = "", s_head ="";
            string sstatus = "", s_stbyr = "" ;
            

            if (gridView1.RowCount < 1)
                return;

            s_nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            id_visit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[20]).ToString();
            s_head = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            sstatus = gridView1.GetRowCellDisplayText(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            s_stbyr = gridView1.GetRowCellDisplayText(gridView1.FocusedRowHandle, gridView1.Columns[9]).ToString();

            if(sstatus.ToString().Equals("Sudah Bayar") && s_stbyr.ToString().Equals("Selesai"))
            {
                sql_user = " select a.USER_ID " +
                   " from CS_USER a " +
                   " where a.status = 'A' " +
                   " and a.USER_ROLE = 'MGR' " +
                   " and a.PASS = '" + textBox1.Text + "' ";

                OleDbConnection sqlCon = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSqlu = new OleDbDataAdapter(sql_user, sqlCon);
                DataTable dtuser = new DataTable();
                adSqlu.Fill(dtuser);


                if (dtuser.Rows.Count > 0)
                {
                    sql_his = " select a.rm_no, b.patient_no, a.visit_no, med_cd, med_qty, receipt_id " +
                        " from KLINIK.cs_receipt a " +
                        " join KLINIK.cs_patient b on (a.rm_no = b.rm_no) " +
                        " where b.status = 'A' " +
                        " and a.confirm = 'Y' " +
                        " and b.patient_no = '" + s_nik + "' " +
                        " and ID_VISIT = " + idvisit + " ";

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

                            command.CommandText = " update KLINIK.cs_visit set status = 'DON', TIME_PAYMENT = null,   " +
                                                  " upd_date = sysdate, upd_emp = '" + DB.vUserId + "'  " +
                                                  " where patient_no = '" + s_nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + s_date + "' " +
                                                  " and que01 = '" + s_que + "' ";
                            command.ExecuteNonQuery();

                            command.CommandText = " insert into KLINIK.cs_treatment_head_cancel select * from KLINIK.cs_treatment_head where  head_id = " + s_head + " ";
                            command.ExecuteNonQuery();

                            command.CommandText = " update KLINIK.cs_treatment_head set status = 'OPN', close_dt = null, pay_status = 'OPN', total_bill = null, " +
                                                   "       TOTAL_COVERED = null, total_pay = null, disc =null, " +
                                                   "       total_trt =null, total_med = null, upd_date = sysdate, upd_emp = '" + DB.vUserId + "', STS_PAY = null, VIA_PAY = null, AN_PAY = null " +

                                                   " where head_id = '" + s_head + "' and pay_status='CLS'  ";
                            command.ExecuteNonQuery();



                            for (int i = 0; i < gridView2.RowCount; i++)
                            {
                                string receipt_id = "", temp_qty = "", temp_price = "";

                                receipt_id = gridView2.GetRowCellValue(i, gridView2.Columns[8]).ToString(); 
                                if (!receipt_id.ToString().Equals("0"))
                                {
                                    command.CommandText = " update cs_receipt set QTY_PAY = null,PRICE_PAY = null, ID_KASIR = null, PAY_DATE = null " +
                                                     " where receipt_id = " + receipt_id + " and id_visit = " + id_visit + " ";

                                    command.ExecuteNonQuery();
                                }
                            } 
                            trans.Commit();


                            pnlCancel.Visible = false;
                            gridControl1.Enabled = true;
                            gridControl2.Enabled = true;
                            btnPayment.Enabled = true;
                            btnPrint.Enabled = true;

                            LoadData();
                            //MessageBox.Show(sql_insert);
                            //MessageBox.Show("Query Exec : " + sql_insert);

                            MessageBox.Show("Data Transaksi Berhasil Dibatalkan.");

                            pnlCancel.Visible = true;
                            gridControl1.Enabled = true;
                            gridControl2.Enabled = true;
                            btnPayment.Enabled = true;
                            btnPrint.Enabled = true;
                            labelControl11.Visible = false;
                            textBox1.Visible = false;
                            textBox1.Text = "";
                            simpleButton1.Enabled = false;
                            simpleButton1.Visible = false;
                            simpleButton2.Text = "Cancel";

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
                    MessageBox.Show("Password Salah. Pembatalan Transaksi Gagal..");
                    return;
                }
            }
            

        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            string SQL = "", SQL2 = "", limit = "", s_head = "", s_pasno = "", s_rmno = "", s_date = "", s_que, s_payst = "", s_plyn = "", s_tipe="", age ="", sql_diag ="", diag_cnt="";
            string sql_cek = "", cd_val = "", sql_tmp = "", id_visit ="", sql_all="", gnder="",  sname ="", PoliCd = "", p_inpatient ="", p_anamnesa ="";
            string sql_cek_out = "", tmp_rsout = "", tmp_passtat = "", dt_out = "";
            string sql_cek_diag = "", tmp_diag = "";
            string sql_cek_amt_laya = "", tmp_amt_laya = "";
            string sql_cek_amt_med = "", tmp_amt_med = "";
            string stspay = "", stspaym = "", p_kirid ="", p_class ="";

            int p_tot_bill = 0, p_tot_pay=0;
            p_tot_bill = totBill;
            p_tot_pay = totPayment;

            if (comboBox1.Text.ToString().Equals("Lain-Lain"))
            {
                if (gridView4.RowCount < 1)
                    return; 

                p_kirid = gridView4.GetRowCellValue(gridView4.FocusedRowHandle, gridView4.Columns[0]).ToString();
                p_class = gridView4.GetRowCellValue(gridView4.FocusedRowHandle, gridView4.Columns[5]).ToString();
                 
                string sql_update = " ";
                
                sql_update = sql_update + " update cs_kir set PRICE = " + Convert.ToInt64(lTotalPay.Text.Replace(".", ""))  + ",  ";
                sql_update = sql_update + "        TOTAL_PAY  = '" + Convert.ToInt64(lTotalPayment.Text.Replace(".", ""))  + "',  ";
                sql_update = sql_update + "        TTLDISC  = '" + Convert.ToInt64(tDiskon.Text) + "', ";
                sql_update = sql_update + "        STAT_PAY  = 'Y', PAY_DATE = sysdate, PAY_BY =  '" + DB.vUserId + "' "; 
                sql_update = sql_update + " where  KIR_ID = '" + p_kirid + "' and F_TYPE = '" + p_class + "' ";

                try
                {
                    OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm2 = new OleDbCommand(sql_update, oraConnect2);
                    oraConnect2.Open();
                    cm2.ExecuteNonQuery();
                    oraConnect2.Close();
                    cm2.Dispose();

                    for (int i = 0; i < gridView2.RowCount; i++)
                    {
                        string receipt_id = "", temp_qty = "", temp_price = "", sql ="";

                        receipt_id = gridView2.GetRowCellValue(i, gridView2.Columns[8]).ToString();
                        temp_qty = gridView2.GetRowCellValue(i, gridView2.Columns[4]).ToString();
                        temp_price = gridView2.GetRowCellValue(i, gridView2.Columns[5]).ToString();
                        if (!receipt_id.ToString().Equals("0"))
                        {
                            sql = "";
                            sql = sql + " update cs_receipt set QTY_PAY = " + temp_qty + ",PRICE_PAY = " + temp_price + ", ID_KASIR = '" + DB.vUserId + "', PAY_DATE = sysdate ";
                            sql = sql + " where receipt_id = " + receipt_id + " and ATT3_RECIEPT = " + p_kirid + " ";

                            ORADB.Execute(ORADB.XE, sql); 
                        }
                    }


                    MessageBox.Show("Pembayaran Pelayanan " + p_class + "  Berhasil."); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
                 
            }
            else
            { 
                if(gridView3.RowCount > 0)
                {
                    MessageBox.Show("Data obat belum di konfirmasi.");
                    return;
                }


                s_head = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                s_pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                s_rmno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
                s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
                s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[3]).ToString();
                s_payst = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[9]).ToString();
                s_plyn = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[7]).ToString();
                id_visit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[20]).ToString();
                gnder = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[21]).ToString();
                sname = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();
                PoliCd = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[23]).ToString();
                p_inpatient = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[24]).ToString();
                p_anamnesa = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[25]).ToString();
                age = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[26]).ToString();

                if (s_payst == "CLS")
                {
                    MessageBox.Show("Data tidak bisa dirubah");
                    return;
                }

                if (rdTransfer.Checked)
                {
                    if (rdBank.Checked)
                    {
                        if (cbbank.Text.ToString().Equals(""))
                        {
                            MessageBox.Show("Silahkan tentukan Bank yang di tuju.");
                            return;
                        }
                    }
                }

                if (rdTunai.Checked)
                    stspay = "Tunai";
                else
                    stspay = "Transfer";

                if (rdEDC.Checked)
                    stspaym = "EDC";
                else
                    stspaym = "BANK";

                if (comboBox1.Text == "Rawat Inap")
                {
                    sql_cek_out = " ";
                    sql_cek_out = sql_cek_out + Environment.NewLine + "select to_char(date_out,'yyyy-mm-dd') date_out, rs_out, patient_stat, room_id from KLINIK.cs_inpatient ";
                    sql_cek_out = sql_cek_out + Environment.NewLine + "where INPATIENT_ID=" + p_inpatient + " ";
                    //sql_cek_out = sql_cek_out + Environment.NewLine + "and to_char(reg_date,'yyyy-mm-dd')='" + s_date + "'  ";

                    OleDbConnection sqlConnectco = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSqlco = new OleDbDataAdapter(sql_cek_out, sqlConnectco);
                    DataTable dtco = new DataTable();
                    adSqlco.Fill(dtco);
                    if (dtco.Rows.Count > 0)
                    {
                        dt_out = dtco.Rows[0]["date_out"].ToString();
                        tmp_rsout = dtco.Rows[0]["rs_out"].ToString();
                        tmp_passtat = dtco.Rows[0]["patient_stat"].ToString();
                        cd_val = dtco.Rows[0]["room_id"].ToString();
                    }

                    if (comboBox1.Text == "Rawat Inap" && (dt_out.Trim() == "" || tmp_rsout.Trim() == "" || tmp_passtat.Trim() == ""))
                    {
                        MessageBox.Show("Silahkan Isi tgl keluar, cara keluar dan status pasien pada menu Reservasi Rawat Inap");
                        return;
                    }

                    sql_cek_diag = "";
                    sql_cek_diag = sql_cek_diag + Environment.NewLine + "select count(0) cnt from KLINIK.cs_diagnosa ";
                    sql_cek_diag = sql_cek_diag + Environment.NewLine + "where ANAMNESA_ID ='" + p_anamnesa + "'  ";
                    sql_cek_diag = sql_cek_diag + Environment.NewLine + "and type_diagnosa='E'  ";

                    OleDbConnection sqlConnectd = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSqld = new OleDbDataAdapter(sql_cek_diag, sqlConnectd);
                    DataTable dtd = new DataTable();
                    adSqld.Fill(dtd);
                    if (dtd.Rows.Count > 0)
                    {
                        tmp_diag = dtd.Rows[0]["cnt"].ToString();
                    }
                } 

                

               

                if (gridView3.RowCount > 0)
                {
                    MessageBox.Show("Obat belum dikonfirmasi. Silahkan menghubungi bagian Farmasi.");
                    return;
                }

                sql_cek_amt_med = "";
                sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " select nvl(sum(price),0) amt_med ";
                sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " from KLINIK.cs_receipt a  ";
                sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " join KLINIK.cs_patient b on (a.rm_no = b.rm_no)  ";
                sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " join KLINIK.cs_medicine c on(a.med_cd = c.med_cd)  ";
                sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " join KLINIK.cs_medicine_trans d on(a.receipt_id = d.receipt_id)  ";
                if (comboBox1.Text == "Rawat Jalan")
                {

                }
                else
                {
                    sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " join KLINIK.cs_inpatient e on (a.rm_no=e.rm_no and a.visit_dt=e.reg_date)   ";
                }
                sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " where b.status = 'A'  ";
                sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " and c.status = 'A'  ";
                sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " and b.patient_no = '" + s_pasno + "'  ";

                if (comboBox1.Text == "Rawat Jalan")
                {
                    sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " and to_char(insp_date, 'yyyy-mm-dd') = '" + s_date + "'  ";
                }

                sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " and visit_no = '" + s_que + "' and a.id_visit  = " + id_visit + " ";

                OleDbConnection sqlConnectmed = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSqlmed = new OleDbDataAdapter(sql_cek_amt_med, sqlConnectmed);
                DataTable dtmed = new DataTable();
                adSqlmed.Fill(dtmed);
                if (dtmed.Rows.Count > 0)
                {
                    tmp_amt_med = dtmed.Rows[0]["amt_med"].ToString();
                }
                else
                {
                    tmp_amt_med = "0";
                }

                if (comboBox1.Text == "Rawat Inap" && (Convert.ToInt16(tmp_diag) == 0))
                {
                    MessageBox.Show("Silahkan Isi Diagnosa Akhir");
                    return;
                }

                if(comboBox1.Text == "Rawat Jalan")
                {
                    sql_diag = " select count(0) cnt from KLINIK.cs_diagnosa where to_char(insp_date,'yyyy-mm-dd') = '" + s_date + "' and ANAMNESA_ID = " + p_anamnesa + "   ";
                    OleDbConnection oraConnectd = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adOrad = new OleDbDataAdapter(sql_diag, oraConnectd);
                    DataTable dtd = new DataTable();
                    adOrad.Fill(dtd);
                    diag_cnt = dtd.Rows[0]["cnt"].ToString();

                    if (diag_cnt == "0")
                    {
                        MessageBox.Show("Diagnosa Belum Di Input");
                        return;
                    }
                }
                 
                string rm_type = "", p1 = "", p2 = "", teks = "", callid = "";

                sql_all = "";
                sql_all = sql_all + @" select a.CALL_ID, TYPE_INS, a.que
                                    from KLINIK.CS_CALL_LOG a, KLINIK.cs_visit b
                                    where a.que = b.que01
                                    AND a.que = '" + s_que + @"'    
                                    AND b.id_visit = '" + id_visit + @"'    
                                    AND TRUNC(A.INS_DATE) = TRUNC(VISIT_DATE)  ";

                OleDbConnection oraConnect5 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra5 = new OleDbDataAdapter(sql_all, oraConnect5);
                DataTable dt5 = new DataTable();
                adOra5.Fill(dt5);
                if (dt5.Rows.Count > 0)
                {
                    rm_type = dt5.Rows[0]["TYPE_INS"].ToString();
                    callid = dt5.Rows[0]["CALL_ID"].ToString();
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
                     
                    
                    // s_tipe
                    if (comboBox1.Text == "Rawat Inap")
                    {
                        s_tipe = "INP";
                    }
                    else
                    {
                        s_tipe = "DON";
                    } 

                    if (PoliCd.ToString().Equals("POL0004"))
                    {
                        command.CommandText = " update KLINIK.cs_inpatient set status = 'CLS', upd_emp = '" + DB.vUserId + "', upd_date = sysdate where INPATIENT_ID=" + p_inpatient + "  ";
                        command.ExecuteNonQuery();

                        command.CommandText = " update KLINIK.cs_bed set use_yn = 'N', upd_date = sysdate, upd_emp = '" + DB.vUserId + "' where bed_id = '" + cd_val + "' ";
                        command.ExecuteNonQuery();
                    }

                    if (PoliCd.ToString().Equals("POL0007"))
                    {
                        command.CommandText = " update KLINIK.cs_visit set status = 'CLS', TIME_END = sysdate, time_payment=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate, MENU_LAST_UPDATED = 'BillList', M_UPDATED_DATE = sysdate where patient_no = '" + s_pasno + "' and id_visit = " + id_visit + "   ";
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText = " update KLINIK.cs_visit set status = '" + s_tipe + "', time_payment=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate, MENU_LAST_UPDATED = 'BillList', M_UPDATED_DATE = sysdate  where patient_no = '" + s_pasno + "' and id_visit = " + id_visit + " ";
                        command.ExecuteNonQuery();
                    }

                    for (int i = 0; i < gridView2.RowCount; i++)
                    {
                        string receipt_id = "", temp_qty = "", temp_price = "", treat_id ="", rmarks ="", sqlroom ="", tdate="", trname="";

                        receipt_id = gridView2.GetRowCellValue(i, gridView2.Columns[8]).ToString();
                        trname = gridView2.GetRowCellValue(i, gridView2.Columns[2]).ToString();
                        tdate = gridView2.GetRowCellValue(i, gridView2.Columns[3]).ToString();
                        temp_qty = gridView2.GetRowCellValue(i, gridView2.Columns[4]).ToString();
                        temp_price = gridView2.GetRowCellValue(i, gridView2.Columns[5]).ToString();
                        rmarks = gridView2.GetRowCellValue(i, gridView2.Columns[6]).ToString();
                        treat_id = gridView2.GetRowCellValue(i, gridView2.Columns[0]).ToString();

                        if (!receipt_id.ToString().Equals("0"))
                        {
                            command.CommandText = " update cs_receipt set QTY_PAY = " + temp_qty + ",PRICE_PAY = " + temp_price + ", ID_KASIR = '" + DB.vUserId + "', PAY_DATE = sysdate " +
                                             " where receipt_id = " + receipt_id + " and id_visit = " + id_visit + " ";

                            command.ExecuteNonQuery();
                        }

                        if (rmarks.ToString().Equals("Ruangan"))
                        {
                            sqlroom = "";
                            sqlroom = sqlroom + " insert into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME) values ( ";
                            sqlroom = sqlroom + " CS_TREATMENT_DETAIL_SEQ.nextval ," + s_head + "," + treat_id + ",  TO_DATE('" + tdate + "', 'yyyy-MM-dd'), ";
                            sqlroom = sqlroom + " " + temp_qty + ", " + temp_price + " , " + Convert.ToInt32(temp_qty) * Convert.ToInt32(temp_price) + ", ";
                            sqlroom = sqlroom + " '" + trname + "' ,  sysdate, '" + DB.vUserId + "', '' , 'gridView2' )";

                            command.CommandText = sqlroom;
                            command.ExecuteNonQuery();
                        }
                            

                    }

                    sql_cek_amt_laya = "";
                    sql_cek_amt_laya = sql_cek_amt_laya + Environment.NewLine + " select nvl(sum(b.treat_item_price),0) amt_laya ";
                    sql_cek_amt_laya = sql_cek_amt_laya + Environment.NewLine + " from KLINIK.cs_treatment_item a ";
                    sql_cek_amt_laya = sql_cek_amt_laya + Environment.NewLine + " join KLINIK.cs_treatment_detail b on (a.treat_item_id=b.treat_item_id) ";
                    sql_cek_amt_laya = sql_cek_amt_laya + Environment.NewLine + " where b.head_id='" + s_head + "' ";

                    OleDbConnection sqlConnectlaya = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSqllaya = new OleDbDataAdapter(sql_cek_amt_laya, sqlConnectlaya);
                    DataTable dtlaya = new DataTable();
                    adSqllaya.Fill(dtlaya);
                    if (dtlaya.Rows.Count > 0)
                    {
                        tmp_amt_laya = dtlaya.Rows[0]["amt_laya"].ToString();
                    }
                    else
                    {
                        tmp_amt_laya = "0";
                    }

                    command.CommandText = " update KLINIK.cs_treatment_head set status = 'CLS', close_dt = sysdate, pay_status = 'CLS', total_bill = " + p_tot_bill + ", " +
                                           "       TOTAL_COVERED = " + Convert.ToInt64(txt_cover.Text.Replace(".", "")) + ", total_pay = " + totPayment + ", disc = " + tDiskon.Text + ", " +
                                           "       total_trt = " + tmp_amt_laya + ", total_med = " + tmp_amt_med + ", upd_date = sysdate, upd_emp = '" + DB.vUserId + "', STS_PAY = '" + stspay + "', VIA_PAY = decode('" + stspay + "','Tunai','','" + stspaym + "'), AN_PAY =  decode('" + stspay + "','Tunai','','" + txtnama.Text + "') " +
                                           " where head_id = '" + s_head + "' and pay_status='OPN'  ";
                    command.ExecuteNonQuery();

                    if (comboBox1.Text == "Rawat Jalan")
                    {
                        if (rm_type.ToString().Equals("PAY"))
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

                            p2 = sname + " "; 

                            teks = "Nomor Antrian " + s_que + " " + p1 + p2 + " Silahkan Menuju Ke Farmasi";

                            command.CommandText = " UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'W', type_ins ='MED', stat ='Closed', param = '" + teks + "' WHERE CALL_ID = " + callid + " ";
                            command.ExecuteNonQuery();
                        }
                    }

                    if (PoliCd.ToString().Equals("POL0007"))
                    {
                        command.CommandText = " UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'Y', type_ins ='ETC', stat ='Closed'  WHERE CALL_ID = " + callid + " ";
                        command.ExecuteNonQuery();
                    }

                    trans.Commit();

                    MessageBox.Show("Transaksi Pembayaran Berhasil disimpan");

                    LoadData();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("ERROR: " + ex.Message);
                }

                oraConnectTrans.Close();
            }
        }

        

        private void btnCall_Click(object sender, EventArgs e)
        {
            string sql_check5 = "", rm_number = "", p_que = "", id_visit = "", sql1 = "", p_que2 = "";

            p_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[3]).ToString();
            id_visit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[20]).ToString();

            if (comboBox1.Text == "Rawat Jalan")
            {
                sql_check5 = "";
                sql_check5 = sql_check5 + @" select TYPE_INS, a.que
                                   from KLINIK.CS_CALL_LOG a, KLINIK.cs_visit b
                                  where a.que = b.que01
                                    AND a.que = '" + p_que + @"'    
                                    AND b.id_visit = '" + id_visit + @"'    
                                    AND TRUNC(a.INS_DATE) = TRUNC(SYSDATE)
                                    AND TRUNC(A.INS_DATE) = TRUNC(VISIT_DATE(+))  ";
                 
                OleDbConnection oraConnect5 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra5 = new OleDbDataAdapter(sql_check5, oraConnect5);
                DataTable dt5 = new DataTable();
                adOra5.Fill(dt5);
                if (dt5.Rows.Count > 0)
                {
                    rm_number = dt5.Rows[0]["TYPE_INS"].ToString();
                }

                if (rm_number.ToString().Equals("PAY"))
                {
                    sql1 = " ";
                    sql1 = @"UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'N' WHERE QUE = '" + p_que + "' and TYPE_INS ='PAY' AND TRUNC(INS_DATE) = TRUNC(SYSDATE)";

                    ORADB.Execute(ORADB.XE, sql1);
                }
                else
                {
                    MessageBox.Show("Maaf Pasien sudah di Proses, Tidak Dapat Dipanggil Di Bagian Kasir.");
                    return;
                }
            }  
        }
    }
}