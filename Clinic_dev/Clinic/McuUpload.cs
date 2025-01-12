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
using System.Globalization;

namespace Clinic
{
    public partial class McuUpload : DevExpress.XtraEditors.XtraForm
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

        public McuUpload()
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

            template_upload.Columns.Add("No MCU", typeof(string));
            template_upload.Columns.Add("NIK", typeof(string));
            template_upload.Columns.Add("Nama", typeof(string));
            template_upload.Columns.Add("Tanggal MCU", typeof(string));
            template_upload.Columns.Add("Type MCU", typeof(string));
            template_upload.Columns.Add("Paket", typeof(string));
            template_upload.Columns.Add("Kesimpulan", typeof(string));
            template_upload.Columns.Add("Emp Status", typeof(string));
            template_upload.Columns.Add("Riwayat", typeof(string));
            template_upload.Columns.Add("TB", typeof(string));
            template_upload.Columns.Add("BB", typeof(string));
            template_upload.Columns.Add("BMI", typeof(string));
            template_upload.Columns.Add("Tensi", typeof(string));
            template_upload.Columns.Add("VisusKn", typeof(string));
            template_upload.Columns.Add("VisusKr", typeof(string));
            template_upload.Columns.Add("Buta Warna", typeof(string));
            template_upload.Columns.Add("Ksm Fisik", typeof(string));
            template_upload.Columns.Add("LabSmua", typeof(string));
            template_upload.Columns.Add("LabHema", typeof(string));
            template_upload.Columns.Add("LabKimia", typeof(string));
            template_upload.Columns.Add("LabUrine", typeof(string));
            template_upload.Columns.Add("Rontgen", typeof(string));
            template_upload.Columns.Add("Jantung", typeof(string));
            template_upload.Columns.Add("Audio", typeof(string));
            template_upload.Columns.Add("Spiro", typeof(string));

            template_upload.Rows.Add(new Object[]{ "001", "TT17010001", "Nama", "2019-12-31", "N", "A", "Anemia (HB:11.2 g/dl)", "FIT","Maag","153","45","19.22","120/80","6/15","6/12","Tidak","Gigi berlubang, Myooia","DBN","DBN","DBN","DBN","Thorax Foto: Normal","EKG: Normal","Audiometri: Normal","Spirometri: Obstruksi"});
            template_upload.Rows.Add(new Object[]{ "043", "TT20010001", "Nama", "2020-05-21", "N", "A+", "Anemia (HB:11.2 g/dl)", "FIT", "Maag", "153", "45", "19.22", "120/80", "6/15", "6/12", "Tidak", "Gigi berlubang, Myooia", "DBN", "DBN", "DBN", "DBN", "Thorax Foto: Normal", "EKG: Normal", "Audiometri: Normal", "Spirometri: Obstruksi" });
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

            sql_status = " select count(0) cnt from cs_upload where b_v = 'CS_MCU' and c_v='" + s_name + "' ";
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
                    char cr = (char)10;
                    //lines[i] = lines[i].Replace("\"", "");
                    //lines[i] = lines[i].Replace("\n", "");
                    //lines[i] = lines[i].Replace("\r", "");
                    //lines[i] = lines[i].Replace(cr.ToString(), "");
                    //lines[i] = lines[i].Replace(System.Environment.NewLine, "");
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
                            values[j] = values[j].Replace("\n", "");
                            values[j] = values[j].Replace("\r", "");
                            values[j] = values[j].Replace(System.Environment.NewLine, "");
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

