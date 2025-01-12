using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
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
using System.Threading;
using DevExpress.XtraEditors.Repository;
using System.IO;
using System.Net;
using NAudio.Wave;
using System.Media;
using System.Web;

namespace Clinic
{
    public partial class ReservationQueue : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        int tempTimer = 0, timer = 0;
        string call="", param="", id_call="";

        public ReservationQueue()
        {
            InitializeComponent();
        }

        private void ReservationQueue_Load(object sender, EventArgs e)
        {
            OracleDepe();
            OracleDepe2();
            LoadDataDoc();
            LoadDataMid();
            LoadDataMed();
        }

        public void OracleDepe()
        {
            ConnectDb ConnOra = new ConnectDb();
            // Open the connection
            // OracleConnection connection = new OracleConnection ("DATA SOURCE=172.70.10.74/tterg;PERSIST SECURITY INFO=True;USER ID=TTIT; Password=TTIT;Pooling = False; ");
            OracleConnection connection = new OracleConnection("DATA SOURCE=localhost:1521/XE;PERSIST SECURITY INFO=True;USER ID=klinik; Password=klinik;Pooling = False; ");
            connection.Open();

            OracleCommand selectCommand = new OracleCommand("select * from cs_visit", connection);
            OracleDependency dependency = new OracleDependency(selectCommand);
            dependency.QueryBasedNotification = false;

            selectCommand.Notification.IsNotifiedOnce = false;
            // Specifies whether notifications will contain information on rows changed.

            // Set the event handler to the OnChange event.
            dependency.OnChange += new OnChangeEventHandler(OnQueueChange);

            //selectCommand.ExecuteReader();
            //selectCommand.ExecuteNonQuery();
            //Thread.Sleep(10000);
        }

        public void OracleDepe2()
        {
            ConnectDb ConnOra = new ConnectDb();
            // Open the connection
            // OracleConnection connection = new OracleConnection ("DATA SOURCE=172.70.10.74/tterg;PERSIST SECURITY INFO=True;USER ID=TTIT; Password=TTIT;Pooling = False; ");
            OracleConnection connection = new OracleConnection("DATA SOURCE=localhost:1521/XE;PERSIST SECURITY INFO=True;USER ID=klinik; Password=klinik;Pooling = False; ");
            connection.Open();

            OracleCommand selectCommand = new OracleCommand("select * from cs_call_log", connection);
            OracleDependency dependency = new OracleDependency(selectCommand);
            dependency.QueryBasedNotification = false;

            selectCommand.Notification.IsNotifiedOnce = false;
            // Specifies whether notifications will contain information on rows changed.

            // Set the event handler to the OnChange event.
            dependency.OnChange += new OnChangeEventHandler(OnQueueChange2);

            //selectCommand.ExecuteReader();
            //selectCommand.ExecuteNonQuery();
            //Thread.Sleep(10000);
        }

        public void OnQueueChange(Object sender, OracleNotificationEventArgs args)
        {
            timer = 29;
        }

        public void OnQueueChange2(Object sender, OracleNotificationEventArgs args)
        {
            string sql_search, s_flag = "";

            sql_search = " select call_id,flag, param from cs_call_log where 1=1 " +
                         " and call_id = (select max(call_id) from cs_call_log) ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_search, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            s_flag = dt.Rows[0]["flag"].ToString();
            param = dt.Rows[0]["param"].ToString();
            id_call = dt.Rows[0]["call_id"].ToString();

            if (s_flag == "N")
            {
                call = "Y";
            }
            else
            {
                call = "";
            }
        }

