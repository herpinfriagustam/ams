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
using DevExpress.XtraCharts;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using System.Drawing.Drawing2D;

namespace Clinic
{
    public partial class DashboardMonthly : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Status> listPIC = new List<Status>();
        List<string> listCol = new List<string>();
        DataSet dsSkd = new DataSet();
        DataSet dsSkdKk = new DataSet();
        DataSet dsSkdRujuk = new DataSet();
        DataSet dsSkdRekom = new DataSet();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM");
        string today2 = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string type = "";

        public DashboardMonthly()
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
            if (xtraTabControl1.SelectedTabPage.Text == "Summary")
            {
                //LoadDataSum();
                LoadDataTotEmp();
                LoadDataByPoli();
                LoadDataPregVisit();
                LoadDataJoinKb();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Trend Visit")
            {
                LoadDataTrendVisitDaily();
                LoadDataVisitMonthly();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Top 10 Disease")
            {
                LoadDataTopTenDisease();
                LoadDataTopTenDiseaseLetter();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Top 10 Disease Detail")
            {
                LoadDataTopTenDiseaseDet();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Pregnant And KB")
            {
                LoadDataPregnant();
                LoadDataKB();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Visit By Dept")
            {
                LoadDataVisitByDept();
                //LoadDataVisitByDept2();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Disease By Dept")
            {
                LoadDataDiseaseByDept();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Time Visit")
            {
                LoadDataAVGDailyTime();
                LoadDataAvgMonthlyTime();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Group Disease")
            {
                LoadDataGroupDisease1();
                LoadDataGroupDisease2();
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Disease Detail")
            {
                LoadDataDiseaseDatail();
                //LoadDataDiseaseDatail2();
            }
        }

        private void InitData()
        {
            cmbFilter.Items.Clear();
            cmbFilter.Items.Add("Jumlah");
            cmbFilter.Items.Add("% Kunjungan");
            cmbFilter.Items.Add("% Karyawan");
            cmbFilter.SelectedIndex = 0;

            dStartDt.Text = today;

            string sql_date = "", sdate="", edate="";
            sql_date = " select to_char(trunc(sysdate,'MM'),'yyyy-mm-dd') sdate, to_char(last_day(sysdate),'yyyy-mm-dd') edate from dual ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_date, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            sdate = dt.Rows[0]["sdate"].ToString();
            edate = dt.Rows[0]["edate"].ToString();

            dBgn.Text = sdate;
            dEnd.Text = edate;

            //panel6.Paint += new PaintEventHandler(panel6_Paint);
            //panel6.Refresh();

        }

        private void LoadDataSum()
        {
            string SQL = "", s_tot_emp = "", s_tot_preg = "", s_tot_kb = "", s_tot_wa = "";
            string s_tot_visit = "", s_tot_doc = "", s_tot_mid = "", s_tot_wt1 = "", s_tot_wt2 = "";
            string s_prog_visit = "", s_prog_doc = "", s_prog_mid = "";
            string s_mon_visit = "", s_new_preg = "", s_new_kb = "", s_new_wa = "";

            SQL = SQL + Environment.NewLine + "select to_char(TTIT.CS_MON_VISIT(to_char(sysdate,'yyyy-mm')),'fm999,999') mon_visit, ";
            SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_TOT_EMP('LOCAL'),'fm999,999') tot_emp, ";
            SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_NEW_STATUS('PREG'),'fm999,999') new_preg, ";
            SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_PREG_STATUS2(to_char(sysdate,'yyyy-mm-dd')),'fm999,999') tot_preg, ";
            SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_NEW_STATUS('FAMP'),'fm999,999') new_famp, ";
            SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_TOT_PATIENT('FAMP'),'fm999,999') tot_famp, ";
            SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_DAILY_WA(to_char(sysdate,'yyyy-mm-dd')),'fm999,999') new_wa, ";
            SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_TOT_WA('2020-09'),'fm999,999') tot_wa, ";
            SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_PROG_VISIT(to_char(sysdate,'yyyy-mm-dd'),''),'fm999,999') prog_visit, ";
            SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_TOT_VISIT(to_char(sysdate,'yyyy-mm-dd'),''),'fm999,999') tot_visit, ";
            SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_PROG_VISIT(to_char(sysdate,'yyyy-mm-dd'),'DOC'),'fm999,999') prog_doc, ";
            SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_TOT_VISIT(to_char(sysdate,'yyyy-mm-dd'),'DOC'),'fm999,999') tot_doc, ";
            SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_PROG_VISIT(to_char(sysdate,'yyyy-mm-dd'),'MID'),'fm999,999') prog_mid, ";
            SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_TOT_VISIT(to_char(sysdate,'yyyy-mm-dd'),'MID'),'fm999,999') tot_mid, ";
            SQL = SQL + Environment.NewLine + "round(avg(nvl((a.rsv-nvl(a.hold,0)) + a.ins + a.med,0)),2) as doc_avg,  ";
            SQL = SQL + Environment.NewLine + "round(avg(nvl((b.rsv-nvl(b.hold,0)) + b.ins + b.med,0)),2) as mid_avg ";
            SQL = SQL + Environment.NewLine + "from (   ";
            SQL = SQL + Environment.NewLine + "select a.empid, name, dept, type_patient, work_accident, purpose,   ";
            SQL = SQL + Environment.NewLine + "que01, to_char(visit_date,'yyyy-mm-dd') visit_date,  to_char(visit_date,'dd-Dy') info_date,  ";
            SQL = SQL + Environment.NewLine + "to_char(visit_date,'hh24:mi:ss') visit_time,   ";
            SQL = SQL + Environment.NewLine + "to_char(time_reservation,'hh24:mi:ss') reservation_time,   ";
            SQL = SQL + Environment.NewLine + "to_char(time_inspection,'hh24:mi:ss') inspection_time,   ";
            SQL = SQL + Environment.NewLine + "to_char(decode(observation,'Y',time_receipt,time_end),'hh24:mi:ss') end_time,   ";
            SQL = SQL + Environment.NewLine + "round((time_reservation-visit_date) * 24 * 60) rsv,   ";
            SQL = SQL + Environment.NewLine + "round((time_inspection-time_reservation) * 24 * 60) ins,   ";
            SQL = SQL + Environment.NewLine + "round((time_receipt-time_inspection) * 24 * 60) med,  ";
            SQL = SQL + Environment.NewLine + "round((end_hold-start_hold) * 24 * 60) hold, a.ins_date   ";
            SQL = SQL + Environment.NewLine + "from cs_visit a     ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)     ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (b.empid=c.empid)   ";
            SQL = SQL + Environment.NewLine + "join cs_anamnesa d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no)   ";
            SQL = SQL + Environment.NewLine + "where 1=1    ";
            SQL = SQL + Environment.NewLine + "and to_char(visit_date,'yyyy-mm-dd') = to_char(sysdate,'yyyy-mm-dd')  ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS'  ";
            SQL = SQL + Environment.NewLine + "and a.purpose='DOC') a left join  ";
            SQL = SQL + Environment.NewLine + "(   ";
            SQL = SQL + Environment.NewLine + "select a.empid, name, dept, type_patient, work_accident, purpose,   ";
            SQL = SQL + Environment.NewLine + "que01, to_char(visit_date,'yyyy-mm-dd') visit_date, to_char(visit_date,'dd-Dy') info_date,   ";
            SQL = SQL + Environment.NewLine + "to_char(visit_date,'hh24:mi:ss') visit_time,   ";
            SQL = SQL + Environment.NewLine + "to_char(time_reservation,'hh24:mi:ss') reservation_time,   ";
            SQL = SQL + Environment.NewLine + "to_char(time_inspection,'hh24:mi:ss') inspection_time,   ";
            SQL = SQL + Environment.NewLine + "to_char(decode(observation,'Y',time_receipt,time_end),'hh24:mi:ss') end_time,   ";
            SQL = SQL + Environment.NewLine + "round((time_reservation-visit_date) * 24 * 60) rsv,   ";
            SQL = SQL + Environment.NewLine + "round((time_inspection-time_reservation) * 24 * 60) ins,   ";
            SQL = SQL + Environment.NewLine + "round((time_receipt-time_inspection) * 24 * 60) med,  ";
            SQL = SQL + Environment.NewLine + "round((end_hold-start_hold) * 24 * 60) hold, a.ins_date   ";
            SQL = SQL + Environment.NewLine + "from cs_visit a     ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)     ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (b.empid=c.empid)   ";
            SQL = SQL + Environment.NewLine + "join cs_anamnesa d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no)   ";
            SQL = SQL + Environment.NewLine + "where 1=1    ";
            SQL = SQL + Environment.NewLine + "and to_char(visit_date,'yyyy-mm-dd') = to_char(sysdate,'yyyy-mm-dd')  ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS'  ";
            SQL = SQL + Environment.NewLine + "and a.purpose='MID') b on a.visit_date=b.visit_date  ";
            SQL = SQL + Environment.NewLine + "where 1=1   ";
            SQL = SQL + Environment.NewLine + "group by a.visit_date, a.info_date  ";
            SQL = SQL + Environment.NewLine + "order by a.visit_date  ";


            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            s_mon_visit = dt.Rows[0]["mon_visit"].ToString();
            s_tot_emp = dt.Rows[0]["tot_emp"].ToString();
            s_new_preg = dt.Rows[0]["new_preg"].ToString();
            s_tot_preg = dt.Rows[0]["tot_preg"].ToString();
            s_new_kb = dt.Rows[0]["new_famp"].ToString();
            s_tot_kb = dt.Rows[0]["tot_famp"].ToString();
            s_new_wa = dt.Rows[0]["new_wa"].ToString();
            s_tot_wa = dt.Rows[0]["tot_wa"].ToString();

            s_prog_visit = dt.Rows[0]["prog_visit"].ToString();
            s_tot_visit = dt.Rows[0]["tot_visit"].ToString();
            s_prog_doc = dt.Rows[0]["prog_doc"].ToString();
            s_tot_doc = dt.Rows[0]["tot_doc"].ToString();
            s_prog_mid = dt.Rows[0]["prog_mid"].ToString();
            s_tot_mid = dt.Rows[0]["tot_mid"].ToString();
            s_tot_wt1 = dt.Rows[0]["doc_avg"].ToString();
            s_tot_wt2 = dt.Rows[0]["mid_avg"].ToString();


            lMonVisit.Text = s_mon_visit;
            lTotEmp.Text = s_tot_emp;
            lNewPreg.Text = s_new_preg;
            lTotPreg.Text = s_tot_preg;
            lNewKb.Text = s_new_kb;
            lTotKb.Text = s_tot_kb;
            lNewWa.Text = s_new_wa;
            lTotWa.Text = s_tot_wa;

            lProgVisit.Text = s_prog_visit;
            lTotVisit.Text = s_tot_visit;
            lProgDoc.Text = s_prog_doc;
            lTotDoc.Text = s_tot_doc;
            lProgMid.Text = s_prog_mid;
            lTotMid.Text = s_tot_mid;
            lTotWait.Text = s_tot_wt1;
            lTotWait2.Text = s_tot_wt2;

        }
        private void LoadDataTotEmp()
        {
            string SQL = "" ;


            SQL = "";
            SQL = SQL + Environment.NewLine + "select gender, count(0) cnt  ";
            SQL = SQL + Environment.NewLine + "from cs_patient_info ";
            SQL = SQL + Environment.NewLine + "where status = 'A' ";
            SQL = SQL + Environment.NewLine + "group by gender order by gender asc";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                // Create a doughnut series. 
                Series series1 = new Series("Series 1", ViewType.Doughnut);

                chartControl1.Series.Clear();
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    series1.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[1]) }));
                }

                // Add the series to the chart. 
                chartControl1.Series.Add(series1);

                // Specify the text pattern of series labels. 
                series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series1.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Specify how series points are sorted. 
                series1.SeriesPointsSorting = SortingMode.Ascending;
                series1.SeriesPointsSortingKey = SeriesPointKey.Argument;


                // Specify the behavior of series labels. 
                ((DoughnutSeriesLabel)series1.Label).Position = PieSeriesLabelPosition.TwoColumns;
                ((DoughnutSeriesLabel)series1.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
                ((DoughnutSeriesLabel)series1.Label).ResolveOverlappingMinIndent = 5;

                // Adjust the view-type-specific options of the series. 
                //((DoughnutSeriesView)series1.View).ExplodedPoints.Add(series1.Points[0]);
                ((DoughnutSeriesView)series1.View).ExplodedDistancePercentage = 30;

                // Access the diagram's options. 
                ((SimpleDiagram)chartControl1.Diagram).Dimension = 2;

                // Add a title to the chart and hide the legend. 
                ChartTitle chartTitle1 = new ChartTitle();
                chartTitle1.Text = "Total Employees";

                chartControl1.Titles.Clear();
                chartControl1.Titles.Add(chartTitle1);
                chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

                // Add the chart to the form. 
                chartControl1.Dock = DockStyle.Fill;
                tableLayoutPanel2.Controls.Add(chartControl1);

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();
            
        }

        private void LoadDataByPoli()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select poli_name, count(0) cnt  ";
            SQL = SQL + Environment.NewLine + "from cs_visit a ";
            SQL = SQL + Environment.NewLine + "join cs_policlinic b on (a.poli_cd=b.poli_cd) ";
            SQL = SQL + Environment.NewLine + "where b.status='A' ";
            SQL = SQL + Environment.NewLine + "and to_char(visit_date,'yyyy-mm')='" + dStartDt.Text + "' ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS' ";
            SQL = SQL + Environment.NewLine + "group by poli_name ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                // Create a doughnut series. 
                Series series1 = new Series("Series 1", ViewType.Doughnut);

                chartControl2.Series.Clear();
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    series1.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[1]) }));
                }

                // Add the series to the chart. 
                chartControl2.Series.Add(series1);

                // Specify the text pattern of series labels. 
                series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series1.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Specify how series points are sorted. 
                series1.SeriesPointsSorting = SortingMode.Ascending;
                series1.SeriesPointsSortingKey = SeriesPointKey.Argument;


                // Specify the behavior of series labels. 
                ((DoughnutSeriesLabel)series1.Label).Position = PieSeriesLabelPosition.TwoColumns;
                ((DoughnutSeriesLabel)series1.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
                ((DoughnutSeriesLabel)series1.Label).ResolveOverlappingMinIndent = 5;

                // Adjust the view-type-specific options of the series. 
                //((DoughnutSeriesView)series1.View).ExplodedPoints.Add(series1.Points[0]);
                ((DoughnutSeriesView)series1.View).ExplodedDistancePercentage = 30;

                // Access the diagram's options. 
                ((SimpleDiagram)chartControl2.Diagram).Dimension = 2;

                // Add a title to the chart and hide the legend. 
                ChartTitle chartTitle1 = new ChartTitle();
                chartTitle1.Text = "Visit By Poli";

                chartControl2.Titles.Clear();
                chartControl2.Titles.Add(chartTitle1);
                chartControl2.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

                // Add the chart to the form. 
                chartControl2.Dock = DockStyle.Fill;
                tableLayoutPanel2.Controls.Add(chartControl2);

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();

        }

        private void LoadDataPregVisit()
        {
            string SQL = "", p_visit = "", p_nonvisit = "";

            SQL = "";
            //SQL = SQL + Environment.NewLine + "select cnt visit, (TTIT.CS_TOT_PATIENT('PREG') - cnt) non_visit ";
            SQL = SQL + Environment.NewLine + "select cnt visit, (TTIT.CS_PREG_STATUS('" + dStartDt.Text + "') - cnt) non_visit ";
            SQL = SQL + Environment.NewLine + "from ( ";
            SQL = SQL + Environment.NewLine + "SELECT COUNT (0) cnt ";
            SQL = SQL + Environment.NewLine + "        FROM cs_visit a ";
            SQL = SQL + Environment.NewLine + "       WHERE a.status = 'CLS' ";
            SQL = SQL + Environment.NewLine + "         AND poli_cd = 'POL0002' ";
            SQL = SQL + Environment.NewLine + "         AND TO_CHAR (visit_date, 'yyyy-mm') = '" + dStartDt.Text + "') ";


            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                // Create a doughnut series. 
                Series series1 = new Series("Series 1", ViewType.Doughnut);

                chartControl3.Series.Clear();
                //foreach (DataRow row in dt.Rows) // Loop over the rows.
                //{
                //    series1.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[1]) }));
                //}

                if (dt.Rows.Count > 0)
                {
                    p_visit = dt.Rows[0]["visit"].ToString();
                    p_nonvisit = dt.Rows[0]["non_visit"].ToString();
                }

                series1.Points.Add(new SeriesPoint("Pregant Visit", Convert.ToDouble(p_visit)));
                series1.Points.Add(new SeriesPoint("Pregant Not Visit", Convert.ToDouble(p_nonvisit)));

                // Add the series to the chart. 
                chartControl3.Series.Add(series1);

                // Specify the text pattern of series labels. 
                series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series1.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Specify how series points are sorted. 
                series1.SeriesPointsSorting = SortingMode.Ascending;
                series1.SeriesPointsSortingKey = SeriesPointKey.Argument;


                // Specify the behavior of series labels. 
                ((DoughnutSeriesLabel)series1.Label).Position = PieSeriesLabelPosition.TwoColumns;
                ((DoughnutSeriesLabel)series1.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
                ((DoughnutSeriesLabel)series1.Label).ResolveOverlappingMinIndent = 5;

                // Adjust the view-type-specific options of the series. 
                //((DoughnutSeriesView)series1.View).ExplodedPoints.Add(series1.Points[0]);
                ((DoughnutSeriesView)series1.View).ExplodedDistancePercentage = 30;

                // Access the diagram's options. 
                ((SimpleDiagram)chartControl3.Diagram).Dimension = 2;

                // Add a title to the chart and hide the legend. 
                ChartTitle chartTitle1 = new ChartTitle();
                chartTitle1.Text = "Pregnant Visit";

                chartControl3.Titles.Clear();
                chartControl3.Titles.Add(chartTitle1);
                chartControl3.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

                // Add the chart to the form. 
                chartControl3.Dock = DockStyle.Fill;
                tableLayoutPanel2.Controls.Add(chartControl3);

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();

        }

        private void LoadDataJoinKb()
        {
            string SQL = "", p_join = "", p_nonjoin = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select TTIT.CS_TOT_PATIENT('FAMP') kb, ";
            SQL = SQL + Environment.NewLine + "       cnt - TTIT.CS_TOT_PATIENT('FAMP') non_kb ";
            SQL = SQL + Environment.NewLine + "from ( ";
            SQL = SQL + Environment.NewLine + "select count(0) cnt ";
            SQL = SQL + Environment.NewLine + "  from cs_patient_info ";
            SQL = SQL + Environment.NewLine + " where status='A' ";
            SQL = SQL + Environment.NewLine + "   and gender = 'P' ) ";


            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                // Create a doughnut series. 
                Series series1 = new Series("Series 1", ViewType.Doughnut);

                chartControl4.Series.Clear();
                //foreach (DataRow row in dt.Rows) // Loop over the rows.
                //{
                //    series1.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[1]) }));
                //}

                if (dt.Rows.Count > 0)
                {
                    p_join = dt.Rows[0]["kb"].ToString();
                    p_nonjoin = dt.Rows[0]["non_kb"].ToString();
                }

                series1.Points.Add(new SeriesPoint("Join KB", Convert.ToDouble(p_join)));
                series1.Points.Add(new SeriesPoint("Not Join KB", Convert.ToDouble(p_nonjoin)));

                // Add the series to the chart. 
                chartControl4.Series.Add(series1);

                // Specify the text pattern of series labels. 
                series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series1.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Specify how series points are sorted. 
                series1.SeriesPointsSorting = SortingMode.Ascending;
                series1.SeriesPointsSortingKey = SeriesPointKey.Argument;


                // Specify the behavior of series labels. 
                ((DoughnutSeriesLabel)series1.Label).Position = PieSeriesLabelPosition.TwoColumns;
                ((DoughnutSeriesLabel)series1.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
                ((DoughnutSeriesLabel)series1.Label).ResolveOverlappingMinIndent = 5;

                // Adjust the view-type-specific options of the series. 
                //((DoughnutSeriesView)series1.View).ExplodedPoints.Add(series1.Points[0]);
                ((DoughnutSeriesView)series1.View).ExplodedDistancePercentage = 30;

                // Access the diagram's options. 
                ((SimpleDiagram)chartControl4.Diagram).Dimension = 2;

                // Add a title to the chart and hide the legend. 
                ChartTitle chartTitle1 = new ChartTitle();
                chartTitle1.Text = "Join KB";

                chartControl4.Titles.Clear();
                chartControl4.Titles.Add(chartTitle1);
                chartControl4.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

                // Add the chart to the form. 
                chartControl4.Dock = DockStyle.Fill;
                tableLayoutPanel2.Controls.Add(chartControl4);

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();

        }

        private void LoadDataTrendVisitDaily()
        {
            string SQL = "", p_join = "", p_nonjoin = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select to_char(to_date(ddate,'yyyy-mm-dd'),'dd-Dy') ddate, cnt, ";
            SQL = SQL + Environment.NewLine + "(select count(0) cnt_umum ";
            SQL = SQL + Environment.NewLine + "from cs_visit ";
            SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm-dd')=ddate ";
            SQL = SQL + Environment.NewLine + "and status='CLS' ";
            SQL = SQL + Environment.NewLine + "and poli_cd in ('POL0000','POL0001')) cnt_umum, ";
            SQL = SQL + Environment.NewLine + "(select count(0) cnt_hamil ";
            SQL = SQL + Environment.NewLine + "from cs_visit ";
            SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm-dd')=ddate ";
            SQL = SQL + Environment.NewLine + "and status='CLS' ";
            SQL = SQL + Environment.NewLine + "and poli_cd in ('POL0002')) cnt_hamil, ";
            SQL = SQL + Environment.NewLine + "(select count(0) cnt_kb ";
            SQL = SQL + Environment.NewLine + "from cs_visit ";
            SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm-dd')=ddate ";
            SQL = SQL + Environment.NewLine + "and status='CLS' ";
            SQL = SQL + Environment.NewLine + "and poli_cd in ('POL0003')) cnt_kb ";
            SQL = SQL + Environment.NewLine + "from ( ";
            SQL = SQL + Environment.NewLine + "select to_char(visit_date,'yyyy-mm-dd') ddate, count(0) cnt ";
            SQL = SQL + Environment.NewLine + "from cs_visit ";
            SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm')='" + dStartDt.Text + "' ";
            SQL = SQL + Environment.NewLine + "and status='CLS' ";
            SQL = SQL + Environment.NewLine + "group by  to_char(visit_date,'yyyy-mm-dd') ";
            SQL = SQL + Environment.NewLine + ") where 1=1 order by ddate asc ";



            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                // Create a line series. 
                Series series1 = new Series("Total Visit", ViewType.Line);

                chartControl5.Series.Clear();
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    series1.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[1]) }));
                }

                // Add the series to the chart. 
                chartControl5.Series.Add(series1);

                // Set the numerical argument scale types for the series, 
                // as it is qualitative, by default. 
                series1.ArgumentScaleType = ScaleType.Qualitative;
                series1.ValueScaleType = ScaleType.Numerical;
                series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

                // Access the view-type-specific options of the series. 
                ((LineSeriesView)series1.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                ((LineSeriesView)series1.View).LineMarkerOptions.Kind = MarkerKind.Circle;
                //((LineSeriesView)series1.View).LineStyle.DashStyle = DashStyle.Dash;

                // Access the type-specific options of the diagram. 
                ((XYDiagram)chartControl5.Diagram).EnableAxisXZooming = true;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series1.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);


                // Create a line series. 
                Series series2 = new Series("Poli Umum", ViewType.Line);

                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    series2.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[2]) }));
                }

                // Add the series to the chart. 
                chartControl5.Series.Add(series2);

                // Set the numerical argument scale types for the series, 
                // as it is qualitative, by default. 
                series2.ArgumentScaleType = ScaleType.Qualitative;
                series2.ValueScaleType = ScaleType.Numerical;
                //series2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

                // Access the view-type-specific options of the series. 
                ((LineSeriesView)series2.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                ((LineSeriesView)series2.View).LineMarkerOptions.Kind = MarkerKind.Circle;
                //((LineSeriesView)series2.View).LineStyle.DashStyle = DashStyle.Dash;

                // Access the type-specific options of the diagram. 
                ((XYDiagram)chartControl5.Diagram).EnableAxisXZooming = true;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series2.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);


                // Create a line series. 
                Series series3 = new Series("Poli Ibu Hamil", ViewType.Line);

                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    series3.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[3]) }));
                }

                // Add the series to the chart. 
                chartControl5.Series.Add(series3);

                // Set the numerical argument scale types for the series, 
                // as it is qualitative, by default. 
                series3.ArgumentScaleType = ScaleType.Qualitative;
                series3.ValueScaleType = ScaleType.Numerical;
                //series3.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

                // Access the view-type-specific options of the series. 
                ((LineSeriesView)series3.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                ((LineSeriesView)series3.View).LineMarkerOptions.Kind = MarkerKind.Circle;
                //((LineSeriesView)series2.View).LineStyle.DashStyle = DashStyle.Dash;

                // Access the type-specific options of the diagram. 
                ((XYDiagram)chartControl5.Diagram).EnableAxisXZooming = true;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series3.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Create a line series. 
                Series series4 = new Series("Poli KB", ViewType.Line);

                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    series4.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[4]) }));
                }

                // Add the series to the chart. 
                chartControl5.Series.Add(series4);

                // Set the numerical argument scale types for the series, 
                // as it is qualitative, by default. 
                series4.ArgumentScaleType = ScaleType.Qualitative;
                series4.ValueScaleType = ScaleType.Numerical;
                //series4.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

                // Access the view-type-specific options of the series. 
                ((LineSeriesView)series4.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                ((LineSeriesView)series4.View).LineMarkerOptions.Kind = MarkerKind.Circle;
                //((LineSeriesView)series2.View).LineStyle.DashStyle = DashStyle.Dash;

                // Access the type-specific options of the diagram. 
                ((XYDiagram)chartControl5.Diagram).EnableAxisXZooming = true;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series4.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Specify how series points are sorted. 
                //series1.SeriesPointsSorting = SortingMode.Ascending;
                //series1.SeriesPointsSortingKey = SeriesPointKey.Argument;

                // Add a title to the chart and hide the legend. 
                ChartTitle chartTitle1 = new ChartTitle();
                chartTitle1.Text = "Daily Visit";

                chartControl5.Titles.Clear();
                chartControl5.Titles.Add(chartTitle1);
                chartControl5.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                // Add the chart to the form. 
                chartControl5.Dock = DockStyle.Fill;
                tableLayoutPanel3.Controls.Add(chartControl5);

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();

        }

        private void LoadDataVisitMonthly()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select to_char(visit_date,'yyyy-mm') ddate, count(0) cnt ";
            SQL = SQL + Environment.NewLine + "from cs_visit ";
            SQL = SQL + Environment.NewLine + "where trunc(visit_date) between trunc(add_months(sysdate,-12))  ";
            SQL = SQL + Environment.NewLine + "and last_day(to_date('" + dStartDt.Text + "','yyyy-mm')) ";
            SQL = SQL + Environment.NewLine + "and status='CLS' ";
            SQL = SQL + Environment.NewLine + "group by  to_char(visit_date,'yyyy-mm')  ";
            SQL = SQL + Environment.NewLine + "order by 1 asc ";


            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                // Create a doughnut series. 
                Series series1 = new Series("Series 1", ViewType.Bar);

                chartControl6.Series.Clear();
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    series1.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[1]) }));
                }

                // Add the series to the chart. 
                chartControl6.Series.Add(series1);
                series1.ArgumentScaleType = ScaleType.Qualitative;
                series1.ValueScaleType = ScaleType.Numerical;

                series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                series1.Label.BackColor = Color.White;

                ((BarSeriesLabel)series1.Label).Position = BarSeriesLabelPosition.Top;
                //series1.Label.Font = new Font("Tahoma", 10, FontStyle.Bold);
                series1.Label.TextOrientation = TextOrientation.Horizontal;

                // Hide the legend (if necessary). 
                chartControl6.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series1.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Specify how series points are sorted. 
                //series1.SeriesPointsSorting = SortingMode.Ascending;
                //series1.SeriesPointsSortingKey = SeriesPointKey.Argument;

                // Add a title to the chart and hide the legend. 
                ChartTitle chartTitle1 = new ChartTitle();
                chartTitle1.Text = "Monthly Visit";

                chartControl6.Titles.Clear();
                chartControl6.Titles.Add(chartTitle1);
                chartControl6.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

                // Add the chart to the form. 
                chartControl6.Dock = DockStyle.Fill;
                tableLayoutPanel3.Controls.Add(chartControl6);

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();

        }

        private void LoadDataTopTenDisease()
        {
            string SQL = "", SQL2 = "", SQL3 = "", load_sql = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select initcap(item_name) diagnosa, cnt from ( ";
            SQL = SQL + Environment.NewLine + "select c.item_cd, count(distinct a.empid) cnt from cs_visit a  ";
            SQL = SQL + Environment.NewLine + "join cs_patient b on (a.empid=b.empid) ";
            SQL = SQL + Environment.NewLine + "join cs_diagnosa c on (trunc(a.visit_date)=c.insp_date and b.rm_no=c.rm_no and a.que01=c.visit_no) ";
            SQL = SQL + Environment.NewLine + "where b.status='A'  ";
            SQL = SQL + Environment.NewLine + "and trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd') ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS' ";
            SQL = SQL + Environment.NewLine + "and poli_cd in ('POL0000','POL0001') ";
            SQL = SQL + Environment.NewLine + "and c.item_cd not in ('X11') ";
            SQL = SQL + Environment.NewLine + "and type_diagnosa='P' ";
            SQL = SQL + Environment.NewLine + "group by c.item_cd ";
            SQL = SQL + Environment.NewLine + "order by 2 desc) aa ";
            SQL = SQL + Environment.NewLine + "join cs_diagnosa_item bb on (aa.item_cd=bb.item_cd) ";
            SQL = SQL + Environment.NewLine + "where bb.status='A' ";
            SQL = SQL + Environment.NewLine + "and rownum<=10 ";

            SQL2 = "";
            SQL2 = SQL2 + Environment.NewLine + "select diagnosa, round(cnt/greatest(cnt_visit,cnt)*100,3) as cnt from ( ";
            SQL2 = SQL2 + Environment.NewLine + "select initcap(item_name) diagnosa, cnt, ";
            SQL2 = SQL2 + Environment.NewLine + "(select count(0) from cs_visit  ";
            SQL2 = SQL2 + Environment.NewLine + "where status='CLS'  ";
            SQL2 = SQL2 + Environment.NewLine + "and trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd')";
            SQL2 = SQL2 + Environment.NewLine + "and poli_cd in ('POL0000','POL0001') ) cnt_visit ";
            SQL2 = SQL2 + Environment.NewLine + "from (  ";
            SQL2 = SQL2 + Environment.NewLine + "select c.item_cd, count( distinct a.empid) cnt from cs_visit a   ";
            SQL2 = SQL2 + Environment.NewLine + "join cs_patient b on (a.empid=b.empid)  ";
            SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa c on (trunc(a.visit_date)=c.insp_date and b.rm_no=c.rm_no and a.que01=c.visit_no)  ";
            SQL2 = SQL2 + Environment.NewLine + "where b.status='A'   ";
            SQL2 = SQL2 + Environment.NewLine + "and trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd') ";
            SQL2 = SQL2 + Environment.NewLine + "and a.status='CLS'  ";
            SQL2 = SQL2 + Environment.NewLine + "and poli_cd in ('POL0000','POL0001')  ";
            SQL2 = SQL2 + Environment.NewLine + "and c.item_cd not in ('X11')  ";
            SQL2 = SQL2 + Environment.NewLine + "and type_diagnosa='P'  ";
            SQL2 = SQL2 + Environment.NewLine + "group by c.item_cd  ";
            SQL2 = SQL2 + Environment.NewLine + "order by 2 desc) aa  ";
            SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa_item bb on (aa.item_cd=bb.item_cd)  ";
            SQL2 = SQL2 + Environment.NewLine + "where bb.status='A' ) ";
            SQL2 = SQL2 + Environment.NewLine + "where 1=1 ";
            SQL2 = SQL2 + Environment.NewLine + "and rownum<=10  ";

            SQL3 = "";
            SQL3 = SQL3 + Environment.NewLine + "select diagnosa, round(cnt/greatest(cnt_visit,cnt)*100,3) as cnt from ( ";
            SQL3 = SQL3 + Environment.NewLine + "select initcap(item_name) diagnosa, cnt, ";
            SQL3 = SQL3 + Environment.NewLine + "(select count(0) from cs_employees ";
            SQL3 = SQL3 + Environment.NewLine + "where retire_dt is null) cnt_visit ";
            SQL3 = SQL3 + Environment.NewLine + "from (  ";
            SQL3 = SQL3 + Environment.NewLine + "select c.item_cd, count( distinct a.empid) cnt from cs_visit a   ";
            SQL3 = SQL3 + Environment.NewLine + "join cs_patient b on (a.empid=b.empid)  ";
            SQL3 = SQL3 + Environment.NewLine + "join cs_diagnosa c on (trunc(a.visit_date)=c.insp_date and b.rm_no=c.rm_no and a.que01=c.visit_no)  ";
            SQL3 = SQL3 + Environment.NewLine + "where b.status='A'   ";
            SQL3 = SQL3 + Environment.NewLine + "and trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd') ";
            SQL3 = SQL3 + Environment.NewLine + "and a.status='CLS'  ";
            SQL3 = SQL3 + Environment.NewLine + "and poli_cd in ('POL0000','POL0001')  ";
            SQL3 = SQL3 + Environment.NewLine + "and c.item_cd not in ('X11')  ";
            SQL3 = SQL3 + Environment.NewLine + "and type_diagnosa='P'  ";
            SQL3 = SQL3 + Environment.NewLine + "group by c.item_cd  ";
            SQL3 = SQL3 + Environment.NewLine + "order by 2 desc) aa  ";
            SQL3 = SQL3 + Environment.NewLine + "join cs_diagnosa_item bb on (aa.item_cd=bb.item_cd)  ";
            SQL3 = SQL3 + Environment.NewLine + "where bb.status='A' ) ";
            SQL3 = SQL3 + Environment.NewLine + "where 1=1 ";
            SQL3 = SQL3 + Environment.NewLine + "and rownum<=10  ";

            if (cmbFilter.Text == "Jumlah")
            {
                load_sql = SQL;
            }
            else if (cmbFilter.Text == "% Kunjungan")
            {
                load_sql = SQL2;
            }
            else if (cmbFilter.Text == "% Karyawan")
            {
                load_sql = SQL3;
            }

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(load_sql, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                // Create a doughnut series. 
                Series series1 = new Series("Series 1", ViewType.Bar);

                chartControl7.Series.Clear();
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    series1.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[1]) }));
                }

                // Add the series to the chart. 
                chartControl7.Series.Add(series1);
                series1.ArgumentScaleType = ScaleType.Qualitative;
                series1.ValueScaleType = ScaleType.Numerical;

                series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                series1.Label.BackColor = Color.White;

                ((BarSeriesLabel)series1.Label).Position = BarSeriesLabelPosition.Top;
                //series1.Label.Font = new Font("Tahoma", 10, FontStyle.Bold);
                series1.Label.TextOrientation = TextOrientation.Horizontal;

                // Hide the legend (if necessary). 
                chartControl7.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series1.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Specify how series points are sorted. 
                //series1.SeriesPointsSorting = SortingMode.Ascending;
                //series1.SeriesPointsSortingKey = SeriesPointKey.Argument;

                // Add a title to the chart and hide the legend. 
                ChartTitle chartTitle1 = new ChartTitle();
                if (cmbFilter.Text == "Jumlah")
                {
                    chartTitle1.Text = "Top 10 Disease";
                }
                else if (cmbFilter.Text == "% Kunjungan")
                {
                    chartTitle1.Text = "Top 10 Disease By Monthly Visit (%)";
                }
                else if (cmbFilter.Text == "% Karyawan")
                {
                    chartTitle1.Text = "Top 10 Disease By All Employees (%)";
                }
                

                chartControl7.Titles.Clear();
                chartControl7.Titles.Add(chartTitle1);
                chartControl7.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

                // Add the chart to the form. 
                chartControl7.Dock = DockStyle.Fill;
                tableLayoutPanel4.Controls.Add(chartControl7);

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();
        }

        private void LoadDataTopTenDiseaseLetter()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select initcap(item_name) diagnosa, cnt from ( ";
            SQL = SQL + Environment.NewLine + "select c.item_cd, count(0) cnt from cs_visit a  ";
            SQL = SQL + Environment.NewLine + "join cs_patient b on (a.empid=b.empid) ";
            SQL = SQL + Environment.NewLine + "join cs_diagnosa c on (trunc(a.visit_date)=c.insp_date and b.rm_no=c.rm_no and a.que01=c.visit_no) ";
            SQL = SQL + Environment.NewLine + "join cs_sick_leter d on (trunc(a.visit_date)=d.insp_date and b.rm_no=d.rm_no and a.que01=d.visit_no) ";
            SQL = SQL + Environment.NewLine + "where b.status='A'  ";
            SQL = SQL + Environment.NewLine + "and to_char(visit_date,'yyyy-mm')='" + dStartDt.Text + "' ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS' ";
            SQL = SQL + Environment.NewLine + "and poli_cd in ('POL0000','POL0001') ";
            SQL = SQL + Environment.NewLine + "and c.item_cd not in ('X11') ";
            SQL = SQL + Environment.NewLine + "and type_diagnosa='P' ";
            SQL = SQL + Environment.NewLine + "group by c.item_cd ";
            SQL = SQL + Environment.NewLine + "order by 2 desc) aa ";
            SQL = SQL + Environment.NewLine + "join cs_diagnosa_item bb on (aa.item_cd=bb.item_cd) ";
            SQL = SQL + Environment.NewLine + "where rownum<=10 ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                // Create a doughnut series. 
                Series series1 = new Series("Series 1", ViewType.Bar);

                chartControl8.Series.Clear();
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    series1.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[1]) }));
                }

                // Add the series to the chart. 
                chartControl8.Series.Add(series1);
                series1.ArgumentScaleType = ScaleType.Qualitative;
                series1.ValueScaleType = ScaleType.Numerical;

                series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                series1.Label.BackColor = Color.White;

                ((BarSeriesLabel)series1.Label).Position = BarSeriesLabelPosition.Top;
                //series1.Label.Font = new Font("Tahoma", 10, FontStyle.Bold);
                series1.Label.TextOrientation = TextOrientation.Horizontal;

                // Hide the legend (if necessary). 
                chartControl8.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series1.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Specify how series points are sorted. 
                //series1.SeriesPointsSorting = SortingMode.Ascending;
                //series1.SeriesPointsSortingKey = SeriesPointKey.Argument;

                // Add a title to the chart and hide the legend. 
                ChartTitle chartTitle1 = new ChartTitle();
                chartTitle1.Text = "Top 10 Disease (Sick Letter)";

                chartControl8.Titles.Clear();
                chartControl8.Titles.Add(chartTitle1);
                chartControl8.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

                // Add the chart to the form. 
                chartControl8.Dock = DockStyle.Fill;
                tableLayoutPanel4.Controls.Add(chartControl8);

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();
        }

        private void LoadDataTopTenDiseaseDet()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select item_cd, item_name from ( ";
            SQL = SQL + Environment.NewLine + "select c.item_cd,initcap(item_name) item_name, count(0) cnt from cs_visit a  ";
            SQL = SQL + Environment.NewLine + "join cs_patient b on (a.empid=b.empid) ";
            SQL = SQL + Environment.NewLine + "join cs_diagnosa c on (trunc(a.visit_date)=c.insp_date and b.rm_no=c.rm_no and a.que01=c.visit_no) ";
            SQL = SQL + Environment.NewLine + "join cs_diagnosa_item d on (c.item_cd=d.item_cd) ";
            SQL = SQL + Environment.NewLine + "where b.status='A'  ";
            SQL = SQL + Environment.NewLine + "and d.status='A' ";
            SQL = SQL + Environment.NewLine + "and to_char(visit_date,'yyyy-mm')='" + dStartDt.Text + "' ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS' ";
            SQL = SQL + Environment.NewLine + "and poli_cd in ('POL0000','POL0001') ";
            SQL = SQL + Environment.NewLine + "and c.item_cd not in ('X11') ";
            SQL = SQL + Environment.NewLine + "and type_diagnosa='P' ";
            SQL = SQL + Environment.NewLine + "group by c.item_cd, initcap(item_name) ";
            SQL = SQL + Environment.NewLine + "order by 3 desc) aa ";
            SQL = SQL + Environment.NewLine + "where rownum<=10 ";



            loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                // Create a doughnut series. 
                //Series series1 = new Series("Series 1", ViewType.Bar);

                chartControl9.Series.Clear();
                checkedListBoxControl1.Items.Clear();

                int no = 0;
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    string SQL2 = "";

                    SQL2 = "";
                    //SQL2 = SQL2 + Environment.NewLine + "select to_char(insp_date,'yyyy-mm') ddate, count(0) cnt ";
                    //SQL2 = SQL2 + Environment.NewLine + "from cs_diagnosa a ";
                    //SQL2 = SQL2 + Environment.NewLine + "where type_diagnosa='P' ";
                    //SQL2 = SQL2 + Environment.NewLine + "and a.item_cd not in ('X11') ";
                    //SQL2 = SQL2 + Environment.NewLine + "and trunc(insp_date) between trunc(add_months(sysdate,-12))  ";
                    //SQL2 = SQL2 + Environment.NewLine + "and last_day(to_date('" + dStartDt.Text + "','yyyy-mm')) ";
                    //SQL2 = SQL2 + Environment.NewLine + "and a.item_cd='" + dt.Rows[no]["item_cd"].ToString() + "' ";
                    //SQL2 = SQL2 + Environment.NewLine + "group by to_char(insp_date,'yyyy-mm')  ";

                    if (cmbFilter.Text == "Jumlah")
                    {
                        SQL2 = SQL2 + Environment.NewLine + "select to_char(insp_date,'yyyy-mm') ddate, count(distinct a.empid) cnt ";
                        SQL2 = SQL2 + Environment.NewLine + "from cs_visit a   ";
                        SQL2 = SQL2 + Environment.NewLine + "join cs_patient b on (a.empid=b.empid)  ";
                        SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa c on (trunc(a.visit_date)=c.insp_date and b.rm_no=c.rm_no and a.que01=c.visit_no)  ";
                        SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa_item d on (c.item_cd=d.item_cd)  ";
                        SQL2 = SQL2 + Environment.NewLine + "where type_diagnosa='P' ";
                        SQL2 = SQL2 + Environment.NewLine + "and b.status='A' ";
                        SQL2 = SQL2 + Environment.NewLine + "and d.status='A' ";
                        SQL2 = SQL2 + Environment.NewLine + "and a.status='CLS' ";
                        SQL2 = SQL2 + Environment.NewLine + "and poli_cd in ('POL0000','POL0001') ";
                        SQL2 = SQL2 + Environment.NewLine + "and c.item_cd not in ('X11') ";
                        SQL2 = SQL2 + Environment.NewLine + "and trunc(insp_date) between trunc(add_months(sysdate,-12))  ";
                        SQL2 = SQL2 + Environment.NewLine + "and last_day(to_date('" + dStartDt.Text + "','yyyy-mm')) ";
                        SQL2 = SQL2 + Environment.NewLine + "and c.item_cd='" + dt.Rows[no]["item_cd"].ToString() + "' ";
                        SQL2 = SQL2 + Environment.NewLine + "group by to_char(insp_date,'yyyy-mm')  ";
                        SQL2 = SQL2 + Environment.NewLine + "order by to_char(insp_date,'yyyy-mm')  ";
                    }
                    else if (cmbFilter.Text == "% Kunjungan")
                    {
                        SQL2 = SQL2 + Environment.NewLine + "select ddate, round(cnt/greatest(cnt_visit,cnt)*100,3) cnt ";
                        SQL2 = SQL2 + Environment.NewLine + "from( ";
                        SQL2 = SQL2 + Environment.NewLine + "select to_char(insp_date,'yyyy-mm') ddate, count(distinct a.empid) cnt, ";
                        SQL2 = SQL2 + Environment.NewLine + "(select count(0) from cs_visit    ";
                        SQL2 = SQL2 + Environment.NewLine + "where status='CLS'    ";
                        SQL2 = SQL2 + Environment.NewLine + "and to_char(visit_date,'yyyy-mm') = to_char(insp_date,'yyyy-mm') ";
                        SQL2 = SQL2 + Environment.NewLine + "and poli_cd in ('POL0000','POL0001') ) cnt_visit ";
                        SQL2 = SQL2 + Environment.NewLine + "from cs_visit a    ";
                        SQL2 = SQL2 + Environment.NewLine + "join cs_patient b on (a.empid=b.empid)   ";
                        SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa c on (trunc(a.visit_date)=c.insp_date and b.rm_no=c.rm_no and a.que01=c.visit_no)   ";
                        SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa_item d on (c.item_cd=d.item_cd)   ";
                        SQL2 = SQL2 + Environment.NewLine + "where type_diagnosa='P'  ";
                        SQL2 = SQL2 + Environment.NewLine + "and b.status='A'  ";
                        SQL2 = SQL2 + Environment.NewLine + "and d.status='A'  ";
                        SQL2 = SQL2 + Environment.NewLine + "and a.status='CLS'  ";
                        SQL2 = SQL2 + Environment.NewLine + "and poli_cd in ('POL0000','POL0001')  ";
                        SQL2 = SQL2 + Environment.NewLine + "and c.item_cd not in ('X11')  ";
                        SQL2 = SQL2 + Environment.NewLine + "and trunc(insp_date) between trunc(add_months(sysdate,-12))   ";
                        SQL2 = SQL2 + Environment.NewLine + "and last_day(to_date('" + dStartDt.Text + "','yyyy-mm'))  ";
                        SQL2 = SQL2 + Environment.NewLine + "and c.item_cd='" + dt.Rows[no]["item_cd"].ToString() + "'  ";
                        SQL2 = SQL2 + Environment.NewLine + "group by to_char(insp_date,'yyyy-mm')   ";
                        SQL2 = SQL2 + Environment.NewLine + "order by to_char(insp_date,'yyyy-mm') ) z ";

                    }
                    else if (cmbFilter.Text == "% Karyawan")
                    {
                        SQL2 = SQL2 + Environment.NewLine + "select ddate, round(cnt/greatest(cnt_visit,cnt)*100,3) cnt ";
                        SQL2 = SQL2 + Environment.NewLine + "from( ";
                        SQL2 = SQL2 + Environment.NewLine + "select to_char(insp_date,'yyyy-mm') ddate, count(distinct a.empid) cnt, ";
                        SQL2 = SQL2 + Environment.NewLine + "(select count(0) from cs_employees  ";
                        SQL2 = SQL2 + Environment.NewLine + "where retire_dt is null) cnt_visit ";
                        SQL2 = SQL2 + Environment.NewLine + "from cs_visit a    ";
                        SQL2 = SQL2 + Environment.NewLine + "join cs_patient b on (a.empid=b.empid)   ";
                        SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa c on (trunc(a.visit_date)=c.insp_date and b.rm_no=c.rm_no and a.que01=c.visit_no)   ";
                        SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa_item d on (c.item_cd=d.item_cd)   ";
                        SQL2 = SQL2 + Environment.NewLine + "where type_diagnosa='P'  ";
                        SQL2 = SQL2 + Environment.NewLine + "and b.status='A'  ";
                        SQL2 = SQL2 + Environment.NewLine + "and d.status='A'  ";
                        SQL2 = SQL2 + Environment.NewLine + "and a.status='CLS'  ";
                        SQL2 = SQL2 + Environment.NewLine + "and poli_cd in ('POL0000','POL0001')  ";
                        SQL2 = SQL2 + Environment.NewLine + "and c.item_cd not in ('X11')  ";
                        SQL2 = SQL2 + Environment.NewLine + "and trunc(insp_date) between trunc(add_months(sysdate,-12))   ";
                        SQL2 = SQL2 + Environment.NewLine + "and last_day(to_date('" + dStartDt.Text + "','yyyy-mm'))  ";
                        SQL2 = SQL2 + Environment.NewLine + "and c.item_cd='" + dt.Rows[no]["item_cd"].ToString() + "'  ";
                        SQL2 = SQL2 + Environment.NewLine + "group by to_char(insp_date,'yyyy-mm')   ";
                        SQL2 = SQL2 + Environment.NewLine + "order by to_char(insp_date,'yyyy-mm') ) z ";

                    }

                    checkedListBoxControl1.Items.Add(dt.Rows[no]["item_name"].ToString());
                    

                    //chartControl9.Series.Add(new Series(dt.Rows[no]["item_name"].ToString(), ViewType.Bar));
                    chartControl9.Series.Add(new Series(dt.Rows[no]["item_name"].ToString(), ViewType.Line));

                    OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adOra2 = new OleDbDataAdapter(SQL2, oraConnect2);
                    DataTable dt2 = new DataTable();
                    adOra2.Fill(dt2);

                    int no2 = 0;
                    foreach (DataRow row2 in dt2.Rows)
                    {
                        chartControl9.Series[no].Points.Add(new SeriesPoint(dt2.Rows[no2][0].ToString(), Convert.ToDouble(dt2.Rows[no2][1].ToString())));
                        no2++;
                    }
                    chartControl9.Series[no].ArgumentScaleType = ScaleType.Qualitative;
                    chartControl9.Series[no].ValueScaleType = ScaleType.Numerical;

                    chartControl9.Series[no].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    chartControl9.Series[no].Label.BackColor = Color.White;

                    //((BarSeriesLabel)chartControl9.Series[no].Label).Position = BarSeriesLabelPosition.Top;
                    //series1.Label.Font = new Font("Tahoma", 10, FontStyle.Bold);
                    chartControl9.Series[no].Label.TextOrientation = TextOrientation.Horizontal;

                    chartControl9.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
                    chartControl9.Series[no].Label.Font = new Font(chartControl9.Series[no].Label.Font.FontFamily, 9, FontStyle.Bold);

                    checkedListBoxControl1.SetItemChecked(no, true);

                    no++;
                }

                // Add a title to the chart and hide the legend. 
                ChartTitle chartTitle1 = new ChartTitle();
                //chartTitle1.Text = "Top 10 Disease Detail";
                if (cmbFilter.Text == "Jumlah")
                {
                    chartTitle1.Text = "Top 10 Disease Detail";
                }
                else if (cmbFilter.Text == "% Kunjungan")
                {
                    chartTitle1.Text = "Top 10 Disease Detail By Monthly Visit (%)";
                }
                else if (cmbFilter.Text == "% Karyawan")
                {
                    chartTitle1.Text = "Top 10 Disease Detail By All Employees (%)";
                }

                chartControl9.Titles.Clear();
                chartControl9.Titles.Add(chartTitle1);
                chartControl9.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                // Add the chart to the form. 
                chartControl9.Dock = DockStyle.Fill;
                tableLayoutPanel17.Controls.Add(chartControl9);

                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();
        }

        private void checkedListBoxControl1_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            //MessageBox.Show("Status: " + checkedListBoxControl1.GetItemChecked(e.Index));
            if (checkedListBoxControl1.GetItemChecked(e.Index) == true)
            {
                chartControl9.Series[e.Index].Visible = true;
            }
            else
            {
                chartControl9.Series[e.Index].Visible = false;
            }
        }

        private void LoadDataPregnant()
        {
            string SQL = "", ddate="", cnt_preg="";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select to_char(visit_date,'yyyy-mm') ddate, count(0) cnt ";
            SQL = SQL + Environment.NewLine + "from cs_visit ";
            SQL = SQL + Environment.NewLine + "where trunc(visit_date) between trunc(add_months(sysdate,-12))  ";
            SQL = SQL + Environment.NewLine + "and last_day(to_date('" + dStartDt.Text + "','yyyy-mm')) ";
            SQL = SQL + Environment.NewLine + "and status='CLS' ";
            SQL = SQL + Environment.NewLine + "group by  to_char(visit_date,'yyyy-mm')  ";
            SQL = SQL + Environment.NewLine + "order by 1 asc ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                // Create a doughnut series. 
                Series series1 = new Series("Pregnant Employees", ViewType.Bar);

                chartControl10.Series.Clear();

                int no = 0;
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    string SQL2 = "";

                    SQL2 = "";
                    SQL2 = SQL2 + Environment.NewLine + "select ";
                    SQL2 = SQL2 + Environment.NewLine + "'" + dt.Rows[no]["ddate"].ToString() + "' as ddate,  ";
                    SQL2 = SQL2 + Environment.NewLine + "TTIT.CS_PREG_STATUS('" + dt.Rows[no]["ddate"].ToString() + "') cnt ";
                    SQL2 = SQL2 + Environment.NewLine + "from dual ";

                    OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adOra2 = new OleDbDataAdapter(SQL2, oraConnect2);
                    DataTable dt2 = new DataTable();
                    adOra2.Fill(dt2);

                    ddate = "";
                    cnt_preg = "";
                    if (dt2.Rows.Count > 0)
                    {
                        ddate = dt2.Rows[0]["ddate"].ToString();
                        cnt_preg = dt2.Rows[0]["cnt"].ToString();
                    }

                    series1.Points.Add(new SeriesPoint(ddate, Convert.ToDouble(cnt_preg)));

                    no++;
                }

                // Add the series to the chart. 
                chartControl10.Series.Add(series1);
                series1.ArgumentScaleType = ScaleType.Qualitative;
                series1.ValueScaleType = ScaleType.Numerical;

                series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                series1.Label.BackColor = Color.White;

                ((BarSeriesLabel)series1.Label).Position = BarSeriesLabelPosition.Top;
                //series1.Label.Font = new Font("Tahoma", 10, FontStyle.Bold);
                series1.Label.TextOrientation = TextOrientation.Horizontal;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series1.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Add a title to the chart and hide the legend. 
                ChartTitle chartTitle1 = new ChartTitle();
                chartTitle1.Text = "Pregnant Employees";

                chartControl10.Titles.Clear();
                chartControl10.Titles.Add(chartTitle1);
                chartControl10.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

                // Add the chart to the form. 
                chartControl10.Dock = DockStyle.Fill;
                tableLayoutPanel6.Controls.Add(chartControl10);

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();
        }

        private void LoadDataKB()
        {
            string SQL = "", ddate = "", cnt_preg = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select to_char(visit_date,'yyyy-mm') ddate, count(0) cnt ";
            SQL = SQL + Environment.NewLine + "from cs_visit ";
            SQL = SQL + Environment.NewLine + "where trunc(visit_date) between trunc(add_months(sysdate,-12))  ";
            SQL = SQL + Environment.NewLine + "and last_day(to_date('" + dStartDt.Text + "','yyyy-mm')) ";
            SQL = SQL + Environment.NewLine + "and status='CLS' ";
            SQL = SQL + Environment.NewLine + "group by  to_char(visit_date,'yyyy-mm')  ";
            SQL = SQL + Environment.NewLine + "order by 1 asc ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                // Create a doughnut series. 
                Series series1 = new Series("KB Employees", ViewType.Bar);

                chartControl11.Series.Clear();

                int no = 0;
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    string SQL2 = "";

                    SQL2 = "";
                    SQL2 = SQL2 + Environment.NewLine + "select ";
                    SQL2 = SQL2 + Environment.NewLine + "'" + dt.Rows[no]["ddate"].ToString() + "' as ddate,  ";
                    SQL2 = SQL2 + Environment.NewLine + "TTIT.CS_KB_STATUS('" + dt.Rows[no]["ddate"].ToString() + "') cnt ";
                    SQL2 = SQL2 + Environment.NewLine + "from dual ";

                    OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adOra2 = new OleDbDataAdapter(SQL2, oraConnect2);
                    DataTable dt2 = new DataTable();
                    adOra2.Fill(dt2);

                    ddate = "";
                    cnt_preg = "";
                    if (dt2.Rows.Count > 0)
                    {
                        ddate = dt2.Rows[0]["ddate"].ToString();
                        cnt_preg = dt2.Rows[0]["cnt"].ToString();
                    }

                    series1.Points.Add(new SeriesPoint(ddate, Convert.ToDouble(cnt_preg)));

                    no++;
                }

                // Add the series to the chart. 
                chartControl11.Series.Add(series1);
                series1.ArgumentScaleType = ScaleType.Qualitative;
                series1.ValueScaleType = ScaleType.Numerical;

                series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                series1.Label.BackColor = Color.White;

                ((BarSeriesLabel)series1.Label).Position = BarSeriesLabelPosition.Top;
                //series1.Label.Font = new Font("Tahoma", 10, FontStyle.Bold);
                series1.Label.TextOrientation = TextOrientation.Horizontal;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series1.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Add a title to the chart and hide the legend. 
                ChartTitle chartTitle1 = new ChartTitle();
                chartTitle1.Text = "KB Employees";

                chartControl11.Titles.Clear();
                chartControl11.Titles.Add(chartTitle1);
                chartControl11.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

                // Add the chart to the form. 
                chartControl11.Dock = DockStyle.Fill;
                tableLayoutPanel6.Controls.Add(chartControl11);

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();
        }

        private void LoadDataVisitByDept()
        {
            string SQL = "";

            SQL = "";
            ////SQL = SQL + Environment.NewLine + "select * from (  ";
            //SQL = SQL + Environment.NewLine + "select dept, count(0) cnt  ";
            //SQL = SQL + Environment.NewLine + "from cs_visit a  ";
            //SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid) ";
            //SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm')='" + dStartDt.Text + "' ";
            //SQL = SQL + Environment.NewLine + "and status='CLS'  ";
            //SQL = SQL + Environment.NewLine + "group by dept ";
            //SQL = SQL + Environment.NewLine + "order by 2 desc ";
            ////SQL = SQL + Environment.NewLine + "where cnt>=20 ";

            if (cmbFilter.Text == "Jumlah")
            {
                SQL = SQL + Environment.NewLine + "select rownum, dept, cnt from ( ";
                SQL = SQL + Environment.NewLine + "select LISTAGG(dept, ', ') WITHIN GROUP (ORDER BY dept ASC) dept, cnt from ( ";
                SQL = SQL + Environment.NewLine + "select dept, count(distinct a.empid) cnt  ";
                SQL = SQL + Environment.NewLine + "from cs_visit a  ";
                SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid) ";
                SQL = SQL + Environment.NewLine + "where trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd')  ";
                SQL = SQL + Environment.NewLine + "and status='CLS'  ";
                SQL = SQL + Environment.NewLine + "and purpose='DOC' ";
                SQL = SQL + Environment.NewLine + "group by dept ";
                SQL = SQL + Environment.NewLine + "order by 2 desc) ";
                SQL = SQL + Environment.NewLine + "group by cnt ";
                SQL = SQL + Environment.NewLine + "order by cnt desc) ";
            }
            else if (cmbFilter.Text == "% Kunjungan")
            {
                SQL = SQL + Environment.NewLine + "select rownum, dept, cnt from ( ";
                SQL = SQL + Environment.NewLine + "select LISTAGG(dept, ', ') WITHIN GROUP (ORDER BY dept ASC) dept, cnt from ( ";
                SQL = SQL + Environment.NewLine + "select dept, count(distinct a.empid) cnt  ";
                SQL = SQL + Environment.NewLine + "from cs_visit a  ";
                SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid) ";
                SQL = SQL + Environment.NewLine + "where trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd')  ";
                SQL = SQL + Environment.NewLine + "and status='CLS'  ";
                SQL = SQL + Environment.NewLine + "and purpose='DOC' ";
                SQL = SQL + Environment.NewLine + "group by dept ";
                SQL = SQL + Environment.NewLine + "order by 2 desc) ";
                SQL = SQL + Environment.NewLine + "group by cnt ";
                SQL = SQL + Environment.NewLine + "order by cnt desc) ";
            }
            else if (cmbFilter.Text == "% Karyawan")
            {
                SQL = SQL + Environment.NewLine + "select rownum, dept, cnt from (  ";
                SQL = SQL + Environment.NewLine + "select LISTAGG(dept, ', ') WITHIN GROUP (ORDER BY dept ASC) dept, cnt from (  ";
                SQL = SQL + Environment.NewLine + "select dept, round(cnt/greatest(cnt_dept,cnt)*100,0) cnt ";
                SQL = SQL + Environment.NewLine + "from ( ";
                SQL = SQL + Environment.NewLine + "select dept, count(distinct a.empid) cnt, ";
                SQL = SQL + Environment.NewLine + "TTIT.CS_CNT_EMP_BY_DEPT(dept) cnt_dept ";
                SQL = SQL + Environment.NewLine + "from cs_visit a   ";
                SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)  ";
                SQL = SQL + Environment.NewLine + "where trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd')  ";
                SQL = SQL + Environment.NewLine + "and status='CLS'   ";
                SQL = SQL + Environment.NewLine + "and purpose='DOC'  ";
                SQL = SQL + Environment.NewLine + "group by dept) ";
                SQL = SQL + Environment.NewLine + "order by 2 desc)  ";
                SQL = SQL + Environment.NewLine + "group by cnt  ";
                SQL = SQL + Environment.NewLine + "order by cnt desc)  ";

            }


            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 40;
                

                gridView1.OptionsView.RowAutoHeight = true;

                RepositoryItemMemoEdit dept_nm = new RepositoryItemMemoEdit();
                gridControl1.RepositoryItems.Add(dept_nm);
                gridView1.Columns[1].ColumnEdit = dept_nm;

                gridView1.Columns[0].Caption = "No";
                gridView1.Columns[1].Caption = "Department";
                gridView1.Columns[2].Caption = "Jumlah";
                
                gridView1.OptionsBehavior.Editable = false;
                gridView1.BestFitColumns();
                gridView1.Columns[2].Width = 80;
                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                // Create a doughnut series. 
                Series series1 = new Series("Department", ViewType.Bar);

                chartControl12.Series.Clear();
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    series1.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[2]) }));
                }

                // Add the series to the chart. 
                chartControl12.Series.Add(series1);
                series1.ArgumentScaleType = ScaleType.Qualitative;
                series1.ValueScaleType = ScaleType.Numerical;

                series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                series1.Label.BackColor = Color.White;

                ((BarSeriesView)series1.View).ColorEach = true;
                ((BarSeriesLabel)series1.Label).Position = BarSeriesLabelPosition.Top;
                //series1.Label.Font = new Font("Tahoma", 10, FontStyle.Bold);
                series1.Label.TextOrientation = TextOrientation.Horizontal;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series1.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Specify how series points are sorted. 
                //series1.SeriesPointsSorting = SortingMode.Ascending;
                //series1.SeriesPointsSortingKey = SeriesPointKey.Argument;

                // Add a title to the chart and hide the legend. 
                ChartTitle chartTitle1 = new ChartTitle();
                //chartTitle1.Text = "Visit By Dept";

                if (cmbFilter.Text == "Jumlah")
                {
                    chartTitle1.Text = "Visit By Dept";
                }
                else if (cmbFilter.Text == "% Kunjungan")
                {
                    chartTitle1.Text = "Visit By Dept";
                }
                else if (cmbFilter.Text == "% Karyawan")
                {
                    chartTitle1.Text = "Visit By Dept Employees (%)";
                }

                chartControl12.Titles.Clear();
                chartControl12.Titles.Add(chartTitle1);
                chartControl12.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

                // Add the chart to the form. 
                chartControl12.Dock = DockStyle.Fill;
                tableLayoutPanel7.Controls.Add(chartControl12);

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();

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
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "visit_by_dept.xls",
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

        private void LoadDataDiseaseByDept()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select rownum num, dept, cnt from( ";
            SQL = SQL + Environment.NewLine + "select dept, count(distinct a.empid) cnt   ";
            SQL = SQL + Environment.NewLine + "from cs_visit a   ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)  ";
            SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm')='" + dStartDt.Text + "'  ";
            SQL = SQL + Environment.NewLine + "and status='CLS'   ";
            SQL = SQL + Environment.NewLine + "and purpose='DOC' ";
            SQL = SQL + Environment.NewLine + "group by dept  ";
            SQL = SQL + Environment.NewLine + "order by 2 desc ) ";
            SQL = SQL + Environment.NewLine + "where rownum<=6 ";


            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                int no = 0;
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    string SQL2 = "";

                    SQL2 = "";
                    SQL2 = SQL2 + Environment.NewLine + "select item_name, count(distinct a.empid) cnt ";
                    SQL2 = SQL2 + Environment.NewLine + "from cs_visit a   ";
                    SQL2 = SQL2 + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)  ";
                    SQL2 = SQL2 + Environment.NewLine + "join cs_patient c on (a.empid=c.empid) ";
                    SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa d on (c.rm_no=d.rm_no and a.que01=d.visit_no and trunc(visit_date)=d.insp_date) ";
                    SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa_item e on (d.item_cd=e.item_cd) ";
                    SQL2 = SQL2 + Environment.NewLine + "where to_char(visit_date,'yyyy-mm')='" + dStartDt.Text + "'  ";
                    SQL2 = SQL2 + Environment.NewLine + "and a.status='CLS'   ";
                    SQL2 = SQL2 + Environment.NewLine + "and a.purpose='DOC' ";
                    SQL2 = SQL2 + Environment.NewLine + "and b.dept='" + dt.Rows[no]["dept"].ToString() + "' ";
                    SQL2 = SQL2 + Environment.NewLine + "and type_diagnosa='P' ";
                    SQL2 = SQL2 + Environment.NewLine + "group by item_name ";
                    SQL2 = SQL2 + Environment.NewLine + "order by 2 desc ";

                    OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adOra2 = new OleDbDataAdapter(SQL2, oraConnect2);
                    DataTable dt2 = new DataTable();
                    adOra2.Fill(dt2);

                    if (no == 0)
                    {
                        
                        if (dt2.Rows.Count > 0)
                        {
                            label1.Text = dt.Rows[no]["dept"].ToString() + " - " + dt.Rows[no]["cnt"].ToString();

                            gridControl2.DataSource = null;
                            gridView2.Columns.Clear();
                            gridControl2.DataSource = dt2;

                            gridView2.OptionsView.ColumnAutoWidth = true;
                            gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                            gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
                            gridView2.IndicatorWidth = 40;

                            gridView2.OptionsView.RowAutoHeight = true;

                            RepositoryItemMemoEdit dept_nm = new RepositoryItemMemoEdit();
                            gridControl2.RepositoryItems.Add(dept_nm);
                            gridView2.Columns[0].ColumnEdit = dept_nm;

                            gridView2.Columns[0].Caption = "Disease";
                            gridView2.Columns[1].Caption = "Jumlah";

                            gridView2.OptionsBehavior.Editable = false;
                            gridView2.BestFitColumns();
                            gridView2.Columns[1].Width = 80;
                            gridView2.Columns[0].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                            dt2 = null;
                        }
                        else
                        {
                            gridControl2.DataSource = null;
                            gridView2.Columns.Clear();
                        }
                    }
                    else if (no == 1)
                    {
                        
                        if (dt2.Rows.Count > 0)
                        {
                            label2.Text = dt.Rows[no]["dept"].ToString() + " - " + dt.Rows[no]["cnt"].ToString();

                            gridControl3.DataSource = null;
                            gridView3.Columns.Clear();
                            gridControl3.DataSource = dt2;

                            gridView3.OptionsView.ColumnAutoWidth = true;
                            gridView3.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                            gridView3.Appearance.HeaderPanel.FontSizeDelta = 0;
                            gridView3.IndicatorWidth = 40;

                            gridView3.OptionsView.RowAutoHeight = true;

                            RepositoryItemMemoEdit dept_nm = new RepositoryItemMemoEdit();
                            gridControl3.RepositoryItems.Add(dept_nm);
                            gridView3.Columns[0].ColumnEdit = dept_nm;

                            gridView3.Columns[0].Caption = "Disease";
                            gridView3.Columns[1].Caption = "Jumlah";

                            gridView3.OptionsBehavior.Editable = false;
                            gridView3.BestFitColumns();
                            gridView3.Columns[1].Width = 80;
                            gridView3.Columns[0].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                            dt2 = null;
                        }
                        else
                        {
                            gridControl3.DataSource = null;
                            gridView3.Columns.Clear();
                        }
                    }
                    else if (no == 2)
                    {
                        
                        if (dt2.Rows.Count > 0)
                        {
                            label3.Text = dt.Rows[no]["dept"].ToString() + " - " + dt.Rows[no]["cnt"].ToString();

                            gridControl4.DataSource = null;
                            gridView4.Columns.Clear();
                            gridControl4.DataSource = dt2;

                            gridView4.OptionsView.ColumnAutoWidth = true;
                            gridView4.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                            gridView4.Appearance.HeaderPanel.FontSizeDelta = 0;
                            gridView4.IndicatorWidth = 40;

                            gridView4.OptionsView.RowAutoHeight = true;

                            RepositoryItemMemoEdit dept_nm = new RepositoryItemMemoEdit();
                            gridControl4.RepositoryItems.Add(dept_nm);
                            gridView4.Columns[0].ColumnEdit = dept_nm;

                            gridView4.Columns[0].Caption = "Disease";
                            gridView4.Columns[1].Caption = "Jumlah";

                            gridView4.OptionsBehavior.Editable = false;
                            gridView4.BestFitColumns();
                            gridView4.Columns[1].Width = 80;
                            gridView4.Columns[0].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                            dt2 = null;
                        }
                        else
                        {
                            gridControl4.DataSource = null;
                            gridView4.Columns.Clear();
                        }
                    }
                    else if (no == 3)
                    {
                        
                        if (dt2.Rows.Count > 0)
                        {
                            label4.Text = dt.Rows[no]["dept"].ToString() + " - " + dt.Rows[no]["cnt"].ToString();

                            gridControl5.DataSource = null;
                            gridView5.Columns.Clear();
                            gridControl5.DataSource = dt2;

                            gridView5.OptionsView.ColumnAutoWidth = true;
                            gridView5.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                            gridView5.Appearance.HeaderPanel.FontSizeDelta = 0;
                            gridView5.IndicatorWidth = 40;

                            gridView5.OptionsView.RowAutoHeight = true;

                            RepositoryItemMemoEdit dept_nm = new RepositoryItemMemoEdit();
                            gridControl5.RepositoryItems.Add(dept_nm);
                            gridView5.Columns[0].ColumnEdit = dept_nm;

                            gridView5.Columns[0].Caption = "Disease";
                            gridView5.Columns[1].Caption = "Jumlah";

                            gridView5.OptionsBehavior.Editable = false;
                            gridView5.BestFitColumns();
                            gridView5.Columns[1].Width = 80;
                            gridView5.Columns[0].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                            dt2 = null;
                        }
                        else
                        {
                            gridControl5.DataSource = null;
                            gridView5.Columns.Clear();
                        }
                    }
                    else if (no == 4)
                    {

                        if (dt2.Rows.Count > 0)
                        {
                            label5.Text = dt.Rows[no]["dept"].ToString() + " - " + dt.Rows[no]["cnt"].ToString();

                            gridControl6.DataSource = null;
                            gridView6.Columns.Clear();
                            gridControl6.DataSource = dt2;

                            gridView6.OptionsView.ColumnAutoWidth = true;
                            gridView6.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                            gridView6.Appearance.HeaderPanel.FontSizeDelta = 0;
                            gridView6.IndicatorWidth = 40;

                            gridView6.OptionsView.RowAutoHeight = true;

                            RepositoryItemMemoEdit dept_nm = new RepositoryItemMemoEdit();
                            gridControl6.RepositoryItems.Add(dept_nm);
                            gridView6.Columns[0].ColumnEdit = dept_nm;

                            gridView6.Columns[0].Caption = "Disease";
                            gridView6.Columns[1].Caption = "Jumlah";

                            gridView6.OptionsBehavior.Editable = false;
                            gridView6.BestFitColumns();
                            gridView6.Columns[1].Width = 80;
                            gridView6.Columns[0].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                            dt2 = null;
                        }
                        else
                        {
                            gridControl6.DataSource = null;
                            gridView6.Columns.Clear();
                        }
                    }
                    else if (no == 5)
                    {

                        if (dt2.Rows.Count > 0)
                        {
                            label6.Text = dt.Rows[no]["dept"].ToString() + " - " + dt.Rows[no]["cnt"].ToString();

                            gridControl7.DataSource = null;
                            gridView7.Columns.Clear();
                            gridControl7.DataSource = dt2;

                            gridView7.OptionsView.ColumnAutoWidth = true;
                            gridView7.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                            gridView7.Appearance.HeaderPanel.FontSizeDelta = 0;
                            gridView7.IndicatorWidth = 40;

                            gridView7.OptionsView.RowAutoHeight = true;

                            RepositoryItemMemoEdit dept_nm = new RepositoryItemMemoEdit();
                            gridControl7.RepositoryItems.Add(dept_nm);
                            gridView7.Columns[0].ColumnEdit = dept_nm;

                            gridView7.Columns[0].Caption = "Disease";
                            gridView7.Columns[1].Caption = "Jumlah";

                            gridView7.OptionsBehavior.Editable = false;
                            gridView7.BestFitColumns();
                            gridView7.Columns[1].Width = 80;
                            gridView7.Columns[0].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                            dt2 = null;
                        }
                        else
                        {
                            gridControl7.DataSource = null;
                            gridView7.Columns.Clear();
                        }
                    }

                    no++;
                }


                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();

        }

        private void LoadDataAVGDailyTime()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select a.visit_date,  round(avg(nvl((a.rsv-nvl(a.hold,0)) + a.ins + a.med,0)),2) as doc_avg, ";
            SQL = SQL + Environment.NewLine + "round(avg(nvl((b.rsv-nvl(b.hold,0)) + b.ins + b.med,0)),2) as mid_avg, a.info_date ";
            SQL = SQL + Environment.NewLine + "from (  ";
            SQL = SQL + Environment.NewLine + "select a.empid, name, dept, type_patient, work_accident, purpose,  ";
            SQL = SQL + Environment.NewLine + "que01, to_char(visit_date,'yyyy-mm-dd') visit_date,  to_char(visit_date,'dd-Dy') info_date, ";
            SQL = SQL + Environment.NewLine + "to_char(visit_date,'hh24:mi:ss') visit_time,  ";
            SQL = SQL + Environment.NewLine + "to_char(time_reservation,'hh24:mi:ss') reservation_time,  ";
            SQL = SQL + Environment.NewLine + "to_char(time_inspection,'hh24:mi:ss') inspection_time,  ";
            SQL = SQL + Environment.NewLine + "to_char(decode(observation,'Y',time_receipt,time_end),'hh24:mi:ss') end_time,  ";
            SQL = SQL + Environment.NewLine + "round((time_reservation-visit_date) * 24 * 60) rsv,  ";
            SQL = SQL + Environment.NewLine + "round((time_inspection-time_reservation) * 24 * 60) ins,  ";
            SQL = SQL + Environment.NewLine + "round((time_receipt-time_inspection) * 24 * 60) med, ";
            SQL = SQL + Environment.NewLine + "round((end_hold-start_hold) * 24 * 60) hold, a.ins_date  ";
            SQL = SQL + Environment.NewLine + "from cs_visit a    ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)    ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (b.empid=c.empid)  ";
            SQL = SQL + Environment.NewLine + "join cs_anamnesa d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no)  ";
            SQL = SQL + Environment.NewLine + "where 1=1   ";
            SQL = SQL + Environment.NewLine + "and to_char(visit_date,'yyyy-mm') = '" + dStartDt.Text + "'  ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS' ";
            SQL = SQL + Environment.NewLine + "and a.purpose='DOC') a left join ";
            SQL = SQL + Environment.NewLine + "(  ";
            SQL = SQL + Environment.NewLine + "select a.empid, name, dept, type_patient, work_accident, purpose,  ";
            SQL = SQL + Environment.NewLine + "que01, to_char(visit_date,'yyyy-mm-dd') visit_date, to_char(visit_date,'dd-Dy') info_date,  ";
            SQL = SQL + Environment.NewLine + "to_char(visit_date,'hh24:mi:ss') visit_time,  ";
            SQL = SQL + Environment.NewLine + "to_char(time_reservation,'hh24:mi:ss') reservation_time,  ";
            SQL = SQL + Environment.NewLine + "to_char(time_inspection,'hh24:mi:ss') inspection_time,  ";
            SQL = SQL + Environment.NewLine + "to_char(decode(observation,'Y',time_receipt,time_end),'hh24:mi:ss') end_time,  ";
            SQL = SQL + Environment.NewLine + "round((time_reservation-visit_date) * 24 * 60) rsv,  ";
            SQL = SQL + Environment.NewLine + "round((time_inspection-time_reservation) * 24 * 60) ins,  ";
            SQL = SQL + Environment.NewLine + "round((time_receipt-time_inspection) * 24 * 60) med, ";
            SQL = SQL + Environment.NewLine + "round((end_hold-start_hold) * 24 * 60) hold, a.ins_date  ";
            SQL = SQL + Environment.NewLine + "from cs_visit a    ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)    ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (b.empid=c.empid)  ";
            SQL = SQL + Environment.NewLine + "join cs_anamnesa d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no)  ";
            SQL = SQL + Environment.NewLine + "where 1=1   ";
            SQL = SQL + Environment.NewLine + "and to_char(visit_date,'yyyy-mm') = '" + dStartDt.Text + "'  ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS' ";
            SQL = SQL + Environment.NewLine + "and a.purpose='MID') b on a.visit_date=b.visit_date ";
            SQL = SQL + Environment.NewLine + "where 1=1  ";
            SQL = SQL + Environment.NewLine + "group by a.visit_date, a.info_date ";
            SQL = SQL + Environment.NewLine + "order by a.visit_date ";


            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                // Create a line series. 
                Series series1 = new Series("Avg Doctor", ViewType.Line);

                chartControl13.Series.Clear();
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    series1.Points.Add(new SeriesPoint(Convert.ToString(row[3]), new double[] { Convert.ToDouble(row[1]) }));
                }

                // Add the series to the chart. 
                chartControl13.Series.Add(series1);

                // Set the numerical argument scale types for the series, 
                // as it is qualitative, by default. 
                series1.ArgumentScaleType = ScaleType.Qualitative;
                series1.ValueScaleType = ScaleType.Numerical;
                series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

                // Access the view-type-specific options of the series. 
                ((LineSeriesView)series1.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                ((LineSeriesView)series1.View).LineMarkerOptions.Kind = MarkerKind.Circle;
                //((LineSeriesView)series1.View).LineStyle.DashStyle = DashStyle.Dash;

                // Access the type-specific options of the diagram. 
                ((XYDiagram)chartControl13.Diagram).EnableAxisXZooming = true;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series1.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);


                // Create a line series. 
                Series series2 = new Series("Avg Midwife", ViewType.Line);

                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    series2.Points.Add(new SeriesPoint(Convert.ToString(row[3]), new double[] { Convert.ToDouble(row[2]) }));
                }

                // Add the series to the chart. 
                chartControl13.Series.Add(series2);

                // Set the numerical argument scale types for the series, 
                // as it is qualitative, by default. 
                series2.ArgumentScaleType = ScaleType.Qualitative;
                series2.ValueScaleType = ScaleType.Numerical;
                //series2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

                // Access the view-type-specific options of the series. 
                ((LineSeriesView)series2.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                ((LineSeriesView)series2.View).LineMarkerOptions.Kind = MarkerKind.Circle;
                //((LineSeriesView)series2.View).LineStyle.DashStyle = DashStyle.Dash;

                // Access the type-specific options of the diagram. 
                ((XYDiagram)chartControl13.Diagram).EnableAxisXZooming = true;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series2.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Specify how series points are sorted. 
                //series1.SeriesPointsSorting = SortingMode.Ascending;
                //series1.SeriesPointsSortingKey = SeriesPointKey.Argument;

                // Add a title to the chart and hide the legend. 
                ChartTitle chartTitle1 = new ChartTitle();
                chartTitle1.Text = "Avg Daily Visit";

                chartControl13.Titles.Clear();
                chartControl13.Titles.Add(chartTitle1);
                chartControl13.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                // Add the chart to the form. 
                chartControl13.Dock = DockStyle.Fill;
                tableLayoutPanel9.Controls.Add(chartControl13);

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();

        }

        private void LoadDataAvgMonthlyTime()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select a.visit_date,  round(avg(nvl((a.rsv-nvl(a.hold,0)) + a.ins + a.med,0)),2) as doc_avg, ";
            SQL = SQL + Environment.NewLine + "round(avg(nvl((b.rsv-nvl(b.hold,0)) + b.ins + b.med,0)),2) as mid_avg ";
            SQL = SQL + Environment.NewLine + "from (  ";
            SQL = SQL + Environment.NewLine + "select a.empid, name, dept, type_patient, work_accident, purpose,  ";
            SQL = SQL + Environment.NewLine + "que01, to_char(visit_date,'yyyy-mm') visit_date,  ";
            SQL = SQL + Environment.NewLine + "to_char(visit_date,'hh24:mi:ss') visit_time,  ";
            SQL = SQL + Environment.NewLine + "to_char(time_reservation,'hh24:mi:ss') reservation_time,  ";
            SQL = SQL + Environment.NewLine + "to_char(time_inspection,'hh24:mi:ss') inspection_time,  ";
            SQL = SQL + Environment.NewLine + "to_char(decode(observation,'Y',time_receipt,time_end),'hh24:mi:ss') end_time,  ";
            SQL = SQL + Environment.NewLine + "round((time_reservation-visit_date) * 24 * 60) rsv,  ";
            SQL = SQL + Environment.NewLine + "round((time_inspection-time_reservation) * 24 * 60) ins,  ";
            SQL = SQL + Environment.NewLine + "round((time_receipt-time_inspection) * 24 * 60) med, ";
            SQL = SQL + Environment.NewLine + "round((end_hold-start_hold) * 24 * 60) hold, a.ins_date  ";
            SQL = SQL + Environment.NewLine + "from cs_visit a    ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)    ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (b.empid=c.empid)  ";
            SQL = SQL + Environment.NewLine + "join cs_anamnesa d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no)  ";
            SQL = SQL + Environment.NewLine + "where 1=1   ";
            SQL = SQL + Environment.NewLine + "and trunc(visit_date) between trunc(add_months(sysdate,-12)) and last_day(to_date('" + dStartDt.Text + "','yyyy-mm')) ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS' ";
            SQL = SQL + Environment.NewLine + "and a.purpose='DOC') a left join ";
            SQL = SQL + Environment.NewLine + "(  ";
            SQL = SQL + Environment.NewLine + "select a.empid, name, dept, type_patient, work_accident, purpose,  ";
            SQL = SQL + Environment.NewLine + "que01, to_char(visit_date,'yyyy-mm') visit_date,  ";
            SQL = SQL + Environment.NewLine + "to_char(visit_date,'hh24:mi:ss') visit_time,  ";
            SQL = SQL + Environment.NewLine + "to_char(time_reservation,'hh24:mi:ss') reservation_time,  ";
            SQL = SQL + Environment.NewLine + "to_char(time_inspection,'hh24:mi:ss') inspection_time,  ";
            SQL = SQL + Environment.NewLine + "to_char(decode(observation,'Y',time_receipt,time_end),'hh24:mi:ss') end_time,  ";
            SQL = SQL + Environment.NewLine + "round((time_reservation-visit_date) * 24 * 60) rsv,  ";
            SQL = SQL + Environment.NewLine + "round((time_inspection-time_reservation) * 24 * 60) ins,  ";
            SQL = SQL + Environment.NewLine + "round((time_receipt-time_inspection) * 24 * 60) med, ";
            SQL = SQL + Environment.NewLine + "round((end_hold-start_hold) * 24 * 60) hold, a.ins_date  ";
            SQL = SQL + Environment.NewLine + "from cs_visit a    ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)    ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (b.empid=c.empid)  ";
            SQL = SQL + Environment.NewLine + "join cs_anamnesa d on (c.rm_no=d.rm_no and trunc(a.visit_date)=d.insp_date and a.que01=d.visit_no)  ";
            SQL = SQL + Environment.NewLine + "where 1=1   ";
            SQL = SQL + Environment.NewLine + "and trunc(visit_date) between trunc(add_months(sysdate,-12)) and last_day(to_date('" + dStartDt.Text + "','yyyy-mm')) ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS' ";
            SQL = SQL + Environment.NewLine + "and a.purpose='MID') b on a.visit_date=b.visit_date ";
            SQL = SQL + Environment.NewLine + "where 1=1  ";
            SQL = SQL + Environment.NewLine + "group by a.visit_date ";
            SQL = SQL + Environment.NewLine + "order by a.visit_date ";


            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                // Create a doughnut series. 
                Series series1 = new Series("Avg Doctor", ViewType.Bar);

                chartControl14.Series.Clear();
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    series1.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[1]) }));
                }

                // Add the series to the chart. 
                chartControl14.Series.Add(series1);
                series1.ArgumentScaleType = ScaleType.Qualitative;
                series1.ValueScaleType = ScaleType.Numerical;

                series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                series1.Label.BackColor = Color.White;

                ((BarSeriesLabel)series1.Label).Position = BarSeriesLabelPosition.Top;
                //series1.Label.Font = new Font("Tahoma", 10, FontStyle.Bold);
                series1.Label.TextOrientation = TextOrientation.Horizontal;

                // Hide the legend (if necessary). 
                chartControl14.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                series1.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Create a doughnut series. 
                Series series2 = new Series("Avg Midwife", ViewType.Bar);

                
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    series2.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[2]) }));
                }

                // Add the series to the chart. 
                chartControl14.Series.Add(series2);
                series2.ArgumentScaleType = ScaleType.Qualitative;
                series2.ValueScaleType = ScaleType.Numerical;

                series2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                series2.Label.BackColor = Color.White;

                ((BarSeriesLabel)series2.Label).Position = BarSeriesLabelPosition.Top;
                //series2.Label.Font = new Font("Tahoma", 10, FontStyle.Bold);
                series2.Label.TextOrientation = TextOrientation.Horizontal;

                // Hide the legend (if necessary). 
                chartControl14.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                // Specify the text pattern of series labels. 
                //series2.Label.TextPattern = "{A}: {V} , {VP:P}";
                series2.Label.Font = new Font(series2.Label.Font.FontFamily, 9, FontStyle.Bold);

                // Specify how series points are sorted. 
                //series2.SeriesPointsSorting = SortingMode.Ascending;
                //series2.SeriesPointsSortingKey = SeriesPointKey.Argument;

                // Add a title to the chart and hide the legend. 
                ChartTitle chartTitle1 = new ChartTitle();
                chartTitle1.Text = "Avg Monthly Visit";

                chartControl14.Titles.Clear();
                chartControl14.Titles.Add(chartTitle1);
                //chartControl14.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

                // Add the chart to the form. 
                chartControl14.Dock = DockStyle.Fill;
                tableLayoutPanel9.Controls.Add(chartControl14);

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();

        }

        private void LoadDataGroupDisease1()
        {
            string SQL = "";

            if (txtJml.Text == "" || txtJml.Text == "0")
            {
                txtJml.Text = "20";
            }

            SQL = "";
            SQL = SQL + Environment.NewLine + "select initcap(item_name) diagnosa, cnt, round(rate/cnt*100,2) skd_rate from (  ";
            SQL = SQL + Environment.NewLine + "select item_cd, sum(cnt) cnt, sum(rate) rate from ( ";
            SQL = SQL + Environment.NewLine + "select c.item_cd, count(distinct a.empid) cnt, 0 rate  ";
            SQL = SQL + Environment.NewLine + "from cs_visit a   ";
            SQL = SQL + Environment.NewLine + "join cs_patient b on (a.empid=b.empid)  ";
            SQL = SQL + Environment.NewLine + "join cs_diagnosa c on (trunc(a.visit_date)=c.insp_date and b.rm_no=c.rm_no and a.que01=c.visit_no)  ";
            SQL = SQL + Environment.NewLine + "where b.status='A'   ";
            SQL = SQL + Environment.NewLine + "and to_char(visit_date,'yyyy-mm')='" + dStartDt.Text + "'  ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS'  ";
            SQL = SQL + Environment.NewLine + "and poli_cd in ('POL0000','POL0001')  ";
            SQL = SQL + Environment.NewLine + "and c.item_cd not in ('X11')  ";
            SQL = SQL + Environment.NewLine + "and type_diagnosa='P'  ";
            SQL = SQL + Environment.NewLine + "group by c.item_cd  ";
            SQL = SQL + Environment.NewLine + "union  ";
            SQL = SQL + Environment.NewLine + "select c.item_cd, 0 cnt, count(0) rate from cs_visit a   ";
            SQL = SQL + Environment.NewLine + "join cs_patient b on (a.empid=b.empid)  ";
            SQL = SQL + Environment.NewLine + "join cs_diagnosa c on (trunc(a.visit_date)=c.insp_date and b.rm_no=c.rm_no and a.que01=c.visit_no)  ";
            SQL = SQL + Environment.NewLine + "join cs_sick_leter d on (trunc(a.visit_date)=d.insp_date and b.rm_no=d.rm_no and a.que01=d.visit_no) ";
            SQL = SQL + Environment.NewLine + "where b.status='A'   ";
            SQL = SQL + Environment.NewLine + "and to_char(visit_date,'yyyy-mm')='" + dStartDt.Text + "'  ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS'  ";
            SQL = SQL + Environment.NewLine + "and poli_cd in ('POL0000','POL0001')  ";
            SQL = SQL + Environment.NewLine + "and c.item_cd not in ('X11')  ";
            SQL = SQL + Environment.NewLine + "and type_diagnosa='P'  ";
            SQL = SQL + Environment.NewLine + "group by c.item_cd ) ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "group by item_cd ";
            SQL = SQL + Environment.NewLine + "order by 2 desc) aa  ";
            SQL = SQL + Environment.NewLine + "join cs_diagnosa_item bb on (aa.item_cd=bb.item_cd)  ";
            SQL = SQL + Environment.NewLine + "where bb.status='A'  ";
            SQL = SQL + Environment.NewLine + "and rownum <= " + txtJml.Text;



            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                gridControl9.DataSource = null;
                gridView9.Columns.Clear();
                gridControl9.DataSource = dt;

                gridView9.OptionsView.ColumnAutoWidth = true;
                gridView9.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView9.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView9.IndicatorWidth = 40;
                gridView9.OptionsView.RowAutoHeight = true;

                gridView9.Columns[0].Caption = "Diagnosa";
                gridView9.Columns[1].Caption = "Jumlah";
                gridView9.Columns[2].Caption = "(%) SKD";

                gridView9.OptionsBehavior.Editable = false;
                gridView9.BestFitColumns();
                gridView9.Columns[1].Width = 80;
                gridView9.Columns[2].Width = 100;
                gridView9.Columns[0].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();
        }

        private void LoadDataGroupDisease2()
        {
            string SQL = "";

            if (txtJml2.Text == "" || txtJml2.Text == "0")
            {
                txtJml2.Text = "20";
            }

            SQL = "";
            SQL = SQL + Environment.NewLine + "select initcap(item_name) diagnosa, cnt from ( ";
            SQL = SQL + Environment.NewLine + "select c.item_cd, count(distinct a.empid) cnt from cs_visit a  ";
            SQL = SQL + Environment.NewLine + "join cs_patient b on (a.empid=b.empid) ";
            SQL = SQL + Environment.NewLine + "join cs_diagnosa c on (trunc(a.visit_date)=c.insp_date and b.rm_no=c.rm_no and a.que01=c.visit_no) ";
            SQL = SQL + Environment.NewLine + "join cs_sick_leter d on (trunc(a.visit_date)=d.insp_date and b.rm_no=d.rm_no and a.que01=d.visit_no) ";
            SQL = SQL + Environment.NewLine + "where b.status='A'  ";
            SQL = SQL + Environment.NewLine + "and to_char(visit_date,'yyyy-mm')='" + dStartDt.Text + "' ";
            SQL = SQL + Environment.NewLine + "and a.status='CLS' ";
            SQL = SQL + Environment.NewLine + "and poli_cd in ('POL0000','POL0001') ";
            SQL = SQL + Environment.NewLine + "and c.item_cd not in ('X11') ";
            SQL = SQL + Environment.NewLine + "and type_diagnosa='P' ";
            SQL = SQL + Environment.NewLine + "group by c.item_cd ";
            SQL = SQL + Environment.NewLine + "order by 2 desc) aa ";
            SQL = SQL + Environment.NewLine + "join cs_diagnosa_item bb on (aa.item_cd=bb.item_cd) ";
            SQL = SQL + Environment.NewLine + "and rownum <= " + txtJml2.Text;


            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                gridControl10.DataSource = null;
                gridView10.Columns.Clear();
                gridControl10.DataSource = dt;

                gridView10.OptionsView.ColumnAutoWidth = true;
                gridView10.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView10.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView10.IndicatorWidth = 40;
                gridView10.OptionsView.RowAutoHeight = true;

                gridView10.Columns[0].Caption = "Diagnosa";
                gridView10.Columns[1].Caption = "Jumlah";

                gridView10.OptionsBehavior.Editable = false;
                gridView10.BestFitColumns();
                gridView10.Columns[1].Width = 80;
                gridView10.Columns[0].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();
        }

        private void LoadDataDiseaseDatail()
        {

            if (txtJml3.Text == "" || txtJml3.Text == "0")
            {
                txtJml3.Text = "10";
            }

            string SQLa = "", s_cnt = "", s_a = "";
            SQLa = SQLa + Environment.NewLine + "select trunc(months_between(to_date('" + dEnd.Text + "','yyyy-mm-dd'),to_date('" + dBgn.Text + "','yyyy-mm-dd'))) ddate, ";
            SQLa = SQLa + Environment.NewLine + "to_char(to_date('" + dBgn.Text + "','yyyy-mm-dd'),'yyyymm') a ";
            SQLa = SQLa + Environment.NewLine + "from dual ";


            OleDbConnection oraConnecta = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraa = new OleDbDataAdapter(SQLa, oraConnecta);
            DataTable dta = new DataTable();
            adOraa.Fill(dta);

            s_cnt = dta.Rows[0]["ddate"].ToString();
            s_a = dta.Rows[0]["a"].ToString();

            if (Convert.ToInt16(s_cnt) < 0)
            {
                MessageBox.Show("Periode tanggal tidak valid");
                return;
            }
            else if (Convert.ToInt16(s_cnt) > 12)
            {
                MessageBox.Show("Periode tanggal maksimal 12 bulan");
                return;
            }
            else
            {
                string SQL = "", SQL2 = "", SQL3 = "", load_sql = "";

                SQL = "";
                SQL = SQL + Environment.NewLine + "select aa.item_cd, initcap(item_name) diagnosa, cnt from ( ";
                SQL = SQL + Environment.NewLine + "select c.item_cd, count(distinct a.empid) cnt from cs_visit a  ";
                SQL = SQL + Environment.NewLine + "join cs_patient b on (a.empid=b.empid) ";
                SQL = SQL + Environment.NewLine + "join cs_diagnosa c on (trunc(a.visit_date)=c.insp_date and b.rm_no=c.rm_no and a.que01=c.visit_no) ";
                SQL = SQL + Environment.NewLine + "where b.status='A'  ";
                SQL = SQL + Environment.NewLine + "and trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd') ";
                SQL = SQL + Environment.NewLine + "and a.status='CLS' ";
                SQL = SQL + Environment.NewLine + "and poli_cd in ('POL0000','POL0001') ";
                SQL = SQL + Environment.NewLine + "and c.item_cd not in ('X11') ";
                SQL = SQL + Environment.NewLine + "and type_diagnosa='P' ";
                SQL = SQL + Environment.NewLine + "group by c.item_cd ";
                SQL = SQL + Environment.NewLine + "order by 2 desc) aa ";
                SQL = SQL + Environment.NewLine + "join cs_diagnosa_item bb on (aa.item_cd=bb.item_cd) ";
                SQL = SQL + Environment.NewLine + "where bb.status='A' ";
                SQL = SQL + Environment.NewLine + "and rownum <= " + txtJml3.Text;

                SQL2 = "";
                SQL2 = SQL2 + Environment.NewLine + "select item_cd, diagnosa, round(cnt/greatest(cnt_visit,cnt)*100,3) as cnt from ( ";
                SQL2 = SQL2 + Environment.NewLine + "select aa.item_cd, initcap(item_name) diagnosa, cnt, ";
                SQL2 = SQL2 + Environment.NewLine + "(select count(0) from cs_visit  ";
                SQL2 = SQL2 + Environment.NewLine + "where status='CLS'  ";
                SQL2 = SQL2 + Environment.NewLine + "and trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd')";
                SQL2 = SQL2 + Environment.NewLine + "and poli_cd in ('POL0000','POL0001') ) cnt_visit ";
                SQL2 = SQL2 + Environment.NewLine + "from (  ";
                SQL2 = SQL2 + Environment.NewLine + "select c.item_cd, count( distinct a.empid) cnt from cs_visit a   ";
                SQL2 = SQL2 + Environment.NewLine + "join cs_patient b on (a.empid=b.empid)  ";
                SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa c on (trunc(a.visit_date)=c.insp_date and b.rm_no=c.rm_no and a.que01=c.visit_no)  ";
                SQL2 = SQL2 + Environment.NewLine + "where b.status='A'   ";
                SQL2 = SQL2 + Environment.NewLine + "and trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd') ";
                SQL2 = SQL2 + Environment.NewLine + "and a.status='CLS'  ";
                SQL2 = SQL2 + Environment.NewLine + "and poli_cd in ('POL0000','POL0001')  ";
                SQL2 = SQL2 + Environment.NewLine + "and c.item_cd not in ('X11')  ";
                SQL2 = SQL2 + Environment.NewLine + "and type_diagnosa='P'  ";
                SQL2 = SQL2 + Environment.NewLine + "group by c.item_cd  ";
                SQL2 = SQL2 + Environment.NewLine + "order by 2 desc) aa  ";
                SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa_item bb on (aa.item_cd=bb.item_cd)  ";
                SQL2 = SQL2 + Environment.NewLine + "where bb.status='A' ) ";
                SQL2 = SQL2 + Environment.NewLine + "where 1=1 ";
                SQL2 = SQL2 + Environment.NewLine + "and rownum <= " + txtJml3.Text;

                SQL3 = "";
                SQL3 = SQL3 + Environment.NewLine + "select item_cd, diagnosa, round(cnt/greatest(cnt_visit,cnt)*100,3) as cnt from ( ";
                SQL3 = SQL3 + Environment.NewLine + "select aa.item_cd, initcap(item_name) diagnosa, cnt, ";
                SQL3 = SQL3 + Environment.NewLine + "(select count(0) from cs_employees ";
                SQL3 = SQL3 + Environment.NewLine + "where retire_dt is null) cnt_visit ";
                SQL3 = SQL3 + Environment.NewLine + "from (  ";
                SQL3 = SQL3 + Environment.NewLine + "select c.item_cd, count( distinct a.empid) cnt from cs_visit a   ";
                SQL3 = SQL3 + Environment.NewLine + "join cs_patient b on (a.empid=b.empid)  ";
                SQL3 = SQL3 + Environment.NewLine + "join cs_diagnosa c on (trunc(a.visit_date)=c.insp_date and b.rm_no=c.rm_no and a.que01=c.visit_no)  ";
                SQL3 = SQL3 + Environment.NewLine + "where b.status='A'   ";
                SQL3 = SQL3 + Environment.NewLine + "and trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd') ";
                SQL3 = SQL3 + Environment.NewLine + "and a.status='CLS'  ";
                SQL3 = SQL3 + Environment.NewLine + "and poli_cd in ('POL0000','POL0001')  ";
                SQL3 = SQL3 + Environment.NewLine + "and c.item_cd not in ('X11')  ";
                SQL3 = SQL3 + Environment.NewLine + "and type_diagnosa='P'  ";
                SQL3 = SQL3 + Environment.NewLine + "group by c.item_cd  ";
                SQL3 = SQL3 + Environment.NewLine + "order by 2 desc) aa  ";
                SQL3 = SQL3 + Environment.NewLine + "join cs_diagnosa_item bb on (aa.item_cd=bb.item_cd)  ";
                SQL3 = SQL3 + Environment.NewLine + "where bb.status='A' ) ";
                SQL3 = SQL3 + Environment.NewLine + "where 1=1 ";
                SQL3 = SQL3 + Environment.NewLine + "and rownum <= " + txtJml3.Text;

                if (cmbFilter.Text == "Jumlah")
                {
                    load_sql = SQL;
                }
                else if (cmbFilter.Text == "% Kunjungan")
                {
                    load_sql = SQL2;
                }
                else if (cmbFilter.Text == "% Karyawan")
                {
                    load_sql = SQL3;
                }



                //loading.ShowWaitForm();
                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adOra = new OleDbDataAdapter(load_sql, oraConnect);
                    DataTable dt = new DataTable();
                    adOra.Fill(dt);

                    gridControl11.DataSource = null;
                    gridView11.Columns.Clear();
                    gridControl11.DataSource = dt;

                    gridView11.OptionsView.ColumnAutoWidth = true;
                    gridView11.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                    gridView11.Appearance.HeaderPanel.FontSizeDelta = 0;
                    gridView11.IndicatorWidth = 40;
                    gridView11.OptionsView.RowAutoHeight = true;

                    gridView11.Columns[0].Caption = "kode";
                    gridView11.Columns[1].Caption = "Diagnosa";
                    gridView11.Columns[2].Caption = "Jumlah";

                    gridView11.OptionsBehavior.Editable = false;
                    gridView11.BestFitColumns();
                    gridView11.Columns[2].Width = 80;
                    gridView11.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                    gridView11.Columns[0].Visible = false;


                    gridControl12.DataSource = null;
                    gridView12.Columns.Clear();
                    lDiagNm.Text = "-";
                    //loading.CloseWaitForm();
                }
                catch (Exception ex)
                {
                    //loading.CloseWaitForm();
                    MessageBox.Show("ERROR: " + ex.Message);
                }
                //loading.CloseWaitForm();
            }


        }


        private void gridView11_RowClick(object sender, RowClickEventArgs e)
        {
            GridView View = sender as GridView;
            string s_diag = "", s_cnt = "", s_a = "", s_diag2 = "";

            s_diag = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            s_diag2 = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);

            string SQL = "";
            SQL = SQL + Environment.NewLine + "select trunc(months_between(to_date('" + dEnd.Text + "','yyyy-mm-dd'),to_date('" + dBgn.Text + "','yyyy-mm-dd'))) ddate, ";
            SQL = SQL + Environment.NewLine + "to_char(to_date('" + dBgn.Text + "','yyyy-mm-dd'),'yyyymm') a ";
            SQL = SQL + Environment.NewLine + "from dual ";


            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            s_cnt = dt.Rows[0]["ddate"].ToString();
            s_a = dt.Rows[0]["a"].ToString();
            lDiagNm.Text = s_diag2;

            if (Convert.ToInt16(s_cnt) < 0)
            {
                MessageBox.Show("Periode tanggal tidak valid");
                return;
            }
            else if (Convert.ToInt16(s_cnt) > 12)
            {
                MessageBox.Show("Periode tanggal maksimal 12 bulan");
                return;
            }
            else
            {
                string SQL2 = "";
                listCol.Clear();

                if (cmbFilter.Text == "Jumlah")
                {
                    SQL2 = SQL2 + Environment.NewLine + "select plant,  ";
                    SQL2 = SQL2 + Environment.NewLine + "TTIT.CS_CNT_DIAG('" + s_a + "','" + s_diag + "',plant) as c" + s_a + ",";
                    if (Convert.ToInt16(s_cnt) != 0)
                    {
                        for (int i = 1; i <= Convert.ToInt16(s_cnt); i++)
                        {
                            string s = "", s_temp = "";
                            s = " select to_char(add_months(to_date('" + dBgn.Text + "','yyyy-mm-dd')," + i + "),'yyyymm') ss from dual";
                            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOra2 = new OleDbDataAdapter(s, oraConnect2);
                            DataTable dt2 = new DataTable();
                            adOra2.Fill(dt2);
                            s_temp = dt2.Rows[0]["ss"].ToString();
                            listCol.Add(s_temp);

                            SQL2 = SQL2 + Environment.NewLine + "TTIT.CS_CNT_DIAG('" + s_temp + "','" + s_diag + "',plant) as c" + s_temp + ",";
                        }
                    }
                    SQL2 = SQL2 + Environment.NewLine + "cnt, ";
                    SQL2 = SQL2 + Environment.NewLine + "case when plant in ('A','B','C','D','E','F') then 'A' ";
                    SQL2 = SQL2 + Environment.NewLine + "when plant in ('PRD','STT COMP') then 'B' ";
                    SQL2 = SQL2 + Environment.NewLine + "when plant in ('DIRECT','INDIRECT') then 'C' ";
                    SQL2 = SQL2 + Environment.NewLine + "else 'D' end urut ";
                    SQL2 = SQL2 + Environment.NewLine + "from ( ";
                    SQL2 = SQL2 + Environment.NewLine + "select plant, sum(cnt) cnt from ( ";
                    SQL2 = SQL2 + Environment.NewLine + "select  c.plant, to_char(visit_date,'yyyy-mm'), count(distinct a.empid) cnt ";
                    SQL2 = SQL2 + Environment.NewLine + "from cs_visit a   ";
                    SQL2 = SQL2 + Environment.NewLine + "join cs_employees b on (a.empid=b.empid) ";
                    SQL2 = SQL2 + Environment.NewLine + "join view_eam100_s1@DL_TTERGTOTTHCMIF c on (b.deptcd=c.deptcd) ";
                    SQL2 = SQL2 + Environment.NewLine + "join cs_patient d on (a.empid=d.empid)  ";
                    SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa e on (trunc(a.visit_date)=e.insp_date and d.rm_no=e.rm_no and a.que01=e.visit_no)  ";
                    SQL2 = SQL2 + Environment.NewLine + "where 1=1 ";
                    SQL2 = SQL2 + Environment.NewLine + "and a.status='CLS'  ";
                    SQL2 = SQL2 + Environment.NewLine + "and trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd') ";
                    SQL2 = SQL2 + Environment.NewLine + "and poli_cd in ('POL0000','POL0001')  ";
                    SQL2 = SQL2 + Environment.NewLine + "and e.item_cd not in ('X11')  ";
                    SQL2 = SQL2 + Environment.NewLine + "and e.type_diagnosa='P'  ";
                    SQL2 = SQL2 + Environment.NewLine + "and e.item_cd='" + s_diag + "' ";
                    SQL2 = SQL2 + Environment.NewLine + "group by plant,to_char(visit_date,'yyyy-mm')) y ";
                    SQL2 = SQL2 + Environment.NewLine + "group by plant ) z ";
                    SQL2 = SQL2 + Environment.NewLine + "order by urut, plant ";
                }
                else if (cmbFilter.Text == "% Kunjungan")
                {
                    SQL2 = SQL2 + Environment.NewLine + "select plant,  ";
                    SQL2 = SQL2 + Environment.NewLine + "round(TTIT.CS_CNT_DIAG('" + s_a + "','" + s_diag + "',plant)/cnt_visit*100,3) as c" + s_a + ", ";
                    if (Convert.ToInt16(s_cnt) != 0)
                    {
                        for (int i = 1; i <= Convert.ToInt16(s_cnt); i++)
                        {
                            string s = "", s_temp = "";
                            s = " select to_char(add_months(to_date('" + dBgn.Text + "','yyyy-mm-dd')," + i + "),'yyyymm') ss from dual";
                            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOra2 = new OleDbDataAdapter(s, oraConnect2);
                            DataTable dt2 = new DataTable();
                            adOra2.Fill(dt2);
                            s_temp = dt2.Rows[0]["ss"].ToString();
                            listCol.Add(s_temp);

                            SQL2 = SQL2 + Environment.NewLine + "round(TTIT.CS_CNT_DIAG('" + s_temp + "','" + s_diag + "',plant)/cnt_visit*100,3) as c" + s_temp + ",";
                        }
                    }
                    SQL2 = SQL2 + Environment.NewLine + "round(cnt/greatest(cnt_visit,cnt)*100,3) as cnt, ";
                    SQL2 = SQL2 + Environment.NewLine + "case when plant in ('A','B','C','D','E','F') then 'A' ";
                    SQL2 = SQL2 + Environment.NewLine + "when plant in ('PRD','STT COMP') then 'B' ";
                    SQL2 = SQL2 + Environment.NewLine + "when plant in ('DIRECT','INDIRECT') then 'C' ";
                    SQL2 = SQL2 + Environment.NewLine + "else 'D' end urut ";
                    SQL2 = SQL2 + Environment.NewLine + "from ( ";
                    SQL2 = SQL2 + Environment.NewLine + "select plant, ";
                    SQL2 = SQL2 + Environment.NewLine + "(select count(0) from cs_visit   ";
                    SQL2 = SQL2 + Environment.NewLine + "where status='CLS'   ";
                    SQL2 = SQL2 + Environment.NewLine + "and trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd')  ";
                    SQL2 = SQL2 + Environment.NewLine + "and poli_cd in ('POL0000','POL0001') ) cnt_visit, ";
                    SQL2 = SQL2 + Environment.NewLine + "cnt  ";
                    SQL2 = SQL2 + Environment.NewLine + "from (  ";
                    SQL2 = SQL2 + Environment.NewLine + "select plant, sum(cnt) cnt from (  ";
                    SQL2 = SQL2 + Environment.NewLine + "select  c.plant, to_char(visit_date,'yyyy-mm'), count(distinct a.empid) cnt  ";
                    SQL2 = SQL2 + Environment.NewLine + "from cs_visit a    ";
                    SQL2 = SQL2 + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)  ";
                    SQL2 = SQL2 + Environment.NewLine + "join view_eam100_s1@DL_TTERGTOTTHCMIF c on (b.deptcd=c.deptcd)  ";
                    SQL2 = SQL2 + Environment.NewLine + "join cs_patient d on (a.empid=d.empid)   ";
                    SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa e on (trunc(a.visit_date)=e.insp_date and d.rm_no=e.rm_no and a.que01=e.visit_no)   ";
                    SQL2 = SQL2 + Environment.NewLine + "where 1=1  ";
                    SQL2 = SQL2 + Environment.NewLine + "and a.status='CLS'   ";
                    SQL2 = SQL2 + Environment.NewLine + "and trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd')  ";
                    SQL2 = SQL2 + Environment.NewLine + "and poli_cd in ('POL0000','POL0001')   ";
                    SQL2 = SQL2 + Environment.NewLine + "and e.item_cd not in ('X11')   ";
                    SQL2 = SQL2 + Environment.NewLine + "and e.type_diagnosa='P'   ";
                    SQL2 = SQL2 + Environment.NewLine + "and e.item_cd='" + s_diag + "'  ";
                    SQL2 = SQL2 + Environment.NewLine + "group by plant,to_char(visit_date,'yyyy-mm')) y  ";
                    SQL2 = SQL2 + Environment.NewLine + "group by plant ) z ) zz ";
                    SQL2 = SQL2 + Environment.NewLine + "order by urut, plant ";

                }
                else if (cmbFilter.Text == "% Karyawan")
                {
                    

                    SQL2 = SQL2 + Environment.NewLine + "select plant,  ";
                    SQL2 = SQL2 + Environment.NewLine + "round(TTIT.CS_CNT_DIAG('" + s_a + "','" + s_diag + "',plant)/cnt_visit*100,3) as c" + s_a + ", ";
                    if (Convert.ToInt16(s_cnt) != 0)
                    {
                        for (int i = 1; i <= Convert.ToInt16(s_cnt); i++)
                        {
                            string s = "", s_temp = "";
                            s = " select to_char(add_months(to_date('" + dBgn.Text + "','yyyy-mm-dd')," + i + "),'yyyymm') ss from dual";
                            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOra2 = new OleDbDataAdapter(s, oraConnect2);
                            DataTable dt2 = new DataTable();
                            adOra2.Fill(dt2);
                            s_temp = dt2.Rows[0]["ss"].ToString();
                            listCol.Add(s_temp);

                            SQL2 = SQL2 + Environment.NewLine + "round(TTIT.CS_CNT_DIAG('" + s_temp + "','" + s_diag + "',plant)/cnt_visit*100,3) as c" + s_temp + ",";
                        }
                    }
                    SQL2 = SQL2 + Environment.NewLine + "round(cnt/greatest(cnt_visit,cnt)*100,3) as cnt, ";
                    SQL2 = SQL2 + Environment.NewLine + "case when plant in ('A','B','C','D','E','F') then 'A' ";
                    SQL2 = SQL2 + Environment.NewLine + "when plant in ('PRD','STT COMP') then 'B' ";
                    SQL2 = SQL2 + Environment.NewLine + "when plant in ('DIRECT','INDIRECT') then 'C' ";
                    SQL2 = SQL2 + Environment.NewLine + "else 'D' end urut ";
                    SQL2 = SQL2 + Environment.NewLine + "from ( ";
                    SQL2 = SQL2 + Environment.NewLine + "select plant, ";
                    SQL2 = SQL2 + Environment.NewLine + "(select count(0) from cs_employees ";
                    SQL2 = SQL2 + Environment.NewLine + "where retire_dt is null) cnt_visit, ";
                    SQL2 = SQL2 + Environment.NewLine + "cnt  ";
                    SQL2 = SQL2 + Environment.NewLine + "from (  ";
                    SQL2 = SQL2 + Environment.NewLine + "select plant, sum(cnt) cnt from (  ";
                    SQL2 = SQL2 + Environment.NewLine + "select  c.plant, to_char(visit_date,'yyyy-mm'), count(distinct a.empid) cnt  ";
                    SQL2 = SQL2 + Environment.NewLine + "from cs_visit a    ";
                    SQL2 = SQL2 + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)  ";
                    SQL2 = SQL2 + Environment.NewLine + "join view_eam100_s1@DL_TTERGTOTTHCMIF c on (b.deptcd=c.deptcd)  ";
                    SQL2 = SQL2 + Environment.NewLine + "join cs_patient d on (a.empid=d.empid)   ";
                    SQL2 = SQL2 + Environment.NewLine + "join cs_diagnosa e on (trunc(a.visit_date)=e.insp_date and d.rm_no=e.rm_no and a.que01=e.visit_no)   ";
                    SQL2 = SQL2 + Environment.NewLine + "where 1=1  ";
                    SQL2 = SQL2 + Environment.NewLine + "and a.status='CLS'   ";
                    SQL2 = SQL2 + Environment.NewLine + "and trunc(visit_date) between to_date('" + dBgn.Text + "','yyyy-mm-dd') and to_date('" + dEnd.Text + "','yyyy-mm-dd')  ";
                    SQL2 = SQL2 + Environment.NewLine + "and poli_cd in ('POL0000','POL0001')   ";
                    SQL2 = SQL2 + Environment.NewLine + "and e.item_cd not in ('X11')   ";
                    SQL2 = SQL2 + Environment.NewLine + "and e.type_diagnosa='P'   ";
                    SQL2 = SQL2 + Environment.NewLine + "and e.item_cd='" + s_diag + "'  ";
                    SQL2 = SQL2 + Environment.NewLine + "group by plant,to_char(visit_date,'yyyy-mm')) y  ";
                    SQL2 = SQL2 + Environment.NewLine + "group by plant ) z ) zz ";
                    SQL2 = SQL2 + Environment.NewLine + "order by urut, plant ";
                }

                loading.ShowWaitForm();
                try
                {
                    OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adOra2 = new OleDbDataAdapter(SQL2, oraConnect2);
                    DataTable dt2 = new DataTable();
                    adOra2.Fill(dt2);

                    gridControl12.DataSource = null;
                    gridView12.Columns.Clear();
                    gridControl12.DataSource = dt2;

                    gridView12.OptionsView.ColumnAutoWidth = true;
                    gridView12.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                    gridView12.Appearance.HeaderPanel.FontSizeDelta = 0;
                    gridView12.IndicatorWidth = 40;
                    gridView12.OptionsView.RowAutoHeight = true;

                    gridView12.Columns[0].Caption = "Plant";
                    gridView12.Columns[1].Caption = s_a;
                    for (int i = 0; i < listCol.Count; i++)
                    {
                        gridView12.Columns[i+2].Caption = listCol[i].ToString();
                    }
                    gridView12.Columns[listCol.Count+2].Caption = "Jumlah";
                    gridView12.Columns[listCol.Count + 3].Caption = "Urut";

                    gridView12.OptionsBehavior.Editable = false;
                    gridView12.BestFitColumns();

                    gridView12.Columns[listCol.Count + 3].Visible = false;

                    loading.CloseWaitForm();
                }
                catch (Exception ex)
                {
                    loading.CloseWaitForm();
                    MessageBox.Show("ERROR: " + ex.Message);
                }
                //loading.CloseWaitForm();

            }


        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

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

        private void gridView9_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView10_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView11_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView12_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView9_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "(%) SKD")
            {
                string rate = View.GetRowCellDisplayText(e.RowHandle, View.Columns[2]);

                if (rate != "")
                {
                    if (Convert.ToDouble(rate) > 50)
                    {
                        e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (Convert.ToDouble(rate) > 85)
                    {
                        e.Appearance.BackColor = Color.Crimson;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }

            }
        }

        private void btnUnduh1_Click(object sender, EventArgs e)
        {
            if (gridView9.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "top_disease.xls",
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
                    gridControl9.ExportToXls(saveDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("Data tidak ditemukan");
            }
        }

        private void btnUnduh2_Click(object sender, EventArgs e)
        {
            if (gridView10.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "top_disease_with_letter.xls",
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
                    gridControl10.ExportToXls(saveDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("Data tidak ditemukan");
            }
        }

        private void btnUnduh3_Click(object sender, EventArgs e)
        {
            if (gridView12.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "detail_diagnosa.xls",
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
                    gridControl12.ExportToXls(saveDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("Data tidak ditemukan");
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage.Text == "Top 10 Disease")
            {
                cmbFilter.Enabled = true;
                dBgn.Enabled = true;
                dEnd.Enabled = true;
                dStartDt.Enabled = false;
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Top 10 Disease Detail")
            {
                cmbFilter.Enabled = true;
                dBgn.Enabled = false;
                dEnd.Enabled = false;
                dStartDt.Enabled = true;
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Visit By Dept")
            {
                cmbFilter.Enabled = true;
                dBgn.Enabled = true;
                dEnd.Enabled = true;
                dStartDt.Enabled = false;
            }
            else if (xtraTabControl1.SelectedTabPage.Text == "Disease Detail")
            {
                cmbFilter.Enabled = true;
                dBgn.Enabled = true;
                dEnd.Enabled = true;
                dStartDt.Enabled = false;
            }
            else
            {
                cmbFilter.Enabled = false;
                dBgn.Enabled = false;
                dEnd.Enabled = false;
                dStartDt.Enabled = true;
            }
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }



        //private void LoadDataVisitByDept2()
        //{
        //    string SQL = "";

        //    SQL = "";
        //    SQL = SQL + Environment.NewLine + "select * from (  ";
        //    SQL = SQL + Environment.NewLine + "select dept, count(0) cnt  ";
        //    SQL = SQL + Environment.NewLine + "from cs_visit a  ";
        //    SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid) ";
        //    SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm')='" + dStartDt.Text + "' ";
        //    SQL = SQL + Environment.NewLine + "and status='CLS'  ";
        //    SQL = SQL + Environment.NewLine + "group by dept ";
        //    SQL = SQL + Environment.NewLine + "order by 2 desc) ";
        //    SQL = SQL + Environment.NewLine + "where cnt<20 ";

        //    //loading.ShowWaitForm();
        //    try
        //    {
        //        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
        //        OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
        //        DataTable dt = new DataTable();
        //        adOra.Fill(dt);

        //        // Create a doughnut series. 
        //        Series series1 = new Series("Department", ViewType.Bar);

        //        chartControl13.Series.Clear();
        //        foreach (DataRow row in dt.Rows) // Loop over the rows.
        //        {
        //            series1.Points.Add(new SeriesPoint(Convert.ToString(row[0]), new double[] { Convert.ToDouble(row[1]) }));
        //        }

        //        // Add the series to the chart. 
        //        chartControl13.Series.Add(series1);
        //        series1.ArgumentScaleType = ScaleType.Qualitative;
        //        series1.ValueScaleType = ScaleType.Numerical;

        //        series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
        //        series1.Label.BackColor = Color.White;

        //        ((BarSeriesLabel)series1.Label).Position = BarSeriesLabelPosition.Top;
        //        //series1.Label.Font = new Font("Tahoma", 10, FontStyle.Bold);
        //        series1.Label.TextOrientation = TextOrientation.Horizontal;

        //        // Specify the text pattern of series labels. 
        //        //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
        //        series1.Label.Font = new Font(series1.Label.Font.FontFamily, 9, FontStyle.Bold);

        //        // Specify how series points are sorted. 
        //        //series1.SeriesPointsSorting = SortingMode.Ascending;
        //        //series1.SeriesPointsSortingKey = SeriesPointKey.Argument;

        //        // Add a title to the chart and hide the legend. 
        //        ChartTitle chartTitle1 = new ChartTitle();
        //        chartTitle1.Text = "Visit By Dept";

        //        chartControl13.Titles.Clear();
        //        chartControl13.Titles.Add(chartTitle1);
        //        chartControl13.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

        //        // Add the chart to the form. 
        //        chartControl13.Dock = DockStyle.Fill;
        //        tableLayoutPanel7.Controls.Add(chartControl13);

        //        //loading.CloseWaitForm();
        //    }
        //    catch (Exception ex)
        //    {
        //        //loading.CloseWaitForm();
        //        MessageBox.Show("ERROR: " + ex.Message);
        //    }
        //    //loading.CloseWaitForm();

        //}
    }
}