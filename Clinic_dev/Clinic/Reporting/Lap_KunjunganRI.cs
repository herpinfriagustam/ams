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
using Clinic.Report;
using DevExpress.XtraReports.UI;
using DevExpress.XtraEditors.Repository;

namespace Clinic
{
    public partial class Lap_KunjunganRI : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Status> listStat = new List<Status>(); List<Status> listStat2 = new List<Status>();
        List<PatientType> listPatientType = new List<PatientType>();
        List<WorkAccident> listWorkAccident = new List<WorkAccident>();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string type = "", sdate="", edate="";

        public Lap_KunjunganRI()
        {
            InitializeComponent();
        }

        private void ObservationList_Load(object sender, EventArgs e)
        {
            InitData();
            //LoadData();
        }

        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void InitData()
        {

            listPatientType.Clear();
            listPatientType.Add(new PatientType() { patientTypeCode = "E", patientTypeName = "Emergency" });
            listPatientType.Add(new PatientType() { patientTypeCode = "U", patientTypeName = "Umum" });

            listWorkAccident.Clear();
            listWorkAccident.Add(new WorkAccident() { workAccidentCode = "Y", workAccidentName = "Yes" });
            listWorkAccident.Add(new WorkAccident() { workAccidentCode = "N", workAccidentName = "No" });

            listStat.Clear();
            listStat.Add(new Status() { statusCode = "", statusName = "All" });
            listStat.Add(new Status() { statusCode = "DOC", statusName = "Dokter" });
            listStat.Add(new Status() { statusCode = "MID", statusName = "Bidan" });

            listStat2.Clear();
            listStat2.Add(new Status() { statusCode = "", statusName = "All" });
            listStat2.Add(new Status() { statusCode = "REG", statusName = "Pendaftaran" });
            listStat2.Add(new Status() { statusCode = "OPN", statusName = "Open" });
            listStat2.Add(new Status() { statusCode = "CLS", statusName = "Closed" });

            luType.Properties.DataSource = listStat;
            luType.Properties.ValueMember = "statusCode";
            luType.Properties.DisplayMember = "statusName";

            luType.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luType.Properties.DropDownRows = listStat.Count;
            luType.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luType.Properties.AutoSearchColumnIndex = 1;
            luType.Properties.NullText = "";
            luType.ItemIndex = 0;

            lookUpEdit1.Properties.DataSource = listStat2;
            lookUpEdit1.Properties.ValueMember = "statusCode";
            lookUpEdit1.Properties.DisplayMember = "statusName";

            lookUpEdit1.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            lookUpEdit1.Properties.DropDownRows = listStat.Count;
            lookUpEdit1.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            lookUpEdit1.Properties.AutoSearchColumnIndex = 1;
            lookUpEdit1.Properties.NullText = "";
            lookUpEdit1.ItemIndex = 0;

            string sql_date="";
            sql_date = " select to_char(sysdate,'yyyy-mm-dd') sdate, to_char(sysdate,'yyyy-mm-dd') edate from dual ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_date, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            sdate = dt.Rows[0]["sdate"].ToString();
            edate = dt.Rows[0]["edate"].ToString();
            dDateBgn.Text = sdate;
            dDateEnd.Text = edate;
        }

