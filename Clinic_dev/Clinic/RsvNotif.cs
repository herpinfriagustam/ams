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
    public partial class RsvNotif : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        List<Status> listStat = new List<Status>();
        List<Stat> listSt = new List<Stat>();
        int timer = 0, cek_interval = 180;

        public RsvNotif()
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
            listStat.Add(new Status() { statusCode = "CLS", statusName = "Close" });
            listStat.Add(new Status() { statusCode = "CAN", statusName = "Cancel" });

            luAksi.Properties.DataSource = listStat;
            luAksi.Properties.ValueMember = "statusCode";
            luAksi.Properties.DisplayMember = "statusName";

            luAksi.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luAksi.Properties.DropDownRows = listStat.Count;
            luAksi.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luAksi.Properties.AutoSearchColumnIndex = 1;
            luAksi.Properties.NullText = "Silahkan Pilih...";

            listSt.Clear();
            listSt.Add(new Stat() { statCode = "PRE", statName = "Preparation" });
            listSt.Add(new Stat() { statCode = "RSV", statName = "Reservation" });
            listSt.Add(new Stat() { statCode = "NUR", statName = "First Inspection" });
            listSt.Add(new Stat() { statCode = "INS", statName = "Inspection" });
            listSt.Add(new Stat() { statCode = "MED", statName = "Medicine" });
            listSt.Add(new Stat() { statCode = "OBS", statName = "Observation" });
            listSt.Add(new Stat() { statCode = "HOL", statName = "Hold" });
        }

        private void LoadData()
        {
            string SQL = "";

            SQL = SQL + Environment.NewLine + "select to_char(visit_date,'yyyy-mm-dd') visit_date,  ";
            SQL = SQL + Environment.NewLine + "a.empid, name,que01,decode(purpose,'DOC','Dokter','Bidan') berobat,  ";
            SQL = SQL + Environment.NewLine + "status, null cd, 'U' act ";
            SQL = SQL + Environment.NewLine + "from cs_visit a ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid) ";
            SQL = SQL + Environment.NewLine + "where trunc(visit_date)<= trunc(sysdate-1) ";
            SQL = SQL + Environment.NewLine + "and status in ('PRE','RSV','INS','MED','OBS','HOL') ";



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

            gridView1.Columns[0].Caption = "Tanggal";
            gridView1.Columns[1].Caption = "NIK";
            gridView1.Columns[2].Caption = "Nama";
            gridView1.Columns[3].Caption = "No";
            gridView1.Columns[4].Caption = "Berobat";
            gridView1.Columns[5].Caption = "Status";
            gridView1.Columns[6].Caption = "Aksi";
            gridView1.Columns[7].Caption = "Action";

            RepositoryItemLookUpEdit stat = new RepositoryItemLookUpEdit();
            stat.DataSource = listSt;
            stat.ValueMember = "statCode";
            stat.DisplayMember = "statName";

            stat.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            stat.DropDownRows = listSt.Count;
            stat.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            stat.AutoSearchColumnIndex = 1;
            stat.NullText = "";
            gridView1.Columns[5].ColumnEdit = stat;

            gridView1.Columns[7].Visible = false;

            gridView1.Columns[0].OptionsColumn.ReadOnly = true;
            gridView1.Columns[1].OptionsColumn.ReadOnly = true;
            gridView1.Columns[2].OptionsColumn.ReadOnly = true;
            gridView1.Columns[3].OptionsColumn.ReadOnly = true;
            gridView1.Columns[4].OptionsColumn.ReadOnly = true;
            gridView1.Columns[5].OptionsColumn.ReadOnly = true;
            gridView1.Columns[7].OptionsColumn.ReadOnly = true;

            RepositoryItemLookUpEdit stLookup = new RepositoryItemLookUpEdit();
            stLookup.DataSource = listStat;
            stLookup.ValueMember = "statusCode";
            stLookup.DisplayMember = "statusName";

            stLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            stLookup.DropDownRows = listStat.Count;
            stLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            stLookup.AutoSearchColumnIndex = 1;
            stLookup.NullText = "Silahkan Pilih...";
            gridView1.Columns[6].ColumnEdit = stLookup;

            gridView1.Columns[0].Width = 80;
            gridView1.Columns[1].Width = 80;
            gridView1.Columns[2].Width = 120;
            gridView1.Columns[3].Width = 50;
            gridView1.Columns[4].Width = 80;
            gridView1.Columns[5].Width = 120;

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

            if (e.Column.Caption == "Berobat")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[4]);
                if (kk == "Dokter")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Bidan")
                {
                    e.Appearance.BackColor = Color.LightCoral;
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
            string action = "", nik = "", no = "", tgl = "", nama = "", cd = "";

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                nik = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                no = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                tgl = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                nama = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                cd = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                action = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();

                if (action == "U")
                {
                    if (cd == "")
                    {
                        MessageBox.Show("Silahkan Konfirmasi pasien a.n. " + nama);
                    }
                    else if (cd == "CAN" || cd == "CLS")
                    {
                        string sql_update = "";

                        sql_update = sql_update + Environment.NewLine + " update cs_visit ";
                        sql_update = sql_update + Environment.NewLine + " set status = '" + cd + "', ";
                        sql_update = sql_update + Environment.NewLine + " upd_emp = '" + v_empid + "', upd_date = sysdate ";
                        sql_update = sql_update + Environment.NewLine + " where empid = '" + nik + "' ";
                        sql_update = sql_update + Environment.NewLine + " and to_char(visit_date,'yyyy-mm-dd') = '" + tgl + "' ";
                        sql_update = sql_update + Environment.NewLine + " and que01 = '" + no + "' ";

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
                gridView1.SetRowCellValue(selectedRowHandle, gridView1.Columns[6], luAksi.GetColumnValue("statusCode").ToString());
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