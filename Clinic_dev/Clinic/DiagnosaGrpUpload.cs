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
using DevExpress.DataAccess.Excel;
using System.Data.OleDb;
using DevExpress.XtraGrid.Views.Grid;

namespace Clinic
{
    public partial class DiagnosaGrpUpload : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string[] lines = null;
        string[] col = null;
        string[] values = null;
        DataTable my_dataTable = new DataTable();
        DataTable template_upload = new DataTable();

        public DiagnosaGrpUpload()
        {
            InitializeComponent();
        }

        private void MedicineUpload_Load(object sender, EventArgs e)
        {
            my_dataTable.Columns.Add("A", typeof(string));
            my_dataTable.Columns.Add("B", typeof(string));
            my_dataTable.Columns.Add("C", typeof(string));
            my_dataTable.Columns.Add("D", typeof(string));
            my_dataTable.Columns.Add("E", typeof(string));
            my_dataTable.Columns.Add("Status", typeof(string));
            my_dataTable.Columns.Add("Info", typeof(string));

            gridControl1.DataSource = my_dataTable;
            getUploadName();

            template_upload.Columns.Add("Kode Category", typeof(string));
            template_upload.Columns.Add("Nama Category", typeof(string));

            template_upload.Rows.Add(new Object[]{ "CAT001", "NAMA"});
            template_upload.Rows.Add(new Object[]{ "CAT999", "NAMA"});
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Delimited Tab|*.txt" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    lines = System.IO.File.ReadAllLines(ofd.FileName);
                    values = null;
                    tUpLoc.Text = ofd.FileName;

                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            string sql_status = "", stat="", s_name="";
            s_name = tUpName.Text;

            sql_status = " select count(0) cnt from KLINIK.cs_upload where b_v = 'CS_DIAGNOSA_CATEGORY' and c_v='" + s_name + "' ";
            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_status, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);
            stat = dt.Rows[0]["cnt"].ToString();

