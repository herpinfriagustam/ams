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
    public partial class ObservationList : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";

        public ObservationList()
        {
            InitializeComponent();
        }

        private void ObservationList_Load(object sender, EventArgs e)
        {
            LoadData();
            dObsDate.Text = today;
        }

        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            LoadData();
            btnObsCls.Enabled = false;
        }

        private void LoadData()
        {
            string sql_select_room = "";

            sql_select_room = sql_select_room + Environment.NewLine + "select a.rm_no, c.que01, to_char(c.visit_date,'yyyy-mm-dd') visit_date, b.obs_id,    ";
            sql_select_room = sql_select_room + Environment.NewLine + "b.room_cd, a.empid, (select name from cs_employees where empid = a.empid ) nama,  ";
            sql_select_room = sql_select_room + Environment.NewLine + "to_char(b.obs_start, 'yyyy-mm-dd') obs_date,    ";
            sql_select_room = sql_select_room + Environment.NewLine + "to_char(b.obs_start, 'hh24:mi:ss') obs_start, hrs_cnt,   ";
            sql_select_room = sql_select_room + Environment.NewLine + "round(24 * (nvl(b.obs_end, sysdate) - b.obs_start), 2) durasi,   ";
            sql_select_room = sql_select_room + Environment.NewLine + "case when round(24 * (nvl(b.obs_end, sysdate) - b.obs_start), 2) > hrs_cnt and obs_end is null then 'Over'   ";
            sql_select_room = sql_select_room + Environment.NewLine + "when round(24 * (nvl(b.obs_end, sysdate) - b.obs_start), 2) <= hrs_cnt and obs_end is null then 'On Observation' else 'Close' end stat,   ";
            sql_select_room = sql_select_room + Environment.NewLine + "to_char(b.obs_end, 'hh24:mi:ss') obs_end, b.obs_remark,  ";
            sql_select_room = sql_select_room + Environment.NewLine + "(select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa from cs_diagnosa a  ";
            sql_select_room = sql_select_room + Environment.NewLine + "join cs_diagnosa_item b on (a.item_cd=b.item_cd)  ";
            sql_select_room = sql_select_room + Environment.NewLine + "where b.status='A'  ";
            sql_select_room = sql_select_room + Environment.NewLine + "and rm_no=a.rm_no  ";
            sql_select_room = sql_select_room + Environment.NewLine + "and insp_date=b.insp_date  ";
            sql_select_room = sql_select_room + Environment.NewLine + "and visit_no=b.visit_no ) diagnosa, d.room_name   ";
            sql_select_room = sql_select_room + Environment.NewLine + "from cs_patient a   ";
            sql_select_room = sql_select_room + Environment.NewLine + "join cs_observation b on (a.rm_no = b.rm_no)   ";
            sql_select_room = sql_select_room + Environment.NewLine + "join cs_visit c on(a.empid = c.empid)   ";
            sql_select_room = sql_select_room + Environment.NewLine + "JOIN cs_room d on (b.room_cd=d.room_id)   ";
            sql_select_room = sql_select_room + Environment.NewLine + "where b.visit_no = c.que01   ";
            sql_select_room = sql_select_room + Environment.NewLine + "and trunc(c.visit_date) = trunc(b.obs_start)  ";
            sql_select_room = sql_select_room + Environment.NewLine + "and a.status = 'A' AND d.status = 'A'   ";
            sql_select_room = sql_select_room + Environment.NewLine + "and to_char(b.insp_date, 'yyyy-mm-dd') = '" + dObsDate.Text + "'   ";
            sql_select_room = sql_select_room + Environment.NewLine + "order by stat desc, durasi desc   ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_select_room, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            gridControl1.DataSource = dt;

            gridView1.OptionsView.ColumnAutoWidth = true;
            gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView1.IndicatorWidth = 30;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.BestFitColumns();

            gridView1.Columns[0].Caption = "RM No";
            gridView1.Columns[1].Caption = "Que";
            gridView1.Columns[2].Caption = "Date";
            gridView1.Columns[3].Caption = "ID";
            gridView1.Columns[4].Caption = "Room ID";
            gridView1.Columns[5].Caption = "NIK";
            gridView1.Columns[6].Caption = "Nama";
            gridView1.Columns[7].Caption = "Tanggal";
            gridView1.Columns[8].Caption = "Jam Mulai";
            gridView1.Columns[9].Caption = "Lama";
            gridView1.Columns[10].Caption = "Durasi";
            gridView1.Columns[11].Caption = "Status";
            gridView1.Columns[12].Caption = "Jam Selesai";
            gridView1.Columns[13].Caption = "Remark";
            gridView1.Columns[14].Caption = "Diagnosa";
            gridView1.Columns[15].Caption = "Ruangan";

            gridView1.Columns[0].Visible = false;
            gridView1.Columns[1].Visible = false;
            gridView1.Columns[2].Visible = false;
            gridView1.Columns[3].Visible = false;
            gridView1.Columns[4].Visible = false;
            //gridView1.Columns[14].Visible = false;

            //gridView1.Columns[5].OptionsColumn.ReadOnly = true;
            //gridView1.Columns[6].OptionsColumn.ReadOnly = true;
            //gridView1.Columns[7].OptionsColumn.ReadOnly = true;
            //gridView1.Columns[8].OptionsColumn.ReadOnly = true;
            //gridView1.Columns[10].OptionsColumn.ReadOnly = true;
            //gridView1.Columns[11].OptionsColumn.ReadOnly = true;
            //gridView1.Columns[12].OptionsColumn.ReadOnly = true;
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Status")
            {
                string stat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);

                if (stat == "On Observation")
                {
                    e.Appearance.BackColor = Color.MediumPurple;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string stat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);

                if (stat == "Over")
                {
                    e.Appearance.BackColor = Color.IndianRed;
                    e.Appearance.BackColor2 = Color.Firebrick;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                    e.HighPriority = true;
                }
            }
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            GridView View = sender as GridView;
            string s_status = "", s_chk = "";

            s_chk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[12]);
            s_status = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);

            if (s_chk == "")
            {
                if (s_status == "Close")
                {
                    btnObsCls.Enabled = false;
                }
                else
                {
                    btnObsCls.Enabled = true;
                }
            }
            else
            {
                btnObsCls.Enabled = false;
            }
            
        }

        private void btnObsCls_Click(object sender, EventArgs e)
        {
            string rm_no = "", que = "", date = "", id = "", nik = "", end_time = "", stat = "", sql_status = "";

            rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
            id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[3]).ToString();
            nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();
            end_time = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[12]).ToString();

            if (end_time != "")
            {
                MessageBox.Show("Data Observasi sudah diclose");
            }
            else
            {
                sql_status = " select decode(time_receipt,null,'OBS','CLS') stat from cs_visit where to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' and empid = '" + nik + "' ";
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_status, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);
                stat = dt.Rows[0]["stat"].ToString();

                OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                OleDbCommand command = new OleDbCommand();
                OleDbTransaction trans = null;

                command.Connection = oraConnectTrans;
                oraConnectTrans.Open();

                try
                {
                    trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                    command.Connection = oraConnectTrans;
                    command.Transaction = trans;

                    command.CommandText = " update cs_observation set obs_end = sysdate,  " +
                                          " upd_date = sysdate, upd_emp = '" + v_empid + "'  " +
                                          " where obs_id = '" + id + "' ";
                    command.ExecuteNonQuery();

                    if (stat == "CLS")
                    {
                        //command.CommandText = " update cs_visit set status = '" + stat + "', time_observation = sysdate, time_end = sysdate, upd_emp = '" + v_empid + "', upd_date = sysdate where empid = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                        command.CommandText = " update cs_visit set status = '" + stat + "', time_observation = sysdate, upd_emp = '" + v_empid + "', upd_date = sysdate where empid = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                    }
                    else
                    {
                        command.CommandText = " update cs_visit set status = '" + stat + "', time_observation = sysdate, upd_emp = '" + v_empid + "', upd_date = sysdate where empid = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
                    }

                    command.ExecuteNonQuery();

                    trans.Commit();
                    //MessageBox.Show(sql_insert);
                    //MessageBox.Show("Query Exec : " + sql_insert);
                    LoadData();
                    MessageBox.Show("Data Berhasil diclose.");
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("ERROR: " + ex.Message);
                }

                oraConnectTrans.Close();
            }
        }
    }
}