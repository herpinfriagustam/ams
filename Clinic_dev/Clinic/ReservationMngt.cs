using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using Clinic.Properties;
using System.IO;
using System.Media;
using System.Threading;
using System.Web;
using NAudio.Wave;
using System.Net;

namespace Clinic
{
    public partial class ReservationMngt : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        Poli poli = new Poli();
        List<Poli> listPoli = new List<Poli>();
        List<PatientType> listPatientType = new List<PatientType>();
        List<WorkAccident> listWorkAccident = new List<WorkAccident>();
        List<Purpose> listPurpose = new List<Purpose>();
        List<Status> listStat = new List<Status>();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string workingDirectory = "", resourcesDirectory = "";

        public ReservationMngt()
        {
            InitializeComponent();
        }

        private void ReservationInput_Load(object sender, EventArgs e)
        {
            workingDirectory = Environment.CurrentDirectory;
            resourcesDirectory = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\";
            initData();
            LoadData();
        }

        private void LoadData()
        {
            string sql_search;

            sql_search = " ";
            sql_search = sql_search + " select que01, a.empid, b.name, b.dept, gender, age, poli_cd, type_patient, work_accident, purpose, status, 'S' action,  " +
                                      " case  when observation = 'Y' then 'Yes' else 'No' end as observation, visit_remark " +
                                      " from cs_visit a join cs_employees b on a.empid = b.empid " +
                                      " where 1 = 1  " +
                                      " and to_char(visit_date,'yyyy-mm-dd')= '" + today + "'  " +
                                      " and status in ('PRE','RSV','NUR','INS','OBS')" +
                                      " order by purpose, que01 ";

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

                simpleButton2.Enabled = false;

                //gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 30;
                //gridView1.OptionsBehavior.Editable = false;
                gridView1.BestFitColumns();
                //gridView1.OptionsSelection.MultiSelect = true;
                //gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView1.VisibleColumns[0].Width = 20;
                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[1].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[2].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[3].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[4].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[5].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                gridView1.Columns[1].OptionsColumn.ReadOnly = true;
                gridView1.Columns[2].OptionsColumn.ReadOnly = true;
                gridView1.Columns[3].OptionsColumn.ReadOnly = true;
                gridView1.Columns[4].OptionsColumn.ReadOnly = true;
                gridView1.Columns[5].OptionsColumn.ReadOnly = true;
                gridView1.Columns[11].OptionsColumn.ReadOnly = true;
                gridView1.Columns[12].OptionsColumn.ReadOnly = true;

                gridView1.Columns[0].Caption = "Antrian";
                gridView1.Columns[1].Caption = "NIK";
                gridView1.Columns[2].Caption = "Nama";
                gridView1.Columns[3].Caption = "Dept";
                gridView1.Columns[4].Caption = "Jenis Kelamin";
                gridView1.Columns[5].Caption = "Umur";
                gridView1.Columns[6].Caption = "Poli";
                gridView1.Columns[7].Caption = "Pasien";
                gridView1.Columns[8].Caption = "KK";
                gridView1.Columns[9].Caption = "Berobat";
                gridView1.Columns[10].Caption = "Status";
                gridView1.Columns[11].Caption = "Action";
                gridView1.Columns[12].Caption = "Observation";
                gridView1.Columns[13].Caption = "Remark";
                gridView1.Columns[6].MinWidth = 90;
                gridView1.Columns[6].MinWidth = 90;
                gridView1.Columns[7].MinWidth = 90;
                gridView1.Columns[7].MinWidth = 90;
                gridView1.Columns[10].MinWidth = 100;
                gridView1.Columns[10].MinWidth = 100;


                //PRE, RSV, NUR, INS, OBS, MED, CLS, CAN

                RepositoryItemLookUpEdit poliLookup = new RepositoryItemLookUpEdit();
                poliLookup.DataSource = listPoli;
                poliLookup.ValueMember = "poliCode";
                poliLookup.DisplayMember = "poliName";

                poliLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                poliLookup.DropDownRows = listPoli.Count;
                poliLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                poliLookup.AutoSearchColumnIndex = 1;
                poliLookup.NullText = "";
                gridView1.Columns[6].ColumnEdit = poliLookup;

                RepositoryItemLookUpEdit patientLookup = new RepositoryItemLookUpEdit();
                patientLookup.DataSource = listPatientType;
                patientLookup.ValueMember = "patientTypeCode";
                patientLookup.DisplayMember = "patientTypeName";

                patientLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                patientLookup.DropDownRows = listPatientType.Count;
                patientLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                patientLookup.AutoSearchColumnIndex = 1;
                patientLookup.NullText = "";
                gridView1.Columns[7].ColumnEdit = patientLookup;

                RepositoryItemLookUpEdit workAccLookup = new RepositoryItemLookUpEdit();
                workAccLookup.DataSource = listWorkAccident;
                workAccLookup.ValueMember = "workAccidentCode";
                workAccLookup.DisplayMember = "workAccidentName";

                workAccLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                workAccLookup.DropDownRows = listWorkAccident.Count;
                workAccLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                workAccLookup.AutoSearchColumnIndex = 1;
                workAccLookup.NullText = "";
                gridView1.Columns[8].ColumnEdit = workAccLookup;

                RepositoryItemLookUpEdit purposeLookup = new RepositoryItemLookUpEdit();
                purposeLookup.DataSource = listPurpose;
                purposeLookup.ValueMember = "purposeCode";
                purposeLookup.DisplayMember = "purposeName";

                purposeLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                purposeLookup.DropDownRows = listPurpose.Count;
                purposeLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                purposeLookup.AutoSearchColumnIndex = 1;
                purposeLookup.NullText = "";
                gridView1.Columns[9].ColumnEdit = purposeLookup;

                RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
                statusLookup.DataSource = listStat;
                statusLookup.ValueMember = "statusCode";
                statusLookup.DisplayMember = "statusName";

                statusLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup.DropDownRows = listStat.Count;
                statusLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup.AutoSearchColumnIndex = 1;
                statusLookup.NullText = "";
                gridView1.Columns[10].ColumnEdit = statusLookup;
                gridView1.BestFitColumns();
                
                gridView1.Columns[11].Visible = false;
                gridView1.Columns[13].Visible = false;

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string fname = ".wav", p_que="", p1="", p2 = "", p3 = "", p4 = "", p_dir="", s_gender="", s_name="", urltts="", teks="";

            //p_dir = resourcesDirectory;
            p_dir = "C:\\TTCMS_PGM\\TTCMS_CLINIC\\";

            p_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_gender = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
            s_name = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();

            p1 = p_que.Substring(0,1);
            p2 = p_que.Substring(1, 1);
            p3 = p_que.Substring(2, 1);
            p4 = p_que.Substring(3, 1);

            if (s_gender == "Perempuan")
            {
                p1 = "Ibu";
            }
            else
            {
                p1 = "Bapak";
            }

            p2 = s_name;

            teks = p1 + p2;

            loading.ShowWaitForm();
            try
            {
                urltts = string.Format("https://translate.googleapis.com/translate_tts?ie=UTF-8&q={0}&tl={1}&total=1&idx=0&textlen={2}&client=gtx", HttpUtility.UrlEncode(teks, Encoding.GetEncoding("utf-8")), "id" + "-gb&q=", teks.Length);
                PlayMp3FromUrl(urltts);

                //SoundPlayer player = new SoundPlayer(p_dir + "antrian" + fname);
                //SoundPlayer player2 = new SoundPlayer(p_dir + p1 + fname);
                //SoundPlayer player3 = new SoundPlayer(p_dir + "_" + p2 + fname);
                //SoundPlayer player4 = new SoundPlayer(p_dir + "_" + p3 + fname);
                //SoundPlayer player5 = new SoundPlayer(p_dir + "_" + p4 + fname);
                SoundPlayer player6 = new SoundPlayer(p_dir + "IN" + fname);
                //player.PlaySync();
                ////Thread.Sleep(2000);
                //player2.PlaySync();
                ////Thread.Sleep(900);
                //player3.PlaySync();
                ////Thread.Sleep(900);
                //player4.PlaySync();
                ////Thread.Sleep(900);
                //player5.PlaySync();
                //Thread.Sleep(900);
                player6.PlaySync();
                //Thread.Sleep(2000);

                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            
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

        private void initData()
        {
            string sql_poli = " select poli_cd, poli_name from cs_policlinic where status = 'A' ";
            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_poli, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);
            listPoli.Clear();
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                listPoli.Add(new Poli() { poliCode = dt2.Rows[i]["poli_cd"].ToString(), poliName = dt2.Rows[i]["poli_name"].ToString() });
                //poli.poliCode = dt2.Rows[i]["poli_cd"].ToString();
                //poli.poliName = dt2.Rows[i]["poli_name"].ToString();
                //listPoli.Add(poli);
            }


            listPatientType.Clear();
            listPatientType.Add(new PatientType() { patientTypeCode = "E", patientTypeName = "Emergency" });
            listPatientType.Add(new PatientType() { patientTypeCode = "U", patientTypeName = "Umum" });

            listWorkAccident.Clear();
            listWorkAccident.Add(new WorkAccident() { workAccidentCode = "Y", workAccidentName = "Yes" });
            listWorkAccident.Add(new WorkAccident() { workAccidentCode = "N", workAccidentName = "No" });

            listPurpose.Clear();
            listPurpose.Add(new Purpose() { purposeCode = "DOC", purposeName = "Dokter" });
            listPurpose.Add(new Purpose() { purposeCode = "MID", purposeName = "Bidan" });

            listStat.Clear();
            listStat.Add(new Status() { statusCode = "PRE", statusName = "Preparation" });
            listStat.Add(new Status() { statusCode = "RSV", statusName = "Reservation" });
            listStat.Add(new Status() { statusCode = "NUR", statusName = "First Inspection" });
            listStat.Add(new Status() { statusCode = "INS", statusName = "Inspection" });
            listStat.Add(new Status() { statusCode = "OBS", statusName = "Observation" });
            //listStat.Add(new Status() { statusCode = "MED", statusName = "Medicine" });
            //listStat.Add(new Status() { statusCode = "CLS", statusName = "Completed" });
            listStat.Add(new Status() { statusCode = "CAN", statusName = "Cancel" });
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //GridView view = sender as GridView;
            //if (e.Column.Caption != "Berobat") return;
            //// Fill a cell's background if its value is greater than 30. 
            //if (e.CellValue.ToString() == "MID")
            //{
            //    e.Appearance.BackColor = Color.FromArgb(40, Color.LightCoral);
            //}
            //else
            //{
            //    e.Appearance.BackColor = Color.FromArgb(40, Color.DodgerBlue);
            //}
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string pur = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);
            //    string stat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
            //    if (stat == "Inspection" && pur == "Dokter")
            //    {
            //        //e.Appearance.BackColor = Color.FromArgb(40, Color.DodgerBlue);
            //        e.Appearance.BackColor = Color.DodgerBlue;
            //        //e.Appearance.BackColor2 = Color.White;
            //        e.Appearance.ForeColor = Color.White;
            //        //e.Appearance.Font = new Font("Arial", 9, FontStyle.Bold);
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //        e.HighPriority = true;
            //    }