            if (tUpLoc.Text == "")
            {
                MessageBox.Show("Silahkan pilih file yang akan diupload.");
            }
            else if (s_name == "")
            {
                MessageBox.Show("Upload Name harus diisi.");
            }
            else if (Convert.ToInt16(stat) > 0)
            {
                MessageBox.Show("Upload Name sudah tersedia, silahkan ganti Upload Name atau delete data sebelumnya.");
            }
            else
            {
                my_dataTable.Columns.Clear();
                my_dataTable.Clear();
                for (int i = 0; i <= lines.Length - 1; i++)
                {
                    values = lines[i].ToString().Split('\t');
                    if (i == 0)
                    {
                        string[] col = new string[values.Length];
                        for (int c = 0; c <= values.Length - 1; c++)
                        {
                            my_dataTable.Columns.Add(values[c]);
                        }
                        my_dataTable.Columns.Add("Status", typeof(string));
                        my_dataTable.Columns.Add("Info", typeof(string));
                    }
                    else
                    {
                        string[] row = new string[values.Length];
                        for (int j = 0; j <= values.Length - 1; j++)
                        {
                            values[j] = values[j].Replace("\"", "");
                            values[j] = values[j].Replace("\'", "");
                            values[j] = values[j].Trim();
                            row[j] = values[j];
                        }
                        my_dataTable.Rows.Add(row);
                    }

                }
                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = my_dataTable;
                gridView1.OptionsBehavior.Editable = false;

                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 50;

                OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                OleDbCommand command = new OleDbCommand();
                OleDbTransaction trans = null;

                string tmp_cat_cd = "", tmp_cat_name = "",  tmp_stat="", tmp_info="";
                int rownum = 1;

                command.Connection = oraConnectTrans;
                oraConnectTrans.Open();

                loading.ShowWaitForm();
                try
                {
                    trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                    command.Connection = oraConnectTrans;
                    command.Transaction = trans;

                    for (int a = 0; a < gridView1.RowCount ; a++)
                    {
                        tmp_stat = ""; tmp_info = "";
                        tmp_cat_cd = gridView1.GetRowCellValue(a, gridView1.Columns[0]).ToString();
                        tmp_cat_name = gridView1.GetRowCellValue(a, gridView1.Columns[1]).ToString();
                        if (tmp_cat_cd == "")
                        {
                            tmp_stat = "Error";
                            tmp_info = "Kode Category harus diisi.";
                            gridView1.SetRowCellValue(a, gridView1.Columns[2], tmp_stat);
                            gridView1.SetRowCellValue(a, gridView1.Columns[3], tmp_info);
                        }

                        if (tmp_cat_name == "")
                        {
                            tmp_stat = "Error";
                            tmp_info = "Nama Category harus diisi.";
                            gridView1.SetRowCellValue(a, gridView1.Columns[2], tmp_stat);
                            gridView1.SetRowCellValue(a, gridView1.Columns[3], tmp_info);
                        }

                        command.CommandText = " insert into KLINIK.cs_upload (a_v, b_v, c_v, d_v, e_v, f_v, g_v, h_v, i_v, j_v) " +
                                              " values ("+ rownum + ",'CS_DIAGNOSA_CATEGORY','" + s_name + "','" + tUpLoc.Text + "','" + v_empid + "',to_char(sysdate,'yyyy-mm-dd hh24:mi:ss'),'" + tmp_stat + "','" + tmp_info + "','" + tmp_cat_cd + "','" + tmp_cat_name + "') ";
                        command.ExecuteNonQuery();

                        rownum++;
                    }

                    trans.Commit();
                    //MessageBox.Show(sql_insert);
                    //MessageBox.Show("Query Exec : " + sql_insert);
                    //MessageBox.Show("Data Berhasil disimpan.");
                    int rowHandle = gridView1.LocateByValue("Status", "Error", OnRowSearchComplete);
                    if (rowHandle < 0)
                    {
                        btnValid.Enabled = true; btnSave.Enabled = false;
                    }
                    else
                    {
                        btnValid.Enabled = false; btnSave.Enabled = false;
                    }

                    if (rowHandle != DevExpress.Data.DataController.OperationInProgress)
                        FocusRow(gridView1, rowHandle);

                    getUploadName();
                    
                    cmbSearchNm.Text = tUpName.Text;
                    loadData();

                    tUpLoc.Text = "";
                    tUpName.Text = "";
                    
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    loading.CloseWaitForm();
                    MessageBox.Show("ERROR: " + ex.Message);
                }

                oraConnectTrans.Close();
                loading.CloseWaitForm();

            }
        }

        void OnRowSearchComplete(object rh)
        {
            int rowHandle = (int)rh;
            if (gridView1.IsValidRowHandle(rowHandle))
                FocusRow(gridView1, rowHandle);
        }

        public void FocusRow(GridView view, int rowHandle)
        {
            view.FocusedRowHandle = rowHandle;
        }

        private void getUploadName()
        {
            string sql_upName = " select c_v from ( select c_v, max(a_v)a_v from KLINIK.cs_upload where b_v = 'CS_DIAGNOSA_CATEGORY' group by c_v) order by a_v desc ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_upName, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            cmbSearchNm.Items.Clear();
            cmbSearchNm.Items.Add("");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbSearchNm.Items.Add(dt.Rows[i]["c_v"].ToString());
            }
            cmbSearchNm.SelectedIndex = 0;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
        {
            string sql_load = " select i_v, j_v, g_v, h_v, a_v, c_v from KLINIK.cs_upload where b_v = 'CS_DIAGNOSA_CATEGORY' and c_v = '" + cmbSearchNm.Text + "' order by g_v asc, to_number(a_v) asc ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_load, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            gridControl1.DataSource = dt;
            gridView1.OptionsBehavior.Editable = false;

            gridView1.OptionsView.ColumnAutoWidth = true;
            gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView1.IndicatorWidth = 50;

            gridView1.Columns[0].Caption = "Kode Category";
            gridView1.Columns[1].Caption = "Nama Category";
            gridView1.Columns[2].Caption = "Status";
            gridView1.Columns[3].Caption = "Info";
            gridView1.Columns[4].Caption = "ID";
            gridView1.Columns[5].Caption = "Upload Name";

            gridView1.Columns[4].Visible = false;
            gridView1.Columns[5].Visible = false;

            int rowHandle = gridView1.LocateByValue("G_V", "Error", OnRowSearchComplete);
            if (rowHandle < 0)
            {
                int rowHandle2 = gridView1.LocateByValue("G_V", "Completed", OnRowSearchComplete);
                if (rowHandle2 >= 0)
                {
                    btnValid.Enabled = false; btnSave.Enabled = false;
                }
                else
                {
                    btnValid.Enabled = true; btnSave.Enabled = false;
                }
            }
            else
            {
                btnValid.Enabled = false; btnSave.Enabled = false;
            }

            if (rowHandle != DevExpress.Data.DataController.OperationInProgress)
                FocusRow(gridView1, rowHandle);
        }

        private void cmbSearchNm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSearchNm.Text != "")
            {
                btnDel.Enabled = true;
            }
            else
            {
                btnDel.Enabled = false;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                     "Message",
                      MessageBoxButtons.YesNo,
                      MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string sql_delete = "", id = "";

                sql_delete = "";

                sql_delete = sql_delete + " delete KLINIK.cs_upload ";
                sql_delete = sql_delete + " where b_v = 'CS_DIAGNOSA_CATEGORY' and c_v = '" + cmbSearchNm.Text + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);

                    MessageBox.Show("Data Berhasil didelete");
                    getUploadName();
                    gridControl1.DataSource = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void btnValid_Click(object sender, EventArgs e)
        {
            string tmp_cat_cd = "", tmp_cat_id = "", tmp_upName = "", tmp_stat = "", tmp_info = "", stat="";

            try
            {
                loading.ShowWaitForm();
                for (int a = 0; a < gridView1.RowCount; a++)
                {
                    tmp_stat = ""; tmp_info = "";
                    tmp_cat_cd = gridView1.GetRowCellValue(a, gridView1.Columns[0]).ToString();
                    tmp_upName = gridView1.GetRowCellValue(a, gridView1.Columns[5]).ToString();
                    tmp_cat_id = gridView1.GetRowCellValue(a, gridView1.Columns[4]).ToString();

                    string sql_load = " select count(0) cnt from KLINIK.CS_DIAGNOSA_CATEGORY where status = 'A' and cat_id = '" + tmp_cat_cd + "' ";
                    OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSql = new OleDbDataAdapter(sql_load, sqlConnect);
                    DataTable dt = new DataTable();
                    adSql.Fill(dt);
                    stat = dt.Rows[0]["cnt"].ToString();

                    string sql_upd = "", sql_upd1 = "", sql_upd2 = "";

                    if (Convert.ToInt16(stat) > 0)
                    {
                        sql_upd = "";

                        sql_upd = sql_upd + " update KLINIK.cs_upload set g_v = 'Error', h_v = 'Kode Category sudah terdaftar.' ";
                        sql_upd = sql_upd + " where b_v = 'CS_DIAGNOSA_CATEGORY' and c_v = '" + tmp_upName + "' and a_v = '" + tmp_cat_id + "' ";

                        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm2 = new OleDbCommand(sql_upd, oraConnect2);
                        oraConnect2.Open();
                        cm2.ExecuteNonQuery();
                        oraConnect2.Close();
                        cm2.Dispose();
                    }
                    else
                    {
                        sql_upd1 = "";

                        sql_upd1 = sql_upd1 + " update KLINIK.cs_upload set g_v = 'Valid' ";
                        sql_upd1 = sql_upd1 + " where b_v = 'CS_DIAGNOSA_CATEGORY' and c_v = '" + tmp_upName + "' and a_v = '" + tmp_cat_id + "' ";

                        OleDbConnection oraConnect4 = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm4 = new OleDbCommand(sql_upd1, oraConnect4);
                        oraConnect4.Open();
                        cm4.ExecuteNonQuery();
                        oraConnect4.Close();
                        cm4.Dispose();
                    }

                    sql_upd2 = sql_upd2 + Environment.NewLine + " MERGE INTO KLINIK.cs_upload t ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "   USING (SELECT * ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "            FROM (SELECT t.ROWID AS row_id, t.a_v, t.i_v, ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "                         MIN (a_v) OVER (PARTITION BY j_v ORDER BY a_v)  AS min_line_no ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "                    FROM KLINIK.cs_upload t ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "                   WHERE b_v = 'CS_DIAGNOSA_CATEGORY' AND c_v = '" + tmp_upName + "') ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "           WHERE a_v <> min_line_no) s ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "   ON (t.ROWID = s.row_id) ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "   WHEN MATCHED THEN ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "      UPDATE ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "         SET g_v = 'Error', ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "             h_v = 'Duplikat data di line ' || s.a_v || ' dan '  || s.min_line_no || ' dengan Kode ' || s.i_v ";

                    OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm3 = new OleDbCommand(sql_upd2, oraConnect3);
                    oraConnect3.Open();
                    cm3.ExecuteNonQuery();
                    oraConnect3.Close();
                    cm3.Dispose();

                }


            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }

            loading.CloseWaitForm();
            loadData();
            int rowHandle = gridView1.LocateByValue("G_V", "Error", OnRowSearchComplete);
            if (rowHandle < 0)
            {
                btnValid.Enabled = true; btnSave.Enabled = true;
            }
            else
            {
                btnValid.Enabled = false; btnSave.Enabled = false;
            }

            if (rowHandle != DevExpress.Data.DataController.OperationInProgress)
                FocusRow(gridView1, rowHandle);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string stat = "", sql_insert="";

            string sql_load = " select count(0) cnt from KLINIK.cs_upload where b_v = 'CS_DIAGNOSA_CATEGORY' and c_v = '" + cmbSearchNm.Text + "' and g_v in ('Error','Completed')  ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_load, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            stat = dt.Rows[0]["cnt"].ToString();

            OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
            OleDbCommand command = new OleDbCommand();
            OleDbTransaction trans = null;

            command.Connection = oraConnectTrans;
            oraConnectTrans.Open();
            
            try
            {
                if (Convert.ToInt16(stat) > 0)
                {
                    MessageBox.Show("Data Sudah Terupload");
                }
                else
                {
                    string tmp_cat_cd = "", tmp_cat_name = "";
                    trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                    command.Connection = oraConnectTrans;
                    command.Transaction = trans;
                    loading.ShowWaitForm();

                    for (int a = 0; a < gridView1.RowCount; a++)
                    {
                        tmp_cat_cd = gridView1.GetRowCellValue(a, gridView1.Columns[0]).ToString();
                        tmp_cat_name = gridView1.GetRowCellValue(a, gridView1.Columns[1]).ToString();

                        command.CommandText = " insert into KLINIK.CS_DIAGNOSA_CATEGORY (cat_id, cat_name, status, ins_date, ins_emp) " +
                                              " values ('" + tmp_cat_cd + "','" + tmp_cat_name + "', 'A', sysdate, '" + v_empid + "') ";
                        command.ExecuteNonQuery();

                    }

                    command.CommandText = " update KLINIK.cs_upload set g_v = 'Completed' " +
                                          " where  b_v = 'CS_DIAGNOSA_CATEGORY' and c_v = '" + cmbSearchNm.Text + "' and g_v = 'Valid' ";
                    command.ExecuteNonQuery();

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }


            oraConnectTrans.Close();
            loading.CloseWaitForm();
            loadData();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            gridControl2.DataSource = null;
            gridView2.Columns.Clear();
            gridControl2.DataSource = template_upload;

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "XLS (*.xls)|*.xlsx",
                FileName = "template_upload_diagnosa_category.xls",
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
                gridControl2.ExportToXls(saveDialog.FileName);
            }
        }
    }
}