                string tmp_mcu_no = "", tmp_empid = "", tmp_name = "", tmp_mcu_dt = "", tmp_stat="", tmp_info="";
                string tmp_emp_stat = "", tmp_paket = "", tmp_kesim = "", tmp_status = "", tmp_riwayat = "";
                string tmp_tb = "", tmp_bb = "", tmp_bmi = "", tmp_tensi = "";
                string tmp_visuskn = "", tmp_visuskr = "", tmp_buta = "", tmp_ksmfisik = "";
                string tmp_labsmua = "", tmp_labhema = "", tmp_labkimia = "", tmp_laburine = "";
                string tmp_rontgen = "", tmp_jantung = "", tmp_audio = "", tmp_spiro = "";
                DateTime result;

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
                        tmp_mcu_no = gridView1.GetRowCellValue(a, gridView1.Columns[0]).ToString();
                        tmp_empid = gridView1.GetRowCellValue(a, gridView1.Columns[1]).ToString();
                        tmp_name = gridView1.GetRowCellValue(a, gridView1.Columns[2]).ToString();
                        tmp_mcu_dt = gridView1.GetRowCellValue(a, gridView1.Columns[3]).ToString();
                        tmp_emp_stat = gridView1.GetRowCellValue(a, gridView1.Columns[4]).ToString();
                        tmp_paket = gridView1.GetRowCellValue(a, gridView1.Columns[5]).ToString();
                        tmp_kesim = gridView1.GetRowCellValue(a, gridView1.Columns[6]).ToString();
                        tmp_status = gridView1.GetRowCellValue(a, gridView1.Columns[7]).ToString();
                        tmp_riwayat = gridView1.GetRowCellValue(a, gridView1.Columns[8]).ToString();
                        tmp_tb = gridView1.GetRowCellValue(a, gridView1.Columns[9]).ToString();
                        tmp_bb = gridView1.GetRowCellValue(a, gridView1.Columns[10]).ToString();
                        tmp_bmi = gridView1.GetRowCellValue(a, gridView1.Columns[11]).ToString();
                        tmp_tensi = gridView1.GetRowCellValue(a, gridView1.Columns[12]).ToString();
                        tmp_visuskn = gridView1.GetRowCellValue(a, gridView1.Columns[13]).ToString();
                        tmp_visuskr = gridView1.GetRowCellValue(a, gridView1.Columns[14]).ToString();
                        tmp_buta = gridView1.GetRowCellValue(a, gridView1.Columns[15]).ToString();
                        tmp_ksmfisik = gridView1.GetRowCellValue(a, gridView1.Columns[16]).ToString();
                        tmp_labsmua = gridView1.GetRowCellValue(a, gridView1.Columns[17]).ToString();
                        tmp_labhema = gridView1.GetRowCellValue(a, gridView1.Columns[18]).ToString();
                        tmp_labkimia = gridView1.GetRowCellValue(a, gridView1.Columns[19]).ToString();
                        tmp_laburine = gridView1.GetRowCellValue(a, gridView1.Columns[20]).ToString();
                        tmp_rontgen = gridView1.GetRowCellValue(a, gridView1.Columns[21]).ToString();
                        tmp_jantung = gridView1.GetRowCellValue(a, gridView1.Columns[22]).ToString();
                        tmp_audio = gridView1.GetRowCellValue(a, gridView1.Columns[23]).ToString();
                        tmp_spiro = gridView1.GetRowCellValue(a, gridView1.Columns[24]).ToString();

                        if (tmp_mcu_no == "")
                        {
                            tmp_stat = "Error";
                            tmp_info = "No MCU harus diisi";
                            gridView1.SetRowCellValue(a, gridView1.Columns[25], tmp_stat);
                            gridView1.SetRowCellValue(a, gridView1.Columns[26], tmp_info);
                        }

                        if (tmp_empid == "")
                        {
                            tmp_stat = "Error";
                            tmp_info = "NIK harus diisi";
                            gridView1.SetRowCellValue(a, gridView1.Columns[25], tmp_stat);
                            gridView1.SetRowCellValue(a, gridView1.Columns[26], tmp_info);
                        }

                        if (tmp_mcu_dt == "")
                        {
                            tmp_stat = "Error";
                            tmp_info = "Tanggal MCU harus diisi";
                            gridView1.SetRowCellValue(a, gridView1.Columns[25], tmp_stat);
                            gridView1.SetRowCellValue(a, gridView1.Columns[26], tmp_info);
                        }

