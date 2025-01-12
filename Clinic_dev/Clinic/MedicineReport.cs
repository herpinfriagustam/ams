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
    public partial class MedicineReport : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Status> listStat = new List<Status>();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM");
        //string today = "2019-11-27";
        string sdate = "";

        public MedicineReport()
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
        }

        private void LoadData()
        {
            string SQL, p_type = "";
            

            SQL = "";
            SQL = SQL + Environment.NewLine + "select x.*, nvl(stok_in,0) - nvl(stok_out,0) as saldo from  ";
            SQL = SQL + Environment.NewLine + "(select a.*, m01,m02, nvl(stok_awal,0) + nvl(m01,0) + nvl(m02,0) as stok_in,  ";
            SQL = SQL + Environment.NewLine + "d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12, d13, d14, d15, ";
            SQL = SQL + Environment.NewLine + "d16, d17, d18, d19, d20, d21, d22, d23, d24, d25,d26, d27, d28, d29, d30, d31, ";
            SQL = SQL + Environment.NewLine + "nvl(d1,0)+nvl(d2,0)+nvl(d3,0)+nvl(d4,0)+nvl(d5,0)+nvl(d6,0)+nvl(d7,0)+nvl(d8,0)+ ";
            SQL = SQL + Environment.NewLine + "nvl(d9,0)+nvl(d10,0)+nvl(d11,0)+nvl(d12,0)+nvl(d13,0)+nvl(d14,0)+nvl(d15,0)+nvl(d16,0)+ ";
            SQL = SQL + Environment.NewLine + "nvl(d17,0)+nvl(d18,0)+nvl(d19,0)+nvl(d20,0)+nvl(d21,0)+nvl(d22,0)+nvl(d23,0)+nvl(d24,0)+ ";
            SQL = SQL + Environment.NewLine + "nvl(d25,0)+nvl(d26,0)+nvl(d27,0)+nvl(d28,0)+nvl(d29,0)+nvl(d30,0)+nvl(d31,0) as stok_out ";
            SQL = SQL + Environment.NewLine + " from  ";
            SQL = SQL + Environment.NewLine + "(select med_cd, initcap(med_name) med_name, initcap(uom) uom,  ";
            SQL = SQL + Environment.NewLine + "klinik.FN_CS_INIT_STOCK(to_date('" + dDateBgn.Text + "','yyyy-mm'),med_cd) as stok_awal ";
            SQL = SQL + Environment.NewLine + "from cs_medicine ";
            SQL = SQL + Environment.NewLine + "where status='A') a ";
            SQL = SQL + Environment.NewLine + "left join  ";
            SQL = SQL + Environment.NewLine + "(select med_cd, med_name,nvl(d1,0)+nvl(d2,0)+nvl(d3,0)+nvl(d4,0)+nvl(d5,0)+ ";
            SQL = SQL + Environment.NewLine + "nvl(d6,0)+nvl(d7,0)+nvl(d8,0)+nvl(d9,0)+nvl(d10,0)+ ";
            SQL = SQL + Environment.NewLine + "nvl(d11,0)+nvl(d12,0)+nvl(d13,0)+nvl(d14,0)+nvl(d15,0) as m01, ";
            SQL = SQL + Environment.NewLine + "nvl(d16,0)+nvl(d17,0)+nvl(d18,0)+nvl(d19,0)+nvl(d20,0)+ ";
            SQL = SQL + Environment.NewLine + "nvl(d21,0)+nvl(d22,0)+nvl(d23,0)+nvl(d24,0)+nvl(d25,0)+ ";
            SQL = SQL + Environment.NewLine + "nvl(d26,0)+nvl(d27,0)+nvl(d28,0)+nvl(d29,0)+nvl(d30,0)+nvl(d31,0) as m02 ";
            SQL = SQL + Environment.NewLine + "from ( ";
            SQL = SQL + Environment.NewLine + "select * from ( ";
            SQL = SQL + Environment.NewLine + "select a.med_cd, initcap(med_name) med_name,  ";
            SQL = SQL + Environment.NewLine + "to_char(trans_date,'dd') tgl, trans_qty ";
            SQL = SQL + Environment.NewLine + "from cs_medicine a ";
            SQL = SQL + Environment.NewLine + "join cs_medicine_trans b on a.med_cd=b.med_cd ";
            SQL = SQL + Environment.NewLine + "where a.status='A' ";
            SQL = SQL + Environment.NewLine + "and to_char(trans_date,'yyyy-mm')='" + dDateBgn.Text + "' ";
            SQL = SQL + Environment.NewLine + "and trans_type='IN' ) ";
            SQL = SQL + Environment.NewLine + "pivot ";
            SQL = SQL + Environment.NewLine + "( ";
            SQL = SQL + Environment.NewLine + "  sum(trans_qty) ";
            SQL = SQL + Environment.NewLine + "  for tgl IN ('01' as d1,'02' as d2,'03' as d3,'04' as d4,'05' as d5,'06' as d6,'07' as d7,'08' as d8,'09' as d9,'10' as d10, ";
            SQL = SQL + Environment.NewLine + "            '11' as d11,'12' as d12,'13' as d13,'14' as d14,'15' as d15,'16' as d16,'17' as d17,'18' as d18,'19' as d19,'20' as d20, ";
            SQL = SQL + Environment.NewLine + "            '21' as d21,'22' as d22,'23' as d23,'24' as d24,'25' as d25,'26' as d26,'27' as d27,'28' as d28,'29' as d29,'30' as d30,'31' as d31) ";
            SQL = SQL + Environment.NewLine + ") a ";
            SQL = SQL + Environment.NewLine + ")) b on a.med_cd=b.med_cd ";
            SQL = SQL + Environment.NewLine + "left join  ";
            SQL = SQL + Environment.NewLine + "(select * from ( ";
            SQL = SQL + Environment.NewLine + "select a.med_cd, initcap(med_name) med_name,  ";
            SQL = SQL + Environment.NewLine + "to_char(trans_date,'dd') tgl, trans_qty ";
            SQL = SQL + Environment.NewLine + "from cs_medicine a ";
            SQL = SQL + Environment.NewLine + "join cs_medicine_trans b on a.med_cd=b.med_cd ";
            SQL = SQL + Environment.NewLine + "where a.status='A' ";
            SQL = SQL + Environment.NewLine + "and to_char(trans_date,'yyyy-mm')='" + dDateBgn.Text + "' ";
            SQL = SQL + Environment.NewLine + "and trans_type='OUT' ) ";
            SQL = SQL + Environment.NewLine + "pivot ";
            SQL = SQL + Environment.NewLine + "( ";
            SQL = SQL + Environment.NewLine + "  sum(trans_qty) ";
            SQL = SQL + Environment.NewLine + "  for tgl IN ('01' as d1,'02' as d2,'03' as d3,'04' as d4,'05' as d5,'06' as d6,'07' as d7,'08' as d8,'09' as d9,'10' as d10, ";
            SQL = SQL + Environment.NewLine + "            '11' as d11,'12' as d12,'13' as d13,'14' as d14,'15' as d15,'16' as d16,'17' as d17,'18' as d18,'19' as d19,'20' as d20, ";
            SQL = SQL + Environment.NewLine + "            '21' as d21,'22' as d22,'23' as d23,'24' as d24,'25' as d25,'26' as d26,'27' as d27,'28' as d28,'29' as d29,'30' as d30,'31' as d31) ";
            SQL = SQL + Environment.NewLine + ")) c on a.med_cd=c.med_cd) x ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "order by 2 asc ";



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

                gridView1.FixedLineWidth = 7;
                gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[5].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[6].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                //gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 60;
                gridView1.OptionsBehavior.Editable = false;
                

                gridView1.Columns[0].Caption = "Kode";
                gridView1.Columns[1].Caption = "Nama Obat";
                gridView1.Columns[2].Caption = "Satuan";
                gridView1.Columns[3].Caption = "Stok Awal";
                gridView1.Columns[4].Caption = "Tgl 1-15";
                gridView1.Columns[5].Caption = "Tgl 16-31";
                gridView1.Columns[6].Caption = "Stok Masuk";
                gridView1.Columns[7].Caption = "1";
                gridView1.Columns[8].Caption = "2";
                gridView1.Columns[9].Caption = "3";
                gridView1.Columns[10].Caption = "4";
                gridView1.Columns[11].Caption = "5";
                gridView1.Columns[12].Caption = "6";
                gridView1.Columns[13].Caption = "7";
                gridView1.Columns[14].Caption = "8";
                gridView1.Columns[15].Caption = "9";
                gridView1.Columns[16].Caption = "10";
                gridView1.Columns[17].Caption = "11";
                gridView1.Columns[18].Caption = "12";
                gridView1.Columns[19].Caption = "13";
                gridView1.Columns[20].Caption = "14";
                gridView1.Columns[21].Caption = "15";
                gridView1.Columns[22].Caption = "16";
                gridView1.Columns[23].Caption = "17";
                gridView1.Columns[24].Caption = "18";
                gridView1.Columns[25].Caption = "19";
                gridView1.Columns[26].Caption = "20";
                gridView1.Columns[27].Caption = "21";
                gridView1.Columns[28].Caption = "22";
                gridView1.Columns[29].Caption = "23";
                gridView1.Columns[30].Caption = "24";
                gridView1.Columns[31].Caption = "25";
                gridView1.Columns[32].Caption = "26";
                gridView1.Columns[33].Caption = "27";
                gridView1.Columns[34].Caption = "28";
                gridView1.Columns[35].Caption = "29";
                gridView1.Columns[36].Caption = "30";
                gridView1.Columns[37].Caption = "31";
                gridView1.Columns[38].Caption = "Stok Keluar";
                gridView1.Columns[39].Caption = "Stok Saat Ini";

                //gridView1.Columns[0].Width = 80;
                //gridView1.Columns[1].Width = 150;
                //gridView1.Columns[2].Width = 150;
                //gridView1.Columns[3].Width = 50;
                //gridView1.Columns[4].Width = 40;
                //gridView1.Columns[5].Width = 50;
                //gridView1.Columns[6].Width = 80;
                //gridView1.Columns[7].Width = 250;
                //gridView1.Columns[8].Width = 250;
                //gridView1.Columns[9].Width = 250;
                //gridView1.Columns[10].Width = 60;
                //gridView1.Columns[11].Width = 150;
                gridView1.Columns[0].Visible= false;

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
            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Satuan")
            {

            }
            else if (e.Column.Caption == "Stok Awal")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
                e.Appearance.FontStyleDelta = FontStyle.Bold;
            }
            else if (e.Column.Caption == "Tgl 1-15" || e.Column.Caption == "Tgl 16-31")
            {
                e.Appearance.BackColor = Color.FromArgb(100, Color.AliceBlue);
                e.Appearance.ForeColor = Color.Black;
                //e.Appearance.FontStyleDelta = FontStyle.Bold;
            }
            else if (e.Column.Caption == "Stok Masuk")
            {
                e.Appearance.BackColor = Color.FromArgb(100, Color.AliceBlue);
                e.Appearance.ForeColor = Color.Black;
                e.Appearance.FontStyleDelta = FontStyle.Bold;
            }
            else if (e.Column.Caption == "Stok Keluar")
            {
                e.Appearance.BackColor = Color.FromArgb(100, Color.MistyRose);
                e.Appearance.ForeColor = Color.Black;
                e.Appearance.FontStyleDelta = FontStyle.Bold;
            }
            else if (e.Column.Caption == "Stok Saat Ini")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[39]);

                if (e.RowHandle >= 0)
                {
                    if (Convert.ToInt16(kk) <= 0)
                    {
                        e.Appearance.BackColor = Color.Crimson;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (Convert.ToInt16(kk) <= 20)
                    {
                        e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.OldLace;
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
            }
            else 
            {
                e.Appearance.BackColor = Color.FromArgb(100, Color.MistyRose);
                e.Appearance.ForeColor = Color.Black;
                //e.Appearance.FontStyleDelta = FontStyle.Bold;
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
                    FileName = "medicine_report.xls",
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