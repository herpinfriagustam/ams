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
    public partial class InPatientReport : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Status> listStat = new List<Status>();
        List<Status> listStat4 = new List<Status>();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM");
        //string today = "2019-11-27";
        string sdate = "", edate = "";

        public InPatientReport()
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
            sql_date = " select to_char(trunc(sysdate,'MM'),'yyyy-mm-dd') sdate, to_char(last_day(sysdate),'yyyy-mm-dd') edate from dual ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_date, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            sdate = dt.Rows[0]["sdate"].ToString();
            edate = dt.Rows[0]["edate"].ToString();
            dDateBgn.Text = sdate;
            dDateEnd.Text = edate;

            listStat4.Clear();
            listStat4.Add(new Status() { statusCode = "", statusName = "All" });
            listStat4.Add(new Status() { statusCode = "B", statusName = "BPJS" });
            listStat4.Add(new Status() { statusCode = "U", statusName = "Umum" });
            listStat4.Add(new Status() { statusCode = "P", statusName = "Perusahaan" });
        }

        private void LoadData()
        {
            string SQL;
            

            SQL = "";
            SQL = SQL + Environment.NewLine + "select to_char(visit_date,'yyyy-mm-dd') visit_date, name, gender, age, address,  ";
            SQL = SQL + Environment.NewLine + "insu_flag, baru, rm_no, adj_flag, dokter_jaga, ugd, admin, visit, sewa_kamar,  ";
            SQL = SQL + Environment.NewLine + "tindakan, keperawatan, obat, bhp, rekam_medis, gizi, paket_ranap, lab, rontgen, ";
            SQL = SQL + Environment.NewLine + "ambulan, akomodasi, total, pendapatan, diagnosa  ";
            SQL = SQL + Environment.NewLine + "from KLINIK.CS_REPORT_RWT_INP_V ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and visit_date between to_date('"+dDateBgn.Text+"','yyyy-mm-dd') ";
            SQL = SQL + Environment.NewLine + "and to_date('" + dDateEnd.Text + "','yyyy-mm-dd') ";
            SQL = SQL + Environment.NewLine + "order by 1,2 ";



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
                

                gridView1.Columns[0].Caption = "Tanggal";
                gridView1.Columns[1].Caption = "Nama";
                gridView1.Columns[2].Caption = "Jenis Kelamin";
                gridView1.Columns[3].Caption = "Umur";
                gridView1.Columns[4].Caption = "Alamat";
                gridView1.Columns[5].Caption = "Tipe";
                gridView1.Columns[6].Caption = "Ket RM";
                gridView1.Columns[7].Caption = "No RM";
                gridView1.Columns[8].Caption = "Status Adj";
                gridView1.Columns[9].Caption = "Dokter Jaga";
                gridView1.Columns[10].Caption = "UGD";
                gridView1.Columns[11].Caption = "Admin";
                gridView1.Columns[12].Caption = "Visit";
                gridView1.Columns[13].Caption = "Sewa Kamar";
                gridView1.Columns[14].Caption = "Tindakan";
                gridView1.Columns[15].Caption = "Keperawatan";
                gridView1.Columns[16].Caption = "Obat";
                gridView1.Columns[17].Caption = "BHP";
                gridView1.Columns[18].Caption = "Rekam Medis";
                gridView1.Columns[19].Caption = "Gizi";
                gridView1.Columns[20].Caption = "Paket Ranap";
                gridView1.Columns[21].Caption = "Lab";
                gridView1.Columns[22].Caption = "Rontgen";
                gridView1.Columns[23].Caption = "Ambulan";
                gridView1.Columns[24].Caption = "Akomodasi";
                gridView1.Columns[25].Caption = "Total";
                gridView1.Columns[26].Caption = "Pendapatan";
                gridView1.Columns[27].Caption = "Diagnosa";

                RepositoryItemLookUpEdit statusLookup4 = new RepositoryItemLookUpEdit();
                statusLookup4.DataSource = listStat4;
                statusLookup4.ValueMember = "statusCode";
                statusLookup4.DisplayMember = "statusName";

                statusLookup4.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup4.DropDownRows = listStat4.Count;
                statusLookup4.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup4.AutoSearchColumnIndex = 1;
                statusLookup4.NullText = "";
                gridView1.Columns[5].ColumnEdit = statusLookup4;

                //gridView1.Columns[11].Width = 150;
                //gridView1.Columns[0].Visible= false;

                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

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

            if (e.Column.Caption == "Tipe")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[5]);
                if (kk == "U")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "B")
                {
                    e.Appearance.BackColor = Color.Green;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else
                {

                }
            }

            if (e.Column.Caption == "Tipe Adj")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);
                if (kk == "Y")
                {
                    e.Appearance.BackColor = Color.Yellow;
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else
                {

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
                    FileName = "laporan_rawat_inap.xls",
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