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
    public partial class LetterReport : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Status> listPIC = new List<Status>();
        DataSet dsSkd = new DataSet();
        DataSet dsSkdKk = new DataSet();
        DataSet dsSkdRujuk = new DataSet();
        DataSet dsSkdRekom = new DataSet();
        DataSet dsCutiHamil = new DataSet();
        DataSet dsRekomHamil = new DataSet();
        DataSet dsAction= new DataSet();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string type = "";

        public LetterReport()
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
            btnPreview.Enabled = false;
            if (xtraTabControl1.SelectedTabPage.Text == "SKD")
            {
                LoadDataSKD();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "SKD KK")
            {
                LoadDataSKDKK();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Rujukan")
            {
                LoadDataRujuk();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Rekomendasi")
            {
                LoadDataRekom();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Surat Cuti Hamil")
            {
                LoadDataCutiHamil();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Rekomendasi Hamil")
            {
                LoadDataRekomHamil();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Tindakan")
            {
                LoadDataTindakan();
            }

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage.Text == "SKD")
            {
                if (gridView1.RowCount > 0)
                {
                    PreviewDataSKD();
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "SKD KK")
            {
                if (gridView2.RowCount > 0)
                {
                    PreviewDataSKDKK();
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Rujukan")
            {
                if (gridView3.RowCount > 0)
                {
                    PreviewDataRujukan();
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Rekomendasi")
            {
                if (gridView4.RowCount > 0)
                {
                    PreviewDataRekom();
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Surat Cuti Hamil")
            {
                if (gridView5.RowCount > 0)
                {
                    PreviewCutiHamil();
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Rekomendasi Hamil")
            {
                if (gridView6.RowCount > 0)
                {
                    PreviewRekomHamil();
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Tindakan")
            {
                if (gridView7.RowCount > 0)
                {
                    PreviewDataTindakan();
                }
            }
        }

        private void InitData()
        {

            listPIC.Clear();
            listPIC.Add(new Status() { statusCode = "", statusName = "All" });
            listPIC.Add(new Status() { statusCode = "DOC", statusName = "Dokter" });
            listPIC.Add(new Status() { statusCode = "MID", statusName = "Bidan" });

            luType.Properties.DataSource = listPIC;
            luType.Properties.ValueMember = "statusCode";
            luType.Properties.DisplayMember = "statusCode";

            luType.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luType.Properties.DropDownRows = listPIC.Count;
            luType.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luType.Properties.AutoSearchColumnIndex = 1;
            luType.Properties.NullText = "";
            luType.ItemIndex = 0;

            dStartDt.Text = today;
            dEndDate.Text = today;
        }

        private void LoadDataSKD()
        {
            string SQL = "";


            SQL = "";
            SQL = SQL + Environment.NewLine + "select a.patient_no, name, null dept, to_char(d.letter_dt,'yyyy-mm-dd') ldate,  ";
            SQL = SQL + Environment.NewLine + "to_char(d.bgn_rest,'yyyy-mm-dd') sdate, ";
            SQL = SQL + Environment.NewLine + "to_char(d.end_rest,'yyyy-mm-dd') edate, d.cnt_rest, a.purpose, ";
            SQL = SQL + Environment.NewLine + "klinik.FN_GET_NAME(d.ins_emp) pic, ";
            SQL = SQL + Environment.NewLine + "to_char(a.visit_date,'yyyy-mm-dd') visit_date, a.que01, c.rm_no, "; 
            SQL = SQL + Environment.NewLine + "b.gender, round((sysdate-b.birth_date)/30/12) age  ";
            SQL = SQL + Environment.NewLine + "from cs_visit a   ";
            SQL = SQL + Environment.NewLine + "join cs_patient_info b on (a.patient_no=b.patient_no)   ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (a.patient_no=c.patient_no) ";
            SQL = SQL + Environment.NewLine + "join cs_sick_leter d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no) ";
            SQL = SQL + Environment.NewLine + "where 1=1  ";
            SQL = SQL + Environment.NewLine + "and trunc(visit_date) between to_date('" + dStartDt.Text + "','yyyy-mm-dd') and to_date('" + dEndDate.Text + "','yyyy-mm-dd') ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS' ";


            if (luType.Text != "")
            {
                SQL = SQL + Environment.NewLine + "and a.purpose = '" + luType.Text + "' ";
            }

            SQL = SQL + Environment.NewLine + "order by a.ins_date";


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
                
                //gridView1.FixedLineWidth = 5;
                //gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView1.Columns[0].Caption = "NIK";
                gridView1.Columns[1].Caption = "Nama";
                gridView1.Columns[2].Caption = "Departemen";
                gridView1.Columns[3].Caption = "Tanggal Surat";
                gridView1.Columns[4].Caption = "Mulai";
                gridView1.Columns[5].Caption = "Selesai";
                gridView1.Columns[6].Caption = "Lama Hari";
                gridView1.Columns[7].Caption = "Berobat";
                gridView1.Columns[8].Caption = "Nama Pemeriksa";
                gridView1.Columns[9].Caption = "Visit Date";
                gridView1.Columns[10].Caption = "Antrian";
                gridView1.Columns[11].Caption = "RM No";
                gridView1.Columns[12].Caption = "Jenis Kelamin";
                gridView1.Columns[13].Caption = "Umur";

                RepositoryItemLookUpEdit picLookup = new RepositoryItemLookUpEdit();
                picLookup.DataSource = listPIC;
                picLookup.ValueMember = "statusCode";
                picLookup.DisplayMember = "statusName";

                picLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                picLookup.DropDownRows = listPIC.Count;
                picLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                picLookup.AutoSearchColumnIndex = 1;
                picLookup.NullText = "";
                gridView1.Columns[7].ColumnEdit = picLookup;

                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[8].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                gridView1.Columns[2].Visible = false;
                gridView1.Columns[9].Visible = false;
                gridView1.Columns[10].Visible = false;
                gridView1.Columns[11].Visible = false;
                gridView1.Columns[12].Visible = false;
                gridView1.Columns[13].Visible = false;
                gridView1.BestFitColumns();

                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();
            
        }

        private void LoadDataSKDKK()
        {
            string SQL = "";


            SQL = "";
            SQL = SQL + Environment.NewLine + "select a.empid, name, dept, letter_no,  ";
            SQL = SQL + Environment.NewLine + "to_char(d.letter_dt,'yyyy-mm-dd') ldate,  ";
            SQL = SQL + Environment.NewLine + "case when return_work = 'Y' then 'Normal' ";
            SQL = SQL + Environment.NewLine + "when d.bgn_limit is not null and d.end_limit is not null  ";
            SQL = SQL + Environment.NewLine + "then 'Limitasi ' || to_char(d.end_limit - (d.bgn_limit-1)) || ' hari' ";
            SQL = SQL + Environment.NewLine + "when d.bgn_rest is not null and d.end_rest is not null  ";
            SQL = SQL + Environment.NewLine + "then 'Istirahat ' || to_char(d.end_rest - (d.bgn_rest-1)) || ' hari' ";
            SQL = SQL + Environment.NewLine + "end as status, return_work, ";
            SQL = SQL + Environment.NewLine + "to_char(d.bgn_limit,'yyyy-mm-dd') lim_sdate, ";
            SQL = SQL + Environment.NewLine + "to_char(d.end_limit,'yyyy-mm-dd') lim_edate, ";
            SQL = SQL + Environment.NewLine + "limit01, limit02, limit03, remark_machine, limit04, limit05, ";
            SQL = SQL + Environment.NewLine + "limit06, limit07, limit08, limit09, limit10, remark, ";
            SQL = SQL + Environment.NewLine + "to_char(d.bgn_rest,'yyyy-mm-dd') sdate,  ";
            SQL = SQL + Environment.NewLine + "to_char(d.end_rest,'yyyy-mm-dd') edate,  ";
            SQL = SQL + Environment.NewLine + "to_char(d.control,'yyyy-mm-dd') cdate, a.purpose,  ";
            SQL = SQL + Environment.NewLine + "TTIT.FN_GET_NAME(d.ins_emp) pic,  ";
            SQL = SQL + Environment.NewLine + "to_char(a.visit_date,'yyyy-mm-dd') visit_date, a.que01, c.rm_no ";
            SQL = SQL + Environment.NewLine + "from cs_visit a    ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)    ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (b.empid=c.empid)  ";
            SQL = SQL + Environment.NewLine + "join cs_sick_leter d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no)  ";
            SQL = SQL + Environment.NewLine + "where 1=1   ";
            SQL = SQL + Environment.NewLine + "and trunc(visit_date) between to_date('" + dStartDt.Text + "','yyyy-mm-dd') and to_date('" + dEndDate.Text + "','yyyy-mm-dd') ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS'  ";
            SQL = SQL + Environment.NewLine + "and a.work_accident='Y' ";

            if (luType.Text != "")
            {
                SQL = SQL + Environment.NewLine + "and a.purpose = '" + luType.Text + "' ";
            }

            SQL = SQL + Environment.NewLine + "order by a.ins_date";


            loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect2);
                DataTable dt2 = new DataTable();
                adSql.Fill(dt2);

                gridControl2.DataSource = null;
                gridView2.Columns.Clear();
                gridControl2.DataSource = dt2;

                //gridView2.OptionsView.ColumnAutoWidth = true;
                gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView2.IndicatorWidth = 60;
                gridView2.OptionsBehavior.Editable = false;

                

                gridView2.Columns[0].Caption = "NIK";
                gridView2.Columns[1].Caption = "Nama";
                gridView2.Columns[2].Caption = "Departemen";
                gridView2.Columns[3].Caption = "No Surat";
                gridView2.Columns[4].Caption = "Tanggal Surat";
                gridView2.Columns[5].Caption = "Status";
                gridView2.Columns[6].Caption = "Kembali Bekerja";
                gridView2.Columns[7].Caption = "Tgl Limitasi Mulai";
                gridView2.Columns[8].Caption = "Tgl Limitasi Selesai";
                gridView2.Columns[9].Caption = "Non Shift";
                gridView2.Columns[10].Caption = "Limitasi Duduk/Berdiri";
                gridView2.Columns[11].Caption = "Limitasi Mesin";
                gridView2.Columns[12].Caption = "Nama Mesin";
                gridView2.Columns[13].Caption = "Limitasi Bahan Kimia";
                gridView2.Columns[14].Caption = "Limitasi Berjalan";
                gridView2.Columns[15].Caption = "Limitasi Beban";
                gridView2.Columns[16].Caption = "Pengolahan Makanan";
                gridView2.Columns[17].Caption = "Limitasi Ketinggian";
                gridView2.Columns[18].Caption = "Hanya 1 Tangan";
                gridView2.Columns[19].Caption = "Lain-lain";
                gridView2.Columns[20].Caption = "Remark";
                gridView2.Columns[21].Caption = "Tgl Istirahat Mulai";
                gridView2.Columns[22].Caption = "Tgl Istirahat Selesai";
                gridView2.Columns[23].Caption = "Tgl Kontrol";
                gridView2.Columns[24].Caption = "Berobat";
                gridView2.Columns[25].Caption = "Pemeriksa";
                gridView2.Columns[26].Caption = "Visit Date";
                gridView2.Columns[27].Caption = "Antrian";
                gridView2.Columns[28].Caption = "RM No";
                
                

                gridView2.FixedLineWidth = 5;
                gridView2.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView2.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView2.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView2.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView2.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView2.Columns[5].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                RepositoryItemLookUpEdit picLookup = new RepositoryItemLookUpEdit();
                picLookup.DataSource = listPIC;
                picLookup.ValueMember = "statusCode";
                picLookup.DisplayMember = "statusName";

                picLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                picLookup.DropDownRows = listPIC.Count;
                picLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                picLookup.AutoSearchColumnIndex = 1;
                picLookup.NullText = "";
                gridView2.Columns[24].ColumnEdit = picLookup;

                gridView2.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView2.Columns[25].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                gridView2.Columns[26].Visible = false;
                gridView2.Columns[27].Visible = false;
                gridView2.Columns[28].Visible = false;
                gridView2.BestFitColumns();

                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();

        }

        private void LoadDataRujuk()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select a.patient_no, name, null dept, to_char(d.letter_dt,'yyyy-mm-dd') ldate,  ";
            SQL = SQL + Environment.NewLine + "letter_no, hos_name, hos_doc, a.purpose, ";
            SQL = SQL + Environment.NewLine + "klinik.FN_GET_NAME(d.ins_emp) pic, ";
            SQL = SQL + Environment.NewLine + "to_char(a.visit_date,'yyyy-mm-dd') visit_date, a.que01, c.rm_no ";
            SQL = SQL + Environment.NewLine + "from cs_visit a   ";
            SQL = SQL + Environment.NewLine + "join cs_patient_info b on (a.patient_no=b.patient_no)   ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (a.patient_no=c.patient_no) ";
            SQL = SQL + Environment.NewLine + "join cs_refer d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no) ";
            SQL = SQL + Environment.NewLine + "where 1=1  ";
            SQL = SQL + Environment.NewLine + "and trunc(visit_date) between to_date('" + dStartDt.Text + "','yyyy-mm-dd') and to_date('" + dEndDate.Text + "','yyyy-mm-dd') ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS' ";

            if (luType.Text != "")
            {
                SQL = SQL + Environment.NewLine + "and a.purpose = '" + luType.Text + "' ";
            }

            SQL = SQL + Environment.NewLine + "order by a.ins_date";


            loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl3.DataSource = null;
                gridView3.Columns.Clear();
                gridControl3.DataSource = dt;

                //gridView3.OptionsView.ColumnAutoWidth = true;
                gridView3.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView3.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView3.IndicatorWidth = 60;
                gridView3.OptionsBehavior.Editable = false;

                //gridView3.FixedLineWidth = 5;
                //gridView3.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView3.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView3.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView3.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView3.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView3.Columns[0].Caption = "NIK";
                gridView3.Columns[1].Caption = "Nama";
                gridView3.Columns[2].Caption = "Departemen";
                gridView3.Columns[3].Caption = "Tanggal Surat";
                gridView3.Columns[4].Caption = "No Surat";
                gridView3.Columns[5].Caption = "Nama RS";
                gridView3.Columns[6].Caption = "Dokter Rujukan";
                gridView3.Columns[7].Caption = "Berobat";
                gridView3.Columns[8].Caption = "Nama Pemeriksa";
                gridView3.Columns[9].Caption = "Visit Date";
                gridView3.Columns[10].Caption = "Antrian";
                gridView3.Columns[11].Caption = "RM No";

                RepositoryItemLookUpEdit picLookup = new RepositoryItemLookUpEdit();
                picLookup.DataSource = listPIC;
                picLookup.ValueMember = "statusCode";
                picLookup.DisplayMember = "statusName";

                picLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                picLookup.DropDownRows = listPIC.Count;
                picLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                picLookup.AutoSearchColumnIndex = 1;
                picLookup.NullText = "";
                gridView3.Columns[7].ColumnEdit = picLookup;

                gridView3.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView3.Columns[8].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                gridView3.Columns[2].Visible = false;
                gridView3.Columns[9].Visible = false;
                gridView3.Columns[10].Visible = false;
                gridView3.Columns[11].Visible = false;
                gridView3.BestFitColumns();

                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();

        }

        private void LoadDataRekom()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select a.empid, name, dept, to_char(d.letter_dt,'yyyy-mm-dd') ldate, recom_remark, ";
            SQL = SQL + Environment.NewLine + "a.purpose, TTIT.FN_GET_NAME(d.ins_emp) pic, ";
            SQL = SQL + Environment.NewLine + "to_char(a.visit_date,'yyyy-mm-dd') visit_date, a.que01, c.rm_no ";
            SQL = SQL + Environment.NewLine + "from cs_visit a   ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)   ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (b.empid=c.empid) ";
            SQL = SQL + Environment.NewLine + "join cs_recommendation d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no) ";
            SQL = SQL + Environment.NewLine + "where 1=1  ";
            SQL = SQL + Environment.NewLine + "and trunc(visit_date) between to_date('" + dStartDt.Text + "','yyyy-mm-dd') and to_date('" + dEndDate.Text + "','yyyy-mm-dd') ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS' ";
            SQL = SQL + Environment.NewLine + "and d.poli_cd='POL0001' ";

            if (luType.Text != "")
            {
                SQL = SQL + Environment.NewLine + "and a.purpose = '" + luType.Text + "' ";
            }

            SQL = SQL + Environment.NewLine + "order by a.ins_date";


            loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl4.DataSource = null;
                gridView4.Columns.Clear();
                gridControl4.DataSource = dt;

                //gridView4.OptionsView.ColumnAutoWidth = true;
                gridView4.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView4.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView4.IndicatorWidth = 60;
                gridView4.OptionsBehavior.Editable = false;

                //gridView4.FixedLineWidth = 5;
                //gridView4.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView4.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView4.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView4.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView4.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView4.Columns[0].Caption = "NIK";
                gridView4.Columns[1].Caption = "Nama";
                gridView4.Columns[2].Caption = "Departemen";
                gridView4.Columns[3].Caption = "Tanggal Surat";
                gridView4.Columns[4].Caption = "Rekomendasi";
                gridView4.Columns[5].Caption = "Berobat";
                gridView4.Columns[6].Caption = "Nama Pemeriksa";
                gridView4.Columns[7].Caption = "Visit Date";
                gridView4.Columns[8].Caption = "Antrian";
                gridView4.Columns[9].Caption = "RM No";

                RepositoryItemLookUpEdit picLookup = new RepositoryItemLookUpEdit();
                picLookup.DataSource = listPIC;
                picLookup.ValueMember = "statusCode";
                picLookup.DisplayMember = "statusName";

                picLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                picLookup.DropDownRows = listPIC.Count;
                picLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                picLookup.AutoSearchColumnIndex = 1;
                picLookup.NullText = "";
                gridView4.Columns[5].ColumnEdit = picLookup;

                gridView4.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView4.Columns[6].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                gridView4.Columns[7].Visible = false;
                gridView4.Columns[8].Visible = false;
                gridView4.Columns[9].Visible = false;
                gridView4.BestFitColumns();

                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();

        }

        private void LoadDataCutiHamil()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select a.empid, name, dept, decode(cer_type,'B','Kehamilan','M','Keguguran') cer_type,  ";
            SQL = SQL + Environment.NewLine + "to_char(d.letter_dt,'yyyy-mm-dd') ldate,  ";
            SQL = SQL + Environment.NewLine + "letter_no, birth_week, to_char(d.birth_date,'yyyy-mm-dd') bdate, cnt_leave, ";
            SQL = SQL + Environment.NewLine + "to_char(d.bgn_date,'yyyy-mm-dd') bgn_date, to_char(d.end_date,'yyyy-mm-dd') end_date, ";
            SQL = SQL + Environment.NewLine + " to_char(d.bgn_work,'yyyy-mm-dd') bgn_work, ";
            SQL = SQL + Environment.NewLine + "a.purpose, TTIT.FN_GET_NAME(d.ins_emp) pic,  ";
            SQL = SQL + Environment.NewLine + "to_char(a.visit_date,'yyyy-mm-dd') visit_date, a.que01, c.rm_no  ";
            SQL = SQL + Environment.NewLine + "from cs_visit a    ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)    ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (b.empid=c.empid)  ";
            SQL = SQL + Environment.NewLine + "join cs_birth_certificate d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no)  ";
            SQL = SQL + Environment.NewLine + "where 1=1   ";
            SQL = SQL + Environment.NewLine + "and trunc(visit_date) between to_date('" + dStartDt.Text + "','yyyy-mm-dd') and to_date('" + dEndDate.Text + "','yyyy-mm-dd')  ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS'  ";
            SQL = SQL + Environment.NewLine + "and a.poli_cd='POL0002'  ";

            if (luType.Text != "")
            {
                SQL = SQL + Environment.NewLine + "and a.purpose = '" + luType.Text + "' ";
            }

            SQL = SQL + Environment.NewLine + "order by a.ins_date";


            loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl5.DataSource = null;
                gridView5.Columns.Clear();
                gridControl5.DataSource = dt;

                //gridView5.OptionsView.ColumnAutoWidth = true;
                gridView5.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView5.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView5.IndicatorWidth = 60;
                gridView5.OptionsBehavior.Editable = false;

                //gridView5.FixedLineWidth = 5;
                //gridView5.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView5.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView5.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView5.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView5.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView5.Columns[0].Caption = "NIK";
                gridView5.Columns[1].Caption = "Nama";
                gridView5.Columns[2].Caption = "Departemen";
                gridView5.Columns[3].Caption = "Tipe Surat";
                gridView5.Columns[4].Caption = "Tanggal Surat";
                gridView5.Columns[5].Caption = "No Surat";
                gridView5.Columns[6].Caption = "Minggu Kehamilan";
                gridView5.Columns[7].Caption = "Tanggal Kelahiran";
                gridView5.Columns[8].Caption = "Lama Cuti";
                gridView5.Columns[9].Caption = "Mulai Cuti";
                gridView5.Columns[10].Caption = "Selesai Cuti";
                gridView5.Columns[11].Caption = "Mulai Masuk";
                gridView5.Columns[12].Caption = "Berobat";
                gridView5.Columns[13].Caption = "Nama Pemeriksa";
                gridView5.Columns[14].Caption = "Visit Date";
                gridView5.Columns[15].Caption = "Antrian";
                gridView5.Columns[16].Caption = "RM No";

                RepositoryItemLookUpEdit picLookup = new RepositoryItemLookUpEdit();
                picLookup.DataSource = listPIC;
                picLookup.ValueMember = "statusCode";
                picLookup.DisplayMember = "statusName";

                picLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                picLookup.DropDownRows = listPIC.Count;
                picLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                picLookup.AutoSearchColumnIndex = 1;
                picLookup.NullText = "";
                gridView5.Columns[12].ColumnEdit = picLookup;

                gridView5.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView5.Columns[13].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                gridView5.Columns[14].Visible = false;
                gridView5.Columns[15].Visible = false;
                gridView5.Columns[16].Visible = false;
                gridView5.BestFitColumns();

                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();

        }

        private void LoadDataRekomHamil()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select a.empid, name, dept, to_char(d.letter_dt,'yyyy-mm-dd') ldate, ";
            SQL = SQL + Environment.NewLine + "decode(recom_01,'Y','Tidak lembur','') rekom_1,  ";
            SQL = SQL + Environment.NewLine + "decode(recom_02,'Y','Tidak shift malam','') rekom_2,  ";
            SQL = SQL + Environment.NewLine + "decode(recom_03,'Y','Tidak berdiri lama dalam bekerja','') rekom_3, ";
            SQL = SQL + Environment.NewLine + "decode(recom_04,'Y','Tidak bekerja yang perlu keseimbangan','') rekom_4, ";
            SQL = SQL + Environment.NewLine + "a.purpose, TTIT.FN_GET_NAME(d.ins_emp) pic, ";
            SQL = SQL + Environment.NewLine + "to_char(a.visit_date,'yyyy-mm-dd') visit_date, a.que01, c.rm_no ";
            SQL = SQL + Environment.NewLine + "from cs_visit a   ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)   ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (b.empid=c.empid) ";
            SQL = SQL + Environment.NewLine + "join cs_recommendation d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no) ";
            SQL = SQL + Environment.NewLine + "where 1=1  ";
            SQL = SQL + Environment.NewLine + "and trunc(visit_date) between to_date('" + dStartDt.Text + "','yyyy-mm-dd') and to_date('" + dEndDate.Text + "','yyyy-mm-dd') ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS' ";
            SQL = SQL + Environment.NewLine + "and d.poli_cd='POL0002' ";

            if (luType.Text != "")
            {
                SQL = SQL + Environment.NewLine + "and a.purpose = '" + luType.Text + "' ";
            }

            SQL = SQL + Environment.NewLine + "order by a.ins_date";


            loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl6.DataSource = null;
                gridView6.Columns.Clear();
                gridControl6.DataSource = dt;

                //gridView6.OptionsView.ColumnAutoWidth = true;
                gridView6.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView6.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView6.IndicatorWidth = 60;
                gridView6.OptionsBehavior.Editable = false;

                //gridView5.FixedLineWidth = 5;
                //gridView5.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView5.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView5.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView5.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView5.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView6.Columns[0].Caption = "NIK";
                gridView6.Columns[1].Caption = "Nama";
                gridView6.Columns[2].Caption = "Departemen";
                gridView6.Columns[3].Caption = "Tanggal Surat";
                gridView6.Columns[4].Caption = "Rekomendasi 1";
                gridView6.Columns[5].Caption = "Rekomendasi 2";
                gridView6.Columns[6].Caption = "Rekomendasi 3";
                gridView6.Columns[7].Caption = "Rekomendasi 4";
                gridView6.Columns[8].Caption = "Berobat";
                gridView6.Columns[9].Caption = "Nama Pemeriksa";
                gridView6.Columns[10].Caption = "Visit Date";
                gridView6.Columns[11].Caption = "Antrian";
                gridView6.Columns[12].Caption = "RM No";

                RepositoryItemLookUpEdit picLookup = new RepositoryItemLookUpEdit();
                picLookup.DataSource = listPIC;
                picLookup.ValueMember = "statusCode";
                picLookup.DisplayMember = "statusName";

                picLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                picLookup.DropDownRows = listPIC.Count;
                picLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                picLookup.AutoSearchColumnIndex = 1;
                picLookup.NullText = "";
                gridView6.Columns[8].ColumnEdit = picLookup;

                gridView6.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView6.Columns[9].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                gridView6.Columns[10].Visible = false;
                gridView6.Columns[11].Visible = false;
                gridView6.Columns[12].Visible = false;
                gridView6.BestFitColumns();

                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();

        }

        private void LoadDataTindakan()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select a.empid, name, dept, to_char(d.insp_date,'yyyy-mm-dd') ldate,   ";
            SQL = SQL + Environment.NewLine + "decode(act_type,'A','Persetujuan','D','Penolakan','') act_type, act_name, act_remark, a.purpose,  ";
            SQL = SQL + Environment.NewLine + "TTIT.FN_GET_NAME(d.ins_emp) pic, ";
            SQL = SQL + Environment.NewLine + "to_char(a.visit_date,'yyyy-mm-dd') visit_date, a.que01, c.rm_no  ";
            SQL = SQL + Environment.NewLine + "from cs_visit a    ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)    ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (b.empid=c.empid)  ";
            SQL = SQL + Environment.NewLine + "join cs_action d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no)  ";
            SQL = SQL + Environment.NewLine + "where 1=1   ";
            SQL = SQL + Environment.NewLine + "and trunc(visit_date) between to_date('" + dStartDt.Text + "','yyyy-mm-dd') and to_date('" + dEndDate.Text + "','yyyy-mm-dd')  ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS'  ";

            if (luType.Text != "")
            {
                SQL = SQL + Environment.NewLine + "and a.purpose = '" + luType.Text + "' ";
            }

            SQL = SQL + Environment.NewLine + "order by a.ins_date";


            loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl7.DataSource = null;
                gridView7.Columns.Clear();
                gridControl7.DataSource = dt;

                //gridView7.OptionsView.ColumnAutoWidth = true;
                gridView7.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView7.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView7.IndicatorWidth = 60;
                gridView7.OptionsBehavior.Editable = false;

                //gridView7.FixedLineWidth = 5;
                //gridView7.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView7.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView7.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView7.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView7.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView7.Columns[0].Caption = "NIK";
                gridView7.Columns[1].Caption = "Nama";
                gridView7.Columns[2].Caption = "Departemen";
                gridView7.Columns[3].Caption = "Tanggal Surat";
                gridView7.Columns[4].Caption = "Tipe";
                gridView7.Columns[5].Caption = "Tindakan";
                gridView7.Columns[6].Caption = "Remark";
                gridView7.Columns[7].Caption = "Berobat";
                gridView7.Columns[8].Caption = "Nama Pemeriksa";
                gridView7.Columns[9].Caption = "Visit Date";
                gridView7.Columns[10].Caption = "Antrian";
                gridView7.Columns[11].Caption = "RM No";

                RepositoryItemLookUpEdit picLookup = new RepositoryItemLookUpEdit();
                picLookup.DataSource = listPIC;
                picLookup.ValueMember = "statusCode";
                picLookup.DisplayMember = "statusName";

                picLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                picLookup.DropDownRows = listPIC.Count;
                picLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                picLookup.AutoSearchColumnIndex = 1;
                picLookup.NullText = "";
                gridView7.Columns[7].ColumnEdit = picLookup;

                gridView7.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView7.Columns[8].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                gridView7.Columns[9].Visible = false;
                gridView7.Columns[10].Visible = false;
                gridView7.Columns[11].Visible = false;
                gridView7.BestFitColumns();

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

            if (e.Column.Caption == "Trans Type")
            {
                string type = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
                if (type == "IN")
                {
                    //e.Appearance.BackColor = Color.FromArgb(150, Color.Blue);
                    //e.Appearance.BackColor2 = Color.FromArgb(150, Color.Blue);

                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else
                {
                    e.Appearance.BackColor = Color.Crimson;
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
            btnPreview.Enabled = true;
            //GridView View = sender as GridView;
            //string s_status = "", sql_chk = "";

            //s_status = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);

            //if (s_status == "Over")
            //{
            //    btnObsCls.Enabled = true;
            //}
            //else
            //{
            //    btnObsCls.Enabled = false;
            //}
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage.Text == "SKD")
            {
                if (gridView1.RowCount > 0)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog
                    {
                        Filter = "XLS (*.xls)|*.xlsx",
                        FileName = "skd_list.xls",
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
            else if (xtraTabControl1.SelectedTabPage.Text == "SKD KK")
            {
                if (gridView2.RowCount > 0)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog
                    {
                        Filter = "XLS (*.xls)|*.xlsx",
                        FileName = "skd_kk_list.xls",
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
                        gridControl2.ExportToXls(saveDialog.FileName);
                    }
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Rujukan")
            {
                if (gridView3.RowCount > 0)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog
                    {
                        Filter = "XLS (*.xls)|*.xlsx",
                        FileName = "rujukan_list.xls",
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
                        gridControl3.ExportToXls(saveDialog.FileName);
                    }
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Rekomendasi")
            {
                if (gridView4.RowCount > 0)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog
                    {
                        Filter = "XLS (*.xls)|*.xlsx",
                        FileName = "rekomendasi_list.xls",
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
                        gridControl4.ExportToXls(saveDialog.FileName);
                    }
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Surat Cuti Hamil")
            {
                if (gridView5.RowCount > 0)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog
                    {
                        Filter = "XLS (*.xls)|*.xlsx",
                        FileName = "cuti_hamil_list.xls",
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
                        gridControl5.ExportToXls(saveDialog.FileName);
                    }
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Rekomendasi Hamil")
            {
                if (gridView6.RowCount > 0)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog
                    {
                        Filter = "XLS (*.xls)|*.xlsx",
                        FileName = "rekomendasi_hamil_list.xls",
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
                        gridControl6.ExportToXls(saveDialog.FileName);
                    }
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Tindakan")
            {
                if (gridView7.RowCount > 0)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog
                    {
                        Filter = "XLS (*.xls)|*.xlsx",
                        FileName = "tindakan.xls",
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
                        gridControl7.ExportToXls(saveDialog.FileName);
                    }
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
        }

        private void PreviewDataSKD()
        {
            string sql = "";
            string p_date = "", p_que = "", p_rm = "", p_berobat = "", berobat = "";

            p_berobat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[7]).ToString();
            p_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[9]).ToString();
            p_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            p_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[11]).ToString();

            string sql_skd = "";

            if (p_berobat == "MID")
            {
                berobat = "PREG";
            }
            else
            {
                berobat = "COMM";
            }

            sql_skd = "  select a.patient_no, a.name, a.gender, round((sysdate-b.birth_date)/30/12) age, null dept, null position, " +
                      "  (select LISTAGG(item_name, ', ') WITHIN GROUP(ORDER BY type_diagnosa asc) diagnosa " +
                      "  from cs_diagnosa a   join cs_diagnosa_item b on (a.item_cd = b.item_cd) " +
                      "  where b.status = 'A' " +
                      "  and rm_no = c.rm_no " +
                      "  and insp_date = trunc(b.visit_date) " +
                      "  and visit_no = b.que01) as diagnosa, letter_no, " +
                      "  TO_CHAR(visit_date, 'dd Month yyyy', 'nls_date_language = INDONESIAN') visit_date, " +
                      "  TO_CHAR(letter_dt, 'dd fmMonth yyyy', 'nls_date_language = INDONESIAN') letter_dt, " +
                      "  TO_CHAR(bgn_rest, 'dd Mon yyyy', 'nls_date_language = INDONESIAN') bgn_rest, " +
                      "  TO_CHAR(end_rest, 'dd Mon yyyy', 'nls_date_language = INDONESIAN') end_rest, cnt_rest, " +
                      "  TO_CHAR(bgn_limit, 'dd Mon yyyy', 'nls_date_language = INDONESIAN') bgn_limit, " +
                      "  TO_CHAR(end_limit, 'dd Mon yyyy', 'nls_date_language = INDONESIAN') end_limit, " +
                      "  decode(limit01, 'Y','V','') limit01,  " +
                      "  decode(limit02, 'Y','V','') limit02,  " +
                      "  decode(limit03, 'Y','V','') limit03, remark_machine, " +
                      "  decode(limit04, 'Y','V','') limit04,  " +
                      "  decode(limit05, 'Y','V','') limit05,  " +
                      "  decode(limit06, 'Y','V','') limit06,  " +
                      "  decode(limit07, 'Y','V','') limit07,  " +
                      "  decode(limit08, 'Y','V','') limit08,  " +
                      "  decode(limit09, 'Y','V','') limit09,  " +
                      "  decode(limit10, 'Y','V','') limit10, remark, decode(return_work, 'Y','V','') return_work, " +
                      "  TO_CHAR(control, 'dd Month yyyy', 'nls_date_language = INDONESIAN') control, b.purpose, " +
                      "  decode (b.purpose,'DOC','dr. ','') || (select distinct TTIT.FN_GET_NAME(ins_emp) nama " +
                      "   from cs_diagnosa a     " +
                      "   where rm_no = c.rm_no    " +
                      "   and insp_date = trunc(visit_date) " +
                      "   and type_diagnosa = 'P' " +
                      "   and visit_no = que01) pic, " +
                      "  'Dokter Pemeriksa' as pic_info " +
                      "  from cs_patient_info a " +
                      "  join cs_visit b on (a.patient_no = b.patient_no) " +
                      "  join cs_patient c on(b.patient_no = c.patient_no) " +
                      "  join cs_sick_leter d on(c.rm_no = d.rm_no) " +
                      "  where b.que01 = d.visit_no " +
                      "  and trunc(b.visit_date) = d.insp_date " +
                      "  and to_char(b.visit_date, 'yyyy-mm-dd') = '" + p_date + "' " +
                      "  and c.status = 'A'   and b.que01 = '" + p_que + "' " +
                      "  and c.group_patient = '" + berobat + "'   and c.rm_no = '" + p_rm + "'  ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_skd, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            dsSkd.Tables.Clear();
            dsSkd.Tables.Add(dt);

            ReportSkdUmum report = new ReportSkdUmum(dsSkd);
            report.ShowPreviewDialog();
        }

        private void PreviewDataSKDKK()
        {
            string sql = "";
            string p_date = "", p_que = "", p_rm = "";

            p_date = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[26]).ToString();
            p_que = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[27]).ToString();
            p_rm = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[28]).ToString();

            string sql_skd = "";

            sql_skd = "  select a.empid, a.name, a.gender, a.age, a.dept, a.position, " +
                      "  (select LISTAGG(item_name, ', ') WITHIN GROUP(ORDER BY type_diagnosa asc) diagnosa " +
                      "  from cs_diagnosa a   join cs_diagnosa_item b on (a.item_cd = b.item_cd) " +
                      "  where b.status = 'A' " +
                      "  and rm_no = c.rm_no " +
                      "  and insp_date = trunc(b.visit_date) " +
                      "  and visit_no = b.que01) as diagnosa, letter_no, " +
                      "  TO_CHAR(visit_date, 'dd Month yyyy', 'nls_date_language = INDONESIAN') visit_date, " +
                      "  TO_CHAR(letter_dt, 'dd fmMonth yyyy', 'nls_date_language = INDONESIAN') letter_dt, " +
                      "  TO_CHAR(bgn_rest, 'dd Mon yyyy', 'nls_date_language = INDONESIAN') bgn_rest, " +
                      "  TO_CHAR(end_rest, 'dd Mon yyyy', 'nls_date_language = INDONESIAN') end_rest, cnt_rest, " +
                      "  TO_CHAR(bgn_limit, 'dd Mon yyyy', 'nls_date_language = INDONESIAN') bgn_limit, " +
                      "  TO_CHAR(end_limit, 'dd Mon yyyy', 'nls_date_language = INDONESIAN') end_limit, " +
                      "  decode(limit01, 'Y','V','') limit01,  " +
                      "  decode(limit02, 'Y','V','') limit02,  " +
                      "  decode(limit03, 'Y','V','') limit03, remark_machine, " +
                      "  decode(limit04, 'Y','V','') limit04,  " +
                      "  decode(limit05, 'Y','V','') limit05,  " +
                      "  decode(limit06, 'Y','V','') limit06,  " +
                      "  decode(limit07, 'Y','V','') limit07,  " +
                      "  decode(limit08, 'Y','V','') limit08,  " +
                      "  decode(limit09, 'Y','V','') limit09,  " +
                      "  decode(limit10, 'Y','V','') limit10, remark, decode(return_work, 'Y','V','') return_work, " +
                      "  TO_CHAR(control, 'dd Month yyyy', 'nls_date_language = INDONESIAN') control, b.purpose, " +
                      "  decode (b.purpose,'DOC','dr. ','') || (select distinct TTIT.FN_GET_NAME(ins_emp) nama " +
                      "   from cs_diagnosa a     " +
                      "   where rm_no = c.rm_no    " +
                      "   and insp_date = trunc(visit_date) " +
                      "   and visit_no = que01) pic, " +
                      "  'Dokter Pemeriksa' as pic_info " +
                      "  from cs_employees a " +
                      "  join cs_visit b on (a.empid = b.empid) " +
                      "  join cs_patient c on(b.empid = c.empid) " +
                      "  join cs_sick_leter d on(c.rm_no = d.rm_no) " +
                      "  where b.que01 = d.visit_no " +
                      "  and trunc(b.visit_date) = d.insp_date " +
                      "  and to_char(b.visit_date, 'yyyy-mm-dd') = '" + p_date + "' " +
                      "  and c.status = 'A'   and b.que01 = '" + p_que + "' " +
                      "  and c.group_patient = 'COMM'   and c.rm_no = '" + p_rm + "'  ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_skd, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            dsSkdKk.Tables.Clear();
            dsSkdKk.Tables.Add(dt);

            ReportSkdKK report = new ReportSkdKK(dsSkdKk);
            report.ShowPreviewDialog();
        }

        private void PreviewDataRujukan()
        {
            string sql = "";
            string p_date = "", p_que = "", p_rm = "";

            p_date = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns[9]).ToString();
            p_que = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns[10]).ToString();
            p_rm = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns[11]).ToString();

            string sql_skd = "";

            sql_skd  = " select a.patient_no, a.name, a.address, round((sysdate-b.birth_date)/30/12) age, a.gender, " +
                       " c.rm_no, to_char(b.visit_date,'yyyy-mm-dd') visit_date, que01, refer_id," +
                       " decode(work_accident,'N','Bukan Kecelakaan Kerja','Kecelakaan Kerja') work_accident, " +
                       " (select disease_now || ' - ' || disease_then    " +
                       " from cs_anamnesa " +
                       " where rm_no=c.rm_no " +
                       " and insp_date=trunc(b.visit_date)  " +
                       " and visit_no=b.que01) riwayat,  " +
                       " (select LISTAGG(initcap(item_name), ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa  " +
                       " from cs_diagnosa a  " +
                       " join cs_diagnosa_item b on (a.item_cd=b.item_cd)  " +
                       " where b.status='A'  " +
                       " and rm_no=c.rm_no  " +
                       " and insp_date=trunc(b.visit_date) " +
                       " and visit_no=b.que01) as diagnosa, " +
                       " (select LISTAGG(initcap(med_name), ', ') WITHIN GROUP (ORDER BY med_name asc) resep   " +
                       " from cs_receipt a " +
                       " join cs_medicine b on (a.med_cd=b.med_cd)  " +
                       " where b.status='A'  " +
                       " and rm_no=c.rm_no  " +
                       " and insp_date=trunc(b.visit_date) " +
                       " and visit_no=b.que01) as resep," +
                       " to_char(nvl(letter_dt,sysdate),'yyyy-mm-dd') letter_dt, hos_doc, hos_name, letter_no, " +
                       " TO_CHAR(letter_dt, 'dd Month yyyy','nls_date_language = INDONESIAN') letter_dt2 " +
                       " from cs_patient_info a  " +
                       " join cs_visit b on (a.patient_no = b.patient_no)  " +
                       " join cs_patient c on(b.patient_no = c.patient_no)  " +
                       " left join cs_refer d on (c.rm_no = d.rm_no and trunc(visit_date)=d.insp_date and que01=d.visit_no)  " +
                       " where  to_char(b.visit_date, 'yyyy-mm-dd') = '" + p_date + "'  " +
                       " and c.status = 'A'  " +
                       " and b.que01 = '" + p_que + "'  " +
                       " and c.group_patient = 'COMM'  " +
                       " and c.rm_no = '" + p_rm + "' ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_skd, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            dsSkdRujuk.Tables.Clear();
            dsSkdRujuk.Tables.Add(dt);

            ReportRujukan report = new ReportRujukan(dsSkdRujuk);
            report.ShowPreviewDialog();
        }


        private void PreviewDataRekom()
        {
            string sql = "";
            string p_date = "", p_que = "", p_rm = "";

            p_date = gridView4.GetRowCellValue(gridView4.FocusedRowHandle, gridView4.Columns[7]).ToString();
            p_que = gridView4.GetRowCellValue(gridView4.FocusedRowHandle, gridView4.Columns[8]).ToString();
            p_rm = gridView4.GetRowCellValue(gridView4.FocusedRowHandle, gridView4.Columns[9]).ToString();

            string sql_skd = "";

            sql_skd =  " select a.empid, a.name, a.line, a.age,  " +
                       " c.rm_no, b.poli_cd, to_char(b.visit_date,'yyyy-mm-dd') visit_date, que01, recom_id, " +
                       " (select  'Tensi : ' || blood_press || ', Nadi : ' || pulse || " +
                       " ', Suhu : ' || temperature || ', Alergi : ' || allergy || ', Keluhan : ' || anamnesa as anamnesa   " +
                       " from cs_anamnesa " +
                       " where rm_no=c.rm_no " +
                       " and insp_date=trunc(b.visit_date)  " +
                       " and visit_no=b.que01) anamnesa, " +
                       " (select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa  " +
                       " from cs_diagnosa a  " +
                       " join cs_diagnosa_item b on (a.item_cd=b.item_cd)  " +
                       " where b.status='A'  " +
                       " and rm_no=c.rm_no  " +
                       " and insp_date=trunc(b.visit_date) " +
                       " and visit_no=b.que01) as diagnosa, " +
                       " to_char(nvl(letter_dt,sysdate),'yyyy-mm-dd') letter_dt, recom_remark, " +
                       " TO_CHAR(letter_dt, 'fmdd Month yyyy','nls_date_language = INDONESIAN') letter_dt2  " +
                       " from cs_employees a  " +
                       " join cs_visit b on (a.empid = b.empid)  " +
                       " join cs_patient c on(b.empid = c.empid)  " +
                       " left join cs_recommendation d on (c.rm_no = d.rm_no and trunc(visit_date)=d.insp_date and que01=d.visit_no) " +
                       " where  to_char(b.visit_date, 'yyyy-mm-dd') = '" + p_date + "'  " +
                       " and c.status = 'A'  " +
                       " and b.que01 = '" + p_que + "'  " +
                       " and c.group_patient = 'COMM'  " +
                       " and c.rm_no = '" + p_rm + "' ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_skd, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            dsSkdRekom.Tables.Clear();
            dsSkdRekom.Tables.Add(dt);

            ReportRekomendasics report = new ReportRekomendasics(dsSkdRekom);
            report.ShowPreviewDialog();
        }

        private void PreviewCutiHamil()
        {
            string sql = "";
            string p_date = "", p_que = "", p_rm = "";

            p_date = gridView5.GetRowCellValue(gridView5.FocusedRowHandle, gridView5.Columns[14]).ToString();
            p_que = gridView5.GetRowCellValue(gridView5.FocusedRowHandle, gridView5.Columns[15]).ToString();
            p_rm = gridView5.GetRowCellValue(gridView5.FocusedRowHandle, gridView5.Columns[16]).ToString();

            string sql_load = "";

            sql_load = "";
            sql_load = sql_load + Environment.NewLine + "SELECT x.empid, x.NAME, x.line, x.age, x.rm_no, x.poli_cd, x.visit_date,que01, cer_id, ";
            sql_load = sql_load + Environment.NewLine + "       DECODE (cer_type, 'B', 'Melahirkan', 'M', 'Keguguran', null) cer_type, ";
            sql_load = sql_load + Environment.NewLine + "       letter_no, info02, info07, cnt_leave, info09, info10, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (bgn_work, 'yyyy-mm-dd') bgn_work, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (NVL (letter_dt, SYSDATE), 'yyyy-mm-dd') letter_dt, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (letter_dt,'dd fmMonth yyyy','nls_date_language = INDONESIAN') letter_dt2, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (birth_date,'dd fmMonth yyyy','nls_date_language = INDONESIAN') birth_date2, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (TO_DATE (info09, 'yyyy-mm-dd'),'dd fmMonth yyyy','nls_date_language = INDONESIAN') bgn_date2, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (TO_DATE (info10, 'yyyy-mm-dd'),'dd fmMonth yyyy','nls_date_language = INDONESIAN') end_date2, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (bgn_work,'dd fmMonth yyyy','nls_date_language = INDONESIAN') bgn_work2 ";
            sql_load = sql_load + Environment.NewLine + "  FROM (SELECT a.empid, a.NAME, a.line, a.age, c.rm_no, b.poli_cd, ";
            sql_load = sql_load + Environment.NewLine + "               TO_CHAR (b.visit_date, 'yyyy-mm-dd') visit_date, que01, info02, ";
            sql_load = sql_load + Environment.NewLine + "               info07, info09, info10 ";
            sql_load = sql_load + Environment.NewLine + "          FROM cs_employees a JOIN cs_visit b ON (a.empid = b.empid) ";
            sql_load = sql_load + Environment.NewLine + "               JOIN cs_patient c ON (b.empid = c.empid) ";
            sql_load = sql_load + Environment.NewLine + "         WHERE c.status = 'A' AND c.group_patient <> 'COMM') x ";
            sql_load = sql_load + Environment.NewLine + "  LEFT JOIN cs_birth_certificate y  ";
            sql_load = sql_load + Environment.NewLine + "  ON ( x.rm_no = y.rm_no AND TO_DATE (x.visit_date, 'yyyy-mm-dd') = y.insp_date AND x.que01 = y.visit_no ) ";
            sql_load = sql_load + Environment.NewLine + "  WHERE 1=1 ";
            //sql_load = sql_load + Environment.NewLine + "  AND  que01 = '" + s_que + "' ";
            //sql_load = sql_load + Environment.NewLine + "  AND  x.visit_date = '" + s_date + "' ";
            sql_load = sql_load + Environment.NewLine + "  AND (cer_id is not null or rownum<=1)";
            sql_load = sql_load + Environment.NewLine + "  AND  x.rm_no = '" + p_rm + "' ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_load, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            dsCutiHamil.Tables.Clear();
            dsCutiHamil.Tables.Add(dt);

            ReportCutiHamil report = new ReportCutiHamil(dsCutiHamil);
            report.ShowPreviewDialog();
        }

        private void PreviewRekomHamil()
        {
            string sql = "";
            string p_date = "", p_que = "", p_rm = "";

            p_date = gridView6.GetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns[10]).ToString();
            p_que = gridView6.GetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns[11]).ToString();
            p_rm = gridView6.GetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns[12]).ToString();

            string sql_load = "";

            sql_load = "";
            sql_load = sql_load + Environment.NewLine + "SELECT x.empid, x.NAME, x.line, x.age, x.rm_no, x.poli_cd, x.visit_date, ";
            sql_load = sql_load + Environment.NewLine + "       que01, recom_id, address, letter_no, info01, info02, info03, info05, ";
            sql_load = sql_load + Environment.NewLine + "       NVL (recom_01, 'N') recom_01, NVL (recom_02, 'N') recom_02, ";
            sql_load = sql_load + Environment.NewLine + "       NVL (recom_03, 'N') recom_03, NVL (recom_04, 'N') recom_04, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (NVL (letter_dt, SYSDATE), 'yyyy-mm-dd') letter_dt,recom_remark, ";
            sql_load = sql_load + Environment.NewLine + "       TO_CHAR (letter_dt,'dd fmMonth yyyy','nls_date_language = INDONESIAN') letter_dt2 ";
            sql_load = sql_load + Environment.NewLine + "  FROM (SELECT a.empid, a.NAME, a.line, a.age, c.rm_no, b.poli_cd, ";
            sql_load = sql_load + Environment.NewLine + "               TO_CHAR (b.visit_date, 'yyyy-mm-dd') visit_date, address, ";
            sql_load = sql_load + Environment.NewLine + "               que01, info01, info02, info03, info05 ";
            sql_load = sql_load + Environment.NewLine + "          FROM cs_employees a JOIN cs_visit b ON (a.empid = b.empid) ";
            sql_load = sql_load + Environment.NewLine + "               JOIN cs_patient c ON (b.empid = c.empid) ";
            sql_load = sql_load + Environment.NewLine + "         WHERE c.status = 'A' AND c.group_patient <> 'COMM') x ";
            sql_load = sql_load + Environment.NewLine + "  LEFT JOIN cs_recommendation y  ";
            sql_load = sql_load + Environment.NewLine + "       ON ( x.rm_no = y.rm_no AND TO_DATE (x.visit_date, 'yyyy-mm-dd') = y.insp_date AND x.que01 = y.visit_no ) ";
            sql_load = sql_load + Environment.NewLine + " WHERE 1 = 1  ";
            //sql_load = sql_load + Environment.NewLine + "   AND  que01 = '" + s_que + "'  ";
            //sql_load = sql_load + Environment.NewLine + "   AND  x.visit_date = '" + s_date + "'  ";
            sql_load = sql_load + Environment.NewLine + "   AND (recom_id is not null or rownum<=1)";
            sql_load = sql_load + Environment.NewLine + "   AND x.rm_no = '" + p_rm + "' ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_load, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            dsRekomHamil.Tables.Clear();
            dsRekomHamil.Tables.Add(dt);

            ReportRekomendasiPr report = new ReportRekomendasiPr(dsRekomHamil);
            report.ShowPreviewDialog();
        }

        private void PreviewDataTindakan()
        {
            string sql = "";
            string p_date = "", p_que = "", p_rm = "";

            p_date = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns[9]).ToString();
            p_que = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns[10]).ToString();
            p_rm = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns[11]).ToString();

            string sql_load = "";

            sql_load = " select a.empid, a.name, a.gender, a.age, a.line, c.rm_no, to_char(b.visit_date,'yyyy-mm-dd') visit_date, que01, act_id, " +
                       " TTIT.FN_GET_PIC(c.rm_no, trunc(visit_date), que01) pic, " +
                       " 'Subang, ' || TO_CHAR(b.visit_date, 'fmdd Month yyyy','nls_date_language = INDONESIAN') as tgl, " +
                       " (select  'Tensi : ' || blood_press || ', Nadi : ' || pulse ||  " +
                       " ', Suhu : ' || temperature || ', BB : ' || bb || ', TB : ' || tb || ', Alergi : ' || allergy || ', Keluhan : ' || anamnesa as anamnesa   " +
                       " from cs_anamnesa " +
                       " where rm_no=c.rm_no " +
                       " and insp_date=trunc(b.visit_date)  " +
                       " and visit_no=b.que01) anamnesa,  " +
                       " (select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa  " +
                       " from cs_diagnosa a  " +
                       " join cs_diagnosa_item b on (a.item_cd=b.item_cd)  " +
                       " where b.status='A'  " +
                       " and rm_no=c.rm_no  " +
                       " and insp_date=trunc(b.visit_date) " +
                       " and visit_no=b.que01) as diagnosa, " +
                       " act_name, act_remark, act_type " +
                       " from cs_employees a  " +
                       " join cs_visit b on (a.empid = b.empid)  " +
                       " join cs_patient c on(b.empid = c.empid)  " +
                       " left join cs_action d on (c.rm_no = d.rm_no and trunc(visit_date)=d.insp_date and que01=d.visit_no)  " +
                       " where  to_char(b.visit_date, 'yyyy-mm-dd') = '" + p_date + "'  " +
                       " and c.status = 'A'  " +
                       " and b.que01 = '" + p_que + "'  " +
                       " and c.group_patient = 'COMM'  " +
                       " and c.rm_no = '" + p_rm + "' ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_load, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            dsAction.Tables.Clear();
            dsAction.Tables.Add(dt);

            ReportAction report = new ReportAction(dsAction);
            report.ShowPreviewDialog();
        }

        private void gridView2_RowClick(object sender, RowClickEventArgs e)
        {
            btnPreview.Enabled = true;
        }

        private void gridView3_RowClick(object sender, RowClickEventArgs e)
        {
            btnPreview.Enabled = true;
        }

        private void gridView4_RowClick(object sender, RowClickEventArgs e)
        {
            btnPreview.Enabled = true;
        }

        private void gridView5_RowClick(object sender, RowClickEventArgs e)
        {
            btnPreview.Enabled = true;
        }

        private void gridView6_RowClick(object sender, RowClickEventArgs e)
        {
            btnPreview.Enabled = true;
        }

        private void gridView7_RowClick(object sender, RowClickEventArgs e)
        {
            btnPreview.Enabled = true;
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

    }
}