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
using System.Globalization;

namespace Clinic
{
    public partial class ReservationMngt2 : DevExpress.XtraEditors.XtraForm
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

        public ReservationMngt2()
        {
            InitializeComponent();
        }

        private void ReservationInput_Load(object sender, EventArgs e)
        {
            workingDirectory = Environment.CurrentDirectory;
            resourcesDirectory = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\";
            dDateVisitBgn.Text = today;
            dDateVisitEnd.Text = today;
            initData();
            //LoadData();
        }

        private void LoadData()
        {
            string sql_search;

            sql_search = " ";
            sql_search = sql_search + " select to_char(visit_date,'yyyy-mm-dd') visit_date, que01, a.patient_no, b.name, null dept,  poli_cd, type_patient, null work_accident, purpose, a.status, 'S' action,  " +
                                      " null observation, visit_remark, to_char(visit_date,'hh24:mi:ss') visit_time, a.status stat, " +
                                      " null shold, null ehold " +
                                      " from cs_visit a join cs_patient_info b on a.patient_no = b.patient_no " +
                                      " where 1 = 1  " +
                                      " and trunc(visit_date) between to_date('" + dDateVisitBgn.Text + "','yyyy-mm-dd') and to_date ('" + dDateVisitEnd.Text + "','yyyy-mm-dd')  " +
                                      //" and status in ('PRE','RSV','NUR','INS','OBS')" +
                                      " order by a.ins_date ";

            loading.ShowWaitForm();
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
                //gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 30;
                //gridView1.OptionsBehavior.Editable = false;
                //gridView1.BestFitColumns();
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
                gridView1.Columns[10].OptionsColumn.ReadOnly = true;
                gridView1.Columns[11].OptionsColumn.ReadOnly = true;
                gridView1.Columns[13].OptionsColumn.ReadOnly = true;

                gridView1.Columns[0].Caption = "Tanggal";
                gridView1.Columns[1].Caption = "Antrian";
                gridView1.Columns[2].Caption = "Pasien No";
                gridView1.Columns[3].Caption = "Nama";
                gridView1.Columns[4].Caption = "Dept";
                gridView1.Columns[5].Caption = "Poli";
                gridView1.Columns[6].Caption = "Pasien";
                gridView1.Columns[7].Caption = "KK";
                gridView1.Columns[8].Caption = "Berobat";
                gridView1.Columns[9].Caption = "Status";
                gridView1.Columns[10].Caption = "Action";
                gridView1.Columns[11].Caption = "Observation";
                gridView1.Columns[12].Caption = "Remark";
                gridView1.Columns[13].Caption = "Jam";
                gridView1.Columns[14].Caption = "Stat";
                gridView1.Columns[15].Caption = "Mulai Tunda";
                gridView1.Columns[16].Caption = "Selesai Tunda";
                gridView1.Columns[13].VisibleIndex = 1;

                gridView1.Columns[5].MinWidth = 90;
                gridView1.Columns[5].MaxWidth = 90;
                gridView1.Columns[6].MaxWidth = 90;
                gridView1.Columns[6].MinWidth = 90;
                gridView1.Columns[9].MinWidth = 100;
                gridView1.Columns[9].MaxWidth = 100;

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
                gridView1.Columns[5].ColumnEdit = poliLookup;

                RepositoryItemLookUpEdit patientLookup = new RepositoryItemLookUpEdit();
                patientLookup.DataSource = listPatientType;
                patientLookup.ValueMember = "patientTypeCode";
                patientLookup.DisplayMember = "patientTypeName";

                patientLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                patientLookup.DropDownRows = listPatientType.Count;
                patientLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                patientLookup.AutoSearchColumnIndex = 1;
                patientLookup.NullText = "";
                gridView1.Columns[6].ColumnEdit = patientLookup;

                RepositoryItemLookUpEdit workAccLookup = new RepositoryItemLookUpEdit();
                workAccLookup.DataSource = listWorkAccident;
                workAccLookup.ValueMember = "workAccidentCode";
                workAccLookup.DisplayMember = "workAccidentName";

                workAccLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                workAccLookup.DropDownRows = listWorkAccident.Count;
                workAccLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                workAccLookup.AutoSearchColumnIndex = 1;
                workAccLookup.NullText = "";
                gridView1.Columns[7].ColumnEdit = workAccLookup;

                RepositoryItemLookUpEdit purposeLookup = new RepositoryItemLookUpEdit();
                purposeLookup.DataSource = listPurpose;
                purposeLookup.ValueMember = "purposeCode";
                purposeLookup.DisplayMember = "purposeName";

                purposeLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                purposeLookup.DropDownRows = listPurpose.Count;
                purposeLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                purposeLookup.AutoSearchColumnIndex = 1;
                purposeLookup.NullText = "";
                gridView1.Columns[8].ColumnEdit = purposeLookup;

                RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
                statusLookup.DataSource = listStat;
                statusLookup.ValueMember = "statusCode";
                statusLookup.DisplayMember = "statusName";

                statusLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup.DropDownRows = listStat.Count;
                statusLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup.AutoSearchColumnIndex = 1;
                statusLookup.NullText = "";
                gridView1.Columns[9].ColumnEdit = statusLookup;
                gridView1.BestFitColumns();

                gridView1.Columns[4].Visible = false;
                gridView1.Columns[7].Visible = false;
                gridView1.Columns[11].Visible = false;
                gridView1.Columns[10].Visible = false;
                //gridView1.Columns[12].Visible = false;
                gridView1.Columns[14].Visible = false;

                gridView1.Columns[15].Visible = false;
                gridView1.Columns[16].Visible = false;

                gridView1.Columns[12].Width = 150;
                gridView1.Columns[15].Width = 150;
                gridView1.Columns[16].Width = 150;

                gridView1.Columns[3].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[4].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                loading.CloseWaitForm();

                if (gridView1.RowCount > 0)
                {
                    simpleButton4.Enabled = true;
                }
                else
                {
                    simpleButton4.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
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
            listPatientType.Add(new PatientType() { patientTypeCode = "B", patientTypeName = "BPJS" });
            listPatientType.Add(new PatientType() { patientTypeCode = "U", patientTypeName = "Umum" });
            listPatientType.Add(new PatientType() { patientTypeCode = "P", patientTypeName = "Perusahaan" });

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
            listStat.Add(new Status() { statusCode = "MED", statusName = "Medicine" });
            listStat.Add(new Status() { statusCode = "CLS", statusName = "Completed" });
            listStat.Add(new Status() { statusCode = "PAY", statusName = "Pembayaran" });
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
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);
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
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[7]);
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
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);
                string pur = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);

                if (kk == "Completed")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Green);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Observation")
                {
                    e.Appearance.BackColor = Color.MediumPurple;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Cancel")
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Preparation")
                {
                    e.Appearance.BackColor = Color.OldLace;
                    e.Appearance.ForeColor = Color.Black;
                }

            }
            

            if (e.Column.Caption == "Tanggal" || e.Column.Caption == "Antrian" || e.Column.Caption == "Pasien No" || e.Column.Caption == "Poli" || e.Column.Caption == "Berobat" || e.Column.Caption == "Mulai Tunda" || e.Column.Caption == "Selesai Tunda")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
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
            gridView1.Columns[0].OptionsColumn.ReadOnly = false;
            gridView1.Columns[2].OptionsColumn.ReadOnly = false;
            gridView1.AddNewRow();
            //gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns[8], "DOC");
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            
            if (e.Column.Caption == "Pasien No")
            {
                string p_empid = e.Value.ToString();
                string empid = "", name = "", dept = "";
                string sql_emp = " select patient_no, name, null dept from cs_patient_info where 1 = 1 and patient_no = '" + p_empid + "' ";

                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_emp, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    name = dt.Rows[0]["name"].ToString();
                    dept = dt.Rows[0]["dept"].ToString();
                }
                else
                {
                    empid = ""; dept = "";
                    view.SetColumnError(gridView1.Columns[2], "Data pasien tidak ditemukan");
                }

                view.SetRowCellValue(e.RowHandle, view.Columns[3], name);
                view.SetRowCellValue(e.RowHandle, view.Columns[4], dept);

                view.SetRowCellValue(e.RowHandle, view.Columns[6], "U");
                view.SetRowCellValue(e.RowHandle, view.Columns[7], "N");
                view.SetRowCellValue(e.RowHandle, view.Columns[9], "PRE");
                view.SetRowCellValue(e.RowHandle, view.Columns[10], "I");
            }

            if (e.Column.Caption == "Tanggal" || e.Column.Caption == "Poli" || e.Column.Caption == "Pasien" || e.Column.Caption == "Kecelakaan kerja" || e.Column.Caption == "Berobat" || e.Column.Caption == "Status" || e.Column.Caption == "Remark" || e.Column.Caption == "Mulai Tunda" || e.Column.Caption == "Selesai Tunda")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[10]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "U");
                    simpleButton2.Enabled = true;
                }
            }
            gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                     "Message",
                      MessageBoxButtons.YesNo,
                      MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string sql_delete = "", nik = "", tgl="", que="";

                tgl = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();

                sql_delete = "";

                sql_delete = sql_delete + " delete cs_visit ";
                sql_delete = sql_delete + " where to_char(visit_date,'yyyy-mm-dd') = '" + tgl + "' and que01 = '" + que + "' and patient_no = '" + nik + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);
                    LoadData();
                    MessageBox.Show("Data Berhasil didelete");
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
                LoadData();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "reservation.xls",
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string tgl = "", que = "", nik = "", nama = "", poli = "", pasien = "", workA = "", purpose = "", status = "", action = "", cek="", remark = "", stat = "";
            string sql_check = "", sql_cnt = "", sql_insert="", sql_update = "", c_que = "", tmp_queue= "", visit_cnt="", s_hold="", e_hold="";
            int queue = 0, visit=0;
            DateTime result;

            cek = "";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                tgl = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                que = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                nik = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                nama = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                poli = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
                pasien = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                workA = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();
                purpose = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();
                status = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                action = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                remark = gridView1.GetRowCellValue(i, gridView1.Columns[12]).ToString();
                stat = gridView1.GetRowCellValue(i, gridView1.Columns[14]).ToString();
                s_hold = gridView1.GetRowCellValue(i, gridView1.Columns[15]).ToString();
                e_hold = gridView1.GetRowCellValue(i, gridView1.Columns[16]).ToString();

                if (action == "I")
                {
                    if (tgl == "")
                    {
                        MessageBox.Show("Tanggal harus diisi");
                    }
                    else if (!DateTime.TryParseExact(
                             tgl,
                             "yyyy-MM-dd",
                             CultureInfo.InvariantCulture,
                             DateTimeStyles.AssumeUniversal,
                             out result))
                    {
                        MessageBox.Show("Format tanggal harus yyyy-mm-dd");
                    }
                    else if (nama == "")
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
                        else if (purpose == "MID")
                        {
                            c_que = "M";
                        }
                        else
                        {
                            c_que = "E";
                        }

                        sql_check = " select  nvl(max(to_number(substr(que01,2,3))),0) que from cs_visit where to_char(visit_date,'yyyy-mm-dd')= '" + tgl + "' and purpose = '" + purpose + "' ";

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

                        sql_cnt = " select count(patient_no) cnt from cs_visit where patient_no = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd')= '" + tgl + "' and status not in ('CLS','CAN') ";
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

                            sql_insert = " insert into cs_visit (patient_no, visit_date, status, poli_cd, type_patient, work_accident, purpose, visit_remark, visit_cnt, que01, ins_date, ins_emp, start_hold, end_hold) values ('" + nik + "',to_date('" + tgl + "','yyyy-mm-dd'), '" + status + "', '" + poli + "', '" + pasien + "','" + workA + "', '" + purpose + "', '" + remark + "', '" + Convert.ToString(visit) + "', '" + c_que + que + "' , sysdate, '" + v_empid + "',to_date('" + s_hold + "','yyyy-mm-dd hh24:mi:ss'),to_date('" + e_hold + "','yyyy-mm-dd hh24:mi:ss')) ";

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
                    if (stat == "CLS")
                    {
                        MessageBox.Show("Data Sudah Completed, tidak bisa dirubah.");
                    }
                    else
                    {
                        sql_update = "";

                        sql_update = sql_update + " update cs_visit " +
                                     " set poli_cd = '" + poli + "', type_patient = '" + pasien + "', " +
                                     " work_accident = '" + workA + "', purpose = '" + purpose + "', visit_remark = '" + remark + "', status = '" + status + "', " +
                                     " start_hold = to_date('" + s_hold + "','yyyy-mm-dd hh24:mi:ss'), end_hold = to_date('" + e_hold + "','yyyy-mm-dd hh24:mi:ss'), ";
                        //if (status == "INS")
                        //{
                        //    sql_update = sql_update + " time_reservation = sysdate, ";
                        //}
                        sql_update = sql_update + " upd_emp = '" + v_empid + "', upd_date = sysdate ";
                        sql_update = sql_update + " where que01 = '" + que + "' and patient_no = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + tgl + "' ";

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
            }
            richTextBox1.Text = cek;
            //MessageBox.Show(action);
            LoadData();
        }
    }
}
