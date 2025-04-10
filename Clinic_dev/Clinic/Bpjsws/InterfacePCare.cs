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

namespace Clinic.Bpjsws
{
    public partial class InterfacePCare : DevExpress.XtraEditors.XtraForm
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

        public InterfacePCare()
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
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "InfRawatJalan");
            
        }

        private void initData()
        {
            dtGlRole.Clear();

            userStatus.Clear();
            userStatus.Add(new FlagYn() { flagCode = "", flagName = "" });
            userStatus.Add(new FlagYn() { flagCode = "A", flagName = "Aktif" });
            userStatus.Add(new FlagYn() { flagCode = "I", flagName = "Tidak Aktif" });
            
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

            //Sql = "";
            //Sql = Sql + Environment.NewLine + "select 'S' action, ID_JADWAL, TGL_JADWAL, JAM_AWAL, JAM_AKHIR, d.POLI_CD, b.ID_DOKTER, b.NM_DOKTER, b.SPESIALIS, b.NIK_DOKTER, ";
            //Sql = Sql + Environment.NewLine + "       a.ID_PENGGANTI, c.NM_DOKTER PDOKTER, c.SPESIALIS PSPESIALIS, C.NIK_DOKTER, a.nremark,  FLIMIT, NVL(a.UPD_DATE,a.INS_DATE) INS_DATE, NVL(a.UPD_EMP,a.INS_EMP) INS_EMP, A.F_AKTIF ";
            //Sql = Sql + Environment.NewLine + "  from KLINIK.CS_DOKTER_SCH a, ";
            //Sql = Sql + Environment.NewLine + "       KLINIK.CS_DOKTER b, ";
            //Sql = Sql + Environment.NewLine + "       KLINIK.CS_DOKTER c, klinik.CS_POLICLINIC d ";
            //Sql = Sql + Environment.NewLine + " where a.ID_DOKTER  = b.BPJS_ID_DOKTER ";
            //Sql = Sql + Environment.NewLine + "   and a.ID_PENGGANTI = c.ID_DOKTER(+) and a.POLI_CD = d.BPJS_KODE_POLI  ";
            //Sql = Sql + Environment.NewLine + "   and trunc(TGL_JADWAL) = trunc(to_date( '" + dDateBgn.Text.TrimEnd()  + "','yyyy-MM-dd'))   ";
            //Sql = Sql + Environment.NewLine + " order by 3,2,1   ";

            string SQL = "";
            SQL = SQL + Environment.NewLine + "select distinct '0129B010' kd_provider, to_char(visit_date,'dd-mm-yyyy') tanggal, c.RM_NO, b.name, b.insu_no,  ";
            SQL = SQL + Environment.NewLine + "       NID NIK, decode(GENDER,'L','Laki-Laki','Perempuan') GENDER, PHONE, P_AGE usia,  ADDRESS,   ";
            SQL = SQL + Environment.NewLine + "       decode(type_patient,'B','BPJS','A','ASURANSI','UMUM') type_patient,  ";
            SQL = SQL + Environment.NewLine + "       DECODE(purpose,'DOC','Dokter','MID','Bidan','Lain-Lain') purpose,  ";
            SQL = SQL + Environment.NewLine + "       BPJS_KODE_POLI POLI_CD, POLI_NAME  POLI_NAME, d.ANAMNESA,   ";
            SQL = SQL + Environment.NewLine + "       substr(blood_press,0, (instr(blood_press,'/')-1))sistole,substr(blood_press,(instr(blood_press,'/')+1),  length(blood_press)-(instr(blood_press,'/')))diastole, ";
            SQL = SQL + Environment.NewLine + "       d.bb, d.tb,d.vitalrr respRate, d.LING_PERUT lkperut, d.PULSE heartRate, 0 rujuk, 10 kdtkp, a.ID_VISIT, a.QUE01 ANTRIAN_NO  ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_visit a     ";
            SQL = SQL + Environment.NewLine + "join KLINIK.cs_patient_info b on (a.PATIENT_NO=b.PATIENT_NO)     ";
            SQL = SQL + Environment.NewLine + "join KLINIK.cs_patient c on (b.PATIENT_NO=c.PATIENT_NO)   ";
            SQL = SQL + Environment.NewLine + "left join KLINIK.cs_anamnesa d on (c.rm_no=d.rm_no and a.ID_VISIT=d.ID_VISIT )   ";
            SQL = SQL + Environment.NewLine + "left join KLINIK.cs_diagnosa e on (d.ANAMNESA_ID=e.ANAMNESA_ID  ) left join KLINIK.cs_anamnesa_dtl j on (d.ANAMNESA_ID=j.ANAMNESA_ID)   ";
            SQL = SQL + Environment.NewLine + "left join KLINIK.CS_DIAGNOSA_ITEM f on (f.ITEM_CD=e.ITEM_CD )   ";
            SQL = SQL + Environment.NewLine + "left join KLINIK.cs_user g on (e.INS_EMP=g.USER_ID and g.STATUS ='A')    ";
            SQL = SQL + Environment.NewLine + "JOIN KLINIK.CS_CODE_DATA H ON (H.CODE_ID = A.STATUS AND H.CODE_CLASS_ID = 'ST_PASIEN')  ";
            SQL = SQL + Environment.NewLine + "join KLINIK.CS_POLICLINIC i on(i.POLI_CD = a.POLI_CD)   ";
            SQL = SQL + Environment.NewLine + "where 1=1  AND A.PLAN ='TRT01' and  ";
            SQL = SQL + Environment.NewLine + "and trunc(visit_date) between to_date('" + dDateBgn.Text.TrimEnd() + "','yyyy-mm-dd') and to_date('2025-03-19','yyyy-mm-dd')   ";
            SQL = SQL + Environment.NewLine + "order by 1,2,6,5  ";

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
                gridView1.Columns[17].Caption = "Register By";
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
                gridView1.Columns[1].Visible = false;
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

        public void RunAsyncScheduleBPJS()
        {
            BpjswsResponse resp; int nextna = 0; 

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

                        string sql = "SELECT BPJS_KODE_POLI, BPJS_NAMA_POLI FROM CS_POLICLINIC where BPJS_KODE_POLI is not null ";
                        DataTable dt_poli = ConnOra.Data_Table_ora(sql);

                        for (int i = 0; i < dt_poli.Rows.Count; i++)
                        {
                            //listPoli.Add(new Poli() { poliCode = dt1.Rows[i]["POLI_CD"].ToString(), poliName = dt1.Rows[i]["POLI_NAME"].ToString() });
                            resp = BpjswsAntrol.GetReferensiDokter(dt_poli.Rows[i]["BPJS_KODE_POLI"].ToString(), today);
                            if (resp.Metadata.Code != 200)
                            {
                                //MessageBox.Show($"Code: { resp.Metadata.Code }, Message: { resp.Metadata.Message }", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                Console.WriteLine($"Data Tidak ada.");
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

                                string sql2 = " delete KLINIK.CS_DOKTER_SCH where ID_DOKTER = '" + response["kodedokter"] + "' and trunc(TGL_JADWAL) = trunc(sysdate)  ";
                                ORADB.Execute(ORADB.XE, sql2);


                                string query = @" INSERT INTO CS_DOKTER_SCH (TGL_JADWAL, JAM_AWAL, JAM_AKHIR,ID_DOKTER, POLI_CD,  
                                                                    F_AKTIF, FLIMIT, INS_DATE, INS_EMP )
                                            VALUES (TO_DATE(?, 'YYYY-MM-DD'), ?, ?, ?, ?, 
                                                    ?, ?, sysdate, 'ANTROL BPJS')";

                                using (OleDbCommand cmd = new OleDbCommand(query, conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("?", (string)today);
                                    cmd.Parameters.AddWithValue("?", (string)response["jampraktek"].ToString().Substring(0, 5));
                                    cmd.Parameters.AddWithValue("?", (string)response["jampraktek"].ToString().Substring(6, 5));
                                    cmd.Parameters.AddWithValue("?", (string)response["kodedokter"]);
                                    cmd.Parameters.AddWithValue("?", (string)dt_poli.Rows[i]["BPJS_KODE_POLI"].ToString());
                                    cmd.Parameters.AddWithValue("?", (string)"A");
                                    cmd.Parameters.AddWithValue("?", (string)response["kapasitas"]);

                                    int rowsAffected = cmd.ExecuteNonQuery();
                                    Console.WriteLine($"Insert sukses! {rowsAffected} baris ditambahkan.");
                                }
                                trans.Commit();

                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                Console.WriteLine("Error: " + ex.Message);
                            }
                            nextn:
                            Console.WriteLine("data tidak ada"); 
                        }
                         
                        Console.WriteLine($"Insert sukses!Terlewatkan.");

                        //conn.Close();
                        Console.WriteLine("Insert berhasil!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            });
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            RunAsyncScheduleBPJS();
        }
         
    } 
}