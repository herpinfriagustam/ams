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
using DevExpress.XtraGrid;

namespace Clinic
{
    public partial class PregnantReport : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Status> listStat = new List<Status>();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM");
        //string today = "2019-11-27";
        string sdate = "";

        public PregnantReport()
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


            string sql_date="";
            sql_date = " select to_char(trunc(sysdate,'MM'),'yyyy-mm') sdate  from dual ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_date, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            sdate = dt.Rows[0]["sdate"].ToString();
            dDateBgn.Text = sdate;

            cmbSt.Items.Clear();
            cmbSt.Items.Add("");
            cmbSt.Items.Add("Aktif");
            cmbSt.Items.Add("Sudah Diperiksa");
            cmbSt.Items.Add("Belum Diperiksa");
            cmbSt.SelectedIndex = 0;
        }

        private void LoadData()
        {
            string SQL, p_type = "";

            SQL = "";

            if (cmbSt.Text == "Sudah Diperiksa")
            {
                SQL = SQL + Environment.NewLine + "select b.rm_no, a.empid, name, dept, position, info01 hamil_ke, info03 anak_ke, ";
                SQL = SQL + Environment.NewLine + "info04 gpa, info05 hpht,  ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K1'),'yyyy-mm-dd') k1, ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K2'),'yyyy-mm-dd') k2, ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K3'),'yyyy-mm-dd') k3, ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K4'),'yyyy-mm-dd') k4, ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K5'),'yyyy-mm-dd') k5, ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K6'),'yyyy-mm-dd') k6, ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K7'),'yyyy-mm-dd') k7, ";
                SQL = SQL + Environment.NewLine + "info07 taksiran, info08 tgl_ambil,  info09 mulai_cuti, info10 selesai_cuti ";
                SQL = SQL + Environment.NewLine + "from cs_employees a ";
                SQL = SQL + Environment.NewLine + "join cs_patient b ";
                SQL = SQL + Environment.NewLine + "on a.empid=b.empid ";
                SQL = SQL + Environment.NewLine + "where 1=1 ";
                SQL = SQL + Environment.NewLine + "and group_patient='PREG' ";
                SQL = SQL + Environment.NewLine + "and a.empid in (select distinct empid  ";
                SQL = SQL + Environment.NewLine + "from cs_visit ";
                SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm')='" + dDateBgn.Text + "' ";
                SQL = SQL + Environment.NewLine + "and poli_cd='POL0002' ";
                SQL = SQL + Environment.NewLine + "and status='CLS') ";
            }
            else if (cmbSt.Text == "Belum Diperiksa")
            {
                SQL = SQL + Environment.NewLine + "select rm_no, empid, name, dept, position, hamil_ke, anak_ke,  ";
                SQL = SQL + Environment.NewLine + "gpa, hpht, k1, k2, k3, k4, k5, k6, k7,  ";
                SQL = SQL + Environment.NewLine + "taksiran, tgl_ambil, mulai_cuti, selesai_cuti, aktif ";
                SQL = SQL + Environment.NewLine + "from ( ";
                SQL = SQL + Environment.NewLine + "select b.rm_no, a.empid, name, dept, position, info01 hamil_ke, info03 anak_ke,  ";
                SQL = SQL + Environment.NewLine + "info04 gpa, info05 hpht,   ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K1'),'yyyy-mm-dd') k1,  ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K2'),'yyyy-mm-dd') k2,  ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K3'),'yyyy-mm-dd') k3,  ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K4'),'yyyy-mm-dd') k4,  ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K5'),'yyyy-mm-dd') k5,  ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K6'),'yyyy-mm-dd') k6,  ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K7'),'yyyy-mm-dd') k7,  ";
                SQL = SQL + Environment.NewLine + "info07 taksiran, info08 tgl_ambil,  info09 mulai_cuti, info10 selesai_cuti, ";
                SQL = SQL + Environment.NewLine + "CASE WHEN trunc(sysdate) BETWEEN TO_DATE (info05, 'yyyy-mm-dd') ";
                SQL = SQL + Environment.NewLine + "                             AND TO_DATE (info09, 'yyyy-mm-dd') ";
                SQL = SQL + Environment.NewLine + "     THEN 'A' ";
                SQL = SQL + Environment.NewLine + "     ELSE 'I' ";
                SQL = SQL + Environment.NewLine + "      END as aktif ";
                SQL = SQL + Environment.NewLine + "from cs_employees a  ";
                SQL = SQL + Environment.NewLine + "join cs_patient b  ";
                SQL = SQL + Environment.NewLine + "on a.empid=b.empid  ";
                SQL = SQL + Environment.NewLine + "where 1=1  ";
                SQL = SQL + Environment.NewLine + "and group_patient='PREG' ) a ";
                SQL = SQL + Environment.NewLine + "where 1=1 ";
                SQL = SQL + Environment.NewLine + "and aktif='A' ";
                SQL = SQL + Environment.NewLine + "and empid not in (select distinct empid  ";
                SQL = SQL + Environment.NewLine + "from cs_visit ";
                SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm')='" + dDateBgn.Text + "' ";
                SQL = SQL + Environment.NewLine + "and poli_cd='POL0002' ";
                SQL = SQL + Environment.NewLine + "and status='CLS') ";
            }
            else if (cmbSt.Text == "Aktif")
            {
                SQL = SQL + Environment.NewLine + "select rm_no, empid, name, dept, position, hamil_ke, anak_ke,  ";
                SQL = SQL + Environment.NewLine + "gpa, hpht, k1, k2, k3, k4, k5, k6, k7,  ";
                SQL = SQL + Environment.NewLine + "taksiran, tgl_ambil, mulai_cuti, selesai_cuti, aktif ";
                SQL = SQL + Environment.NewLine + "from ( ";
                SQL = SQL + Environment.NewLine + "select b.rm_no, a.empid, name, dept, position, info01 hamil_ke, info03 anak_ke,  ";
                SQL = SQL + Environment.NewLine + "info04 gpa, info05 hpht,   ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K1'),'yyyy-mm-dd') k1,  ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K2'),'yyyy-mm-dd') k2,  ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K3'),'yyyy-mm-dd') k3,  ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K4'),'yyyy-mm-dd') k4,  ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K5'),'yyyy-mm-dd') k5,  ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K6'),'yyyy-mm-dd') k6,  ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K7'),'yyyy-mm-dd') k7,  ";
                SQL = SQL + Environment.NewLine + "info07 taksiran, info08 tgl_ambil,  info09 mulai_cuti, info10 selesai_cuti, ";
                SQL = SQL + Environment.NewLine + "CASE WHEN trunc(sysdate) BETWEEN TO_DATE (info05, 'yyyy-mm-dd') ";
                SQL = SQL + Environment.NewLine + "                             AND TO_DATE (info09, 'yyyy-mm-dd') ";
                SQL = SQL + Environment.NewLine + "     THEN 'A' ";
                SQL = SQL + Environment.NewLine + "     ELSE 'I' ";
                SQL = SQL + Environment.NewLine + "      END as aktif ";
                SQL = SQL + Environment.NewLine + "from cs_employees a  ";
                SQL = SQL + Environment.NewLine + "join cs_patient b  ";
                SQL = SQL + Environment.NewLine + "on a.empid=b.empid  ";
                SQL = SQL + Environment.NewLine + "where 1=1  ";
                SQL = SQL + Environment.NewLine + "and group_patient='PREG' ) a ";
                SQL = SQL + Environment.NewLine + "where 1=1 ";
                SQL = SQL + Environment.NewLine + "and aktif='A' ";

            }
            else
            {
                SQL = SQL + Environment.NewLine + "select b.rm_no, a.empid, name, dept, position, info01 hamil_ke, info03 anak_ke, ";
                SQL = SQL + Environment.NewLine + "info04 gpa, info05 hpht,  ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K1'),'yyyy-mm-dd') k1, ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K2'),'yyyy-mm-dd') k2, ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K3'),'yyyy-mm-dd') k3, ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K4'),'yyyy-mm-dd') k4, ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K5'),'yyyy-mm-dd') k5, ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K6'),'yyyy-mm-dd') k6, ";
                SQL = SQL + Environment.NewLine + "to_char(TTIT.CS_GET_INFOK(b.rm_no,'K7'),'yyyy-mm-dd') k7, ";
                SQL = SQL + Environment.NewLine + "info07 taksiran, info08 tgl_ambil,  info09 mulai_cuti, info10 selesai_cuti ";
                SQL = SQL + Environment.NewLine + "from cs_employees a ";
                SQL = SQL + Environment.NewLine + "join cs_patient b ";
                SQL = SQL + Environment.NewLine + "on a.empid=b.empid ";
                SQL = SQL + Environment.NewLine + "where 1=1 ";
                SQL = SQL + Environment.NewLine + "and group_patient='PREG' ";
            }
            


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

                gridView1.FixedLineWidth = 5;
                gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                //gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 60;
                gridView1.OptionsBehavior.Editable = false;
                

                gridView1.Columns[0].Caption = "RM No";
                gridView1.Columns[1].Caption = "NIK";
                gridView1.Columns[2].Caption = "Nama";
                gridView1.Columns[3].Caption = "Departemen";
                gridView1.Columns[4].Caption = "Posisi";
                gridView1.Columns[5].Caption = "Hamil Ke";
                gridView1.Columns[6].Caption = "Anak Ke";
                gridView1.Columns[7].Caption = "GPA";
                gridView1.Columns[8].Caption = "HPHT";
                gridView1.Columns[9].Caption = "K1";
                gridView1.Columns[10].Caption = "K2";
                gridView1.Columns[11].Caption = "K3";
                gridView1.Columns[12].Caption = "K4";
                gridView1.Columns[13].Caption = "K5";
                gridView1.Columns[14].Caption = "K6";
                gridView1.Columns[15].Caption = "K7";
                gridView1.Columns[16].Caption = "Taksiran";
                gridView1.Columns[17].Caption = "Tgl Ambil";
                gridView1.Columns[18].Caption = "Mulai Cuti";
                gridView1.Columns[19].Caption = "Selesai Cuti";

                //gridView1.Columns[0].Width = 80;
                //gridView1.Columns[1].Width = 150;

                //gridView1.Columns[0].Visible= false;

                gridView1.Columns[2].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

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

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            //if (e.Column.Caption == "Stok Awal")
            //{
            //    string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[4]);
            //    if (kk == "Y")
            //    {
            //        e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
            //        e.Appearance.BackColor2 = Color.FromArgb(150, Color.OrangeRed);
            //        e.Appearance.ForeColor = Color.White;
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //    }
            //}

            //if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Satuan")
            //{

            //}
            //else if (e.Column.Caption == "Stok Awal")
            //{
            //    e.Appearance.BackColor = Color.OldLace;
            //    e.Appearance.ForeColor = Color.Black;
            //    e.Appearance.FontStyleDelta = FontStyle.Bold;
            //}
            //else if (e.Column.Caption == "Tgl 1-15" || e.Column.Caption == "Tgl 16-31")
            //{
            //    e.Appearance.BackColor = Color.FromArgb(100, Color.AliceBlue);
            //    e.Appearance.ForeColor = Color.Black;
            //    //e.Appearance.FontStyleDelta = FontStyle.Bold;
            //}
            //else if (e.Column.Caption == "Stok Masuk")
            //{
            //    e.Appearance.BackColor = Color.FromArgb(100, Color.AliceBlue);
            //    e.Appearance.ForeColor = Color.Black;
            //    e.Appearance.FontStyleDelta = FontStyle.Bold;
            //}
            //else if (e.Column.Caption == "Stok Keluar")
            //{
            //    e.Appearance.BackColor = Color.FromArgb(100, Color.MistyRose);
            //    e.Appearance.ForeColor = Color.Black;
            //    e.Appearance.FontStyleDelta = FontStyle.Bold;
            //}
            //else if (e.Column.Caption == "Stok Saat Ini")
            //{
            //    string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[39]);

            //    if (e.RowHandle >= 0)
            //    {
            //        if (Convert.ToInt16(kk) <= 0)
            //        {
            //            e.Appearance.BackColor = Color.Crimson;
            //            e.Appearance.ForeColor = Color.White;
            //            e.Appearance.FontStyleDelta = FontStyle.Bold;
            //        }
            //        else if (Convert.ToInt16(kk) <= 20)
            //        {
            //            e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
            //            e.Appearance.ForeColor = Color.White;
            //            e.Appearance.FontStyleDelta = FontStyle.Bold;
            //        }
            //        else
            //        {
            //            e.Appearance.BackColor = Color.OldLace;
            //            e.Appearance.ForeColor = Color.Black;
            //        }
            //    }
            //}
            //else 
            //{
            //    e.Appearance.BackColor = Color.FromArgb(100, Color.MistyRose);
            //    e.Appearance.ForeColor = Color.Black;
            //    //e.Appearance.FontStyleDelta = FontStyle.Bold;
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
                    FileName = "pregnant_report.xls",
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

        private void cmbSt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSt.Text == "Sudah Diperiksa" || cmbSt.Text == "Belum Diperiksa")
            {
                dDateBgn.Enabled = true;
            }
            else
            {
                dDateBgn.Enabled = false;
            }
        }
    }
}