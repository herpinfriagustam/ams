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

namespace Clinic
{
    public partial class AudiometriReport : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        DataSet dsAudiometri = new DataSet();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string type = "";
        ReportAudiometri reportAudiometri = null;

        public AudiometriReport()
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
            
        }

        private void LoadData()
        {
            string SQL;

            SQL = "";
            SQL = SQL + Environment.NewLine + "select a.empid, name, dept ";
            SQL = SQL + Environment.NewLine + "from cs_patient a  ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)  ";
            SQL = SQL + Environment.NewLine + "join cs_audiometri c on (b.empid=c.empid)  ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and a.status = 'A' ";
            SQL = SQL + Environment.NewLine + "and b.retire_dt is null ";
            SQL = SQL + Environment.NewLine + "and a.group_patient = 'COMM' ";
            SQL = SQL + Environment.NewLine + "order by name asc ";


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
                gridView1.BestFitColumns();

                gridView1.Columns[0].Caption = "NIK";
                gridView1.Columns[1].Caption = "Nama";
                gridView1.Columns[2].Caption = "Department";

                gridView1.Columns[0].Width = 80;
                gridView1.Columns[1].Width = 120;
                gridView1.Columns[2].Width = 180;

                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[2].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

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
            GridView View = sender as GridView;
            string sql_print="", s_nik = "", s_nama = "", s_rm = "", s_alamat = "", s_umur = "", s_jk = "", p_type = "";
            string s_dept = "", s_gpa = "", s_hpht = "", s_tp = "", s_darah = "";

            s_nik = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            
            sql_print = "";
            s_nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();

            sql_print = sql_print + Environment.NewLine + "select a.empid, periode, kn250, kn500, ";
            sql_print = sql_print + Environment.NewLine + "kn1000, kn2000, kn3000, kn4000, kn6000, kn8000,  ";
            sql_print = sql_print + Environment.NewLine + "kr250, kr500, ";
            sql_print = sql_print + Environment.NewLine + "kr1000, kr2000, kr3000, kr4000, kr6000, kr8000,  ";
            sql_print = sql_print + Environment.NewLine + "name, dept, to_char(age) age, line ";
            sql_print = sql_print + Environment.NewLine + "from cs_employees a ";
            sql_print = sql_print + Environment.NewLine + "join cs_audiometri b on (a.empid=b.empid) ";
            sql_print = sql_print + Environment.NewLine + "where a.empid='" + s_nik + "' ";


            OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra3 = new OleDbDataAdapter(sql_print, oraConnect3);
            DataTable dt3 = new DataTable();
            adOra3.Fill(dt3);

            dsAudiometri.Tables.Clear();
            dsAudiometri.Tables.Add(dt3);

            reportAudiometri = new ReportAudiometri(dsAudiometri);
            reportAudiometri.CreateDocument();
            //reportAudiometri.ShowPreviewDialog();
            documentViewer1.DocumentSource = reportAudiometri;

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

        private void btnDownload_Click(object sender, EventArgs e)
        {
            reportAudiometri.ShowPreviewDialog();
        }
    }
}