        public void LoadDataDoc()
        {
            string sql_search;

            sql_search = "  select que01, (select name from cs_patient_info where patient_no=a.patient_no ) name, status from cs_visit a " +
                          "  where to_char(visit_date,'yyyy-mm-dd')= '" + today + "'  " +
                          "  and purpose = 'DOC'  " +
                          "  and status in ('PRE','RSV','NUR','INS')  " +
                          "  and (type_patient is null or type_patient not in ('E')) " +
                          "  order by que01 asc  " ;

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_search, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.Appearance.Row.Font = new Font("Malgun Gothic", 40, FontStyle.Bold);
                gridView1.OptionsBehavior.Editable = false;

                gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
                //gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.False;

                gridView1.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                gridView1.Columns[1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                //gridView1.Columns[0].OptionsColumn.AllowEdit = false;
                //gridView1.Columns[1].OptionsColumn.AllowEdit = false;

                //gridView1.BestFitColumns();
                gridView1.Columns[0].Width = 100;
                gridView1.Columns[2].Visible = false;

                RepositoryItemMemoEdit nama = new RepositoryItemMemoEdit();
                nama.WordWrap = true;
                gridView1.Columns[1].ColumnEdit = nama;

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        public void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
                if (e.Column.FieldName == "QUE01" || e.Column.FieldName == "NAME")
                {
                    string info = view.GetRowCellDisplayText(e.RowHandle, view.Columns[2]);
                    if (info == "INS")
                    {
                        e.Appearance.BackColor = Color.DodgerBlue;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.Font = new Font("Malgun Gothic", 45, FontStyle.Bold);
                    }
                    
                }
                
            }
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //GridView view = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string info = view.GetRowCellDisplayText(e.RowHandle, view.Columns[0]);
            //    if (info != "000")
            //    {
            //        e.Appearance.BackColor = Color.DodgerBlue;
            //        e.Appearance.BackColor2 = Color.White;
            //        e.Appearance.ForeColor = Color.White;
            //    }

            //}
        }

        public void LoadDataMid()
        {
            string sql_search;

            sql_search = "  select que01, (select name from cs_patient_info where patient_no=a.patient_no ) name, status from cs_visit a " +
                          "  where to_char(visit_date,'yyyy-mm-dd')= '" + today + "'  " +
                          "  and purpose = 'MID'  " +
                          "  and status in ('PRE','RSV','NUR','INS')  " +
                          "  and (type_patient is null or type_patient not in ('E')) " +
                          "  order by que01 asc  ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_search, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl2.DataSource = null;
                gridView2.Columns.Clear();
                gridControl2.DataSource = dt;

                gridView2.OptionsView.ColumnAutoWidth = true;
                gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView2.Appearance.Row.Font = new Font("Malgun Gothic", 40, FontStyle.Bold);
                gridView2.OptionsBehavior.Editable = false;

                gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
                //gridView2.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
                //gridView2.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
                //gridView2.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.False;

                gridView2.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                gridView2.Columns[1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                //gridView2.Columns[0].OptionsColumn.AllowEdit = false;
                //gridView2.Columns[1].OptionsColumn.AllowEdit = false;

                //gridView1.BestFitColumns();
                gridView2.Columns[0].Width = 100;
                gridView2.Columns[2].Visible = false;

                RepositoryItemMemoEdit nama = new RepositoryItemMemoEdit();
                nama.WordWrap = true;
                gridView2.Columns[1].ColumnEdit = nama;

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
                if (e.Column.FieldName == "QUE01" || e.Column.FieldName == "NAME")
                {
                    string info = view.GetRowCellDisplayText(e.RowHandle, view.Columns[2]);
                    if (info == "INS")
                    {
                        e.Appearance.BackColor = Color.LightCoral; // Crimson
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.Font = new Font("Malgun Gothic", 45, FontStyle.Bold);
                    }

                }

            }
        }

        private void gridView2_RowStyle(object sender, RowStyleEventArgs e)
        {
            //GridView view = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string info = view.GetRowCellDisplayText(e.RowHandle, view.Columns[0]);
            //    if (info != "000")
            //    {
            //        e.Appearance.BackColor = Color.DodgerBlue;
            //        e.Appearance.BackColor2 = Color.White;
            //        e.Appearance.ForeColor = Color.White;
            //    }

            //}
        }

