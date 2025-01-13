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
    public partial class MasterPoli : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<FlagYn> diagnosaStatus = new List<FlagYn>();
        List<Status> rmStatus = new List<Status>();
        List<Purpose> purpose = new List<Purpose>();
        List<Poli> poli = new List<Poli>();

        public string v_name = "";
        string kate_cd = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";

        public MasterPoli()
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

        private void gridView2_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void MasterPoli_Load(object sender, EventArgs e)
        {
            initData();
            loadData();
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "MasterPoli");
        }

        private void initData()
        {
            rmStatus.Clear();
            rmStatus.Add(new Status() { statusCode = "COMM", statusName = "Umum" });
            rmStatus.Add(new Status() { statusCode = "PREG", statusName = "Ibu Hamil" });
            rmStatus.Add(new Status() { statusCode = "FAMP", statusName = "KB" });

            purpose.Clear();
            purpose.Add(new Purpose() { purposeCode = "DOC", purposeName = "Dokter" });
            purpose.Add(new Purpose() { purposeCode = "MID", purposeName = "Obgyn" });

            diagnosaStatus.Clear();
            diagnosaStatus.Add(new FlagYn() { flagCode = "", flagName = "" });
            diagnosaStatus.Add(new FlagYn() { flagCode = "A", flagName = "Aktif" });
            diagnosaStatus.Add(new FlagYn() { flagCode = "I", flagName = "Tidak Aktif" });

            string sql_poli = " select poli_cd, poli_name from cs_policlinic where visible = 'Y' order by poli_cd ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_poli, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            poli.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                poli.Add(new Poli() { poliCode = dt.Rows[i]["poli_cd"].ToString(), poliName = dt.Rows[i]["poli_name"].ToString() });

            }
        }

        private void btnLoadPoli_Click(object sender, EventArgs e)
        {
            initData();
            loadData();
        }

        private void loadData()
        {
            string sql_search, stat = "";
            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select 'S' action, poli_cd, poli_name, poli_group, poli_pic, status ";
            sql_search = sql_search + Environment.NewLine + "from cs_policlinic ";
            sql_search = sql_search + Environment.NewLine + "where visible='Y' ";
            sql_search = sql_search + Environment.NewLine + "order by poli_cd ";

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
                

                //gridView1.OptionsSelection.MultiSelect = true;
                //gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView1.VisibleColumns[0].Width = 20;
                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[4].Visible = false;

                gridView1.Columns[0].Caption = "Action";
                gridView1.Columns[1].Caption = "Kode Poli";
                gridView1.Columns[2].Caption = "Nama Poli";
                gridView1.Columns[3].Caption = "Grup Poli";
                gridView1.Columns[4].Caption = "PIC Poli";
                gridView1.Columns[5].Caption = "Status";

                RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
                statusLookup.DataSource = rmStatus;
                statusLookup.ValueMember = "statusCode";
                statusLookup.DisplayMember = "statusName";

                statusLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup.DropDownRows = rmStatus.Count;
                statusLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup.AutoSearchColumnIndex = 1;
                statusLookup.NullText = "";
                gridView1.Columns[3].ColumnEdit = statusLookup;

                RepositoryItemLookUpEdit pLookup = new RepositoryItemLookUpEdit();
                pLookup.DataSource = purpose;
                pLookup.ValueMember = "purposeCode";
                pLookup.DisplayMember = "purposeName";

                pLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                pLookup.DropDownRows = purpose.Count;
                pLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                pLookup.AutoSearchColumnIndex = 1;
                pLookup.NullText = "";
                gridView1.Columns[4].ColumnEdit = pLookup;

                RepositoryItemLookUpEdit dLookup = new RepositoryItemLookUpEdit();
                dLookup.DataSource = diagnosaStatus;
                dLookup.ValueMember = "flagCode";
                dLookup.DisplayMember = "flagName";

                dLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                dLookup.DropDownRows = diagnosaStatus.Count;
                dLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                dLookup.AutoSearchColumnIndex = 1;
                dLookup.NullText = "";
                gridView1.Columns[5].ColumnEdit = dLookup;

                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].OptionsColumn.AllowEdit = false;
                //gridView1.Columns[1].OptionsColumn.ReadOnly = true;
                gridView1.Columns[5].OptionsColumn.ReadOnly = true;

                gridView1.BestFitColumns();
                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            GridView View = sender as GridView;

            string poli_cd = "";

            poli_cd = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);

            string sql_search, stat = "";
            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select 'S' action, poli_cd, info_cd, description, status ";
            sql_search = sql_search + Environment.NewLine + "from cs_add_info ";
            sql_search = sql_search + Environment.NewLine + "where visible='Y' ";
            sql_search = sql_search + Environment.NewLine + "and poli_cd = '" + poli_cd + "' ";
            sql_search = sql_search + Environment.NewLine + "order by info_cd ";

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

                //gridView2.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView2.OptionsView.ColumnAutoWidth = true;
                gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView2.IndicatorWidth = 30;
                gridView2.OptionsBehavior.Editable = false;


                //gridView2.OptionsSelection.MultiSelect = true;
                //gridView2.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView2.VisibleColumns[0].Width = 20;
                //gridView2.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView2.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView2.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView2.Columns[4].Visible = false;

                gridView2.Columns[0].Caption = "Action";
                gridView2.Columns[1].Caption = "Nama Poli";
                gridView2.Columns[2].Caption = "Kode Info";
                gridView2.Columns[3].Caption = "Keterangan";
                gridView2.Columns[4].Caption = "Status";

                RepositoryItemLookUpEdit poliLookup = new RepositoryItemLookUpEdit();
                poliLookup.DataSource = poli;
                poliLookup.ValueMember = "poliCode";
                poliLookup.DisplayMember = "poliName";

                poliLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                poliLookup.DropDownRows = poli.Count;
                poliLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                poliLookup.AutoSearchColumnIndex = 1;
                poliLookup.NullText = "";
                gridView2.Columns[1].ColumnEdit = poliLookup;

                RepositoryItemLookUpEdit dLookup = new RepositoryItemLookUpEdit();
                dLookup.DataSource = diagnosaStatus;
                dLookup.ValueMember = "flagCode";
                dLookup.DisplayMember = "flagName";

                dLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                dLookup.DropDownRows = diagnosaStatus.Count;
                dLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                dLookup.AutoSearchColumnIndex = 1;
                dLookup.NullText = "";
                gridView2.Columns[4].ColumnEdit = dLookup;

                gridView2.Columns[0].Visible = false;

                gridView2.BestFitColumns();
                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void btnAddPoli_Click(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView1.AddNewRow();
        }

        private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
        }

        private void btnSavePoli_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_update = "", sql_cnt = "", p_group = "", p_pic="";
            string p_kode = "", p_nama = "", p_status = "", p_action = "";

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                p_action = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                p_kode = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                p_nama = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                p_group = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                p_pic = gridView1.GetRowCellValue(i, gridView1.Columns[4]).ToString();
                p_status = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
                

                if (p_nama == "")
                {
                    MessageBox.Show("Nama Poli harus diisi");
                }
                else if (p_group == "")
                {
                    MessageBox.Show("Grup Poli harus diisi");
                }
                else if (p_pic == "")
                {
                    MessageBox.Show("PIC Poli harus diisi");
                }
                else
                {
                    if (p_action == "I")
                    {
                        sql_insert = "";

                        sql_insert = sql_insert + " insert into cs_policlinic (poli_cd, poli_name, poli_group, poli_pic, status, visible, ins_date, ins_emp) values ";
                        sql_insert = sql_insert + " ('POL' || lpad(CS_POLI_SEQ.nextval,4,'0'), '" + p_nama + "', '" + p_group + "', '" + p_pic + "', 'A', 'Y', sysdate, '" + DB.vUserId + "') ";

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

                        sql_update = sql_update + " update cs_policlinic set poli_name = '" + p_nama + "', poli_group = '" + p_group + "', poli_pic = '" + p_pic + "', ";
                        sql_update = sql_update + " upd_date = sysdate, upd_emp = '" + DB.vUserId + "' ";
                        sql_update = sql_update + " where poli_cd = '" + p_kode + "' ";

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
            btnSavePoli.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Nama Poli" || e.Column.Caption == "Grup Poli" || e.Column.Caption == "PIC Poli")
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

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama Poli" || e.Column.Caption == "Grup Poli" || e.Column.Caption == "PIC Poli")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "poli.xls",
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