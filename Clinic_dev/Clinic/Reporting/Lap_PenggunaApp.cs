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
    public partial class Lap_PenggunaApp : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Status> listStat = new List<Status>(); List<Status> listStat2 = new List<Status>();
        List<PatientType> listPatientType = new List<PatientType>();
        List<WorkAccident> listWorkAccident = new List<WorkAccident>();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string type = "", sdate="", edate="";

        public Lap_PenggunaApp()
        {
            InitializeComponent();
        }

        private void ObservationList_Load(object sender, EventArgs e)
        {
            InitData();
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "Lap_PenggunaApp");
            //LoadData();
        }

        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void InitData()
        {  
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
   
            string Sql = "";
            Sql = Sql + Environment.NewLine + " ";
            Sql = Sql + Environment.NewLine + "select DISTINCT to_char(S_DATE,'YYYY-MM-DD') TANGGAL, b.NAME, c.CODE_NAME Bagian,  D.CODE_NAME NAMA_MENU,  ";
            Sql = Sql + Environment.NewLine + "       to_char(S_DATE,'HH:mm:dd') SJAM, to_char(E_DATE,'HH:mm:dd') EJAM, A.IP_KOMPUTER ";
            Sql = Sql + Environment.NewLine + "  from CS_HISTORY_LOGIN a, ";
            Sql = Sql + Environment.NewLine + "       cs_user b, ";
            Sql = Sql + Environment.NewLine + "       cs_code_data c, ";
            Sql = Sql + Environment.NewLine + "       cs_code_data D ";
            Sql = Sql + Environment.NewLine + " where a.USER_ID       = b.USER_ID ";
            Sql = Sql + Environment.NewLine + "   and b.USER_ROLE     = c.CODE_ID ";
            Sql = Sql + Environment.NewLine + "   AND A.F_MENU        = D.CODE_ID AND a.USER_ID  <> 'admin'";
            Sql = Sql + Environment.NewLine + "   and c.CODE_CLASS_ID = 'USER_ROLE'      ";
            Sql = Sql + Environment.NewLine + "   and D.CODE_CLASS_ID = 'MENU_FORM'     ";
            Sql = Sql + Environment.NewLine + "   and trunc(A.S_DATE) between to_date('" + dDateBgn.Text + "','yyyy-mm-dd') and to_date('" + dDateEnd.Text + "','yyyy-mm-dd')  ";
            Sql = Sql + Environment.NewLine + "ORDER BY 1,4,6    ";



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
                gridView1.IndicatorWidth = 50;
                gridView1.OptionsBehavior.Editable = false;

                //gridView1.FixedLineWidth = 2;
                //gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView1.Columns[0].Caption = "TANGGAL";  
                gridView1.Columns[1].Caption = "NAMA USER";
                gridView1.Columns[2].Caption = "BAGIAN";
                gridView1.Columns[3].Caption = "MENU AKSES";
                gridView1.Columns[4].Caption = "JAM AWAL";
                gridView1.Columns[5].Caption = "JAM AKHIR"; 
                gridView1.Columns[6].Caption = "IP KOMPUTER";  

                gridView1.BestFitColumns();
                //gridView1.Columns[18].Width = 100;

                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                 
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