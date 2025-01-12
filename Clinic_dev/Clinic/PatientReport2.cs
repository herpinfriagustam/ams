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
    public partial class PatientReport2 : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Status> listStat = new List<Status>();
        DataSet dsMRRanap = new DataSet();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string pub_rm_no = "", pub_que = "", pub_reg_date = "";
        //string today = "2019-11-27";
        string type = "";
        ReportMRRanap reportRanap = null;

        public PatientReport2()
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
            lRanapNm.Text = "-";
            lRanapAge.Text = "-";
            lRanapRm.Text = "-";
            lRanapRoom.Text = "-";
            gridControl2.DataSource = null;
        }

        private void InitData()
        {
            dStartDt.Text = today;
            dEndDt.Text = today;
        }

        private void LoadData()
        {
            string SQL, p_type = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select a.inpatient_id, a.rm_no, d.que01, to_char(reg_date, 'yyyy-mm-dd') reg_date,  ";
            SQL = SQL + Environment.NewLine + "c.name, a.room_id, round(a.date_out-a.date_in) days, ";
            SQL = SQL + Environment.NewLine + "c.birth_place || ', ' || birth_date || ' (' || round(((sysdate-birth_date)/30)/12) || ' tahun)' as ttl ";
            SQL = SQL + Environment.NewLine + "from cs_inpatient a ";
            SQL = SQL + Environment.NewLine + "join cs_patient b on (a.rm_no=b.rm_no) ";
            SQL = SQL + Environment.NewLine + "join cs_patient_info c on (b.patient_no=c.patient_no) ";
            SQL = SQL + Environment.NewLine + "join cs_visit d on (a.inpatient_id=d.inpatient_id) ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and reg_date between to_date('" + dStartDt.Text + "','yyyy-mm-dd') and to_date('" + dEndDt.Text + "','yyyy-mm-dd') ";
            SQL = SQL + Environment.NewLine + "order by 4, 6";

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

                gridView1.Columns[0].Caption = "ID";
                gridView1.Columns[1].Caption = "No RM";
                gridView1.Columns[2].Caption = "Antrian";
                gridView1.Columns[3].Caption = "Tanggal";
                gridView1.Columns[4].Caption = "Nama";
                gridView1.Columns[5].Caption = "Kode Ruangan";
                gridView1.Columns[6].Caption = "Lama Hari";
                gridView1.Columns[7].Caption = "TTL";

                gridView1.Columns[3].Width = 80;
                gridView1.Columns[4].Width = 150;
                gridView1.Columns[5].Width = 100;
                gridView1.Columns[6].Width = 100;

                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].Visible = false;
                gridView1.Columns[2].Visible = false;
                gridView1.Columns[7].Visible = false;

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
            string sql_mr_load = "", s_nama = "", s_age="", s_room="";


            pub_rm_no = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
            pub_que = View.GetRowCellDisplayText(e.RowHandle, View.Columns[2]);
            pub_reg_date = View.GetRowCellDisplayText(e.RowHandle, View.Columns[3]);
            s_nama = View.GetRowCellDisplayText(e.RowHandle, View.Columns[4]);
            s_age = View.GetRowCellDisplayText(e.RowHandle, View.Columns[7]);
            s_room = View.GetRowCellDisplayText(e.RowHandle, View.Columns[5]);

            lRanapNm.Text = s_nama;
            lRanapAge.Text = s_age;
            lRanapRm.Text = pub_rm_no;
            lRanapRoom.Text = s_room;

            sql_mr_load = "";
            sql_mr_load = sql_mr_load + Environment.NewLine + "select 1 ord, 'Anamnesa' info,CS_DETAIL_INS_VALUE('ANAMNESA','" + pub_rm_no + "',to_date('" + pub_reg_date + "','yyyy-mm-dd'),'" + pub_que + "') as val from dual ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "union  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "select 2 ord, 'Alergi' info,CS_DETAIL_INS_VALUE('ALERGI','" + pub_rm_no + "',to_date('" + pub_reg_date + "','yyyy-mm-dd'),'" + pub_que + "') as val from dual ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "union  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "select 3 ord, 'Riwayat' info,CS_DETAIL_INS_VALUE('RIWAYAT','" + pub_rm_no + "',to_date('" + pub_reg_date + "','yyyy-mm-dd'),'" + pub_que + "') as val from dual ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "union  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "select 4 ord, 'Pemeriksaan Penunjuang' info,CS_DETAIL_INS_VALUE('PENUNJANG','" + pub_rm_no + "',to_date('" + pub_reg_date + "','yyyy-mm-dd'),'" + pub_que + "') as val from dual ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "union  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "select 5 ord, 'Diagnosa' info,CS_DETAIL_INS_VALUE('DIAGNOSA','" + pub_rm_no + "',to_date('" + pub_reg_date + "','yyyy-mm-dd'),'" + pub_que + "') as val from dual ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "union  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "select 6 ord, 'Kondisi Umum' info,CS_DETAIL_INS_VALUE('KONDISI','" + pub_rm_no + "',to_date('" + pub_reg_date + "','yyyy-mm-dd'),'" + pub_que + "') as val from dual ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "union  ";
            sql_mr_load = sql_mr_load + Environment.NewLine + "select 7 ord, 'Pengobatan' info,CS_DETAIL_INS_VALUE('OBAT','" + pub_rm_no + "',to_date('" + pub_reg_date + "','yyyy-mm-dd'),'" + pub_que + "') as val from dual ";



            OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra3 = new OleDbDataAdapter(sql_mr_load, oraConnect3);
            DataTable dt3 = new DataTable();
            adOra3.Fill(dt3);

            gridControl2.DataSource = null;
            gridView2.Columns.Clear();
            gridControl2.DataSource = dt3;

            gridView2.OptionsView.ColumnAutoWidth = true;
            gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView2.IndicatorWidth = 30;
            gridView2.OptionsBehavior.Editable = false;
            //gridView2.BestFitColumns();
            gridView2.OptionsView.RowAutoHeight = true;

            RepositoryItemMemoEdit tgl = new RepositoryItemMemoEdit();
            gridControl2.RepositoryItems.Add(tgl);
            gridView2.Columns[2].ColumnEdit = tgl;

            gridView2.Columns[0].Caption = "No";
            gridView2.Columns[1].Caption = "Perihal";
            gridView2.Columns[2].Caption = "Keterangan";

            gridView2.Columns[0].Visible = false;

            gridView2.BestFitColumns();
            gridView2.Columns[1].Width = 100;

            dsMRRanap.Tables.Clear();
            dsMRRanap.Tables.Add(dt3);

            LoadDataRanapPrint(pub_rm_no, pub_que, pub_reg_date, s_nama, s_age, s_room);
        }

        private void LoadDataRanapPrint(string p_rm, string p_no, string p_vdate, string p_name, string p_age, string p_room)
        {
            string sql_mr_print = "";

            sql_mr_print = "";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select '" + p_rm + "' rm, '" + p_name + "' nama, '" + p_age + "' age, '" + p_room + "' room,  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "ord, info, val from  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "( ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select 1 ord, 'Anamnesa' info,CS_DETAIL_INS_VALUE('ANAMNESA','" + pub_rm_no + "',to_date('" + pub_reg_date + "','yyyy-mm-dd'),'" + pub_que + "') as val from dual ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "union  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select 2 ord, 'Alergi' info,CS_DETAIL_INS_VALUE('ALERGI','" + pub_rm_no + "',to_date('" + pub_reg_date + "','yyyy-mm-dd'),'" + pub_que + "') as val from dual ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "union  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select 3 ord, 'Riwayat' info,CS_DETAIL_INS_VALUE('RIWAYAT','" + pub_rm_no + "',to_date('" + pub_reg_date + "','yyyy-mm-dd'),'" + pub_que + "') as val from dual ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "union  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select 4 ord, 'Pemeriksaan Penunjuang' info,CS_DETAIL_INS_VALUE('PENUNJANG','" + pub_rm_no + "',to_date('" + pub_reg_date + "','yyyy-mm-dd'),'" + pub_que + "') as val from dual ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "union  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select 5 ord, 'Diagnosa' info,CS_DETAIL_INS_VALUE('DIAGNOSA','" + pub_rm_no + "',to_date('" + pub_reg_date + "','yyyy-mm-dd'),'" + pub_que + "') as val from dual ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "union  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select 6 ord, 'Kondisi Umum' info,CS_DETAIL_INS_VALUE('KONDISI','" + pub_rm_no + "',to_date('" + pub_reg_date + "','yyyy-mm-dd'),'" + pub_que + "') as val from dual ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "union  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select 7 ord, 'Pengobatan' info,CS_DETAIL_INS_VALUE('OBAT','" + pub_rm_no + "',to_date('" + pub_reg_date + "','yyyy-mm-dd'),'" + pub_que + "') as val from dual ";
            sql_mr_print = sql_mr_print + Environment.NewLine + ") ";

            OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra3 = new OleDbDataAdapter(sql_mr_print, oraConnect3);
            DataTable dt3 = new DataTable();
            adOra3.Fill(dt3);

            dsMRRanap.Tables.Clear();
            dsMRRanap.Tables.Add(dt3);
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
            ReportMRRanap report = new ReportMRRanap(dsMRRanap);
            report.ShowPreviewDialog();
        }
    }
}