                        if (!DateTime.TryParseExact(
                             tmp_mcu_dt,
                             "yyyy-MM-dd",
                             CultureInfo.InvariantCulture,
                             DateTimeStyles.AssumeUniversal,
                             out result))
                        {
                            tmp_stat = "Error";
                            tmp_info = "Tanggal MCU harus diisi dengan format tanggal yyyy-mm-dd.";
                            gridView1.SetRowCellValue(a, gridView1.Columns[25], tmp_stat);
                            gridView1.SetRowCellValue(a, gridView1.Columns[26], tmp_info);
                        };

                        command.CommandText = " insert into cs_upload (a_v, b_v, c_v, d_v, e_v, f_v, g_v, h_v, i_v, j_v, k_v,l_v,m_v,n_v,o_v,p_v,q_v,r_v,s_v,t_v,u_v,v_v,w_v,x_v,y_v,z_v,a_w,b_w,c_w,d_w,e_w,f_w,g_w) " +
                                              " values ("+ rownum + ",'CS_MCU','" + s_name + "','" + tUpLoc.Text + "','" + v_empid + 
                                              "',to_char(sysdate,'yyyy-mm-dd hh24:mi:ss'),'" + tmp_stat + "','" + tmp_info + 
                                              "','" + tmp_mcu_no + "','" + tmp_empid + "','" + tmp_name + "', '" + tmp_mcu_dt + 
                                              "','" + tmp_emp_stat + "','" + tmp_paket + "','" + tmp_kesim + "', '" + tmp_status + "', '" + tmp_riwayat +
                                              "','" + tmp_tb + "','" + tmp_bb + "','" + tmp_bmi + "', '" + tmp_tensi +
                                              "','" + tmp_visuskn + "','" + tmp_visuskr + "','" + tmp_buta + "', '" + tmp_ksmfisik +
                                              "','" + tmp_labsmua + "','" + tmp_labhema + "','" + tmp_labkimia + "', '" + tmp_laburine +
                                              "','" + tmp_rontgen + "','" + tmp_jantung + "','" + tmp_audio + "', '" + tmp_spiro +
                                              "') ";
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
            string sql_upName = " select c_v from ( select c_v, max(a_v)a_v from cs_upload where b_v = 'CS_MCU' group by c_v) order by a_v desc ";
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
            string sql_load = "  ";

            sql_load = sql_load + " select i_v, j_v, k_v,l_v,m_v,n_v,o_v,p_v,q_v,r_v,s_v,t_v,u_v,v_v,w_v,x_v,y_v,z_v,  ";
            sql_load = sql_load + " a_w,b_w,c_w,d_w,e_w,f_w,g_w, ";
            sql_load = sql_load + " g_v, h_v, a_v, c_v  ";
            sql_load = sql_load + " from cs_upload where b_v = 'CS_MCU' and c_v = '" + cmbSearchNm.Text + "' order by g_v asc, to_number(a_v) asc ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_load, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            gridControl1.DataSource = dt;
            gridView1.OptionsBehavior.Editable = false;

            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView1.IndicatorWidth = 70;
            gridView1.FixedLineWidth = 3;
            gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView1.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView1.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            gridView1.Columns[0].Caption = "No MCU";
            gridView1.Columns[1].Caption = "NIK";
            gridView1.Columns[2].Caption = "Nama";
            gridView1.Columns[3].Caption = "Tanggal MCU";
            gridView1.Columns[4].Caption = "Emp Status";
            gridView1.Columns[5].Caption = "Paket";
            gridView1.Columns[6].Caption = "Kesimpulan";
            gridView1.Columns[7].Caption = "Status";
            gridView1.Columns[8].Caption = "Riwayat";
            gridView1.Columns[9].Caption = "TB";
            gridView1.Columns[10].Caption = "BB";
            gridView1.Columns[11].Caption = "BMI";
            gridView1.Columns[12].Caption = "Tensi";
            gridView1.Columns[13].Caption = "VisusKn";
            gridView1.Columns[14].Caption = "VisusKr";
            gridView1.Columns[15].Caption = "Buta Warna";
            gridView1.Columns[16].Caption = "Ksm Fisik";
            gridView1.Columns[17].Caption = "LabSmua";
            gridView1.Columns[18].Caption = "LabHema";
            gridView1.Columns[19].Caption = "LabKimia";
            gridView1.Columns[20].Caption = "LabUrine";
            gridView1.Columns[21].Caption = "Rontgen";
            gridView1.Columns[22].Caption = "Jantung";
            gridView1.Columns[23].Caption = "Audio";
            gridView1.Columns[24].Caption = "Spiro";
            gridView1.Columns[25].Caption = "Status";
            gridView1.Columns[26].Caption = "Info";
            gridView1.Columns[27].Caption = "ID";
            gridView1.Columns[28].Caption = "Upload Name";

