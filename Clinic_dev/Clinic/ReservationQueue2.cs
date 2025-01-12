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

namespace Clinic
{
    public partial class ReservationQueue2 : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        int tempTimer = 0, timer = 0, timer_reload = 10, tmp_row_cnt = 0, row_cnt = 0, row_time_cnt=0;


        public ReservationQueue2()
        {
            InitializeComponent();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView2_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void ReservationQueue2_Load(object sender, EventArgs e)
        {
            LoadDataPoli();
            gridView1.FocusedRowHandle = 0;
            LoadDataPasien();
        }

        public void LoadDataPoli()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select poli_cd, ord, poli_name, cnt  ";
            SQL = SQL + Environment.NewLine + "from cs_resv_queue_v ";
            SQL = SQL + Environment.NewLine + "order by 2,3 ";

            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                row_cnt = 0;
                row_cnt = dt.Rows.Count;
                row_time_cnt = row_cnt * timer_reload;

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.Appearance.Row.Font = new Font("Trebuchet MS", 26, FontStyle.Bold);
                gridView1.OptionsBehavior.Editable = false;
                gridView1.IndicatorWidth = 70;

                gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
                gridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
                gridView1.ColumnPanelRowHeight = 50;
                gridView1.Appearance.HeaderPanel.Font = new Font("Trebuchet MS", 28, FontStyle.Bold);
                //gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.False;

                //gridView1.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                //gridView1.Columns[1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                //gridView1.Columns[0].OptionsColumn.AllowEdit = false;
                //gridView1.Columns[1].OptionsColumn.AllowEdit = false;

                gridView1.Columns[2].Caption = "Nama Poli";
                gridView1.Columns[3].Caption = "Jml Antrian";

                //gridView1.BestFitColumns();
                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].Visible = false;

                //RepositoryItemMemoEdit nama = new RepositoryItemMemoEdit();
                //nama.WordWrap = true;
                //gridView1.Columns[1].ColumnEdit = nama;

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        public void LoadDataPasien()
        {
            string SQL = "", nm_poli = "", cd_poli="";

            nm_poli = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
            cd_poli = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();

            SQL = "";

            if (nm_poli == "Poli Umum")
            {
                SQL = "";
                SQL = SQL + Environment.NewLine + "select que01 nno,  ";
                SQL = SQL + Environment.NewLine + "(select name from cs_patient_info where patient_no=a.patient_no ) name, status  ";
                SQL = SQL + Environment.NewLine + "from cs_visit a  ";
                SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm-dd')= to_char(sysdate,'yyyy-mm-dd') ";
                SQL = SQL + Environment.NewLine + "and poli_cd in ('POL0001','POL0000') ";
                SQL = SQL + Environment.NewLine + "and status in ('PRE','RSV','NUR','INS')   ";
                SQL = SQL + Environment.NewLine + "order by que01 asc ";
            }
            else if (nm_poli == "Poli Obgyn")
            {
                SQL = "";
                SQL = SQL + Environment.NewLine + "select que01 nno,  ";
                SQL = SQL + Environment.NewLine + "(select name from cs_patient_info where patient_no=a.patient_no ) name, status  ";
                SQL = SQL + Environment.NewLine + "from cs_visit a  ";
                SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm-dd')= to_char(sysdate,'yyyy-mm-dd') ";
                SQL = SQL + Environment.NewLine + "and poli_cd in ('POL0002','POL0003') ";
                SQL = SQL + Environment.NewLine + "and status in ('PRE','RSV','NUR','INS')   ";
                SQL = SQL + Environment.NewLine + "order by que01 asc ";
            }
            else if (nm_poli == "Obat")
            {
                SQL = "";
                SQL = SQL + Environment.NewLine + "select que02 nno,  ";
                SQL = SQL + Environment.NewLine + "(select name from cs_patient_info where patient_no=a.patient_no ) name, status  ";
                SQL = SQL + Environment.NewLine + "from cs_visit a  ";
                SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm-dd')= to_char(sysdate,'yyyy-mm-dd') ";
                SQL = SQL + Environment.NewLine + "and status in ('MED')    ";
                SQL = SQL + Environment.NewLine + "order by que02 asc ";
            }
            else
            {
                SQL = "";
                SQL = SQL + Environment.NewLine + "select que01 nno,  ";
                SQL = SQL + Environment.NewLine + "(select name from cs_patient_info where patient_no=a.patient_no ) name, status  ";
                SQL = SQL + Environment.NewLine + "from cs_visit a  ";
                SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm-dd')= to_char(sysdate,'yyyy-mm-dd') ";
                SQL = SQL + Environment.NewLine + "and poli_cd in ('" + cd_poli + "') ";
                SQL = SQL + Environment.NewLine + "and status in ('PRE','RSV','NUR','INS')   ";
                SQL = SQL + Environment.NewLine + "order by que01 asc ";
            }

            

            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl2.DataSource = null;
                gridView2.Columns.Clear();
                gridControl2.DataSource = dt;

                gridView2.OptionsView.ColumnAutoWidth = true;
                gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView2.Appearance.Row.Font = new Font("Trebuchet MS", 26, FontStyle.Bold);
                gridView2.OptionsBehavior.Editable = false;
                gridView2.IndicatorWidth = 70;

                gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
                gridView2.OptionsSelection.EnableAppearanceFocusedRow = false;
                gridView2.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
                gridView2.ColumnPanelRowHeight = 50;
                gridView2.Appearance.HeaderPanel.Font = new Font("Trebuchet MS", 28, FontStyle.Bold);
                //gridView2.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
                //gridView2.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
                //gridView2.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.False;

                //gridView2.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                //gridView2.Columns[1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                //gridView2.Columns[0].OptionsColumn.AllowEdit = false;
                //gridView2.Columns[1].OptionsColumn.AllowEdit = false;

                gridView2.Columns[0].Caption = "Antrian";
                gridView2.Columns[1].Caption = "Nama pasien";
                gridView2.Columns[2].Caption = "Status";

                //gridView2.BestFitColumns();
                gridView2.Columns[0].Visible = false;
                gridView2.Columns[2].Visible = false;

                //RepositoryItemMemoEdit nama = new RepositoryItemMemoEdit();
                //nama.WordWrap = true;
                //gridView1.Columns[1].ColumnEdit = nama;

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }

        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
                if (e.Column.FieldName == "NNO" || e.Column.FieldName == "NAME")
                {
                    string info = view.GetRowCellDisplayText(e.RowHandle, view.Columns[2]);
                    if (info == "INS")
                    {
                        e.Appearance.BackColor = Color.DodgerBlue; // Crimson
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.Font = new Font("Trebuchet MS", 27, FontStyle.Bold);
                    }

                }

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tempTimer = timer_reload;

            timer++;
            if (timer == ((tmp_row_cnt + 1) * tempTimer))
            {
                tmp_row_cnt = tmp_row_cnt + 1;
                gridView1.FocusedRowHandle = tmp_row_cnt;
                LoadDataPasien();
                //timer = 0; tempTimer = 0;
            }
            else if (timer > row_time_cnt)
            {
                timer = 0; tempTimer = 0; row_cnt = 0; tmp_row_cnt = 0;
                gridView1.FocusedRowHandle = 0;
                LoadDataPasien();
            }
            else
            {
                ltimer.Text = timer.ToString();
            }
        }
    }
}