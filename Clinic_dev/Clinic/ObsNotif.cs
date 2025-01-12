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
using System.Diagnostics;
using System.Data.OleDb;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;

namespace Clinic
{
    public partial class ObsNotif : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        List<Status> listStat = new List<Status>();
        int timer = 0, cek_interval = 180;

        public ObsNotif()
        {
            InitializeComponent();
        }

        private void ObsNotif_Load(object sender, EventArgs e)
        {
            InitData();
            LoadData();
            timerCek.Start();
            SoftBlink(labelControl1, Color.LightPink, Color.Red, 1600, false);
        }

        private async void SoftBlink(Control ctrl, Color c1, Color c2, short CycleTime_ms, bool BkClr)
        {
            var sw = new Stopwatch(); sw.Start();
            short halfCycle = (short)Math.Round(CycleTime_ms * 0.5);
            while (true)
            {
                await Task.Delay(1);
                var n = sw.ElapsedMilliseconds % CycleTime_ms;
                var per = (double)Math.Abs(n - halfCycle) / halfCycle;
                var red = (short)Math.Round((c2.R - c1.R) * per) + c1.R;
                var grn = (short)Math.Round((c2.G - c1.G) * per) + c1.G;
                var blw = (short)Math.Round((c2.B - c1.B) * per) + c1.B;
                var clr = Color.FromArgb(red, grn, blw);
                if (BkClr) ctrl.BackColor = clr; else ctrl.ForeColor = clr;
            }
        }

        private void InitData()
        {
            listStat.Clear();
            listStat.Add(new Status() { statusCode = "A", statusName = "Tambah 1 Jam" });
            listStat.Add(new Status() { statusCode = "F", statusName = "Selesai Obs" });

            luAksi.Properties.DataSource = listStat;
            luAksi.Properties.ValueMember = "statusCode";
            luAksi.Properties.DisplayMember = "statusName";

            luAksi.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luAksi.Properties.DropDownRows = listStat.Count;
            luAksi.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luAksi.Properties.AutoSearchColumnIndex = 1;
            luAksi.Properties.NullText = "Silahkan Pilih...";
        }