        private void LoadDataMed()
        {
            string sql_search;

            sql_search = "  select que02, (select name from cs_patient_info where patient_no=a.patient_no ) name, status, time_receipt from cs_visit a " +
                          "  where to_char(visit_date,'yyyy-mm-dd')= '" + today + "'  " +
                          "  and status in ('MED')  " +
                          "  order by que02 asc  ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_search, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl3.DataSource = null;
                gridView3.Columns.Clear();
                gridControl3.DataSource = dt;

                gridView3.OptionsView.ColumnAutoWidth = true;
                gridView3.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView3.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView3.Appearance.Row.Font = new Font("Malgun Gothic", 40, FontStyle.Bold);
                gridView3.OptionsBehavior.Editable = false;

                gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
                //gridView2.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
                //gridView2.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
                //gridView2.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.False;

                gridView3.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                gridView3.Columns[1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                //gridView2.Columns[0].OptionsColumn.AllowEdit = false;
                //gridView2.Columns[1].OptionsColumn.AllowEdit = false;

                //gridView1.BestFitColumns();
                gridView3.Columns[0].Width = 100;
                gridView3.Columns[2].Visible = false;
                gridView3.Columns[3].Visible = false;

                RepositoryItemMemoEdit nama = new RepositoryItemMemoEdit();
                nama.WordWrap = true;
                gridView1.Columns[1].ColumnEdit = nama;

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void gridView3_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
                if (e.Column.FieldName == "QUE02" || e.Column.FieldName == "NAME")
                {
                    string info = view.GetRowCellDisplayText(e.RowHandle, view.Columns[3]);
                    if (info != "")
                    {
                        e.Appearance.BackColor = Color.YellowGreen;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.Font = new Font("Malgun Gothic", 45, FontStyle.Bold);
                    }

                }

            }
        }

        private void gridView3_RowStyle(object sender, RowStyleEventArgs e)
        {
            //GridView view = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string info = view.GetRowCellDisplayText(e.RowHandle, view.Columns[0]);
            //    if (info != "000")
            //    {
            //        e.Appearance.BackColor = Color.DodgerBlue;
            //        e.Appearance.BackColor2 = Color.White;
            //        e.Appearance.ForeColor = Color.White;
            //    }

            //}
        }

        public void CallPasien()
        {

            string fname = ".wav",  p_dir = "", urltts = "", teks = "";
            string sql_insert = "", s_stat = "", sql_upd="";
            //p_dir = resourcesDirectory;
            p_dir = "C:\\TTCMS_PGM\\TTCMS_CLINIC\\";
            teks = param;

            SoundPlayer player = new SoundPlayer(p_dir + "suara_antrian1" + fname);
            player.PlaySync();
            urltts = string.Format("https://translate.googleapis.com/translate_tts?ie=UTF-8&q={0}&tl={1}&total=1&idx=0&textlen={2}&client=gtx", HttpUtility.UrlEncode(teks, Encoding.GetEncoding("utf-8")), "id" + "-gb&q=", teks.Length);
            PlayMp3FromUrl(urltts);
            //SoundPlayer player2 = new SoundPlayer(p_dir + "suara_antrian2" + fname);
            //player2.PlaySync();

            sql_upd = sql_upd + " update cs_call_log";
            sql_upd = sql_upd + " set flag = 'Y' ";
            sql_upd = sql_upd + " where call_id = '" + id_call + "' ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbCommand cm = new OleDbCommand(sql_upd, oraConnect);
            oraConnect.Open();
            cm.ExecuteNonQuery();
            oraConnect.Close();
            cm.Dispose();

            call = "";
        }

        public static void PlayMp3FromUrl(string url)
        {
            using (Stream ms = new MemoryStream())
            {
                using (Stream stream = WebRequest.Create(url)
                    .GetResponse().GetResponseStream())
                {
                    byte[] buffer = new byte[32768];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                }

                ms.Position = 0;
                using (WaveStream blockAlignedStream =
                    new BlockAlignReductionStream(
                        WaveFormatConversionStream.CreatePcmStream(
                            new Mp3FileReader(ms))))
                {
                    using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        waveOut.Init(blockAlignedStream);
                        waveOut.Play();
                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tempTimer = 30;

            timer++;
            if (timer == (tempTimer ))
            {
                LoadDataDoc();
                LoadDataMid();
                LoadDataMed();
                timer = 0; tempTimer = 0;
            }
            else
            {
                ltimer.Text = timer.ToString();
            }

            if (call == "Y")
            {
                CallPasien();
                
            }
            else
            {

            }
        }
    }
}