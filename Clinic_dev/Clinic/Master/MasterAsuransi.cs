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
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;

namespace Clinic
{
    public partial class MasterAsuransi : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<FlagYn> userStatus = new List<FlagYn>();
        List<Medicine> listRole = new List<Medicine>();
        DataTable dtGlRole = new DataTable();

        public string   v_name = "";
        string kate_cd = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";

        public MasterAsuransi()
        {
            InitializeComponent();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void MasterFormula_Load(object sender, EventArgs e)
        {
            initData();
            loadData();
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "MasterAsuransi");
        }

        private void initData()
        {
            dtGlRole.Clear();

            userStatus.Clear();
            userStatus.Add(new FlagYn() { flagCode = "", flagName = "" });
            userStatus.Add(new FlagYn() { flagCode = "A", flagName = "Aktif" });
            userStatus.Add(new FlagYn() { flagCode = "I", flagName = "Tidak Aktif" });
        }

        private void btnLoadDosis_Click(object sender, EventArgs e)
        {
            initData();
            loadData();
        }

        private void loadData()
        {
            string sql_search, stat = "";

            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select 'S' action, ID_PT, NAMA_PT, LIMIT_RJ, LIMIT_RI, INS_DATE, INS_EMP, status";
            sql_search = sql_search + Environment.NewLine + "from CS_ASURANSI_PT ";
            sql_search = sql_search + Environment.NewLine + "where status='A' ";
            sql_search = sql_search + Environment.NewLine + "order by 2,1 ";

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

                //gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 40;
                gridView1.OptionsBehavior.Editable = true; 

                gridView1.Columns[0].Caption = "Action";
                gridView1.Columns[1].Caption = "ID PT";
                gridView1.Columns[2].Caption = "Perusahaan";
                gridView1.Columns[3].Caption = "Limit Rawat Jalan";
                gridView1.Columns[4].Caption = "Limit Rawat Inap";
                gridView1.Columns[5].Caption = "Tanggal Register";
                gridView1.Columns[6].Caption = "ID Register";
                gridView1.Columns[7].Caption = "Status";

                //RepositoryItemGridLookUpEdit glRole = new RepositoryItemGridLookUpEdit();
                //glRole.DataSource = listRole;
                //glRole.ValueMember = "medicineCode";
                //glRole.DisplayMember = "medicineName";

                //glRole.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //glRole.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                //glRole.ImmediatePopup = true;
                //glRole.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                //glRole.NullText = "";
                //gridView1.Columns[2].ColumnEdit = glRole;

                //RepositoryItemLookUpEdit dLookup = new RepositoryItemLookUpEdit();
                //dLookup.DataSource = userStatus;
                //dLookup.ValueMember = "flagCode";
                //dLookup.DisplayMember = "flagName";

                //dLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //dLookup.DropDownRows = userStatus.Count;
                //dLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                //dLookup.AutoSearchColumnIndex = 1;
                //dLookup.NullText = "";
                //gridView1.Columns[7].ColumnEdit = dLookup;

                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].OptionsColumn.ReadOnly = true;
                gridView1.Columns[5].OptionsColumn.ReadOnly = false;
                gridView1.Columns[6].OptionsColumn.ReadOnly = false; 
                gridView1.Columns[7].OptionsColumn.ReadOnly = true;
                gridView1.Columns[3].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns[3].DisplayFormat.FormatString = "#,#";
                gridView1.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns[4].DisplayFormat.FormatString = "#,#";


                gridView1.BestFitColumns();
                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void btnAddDosis_Click(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView1.Columns[1].OptionsColumn.ReadOnly = true;
            gridView1.Columns[5].OptionsColumn.ReadOnly = false;
            gridView1.Columns[6].OptionsColumn.ReadOnly = false;
            gridView1.AddNewRow();
        }
        
        private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[7], "A");
        }

        private void btnSaveDosis_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_update = "", sql_cnt = "", p_lmrj = "", p_nama = "";
            string p_lmri = "", p_pass = "", p_id = "", p_status = "", p_action = "";
            
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                p_action = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                p_id = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                p_nama = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                p_lmrj = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                p_lmri = gridView1.GetRowCellValue(i, gridView1.Columns[4]).ToString();          


                if (p_nama == "")
                {
                    MessageBox.Show("Nama harus diisi");
                } 
                else
                {
                    if (p_action == "I")
                    {
                        sql_insert = "";  
                        sql_insert = sql_insert + " insert into CS_ASURANSI_PT (ID_PT, NAMA_PT, LIMIT_RJ, LIMIT_RI, STATUS, INS_DATE, INS_EMP) values ";
                        sql_insert = sql_insert + " (KLINIK.CS_ASURANSI_PT_SEQ.nextval , '" + p_nama + "', nvl('" + p_lmrj + "',0),  nvl('" + p_lmri + "',0), 'A', sysdate, '" + DB.vUserId + "') ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);
                            //LoadDataKate();
                            MessageBox.Show("Data Berhasil ditambah");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                    else if (p_action == "U")
                    {
                        sql_update = "";

                        sql_update = sql_update + " update CS_ASURANSI_PT set  LIMIT_RJ = '" + p_pass + "', LIMIT_RI = '" + p_nama + "'  ";
                        sql_update = sql_update + " UPD_DATE = sysdate, UPD_EMP = '" + DB.vUserId + "' ";
                        sql_update = sql_update + " where ID_PT = '" + p_id + "' ";

                        try
                        {
                            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm2 = new OleDbCommand(sql_update, oraConnect2);
                            oraConnect2.Open();
                            cm2.ExecuteNonQuery();
                            oraConnect2.Close();
                            cm2.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);
                            //LoadDataKate();
                            MessageBox.Show("Data Berhasil dirubah");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
            }
            loadData();
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveUser.Enabled = true;
            GridView view = sender as GridView;
 
            if (e.Column.Caption == "Perusahaan" || e.Column.Caption == "Limit Rawat Jalan" || e.Column.Caption == "Limit Rawat Inap"  )
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[0], "U");
                }
            }
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Perusahaan" || e.Column.Caption == "Limit Rawat Jalan" || e.Column.Caption == "Limit Rawat Inap"  || e.Column.Caption == "Status")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnDelDosis_Click(object sender, EventArgs e)
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

                id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();

                sql_delete = "";

                sql_delete = sql_delete + " update CS_ASURANSI_PT set status = 'I' ";
                sql_delete = sql_delete + " where user_id = '" + id + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);
                    gridView1.DeleteRow(gridView1.FocusedRowHandle);
                    MessageBox.Show("Data Berhasil dihapus");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "user.xls",
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