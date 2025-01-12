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
    public partial class MasterRoom : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<FlagYn> diagnosaStatus = new List<FlagYn>();
        List<Status> rmStatus = new List<Status>();
        List<Purpose> purpose = new List<Purpose>();
        List<Room> room = new List<Room>();

        public string   v_name = "";
        string kate_cd = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";

        public MasterRoom()
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

            string sql_room = " select room_id, room_name from cs_room  order by room_name ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_room, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            room.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                room.Add(new Room() { roomCode = dt.Rows[i]["room_id"].ToString(), roomName = dt.Rows[i]["room_name"].ToString() });

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
            sql_search = sql_search + Environment.NewLine + "select 'S' action, room_id, room_name, nvl(bed_qty,0) bed_qty, a.ROOM_PRICE CLASS_PRICE, a.CLASS_ID ,room_spv, a.status ";
            sql_search = sql_search + Environment.NewLine + " from cs_room a, CS_ROOM_CLASS b where a.CLASS_ID = b.CLASS_ID(+) ";
            //sql_search = sql_search + Environment.NewLine + "where visible='Y' ";
            sql_search = sql_search + Environment.NewLine + "order by room_name ";

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
                gridView1.Columns[1].Caption = "Kode Ruangan";
                gridView1.Columns[2].Caption = "Nama Ruangan";
                gridView1.Columns[3].Caption = "Tempat Tidur";
                gridView1.Columns[4].Caption = "Harga";
                gridView1.Columns[5].Caption = "Kelas";
                gridView1.Columns[6].Caption = "Penanggung Jawab";
                gridView1.Columns[7].Caption = "Status";

                RepositoryItemLookUpEdit dLookup = new RepositoryItemLookUpEdit();
                dLookup.DataSource = diagnosaStatus;
                dLookup.ValueMember = "flagCode";
                dLookup.DisplayMember = "flagName";

                dLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                dLookup.DropDownRows = diagnosaStatus.Count;
                dLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                dLookup.AutoSearchColumnIndex = 1;
                dLookup.NullText = "";
                gridView1.Columns[7].ColumnEdit = dLookup;

                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].OptionsColumn.AllowEdit = false;
                gridView1.Columns[3].OptionsColumn.AllowEdit = false;
                //gridView1.Columns[1].OptionsColumn.ReadOnly = true;
                gridView1.Columns[7].OptionsColumn.ReadOnly = true;

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

            string room_id = "";

            room_id = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);

            string sql_search, stat = "";
            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select 'S' action, room_id, bed_id, use_yn, status ";
            sql_search = sql_search + Environment.NewLine + "from cs_bed ";
            sql_search = sql_search + Environment.NewLine + "where 1=1 ";
            sql_search = sql_search + Environment.NewLine + "and room_id = '" + room_id + "' ";
            sql_search = sql_search + Environment.NewLine + "order by bed_id ";

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
                gridView2.OptionsBehavior.Editable = true;


                //gridView2.OptionsSelection.MultiSelect = true;
                //gridView2.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView2.VisibleColumns[0].Width = 20;
                //gridView2.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView2.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView2.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView2.Columns[4].Visible = false;

                gridView2.Columns[0].Caption = "Action";
                gridView2.Columns[1].Caption = "Nama Ruangan";
                gridView2.Columns[2].Caption = "Kode";
                gridView2.Columns[3].Caption = "Dipakai / Tidak";
                gridView2.Columns[4].Caption = "Status";

                RepositoryItemLookUpEdit rLookup = new RepositoryItemLookUpEdit();
                rLookup.DataSource = room;
                rLookup.ValueMember = "roomCode";
                rLookup.DisplayMember = "roomName";

                rLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                rLookup.DropDownRows = room.Count;
                rLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                rLookup.AutoSearchColumnIndex = 1;
                rLookup.NullText = "";
                gridView2.Columns[1].ColumnEdit = rLookup;

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
                gridView2.Columns[1].OptionsColumn.ReadOnly = true;
                gridView2.Columns[2].OptionsColumn.ReadOnly = true;
                gridView2.Columns[3].OptionsColumn.ReadOnly = true;

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
            string sql_insert = "", sql_update = "", sql_cnt = "", p_jml = "", p_pic="", p_class="", p_price ="";
            string p_kode = "", p_nama = "", p_status = "", p_action = "";
            
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                p_action = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                p_kode = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                p_nama = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                p_jml = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                p_price = gridView1.GetRowCellValue(i, gridView1.Columns[4]).ToString();
                p_class = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
                p_pic = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                p_status = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();
                

                if (p_nama == "")
                {
                    MessageBox.Show("Nama Ruangan harus diisi");
                }
                else
                {
                    if (p_action == "I")
                    {
                        sql_insert = "";

                        sql_insert = sql_insert + " insert into cs_room (room_id, room_name, bed_qty, room_spv, CLASS_ID, status, ins_date, ins_emp, ROOM_PRICE) values ";
                        sql_insert = sql_insert + " ('RM' || lpad(CS_ROOM_SEQ.nextval,3,'0'), '" + p_nama + "', '0', '" + p_pic + "', " + p_class +", 'A', sysdate, '" + DB.vUserId + "', '" + p_price + "') ";

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

                        sql_update = sql_update + " update cs_room set room_name = '" + p_nama + "', room_spv = '" + p_pic + "', ROOM_PRICE =  '" + p_price + "', ";
                        sql_update = sql_update + " upd_date = sysdate, upd_emp = '" + DB.vUserId + "' ";
                        sql_update = sql_update + " where room_id = '" + p_kode + "' ";

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

            if (e.Column.Caption == "Nama Ruangan" || e.Column.Caption == "Penanggung Jawab" || e.Column.Caption == "Tempat Tidur" || e.Column.Caption == "Harga" || e.Column.Caption == "Kelas")
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

            if (e.Column.Caption == "Nama Ruangan" || e.Column.Caption == "Penanggung Jawab" || e.Column.Caption == "Tempat Tidur" || e.Column.Caption == "Harga" || e.Column.Caption == "Kelas")
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
                    FileName = "ruangan.xls",
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

        private void btnAddBed_Click(object sender, EventArgs e)
        {
            string kd_ruangan = "", sql_cek="", cur_val="";
            int next_val = 0;

            kd_ruangan = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();

            sql_cek = sql_cek + Environment.NewLine + "select nvl(max(to_number(substr(bed_id,-2))),0) nno from cs_bed where room_id = '"+ kd_ruangan + "'  ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cek, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                cur_val = dt.Rows[0]["nno"].ToString();
            }
            else
            {
                cur_val = "0";
            }

            if (Convert.ToInt32(cur_val) >= 10)
            {
                MessageBox.Show("Data tidak dapat ditambah");
                return;
            }

            next_val = Convert.ToInt32(cur_val) + 1;

            OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
            OleDbCommand command = new OleDbCommand();
            OleDbTransaction trans = null;

            command.Connection = oraConnectTrans;
            oraConnectTrans.Open();

            try
            {
                trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                command.Connection = oraConnectTrans;
                command.Transaction = trans;

                command.CommandText = " insert into cs_bed (bed_id, room_id, status, use_yn, ins_date, ins_emp) " +
                                      " values('"+ kd_ruangan + "' || '-' || lpad('" + next_val.ToString() + "',2,'0') , '" + kd_ruangan + "', 'A', 'N', sysdate, '" + DB.vUserId + "') ";
                command.ExecuteNonQuery();

                command.CommandText = " update cs_room set bed_qty = '" + next_val.ToString() + "', upd_emp = '" + DB.vUserId + "', upd_date = sysdate where room_id = '" + kd_ruangan + "'  ";
                command.ExecuteNonQuery();

                trans.Commit();
                MessageBox.Show("Data Berhasil disimpan.");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                MessageBox.Show("ERROR: " + ex.Message);
            }

            oraConnectTrans.Close();
            loadData();
        }

        private void gridView2_InitNewRow(object sender, InitNewRowEventArgs e)
        {

        }

        private void btnSaveBed_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_update = "", sql_cnt = "", p_use = "", p_pic = "";
            string p_kode = "", p_nama = "", p_status = "", p_action = "";

            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                p_action = gridView2.GetRowCellValue(i, gridView2.Columns[0]).ToString();
                p_nama = gridView2.GetRowCellValue(i, gridView2.Columns[1]).ToString();
                p_kode = gridView2.GetRowCellValue(i, gridView2.Columns[2]).ToString();
                p_use = gridView2.GetRowCellValue(i, gridView2.Columns[3]).ToString();
                p_status = gridView2.GetRowCellValue(i, gridView2.Columns[4]).ToString();


                if (p_use == "Y")
                {
                    MessageBox.Show("Data tidak bisa dirubah");
                    return;
                }
                else
                {
                    if (p_action == "U")
                    {
                        sql_update = "";

                        sql_update = sql_update + " update cs_bed set status = '" + p_status + "',   ";
                        sql_update = sql_update + " upd_date = sysdate, upd_emp = '" + DB.vUserId + "' ";
                        sql_update = sql_update + " where bed_id = '" + p_kode + "' ";

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

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveBed.Enabled = true;
            GridView view = sender as GridView;
            if (e.Column.Caption == "Status")
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

            if (e.Column.Caption == "Status")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
            
            if (e.Column.Caption == "Dipakai / Tidak")
            {
                string tmp_stat = gridView2.GetRowCellValue(e.RowHandle, gridView2.Columns[3]).ToString();
                if (tmp_stat == "Y")
                {
                    e.Appearance.BackColor = Color.Crimson;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                
            }
        }
    }
}