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
    public partial class MedicineMngt : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Medicine> listMedicine = new List<Medicine>();
        List<MedicineInfo> listMedicineInfo = new List<MedicineInfo>();
        List<Formula2> listFormula2 = new List<Formula2>();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string workingDirectory = "", resourcesDirectory = "", period="";

        public MedicineMngt()
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

            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "SELECT   TO_CHAR (insp_date, 'yyyy-mm-dd') insp_date, visit_no, c.patient_no, ";
            sql_search = sql_search + Environment.NewLine + "         c.NAME, null dept, b.rm_no, a.med_cd, initcap(med_name) med_name, formula, type_drink, med_qty, ";
            sql_search = sql_search + Environment.NewLine + "         klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) + ";
            sql_search = sql_search + Environment.NewLine + "         klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -  ";
            sql_search = sql_search + Environment.NewLine + "         klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) -  ";
            sql_search = sql_search + Environment.NewLine + "         klinik.FN_CS_REQ_STOCK(a.insp_date,a.med_cd) stock,  ";
            sql_search = sql_search + Environment.NewLine + "         initcap(uom) uom, confirm, 'S' action, receipt_id, price, qty_day, days, dosis ";
            sql_search = sql_search + Environment.NewLine + "    FROM cs_receipt a JOIN cs_patient b ON a.rm_no = b.rm_no ";
            sql_search = sql_search + Environment.NewLine + "         JOIN cs_patient_info c ON b.patient_no = c.patient_no ";
            sql_search = sql_search + Environment.NewLine + "         JOIN cs_medicine d ON a.med_cd=d.med_cd ";
            sql_search = sql_search + Environment.NewLine + "   WHERE 1 = 1 ";
            sql_search = sql_search + Environment.NewLine + "     AND b.status = 'A' ";
            sql_search = sql_search + Environment.NewLine + "     AND to_char(insp_date,'yyyymm') = '" + period + "' ";
            sql_search = sql_search + Environment.NewLine + "     AND TRUNC (insp_date) BETWEEN TO_DATE ('" + dDateBgn.Text + "', 'yyyy-mm-dd') ";
            sql_search = sql_search + Environment.NewLine + "                               AND TO_DATE ('" + dDateEnd.Text + "', 'yyyy-mm-dd') ";
            sql_search = sql_search + Environment.NewLine + "     AND c.patient_no  like '" + tNik.Text + "%' ";
            sql_search = sql_search + Environment.NewLine + "ORDER BY insp_date, visit_no, a.med_cd ";



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
                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 50;
                //gridView1.OptionsBehavior.Editable = false;
                gridView1.BestFitColumns();

                gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                gridView1.Columns[1].OptionsColumn.ReadOnly = true;
                gridView1.Columns[2].OptionsColumn.ReadOnly = true;
                gridView1.Columns[3].OptionsColumn.ReadOnly = true;
                gridView1.Columns[4].OptionsColumn.ReadOnly = true;
                gridView1.Columns[5].OptionsColumn.ReadOnly = true;
                gridView1.Columns[7].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[8].OptionsColumn.ReadOnly = true;
                gridView1.Columns[11].OptionsColumn.ReadOnly = true;
                gridView1.Columns[12].OptionsColumn.ReadOnly = true;
                gridView1.Columns[13].OptionsColumn.ReadOnly = true;
                gridView1.Columns[14].OptionsColumn.ReadOnly = true;
                gridView1.Columns[15].OptionsColumn.ReadOnly = true;
                gridView1.Columns[16].OptionsColumn.ReadOnly = true;
                gridView1.Columns[17].OptionsColumn.ReadOnly = true;

                gridView1.Columns[0].Caption = "Tanggal";
                gridView1.Columns[1].Caption = "Antrian";
                gridView1.Columns[2].Caption = "Pasien No";
                gridView1.Columns[3].Caption = "Nama";
                gridView1.Columns[4].Caption = "Dept";
                gridView1.Columns[5].Caption = "Med. Record No";
                gridView1.Columns[6].Caption = "Kode Obat";
                gridView1.Columns[7].Caption = "Nama Obat";
                gridView1.Columns[8].Caption = "Kode Dosis";
                gridView1.Columns[9].Caption = "Info";
                gridView1.Columns[10].Caption = "Jumlah";
                gridView1.Columns[11].Caption = "Stok";
                gridView1.Columns[12].Caption = "Satuan";
                gridView1.Columns[13].Caption = "Confirm";
                gridView1.Columns[14].Caption = "Action";
                gridView1.Columns[15].Caption = "ID";
                gridView1.Columns[16].Caption = "Harga";
                gridView1.Columns[17].Caption = "Jumlah per Hari";
                gridView1.Columns[18].Caption = "Jml";
                gridView1.Columns[19].Caption = "Dosis";

                gridView1.Columns[16].VisibleIndex = 12;
                gridView1.Columns[18].VisibleIndex = 11;
                gridView1.Columns[19].VisibleIndex = 10;

                gridView1.Columns[0].MinWidth = 70;
                gridView1.Columns[0].MaxWidth = 70;
                gridView1.Columns[1].MinWidth = 60;
                gridView1.Columns[1].MaxWidth = 60;
                gridView1.Columns[2].MinWidth = 80;
                gridView1.Columns[2].MaxWidth = 80;
                gridView1.Columns[5].MinWidth = 100;
                gridView1.Columns[5].MaxWidth = 100;
                gridView1.Columns[6].MinWidth = 80;
                gridView1.Columns[6].MaxWidth = 80;
                gridView1.Columns[7].MinWidth = 150;
                gridView1.Columns[7].MaxWidth = 150;
                gridView1.Columns[9].MinWidth = 100;
                gridView1.Columns[9].MaxWidth = 100;
                gridView1.Columns[10].MinWidth = 60;
                gridView1.Columns[10].MaxWidth = 60;
                gridView1.Columns[11].MinWidth = 60;
                gridView1.Columns[11].MaxWidth = 60;
                gridView1.Columns[12].MinWidth = 60;
                gridView1.Columns[12].MaxWidth = 60;
                gridView1.Columns[13].MinWidth = 60;
                gridView1.Columns[13].MaxWidth = 60;

                RepositoryItemLookUpEdit medicineLookup = new RepositoryItemLookUpEdit();
                medicineLookup.DataSource = listMedicine;
                medicineLookup.ValueMember = "medicineCode";
                medicineLookup.DisplayMember = "medicineCode";

                medicineLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                medicineLookup.DropDownRows = listMedicine.Count;
                medicineLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                medicineLookup.AutoSearchColumnIndex = 1;
                medicineLookup.NullText = "";
                gridView1.Columns[6].ColumnEdit = medicineLookup;

                string sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from cs_formula a join cs_medicine b on(a.med_cd=b.med_cd) where 1=1 ";
                OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
                DataTable dtf = new DataTable();
                adOraf.Fill(dtf);
                //listFormula.Clear();
                listFormula2.Clear();
                for (int i = 0; i < dtf.Rows.Count; i++)
                {
                    listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                }

                RepositoryItemGridLookUpEdit glfor = new RepositoryItemGridLookUpEdit();
                glfor.DataSource = listFormula2;
                glfor.ValueMember = "formulaCode";
                glfor.DisplayMember = "formulaName";

                glfor.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glfor.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                glfor.ImmediatePopup = true;
                glfor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glfor.NullText = "";
                gridView1.Columns[8].ColumnEdit = glfor;

                RepositoryItemLookUpEdit medicineInfoLookup = new RepositoryItemLookUpEdit();
                medicineInfoLookup.DataSource = listMedicineInfo;
                medicineInfoLookup.ValueMember = "medicineInfoCode";
                medicineInfoLookup.DisplayMember = "medicineInfoName";

                medicineInfoLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                medicineInfoLookup.DropDownRows = listMedicineInfo.Count;
                medicineInfoLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                medicineInfoLookup.AutoSearchColumnIndex = 1;
                medicineInfoLookup.NullText = "";
                gridView1.Columns[9].ColumnEdit = medicineInfoLookup;

                gridView1.Columns[4].Visible = false;
                gridView1.Columns[10].Visible = false;
                gridView1.Columns[17].Visible = false;
                gridView1.Columns[12].Visible = false;
                gridView1.Columns[14].Visible = false;
                gridView1.Columns[15].Visible = false;
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
            string sql_stok = " select max(period)+1 as period from cs_medicine_stok ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_stok, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            period = dt.Rows[0]["period"].ToString();

            string sql_med = " select med_cd, initcap(med_name) || ' (BPJS: ' || bpjs_cover || ')' med_name from cs_medicine where status = 'A' order by med_cd asc ";
            OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_med, sqlConnect3);
            DataTable dt3 = new DataTable();
            adSql3.Fill(dt3);
            listMedicine.Clear();
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                listMedicine.Add(new Medicine() { medicineCode = dt3.Rows[i]["med_cd"].ToString(), medicineName = dt3.Rows[i]["med_name"].ToString() });
            }

            listMedicineInfo.Clear();
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "A", medicineInfoName = "(P.C.) Sesudah Makan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "B", medicineInfoName = "(A.C.) Sebelum Makan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "C", medicineInfoName = "(P.R.N.) Bila Perlu" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "D", medicineInfoName = "(G.T.T) Diteteskan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "E", medicineInfoName = "(U.E) Pemakaian Luar" });

            string sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from cs_formula a join cs_medicine b on(a.med_cd=b.med_cd) where 1=1 ";
            OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
            DataTable dtf = new DataTable();
            adOraf.Fill(dtf);

            listFormula2.Clear();
            for (int i = 0; i < dtf.Rows.Count; i++)
            {
                listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
            }
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

            if (e.Column.Caption == "Tanggal" || e.Column.Caption == "Antrian" || e.Column.Caption == "Pasien No" || e.Column.Caption == "Kode Obat" || 
                e.Column.Caption == "Dosis" || e.Column.Caption == "Info" || e.Column.Caption == "Jumlah" || e.Column.Caption == "Kode Dosis" || e.Column.Caption == "Jml")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }

            if (e.Column.Caption == "Stok")
            {
                string stok = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);

                if (stok != "")
                {
                    if (Convert.ToInt16(stok) == 0)
                    {
                        e.Appearance.BackColor = Color.Crimson;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (Convert.ToInt16(stok) <= 20)
                    {
                        e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
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
                string empid = "", name = "", dept = "", rm="", grp = "";
                string sql_emp = "";

                string tmp_pasno = view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();
                string tmp_que = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string tmp_date = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();

                string SQL = "";
                SQL = SQL + Environment.NewLine + "select (select poli_group from CS_POLICLINIC where poli_cd=v.poli_cd) p_group ";
                SQL = SQL + Environment.NewLine + "from cs_visit v ";
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
                sql_emp = sql_emp + Environment.NewLine + "(select rm_no from cs_patient where status='A' and patient_no=a.patient_no and group_patient='" + grp + "') rm_no  ";
                sql_emp = sql_emp + Environment.NewLine + "from cs_patient_info  a ";
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
                
                view.SetRowCellValue(e.RowHandle, view.Columns[14], "I");
            }

            if (e.Column.Caption == "Kode Obat")
            {
                string tmp_stat2 = view.GetRowCellValue(e.RowHandle, view.Columns[14]).ToString();
                string a = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                string b = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();

                string sql_med = "", med_cd = "", med_name = "", med_group = "", med_stok = "", med_uom = "";

                sql_med = " select med_cd, initcap(med_name) med_name, " +
                          //" stock - (select nvl(SUM(med_qty),0) from cs_receipt  " +
                          //"           where TO_CHAR(insp_date, 'yyyy-mm-dd') = '" + lMedDate.Text + "'  " +
                          //"             and confirm = 'N'  " +
                          //"             and med_cd = a.med_cd) stock, uom  " +
                          " klinik.FN_CS_INIT_STOCK(to_date('" + period + "','yyyymm'),'" + a + "') +  " +
                          " klinik.FN_CS_TRX_IN(to_date('" + period + "','yyyymm'),'" + a + "') -  " +
                          " klinik.FN_CS_TRX_OUT(to_date('" + period + "','yyyymm'),'" + a + "') - " +
                          " klinik.FN_CS_REQ_STOCK(to_date('" + period + "','yyyymm'),'" + a + "') stock, initcap(uom) uom " +
                          " from cs_medicine a  " +
                          " where status = 'A'  " +
                          " and med_cd = '" + a + "'";

                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_med, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                med_cd = dt.Rows[0]["med_cd"].ToString();
                med_name = dt.Rows[0]["med_name"].ToString();
                med_stok = dt.Rows[0]["stock"].ToString();
                med_uom = dt.Rows[0]["uom"].ToString();

                string sql_for = "";
                sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from cs_formula a join cs_medicine b on(a.med_cd=b.med_cd) where 1=1  and  b.med_cd = '" + med_cd + "' ";
                OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
                DataTable dtf = new DataTable();
                adOraf.Fill(dtf);
                //listFormula.Clear();
                listFormula2.Clear();
                for (int i = 0; i < dtf.Rows.Count; i++)
                {
                    listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                }

                view.SetRowCellValue(e.RowHandle, view.Columns[10], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[8], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[18], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[16], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[17], 0);


                if (tmp_stat2 == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[14], "I");
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], a);
                    view.SetRowCellValue(e.RowHandle, view.Columns[7], med_name);
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], med_stok);
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], "N");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[14], "U");
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], med_stok);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "0");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], "N");
                }
            }

            if (e.Column.Caption == "Kode Dosis")
            {
                string medicine_cd = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string formula_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string reg_dt = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string rm = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string que = view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();
                string stat = view.GetRowCellValue(e.RowHandle, view.Columns[14]).ToString();

                string kode = "", sql_pilihan = "";

                if (stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[18], "");
                    view.SetRowCellValue(e.RowHandle, view.Columns[16], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[17], 0);
                }
                else
                {
                    sql_pilihan = " select med_cd from cs_formula where formula_id = '" + formula_cd + "' ";
                    OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_pilihan, oraConnectf);
                    DataTable dtf = new DataTable();
                    adOraf.Fill(dtf);

                    if (dtf.Rows.Count > 0)
                    {
                        kode = dtf.Rows[0]["med_cd"].ToString();

                    }
                    else
                    {
                        kode = "";
                    }
                    
                    if (kode == medicine_cd)
                    {
                        view.SetRowCellValue(e.RowHandle, view.Columns[10], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[18], "");
                        view.SetRowCellValue(e.RowHandle, view.Columns[16], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[17], 0);
                    }
                    else
                    {
                        MessageBox.Show("Kode Formula tidak valid");
                        return;
                        //LoadDataResep();
                    }
                }


            }

            if (e.Column.Caption == "Jml")
            {
                string sql_for = "", med_price = "", qty = "", tmp_stat = "";
                string for_cd = view.GetRowCellValue(e.RowHandle, view.Columns[8]).ToString();
                string tmp_hari = view.GetRowCellValue(e.RowHandle, view.Columns[18]).ToString();
                int tot_hari = 0, tot_harga = 0;

                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[14]).ToString();

                sql_for = " select med_price, qty from cs_formula where formula_id = '" + for_cd + "' ";
                OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
                DataTable dtf = new DataTable();
                adOraf.Fill(dtf);

                if (dtf.Rows.Count > 0)
                {
                    med_price = dtf.Rows[0]["med_price"].ToString();
                    qty = dtf.Rows[0]["qty"].ToString();
                }
                else
                {
                    med_price = "0";
                    qty = "0";
                }

                if (tmp_hari == "")
                {
                    tmp_hari = "0";
                }

                tot_hari = Convert.ToInt16(tmp_hari) * Convert.ToInt16(qty);
                tot_harga = Convert.ToInt16(tmp_hari) * Convert.ToInt16(med_price);

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[14], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns[16], tot_harga.ToString());
                    view.SetRowCellValue(e.RowHandle, view.Columns[17], qty);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], tot_hari.ToString());
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[14], "U");
                    view.SetRowCellValue(e.RowHandle, view.Columns[16], tot_harga.ToString());
                    view.SetRowCellValue(e.RowHandle, view.Columns[17], qty);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], tot_hari.ToString());
                }
            }

            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Dosis" || e.Column.Caption == "Info" || e.Column.Caption == "Jml" || e.Column.Caption == "Kode Dosis")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[14]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[14], "I");
                    simpleButton2.Enabled = true;
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[14], "U");
                    simpleButton2.Enabled = true;
                }
            }

            gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "receipt.xls",
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
                string sql_delete = "", nik = "", tgl="", que="", id="", confirm="";

                tgl = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
                id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[15]).ToString();
                confirm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[13]).ToString();

                if (confirm == "Y")
                {
                    MessageBox.Show("Data tidak bisa dirubah.");
                }
                else
                {
                    sql_delete = "";

                    sql_delete = sql_delete + " delete cs_receipt ";
                    sql_delete = sql_delete + " where receipt_id = '" + id + "' ";

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
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string tgl = "", que = "", nik = "", nama = "", rm_no = "", kode_obat = "", dosis = "", info = "", jumlah = "", stok="", confirm="", action = "";
            string sql_check = "", sql_cnt = "", sql_insert = "", sql_update = "", cek="", anam_cnt="", id = "", sql_cnt2 = "", visit_cnt = "", cek_dt = "";
            string s_harga = "", s_hari = "", s_jml="", s_formula="";
            DateTime result;

            cek = "";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                tgl = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                que = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                nik = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                nama = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                rm_no = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
                kode_obat = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                dosis = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();
                info = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                jumlah = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                stok = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
                confirm = gridView1.GetRowCellValue(i, gridView1.Columns[13]).ToString();
                action = gridView1.GetRowCellValue(i, gridView1.Columns[14]).ToString();
                id = gridView1.GetRowCellValue(i, gridView1.Columns[15]).ToString();
                s_harga = gridView1.GetRowCellValue(i, gridView1.Columns[16]).ToString();
                s_hari = gridView1.GetRowCellValue(i, gridView1.Columns[17]).ToString();
                s_jml = gridView1.GetRowCellValue(i, gridView1.Columns[18]).ToString();
                s_formula = gridView1.GetRowCellValue(i, gridView1.Columns[19]).ToString();

                string sql_date = " select case when to_date('" + tgl + "','yyyy-mm-dd') < to_date('" + period + "', 'yyyymm') then '1' else '0' end aa from dual ";
                OleDbConnection sqlConnectz = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSqlz = new OleDbDataAdapter(sql_date, sqlConnectz);
                DataTable dtz = new DataTable();
                adSqlz.Fill(dtz);
                cek_dt = dtz.Rows[0]["aa"].ToString();


                if (action == "I")
                {

                    if (tgl == "")
                    {
                        MessageBox.Show("Tanggal Harus diisi.");
                    }
                    else if (que == "")
                    {
                        MessageBox.Show("Antrian Harus diisi.");
                    }
                    else if (confirm == "Y")
                    {
                        MessageBox.Show("Data tidak bisa dirubah.");
                    }
                    else if (cek_dt == "1")
                    {
                        MessageBox.Show("Data Stok tidak bisa dirubah.");
                    }
                    else if (stok == "0")
                    {
                        MessageBox.Show("Stok obat tidak tersedia.");
                    }
                    else if (s_jml == "")
                    {
                        MessageBox.Show("Jumlah harus diisi.");
                    }
                    else if (Convert.ToInt16(jumlah) > Convert.ToInt16(stok))
                    {
                        MessageBox.Show("Jumlah melebihi stok");
                    }
                    else if (kode_obat == "")
                    {
                        MessageBox.Show("Kode obat harus diisi.");
                    }
                    else if (s_formula == "")
                    {
                        MessageBox.Show("Kode Dosis harus diisi.");
                    }
                    else if (info == "")
                    {
                        MessageBox.Show("Info harus diisi.");
                    }
                    else
                    {
                        sql_cnt2 = " ";

                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "select count(0) cnt from cs_visit ";
                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "where 1=1 ";
                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "and patient_no='" + nik + "' ";
                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "and to_char(visit_date,'yyyy-mm-dd') = '" + tgl + "' ";
                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "and que01='" + que + "' ";
                        OleDbConnection oraConnect2a = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra2a = new OleDbDataAdapter(sql_cnt2, oraConnect2a);
                        DataTable dt2a = new DataTable();
                        adOra2a.Fill(dt2a);
                        visit_cnt = dt2a.Rows[0]["cnt"].ToString();

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
                            
                            sql_insert = sql_insert + Environment.NewLine + "insert into cs_receipt ";
                            sql_insert = sql_insert + Environment.NewLine + "(receipt_id, rm_no, insp_date, visit_no, med_cd, formula, med_qty, type_drink, confirm, price, qty_day, days, dosis, ins_date, ins_emp) ";
                            sql_insert = sql_insert + Environment.NewLine + "values  ";
                            sql_insert = sql_insert + Environment.NewLine + "(cs_receipt_seq.nextval,'" + rm_no + "',to_date('" + tgl + "','yyyy-mm-dd'),'" + que + "','" + kode_obat + "','" + dosis + "', '" + jumlah + "', '" + info + "', 'N', '" + s_harga + "', '" + s_hari + "', '" + s_jml + "', '" + s_formula + "', ";
                            sql_insert = sql_insert + Environment.NewLine + " sysdate, '" + v_empid + "') ";

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

                    if (confirm == "Y")
                    {
                        MessageBox.Show("Data tidak bisa dirubah.");
                    }
                    else if (cek_dt == "1")
                    {
                        MessageBox.Show("Data Stok tidak bisa dirubah.");
                    }
                    else if (stok == "0")
                    {
                        MessageBox.Show("Stok obat tidak tersedia.");
                    }
                    else if (s_jml == "")
                    {
                        MessageBox.Show("Jumlah harus diisi.");
                    }
                    else if (Convert.ToInt16(jumlah) > Convert.ToInt16(stok))
                    {
                        MessageBox.Show("Jumlah melebihi stok");
                    }
                    else if (kode_obat == "")
                    {
                        MessageBox.Show("Kode obat harus diisi.");
                    }
                    else if (s_formula == "")
                    {
                        MessageBox.Show("Kode Dosis harus diisi.");
                    }
                    else if (info == "")
                    {
                        MessageBox.Show("Info harus diisi.");
                    }
                    else
                    {
                        sql_update = "";

                        sql_update = sql_update + Environment.NewLine + "update cs_receipt ";
                        sql_update = sql_update + Environment.NewLine + "set med_cd = '" + kode_obat + "', formula = '" + dosis + "', med_qty = '" + jumlah + "', type_drink = '" + info + "',  ";
                        sql_update = sql_update + Environment.NewLine + "price = '" + s_harga + "', qty_day = '" + s_hari + "', days = '" + s_jml + "', dosis = '" + s_formula + "',  ";
                        sql_update = sql_update + Environment.NewLine + "upd_date = sysdate, upd_emp = '" + v_empid + "' ";
                        sql_update = sql_update + Environment.NewLine + "where receipt_id = '" + id + "' and confirm='N' ";


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
            }
            richTextBox1.Text = cek;
            //MessageBox.Show(action);
        }
    }
}
