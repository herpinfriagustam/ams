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
    public partial class VisitReport : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Status> listStat = new List<Status>();
        List<PatientType> listPatientType = new List<PatientType>();
        List<WorkAccident> listWorkAccident = new List<WorkAccident>();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string type = "", sdate="", edate="";

        public VisitReport()
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

            luType.Properties.DataSource = listStat;
            luType.Properties.ValueMember = "statusCode";
            luType.Properties.DisplayMember = "statusName";

            luType.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luType.Properties.DropDownRows = listStat.Count;
            luType.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luType.Properties.AutoSearchColumnIndex = 1;
            luType.Properties.NullText = "";
            luType.ItemIndex = 0;

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

            SQL = "";
            SQL = SQL + Environment.NewLine + "select PATIENT_NO, name, dept, type_patient, work_accident, purpose, que01, visit_date,  ";
            SQL = SQL + Environment.NewLine + "visit_time, reservation_time, inspection_time, end_time, s_hold, e_hold,nvl(hold,0) hold, ";
            SQL = SQL + Environment.NewLine + "nvl(rsv,0)-nvl(hold,0) rsv, nvl(ins,0) ins, nvl(med,0) med, nvl((rsv-nvl(hold,0)) + ins + med,0) as total   ";
            SQL = SQL + Environment.NewLine + "from ( ";
            SQL = SQL + Environment.NewLine + "select a.PATIENT_NO, name,'' dept, type_patient, work_accident, purpose, ";
            SQL = SQL + Environment.NewLine + "que01, to_char(visit_date,'yyyy-mm-dd') visit_date, ";
            SQL = SQL + Environment.NewLine + "to_char(visit_date,'hh24:mi:ss') visit_time, ";
            SQL = SQL + Environment.NewLine + "to_char(time_reservation,'hh24:mi:ss') reservation_time, ";
            SQL = SQL + Environment.NewLine + "to_char(time_inspection,'hh24:mi:ss') inspection_time, ";
            SQL = SQL + Environment.NewLine + "to_char(decode(observation,'Y',time_receipt,time_end),'hh24:mi:ss') end_time, ";
            SQL = SQL + Environment.NewLine + "to_char(start_hold,'hh24:mi:ss') s_hold,  ";
            SQL = SQL + Environment.NewLine + "to_char(end_hold,'hh24:mi:ss') e_hold,  ";
            SQL = SQL + Environment.NewLine + "round((time_reservation-visit_date) * 24 * 60) rsv, ";
            SQL = SQL + Environment.NewLine + "round((time_inspection-time_reservation) * 24 * 60) ins, ";
            SQL = SQL + Environment.NewLine + "round((time_receipt-time_inspection) * 24 * 60) med,";
            SQL = SQL + Environment.NewLine + "round((end_hold-start_hold) * 24 * 60) hold, a.ins_date ";
            SQL = SQL + Environment.NewLine + "from cs_visit a   ";
            SQL = SQL + Environment.NewLine + "join cs_patient_info b on (a.PATIENT_NO=b.PATIENT_NO)   ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (b.PATIENT_NO=c.PATIENT_NO) ";
            SQL = SQL + Environment.NewLine + "join cs_anamnesa d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no) ";
            SQL = SQL + Environment.NewLine + "where 1=1  ";
            SQL = SQL + Environment.NewLine + "and trunc(visit_date) between to_date('" + dDateBgn.Text + "','yyyy-mm-dd') and to_date('" + dDateEnd.Text + "','yyyy-mm-dd')  ";

            if (luType.Text == "Dokter")
            {
                SQL = SQL + Environment.NewLine + "and a.purpose = 'DOC' ";
            }
            else if (luType.Text == "Bidan")
            {
                SQL = SQL + Environment.NewLine + "and a.purpose = 'MID' ";
            }
            else
            {

            }

            SQL = SQL + Environment.NewLine + "and a.status='CLS') a ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and inspection_time is not null ";
            SQL = SQL + Environment.NewLine + "order by ins_date ";


            loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
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

                gridView1.FixedLineWidth = 3;
                gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView1.Columns[0].Caption = "NIK";
                gridView1.Columns[1].Caption = "Nama";
                gridView1.Columns[2].Caption = "Department";
                gridView1.Columns[3].Caption = "Tipe Pasien";
                gridView1.Columns[4].Caption = "KK";
                gridView1.Columns[5].Caption = "Berobat";
                gridView1.Columns[6].Caption = "Antrian";
                gridView1.Columns[7].Caption = "Tgl Kunjungan";
                gridView1.Columns[8].Caption = "Waktu Kunjungan";
                gridView1.Columns[9].Caption = "Waktu Reservasi";
                gridView1.Columns[10].Caption = "Waktu Pemeriksaan";
                gridView1.Columns[11].Caption = "Waktu Selesai";
                gridView1.Columns[12].Caption = "Mulai Tunda";
                gridView1.Columns[13].Caption = "Selesai Tunda";
                gridView1.Columns[14].Caption = "Lama Tunda";
                gridView1.Columns[15].Caption = "Lama Reservasi";
                gridView1.Columns[16].Caption = "Lama Pemeriksaan";
                gridView1.Columns[17].Caption = "Lama Ambil Obat";
                gridView1.Columns[18].Caption = "Total";
                gridView1.Columns[2].Visible = false;
                RepositoryItemLookUpEdit patientLookup = new RepositoryItemLookUpEdit();
                patientLookup.DataSource = listPatientType;
                patientLookup.ValueMember = "patientTypeCode";
                patientLookup.DisplayMember = "patientTypeName";

                patientLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                patientLookup.DropDownRows = listPatientType.Count;
                patientLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                patientLookup.AutoSearchColumnIndex = 1;
                patientLookup.NullText = "";
                gridView1.Columns[3].ColumnEdit = patientLookup;

                RepositoryItemLookUpEdit workAccLookup = new RepositoryItemLookUpEdit();
                workAccLookup.DataSource = listWorkAccident;
                workAccLookup.ValueMember = "workAccidentCode";
                workAccLookup.DisplayMember = "workAccidentName";

                workAccLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                workAccLookup.DropDownRows = listWorkAccident.Count;
                workAccLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                workAccLookup.AutoSearchColumnIndex = 1;
                workAccLookup.NullText = "";
                gridView1.Columns[4].ColumnEdit = workAccLookup;

                RepositoryItemLookUpEdit picLookup = new RepositoryItemLookUpEdit();
                picLookup.DataSource = listStat;
                picLookup.ValueMember = "statusCode";
                picLookup.DisplayMember = "statusName";

                picLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                picLookup.DropDownRows = listStat.Count;
                picLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                picLookup.AutoSearchColumnIndex = 1;
                picLookup.NullText = "";
                gridView1.Columns[5].ColumnEdit = picLookup;

                gridView1.BestFitColumns();
                gridView1.Columns[18].Width = 100;

                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                gridView1.Columns[15].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;
                gridView1.Columns[16].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;
                gridView1.Columns[17].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;
                gridView1.Columns[18].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;

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

            if (e.Column.Caption == "KK")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[4]);
                if (kk == "Yes")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.OrangeRed);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.Caption == "Tipe Pasien")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[3]);
                if (kk == "Emergency")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Red);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.Red);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.Caption == "Lama Reservasi")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[15]);
                if (e.RowHandle > 0 && Convert.ToInt16(kk) > 60)
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
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