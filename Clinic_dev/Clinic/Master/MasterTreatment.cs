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
    public partial class MasterTreatment : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<FlagYn> diagnosaStatus = new List<FlagYn>();
        List<Layanan> listTipe = new List<Layanan>();
        List<Layanan> listGroup = new List<Layanan>();
        List<Status> listStat2 = new List<Status>();
        public string  v_name = "";
        string kate_cd = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        //string today = "2019-11-27";

        public MasterTreatment()
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

        private void gridView3_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView4_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void initData()
        {
            diagnosaStatus.Clear();
            diagnosaStatus.Add(new FlagYn() { flagCode = "", flagName = "" });
            diagnosaStatus.Add(new FlagYn() { flagCode = "A", flagName = "Aktif" });
            diagnosaStatus.Add(new FlagYn() { flagCode = "I", flagName = "Tidak Aktif" });

            string sql_tipe = " select treat_type_id, treat_type_name from cs_treatment_type  order by treat_type_id ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_tipe, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            listTipe.Clear();

            listTipe.Add(new Layanan() { layananCode = "", layananName = "Pilih" });
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listTipe.Add(new Layanan() { layananCode = dt.Rows[i]["treat_type_id"].ToString(), layananName = dt.Rows[i]["treat_type_name"].ToString() });

            }

            string sql_group = " select treat_group_id, treat_group_name from cs_treatment_group  order by treat_group_order ";
            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_group, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);
            listGroup.Clear();
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                listGroup.Add(new Layanan() { layananCode = dt2.Rows[i]["treat_group_id"].ToString(), layananName = dt2.Rows[i]["treat_group_name"].ToString() });

            }

            listStat2.Clear();
            listStat2.Add(new Status() { statusCode = "U", statusName = "Umum" });
            listStat2.Add(new Status() { statusCode = "B", statusName = "BPJS" });
            listStat2.Add(new Status() { statusCode = "S", statusName = "Swasta" });

        }

        private void loadDataTrType()
        {
            string sql_search, stat = "";
            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select 'S' action, treat_type_id, treat_type_name, status ";
            sql_search = sql_search + Environment.NewLine + "from cs_treatment_type ";
            sql_search = sql_search + Environment.NewLine + "order by treat_type_id ";

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
                gridView1.OptionsBehavior.Editable = false;


                //gridView1.OptionsSelection.MultiSelect = true;
                //gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView1.VisibleColumns[0].Width = 20;
                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[4].Visible = false;

                gridView1.Columns[0].Caption = "Action";
                gridView1.Columns[1].Caption = "Kode";
                gridView1.Columns[2].Caption = "Tipe Layanan";
                gridView1.Columns[3].Caption = "Status";

                RepositoryItemLookUpEdit dLookup = new RepositoryItemLookUpEdit();
                dLookup.DataSource = diagnosaStatus;
                dLookup.ValueMember = "flagCode";
                dLookup.DisplayMember = "flagName";

                dLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                dLookup.DropDownRows = diagnosaStatus.Count;
                dLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                dLookup.AutoSearchColumnIndex = 1;
                dLookup.NullText = "";
                gridView1.Columns[3].ColumnEdit = dLookup;

                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].OptionsColumn.AllowEdit = false;
                gridView1.Columns[2].OptionsColumn.AllowEdit = false;
                gridView1.Columns[3].OptionsColumn.ReadOnly = true;

                gridView1.BestFitColumns();
                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void loadDataTrGroup()
        {
            string sql_search, stat = "";
            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select 'S' action, treat_group_id, treat_group_name, treat_group_order, status ";
            sql_search = sql_search + Environment.NewLine + "from cs_treatment_group ";
            sql_search = sql_search + Environment.NewLine + "order by treat_group_order ";

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
                gridView2.IndicatorWidth = 40;
                gridView2.OptionsBehavior.Editable = true;


                //gridView2.OptionsSelection.MultiSelect = true;
                //gridView2.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView2.VisibleColumns[0].Width = 20;
                //gridView2.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView2.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView2.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView2.Columns[4].Visible = false;

                gridView2.Columns[0].Caption = "Action";
                gridView2.Columns[1].Caption = "Kode";
                gridView2.Columns[2].Caption = "Grup Layanan";
                gridView2.Columns[3].Caption = "Urut";
                gridView2.Columns[4].Caption = "Status";

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
                gridView2.Columns[1].OptionsColumn.AllowEdit = false;
                gridView2.Columns[4].OptionsColumn.ReadOnly = true;

                gridView2.BestFitColumns();
                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void loadDataTrItem()
        {
            string sql_search, stat = "";
            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select 'S' action, treat_item_id, treat_type_id, treat_group_id, treat_item_name, ";
            sql_search = sql_search + Environment.NewLine + "treat_item_price, default_st, F_STATUS  TYPE, Used_by User_By,  status ";
            sql_search = sql_search + Environment.NewLine + "from cs_treatment_item ";
            sql_search = sql_search + Environment.NewLine + "order by treat_item_id ";

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

                //gridView3.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView3.OptionsView.ColumnAutoWidth = true;
                gridView3.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView3.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView3.IndicatorWidth = 40;
                gridView3.OptionsBehavior.Editable = true;


                //gridView3.OptionsSelection.MultiSelect = true;
                //gridView3.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView3.VisibleColumns[0].Width = 20;
                //gridView3.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView3.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView3.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView3.Columns[4].Visible = false;

                gridView3.Columns[0].Caption = "Action";
                gridView3.Columns[1].Caption = "Kode";
                gridView3.Columns[2].Caption = "Tipe Layanan";
                gridView3.Columns[3].Caption = "Grup Layanan";
                gridView3.Columns[4].Caption = "Nama Layanan";
                gridView3.Columns[5].Caption = "Harga";
                gridView3.Columns[6].Caption = "Default";
                gridView3.Columns[7].Caption = "Type";
                gridView3.Columns[8].Caption = "User_By";
                gridView3.Columns[9].Caption = "Status";

                RepositoryItemLookUpEdit tLookup = new RepositoryItemLookUpEdit();
                tLookup.DataSource = listTipe;
                tLookup.ValueMember = "layananCode";
                tLookup.DisplayMember = "layananName";

                tLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                tLookup.DropDownRows = listTipe.Count;
                tLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                tLookup.AutoSearchColumnIndex = 1;
                tLookup.NullText = "";
                gridView3.Columns[2].ColumnEdit = tLookup;

                RepositoryItemLookUpEdit gLookup = new RepositoryItemLookUpEdit();
                gLookup.DataSource = listGroup;
                gLookup.ValueMember = "layananCode";
                gLookup.DisplayMember = "layananName";

                gLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                gLookup.DropDownRows = listGroup.Count;
                gLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                gLookup.AutoSearchColumnIndex = 1;
                gLookup.NullText = "";
                gridView3.Columns[3].ColumnEdit = gLookup;

                RepositoryItemLookUpEdit gLType = new RepositoryItemLookUpEdit();
                gLType.DataSource = listStat2;
                gLType.ValueMember = "statusCode";
                gLType.DisplayMember = "statusName";

                gLType.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                gLType.DropDownRows = listStat2.Count;
                gLType.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                gLType.AutoSearchColumnIndex = 1;
                gLType.NullText = "";
                gridView3.Columns[7].ColumnEdit = gLType;


                RepositoryItemLookUpEdit dLookup = new RepositoryItemLookUpEdit();
                dLookup.DataSource = diagnosaStatus;
                dLookup.ValueMember = "flagCode";
                dLookup.DisplayMember = "flagName";

                dLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                dLookup.DropDownRows = diagnosaStatus.Count;
                dLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                dLookup.AutoSearchColumnIndex = 1;
                dLookup.NullText = "";
                gridView3.Columns[9].ColumnEdit = dLookup;

                gridView3.Columns[0].Visible = false;
                gridView3.Columns[1].OptionsColumn.AllowEdit = false;
                gridView3.Columns[9].OptionsColumn.ReadOnly = true;

                gridView3.BestFitColumns();
                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void loadDataRsv()
        {
            string sql_search;
            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select 'S' action, code_class_id, code_id, code_name, status, sort_order, ";
            sql_search = sql_search + Environment.NewLine + "attr_01, attr_02, attr_03, attr_04, attr_05, attr_06 ";
            sql_search = sql_search + Environment.NewLine + "from cs_code_data ";
            sql_search = sql_search + Environment.NewLine + "where code_class_id = 'RESV_ITEM' ";
            sql_search = sql_search + Environment.NewLine + "and status = 'A' ";
            sql_search = sql_search + Environment.NewLine + "order by to_number(attr_01), to_number(sort_order) ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_search, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl4.DataSource = null;
                gridView4.Columns.Clear();
                gridControl4.DataSource = dt;

                //gridView4.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView4.OptionsView.ColumnAutoWidth = true;
                gridView4.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView4.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView4.IndicatorWidth = 40;
                gridView4.OptionsBehavior.Editable = true;


                //gridView4.OptionsSelection.MultiSelect = true;
                //gridView4.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView4.VisibleColumns[0].Width = 20;
                //gridView4.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView4.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView4.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView4.Columns[4].Visible = false;

                gridView4.Columns[0].Caption = "Action";
                gridView4.Columns[1].Caption = "Kode Kelas";
                gridView4.Columns[2].Caption = "Kode";
                gridView4.Columns[3].Caption = "Nama";
                gridView4.Columns[4].Caption = "Status";
                gridView4.Columns[5].Caption = "Urut";
                gridView4.Columns[6].Caption = "Attr 01";
                gridView4.Columns[7].Caption = "Attr 02";
                gridView4.Columns[8].Caption = "Attr 03";
                gridView4.Columns[9].Caption = "Attr 04";
                gridView4.Columns[10].Caption = "Attr 05";
                gridView4.Columns[11].Caption = "Attr 06";

                gridView4.Columns[0].Visible = false;
                gridView4.Columns[1].OptionsColumn.ReadOnly = true;
                gridView4.Columns[2].OptionsColumn.ReadOnly = true;
                //gridView4.Columns[7].OptionsColumn.ReadOnly = true;

                gridView4.BestFitColumns();
                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void MasterTreatment_Load(object sender, EventArgs e)
        {
            initData();
            loadDataTrType();
            loadDataTrGroup();
            loadDataTrItem();
            loadDataRsv();
        }

        private void btnLoadType_Click(object sender, EventArgs e)
        {
            loadDataTrType();
        }

        private void btnDownType_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "tipe_layanan.xls",
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

        private void btnLoadGroup_Click(object sender, EventArgs e)
        {
            loadDataTrGroup();
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            gridView2.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView2.AddNewRow();
        }

        private void gridView2_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
        }

        private void btnSaveGroup_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_update = "", sql_cnt = "", p_ord = "";
            string p_kode = "", p_nama = "", p_status = "", p_action = "";
            
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                p_action = gridView2.GetRowCellValue(i, gridView2.Columns[0]).ToString();
                p_kode = gridView2.GetRowCellValue(i, gridView2.Columns[1]).ToString();
                p_nama = gridView2.GetRowCellValue(i, gridView2.Columns[2]).ToString();
                p_ord = gridView2.GetRowCellValue(i, gridView2.Columns[3]).ToString();
                p_status = gridView2.GetRowCellValue(i, gridView2.Columns[4]).ToString();


                if (p_nama == "")
                {
                    MessageBox.Show("Grup Layanan harus diisi");
                }
                else if (p_ord == "")
                {
                    MessageBox.Show("Urut harus diisi");
                }
                else
                {
                    if (p_action == "I")
                    {
                        sql_insert = "";

                        sql_insert = sql_insert + " insert into cs_treatment_group (treat_group_id, treat_group_name, treat_group_order, status, ins_date, ins_emp) values ";
                        sql_insert = sql_insert + " ('TRG' || lpad(CS_TREATMENT_GROUP_SEQ.nextval,2,'0'), '" + p_nama + "', '" + p_ord + "', 'A', sysdate, '" + DB.vUserId + "') ";

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

                        sql_update = sql_update + " update cs_treatment_group set treat_group_name = '" + p_nama + "', treat_group_order = '" + p_ord + "',  ";
                        sql_update = sql_update + " upd_date = sysdate, upd_emp = '" + DB.vUserId + "' ";
                        sql_update = sql_update + " where treat_group_id = '" + p_kode + "' ";

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
            loadDataTrGroup();
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveGroup.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Grup Layanan" || e.Column.Caption == "Urut")
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

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Grup Layanan" || e.Column.Caption == "Urut")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnDownGroup_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "grup_layanan.xls",
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
            else
            {
                MessageBox.Show("Data tidak ditemukan");
            }
        }

        private void btnLoadItem_Click(object sender, EventArgs e)
        {
            loadDataTrItem();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            gridView3.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView3.AddNewRow();
        }

        private void gridView3_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[6], "N");
        }

        private void btnSaveItem_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_update = "";
            string p_kode = "", p_tipe="", p_grup="", p_nama = "", p_harga="", p_default="", p_status = "", p_action = "";

            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                p_action = gridView3.GetRowCellValue(i, gridView3.Columns[0]).ToString();
                p_kode = gridView3.GetRowCellValue(i, gridView3.Columns[1]).ToString();
                p_tipe = gridView3.GetRowCellValue(i, gridView3.Columns[2]).ToString();
                p_grup = gridView3.GetRowCellValue(i, gridView3.Columns[3]).ToString();
                p_nama = gridView3.GetRowCellValue(i, gridView3.Columns[4]).ToString();
                p_harga = gridView3.GetRowCellValue(i, gridView3.Columns[5]).ToString();
                p_default = gridView3.GetRowCellValue(i, gridView3.Columns[6]).ToString();
                p_status = gridView3.GetRowCellValue(i, gridView3.Columns[7]).ToString();

                if (p_grup == "")
                {
                    MessageBox.Show("Grup Layanan harus diisi");
                }
                else if (p_nama == "")
                {
                    MessageBox.Show("Nama Layanan harus diisi");
                }
                else if (p_harga == "")
                {
                    MessageBox.Show("Harga harus diisi");
                }
                else if (p_default == "")
                {
                    MessageBox.Show("Default harus diisi");
                }
                else
                {
                    if (p_action == "I")
                    {
                        sql_insert = "";

                        sql_insert = sql_insert + " insert into cs_treatment_item (treat_item_id, treat_item_name, treat_type_id, treat_group_id, treat_item_price, default_st, status, visible, ins_date, ins_emp) values ";
                        sql_insert = sql_insert + " (CS_TREATMENT_ITEM_SEQ.nextval, '" + p_nama + "', '" + p_tipe + "', '" + p_grup + "', '" + p_harga + "', '" + p_default + "', 'A', 'Y', sysdate, '" + DB.vUserId + "') ";

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

                        sql_update = sql_update + " update cs_treatment_item set treat_item_name = '" + p_nama + "', treat_type_id = '" + p_tipe + "',  ";
                        sql_update = sql_update + " treat_group_id = '" + p_grup + "', treat_item_price = '" + p_harga + "', default_st = '" + p_default + "',  ";
                        sql_update = sql_update + " upd_date = sysdate, upd_emp = '" + DB.vUserId + "' ";
                        sql_update = sql_update + " where treat_item_id = '" + p_kode + "' ";

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
            loadDataTrItem();
        }

        private void gridView3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveItem.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Tipe Layanan" || e.Column.Caption == "Grup Layanan" || e.Column.Caption == "Nama Layanan" || e.Column.Caption == "Harga" || e.Column.Caption == "Default")
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

        private void gridView3_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Tipe Layanan" || e.Column.Caption == "Grup Layanan" || e.Column.Caption == "Nama Layanan" || e.Column.Caption == "Harga" || e.Column.Caption == "Default")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnDelItem_Click(object sender, EventArgs e)
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

                id = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns[1]).ToString();

                sql_delete = "";

                sql_delete = sql_delete + " update cs_treatment_item set visible = 'N' ";
                sql_delete = sql_delete + " where treat_item_id = '" + id + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);
                    gridView3.DeleteRow(gridView3.FocusedRowHandle);
                    MessageBox.Show("Data Berhasil dihapus");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }

        private void btnDownItem_Click(object sender, EventArgs e)
        {
            if (gridView3.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "item_layanan.xls",
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
                    gridControl3.ExportToXls(saveDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("Data tidak ditemukan");
            }
        }

        private void btnLoadRsv_Click(object sender, EventArgs e)
        {
            loadDataRsv();
        }

        private void btnAddRsv_Click(object sender, EventArgs e)
        {
            gridView4.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView4.Columns[2].OptionsColumn.ReadOnly = false;
            gridView4.Columns[2].OptionsColumn.AllowEdit = true;
            gridView4.AddNewRow();
        }

        private void gridView4_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[1], "RESV_ITEM");
            view.SetRowCellValue(e.RowHandle, view.Columns[4], "A");
        }

        private void btnSaveRsv_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_update = "";
            string p_kls = "", p_kode = "", p_nama = "", p_stat = "", p_urut = "", p_attr01 = "", p_attr02 = "", p_attr03 = "", p_action = "";
            string p_attr04 = "", p_attr05 = "", p_attr06 = "";

            for (int i = 0; i < gridView4.DataRowCount; i++)
            {
                p_action = gridView4.GetRowCellValue(i, gridView4.Columns[0]).ToString();
                p_kls = gridView4.GetRowCellValue(i, gridView4.Columns[1]).ToString();
                p_kode = gridView4.GetRowCellValue(i, gridView4.Columns[2]).ToString();
                p_nama = gridView4.GetRowCellValue(i, gridView4.Columns[3]).ToString();
                p_stat = gridView4.GetRowCellValue(i, gridView4.Columns[4]).ToString();
                p_urut = gridView4.GetRowCellValue(i, gridView4.Columns[5]).ToString();
                p_attr01 = gridView4.GetRowCellValue(i, gridView4.Columns[6]).ToString();
                p_attr02 = gridView4.GetRowCellValue(i, gridView4.Columns[7]).ToString();
                p_attr03 = gridView4.GetRowCellValue(i, gridView4.Columns[8]).ToString();
                p_attr04 = gridView4.GetRowCellValue(i, gridView4.Columns[9]).ToString();
                p_attr05 = gridView4.GetRowCellValue(i, gridView4.Columns[10]).ToString();
                p_attr06 = gridView4.GetRowCellValue(i, gridView4.Columns[11]).ToString();

                if (p_kls == "")
                {
                    MessageBox.Show("Kode kelas harus diisi");
                }
                else if (p_kode == "")
                {
                    MessageBox.Show("Kode harus diisi");
                }
                else if (p_nama == "")
                {
                    MessageBox.Show("Nama harus diisi");
                }
                else if (p_stat == "")
                {
                    MessageBox.Show("Status harus diisi");
                }
                else
                {
                    if (p_action == "I")
                    {
                        sql_insert = "";

                        sql_insert = sql_insert + " insert into cs_code_data (code_class_id, code_id, code_name, status, sort_order, attr_01, attr_02, attr_03, attr_04, attr_05, attr_06, ins_date, ins_emp) values ";
                        sql_insert = sql_insert + " ('" + p_kls + "', '" + p_kode + "', '" + p_nama + "', '" + p_stat + "', '" + p_urut + "', '" + p_attr01 + "', '" + p_attr02 + "', '" + p_attr03 + "', '" + p_attr04 + "', '" + p_attr05 + "', '" + p_attr06 + "', sysdate, '" + DB.vUserId + "') ";

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

                        sql_update = sql_update + " update cs_code_data set code_name = '" + p_nama + "', status = '" + p_stat + "',  ";
                        sql_update = sql_update + " sort_order = '" + p_urut + "', attr_01 = '" + p_attr01 + "', attr_02 = '" + p_attr02 + "', attr_03 = '" + p_attr03 + "',  ";
                        sql_update = sql_update + " attr_04 = '" + p_attr04 + "', attr_05 = '" + p_attr05 + "', attr_06 = '" + p_attr06 + "',  ";
                        sql_update = sql_update + " upd_date = sysdate, upd_emp = '" + DB.vUserId + "' ";
                        sql_update = sql_update + " where code_class_id = '" + p_kls + "' and  code_id = '" + p_kode + "'";

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
            loadDataRsv();
        }

        private void gridView4_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveRsv.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Nama" || e.Column.Caption == "Status" || e.Column.Caption == "Urut" || e.Column.Caption == "Attr 01" || e.Column.Caption == "Attr 02" ||
                e.Column.Caption == "Attr 03" || e.Column.Caption == "Attr 04" || e.Column.Caption == "Attr 05" || e.Column.Caption == "Attr 06")
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

        private void gridView4_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama" || e.Column.Caption == "Status" || e.Column.Caption == "Urut" || e.Column.Caption == "Attr 01" || e.Column.Caption == "Attr 02" ||
                e.Column.Caption == "Attr 03" || e.Column.Caption == "Attr 04" || e.Column.Caption == "Attr 05" || e.Column.Caption == "Attr 06")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnDelRsv_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                      "Message",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string sql_delete = "", id_class="", id = "";

                id_class = gridView4.GetRowCellValue(gridView4.FocusedRowHandle, gridView4.Columns[1]).ToString();
                id = gridView4.GetRowCellValue(gridView4.FocusedRowHandle, gridView4.Columns[2]).ToString();

                sql_delete = "";

                sql_delete = sql_delete + " update cs_code_data set status = 'I' ";
                sql_delete = sql_delete + " where code_class_id = '" + id_class + "' and code_id = '" + id + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);
                    gridView4.DeleteRow(gridView4.FocusedRowHandle);
                    MessageBox.Show("Data Berhasil dihapus");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }

        private void btnDownloadRsv_Click(object sender, EventArgs e)
        {
            if (gridView4.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "layanan_reservasi.xls",
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
                    gridControl4.ExportToXls(saveDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("Data tidak ditemukan");
            }

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}