            //    if (stat == "Inspection" && pur == "Bidan")
            //    {
            //        e.Appearance.BackColor = Color.LightCoral;
            //        //e.Appearance.BackColor2 = Color.White;
            //        e.Appearance.ForeColor = Color.White;
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //        e.HighPriority = true;
            //    }
            //}
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.Column.Caption == "Pasien")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[7]);
                if (kk == "Emergency")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Red);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.Red);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else
                {
                    e.Appearance.BackColor = Color.OldLace;
                    e.Appearance.ForeColor = Color.Black;
                }
            }

            if (e.Column.Caption == "KK")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);
                if (kk == "Yes")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.OrangeRed);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else
                {
                    e.Appearance.BackColor = Color.OldLace;
                    e.Appearance.ForeColor = Color.Black;
                }
            }

            if (e.Column.Caption == "Status")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
                string pur = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);

                if (kk == "Inspection" && pur == "Dokter")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.BackColor2 = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Inspection" && pur == "Bidan")
                {
                    e.Appearance.BackColor = Color.LightCoral;
                    e.Appearance.BackColor2 = Color.LightCoral;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "First Inspection" && pur == "Dokter")
                {
                    e.Appearance.BackColor = Color.FromArgb(75, Color.DodgerBlue);
                    e.Appearance.BackColor2 = Color.FromArgb(75, Color.DodgerBlue);
                }
                else if (kk == "First Inspection" && pur == "Bidan")
                {
                    e.Appearance.BackColor = Color.FromArgb(75, Color.LightCoral);
                    e.Appearance.BackColor2 = Color.FromArgb(75, Color.LightCoral);
                }
                else if (kk == "Reservation" && pur == "Dokter")
                {
                    e.Appearance.BackColor = Color.FromArgb(50, Color.DodgerBlue);
                    e.Appearance.BackColor2 = Color.FromArgb(50, Color.DodgerBlue);
                }
                else if (kk == "Reservation" && pur == "Bidan")
                {
                    e.Appearance.BackColor = Color.FromArgb(50, Color.LightCoral);
                    e.Appearance.BackColor2 = Color.FromArgb(50, Color.LightCoral);
                }
                else if (kk == "Observation")
                {
                    e.Appearance.BackColor = Color.MediumPurple;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Preparation")
                {
                    e.Appearance.BackColor = Color.OldLace;
                    e.Appearance.ForeColor = Color.Black;
                }
            }

            if (e.Column.Caption == "Poli")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
                //e.Appearance.FontStyleDelta = FontStyle.Bold;
            }

            if (e.Column.Caption == "Berobat")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
                //e.Appearance.FontStyleDelta = FontStyle.Bold;
            }
        }

        private void gridView1_EditFormPrepared(object sender, EditFormPreparedEventArgs e)
        {

        }

        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            GridView view = sender as GridView;

            //string v1 = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
            //string p_que = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();
            //string p_empid = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
            //string p_date = today;
            //string p_poli = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
            //string p_pasient = view.GetRowCellValue(e.RowHandle, view.Columns[7]).ToString();
            //string p_workA = view.GetRowCellValue(e.RowHandle, view.Columns[8]).ToString();
            //string p_purpose = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();
            //string p_stat = view.GetRowCellValue(e.RowHandle, view.Columns[10]).ToString();
            //MessageBox.Show("Params: " + p_poli + ", " + p_pasient + ", " + p_workA + ", " + p_purpose + ", " + p_stat + ", " + p_que + ", " + p_empid + ", " + p_date);

            //string sql_update;

            //sql_update = " update cs_visit " +
            //             " set poli_cd = '" + p_poli + "', type_patient = '" + p_pasient + "', " +
            //             " work_accident = '" + p_workA + "', purpose = '" + p_purpose + "', status = '" + p_stat + "' " +
            //             " where que01 = '" + p_que  + "' and empid = '" + p_empid + "' and to_char(visit_date,'yyyy-mm-dd') = '" + p_date + "' ";
            try
            {
                //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                //OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                //oraConnect.Open();
                //cm.ExecuteNonQuery();
                //oraConnect.Close();
                //cm.Dispose();

                //MessageBox.Show("Query Exec : " + sql_update);
                
                //MessageBox.Show("Update Success");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView1.Columns[1].OptionsColumn.ReadOnly = false;
            //gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gridView1.AddNewRow();
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns[10], "RSV");
            view.SetRowCellValue(e.RowHandle, view.Columns[9], "DOC");
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            
            if (e.Column.Caption == "NIK")
            {
                string p_empid = e.Value.ToString();
                string empid = "", name = "", dept = "", gender = "", age = "";
                string sql_emp = " select empid, name, dept, gender, age from cs_employees where 1 = 1 and empid = '" + p_empid + "' ";

                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_emp, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    name = dt.Rows[0]["name"].ToString();
                    dept = dt.Rows[0]["dept"].ToString();
                    gender = dt.Rows[0]["gender"].ToString();
                    age = dt.Rows[0]["age"].ToString();
                }
                else
                {
                    empid = ""; dept = ""; gender = ""; age = "";
                    view.SetColumnError(gridView1.Columns[1], "Employees Not Found");
                }

                view.SetRowCellValue(e.RowHandle, view.Columns[2], name);
                view.SetRowCellValue(e.RowHandle, view.Columns[3], dept);
                view.SetRowCellValue(e.RowHandle, view.Columns[4], gender);
                view.SetRowCellValue(e.RowHandle, view.Columns[5], age);

                view.SetRowCellValue(e.RowHandle, view.Columns[7], "U");
                view.SetRowCellValue(e.RowHandle, view.Columns[8], "N");
                view.SetRowCellValue(e.RowHandle, view.Columns[10], "PRE");
                view.SetRowCellValue(e.RowHandle, view.Columns[11], "I");
            }

            if (e.Column.Caption == "Poli" || e.Column.Caption == "Pasien" || e.Column.Caption == "KK" || e.Column.Caption == "Berobat" || e.Column.Caption == "Status" || e.Column.Caption == "Remark")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "U");
                    simpleButton2.Enabled = true;
                }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string que = "", nik = "", nama = "", poli = "", pasien = "", workA = "", purpose = "", status = "", action = "", cek="", remark = "";
            string sql_check = "", sql_cnt = "", sql_insert="", sql_update = "", c_que = "", tmp_queue= "", visit_cnt="";
            int queue = 0, visit=0;
            cek = "";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                que = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                nik = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                nama = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                poli = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                pasien = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();
                workA = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();
                purpose = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                status = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                action = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
                remark = gridView1.GetRowCellValue(i, gridView1.Columns[13]).ToString();
                if (action == "I")
                {
                    if (nama == "")
                    {
                        MessageBox.Show("Employee No tidak valid");
                    }
                    else if (purpose == "")
                    {
                        MessageBox.Show("Tujuan Berobat harus diisi");
                    }
                    else
                    {
                        if (purpose == "DOC")
                        {
                            c_que = "D";
                        }
                        else
                        {
                            c_que = "M";
                        }

                        sql_check = " select  nvl(max(to_number(substr(que01,2,3))),0) que from cs_visit where to_char(visit_date,'yyyy-mm-dd')= '" + today + "' and purpose = '" + purpose + "' ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_check, oraConnect);
                            DataTable dt = new DataTable();
                            adOra.Fill(dt);

                            tmp_queue = dt.Rows[0]["que"].ToString();
                            queue = Convert.ToInt32(tmp_queue) + 1;
                            que = queue.ToString();
                            if (queue < 10)
                            {
                                que = que.PadLeft(que.Length + 2, '0');
                            }
                            else if (queue < 100)
                            {
                                que = que.PadLeft(que.Length + 1, '0');
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }

                        sql_cnt = " select count(empid) cnt from cs_visit where empid = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd')= '" + today + "' and status not in ('CLS','CAN') ";
                        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_cnt, oraConnect2);
                        DataTable dt2 = new DataTable();
                        adOra2.Fill(dt2);
                        visit_cnt = dt2.Rows[0]["cnt"].ToString();
                        if (Convert.ToInt32(visit_cnt) > 0)
                        {
                            //MessageBox.Show("Employee ID " + nik + " sudah terdaftar.");
                        }
                        else
                        {
                            visit = Convert.ToInt32(visit_cnt) + 1;

                            sql_insert = " insert into cs_visit (empid, visit_date, status, poli_cd, type_patient, work_accident, purpose, visit_remark, visit_cnt, que01, ins_date, ins_emp) values ('" + nik + "',sysdate, '" + status + "', '" + poli + "', '" + pasien + "','" + workA + "', '" + purpose + "', '" + remark + "', '" + Convert.ToString(visit) + "', '" + c_que + que + "' , sysdate, '" + v_empid + "') ";
                            
                            cek = cek + sql_insert;
                            try
                            {
                                OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
                                OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect3);
                                oraConnect3.Open();
                                cm.ExecuteNonQuery();
                                oraConnect3.Close();
                                cm.Dispose();

                                //MessageBox.Show(sql_insert);
                                //MessageBox.Show("Query Exec : " + sql);
                                MessageBox.Show("Data Berhasil disimpan.");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ERROR: " + ex.Message);
                            }
                        }
                    }
                }
                else if (action == "U")
                {
                    sql_update = "";

                    sql_update = sql_update + " update cs_visit " +
                                 " set poli_cd = '" + poli + "', type_patient = '" + pasien + "', " +
                                 " work_accident = '" + workA + "', purpose = '" + purpose + "', visit_remark = '" + remark + "', status = '" + status + "', ";
                    if (status == "INS")
                    {
                        sql_update = sql_update + " time_reservation = sysdate, ";
                    }
                    sql_update = sql_update + " upd_emp = '" + v_empid + "', upd_date = sysdate ";
                    sql_update = sql_update + " where que01 = '" + que + "' and empid = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + today + "' ";

                    cek = cek + sql_update;

                    try
                    {
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                        oraConnect.Open();
                        cm.ExecuteNonQuery();
                        oraConnect.Close();
                        cm.Dispose();

                        //MessageBox.Show("Query Exec : " + sql_update);

                        MessageBox.Show("Data Berhasil diupdate");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }
            }
            richTextBox1.Text = cek;
            //MessageBox.Show(action);
            LoadData();
        }
    }
}
