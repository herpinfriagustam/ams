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
    public partial class McuList : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string type = "";

        public McuList()
        {
            InitializeComponent();
        }

        private void ObservationList_Load(object sender, EventArgs e)
        {
            InitData();
            //LoadData();
        }

        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void InitData()
        {
            string sql_upName = " select distinct periode from cs_mcu order by 1 desc ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_upName, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            cmbPeriode.Items.Clear();
            cmbPeriode.Items.Add("");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbPeriode.Items.Add(dt.Rows[i]["periode"].ToString());
            }
            cmbPeriode.SelectedIndex = 1;

            cmbType.Items.Clear();
            cmbType.Items.Add("");
            cmbType.Items.Add("Employees");
            cmbType.Items.Add("New Employees");
            cmbType.SelectedIndex = 0;
        }

        private void LoadData()
        {
            string sql_select_room = "";

            sql_select_room = " select PERIODE, MCU_NO, a.EMPID, name, to_char(MCU_DATE,'yyyy-mm-dd') MCU_DATE, decode(EMP_STAT,'E','Employees','New Employees') EMP_STAT, PAKET, KESIMP, STATUS,  " +
                              " RIWAYAT, TB, BB, BMI, TENSI, VISUSKN, VISUSKR, BUTAWARNA, KSMFISIK, LABSMUA,  " +
                              " LABHEMA, LABKIMIA, LABURINE, RONTGEN, JANTUNG, AUDIO, SPIRO " +
                              " from CS_MCU a join cs_employees b on a.empid=b.empid " +
                              " where PERIODE = '" + cmbPeriode.Text + "' and EMP_STAT like '" + type + "%' ";

            loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_select_room, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                //gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 60;
                gridView1.OptionsBehavior.Editable = false;
                //gridView1.BestFitColumns();
                gridView1.FixedLineWidth = 5;
                gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView1.Columns[0].Caption = "Periode";
                gridView1.Columns[1].Caption = "MCU No";
                gridView1.Columns[2].Caption = "NIK";
                gridView1.Columns[3].Caption = "Nama";
                gridView1.Columns[4].Caption = "Tanggal MCU";
                gridView1.Columns[5].Caption = "Emp Status";
                gridView1.Columns[6].Caption = "Paket";
                gridView1.Columns[7].Caption = "Kesimpulan";
                gridView1.Columns[8].Caption = "Status";
                gridView1.Columns[9].Caption = "Riwayat";
                gridView1.Columns[10].Caption = "TB";
                gridView1.Columns[11].Caption = "BB";
                gridView1.Columns[12].Caption = "BMI";
                gridView1.Columns[13].Caption = "Tensi";
                gridView1.Columns[14].Caption = "VisusKn";
                gridView1.Columns[15].Caption = "VisusKr";
                gridView1.Columns[16].Caption = "Buta Warna";
                gridView1.Columns[17].Caption = "Ksm Fisik";
                gridView1.Columns[18].Caption = "LabSmua";
                gridView1.Columns[19].Caption = "LabHema";
                gridView1.Columns[20].Caption = "LabKimia";
                gridView1.Columns[21].Caption = "LabUrine";
                gridView1.Columns[22].Caption = "Rontgen";
                gridView1.Columns[23].Caption = "Jantung";
                gridView1.Columns[24].Caption = "Audio";
                gridView1.Columns[25].Caption = "Spiro";

                gridView1.Columns[0].Width = 60;
                gridView1.Columns[1].Width = 50;
                gridView1.Columns[2].Width = 80;
                gridView1.Columns[3].Width = 150;
                gridView1.Columns[4].Width = 80;
                gridView1.Columns[5].Width = 100;
                gridView1.Columns[6].Width = 50;
                gridView1.Columns[7].Width = 250;
                gridView1.Columns[8].Width = 250;
                gridView1.Columns[9].Width = 250;
                gridView1.Columns[10].Width = 50;
                gridView1.Columns[11].Width = 50;
                gridView1.Columns[12].Width = 50;
                gridView1.Columns[13].Width = 50;
                gridView1.Columns[14].Width = 50;
                gridView1.Columns[15].Width = 80;
                gridView1.Columns[16].Width = 80;
                gridView1.Columns[17].Width = 150;
                gridView1.Columns[18].Width = 150;
                gridView1.Columns[19].Width = 150;
                gridView1.Columns[20].Width = 80;
                gridView1.Columns[21].Width = 80;
                gridView1.Columns[22].Width = 150;
                gridView1.Columns[23].Width = 150;
                gridView1.Columns[24].Width = 150;
                gridView1.Columns[25].Width = 150;

                gridView1.Columns[3].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[7].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[8].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[8].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                if (gridView1.RowCount > 0)
                {
                    btnObsCls.Enabled = true;
                }
                else
                {
                    btnObsCls.Enabled = false;
                }
                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();
            
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string stat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);

            //    if (stat == "Over")
            //    {
            //        e.Appearance.BackColor = Color.IndianRed;
            //        e.Appearance.BackColor2 = Color.Firebrick;
            //        e.Appearance.ForeColor = Color.White;
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //        e.HighPriority = true;
            //    }
            //}
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            //GridView View = sender as GridView;
            //string s_status = "", sql_chk = "";

            //s_status = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);

            //if (s_status == "Over")
            //{
            //    btnObsCls.Enabled = true;
            //}
            //else
            //{
            //    btnObsCls.Enabled = false;
            //}
        }

        private void btnObsCls_Click(object sender, EventArgs e)
        {
            string mcu_no = "", nik = "", period = "";

            period = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            mcu_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();

            if (MessageBox.Show("Anda yakin akan menghapus data?",
                     "Message",
                      MessageBoxButtons.YesNo,
                      MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string sql_delete = "";

                sql_delete = "";

                sql_delete = sql_delete + " delete cs_mcu ";
                sql_delete = sql_delete + " where periode = '" + period + "' and mcu_no = '" + mcu_no + "' and empid = '" + nik + "' ";

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
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }

            }
        }

    private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.Text == "")
            {
                type = "";
            }
            else if (cmbType.Text == "Employees")
            {
                type = "E";
            }
            else
            {
                type = "N";
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "mcu_list.xls",
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
    }
}