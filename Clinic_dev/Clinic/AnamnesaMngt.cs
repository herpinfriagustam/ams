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
    public partial class AnamnesaMngt : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Kehamilan> listKehamilan = new List<Kehamilan>();
        List<FlagYn> listFlagYn = new List<FlagYn>();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string workingDirectory = "", resourcesDirectory = "";

        public AnamnesaMngt()
        {
            InitializeComponent();
        }

        private void ReservationInput_Load(object sender, EventArgs e)
        {
            workingDirectory = Environment.CurrentDirectory;
            resourcesDirectory = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\";
            dDateBgn.Text = today;
            dDateEnd.Text = today;
            initData();
            //LoadData();
        }

        private void LoadData()
        {
            string sql_search;

            sql_search = " ";
            sql_search = sql_search + Environment.NewLine + " SELECT  TO_CHAR (insp_date, 'yyyy-mm-dd') insp_date, visit_no, c.patient_no, ";
            sql_search = sql_search + Environment.NewLine + "         c.NAME, null dept, b.rm_no, blood_press, pulse, temperature, allergy, ";
            sql_search = sql_search + Environment.NewLine + "         anamnesa, disease_now, disease_then, disease_family, ";
            sql_search = sql_search + Environment.NewLine + "         anamnesa_physical, anamnesa_other,  ";
            sql_search = sql_search + Environment.NewLine + "         decode(infop1,'Rujukan','Y','N') infop1,  ";
            sql_search = sql_search + Environment.NewLine + "         decode(infop2,'Tindakan','Y','N') infop2,  ";
            sql_search = sql_search + Environment.NewLine + "         decode(infop3,'Rekomendasi','Y','N') infop3,  ";
            sql_search = sql_search + Environment.NewLine + "         decode(infop4,'Observasi','Y','N') infop4, ";
            sql_search = sql_search + Environment.NewLine + "         decode(infop5,'Terapi','Y','N') infop5, info_k, 'S' action, anamnesa_id, bb, tb, "; 
            sql_search = sql_search + Environment.NewLine + "         cholesterol, blood_sugar, uric_acid  ";
            sql_search = sql_search + Environment.NewLine + "    FROM KLINIK.cs_anamnesa a JOIN KLINIK.cs_patient b ON a.rm_no = b.rm_no ";
            sql_search = sql_search + Environment.NewLine + "         JOIN KLINIK.cs_patient_info c ON b.patient_no = c.patient_no ";
            sql_search = sql_search + Environment.NewLine + "   WHERE 1 = 1 ";
            sql_search = sql_search + Environment.NewLine + "     AND b.status = 'A' ";
            sql_search = sql_search + Environment.NewLine + "     AND TRUNC (insp_date) BETWEEN TO_DATE ('" + dDateBgn.Text + "', 'yyyy-mm-dd') ";
            sql_search = sql_search + Environment.NewLine + "                               AND TO_DATE ('" + dDateEnd.Text + "', 'yyyy-mm-dd') ";
            sql_search = sql_search + Environment.NewLine + " ORDER BY insp_date, visit_no ";



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
                gridView1.OptionsView.ColumnAutoWidth = false;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 50;
                //gridView1.OptionsBehavior.Editable = false;
                //gridView1.BestFitColumns();
                gridView1.FixedLineWidth = 6;
                gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[5].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                gridView1.Columns[1].OptionsColumn.ReadOnly = true;
                gridView1.Columns[2].OptionsColumn.ReadOnly = true;
                gridView1.Columns[3].OptionsColumn.ReadOnly = true;
                gridView1.Columns[4].OptionsColumn.ReadOnly = true;
                gridView1.Columns[5].OptionsColumn.ReadOnly = true;
                gridView1.Columns[22].OptionsColumn.ReadOnly = true;

                gridView1.Columns[0].Caption = "Tanggal";
                gridView1.Columns[1].Caption = "Antrian";
                gridView1.Columns[2].Caption = "Pasien No";
                gridView1.Columns[3].Caption = "Nama";
                gridView1.Columns[4].Caption = "Dept";
                gridView1.Columns[5].Caption = "Med. Record No";
                gridView1.Columns[6].Caption = "Tensi";
                gridView1.Columns[7].Caption = "Nadi";
                gridView1.Columns[8].Caption = "Suhu";
                gridView1.Columns[9].Caption = "Alergi";
                gridView1.Columns[10].Caption = "Keluhan";
                gridView1.Columns[11].Caption = "RP Sekarang";
                gridView1.Columns[12].Caption = "RP Dahulu";
                gridView1.Columns[13].Caption = "RP Keluarga";
                gridView1.Columns[14].Caption = "Pemeriksaan Fisik";
                gridView1.Columns[15].Caption = "Pemeriksaan Tambahan";
                gridView1.Columns[16].Caption = "Rujukan";
                gridView1.Columns[17].Caption = "Tindakan";
                gridView1.Columns[18].Caption = "Rekomendasi";
                gridView1.Columns[19].Caption = "Observasi";
                gridView1.Columns[20].Caption = "Terapi";
                gridView1.Columns[21].Caption = "Info";
                gridView1.Columns[22].Caption = "Action";
                gridView1.Columns[23].Caption = "ID";
                gridView1.Columns[24].Caption = "BB (Kg)";
                gridView1.Columns[25].Caption = "TB (Cm)";
                gridView1.Columns[26].Caption = "Kolesterol (Mg)";
                gridView1.Columns[27].Caption = "Gula Darah (Mg)";
                gridView1.Columns[28].Caption = "Asam Urat (Mg)";

                gridView1.Columns[0].Width = 80;
                gridView1.Columns[1].Width = 50;
                gridView1.Columns[2].Width = 80;
                gridView1.Columns[3].Width = 150;
                gridView1.Columns[4].Width = 150;
                gridView1.Columns[5].Width = 150;
                gridView1.Columns[6].Width = 80;
                gridView1.Columns[7].Width = 80;
                gridView1.Columns[8].Width = 80;
                gridView1.Columns[9].Width = 150;
                gridView1.Columns[10].Width = 300;
                gridView1.Columns[11].Width = 100;
                gridView1.Columns[12].Width = 100;
                gridView1.Columns[13].Width = 100;
                gridView1.Columns[14].Width = 150;
                gridView1.Columns[15].Width = 150;
                gridView1.Columns[16].Width = 80;
                gridView1.Columns[17].Width = 80;
                gridView1.Columns[18].Width = 80;
                gridView1.Columns[19].Width = 80;
                gridView1.Columns[20].Width = 80;
                gridView1.Columns[21].Width = 80;
                gridView1.Columns[22].Width = 80;
                gridView1.Columns[23].Width = 50;
                gridView1.Columns[24].Width = 60;
                gridView1.Columns[25].Width = 60;
                gridView1.Columns[26].Width = 100;
                gridView1.Columns[27].Width = 100;
                gridView1.Columns[28].Width = 100;

                RepositoryItemLookUpEdit kLookup = new RepositoryItemLookUpEdit();
                kLookup.DataSource = listKehamilan;
                kLookup.ValueMember = "kehamilanCode";
                kLookup.DisplayMember = "kehamilanName";

                kLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                kLookup.DropDownRows = listKehamilan.Count;
                kLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                kLookup.AutoSearchColumnIndex = 1;
                kLookup.NullText = "";
                gridView1.Columns[21].ColumnEdit = kLookup;

                RepositoryItemLookUpEdit statLookup = new RepositoryItemLookUpEdit();
                statLookup.DataSource = listFlagYn;
                statLookup.ValueMember = "flagCode";
                statLookup.DisplayMember = "flagName";

                statLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statLookup.DropDownRows = listFlagYn.Count;
                statLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statLookup.AutoSearchColumnIndex = 1;
                statLookup.NullText = "";
                gridView1.Columns[16].ColumnEdit = statLookup;
                gridView1.Columns[17].ColumnEdit = statLookup;
                gridView1.Columns[18].ColumnEdit = statLookup;
                gridView1.Columns[19].ColumnEdit = statLookup;
                gridView1.Columns[20].ColumnEdit = statLookup;

                gridView1.Columns[4].Visible = false;
                gridView1.Columns[22].Visible = false;
                gridView1.Columns[23].Visible = false;
                gridView1.Columns[3].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[4].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                gridView1.Columns[24].VisibleIndex = 9;
                gridView1.Columns[25].VisibleIndex = 10;

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
            listKehamilan.Clear();
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K1", kehamilanName = "K1" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K2", kehamilanName = "K2" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K3", kehamilanName = "K3" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K4", kehamilanName = "K4" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K5", kehamilanName = "K5" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K6", kehamilanName = "K6" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K7", kehamilanName = "K7" });

            listFlagYn.Clear();
            listFlagYn.Add(new FlagYn() { flagCode = "N", flagName = "No" });
            listFlagYn.Add(new FlagYn() { flagCode = "Y", flagName = "Yes" });
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


            if (e.Column.Caption == "Tanggal" || e.Column.Caption == "Antrian" || e.Column.Caption == "Pasien No" || e.Column.Caption == "Tensi" || e.Column.Caption == "Nadi" || e.Column.Caption == "Suhu" || e.Column.Caption == "BB (Kg)" || e.Column.Caption == "TB (Cm)" || e.Column.Caption == "Alergi" || e.Column.Caption == "Keluhan")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }

            if (e.Column.Caption == "Rujukan")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[16]);
                if (kk == "Yes")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Blue);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.Blue);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.Caption == "Tindakan")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[17]);
                if (kk == "Yes")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Blue);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.Blue);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.Caption == "Rekomendasi")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[18]);
                if (kk == "Yes")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Blue);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.Blue);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.Caption == "Observasi")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[19]);
                if (kk == "Yes")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Blue);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.Blue);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.Caption == "Terapi")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[20]);
                if (kk == "Yes")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Blue);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.Blue);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
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
            gridView1.Columns[1].OptionsColumn.ReadOnly = false;
            gridView1.Columns[2].OptionsColumn.ReadOnly = false;
            gridView1.AddNewRow();
            //gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            //GridView view = sender as GridView;
            //view.SetRowCellValue(e.RowHandle, view.Columns[0], "D010");
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            
            if (e.Column.Caption == "Pasien No")
            {
                string p_empid = e.Value.ToString();
                string empid = "", name = "", dept = "", rm="", grp="";
                string sql_emp = "";

                string tmp_pasno = view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();
                string tmp_que = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string tmp_date = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();

                string SQL = "";
                SQL = SQL + Environment.NewLine + "select (select poli_group from KLINIK.CS_POLICLINIC where poli_cd=v.poli_cd) p_group ";
                SQL = SQL + Environment.NewLine + "from KLINIK.cs_visit v ";
                SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm-dd')='" + tmp_date + "' ";
                SQL = SQL + Environment.NewLine + "and que01='" + tmp_que + "' ";
                SQL = SQL + Environment.NewLine + "and patient_no='" + tmp_pasno + "' ";
                SQL = SQL + Environment.NewLine + "and poli_cd is not null ";

                OleDbConnection sqlConnect1 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql1 = new OleDbDataAdapter(SQL, sqlConnect1);
                DataTable dt1 = new DataTable();
                adSql1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    grp = dt1.Rows[0]["p_group"].ToString();
                }
                else
                {
                    grp = "";
                    view.SetColumnError(gridView1.Columns[2], "Data Reservasi tidak ditemukan");
                }


                sql_emp = sql_emp + Environment.NewLine + "select a.patient_no, name, null dept, ";
                sql_emp = sql_emp + Environment.NewLine + "(select rm_no from KLINIK.cs_patient where status='A' and patient_no=a.patient_no and group_patient='" + grp + "') rm_no  ";
                sql_emp = sql_emp + Environment.NewLine + "from KLINIK.cs_patient_info  a ";
                sql_emp = sql_emp + Environment.NewLine + "where 1=1  ";
                sql_emp = sql_emp + Environment.NewLine + "and a.patient_no = '" + p_empid + "' ";


                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_emp, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    name = dt.Rows[0]["name"].ToString();
                    dept = dt.Rows[0]["dept"].ToString();
                    rm = dt.Rows[0]["rm_no"].ToString();
                }
                else
                {
                    empid = ""; dept = "";
                    view.SetColumnError(gridView1.Columns[2], "Data pasien tidak ditemukan");
                }

                view.SetRowCellValue(e.RowHandle, view.Columns[3], name);
                view.SetRowCellValue(e.RowHandle, view.Columns[4], dept);
                view.SetRowCellValue(e.RowHandle, view.Columns[5], rm);

                view.SetRowCellValue(e.RowHandle, view.Columns[16], "N");
                view.SetRowCellValue(e.RowHandle, view.Columns[17], "N");
                view.SetRowCellValue(e.RowHandle, view.Columns[18], "N");
                view.SetRowCellValue(e.RowHandle, view.Columns[19], "N");
                view.SetRowCellValue(e.RowHandle, view.Columns[20], "N");
                view.SetRowCellValue(e.RowHandle, view.Columns[22], "I");
            }

            if (e.Column.Caption == "Tensi" || e.Column.Caption == "Nadi" || e.Column.Caption == "Suhu" || e.Column.Caption == "BB (Kg)" || e.Column.Caption == "TB (Cm)" || e.Column.Caption == "Alergi" || e.Column.Caption == "Keluhan" ||
                e.Column.Caption == "RP Sekarang" || e.Column.Caption == "RP Dahulu" || e.Column.Caption == "RP Keluarga" || e.Column.Caption == "Pemeriksaan Fisik" || e.Column.Caption == "Pemeriksaan Tambahan" ||
                e.Column.Caption == "Rujukan" || e.Column.Caption == "Tindakan" || e.Column.Caption == "Rekomendasi" || e.Column.Caption == "Observasi" || e.Column.Caption == "Terapi" || e.Column.Caption == "Info" ||
                e.Column.Caption == "Kolesterol (Mg)" || e.Column.Caption == "Gula Darah (Mg)" || e.Column.Caption == "Asam Urat (Mg)")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[22]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[22], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[22], "U");
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
                string sql_delete = "", nik = "", tgl="", que="", id="";

                tgl = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
                id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[23]).ToString();

                sql_delete = "";

                sql_delete = sql_delete + " delete KLINIK.cs_anamnesa ";
                sql_delete = sql_delete + " where anamnesa_id = '" + id + "' ";

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

            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string tgl = "", que = "", nik = "", nama = "", rm_no = "", tensi = "", nadi = "", suhu = "", alergi = "", keluhan = "",  rp_now="", rp_then = "";
            string rp_fam = "", fisik = "", tambah = "", rujukan = "", tindakan = "", rekom = "", obs = "", terapi = "", info = "", action = "";
            string tmp_rujuk = "", tmp_tindakan = "", tmp_rekom = "", tmp_obs = "", tmp_terapi = "", tmp_bb = "", tmp_tb = "";
            string sql_check = "", sql_cnt = "", sql_insert = "", sql_update = "", cek="", anam_cnt="", id = "", sql_cnt2 = "", visit_cnt = "" ;
            string chol = "", bsugar = "", uacid = "";

            DateTime result;

            cek = "";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                tgl = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                que = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                nik = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                nama = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                rm_no = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
                tensi = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                nadi = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();
                suhu = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();
                alergi = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                keluhan = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                rp_now = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
                rp_then = gridView1.GetRowCellValue(i, gridView1.Columns[12]).ToString();
                rp_fam = gridView1.GetRowCellValue(i, gridView1.Columns[13]).ToString();
                fisik = gridView1.GetRowCellValue(i, gridView1.Columns[14]).ToString();
                tambah = gridView1.GetRowCellValue(i, gridView1.Columns[15]).ToString();
                rujukan = gridView1.GetRowCellValue(i, gridView1.Columns[16]).ToString();
                tindakan = gridView1.GetRowCellValue(i, gridView1.Columns[17]).ToString();
                rekom = gridView1.GetRowCellValue(i, gridView1.Columns[18]).ToString();
                obs = gridView1.GetRowCellValue(i, gridView1.Columns[19]).ToString();
                terapi = gridView1.GetRowCellValue(i, gridView1.Columns[20]).ToString();
                info = gridView1.GetRowCellValue(i, gridView1.Columns[21]).ToString();
                action = gridView1.GetRowCellValue(i, gridView1.Columns[22]).ToString();
                id = gridView1.GetRowCellValue(i, gridView1.Columns[23]).ToString();
                tmp_bb = gridView1.GetRowCellValue(i, gridView1.Columns[24]).ToString();
                tmp_tb = gridView1.GetRowCellValue(i, gridView1.Columns[25]).ToString();
                chol = gridView1.GetRowCellValue(i, gridView1.Columns[26]).ToString();
                bsugar = gridView1.GetRowCellValue(i, gridView1.Columns[27]).ToString();
                uacid = gridView1.GetRowCellValue(i, gridView1.Columns[28]).ToString();

                if (rujukan == "Y") { tmp_rujuk = "Rujukan"; } else { tmp_rujuk = ""; }
                if (tindakan == "Y") { tmp_tindakan = "Tindakan"; } else { tmp_tindakan = ""; }
                if (rekom == "Y") { tmp_rekom = "Rekomendasi"; } else { tmp_rekom = ""; }
                if (obs == "Y") { tmp_obs = "Observasi"; } else { tmp_obs = ""; }
                if (terapi == "Y") { tmp_terapi = "Terapi"; } else { tmp_terapi = ""; }

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
                    else if (que == "")
                    {
                        MessageBox.Show("Antrian Harus diisi");
                    }
                    else if (tensi == "")
                    {
                        MessageBox.Show("Tensi Harus diisi");
                    }
                    else if (nadi == "")
                    {
                        MessageBox.Show("Nadi Harus diisi");
                    }
                    else if (tmp_bb == "")
                    {
                        MessageBox.Show("BB Harus diisi");
                    }
                    else if (tmp_tb == "")
                    {
                        MessageBox.Show("TB Harus diisi");
                    }
                    else if (keluhan == "")
                    {
                        MessageBox.Show("Keluhan Harus diisi");
                    }
                    else
                    {
                        sql_cnt2 = " ";

                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "select count(0) cnt from KLINIK.cs_visit ";
                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "where 1=1 ";
                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "and patient_no='" + nik + "' ";
                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "and to_char(visit_date,'yyyy-mm-dd') = '" + tgl + "' ";
                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "and que01='" + que + "' ";
                        OleDbConnection oraConnect2a = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra2a = new OleDbDataAdapter(sql_cnt2, oraConnect2a);
                        DataTable dt2a = new DataTable();
                        adOra2a.Fill(dt2a);
                        visit_cnt = dt2a.Rows[0]["cnt"].ToString();


                        //sql_cnt = " ";

                        //if (Convert.ToInt32(visit_cnt) > 0)
                        //{
                        //    sql_cnt = sql_cnt + Environment.NewLine + "select a.rm_no, count(0) cnt from cs_anamnesa a ";
                        //    sql_cnt = sql_cnt + Environment.NewLine + "join cs_patient b on a.rm_no=b.rm_no ";
                        //    sql_cnt = sql_cnt + Environment.NewLine + "join cs_employees c on b.empid=c.empid ";
                        //    sql_cnt = sql_cnt + Environment.NewLine + "where b.status='A' ";
                        //    sql_cnt = sql_cnt + Environment.NewLine + "and c.empid='" + nik + "' ";
                        //    sql_cnt = sql_cnt + Environment.NewLine + "and to_char(insp_date,'yyyy-mm-dd') = '" + tgl + "' ";
                        //    sql_cnt = sql_cnt + Environment.NewLine + "and visit_no='" + que + "' ";
                        //    sql_cnt = sql_cnt + Environment.NewLine + "group by a.rm_no ";

                        //    OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                        //    OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_cnt, oraConnect2);
                        //    DataTable dt2 = new DataTable();
                        //    adOra2.Fill(dt2);
                        //    anam_cnt = dt2.Rows[0]["cnt"].ToString();
                        //    rm_no = dt2.Rows[0]["rm_no"].ToString();
                        //}

                        if (Convert.ToInt32(visit_cnt) == 0)
                        {
                            MessageBox.Show("Data Reservasi tidak ditemukan, silahkan input data Reservasi");
                        }
                        else if (rm_no == "")
                        {
                            MessageBox.Show("Data Medical Record tidak ditemukan, silahkan input data Medical Record");
                        }
                        else
                        {

                            sql_insert = "  ";
                            
                            sql_insert = sql_insert + Environment.NewLine + "insert into KLINIK.cs_anamnesa ";
                            sql_insert = sql_insert + Environment.NewLine + "(anamnesa_id, rm_no, insp_date, blood_press, pulse, temperature, ";
                            sql_insert = sql_insert + Environment.NewLine + "allergy, anamnesa, disease_now, disease_then, disease_family, ";
                            sql_insert = sql_insert + Environment.NewLine + "anamnesa_physical, anamnesa_other, visit_no, info_k, ";
                            sql_insert = sql_insert + Environment.NewLine + "infop1, infop2, infop3, infop4, infop5,bb,tb,  ";
                            sql_insert = sql_insert + Environment.NewLine + "cholesterol, blood_sugar, uric_acid,  ";
                            sql_insert = sql_insert + Environment.NewLine + "ins_date, ins_emp) ";
                            sql_insert = sql_insert + Environment.NewLine + "values  ";
                            sql_insert = sql_insert + Environment.NewLine + "(cs_anamnesa_seq.nextval,'" + rm_no + "',to_date('" + tgl + "','yyyy-mm-dd'),'" + tensi + "','" + nadi + "','" + suhu + "', ";
                            sql_insert = sql_insert + Environment.NewLine + " '" + alergi + "','" + keluhan + "','" + rp_now + "','" + rp_then + "','" + rp_fam + "','" + fisik + "', ";
                            sql_insert = sql_insert + Environment.NewLine + " '" + tambah + "','" + que + "','" + info + "','" + tmp_rujuk + "','" + tmp_tindakan + "','" + tmp_rekom + "', ";
                            sql_insert = sql_insert + Environment.NewLine + " '" + tmp_obs + "','" + tmp_terapi + "','" + tmp_bb + "','" + tmp_tb + "','" + chol + "','" + bsugar + "','" + uacid + "', sysdate, '" + v_empid + "') ";

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
                                LoadData();
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

                    if (rujukan == "Y") { tmp_rujuk = "Rujukan"; } else { tmp_rujuk = ""; }
                    if (tindakan == "Y") { tmp_tindakan = "Tindakan"; } else { tmp_tindakan = ""; }
                    if (rekom == "Y") { tmp_rekom = "Rekomendasi"; } else { tmp_rekom = ""; }
                    if (obs == "Y") { tmp_obs = "Observasi"; } else { tmp_obs = ""; }
                    if (terapi == "Y") { tmp_terapi = "Terapi"; } else { tmp_terapi = ""; }

                    sql_update = "";

                    sql_update = sql_update + Environment.NewLine + "update KLINIK.cs_anamnesa ";
                    sql_update = sql_update + Environment.NewLine + "set blood_press = '" + tensi + "', pulse = '" + nadi + "', temperature = '" + suhu + "', ";
                    sql_update = sql_update + Environment.NewLine + "allergy = '" + alergi + "', anamnesa = '" + keluhan + "', disease_now = '" + rp_now + "', disease_then = '" + rp_then + "', ";
                    sql_update = sql_update + Environment.NewLine + "disease_family = '" + rp_fam + "', anamnesa_physical = '" + fisik + "', anamnesa_other = '" + tambah + "', ";
                    sql_update = sql_update + Environment.NewLine + "info_k = '" + info + "', infop1 = '" + tmp_rujuk + "', infop2 = '" + tmp_tindakan + "', infop3 = '" + tmp_rekom + "', infop4 = '" + tmp_obs + "',  ";
                    sql_update = sql_update + Environment.NewLine + "infop5 = '" + tmp_terapi + "', bb='" + tmp_bb + "', tb='" + tmp_tb + "', cholesterol='" + chol + "', blood_sugar='" + bsugar + "', uric_acid='" + uacid + "', upd_date = sysdate, upd_emp = '" + v_empid + "' ";
                    sql_update = sql_update + Environment.NewLine + "where anamnesa_id = '" + id + "' ";


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
                        LoadData();
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
        }
    }
}
