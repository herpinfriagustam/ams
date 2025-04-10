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
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using Clinic.Class.Bpjsws;

namespace Clinic
{
    public partial class MasterSchedule : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        private LabelControl _currentLabel;
        List<FlagYn> userStatus = new List<FlagYn>();
        List<Stat> listBagian = new List<Stat>();
        List<Poli> listPoli = new List<Poli>(); List<Dokter> listDokter = new List<Dokter>();
        DataTable dtGlRole = new DataTable();
        RepositoryItemLookUpEdit glRole = new RepositoryItemLookUpEdit();
        RepositoryItemLookUpEdit glStatus = new RepositoryItemLookUpEdit();

        RepositoryItemGridLookUpEdit LokPoli = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit LokDokter = new RepositoryItemGridLookUpEdit();
        int lsOK = 0;
        bool bl_klap = true;
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
            dDateBgn.Text = today;
            initData();
            loadData();
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "MasterSchedule");
            _currentLabel = lInfo;
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

            string sql_poli = " select POLI_CD, POLI_NAME from CS_POLICLINIC where STATUS = 'A' and POLI_CD not in ('POL0000','POL0004','POL0007','POL0008','POL0009')  ";
            OleDbConnection sqlCon1 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql1 = new OleDbDataAdapter(sql_poli, sqlCon1);
            DataTable dt1 = new DataTable();
            adSql1.Fill(dt1);
            listPoli.Clear();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                listPoli.Add(new Poli() { poliCode = dt1.Rows[i]["POLI_CD"].ToString(), poliName = dt1.Rows[i]["POLI_NAME"].ToString() });
            }

            string sql_dokter = " select ID_DOKTER, NM_DOKTER from CS_DOKTER where F_AKTIF = 'A' and ID_DOKTER not in(99)  ";
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

            Sql = " ";
            Sql = Sql + Environment.NewLine + "select 'S' action, ID_JADWAL, TGL_JADWAL, JAM_AWAL, JAM_AKHIR, a.POLI_CD, b.ID_DOKTER DOKTER, b.NM_DOKTER, b.SPESIALIS, b.NIK_DOKTER, ";
            Sql = Sql + Environment.NewLine + "       a.ID_PENGGANTI, c.NM_DOKTER PDOKTER, c.SPESIALIS PSPESIALIS, C.NIK_DOKTER PNIK_DOKTER, a.nremark,  FLIMIT, to_char(NVL(a.UPD_DATE,a.INS_DATE),'yyyy-MM-dd HH:mm:ss') INS_DATE, NVL(a.UPD_EMP,a.INS_EMP) INS_EMP, A.F_AKTIF ";
            Sql = Sql + Environment.NewLine + "  from KLINIK.CS_DOKTER_SCH a, ";
            Sql = Sql + Environment.NewLine + "       KLINIK.CS_DOKTER b, ";
            Sql = Sql + Environment.NewLine + "       KLINIK.CS_DOKTER c, klinik.CS_POLICLINIC d ";
            Sql = Sql + Environment.NewLine + " where a.ID_DOKTER  = b.ID_DOKTER and a.f_aktif ='Y'";
            Sql = Sql + Environment.NewLine + "   and a.ID_PENGGANTI = c.ID_DOKTER(+) and a.POLI_CD = d.BPJS_KODE_POLI(+)  ";
            Sql = Sql + Environment.NewLine + "   and trunc(TGL_JADWAL) = trunc(to_date( '" + dDateBgn.Text.TrimEnd()  + "','yyyy-MM-dd'))   ";
            Sql = Sql + Environment.NewLine + " order by 3,2,1   ";
              
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(Sql, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;
                 
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
                gridView1.Columns[6].Caption = "DOKTER";
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
                gridView1.Columns[17].Caption = "Register By";
                gridView1.Columns[18].Caption = "Status";  

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

                glStatus.DataSource = userStatus;
                glStatus.ValueMember = "flagCode";
                glStatus.DisplayMember = "flagName";

                glStatus.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glStatus.AutoSearchColumnIndex = 1;
                glStatus.ImmediatePopup = true;
                glStatus.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glStatus.NullText = "";
                gridView1.Columns[18].ColumnEdit = glStatus;

                gridView1.Columns[16].DisplayFormat.FormatString = "d";
                gridView1.Columns[16].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;

                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].Visible = false;
                gridView1.Columns[7].Visible = false;
                gridView1.Columns[11].Visible = false;
                gridView1.Columns[12].Visible = false;
                gridView1.Columns[13].Visible = false;
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

        private void loadPoli()
        { 
            string SQL = " ";
            SQL = SQL + Environment.NewLine + "select DISTINCT A.POLI_CD, A.POLI_NAME, '['||A.BPJS_KODE_POLI||']'||A.BPJS_NAMA_POLI BPJS_POLI, B.FLIMIT, DECODE(A.STATUS,'A','AKTIF','NONE AKTIF') STATUS ";
            SQL = SQL + Environment.NewLine + "  from  CS_POLICLINIC a,  KLINIK.CS_DOKTER_SCH  b ";
            SQL = SQL + Environment.NewLine + " where 1=1 ";
            SQL = SQL + Environment.NewLine + "   AND trunc(TGL_JADWAL) = trunc(sysdate)   ";
            SQL = SQL + Environment.NewLine + "   AND a.POLI_CD = B.POLI_CD "; 

            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl2.DataSource = null;
                gridView2.Columns.Clear();
                gridControl2.DataSource = dt;

                gridView2.OptionsView.ColumnAutoWidth = false;
                gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView2.IndicatorWidth = 40;
                //gridView2.OptionsBehavior.Editable = true;

                gridView2.Columns[0].Caption = "ID POLI";
                gridView2.Columns[1].Caption = "NAMA POLI";
                gridView2.Columns[2].Caption = "BPJS POLI";
                gridView2.Columns[3].Caption = "LIMIT";
                gridView2.Columns[4].Caption = "STATUS";

                gridView2.Columns[0].Width =80;
                gridView2.Columns[1].Width = 150;
                gridView2.Columns[2].Width = 170;
                gridView2.Columns[3].Width = 60;
                gridView2.Columns[4].Width = 70;

                //gridView2.BestFitColumns();

                //ConnOra.LookUpGridFilter(listPoli, gridView1, "poliCode", "poliName", LokPoli, 5);
                //ConnOra.LookUpGridFilter(listDokter, gridView1, "ID_Dokter", "Nama_Dokter", LokDokter, 6);
                //ConnOra.LookUpGridFilter(listDokter, gridView1, "ID_Dokter", "Nama_Dokter", LokDokter, 10);

                //RepositoryItemDateEdit rptanggal = new RepositoryItemDateEdit();
                //rptanggal.DisplayFormat.FormatString = "yyyy-MM-dd";
                //rptanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                //rptanggal.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                //rptanggal.Mask.EditMask = "yyyy-MM-dd";
                //rptanggal.Mask.UseMaskAsDisplayFormat = true;
                //gridView1.Columns[2].ColumnEdit = rptanggal;

                //RepositoryItemTextEdit rpjam = new RepositoryItemTextEdit();
                //rpjam.Mask.EditMask = "90:00";
                //rpjam.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
                //gridView1.Columns[3].ColumnEdit = rpjam;
                //gridView1.Columns[4].ColumnEdit = rpjam;

                //glStatus.DataSource = userStatus;
                //glStatus.ValueMember = "flagCode";
                //glStatus.DisplayMember = "flagName";

                //glStatus.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //glStatus.AutoSearchColumnIndex = 1;
                //glStatus.ImmediatePopup = true;
                //glStatus.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                //glStatus.NullText = "";
                //gridView1.Columns[18].ColumnEdit = glStatus;

                //gridView1.Columns[0].Visible = false;
                //gridView1.Columns[1].Visible = false;
                //gridView1.Columns[11].Visible = false;
                //gridView1.Columns[12].Visible = false;
                //gridView1.Columns[13].Visible = false;
                //gridView1.Columns[1].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[7].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[8].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[9].OptionsColumn.ReadOnly = false;

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
            view.SetRowCellValue(e.RowHandle, view.Columns[18], "Y");
        }

        private void btnSaveDosis_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_update = "", p_action = "", sql ="";
            string p_tgl = "", p_awal = "", p_akhir = "", p_poli = "", p_dokter = "", p_pengganti = "", p_remark = "", p_limit = "";
            int ssimpan = 0;
            
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                p_action = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                p_tgl = gridView1.GetRowCellDisplayText(i, gridView1.Columns[2]).ToString();
                p_awal = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                p_akhir = gridView1.GetRowCellValue(i, gridView1.Columns[4]).ToString();
                p_poli = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
                p_dokter = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                p_pengganti = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                p_remark = gridView1.GetRowCellValue(i, gridView1.Columns[14]).ToString();
                p_limit = gridView1.GetRowCellValue(i, gridView1.Columns[15]).ToString(); 

                if (p_dokter == "")
                {
                    MessageBox.Show("Nama Dokter harus diisi"); return;
                }
                else if(p_dokter == p_pengganti)
                {
                    MessageBox.Show("Dokter Pengganti Tidak Boleh Sama."); return;
                }
                else if (p_limit == "")
                {
                    MessageBox.Show("Limit harus diisi"); return;
                }
                else if (p_poli == "")
                {
                    MessageBox.Show("Poli harus ditentukan"); return;
                }
                else if (p_tgl == "")
                {
                    MessageBox.Show("Tanggal harus diisi"); return;
                }
                else if (p_awal == "" || p_akhir =="")
                {
                    MessageBox.Show("Jam Awal dan Akhir harus diisi"); return;
                }
                else
                {
                    sql = "";
                    sql = "SELECT ID_DOKTER FROM KLINIK.CS_DOKTER_SCH where ID_DOKTER = '" + p_dokter + "' AND TRUNC(TGL_JADWAL) = TO_DATE('" + p_tgl + "','YYYY-MM-DD') and  F_AKTIF = 'Y'  ";
                    DataTable dt_dokterp = ConnOra.Data_Table_ora(sql);

                    if(dt_dokterp.Rows.Count > 0)
                    {
                        if (p_action == "U")
                        {
                            sql_update = "";
                            sql_update = sql_update + " update KLINIK.CS_DOKTER_SCH  set  TGL_JADWAL = TO_DATE('" + p_tgl + "','YYYY-MM-DD'), JAM_AWAL = '" + p_awal + "' , JAM_AKHIR = '" + p_akhir + "', ID_PENGGANTI = '" + p_pengganti + "', NREMARK = '" + p_remark + "',FLIMIT = '" + p_limit + "', POLI_CD =  '" + p_poli + "', ";
                            sql_update = sql_update + "        UPD_DATE = sysdate, UPD_EMP = '" + DB.vUserId + "', F_AKTIF = 'Y'  ";
                            sql_update = sql_update + "  where ID_DOKTER = '" + p_dokter + "' AND TRUNC(TGL_JADWAL) = TO_DATE('" + p_tgl + "','YYYY-MM-DD')";

                            try
                            {
                                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                                OleDbCommand cm2 = new OleDbCommand(sql_update, oraConnect2);
                                oraConnect2.Open();
                                cm2.ExecuteNonQuery();
                                oraConnect2.Close();
                                cm2.Dispose();

                                ssimpan = 1;
                                
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ERROR: " + ex.Message);
                            }
                        }
                        else if (p_action == "I")
                        {
                            MessageBox.Show("Dokter sudah ada Schedule. Schedule tidak dapat di proses..!!!");
                            return;
                        }
                    }
                    else
                    {
                        if (p_action == "I")
                        {
                            sql_insert = "";
                            sql_insert = sql_insert + " insert into KLINIK.CS_DOKTER_SCH (TGL_JADWAL,JAM_AWAL,JAM_AKHIR,ID_DOKTER, ID_PENGGANTI, NREMARK, INS_DATE, INS_EMP, FLIMIT, POLI_CD, ID_DOKTER_BPJS ) values ";
                            sql_insert = sql_insert + " ( TO_DATE('" + p_tgl + "','YYYY-MM-DD'), '" + p_awal + "',  '" + p_akhir + "', '" + p_dokter + "','" + p_pengganti + "','" + p_remark + "', sysdate, '" + DB.vUserId + "', '" + p_limit + "', '" + p_poli + "', GET_ID_DOKTER('" + p_dokter + "') ) ";

                            try
                            {
                                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                                OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect);
                                oraConnect.Open();
                                cm.ExecuteNonQuery();
                                oraConnect.Close();
                                cm.Dispose();
                                ssimpan = 2;
                               
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ERROR: " + ex.Message);
                            }
                        }
                    } 
                }
            }
            if(ssimpan ==1)
                MessageBox.Show("Schedule Dokter Berhasil di ubah");
            else if (ssimpan == 2)
                MessageBox.Show("Schedule Dokter Berhasil Dibuat");

            loadData();
        } 
        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveUser.Enabled = true;
            GridView view = sender as GridView;
 
            if (e.Column.Caption == "TGL JADWAL" || e.Column.Caption == "POLI" || e.Column.Caption == "DOKTER" || e.Column.Caption == "JAM AWAL" || e.Column.Caption == "JAM AKHIR" || e.Column.Caption == "LIMIT" || e.Column.Caption == "DOKTER PENGGANTI" || e.Column.Caption == "NREMARK")
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

            if (e.Column.Caption == "TGL JADWAL"  || e.Column.Caption == "POLI" || e.Column.Caption == "DOKTER" || e.Column.Caption == "JAM AWAL" || e.Column.Caption == "JAM AKHIR" || e.Column.Caption == "LIMIT" || e.Column.Caption == "DOKTER PENGGANTI" || e.Column.Caption == "NREMARK")
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
                string sql_delete = "", id = "", p_tgl ="";

                id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[6]).ToString();
                p_tgl  = gridView1.GetRowCellDisplayText(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();

                sql_delete = ""; 
                sql_delete = sql_delete + " update CS_DOKTER_SCH set F_AKTIF = 'N', UPD_DATE = sysdate, UPD_EMP = '" + DB.vUserId + "' ";
                sql_delete = sql_delete + " where ID_DOKTER = '" + id + "' AND TRUNC(TGL_JADWAL) = TO_DATE('" + p_tgl + "','YYYY-MM-DD') ";

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

        public void RunAsyncScheduleBPJS()
        {
            BpjswsResponse resp; int nextna = 0, sstatus = 0; string terror = "";

            Task.Run(() =>
            {
                using (OleDbConnection conn = ConnOra.Create_Connect_Ora())
                {
                    try
                    {

                        OleDbCommand command = new OleDbCommand();
                        OleDbTransaction trans = null;

                        command.Connection = conn;
                        conn.Open();

                        string sql = "SELECT BPJS_KODE_POLI, BPJS_NAMA_POLI, POLI_CD  FROM CS_POLICLINIC where BPJS_KODE_POLI is not null ";
                        DataTable dt_poli = ConnOra.Data_Table_ora(sql);

                        for (int i = 0; i < dt_poli.Rows.Count; i++)
                        {
                            //listPoli.Add(new Poli() { poliCode = dt1.Rows[i]["POLI_CD"].ToString(), poliName = dt1.Rows[i]["POLI_NAME"].ToString() });
                            resp = BpjswsAntrol.GetReferensiDokter(dt_poli.Rows[i]["BPJS_KODE_POLI"].ToString(), today);
                            if (resp.Metadata.Code != 200)
                            {
                                //MessageBox.Show($"Code: { resp.Metadata.Code }, Message: { resp.Metadata.Message }", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                Console.WriteLine($"Data Tidak ada.");
                                sstatus = 2;
                                terror = "Synchronizing GAGAL..!!!";
                                //conn.Close();
                                goto nextn ;
                            }

                            Console.WriteLine($"Before get response ");
                            JObject jsonObj = JObject.Parse(resp?.GetResponseString());
                            Console.WriteLine($"After get response ");
                            JObject response = (JObject)jsonObj["Response"].First;
                            Console.WriteLine($"sukses!Terlewatkan.");
                            try
                            {
                                trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                                string sql2 = " delete KLINIK.CS_DOKTER_SCH where ID_DOKTER_BPJS = '" + response["kodedokter"] + "' and  trunc(TGL_JADWAL) = trunc(to_date( '" + today + "','yyyy-MM-dd'))   ";
                                ORADB.Execute(ORADB.XE, sql2);
                                Console.WriteLine($"Delete sukses.");

                                string query = @" INSERT INTO CS_DOKTER_SCH (TGL_JADWAL, JAM_AWAL, JAM_AKHIR,ID_DOKTER, POLI_CD,  
                                                                    F_AKTIF, FLIMIT, INS_DATE, INS_EMP, ID_DOKTER_BPJS)
                                            VALUES (TO_DATE(?, 'YYYY-MM-DD'), ?, ?, GET_ID_DOKTER(?), GET_POLI_DOKTER(?), 
                                                    ?, ?, sysdate, 'ANTROL BPJS', ?)";

                                using (OleDbCommand cmd = new OleDbCommand(query, conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("?", (string)today);
                                    cmd.Parameters.AddWithValue("?", (string)response["jampraktek"].ToString().Substring(0, 5));
                                    cmd.Parameters.AddWithValue("?", (string)response["jampraktek"].ToString().Substring(6, 5));
                                    cmd.Parameters.AddWithValue("?", (string)response["kodedokter"]);
                                    cmd.Parameters.AddWithValue("?", (string)response["kdPoli"]);
                                    cmd.Parameters.AddWithValue("?", (string)"Y");
                                    cmd.Parameters.AddWithValue("?", (string)response["kapasitas"]);
                                    cmd.Parameters.AddWithValue("?", (string)response["kodedokter"]);

                                    int rowsAffected = cmd.ExecuteNonQuery();
                                    Console.WriteLine($"Insert sukses! {rowsAffected} baris ditambahkan.");
                                }
                                trans.Commit();
                                sstatus = 1;
                                terror = "Synchronizing Schedule Dokter Berhasil.";
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                //Blinking2(terror, 0);
                                Console.WriteLine("Error: " + ex.Message);
                            }
                            nextn:
                            Console.WriteLine("data tidak ada"); 
                        }
                         
                        Console.WriteLine($"Insert sukses!Terlewatkan.");

                        //conn.Close();
                        Console.WriteLine("Insert berhasil!");

                        //if (sstatus == 1)
                        //    Blinking2(terror, 1); 
                           
                    }
                    catch (Exception ex)
                    {
                        //Blinking2(terror, 0);
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    finally
                    {
                        //Blinking2(terror, 0);
                        conn.Close();
                    }
                }
                // Update UI setelah proses selesai
                if (_currentLabel != null && _currentLabel.InvokeRequired)
                {
                    _currentLabel.Invoke(new Action(() =>
                    {
                        if (sstatus == 1)
                            Blinking2(terror, 1);
                        else if (sstatus == 2)
                            Blinking2(terror, 2);
                    }));
                }
                else
                {
                    if (sstatus == 1)
                        Blinking2(terror, 1);
                    else if (sstatus == 2)
                        Blinking2(terror, 2);
                } 
            });
             
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            today = dDateBgn.Text.TrimEnd();
            RunAsyncScheduleBPJS();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            loadPoli();
        }

        private void gridView2_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
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
                    gridControl2.ExportToXls(saveDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("Data tidak ditemukan");
            }
        }

        private void Blinking(LabelControl ctrl, int mbOk)
        {
            //lsMSG = Message;
            lsOK = mbOk;
            //_currentLabel = ctrl;
            timerStart.Interval = 150;
            timerStart.Enabled = true;
            //timer1.Interval = 2000;
            //timer1.Enabled = true;

            timerEnd.Enabled = true;
            timerEnd.Interval = 3000;
            //timer3.Interval = 4000;
            //timer3.Enabled = true;
        }
        private void Blinking2(string ctrl, int mbOk)
        {
            //lsMSG = Message;
            lsOK = mbOk;
            _currentLabel.Text  = ctrl;
            timerStart.Interval = 150;
            timerStart.Enabled = true;
            //timer1.Interval = 2000;
            //timer1.Enabled = true;

            timerEnd.Enabled = true;
            timerEnd.Interval = 3000;
            //timer3.Interval = 4000;
            //timer3.Enabled = true;
        }
        private void timerStart_Tick(object sender, EventArgs e)
        {

            if (lsOK == 2)
            {
                if (bl_klap == true)
                {
                    _currentLabel.ForeColor = Color.Red;
                    _currentLabel.Visible = true;
                    bl_klap = false;
                }
                else
                {
                    bl_klap = true;
                    _currentLabel.Visible = false;
                }
            }
            else
            {
                if (bl_klap == true)
                {
                    _currentLabel.ForeColor = Color.ForestGreen;
                    _currentLabel.Visible = true;
                    bl_klap = false;
                }
                else
                {
                    _currentLabel.Visible = false;
                    bl_klap = true;
                }
            }
        }

        private void timerEnd_Tick(object sender, EventArgs e)
        {
            timerStart.Enabled = false;
            timerEnd.Enabled = false;
            _currentLabel.Visible = false;
        }
    }
}