            gridView1.Columns[27].Visible = false;
            gridView1.Columns[28].Visible = false;

            gridView1.Columns[0].Width = 50;
            gridView1.Columns[1].Width = 80;
            gridView1.Columns[2].Width = 150;
            gridView1.Columns[3].Width = 80;
            gridView1.Columns[4].Width = 80;
            gridView1.Columns[5].Width = 80;
            gridView1.Columns[6].Width = 500;
            gridView1.Columns[7].Width = 50;
            gridView1.Columns[8].Width = 150;
            gridView1.Columns[9].Width = 50;
            gridView1.Columns[10].Width = 50;
            gridView1.Columns[11].Width = 50;
            gridView1.Columns[12].Width = 50;
            gridView1.Columns[13].Width = 50;
            gridView1.Columns[14].Width = 80;
            gridView1.Columns[15].Width = 80;
            gridView1.Columns[16].Width = 150;
            gridView1.Columns[17].Width = 150;
            gridView1.Columns[18].Width = 150;
            gridView1.Columns[19].Width = 80;
            gridView1.Columns[20].Width = 80;
            gridView1.Columns[21].Width = 150;
            gridView1.Columns[22].Width = 150;
            gridView1.Columns[23].Width = 150;
            gridView1.Columns[24].Width = 150;
            gridView1.Columns[25].Width = 80;
            gridView1.Columns[26].Width = 200;

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
                    btnValid.Enabled = true; btnSave.Enabled = true;
                }
                btnDelError.Enabled = false;
            }
            else
            {
                btnValid.Enabled = false; btnSave.Enabled = false; btnDelError.Enabled = true;
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

                sql_delete = sql_delete + " delete cs_upload ";
                sql_delete = sql_delete + " where b_v = 'CS_MCU' and c_v = '" + cmbSearchNm.Text + "' ";

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
            string tmp_nik= "", tmp_mcu_dt = "", tmp_mcu_id = "", tmp_upName = "", tmp_stat = "", tmp_info = "", stat="", stat1 = "";

            try
            {
                loading.ShowWaitForm();
                for (int a = 0; a < gridView1.RowCount; a++)
                {
                    tmp_stat = ""; tmp_info = "";
                    tmp_nik = gridView1.GetRowCellValue(a, gridView1.Columns[1]).ToString();
                    tmp_mcu_dt = gridView1.GetRowCellValue(a, gridView1.Columns[3]).ToString();
                    tmp_upName = gridView1.GetRowCellValue(a, gridView1.Columns[28]).ToString();
                    tmp_mcu_id = gridView1.GetRowCellValue(a, gridView1.Columns[27]).ToString();

                    string sql_load = " select count(0) cnt from CS_MCU where empid = '" + tmp_nik + "' and to_char(mcu_date,'yyyy-mm-dd')='" + tmp_mcu_dt + "' ";
                    OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSql = new OleDbDataAdapter(sql_load, sqlConnect);
                    DataTable dt = new DataTable();
                    adSql.Fill(dt);
                    stat = dt.Rows[0]["cnt"].ToString();

                    string sql_cek = " select count(0) cnt from CS_EMPLOYEES where empid = '" + tmp_nik + "' ";
                    OleDbConnection sqlConnect1 = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSql1 = new OleDbDataAdapter(sql_cek, sqlConnect1);
                    DataTable dt1 = new DataTable();
                    adSql1.Fill(dt1);
                    stat1 = dt1.Rows[0]["cnt"].ToString();

                    string sql_upd = "", sql_upd1 = "", sql_upd2 = "";

                    if (Convert.ToInt16(stat) > 0)
                    {
                        sql_upd = "";

                        sql_upd = sql_upd + " update cs_upload set g_v = 'Error', h_v = 'Data MCU sudah terinput' ";
                        sql_upd = sql_upd + " where b_v = 'CS_MCU' and c_v = '" + tmp_upName + "' and a_v = '" + tmp_mcu_id + "' ";

                        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm2 = new OleDbCommand(sql_upd, oraConnect2);
                        oraConnect2.Open();
                        cm2.ExecuteNonQuery();
                        oraConnect2.Close();
                        cm2.Dispose();
                    }
                    else if (Convert.ToInt16(stat1) == 0)
                    {
                        sql_upd = "";

                        sql_upd = sql_upd + " update cs_upload set g_v = 'Error', h_v = 'Data Karyawan Tidak ditemukan' ";
                        sql_upd = sql_upd + " where b_v = 'CS_MCU' and c_v = '" + tmp_upName + "' and a_v = '" + tmp_mcu_id + "' ";

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

                        sql_upd1 = sql_upd1 + " update cs_upload set g_v = 'Valid' ";
                        sql_upd1 = sql_upd1 + " where b_v = 'CS_MCU' and c_v = '" + tmp_upName + "' and a_v = '" + tmp_mcu_id + "' ";

                        OleDbConnection oraConnect4 = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm4 = new OleDbCommand(sql_upd1, oraConnect4);
                        oraConnect4.Open();
                        cm4.ExecuteNonQuery();
                        oraConnect4.Close();
                        cm4.Dispose();
                    }

                    sql_upd2 = sql_upd2 + Environment.NewLine + " MERGE INTO cs_upload t ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "   USING (SELECT * ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "            FROM (SELECT t.ROWID AS row_id, t.a_v, t.i_v, ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "                         MIN (a_v) OVER (PARTITION BY j_v, k_v, l_v ORDER BY a_v)  AS min_line_no ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "                    FROM cs_upload t ";
                    sql_upd2 = sql_upd2 + Environment.NewLine + "                   WHERE b_v = 'CS_MCU' AND c_v = '" + tmp_upName + "') ";
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

            string sql_load = " select count(0) cnt from cs_upload where b_v = 'CS_MCU' and c_v = '" + cmbSearchNm.Text + "' and g_v in ('Error','Completed')  ";
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
                    string tmp_mcu_no = "", tmp_nik = "", tmp_periode = "", tmp_mcu_date = "", tmp_emp_stat = "";
                    string tmp_paket = "", tmp_kesimp = "", tmp_status = "", tmp_riwayat= "";
                    string tmp_tb = "", tmp_bb = "", tmp_bmi = "", tmp_tensi = "";
                    string tmp_visuskn= "", tmp_visuskr = "", tmp_buta = "", tmp_ksm_fisik = "";
                    string tmp_labsmua = "", tmp_labhema = "", tmp_labkimia = "", tmp_laburine = "";
                    string tmp_rontgen = "", tmp_jantung = "", tmp_audio = "", tmp_spiro = "";

                    trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                    command.Connection = oraConnectTrans;
                    command.Transaction = trans;
                    loading.ShowWaitForm();

                    for (int a = 0; a < gridView1.RowCount; a++)
                    {
                        tmp_mcu_no = gridView1.GetRowCellValue(a, gridView1.Columns[0]).ToString();
                        tmp_nik = gridView1.GetRowCellValue(a, gridView1.Columns[1]).ToString();
                        tmp_mcu_date = gridView1.GetRowCellValue(a, gridView1.Columns[3]).ToString();
                        tmp_periode = tmp_mcu_date.Substring(0, 4);
                        tmp_emp_stat = gridView1.GetRowCellValue(a, gridView1.Columns[4]).ToString();
                        tmp_paket = gridView1.GetRowCellValue(a, gridView1.Columns[5]).ToString();
                        tmp_kesimp = gridView1.GetRowCellValue(a, gridView1.Columns[6]).ToString();
                        tmp_status = gridView1.GetRowCellValue(a, gridView1.Columns[7]).ToString();
                        tmp_riwayat = gridView1.GetRowCellValue(a, gridView1.Columns[8]).ToString();
                        tmp_tb = gridView1.GetRowCellValue(a, gridView1.Columns[9]).ToString();
                        tmp_bb = gridView1.GetRowCellValue(a, gridView1.Columns[10]).ToString();
                        tmp_bmi = gridView1.GetRowCellValue(a, gridView1.Columns[11]).ToString();
                        tmp_tensi = gridView1.GetRowCellValue(a, gridView1.Columns[12]).ToString();
                        tmp_visuskn = gridView1.GetRowCellValue(a, gridView1.Columns[13]).ToString();
                        tmp_visuskr = gridView1.GetRowCellValue(a, gridView1.Columns[14]).ToString();
                        tmp_buta = gridView1.GetRowCellValue(a, gridView1.Columns[15]).ToString();
                        tmp_ksm_fisik = gridView1.GetRowCellValue(a, gridView1.Columns[16]).ToString();
                        tmp_labsmua = gridView1.GetRowCellValue(a, gridView1.Columns[17]).ToString();
                        tmp_labhema = gridView1.GetRowCellValue(a, gridView1.Columns[18]).ToString();
                        tmp_labkimia = gridView1.GetRowCellValue(a, gridView1.Columns[19]).ToString();
                        tmp_laburine = gridView1.GetRowCellValue(a, gridView1.Columns[20]).ToString();
                        tmp_rontgen = gridView1.GetRowCellValue(a, gridView1.Columns[21]).ToString();
                        tmp_jantung = gridView1.GetRowCellValue(a, gridView1.Columns[22]).ToString();
                        tmp_audio = gridView1.GetRowCellValue(a, gridView1.Columns[23]).ToString();
                        tmp_spiro = gridView1.GetRowCellValue(a, gridView1.Columns[24]).ToString();


                        command.CommandText = " insert into CS_MCU (mcu_no, empid, periode, mcu_date, emp_stat, paket, kesimp, status, riwayat, tb, bb, bmi, tensi, visuskn, visuskr, butawarna, ksmfisik, labsmua, labhema, labkimia, laburine, rontgen, jantung, audio, spiro, ins_date, ins_emp) " +
                                              " values ('" + tmp_mcu_no + "','" + tmp_nik + "','" + tmp_periode + "',to_date('" + tmp_mcu_date + "','yyyy-mm-dd'),'E','" + tmp_paket + "','" + tmp_kesimp + "','" + tmp_status + "','" + tmp_riwayat + "','" + tmp_tb + "','" + tmp_bb + "','" + tmp_bmi + "','" + tmp_tensi + "','" + tmp_visuskn + "','" + tmp_visuskr + "','" + tmp_buta + "','" + tmp_ksm_fisik + "','" + tmp_labsmua + "','" + tmp_labhema + "','" + tmp_labkimia + "','" + tmp_laburine + "','" + tmp_rontgen + "','" + tmp_jantung + "','" + tmp_audio + "','" + tmp_spiro + "',sysdate,'" + v_empid + "') ";
                        command.ExecuteNonQuery();

                    }

                    command.CommandText = " update cs_upload set g_v = 'Completed' " +
                                          " where  b_v = 'CS_MCU' and c_v = '" + cmbSearchNm.Text + "' and g_v = 'Valid' ";
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
                FileName = "template_upload_mcu.xls",
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "XLS (*.xls)|*.xlsx",
                FileName = "result_upload_mcu.xls",
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

        private void btnDelError_Click(object sender, EventArgs e)
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

                sql_delete = sql_delete + " delete cs_upload ";
                sql_delete = sql_delete + " where g_v = 'Error' and b_v = 'CS_MCU' and c_v = '" + cmbSearchNm.Text + "' ";

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
                    btnDelError.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
                
            }
        }
    }
}