        private void LoadData()
        {
            string SQL = "";
            SQL = SQL + Environment.NewLine + "select rm_no, to_char(insp_date,'yyyy-mm-dd') insp_date, ";
            SQL = SQL + Environment.NewLine + "visit_no, obs_id, room_name, nama, hrs_cnt, durasi, stat, null cd, 'U' act, empid ";
            SQL = SQL + Environment.NewLine + "from ( ";
            SQL = SQL + Environment.NewLine + "select a.empid, b.rm_no, b.insp_date, b.visit_no, b.obs_id, d.room_name, ";
            SQL = SQL + Environment.NewLine + "(select name from cs_employees where empid = a.empid ) nama,   ";
            SQL = SQL + Environment.NewLine + "hrs_cnt,  round(24 * (nvl(b.obs_end, sysdate) - b.obs_start), 2) durasi,   ";
            SQL = SQL + Environment.NewLine + "case when round(24 * (nvl(b.obs_end, sysdate) - b.obs_start), 2) > hrs_cnt and obs_end is null then 'Waktu habis'   ";
            SQL = SQL + Environment.NewLine + "when round(24 * (nvl(b.obs_end, sysdate) - b.obs_start), 2) <= hrs_cnt and obs_end is null then 'Proses' else 'Selesai' end stat ";
            SQL = SQL + Environment.NewLine + "from cs_patient a   ";
            SQL = SQL + Environment.NewLine + "join cs_observation b on (a.rm_no = b.rm_no)    ";
            SQL = SQL + Environment.NewLine + "JOIN cs_room d on (b.room_cd=d.room_id)   ";
            SQL = SQL + Environment.NewLine + "and a.status = 'A' AND d.status = 'A'   ";
            //SQL = SQL + Environment.NewLine + "and to_char(b.insp_date, 'yyyy-mm-dd') = to_char(sysdate,'yyyy-mm-dd')) ";
            SQL = SQL + Environment.NewLine + "and b.insp_date <= trunc(sysdate)) ";
            SQL = SQL + Environment.NewLine + "where stat='Waktu habis' ";


            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            gridControl1.DataSource = dt;

            gridView1.OptionsView.ColumnAutoWidth = true;
            gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView1.IndicatorWidth = 30;
            gridView1.OptionsBehavior.Editable = true;
            gridView1.BestFitColumns();

            gridView1.Columns[0].Caption = "RM No";
            gridView1.Columns[1].Caption = "Tanggal";
            gridView1.Columns[2].Caption = "Visit No";
            gridView1.Columns[3].Caption = "OBS ID";
            gridView1.Columns[4].Caption = "Ruangan";
            gridView1.Columns[5].Caption = "Nama";
            gridView1.Columns[6].Caption = "Lama";
            gridView1.Columns[7].Caption = "Durasi";
            gridView1.Columns[8].Caption = "Status";
            gridView1.Columns[9].Caption = "Aksi";
            gridView1.Columns[10].Caption = "Action";
            gridView1.Columns[11].Caption = "NIK";

            gridView1.Columns[0].Visible = false;
            //gridView1.Columns[1].Visible = false;
            gridView1.Columns[2].Visible = false;
            gridView1.Columns[3].Visible = false;
            gridView1.Columns[10].Visible = false;
            gridView1.Columns[11].Visible = false;

            gridView1.Columns[1].OptionsColumn.ReadOnly = true;
            gridView1.Columns[4].OptionsColumn.ReadOnly = true;
            gridView1.Columns[5].OptionsColumn.ReadOnly = true;
            gridView1.Columns[6].OptionsColumn.ReadOnly = true;
            gridView1.Columns[7].OptionsColumn.ReadOnly = true;
            gridView1.Columns[8].OptionsColumn.ReadOnly = true;
            gridView1.Columns[11].OptionsColumn.ReadOnly = true;

            RepositoryItemLookUpEdit stLookup = new RepositoryItemLookUpEdit();
            stLookup.DataSource = listStat;
            stLookup.ValueMember = "statusCode";
            stLookup.DisplayMember = "statusName";

            stLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            stLookup.DropDownRows = listStat.Count;
            stLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            stLookup.AutoSearchColumnIndex = 1;
            stLookup.NullText = "Silahkan Pilih...";
            gridView1.Columns[9].ColumnEdit = stLookup;

            gridView1.Columns[1].Width = 80;
            gridView1.Columns[4].Width = 150;
            gridView1.Columns[6].Width = 50;
            gridView1.Columns[7].Width = 50;
            gridView1.Columns[8].Width = 80;
            gridView1.Columns[9].Width = 120;

            if (gridView1.DataRowCount == 0)
            {
                this.Close();
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Status")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);
                if (kk == "Waktu habis")
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.BackColor2 = Color.Red;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.Caption == "Aksi")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView View = sender as GridView;


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string action = "", id = "", cd = "", nama = "", empid = "", visit_no = "", tgl = "";

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                id = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                cd = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                nama = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
                action = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                empid = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
                visit_no = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                tgl = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();

                if (action == "U")
                {
                    if (cd == "")
                    {
                        MessageBox.Show("Silahkan Konfirmasi pasien a.n. " + nama);
                    }
                    else if (cd == "A")
                    {
                        string sql_update = "";

                        sql_update = sql_update + Environment.NewLine + " update cs_observation ";
                        sql_update = sql_update + Environment.NewLine + " set hrs_cnt = to_char(to_number(hrs_cnt)+1), ";
                        sql_update = sql_update + Environment.NewLine + " upd_emp = '" + v_empid + "', upd_date = sysdate ";
                        sql_update = sql_update + Environment.NewLine + " where obs_id = '" + id + "' ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);

                            MessageBox.Show("Data Berhasil dirubah");
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                    else if (cd == "F")
                    {
                        string sql_status = "", stat = "";
                        sql_status = " select decode(time_receipt,null,'OBS','CLS') stat from cs_visit where to_char(visit_date,'yyyy-mm-dd') = '" + tgl + "' and que01 = '" + visit_no + "' and empid = '" + empid + "' ";
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra = new OleDbDataAdapter(sql_status, oraConnect);
                        DataTable dt = new DataTable();
                        adOra.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            stat = dt.Rows[0]["stat"].ToString();
                        }
                        else
                        {
                            stat = "";
                        }
                        

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
                                //command.CommandText = " update cs_visit set status = '" + stat + "', time_observation = sysdate, time_end = sysdate, upd_emp = '" + v_empid + "', upd_date = sysdate where empid = '" + empid + "' and to_char(visit_date,'yyyy-mm-dd') = '" + tgl + "' and que01 = '" + visit_no + "' ";
                                command.CommandText = " update cs_visit set status = '" + stat + "', time_observation = sysdate, upd_emp = '" + v_empid + "', upd_date = sysdate where empid = '" + empid + "' and to_char(visit_date,'yyyy-mm-dd') = '" + tgl + "' and que01 = '" + visit_no + "' ";
                            }
                            else
                            {
                                command.CommandText = " update cs_visit set status = '" + stat + "', time_observation = sysdate, upd_emp = '" + v_empid + "', upd_date = sysdate where empid = '" + empid + "' and to_char(visit_date,'yyyy-mm-dd') = '" + tgl + "' and que01 = '" + visit_no + "' ";
                            }

                            command.ExecuteNonQuery();

                            trans.Commit();
                            //MessageBox.Show(sql_insert);
                            //MessageBox.Show("Query Exec : " + sql_insert);
                            
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
            LoadData();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Int32[] selectedRowHandles = gridView1.GetSelectedRows();
            int temp = 0;
            int selectedRowHandle;

            temp = selectedRowHandles.Length;

            if (temp == 0)
            {
                MessageBox.Show("Silahkan checklist data");
                return;
            }

            if (luAksi.Text == "Silahkan Pilih...")
            {
                MessageBox.Show("Silahkan pilih aksi");
                return;
            }

            for (int i = 0; i < selectedRowHandles.Length; i++)
            {
                selectedRowHandle = selectedRowHandles[i];
                gridView1.SetRowCellValue(selectedRowHandle, gridView1.Columns[9], luAksi.GetColumnValue("statusCode").ToString());
            }
        }

        private void timerCek_Tick(object sender, EventArgs e)
        {
            timer++;

            if (timer == cek_interval)
            {
                timer = 0;
                timerCek.Stop();
                timerCek.Start();

                LoadData();
            }
        }
    }
}