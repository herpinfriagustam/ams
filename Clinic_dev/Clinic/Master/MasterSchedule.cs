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
    public partial class MasterSchedule : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<FlagYn> userStatus = new List<FlagYn>();
        List<Stat> listBagian = new List<Stat>();
        List<Poli> listPoli = new List<Poli>(); List<Dokter> listDokter = new List<Dokter>();
        DataTable dtGlRole = new DataTable();
        RepositoryItemLookUpEdit glRole = new RepositoryItemLookUpEdit();
        RepositoryItemLookUpEdit glStatus = new RepositoryItemLookUpEdit();

        RepositoryItemGridLookUpEdit LokPoli = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit LokDokter = new RepositoryItemGridLookUpEdit(); 

        public string   v_name = "";
        string kate_cd = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";

        public MasterSchedule()
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
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "MasterSchedule");
        }

        private void initData()
        {
            dtGlRole.Clear();

            userStatus.Clear();
            userStatus.Add(new FlagYn() { flagCode = "", flagName = "" });
            userStatus.Add(new FlagYn() { flagCode = "Y", flagName = "Aktif" });
            userStatus.Add(new FlagYn() { flagCode = "N", flagName = "Tidak Aktif" });

            //string sql_bag  = " select CODE_ID, CODE_NAME from CS_CODE_DATA where status = 'A' and CODE_CLASS_ID ='DOC_BAGIAN' ";
            //OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_bag, sqlConnect2);
            //DataTable dt2 = new DataTable();
            //adSql2.Fill(dt2);
            //listBagian.Clear();
            //for (int i = 0; i < dt2.Rows.Count; i++)
            //{
            //    listBagian.Add(new Stat() { statCode = dt2.Rows[i]["CODE_ID"].ToString(), statName = dt2.Rows[i]["CODE_NAME"].ToString() }); 
            //}

            string sql_poli = " select POLI_CD, POLI_NAME from CS_POLICLINIC where STATUS = 'A'   ";
            OleDbConnection sqlCon1 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql1 = new OleDbDataAdapter(sql_poli, sqlCon1);
            DataTable dt1 = new DataTable();
            adSql1.Fill(dt1);
            listPoli.Clear();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                listPoli.Add(new Poli() { poliCode = dt1.Rows[i]["POLI_CD"].ToString(), poliName = dt1.Rows[i]["POLI_NAME"].ToString() });
            }

            string sql_dokter = " select ID_DOKTER, NM_DOKTER from CS_DOKTER where F_AKTIF = 'Y'   ";
            OleDbConnection sqlCon2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_dokter, sqlCon2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);
            listDokter.Clear();
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                listDokter.Add(new Dokter() { ID_Dokter = dt2.Rows[i]["ID_DOKTER"].ToString(), Nama_Dokter = dt2.Rows[i]["NM_DOKTER"].ToString() });
            }
        }

        private void btnLoadDosis_Click(object sender, EventArgs e)
        {
            initData();
            loadData();
        }

        private void loadData()
        {
            string Sql ="" ;

            Sql = "";
            Sql = Sql + Environment.NewLine + "select 'S' action, ID_JADWAL, TGL_JADWAL, JAM_AWAL, JAM_AKHIR, a.POLI_CD, a.ID_DOKTER, b.NM_DOKTER, b.SPESIALIS, b.NIK_DOKTER, ";
            Sql = Sql + Environment.NewLine + "       a.ID_PENGGANTI, c.NM_DOKTER PDOKTER, c.SPESIALIS PSPESIALIS, C.NIK_DOKTER, a.nremark,  FLIMIT, NVL(a.UPD_DATE,a.INS_DATE) INS_DATE, NVL(a.UPD_EMP,a.INS_EMP) INS_EMP, A.F_AKTIF ";
            Sql = Sql + Environment.NewLine + "  from KLINIK.CS_DOKTER_SCH a, ";
            Sql = Sql + Environment.NewLine + "       KLINIK.CS_DOKTER b, ";
            Sql = Sql + Environment.NewLine + "       KLINIK.CS_DOKTER c, klinik.CS_POLICLINIC d ";
            Sql = Sql + Environment.NewLine + " where a.ID_DOKTER = b.ID_DOKTER ";
            Sql = Sql + Environment.NewLine + "   and a.ID_PENGGANTI = c.ID_DOKTER(+)  and a.POLI_CD = d.POLI_CD    ";
            Sql = Sql + Environment.NewLine + " order by 3,2,1   ";
             
            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(Sql, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                //gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView1.OptionsView.ColumnAutoWidth = false;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 40;
                gridView1.OptionsBehavior.Editable = true; 

                gridView1.Columns[0].Caption = "Action";
                gridView1.Columns[1].Caption = "ID JADWAL";
                gridView1.Columns[2].Caption = "TGL JADWAL";
                gridView1.Columns[3].Caption = "JAM AWAL";
                gridView1.Columns[4].Caption = "JAM AKHIR";
                gridView1.Columns[5].Caption = "POLI";
                gridView1.Columns[6].Caption = "ID DOKTER";
                gridView1.Columns[7].Caption = "NAMA DOKTER";
                gridView1.Columns[8].Caption = "SPESIALIS";
                gridView1.Columns[9].Caption = "NIK";
                gridView1.Columns[10].Caption = "DOKTER PENGGANTI";
                gridView1.Columns[11].Caption = "NAMA DOKTER PENGGANTI";
                gridView1.Columns[12].Caption = "SPESIALIS";
                gridView1.Columns[13].Caption = "NIK PENGGANTI";
                gridView1.Columns[14].Caption = "NREMARK";
                gridView1.Columns[15].Caption = "LIMIT";
                gridView1.Columns[16].Caption = "Tgl Register";
                gridView1.Columns[17].Caption = "ID Register";
                gridView1.Columns[18].Caption = "Status"; 

                //gridView1.Columns[8].VisibleIndex = 5;

                ConnOra.LookUpGridFilter(listPoli, gridView1, "poliCode", "poliName", LokPoli, 5);
                ConnOra.LookUpGridFilter(listDokter, gridView1, "ID_Dokter", "Nama_Dokter", LokDokter, 6);
                ConnOra.LookUpGridFilter(listDokter, gridView1, "ID_Dokter", "Nama_Dokter", LokDokter, 10);

                RepositoryItemDateEdit rptanggal = new RepositoryItemDateEdit();
                rptanggal.DisplayFormat.FormatString = "yyyy-MM-dd";
                rptanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                rptanggal.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                rptanggal.Mask.EditMask = "yyyy-MM-dd";
                rptanggal.Mask.UseMaskAsDisplayFormat = true;
                gridView1.Columns[2].ColumnEdit = rptanggal;

                RepositoryItemTextEdit rpjam = new RepositoryItemTextEdit();
                rpjam.Mask.EditMask = "90:00";
                rpjam.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
                gridView1.Columns[3].ColumnEdit = rpjam;
                gridView1.Columns[4].ColumnEdit = rpjam; 


                ////RepositoryItemGridLookUpEdit glRole = new RepositoryItemGridLookUpEdit();
                //glRole.DataSource = listBagian;
                //glRole.ValueMember = "statCode";
                //glRole.DisplayMember = "statName";

                //glRole.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //glRole.AutoSearchColumnIndex = 1;
                //glRole.ImmediatePopup = true;
                //glRole.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                //glRole.NullText = "";
                //gridView1.Columns[4].ColumnEdit = glRole;

                glStatus.DataSource = userStatus;
                glStatus.ValueMember = "flagCode";
                glStatus.DisplayMember = "flagName";

                glStatus.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glStatus.AutoSearchColumnIndex = 1;
                glStatus.ImmediatePopup = true;
                glStatus.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glStatus.NullText = "";
                gridView1.Columns[18].ColumnEdit = glStatus;

                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].OptionsColumn.ReadOnly = true;
                gridView1.Columns[7].OptionsColumn.ReadOnly = true;
                gridView1.Columns[8].OptionsColumn.ReadOnly = true; 
                gridView1.Columns[9].OptionsColumn.ReadOnly = false;  
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
            view.SetRowCellValue(e.RowHandle, view.Columns[7], "Y");
        }

        private void btnSaveDosis_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_update = "", sql_cnt = "", p_spesial = "", p_nama = "", p_nik ="";
            string p_bagian = "", p_pass = "", p_id = "", p_status = "", p_action = "";
            
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                p_action = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                p_id = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                p_nama = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                p_spesial = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                p_bagian = gridView1.GetRowCellValue(i, gridView1.Columns[4]).ToString();
                p_status = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();
                p_nik   = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();

                if (p_nama == "")
                {
                    MessageBox.Show("Nama Dokter harus diisi"); return;
                } 
                else if (p_spesial == "")
                {
                    MessageBox.Show("Spesialis harus diisi"); return;
                }
                else if (p_bagian == "")
                {
                    MessageBox.Show("Bagian harus diisi"); return;
                }
                //else if (p_nik == "")
                //{
                //    MessageBox.Show("NIK harus diisi"); return;
                //}
                else
                {
                    if (p_action == "I")
                    {
                        sql_insert = "";  
                        sql_insert = sql_insert + " insert into KLINIK.CS_DOKTER (ID_DOKTER, NM_DOKTER, SPESIALIS, BAGIAN, F_AKTIF, INS_DATE, INS_EMP, NIK_DOKTER) values ";
                        sql_insert = sql_insert + " (KLINIK.CS_DOKTER_SEQ.nextval , '" + p_nama + "', '" + p_spesial + "',  '" + p_bagian + "', 'Y', sysdate, '" + DB.vUserId + "', '" + p_nik + "') ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();
                             
                            MessageBox.Show("Data Dokter Berhasil ditambah");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                    else if (p_action == "U")
                    {
                        sql_update = "";

                        sql_update = sql_update + " update KLINIK.CS_DOKTER  set  NM_DOKTER = '" + p_nama + "', SPESIALIS = '" + p_spesial + "' , BAGIAN = '" + p_bagian + "', NIK_DOKTER = '" + p_nik + "', ";
                        sql_update = sql_update + " UPD_DATE = sysdate, UPD_EMP = '" + DB.vUserId + "', F_AKTIF = '" + p_status + "'  ";
                        sql_update = sql_update + " where ID_DOKTER = '" + p_id + "' ";

                        try
                        {
                            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm2 = new OleDbCommand(sql_update, oraConnect2);
                            oraConnect2.Open();
                            cm2.ExecuteNonQuery();
                            oraConnect2.Close();
                            cm2.Dispose();
                             
                            MessageBox.Show("Data Dokter Berhasil dirubah");
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
 
            if (e.Column.Caption == "Nama Dokter" || e.Column.Caption == "SPESIALIS" || e.Column.Caption == "BAGIAN" || e.Column.Caption == "NIK" || e.Column.Caption == "Status")
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

            if (e.Column.Caption == "Nama Dokter" || e.Column.Caption == "SPESIALIS" || e.Column.Caption == "BAGIAN" || e.Column.Caption == "Status" || e.Column.Caption == "NIK")
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

                sql_delete = sql_delete + " update CS_DOKTER set F_AKTIF = 'N', UPD_DATE = sysdate, UPD_EMP = '" + DB.vUserId + "' ";
                sql_delete = sql_delete + " where ID_DOKTER = '" + id + "' ";

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