        private void LoadData()
        {
            string SQL, p_type = "";

            if (luType.Text == "Umum")
            {
                p_type = "COMM";
            }
            else if (luType.Text == "Ibu Hamil")
            {
                p_type = "PREG";
            }
            else if (luType.Text == "KB")
            {
                p_type = "FAMP";
            }

            //SQL = "";
            //SQL = SQL + Environment.NewLine + "select PATIENT_NO, name, dept, type_patient, work_accident, purpose, que01, visit_date,  ";
            //SQL = SQL + Environment.NewLine + "visit_time, reservation_time, inspection_time, end_time, s_hold, e_hold,nvl(hold,0) hold, ";
            //SQL = SQL + Environment.NewLine + "nvl(rsv,0)-nvl(hold,0) rsv, nvl(ins,0) ins, nvl(med,0) med, nvl((rsv-nvl(hold,0)) + ins + med,0) as total   ";
            //SQL = SQL + Environment.NewLine + "from ( ";
            //SQL = SQL + Environment.NewLine + "select a.PATIENT_NO, name,'' dept, type_patient, work_accident, purpose, ";
            //SQL = SQL + Environment.NewLine + "que01, to_char(visit_date,'yyyy-mm-dd') visit_date, ";
            //SQL = SQL + Environment.NewLine + "to_char(visit_date,'hh24:mi:ss') visit_time, ";
            //SQL = SQL + Environment.NewLine + "to_char(time_reservation,'hh24:mi:ss') reservation_time, ";
            //SQL = SQL + Environment.NewLine + "to_char(time_inspection,'hh24:mi:ss') inspection_time, ";
            //SQL = SQL + Environment.NewLine + "to_char(decode(observation,'Y',time_receipt,time_end),'hh24:mi:ss') end_time, ";
            //SQL = SQL + Environment.NewLine + "to_char(start_hold,'hh24:mi:ss') s_hold,  ";
            //SQL = SQL + Environment.NewLine + "to_char(end_hold,'hh24:mi:ss') e_hold,  ";
            //SQL = SQL + Environment.NewLine + "round((time_reservation-visit_date) * 24 * 60) rsv, ";
            //SQL = SQL + Environment.NewLine + "round((time_inspection-time_reservation) * 24 * 60) ins, ";
            //SQL = SQL + Environment.NewLine + "round((time_receipt-time_inspection) * 24 * 60) med,";
            //SQL = SQL + Environment.NewLine + "round((end_hold-start_hold) * 24 * 60) hold, a.ins_date ";
            //SQL = SQL + Environment.NewLine + "from cs_visit a   ";
            //SQL = SQL + Environment.NewLine + "join cs_patient_info b on (a.PATIENT_NO=b.PATIENT_NO)   ";
            //SQL = SQL + Environment.NewLine + "join cs_patient c on (b.PATIENT_NO=c.PATIENT_NO) ";
            //SQL = SQL + Environment.NewLine + "join cs_anamnesa d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no) ";
            //SQL = SQL + Environment.NewLine + "where 1=1  ";
            //SQL = SQL + Environment.NewLine + "and trunc(visit_date) between to_date('" + dDateBgn.Text + "','yyyy-mm-dd') and to_date('" + dDateEnd.Text + "','yyyy-mm-dd')  ";

            //string Sql = "";
            //Sql = Sql + Environment.NewLine + "select to_char(visit_date,'yyyy-mm-dd') tanggal, ";
            //Sql = Sql + Environment.NewLine + "       to_char(visit_date,'hh24:mi:ss') jam_mulai,to_char(TIME_END,'hh24:mi:ss') jam_akhir,  ";
            //Sql = Sql + Environment.NewLine + "       c.RM_NO, a.PATIENT_NO, b.name,  decode(GENDER,'L','Laki-Laki','Perempuan') GENDER, P_AGE usia,  ADDRESS, ";
            //Sql = Sql + Environment.NewLine + "       decode(type_patient,'B','BPJS','A','ASURANSI','UMUM') type_patient, ";
            //Sql = Sql + Environment.NewLine + "       DECODE(purpose,'DOC','Dokter','MID','Bidan','Lain-Lain') purpose, POLI_NAME  POLI_CD,"; 
            //Sql = Sql + Environment.NewLine + "      e.ITEM_CD||' '|| ITEM_NAME, g.NAME dokter, H.CODE_NAME STATUS ";
            //Sql = Sql + Environment.NewLine + " from KLINIK.cs_visit a    ";
            //Sql = Sql + Environment.NewLine + " join KLINIK.cs_patient_info b on (a.PATIENT_NO=b.PATIENT_NO)    ";
            //Sql = Sql + Environment.NewLine + " join KLINIK.cs_patient c on (b.PATIENT_NO=c.PATIENT_NO)  ";
            //Sql = Sql + Environment.NewLine + " join KLINIK.cs_anamnesa d on (c.rm_no=d.rm_no and a.ID_VISIT=d.ID_VISIT )  ";
            //Sql = Sql + Environment.NewLine + " join KLINIK.cs_diagnosa e on (d.ANAMNESA_ID=e.ANAMNESA_ID and a.ID_VISIT=d.ID_VISIT )  ";
            //Sql = Sql + Environment.NewLine + " join KLINIK.CS_DIAGNOSA_ITEM f on (f.ITEM_CD=e.ITEM_CD )  ";
            //Sql = Sql + Environment.NewLine + " join KLINIK.cs_user g on (e.INS_EMP=g.USER_ID)   ";
            //Sql = Sql + Environment.NewLine + " JOIN KLINIK.CS_CODE_DATA H ON (H.CODE_ID = A.STATUS AND H.CODE_CLASS_ID = 'ST_PASIEN')  ";
            //Sql = Sql + Environment.NewLine + " join KLINIK.CS_POLICLINIC i on(i.POLI_CD = a.POLI_CD)  ";
            //Sql = Sql + Environment.NewLine + "where 1=1   ";
            //Sql = Sql + Environment.NewLine + "and trunc(visit_date) between to_date('" + dDateBgn.Text + "','yyyy-mm-dd') and to_date('" + dDateEnd.Text + "','yyyy-mm-dd')  ";
            
            string Sql = "";
            Sql = Sql + Environment.NewLine + "select distinct e.RM_NO, b.name, B.NID, a.p_age,   decode(b.GENDER,'L','Laki-Laki','Perempuan') gender, B.PHONE, b.ADDRESS, e.DATE_IN,   ";
            Sql = Sql + Environment.NewLine + "       case when TANGGAL_KELUAR = '0001-01-01' then  ";
            Sql = Sql + Environment.NewLine + "            case when trunc(e.DATE_OUT) = trunc(to_date('0001/01/01','YYYY/MM/DD')) then null else e.DATE_OUT end ";
            Sql = Sql + Environment.NewLine + "            when TANGGAL_KELUAR is null then e.DATE_OUT  ";
            Sql = Sql + Environment.NewLine + "        else to_date(TANGGAL_KELUAR,'YYYY-MM-DD') end DATE_OUT,  ";
            Sql = Sql + Environment.NewLine + "       round( case when TANGGAL_KELUAR = '0001-01-01' then  ";
            Sql = Sql + Environment.NewLine + "            case when trunc(e.DATE_OUT) = trunc(to_date('0001/01/01','YYYY/MM/DD')) then null else e.DATE_OUT end ";
            Sql = Sql + Environment.NewLine + "            when TANGGAL_KELUAR is null then e.DATE_OUT  ";
            Sql = Sql + Environment.NewLine + "        else to_date(TANGGAL_KELUAR,'YYYY-MM-DD') end  -e.DATE_IN) DURASI    ";
            Sql = Sql + Environment.NewLine + "      ,decode(type_patient,'B','BPJS','A','ASURANSI','UMUM') type_patient, e.ROOM_ID, DECODE(A.PURPOSE,'DOC','UMUM','BIDAN') POLI  ";
            Sql = Sql + Environment.NewLine + "      ,( ";
            Sql = Sql + Environment.NewLine + "        select LISTAGG(ad.ITEM_CD||' '|| ITEM_NAME, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa   ";
            Sql = Sql + Environment.NewLine + "          from KLINIK.cs_diagnosa ad   join KLINIK.cs_diagnosa_item bd on (ad.item_cd = bd.item_cd)   ";
            Sql = Sql + Environment.NewLine + "          where bd.status = 'A'    ";
            Sql = Sql + Environment.NewLine + "           and ad.ANAMNESA_ID = f.ANAMNESA_ID  ";
            Sql = Sql + Environment.NewLine + "       ) diagnosa, h.KEADAAN_PULANG ";
            Sql = Sql + Environment.NewLine + "      ,( ";
            Sql = Sql + Environment.NewLine + "        select distinct max(NM_DOKTER) ";
            Sql = Sql + Environment.NewLine + "          from KLINIK.cs_treatment_detail ad,  ";
            Sql = Sql + Environment.NewLine + "               KLINIK.cs_treatment_item ac, ";
            Sql = Sql + Environment.NewLine + "               KLINIK.CS_DOKTER ab ";
            Sql = Sql + Environment.NewLine + "         where ad.TREAT_ITEM_ID = ac.TREAT_ITEM_ID ";
            Sql = Sql + Environment.NewLine + "           and (ad.HEAD_ID = g.HEAD_ID)   ";
            Sql = Sql + Environment.NewLine + "           and ad.ID_DOKTER = ab.ID_DOKTER  ";
            Sql = Sql + Environment.NewLine + "           and TREAT_ITEM_NAME ='Dokter Umum'  ";
            Sql = Sql + Environment.NewLine + "           and F_STATUS = type_patient ";
            Sql = Sql + Environment.NewLine + "       ) dokter,a.id_visit , case when g.status = 'OPN' then 'Proses' else i.CODE_NAME end status ";
            Sql = Sql + Environment.NewLine + " FROM cs_visit a  ";
            Sql = Sql + Environment.NewLine + " JOIN cs_patient_info b ON a.patient_no = b.patient_no  ";
            Sql = Sql + Environment.NewLine + " JOIN cs_patient ab     ON ab.patient_no = b.patient_no  ";
            Sql = Sql + Environment.NewLine + " JOIN cs_policlinic c ON a.poli_cd = c.poli_cd  ";
            Sql = Sql + Environment.NewLine + " join cs_receipt d ON a.id_visit = d.id_visit   ";
            Sql = Sql + Environment.NewLine + " join CS_INPATIENT e on a.INPATIENT_ID = e.INPATIENT_ID ";
            Sql = Sql + Environment.NewLine + " join KLINIK.cs_anamnesa f on (ab.rm_no=f.rm_no and a.ID_VISIT=f.ID_VISIT )  ";
            Sql = Sql + Environment.NewLine + " join cs_treatment_head g ON a.id_visit = g.id_visit   ";
            Sql = Sql + Environment.NewLine + " left join T1_PERENCANAAN_PULANG h on f.ANAMNESA_ID = h.ANAMESA_ID  join cs_code_data i on i.code_id = a.status and i.CODE_CLASS_ID = 'ST_PASIEN' ";
            Sql = Sql + Environment.NewLine + " where a.plan ='TRT02'  ";
            if(radioButton1.Checked)
                Sql = Sql + Environment.NewLine + "   and trunc(e.DATE_IN) between to_date('" + dDateBgn.Text + "','yyyy-mm-dd') and to_date('" + dDateEnd.Text + "','yyyy-mm-dd')  ";
            else if (radioButton2.Checked)
                Sql = Sql + Environment.NewLine + "   and trunc(e.DATE_OUT) between to_date('" + dDateBgn.Text + "','yyyy-mm-dd') and to_date('" + dDateEnd.Text + "','yyyy-mm-dd')  ";

            if(luType.EditValue.ToString().Equals("DOC"))
                Sql = Sql + Environment.NewLine + "   and A.PURPOSE  ='" + luType.EditValue.ToString() + "' ";
            else if (luType.EditValue.ToString().Equals("MID"))
                Sql = Sql + Environment.NewLine + "   and A.PURPOSE  ='" + luType.EditValue.ToString() + "' ";
            Sql = Sql + Environment.NewLine + "order by e.DATE_IN  ";


            //if (luType.Text == "Dokter")
            //{
            //    Sql = Sql + Environment.NewLine + "and a.purpose = 'DOC' ";
            //}
            //else if (luType.Text == "Bidan")
            //{
            //    Sql = Sql + Environment.NewLine + "and a.purpose = 'MID' ";
            //}
            //else
            //{

            //}
            //Sql = Sql + Environment.NewLine + "order by 1,2,6,5 ";  

            loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(Sql, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                //gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 60;
                gridView1.OptionsBehavior.Editable = false;

                gridView1.FixedLineWidth = 2;
                gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView1.Columns[0].Caption = "RM NO";  
                gridView1.Columns[1].Caption = "NAMA PASIEN";
                gridView1.Columns[2].Caption = "NIK PASIEN";
                gridView1.Columns[3].Caption = "USIA";
                gridView1.Columns[4].Caption = "GENDER";
                gridView1.Columns[5].Caption = "NO TELP";
                gridView1.Columns[6].Caption = "ALAMAT";
                gridView1.Columns[7].Caption = "TGL MASUK";
                gridView1.Columns[8].Caption = "TGL KELUAR";
                gridView1.Columns[9].Caption = "DURASI HARI";
                gridView1.Columns[10].Caption = "TYPE PASIEN";
                gridView1.Columns[11].Caption = "RUANGAN";
                gridView1.Columns[12].Caption = "POLI";
                gridView1.Columns[13].Caption = "DIAGNOSA";
                gridView1.Columns[14].Caption = "KONDISI PULANG";
                gridView1.Columns[15].Caption = "DOKTER";
                gridView1.Columns[16].Caption = "IDVISIT";
                gridView1.Columns[16].Visible = false;
                //gridView1.Columns[2].Visible = false;
                //RepositoryItemLookUpEdit patientLookup = new RepositoryItemLookUpEdit();
                //patientLookup.DataSource = listPatientType;
                //patientLookup.ValueMember = "patientTypeCode";
                //patientLookup.DisplayMember = "patientTypeName";

                //patientLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //patientLookup.DropDownRows = listPatientType.Count;
                //patientLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                //patientLookup.AutoSearchColumnIndex = 1;
                //patientLookup.NullText = "";
                //gridView1.Columns[3].ColumnEdit = patientLookup;

                //RepositoryItemLookUpEdit workAccLookup = new RepositoryItemLookUpEdit();
                //workAccLookup.DataSource = listWorkAccident;
                //workAccLookup.ValueMember = "workAccidentCode";
                //workAccLookup.DisplayMember = "workAccidentName";

                //workAccLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //workAccLookup.DropDownRows = listWorkAccident.Count;
                //workAccLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                //workAccLookup.AutoSearchColumnIndex = 1;
                //workAccLookup.NullText = "";
                //gridView1.Columns[4].ColumnEdit = workAccLookup;

                //RepositoryItemLookUpEdit picLookup = new RepositoryItemLookUpEdit();
                //picLookup.DataSource = listStat;
                //picLookup.ValueMember = "statusCode";
                //picLookup.DisplayMember = "statusName";

                //picLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //picLookup.DropDownRows = listStat.Count;
                //picLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                //picLookup.AutoSearchColumnIndex = 1;
                //picLookup.NullText = "";
                //gridView1.Columns[5].ColumnEdit = picLookup;

                gridView1.BestFitColumns();
                //gridView1.Columns[18].Width = 100;

                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                //gridView1.Columns[15].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;
                //gridView1.Columns[16].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;
                //gridView1.Columns[17].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;
                //gridView1.Columns[18].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;

                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();
            
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            //if (e.Column.Caption == "KK")
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

            if (e.Column.Caption == "Tipe Pasien")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[3]);
                if (kk == "BPJS")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Green);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.Green);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            //if (e.Column.Caption == "Lama Reservasi")
            //{
            //    string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[15]);
            //    if (e.RowHandle > 0 && Convert.ToInt16(kk) > 60)
            //    {
            //        e.Appearance.BackColor = Color.Red;
            //        e.Appearance.ForeColor = Color.White;
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //    }
            //}
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string stat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);

            //    if (stat == "Over")
            //    {
            //        e.Appearance.BackColor = Color.IndianRed;
            //        e.Appearance.BackColor2 = Color.Firebrick;
            //        e.Appearance.ForeColor = Color.White;
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //        e.HighPriority = true;
            //    }
            //}
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
          
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "laporan_kunjungan.xls",
                    RestoreDirectory = true,
                    CheckFileExists = false,
                    CheckPathExists = true,
                    OverwritePrompt = true,
                    DereferenceLinks = true,
                    ValidateNames = true,
                    AddExtension = false,
                    FilterIndex = 1
                };
                saveDialog.InitialDirectory = "C:\\";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    gridControl1.ExportToXls(saveDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("Data tidak ditemukan");
            }
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            labelControl3.Text = "Tanggal Masuk";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            labelControl3.Text = "Tanggal Keluar";
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}