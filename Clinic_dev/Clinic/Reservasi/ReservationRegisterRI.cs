using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using Clinic.Properties;
using System.IO;
using System.Media;
using System.Threading;
using System.Web;
using NAudio.Wave;
using System.Net;
using Clinic.Report;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid.Columns;

namespace Clinic
{
    public partial class ReservationRegisterRI : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<PatientType> listPatientType = new List<PatientType>();
        List<Status> listStat = new List<Status>();
        List<Room> listRoom = new List<Room>();
        List<Patient> listPatient = new List<Patient>();
        List<Guarantor> listGuarantor = new List<Guarantor>();
        List<Poli> listPoli = new List<Poli>();
        List<Stat> statIn = new List<Stat>();
        List<Stat> statFrom = new List<Stat>();
        List<Stat> statOut = new List<Stat>();
        List<Stat> statPasien = new List<Stat>();

        DataSet dsAgree = new DataSet();
        DataSet dsKetRanap = new DataSet();

        RepositoryItemDateEdit repositoryItemDateEdit1;
        DateEdit dateedit1;
        RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
        ObsNotif obsNotif = null;
        RsvNotif rsvNotif = null;

        //public string DB.vUserId = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //string today = "2019-11-27";
        string workingDirectory = "", resourcesDirectory = "";
        string upd_col = "", s_policd = "";
        int obst = 0, popup_interval = 999900;

        public ReservationRegisterRI()
        {
            InitializeComponent();
            foreach (GridColumn column in gridView1.Columns)
            {
                if (Type.GetTypeCode(column.ColumnType) == TypeCode.DateTime)
                {
                    column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    column.DisplayFormat.FormatString = @"yyyy-MM-dd"; // @"dd\/MM\/yyyy";
                }
            }
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US", true);
            cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
        }

        private void ReservationInput_Load(object sender, EventArgs e)
        {
            btnSaveInfo.Enabled = false;
            btnAddAnam.Enabled = false;
            //workingDirectory = Environment.CurrentDirectory;
            //resourcesDirectory = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\";
            initData();
            LoadData();
            //tableLayoutPanel1.RowStyles[4] = new RowStyle(SizeType.Absolute, 0);
        }

        private void initData()
        {
            string SQL = "";
            SQL = "";
            SQL = SQL + Environment.NewLine + "select bed_id, room_name || substr(bed_id,-3) room_name, decode(b.use_yn,'N','1','0') qty ";
            SQL = SQL + Environment.NewLine + "from cs_room a ";
            SQL = SQL + Environment.NewLine + "join cs_bed b on (a.room_id=b.room_id) ";
            SQL = SQL + Environment.NewLine + "join cs_room_class c on (a.class_id=c.class_id) ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            //SQL = SQL + Environment.NewLine + "and c.class_id=3 ";
            //SQL = SQL + Environment.NewLine + "and b.use_yn='N' ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            listRoom.Clear();
            listRoom.Add(new Room() { roomCode = "", roomName = "Pilih" });
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listRoom.Add(new Room() { roomCode = dt.Rows[i]["bed_id"].ToString(), roomName = dt.Rows[i]["room_name"].ToString(), roomQty = dt.Rows[i]["qty"].ToString() });
                //poli.poliCode = dt2.Rows[i]["poli_cd"].ToString();
                //poli.poliName = dt2.Rows[i]["poli_name"].ToString();
                //listPoli.Add(poli);
            }

            string SQL2 = "";
            SQL2 = "";
            SQL2 = SQL2 + Environment.NewLine + "select patient_no, name from cs_patient_info where STATUS ='A'";

            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(SQL2, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);
            listPatient.Clear();
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                listPatient.Add(new Patient() { patientCode = dt2.Rows[i]["patient_no"].ToString(), patientName = dt2.Rows[i]["name"].ToString() });

            }

            //string sql_poli = " select POLI_PIC poli_cd, poli_name from cs_policlinic where status = 'A' and poli_cd in ('POL0002', 'POL0004') ";
            //DataTable dt3 = ConnOra.Data_Table_ora(sql_poli); 
            //listPoli.Clear();
            //for (int i = 0; i < dt3.Rows.Count; i++)
            //{
            //    listPoli.Add(new Poli() { poliCode = dt3.Rows[i]["poli_cd"].ToString(), poliName = dt3.Rows[i]["poli_name"].ToString() }); 
            //}

            listPoli.Clear();
            listPoli.Add(new Poli() { poliCode = "DOC", poliName = "Umum" });
            listPoli.Add(new Poli() { poliCode = "MID", poliName = "Kebidanan" });

            listPatientType.Clear();
            listPatientType.Add(new PatientType() { patientTypeCode = "B", patientTypeName = "BPJS" });
            listPatientType.Add(new PatientType() { patientTypeCode = "U", patientTypeName = "Umum" });
            listPatientType.Add(new PatientType() { patientTypeCode = "A", patientTypeName = "Asuransi" });

            listStat.Clear();
            listStat.Add(new Status() { statusCode = "REG", statusName = "Registrasi" });
            listStat.Add(new Status() { statusCode = "OPN", statusName = "Proses" });
            listStat.Add(new Status() { statusCode = "CLS", statusName = "Selesai" });
            listStat.Add(new Status() { statusCode = "CAN", statusName = "Batal" });

            statIn.Clear();
            statIn.Add(new Stat() { statCode = "RSB", statName = "RS/RB" });
            statIn.Add(new Stat() { statCode = "DKT", statName = "Dokter" });
            statIn.Add(new Stat() { statCode = "DSN", statName = "Sendiri" });
            statIn.Add(new Stat() { statCode = "PMD", statName = "Paramedis" });
            statIn.Add(new Stat() { statCode = "POL", statName = "Poisi" });

            statFrom.Clear();
            statFrom.Add(new Stat() { statCode = "BDN", statName = "Bidan Desa" });
            statFrom.Add(new Stat() { statCode = "DKT", statName = "Dokter Praktek" });
            statFrom.Add(new Stat() { statCode = "PLS", statName = "Kasus Polisi" });

            statOut.Clear();
            statOut.Add(new Stat() { statCode = "STJ", statName = "Persetujuan" });
            statOut.Add(new Stat() { statCode = "PLG", statName = "Pulang Paksa" });
            statOut.Add(new Stat() { statCode = "OUT", statName = "Melarikan Diri" });
            statOut.Add(new Stat() { statCode = "RJK", statName = "Dirujuk" });

            statPasien.Clear();
            statPasien.Add(new Stat() { statCode = "SMB", statName = "Sembuh" });
            statPasien.Add(new Stat() { statCode = "PRB", statName = "Perbaikan" });
            statPasien.Add(new Stat() { statCode = "SKT", statName = "Tidak Sembuh" });
            statPasien.Add(new Stat() { statCode = "M01", statName = "Meninggal < 24 Jam" });
            statPasien.Add(new Stat() { statCode = "M02", statName = "Meninggal > 24 Jam" });


            string SQL2a = "";
            SQL2a = "";
            SQL2a = SQL2a + Environment.NewLine + "select gr_no, name from cs_guarantor  ";

            OleDbConnection sqlConnect2a = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2a = new OleDbDataAdapter(SQL2a, sqlConnect2a);
            DataTable dt2a = new DataTable();
            adSql2a.Fill(dt2a);
            listGuarantor.Clear();
            for (int i = 0; i < dt2a.Rows.Count; i++)
            {
                listGuarantor.Add(new Guarantor() { guarantorCode = dt2a.Rows[i]["gr_no"].ToString(), guarantorName = dt2a.Rows[i]["name"].ToString() });
            }
        }

        private void LoadData()
        {
            string sql_search; 

            sql_search = " ";

            sql_search = sql_search + Environment.NewLine + "select 'S' action, a.inpatient_id, b.patient_no, b.que01, a.rm_no, ";
            sql_search = sql_search + Environment.NewLine + " to_char(B.visit_date,'yyyy-MM-dd') visit_date, ";
            sql_search = sql_search + Environment.NewLine + "b.patient_no pasno, a.gr_no, b.type_patient, a.status, a.room_id, ";
            sql_search = sql_search + Environment.NewLine + "to_char(a.date_in , 'YYYY-MM-DD HH24:mm:ss') date_in, ";
            sql_search = sql_search + Environment.NewLine + "a.date_out date_out, a.gr_no grno, a.room_id room_tmp, letter_no, B.ID_VISIT , b.purpose Policlinic";
            sql_search = sql_search + Environment.NewLine + "from cs_inpatient a ";
            sql_search = sql_search + Environment.NewLine + "join cs_visit b on (a.inpatient_id=b.inpatient_id) ";
            sql_search = sql_search + Environment.NewLine + "where 1=1 ";
            sql_search = sql_search + Environment.NewLine + "and a.status in ('REG','OPN') and b.plan = 'TRT02' ";
            sql_search = sql_search + Environment.NewLine + "order by b.visit_date ";


            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_search, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();

                //repositoryItemDateEdit1 = new RepositoryItemDateEdit();
                //repositoryItemDateEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                //repositoryItemDateEdit1.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";
                //repositoryItemDateEdit1.Mask.UseMaskAsDisplayFormat = true;

                ////dateedit1 = new DateEdit();
                ////dateedit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                ////dateedit1.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";
                ////dateedit1.Mask.UseMaskAsDisplayFormat = true;

                //gridControl1.RepositoryItems.AddRange(new RepositoryItem[] { repositoryItemDateEdit1 }); 

                gridControl1.DataSource = dt;

                simpleButton2.Enabled = false;

                //gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 30;
                //gridView1.OptionsBehavior.Editable = false;
                gridView1.BestFitColumns();
                //gridView1.OptionsSelection.MultiSelect = true;
                //gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView1.VisibleColumns[0].Width = 20;
                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[1].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[2].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[3].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[4].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                //gridView1.Columns[5].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                gridView1.Columns[0].OptionsColumn.AllowEdit = false;
                gridView1.Columns[1].OptionsColumn.AllowEdit = false;
                gridView1.Columns[2].OptionsColumn.AllowEdit = false;
                gridView1.Columns[3].OptionsColumn.AllowEdit = false;
                gridView1.Columns[4].OptionsColumn.AllowEdit = false;
                gridView1.Columns[5].OptionsColumn.AllowEdit = false;

                gridView1.Columns[0].Caption = "Action";
                gridView1.Columns[1].Caption = "ID";
                gridView1.Columns[2].Caption = "Pasien No";
                gridView1.Columns[3].Caption = "No Antrian"; 
                gridView1.Columns[4].Caption = "RM No";
                gridView1.Columns[5].Caption = "Tanggal";
                gridView1.Columns[6].Caption = "Pasien";
                gridView1.Columns[7].Caption = "Penjamin";
                gridView1.Columns[8].Caption = "Tipe Pasien";
                gridView1.Columns[9].Caption = "Status";
                gridView1.Columns[10].Caption = "Ruangan";
                gridView1.Columns[11].Caption = "Tgl Masuk";
                gridView1.Columns[12].Caption = "Tgl Keluar";
                gridView1.Columns[13].Caption = "GR";
                gridView1.Columns[14].Caption = "Room";
                gridView1.Columns[15].Caption = "No Surat";
                gridView1.Columns[16].Caption = "ID_Visit";
                gridView1.Columns[17].Caption = "Policlinic";


                gridView1.Columns[5].Width = 70;
                gridView1.Columns[6].Width = 120;
                gridView1.Columns[7].Width = 120;
                gridView1.Columns[8].Width = 70;
                gridView1.Columns[9].Width = 80;
                gridView1.Columns[10].Width = 80;
                gridView1.Columns[11].Width = 80;
                gridView1.Columns[12].Width = 80;

                //gridView1.Columns[17].VisibleIndex = 6;

                //PRE, RSV, NUR, INS, OBS, MED, CLS, CAN

                RepositoryItemGridLookUpEdit glPatient= new RepositoryItemGridLookUpEdit();
                glPatient.DataSource = listPatient;
                glPatient.ValueMember = "patientCode";
                glPatient.DisplayMember = "patientName";

                glPatient.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glPatient.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                glPatient.ImmediatePopup = true;
                glPatient.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glPatient.NullText = "";
                gridView1.Columns[6].ColumnEdit = glPatient;

                
                RepositoryItemGridLookUpEdit glGua = new RepositoryItemGridLookUpEdit();
                glGua.DataSource = listGuarantor;
                glGua.ValueMember = "guarantorCode";
                glGua.DisplayMember = "guarantorName";

                glGua.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glGua.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                glGua.ImmediatePopup = true;
                glGua.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glGua.NullText = "";
                gridView1.Columns[7].ColumnEdit = glGua;


                RepositoryItemLookUpEdit patientLookup = new RepositoryItemLookUpEdit();
                patientLookup.DataSource = listPatientType;
                patientLookup.ValueMember = "patientTypeCode";
                patientLookup.DisplayMember = "patientTypeName";

                patientLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                patientLookup.DropDownRows = listPatientType.Count;
                patientLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                patientLookup.AutoSearchColumnIndex = 1;
                patientLookup.NullText = "";
                gridView1.Columns[8].ColumnEdit = patientLookup;

                RepositoryItemLookUpEdit stLookup = new RepositoryItemLookUpEdit();
                stLookup.DataSource = listStat;
                stLookup.ValueMember = "statusCode";
                stLookup.DisplayMember = "statusName";

                stLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                stLookup.DropDownRows = listStat.Count;
                stLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                stLookup.AutoSearchColumnIndex = 1;
                stLookup.NullText = "";
                gridView1.Columns[9].ColumnEdit = stLookup;

                RepositoryItemLookUpEdit roomLookup = new RepositoryItemLookUpEdit();
                roomLookup.DataSource = listRoom;
                roomLookup.ValueMember = "roomCode";
                roomLookup.DisplayMember = "roomName";

                roomLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                roomLookup.DropDownRows = listRoom.Count;
                roomLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                roomLookup.AutoSearchColumnIndex = 1;
                roomLookup.NullText = "";
                gridView1.Columns[10].ColumnEdit = roomLookup;

                RepositoryItemLookUpEdit poliLookup = new RepositoryItemLookUpEdit();
                poliLookup.DataSource = listPoli;
                poliLookup.ValueMember = "poliCode";
                poliLookup.DisplayMember = "poliName";

                poliLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                poliLookup.DropDownRows = listPoli.Count;
                poliLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                poliLookup.AutoSearchColumnIndex = 1;
                poliLookup.NullText = "";
                gridView1.Columns[17].ColumnEdit = poliLookup;

                //repositoryItemDateEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                //repositoryItemDateEdit1.Mask.EditMask = "YYYY-MM-DD";
                //repositoryItemDateEdit1.Mask.UseMaskAsDisplayFormat = true;


                //repositoryItemDateEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                //repositoryItemDateEdit1.Mask.EditMask = "YYYY-MM-DD HH:MI:SS";
                //repositoryItemDateEdit1.Mask.UseMaskAsDisplayFormat = true;

                //gridView1.Columns[11].ColumnEdit = repositoryItemDateEdit1;
                //gridView1.Columns[12].ColumnEdit = repositoryItemDateEdit1;

                ConnOra.LongTanggal(gridView1, 11);
                ConnOra.LongTanggal(gridView1, 12);
                //repositoryItemDateEdit1.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.True;
                //repositoryItemDateEdit1.CalendarTimeProperties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                //repositoryItemDateEdit1.CalendarTimeProperties.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";
                //repositoryItemDateEdit1.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat = true;

                //repositoryItemDateEdit1 = new RepositoryItemDateEdit();
                //repositoryItemDateEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                //repositoryItemDateEdit1.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";
                //repositoryItemDateEdit1.Mask.UseMaskAsDisplayFormat = true;
                //RepositoryItemDateEdit repoDate1 = new RepositoryItemDateEdit();
                //repoDate1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                //repoDate1.Mask.EditMask = "yyyy-MM-dd HH:mm:ss tt";
                //repoDate1.Mask.UseMaskAsDisplayFormat = true;

                ////gridControl1.RepositoryItems.AddRange(new RepositoryItem[] { repoDate });
                ////gridControl1.DataSource = GetDataSource();

                //gridView1.Columns[11].ColumnEdit = repoDate1;

                //RepositoryItemButtonEdit riButtonEdit = new RepositoryItemButtonEdit();
                //gridControl1.RepositoryItems.Add(riButtonEdit);
                //gridView1.Columns[17].ColumnEdit = riButtonEdit;

                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].Visible = false;
                //gridView1.Columns[2].Visible = false;
                gridView1.Columns[3].Visible = false;
                gridView1.Columns[4].Visible = false;
                gridView1.Columns[12].Visible = false;
                gridView1.Columns[13].Visible = false;
                gridView1.Columns[14].Visible = false;
                gridView1.Columns[15].Visible = false;
                gridView1.Columns[16].Visible = false;

                gridView1.BestFitColumns();
                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            
        }


        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            initData();
            LoadData();
            gridControl2.DataSource = null;
            gridControl3.DataSource = null;
            gridControl4.DataSource = null;
            gridControl5.DataSource = null;
            btnAddAnam.Enabled = false;
            btnSaveInfo.Enabled = false;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string fname = ".wav", p_que="", p1="", p2 = "", p3 = "", p4 = "", p_dir="", s_gender="", s_name="", urltts="", teks="";

            //p_dir = resourcesDirectory;
            p_dir = "C:\\Clinic\\";

            p_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            s_gender = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
            s_name = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();

            p1 = p_que.Substring(0, 1);
            p2 = p_que.Substring(1, 1);
            p3 = p_que.Substring(2, 1);
            p4 = p_que.Substring(3, 1);

            if (s_gender == "Perempuan")
            {
                p1 = "Ibu";
            }
            else
            {
                p1 = "Bapak";
            }

            p2 = s_name;

            teks = p1 + p2 + " silahkan menuju ke konter perawat";

            loading.ShowWaitForm();
            try
            {
                urltts = string.Format("https://translate.googleapis.com/translate_tts?ie=UTF-8&q={0}&tl={1}&total=1&idx=0&textlen={2}&client=gtx", HttpUtility.UrlEncode(teks, Encoding.GetEncoding("utf-8")), "id" + "-gb&q=", teks.Length);
                PlayMp3FromUrl(urltts);

                //SoundPlayer player = new SoundPlayer(p_dir + "antrian" + fname);
                //SoundPlayer player2 = new SoundPlayer(p_dir + p1 + fname);
                //SoundPlayer player3 = new SoundPlayer(p_dir + "_" + p2 + fname);
                //SoundPlayer player4 = new SoundPlayer(p_dir + "_" + p3 + fname);
                //SoundPlayer player5 = new SoundPlayer(p_dir + "_" + p4 + fname);
                //SoundPlayer player6 = new SoundPlayer(p_dir + "IN" + fname);
                //player.PlaySync();
                ////Thread.Sleep(2000);
                //player2.PlaySync();
                ////Thread.Sleep(900);
                //player3.PlaySync();
                ////Thread.Sleep(900);
                //player4.PlaySync();
                ////Thread.Sleep(900);
                //player5.PlaySync();
                //Thread.Sleep(900);
                //player6.PlaySync();
                //Thread.Sleep(2000);

                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            
        }

        public static void PlayMp3FromUrl(string url)
        {
            using (Stream ms = new MemoryStream())
            {
                using (Stream stream = WebRequest.Create(url)
                    .GetResponse().GetResponseStream())
                {
                    byte[] buffer = new byte[32768];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                }

                ms.Position = 0;
                using (WaveStream blockAlignedStream =
                    new BlockAlignReductionStream(
                        WaveFormatConversionStream.CreatePcmStream(
                            new Mp3FileReader(ms))))
                {
                    using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        waveOut.Init(blockAlignedStream);
                        waveOut.Play();
                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
            }
        }
        

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //GridView view = sender as GridView;
            //if (e.Column.Caption != "Berobat") return;
            //// Fill a cell's background if its value is greater than 30. 
            //if (e.CellValue.ToString() == "MID")
            //{
            //    e.Appearance.BackColor = Color.FromArgb(40, Color.LightCoral);
            //}
            //else
            //{
            //    e.Appearance.BackColor = Color.FromArgb(40, Color.DodgerBlue);
            //}
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string pur = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);
            //    string stat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
            //    if (stat == "Inspection" && pur == "Dokter")
            //    {
            //        //e.Appearance.BackColor = Color.FromArgb(40, Color.DodgerBlue);
            //        e.Appearance.BackColor = Color.DodgerBlue;
            //        //e.Appearance.BackColor2 = Color.White;
            //        e.Appearance.ForeColor = Color.White;
            //        //e.Appearance.Font = new Font("Arial", 9, FontStyle.Bold);
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //        e.HighPriority = true;
            //    }

            //    if (stat == "Inspection" && pur == "Bidan")
            //    {
            //        e.Appearance.BackColor = Color.LightCoral;
            //        //e.Appearance.BackColor2 = Color.White;
            //        e.Appearance.ForeColor = Color.White;
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //        e.HighPriority = true;
            //    }
            //}
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.Column.Caption == "Tipe Pasien")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);
                if (kk == "BPJS")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.ForestGreen);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.ForestGreen);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else
                {
                    e.Appearance.BackColor = Color.OldLace;
                    e.Appearance.ForeColor = Color.Black;
                }
            }

            if (e.Column.Caption == "W.T.")
            {
                string wt = View.GetRowCellDisplayText(e.RowHandle, View.Columns[17]);

                if (wt != "")
                {
                    if (Convert.ToInt16(wt) >= 60)
                    {
                        e.Appearance.BackColor = Color.Red;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (Convert.ToInt16(wt) >= 40 && Convert.ToInt16(wt) < 60)
                    {
                        e.Appearance.BackColor = Color.Orange;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else
                    {
                        //e.Appearance.BackColor = Color.OldLace;
                        //e.Appearance.ForeColor = Color.Black;
                    }
                }
                
            }

            if (e.Column.Caption == "Status")
            {
                string pur = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);

                if (pur == "Proses")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.BackColor2 = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (pur == "Selesai")
                {
                    e.Appearance.BackColor = Color.MediumPurple;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (pur == "Batal")
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else
                {
                    e.Appearance.BackColor = Color.OldLace;
                    e.Appearance.ForeColor = Color.Black;
                }
            }


            if (e.Column.Caption == "Tanggal" || e.Column.Caption == "Pasien" || e.Column.Caption == "Penjamin" || e.Column.Caption == "Ruangan" || e.Column.Caption == "Tgl Masuk" || e.Column.Caption == "Tgl Keluar" || e.Column.Caption == "No Surat" || e.Column.Caption == "Policlinic")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
                //e.Appearance.FontStyleDelta = FontStyle.Bold;
            }

        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.Column.Caption == "Nama" || e.Column.Caption == "ID" || e.Column.Caption == "Action")
            {
                
            }
            else
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void gridView3a_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.Column.Caption == "Nama" || e.Column.Caption == "Tanggal" || e.Column.Caption == "Antrian" || e.Column.Caption == "Action")
            {
                
            }
            else
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void gridView1_EditFormPrepared(object sender, EditFormPreparedEventArgs e)
        {

        }

        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            GridView view = sender as GridView;

            //string v1 = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
            //string p_que = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();
            //string p_empid = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
            //string p_date = today;
            //string p_poli = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
            //string p_pasient = view.GetRowCellValue(e.RowHandle, view.Columns[7]).ToString();
            //string p_workA = view.GetRowCellValue(e.RowHandle, view.Columns[8]).ToString();
            //string p_purpose = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();
            //string p_stat = view.GetRowCellValue(e.RowHandle, view.Columns[10]).ToString();
            //MessageBox.Show("Params: " + p_poli + ", " + p_pasient + ", " + p_workA + ", " + p_purpose + ", " + p_stat + ", " + p_que + ", " + p_empid + ", " + p_date);

            //string sql_update;

            //sql_update = " update cs_visit " +
            //             " set poli_cd = '" + p_poli + "', type_patient = '" + p_pasient + "', " +
            //             " work_accident = '" + p_workA + "', purpose = '" + p_purpose + "', status = '" + p_stat + "' " +
            //             " where que01 = '" + p_que  + "' and empid = '" + p_empid + "' and to_char(visit_date,'yyyy-mm-dd') = '" + p_date + "' ";
            try
            {
                //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                //OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                //oraConnect.Open();
                //cm.ExecuteNonQuery();
                //oraConnect.Close();
                //cm.Dispose();

                //MessageBox.Show("Query Exec : " + sql_update);
                
                //MessageBox.Show("Update Success");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView1.Columns[1].OptionsColumn.ReadOnly = false;
            //gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gridView1.Columns[1].OptionsColumn.AllowEdit = true;
            gridView1.Columns[5].OptionsColumn.AllowEdit = true;
            gridView1.AddNewRow();
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[5], DateTime.Now.ToString("yyyy-MM-dd"));
            view.SetRowCellValue(e.RowHandle, view.Columns[9], "REG");
            view.SetRowCellValue(e.RowHandle, view.Columns[11], DateTime.Now.AddHours(0).ToString("yyyy-MM-dd HH:mm:ss"));
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            
            if (e.Column.Caption == "Pasien")
            {
                string p_empid = e.Value.ToString();
                string pasno = "", rm = "";
                string SQL = "";

                SQL = "";
                SQL = SQL + Environment.NewLine + "select a.patient_no, rm_no  ";
                SQL = SQL + Environment.NewLine + "from cs_guarantor  a ";
                SQL = SQL + Environment.NewLine + "join cs_patient b on (a.patient_no=b.patient_no) ";
                SQL = SQL + Environment.NewLine + "where a.patient_no='" + p_empid + "'  ";
                SQL = SQL + Environment.NewLine + "and a.status='A' ";
                SQL = SQL + Environment.NewLine + "and b.status='A' ";


                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    pasno = dt.Rows[0]["patient_no"].ToString();
                    rm = dt.Rows[0]["rm_no"].ToString();

                    string SQL2 = "";
                    SQL2 = "";
                    SQL2 = SQL2 + Environment.NewLine + "select gr_no, name from cs_guarantor where patient_no = '" + p_empid + "' ";

                    OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSql2 = new OleDbDataAdapter(SQL2, sqlConnect2);
                    DataTable dt2 = new DataTable();
                    adSql2.Fill(dt2);
                    listGuarantor.Clear();
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        listGuarantor.Add(new Guarantor() { guarantorCode = dt2.Rows[i]["gr_no"].ToString(), guarantorName = dt2.Rows[i]["name"].ToString() });
                    }
                }
                else
                {
                    pasno = ""; rm = "";
                    //view.SetColumnError(gridView1.Columns[2], "Pasien belum terdaftar");
                    listGuarantor.Clear();
                    MessageBox.Show("Data Penjamin tidak ditemukan");
                }

                view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
                view.SetRowCellValue(e.RowHandle, view.Columns[2], pasno);
                view.SetRowCellValue(e.RowHandle, view.Columns[4], rm);
                view.SetRowCellValue(e.RowHandle, view.Columns[7], "");
            }

            if (e.Column.Caption == "Ruangan")
            {
                string tmp_room = view.GetRowCellValue(e.RowHandle, view.Columns[10]).ToString();
                string SQL = "", stat_room = "";

                SQL = "";
                SQL = SQL + Environment.NewLine + "select use_yn from cs_bed where bed_id='" + tmp_room + "' ";


                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    stat_room = dt.Rows[0]["use_yn"].ToString();
                }

                if (stat_room == "Y")
                {
                    MessageBox.Show("Ruangan sudah dipakai.");
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "");
                }
            }
            

            if (e.Column.Caption == "Tipe Pasien" || e.Column.Caption == "Status" || e.Column.Caption == "Penjamin" || e.Column.Caption == "Ruangan" || e.Column.Caption == "Tgl Masuk" || e.Column.Caption == "Tgl Keluar" || e.Column.Caption == "No Surat" || e.Column.Caption == "Policlinic")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
                    simpleButton2.Enabled = true;
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[0], "U");
                    simpleButton2.Enabled = true;
                }
            }
        }

        private void btnAddAnam_Click(object sender, EventArgs e)
        {
            gridView2.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView2.AddNewRow();
            btnAddAnam.Enabled = false;
            btnSaveInfo.Enabled = true;
        }

        private void gridView2_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            string tmp_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            string tmp_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();
            string tmp_nm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
            view.SetRowCellValue(e.RowHandle, view.Columns[0], today);
            view.SetRowCellValue(e.RowHandle, view.Columns[1], tmp_nm);
            view.SetRowCellValue(e.RowHandle, view.Columns[10], tmp_rm);
            view.SetRowCellValue(e.RowHandle, view.Columns[2], tmp_que);
            view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
            gridView2.Columns[0].OptionsColumn.ReadOnly = true;
            gridView2.Columns[1].OptionsColumn.ReadOnly = true;
            gridView2.Columns[10].OptionsColumn.ReadOnly = true;
            gridView2.Columns[2].OptionsColumn.ReadOnly = true;
            gridView2.Columns[9].OptionsColumn.ReadOnly = true;
        }

        

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.Caption == "Tensi" || e.Column.Caption == "Nadi" || e.Column.Caption == "Suhu" || e.Column.Caption == "BB (Kg)" || e.Column.Caption == "TB (Cm)" || e.Column.Caption == "Alergi" || e.Column.Caption == "Keluhan Utama" || e.Column.Caption == "Riwayat")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string que = "", pasno = "", id = "", rm = "", pasien = "", tgl = "", penjamin = "", status = "", action = "", cek = "", tipe = "", ruangan = "", tglin = "", timetglin ="" , tglout = "";
            string sql_check = "", sql_cnt = "", id_visit = "", sql_update = "", c_que = "", tmp_queue = "", visit_cnt = "", purpose = "", room_tmp = "", lett_no="", poli = "", tglin1="";
            int queue = 0, visit=0, tmp_visit_no = 0;
            cek = "";
            

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                action = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                id = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                pasno = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                que = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                rm = gridView1.GetRowCellValue(i, gridView1.Columns[4]).ToString();
                tgl = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
                pasien = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                penjamin = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();
                tipe = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();
                status = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                ruangan = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                id_visit = gridView1.GetRowCellValue(i, gridView1.Columns[16]).ToString();
                purpose = gridView1.GetRowCellValue(i, gridView1.Columns[17]).ToString();
                tglin1 = gridView1.GetRowCellDisplayText(i, gridView1.Columns[11]).ToString();
                poli = "POL0004";
                object tgli = gridView1.GetRowCellValue(i, "DATE_IN");
                if (tgli != null && tgli is DateTime)
                {
                    DateTime selectedDateTime = (DateTime)tgli;
                    tglin = selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    timetglin = selectedDateTime.ToString("HH:mm:ss");
                }
                else
                {
                    tglin = tgli.ToString();
                    timetglin = tglin.Substring(11, 8).ToString();
                }

                if (tglin1 != null && tglin1 is DateTime)
                {
                    DateTime selectedDateTime = (DateTime)tgli;
                    tglin = selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    timetglin = selectedDateTime.ToString("HH:mm:ss");
                }
                else
                {
                    tglin = tglin1.ToString();
                    timetglin = tglin.Substring(11, 8).ToString();
                }

                object tglo = gridView1.GetRowCellValue(i, "DATE_OUT");
                if (tglo != null && tglo is DateTime)
                {
                    DateTime selectedDateTime = (DateTime)tglo;
                    tglout = selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss"); 
                }
                else
                {
                    tglout = tglo.ToString(); 
                }

                //    tglin = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
                //string timetglin = tglin.Substring(10, 6).ToString();
                //tglout = gridView1.GetRowCellValue(i, gridView1.Columns[12]).ToString();
                //tglout = tglo.ToString();
                room_tmp = gridView1.GetRowCellValue(i, gridView1.Columns[14]).ToString();
                lett_no = gridView1.GetRowCellValue(i, gridView1.Columns[15]).ToString();
                //poli = gridView1.GetRowCellValue(i, gridView1.Columns[17]).ToString();

                if (action == "I")
                {
                    if (pasno == "")
                    {
                        MessageBox.Show("Data pasien tidak ditemukan"); return;
                    }
                    else if (pasien == "")
                    {
                        MessageBox.Show("Data pasien tidak ditemukan"); return;
                    }
                    else if (penjamin == "")
                    {
                        MessageBox.Show("Data penjamin harus diisi"); return;
                    }
                    else if (tipe == "")
                    {
                        MessageBox.Show("Tipe Pasien harus diisi"); return;
                    }
                    else if (status == "CAN")
                    {
                        MessageBox.Show("Status tidak boleh diisi batal"); return;
                    }
                    else if (purpose == "")
                    {
                        MessageBox.Show("Tujuan Policlinic harus di isi"); return;
                    }
                    else
                    {
                        //if(poli.ToString().Equals("POL0004"))
                        //    purpose = "DOC";
                        //else
                        //    purpose = "MID";

                        //poli = "POL0004";

                        c_que = "I";

                        sql_check = " select  nvl(max(to_number(substr(que01,2,3))),0) que from cs_visit where to_char(visit_date,'yyyy-mm-dd')= to_char(sysdate,'yyyy-mm-dd') and purpose = '" + purpose + "' and POLI_CD ='POL0004' ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_check, oraConnect);
                            DataTable dt = new DataTable();
                            adOra.Fill(dt);

                            tmp_queue = dt.Rows[0]["que"].ToString();
                            queue = Convert.ToInt32(tmp_queue) + 1;
                            que = queue.ToString();
                            if (queue < 10)
                            {
                                que = que.PadLeft(que.Length + 2, '0');
                            }
                            else if (queue < 100)
                            {
                                que = que.PadLeft(que.Length + 1, '0');
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }

                        sql_cnt = "";
                        sql_cnt = " select count(patient_no) cnt from cs_visit a, cs_inpatient b where a.inpatient_id=b.inpatient_id and a.patient_no = '" + pasno + "' and a.poli_cd='POL0004' and b.status in ('REG','OPN','PAY')  ";
                        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_cnt, oraConnect2);
                        DataTable dt2 = new DataTable();
                        adOra2.Fill(dt2);
                        visit_cnt = dt2.Rows[0]["cnt"].ToString();
                        if (Convert.ToInt32(visit_cnt) > 0)
                        {
                            MessageBox.Show("Pasien ID  " + pasno + "  sudah terdaftar Rawat Inap.");
                        }
                        else
                        {
                            visit = Convert.ToInt32(visit_cnt) + 1;

                            string sql_seq = "", seq_val = "", sql_tmp = "";
                            sql_seq = " select CS_TREATMENT_HEAD_SEQ.nextval seq from dual ";
                            OleDbConnection oraConnects = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOras = new OleDbDataAdapter(sql_seq, oraConnects);
                            DataTable dts = new DataTable();
                            adOras.Fill(dts);
                            seq_val = dts.Rows[0]["seq"].ToString();

                            string sql_seq2 = "", seq_val2 = "", sql_tmp2 = "", sql_visitno = "";
                            sql_seq2 = " select CS_INPATIENT_SEQ.nextval seq from dual ";
                            OleDbConnection oraConnects2 = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOras2 = new OleDbDataAdapter(sql_seq2, oraConnects2);
                            DataTable dts2 = new DataTable();
                            adOras2.Fill(dts2);
                            seq_val2 = dts2.Rows[0]["seq"].ToString();

                            sql_visitno = " select to_char(sysdate,'yymm') || LPAD(CS_VISIT_SEQ.NEXTVAL, 4, '0') vno from dual ";
                            OleDbConnection oraConnect5 = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOra4 = new OleDbDataAdapter(sql_visitno, oraConnect5);
                            DataTable dt4 = new DataTable();
                            adOra4.Fill(dt4);
                            tmp_visit_no = Convert.ToInt32(dt4.Rows[0]["vno"].ToString());

                            OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                            OleDbCommand command = new OleDbCommand();
                            OleDbTransaction trans = null;

                            command.Connection = oraConnectTrans;
                            oraConnectTrans.Open();

                            //cek = cek + sql_insert;
                            try
                            {
                                trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                                command.Connection = oraConnectTrans;
                                command.Transaction = trans;

                                command.CommandText = " insert into cs_visit (patient_no, visit_date, status, poli_cd, type_patient, purpose, visit_cnt, que01, plan, inpatient_id, ins_date, ins_emp, ID_VISIT) values ('" + pasno + "',to_date('" + tgl + "','YYYY-MM-DD'), 'INP', '" + poli + "', '" + tipe + "', '" + purpose +"', '" + Convert.ToString(visit) + "', '" + c_que + que + "' , 'TRT02', '" + seq_val2 + "', sysdate, '" + DB.vUserId + "', " + tmp_visit_no + ") ";
                                command.ExecuteNonQuery();

                                command.CommandText = " insert into cs_inpatient (inpatient_id, rm_no, gr_no, reg_date, status, room_id, date_in, date_out, letter_no, ins_date, ins_emp) values ('" + seq_val2 + "', '" + rm + "', '" + penjamin + "', to_date('" + tgl + "','YYYY-MM-DD'), '" + status + "', '" + ruangan + "', TO_DATE ( '" + tglin + "', 'YYYY-MM-DD HH24:MI:SS') ,  TO_DATE ( '" + tglout + "',  'YYYY-MM-DD HH24:MI:SS'), '" + lett_no + "', sysdate, '" + DB.vUserId + "') ";
                                command.ExecuteNonQuery();

                                command.CommandText = " insert into cs_anamnesa (anamnesa_id, rm_no, insp_date, visit_no, visit_dt, ins_date, ins_emp, ID_VISIT) values (CS_ANAMNESA_SEQ.nextval, '" + rm + "', to_date('" + tgl + "','yyyy-MM-dd HH24:MI:SS'), '" + c_que + que + "', to_date('" + tgl + "','YYYY-MM-DD'), sysdate, '" + DB.vUserId + "', " + tmp_visit_no + ") ";
                                command.ExecuteNonQuery();

                                command.CommandText = " insert into cs_treatment_head (head_id, rm_no, patient_no, visit_date, visit_no, treat_type_id, status, remarks, pay_status, insu_flag, ins_date, ins_emp, ID_VISIT) values ('" + seq_val + "', '" + rm + "', '" + pasno + "', to_date('" + tgl + "', 'YYYY-MM-DD'), '" + c_que + que + "', 'TRT02', 'OPN', null, 'OPN', '" + tipe + "', sysdate, '" + DB.vUserId + "', " + tmp_visit_no + ") ";
                                command.ExecuteNonQuery();

                                sql_tmp = " ";
                                sql_tmp = sql_tmp + "insert into cs_treatment_detail ";
                                sql_tmp = sql_tmp + "select CS_TREATMENT_DETAIL_SEQ.nextval det_id, " + seq_val + " head_id,  b.treat_item_id, to_date('" + tglin + "','YYYY-MM-DD HH24:MI:SS') visit_date, ";
                                sql_tmp = sql_tmp + "1 treat_qty, 'Initial' remark, sysdate ins_date, '" + DB.vUserId + "' ins_emp, ";
                                sql_tmp = sql_tmp + "null upd_date, null upd_emp, b.treat_item_price,b.treat_item_price ttotal, ";
                                sql_tmp = sql_tmp + " '" + timetglin + "'  TREAT_JAM, 'gridView1RI' gridname, null ID_DOKTER, null att1, null att2 ,'Y'"; 
                                sql_tmp = sql_tmp + "from cs_treatment_type a ";
                                sql_tmp = sql_tmp + "join cs_treatment_item b on (a.treat_type_id=b.treat_type_id) ";
                                sql_tmp = sql_tmp + "join cs_treatment_group c on (b.treat_group_id=c.treat_group_id) ";
                                sql_tmp = sql_tmp + "where 1=1 ";
                                sql_tmp = sql_tmp + "and default_st='Y' ";
                                sql_tmp = sql_tmp + "and a.treat_type_id = 'TRT02' and b.f_status = '" + tipe + "'";


                                command.CommandText = sql_tmp;
                                command.ExecuteNonQuery();

                                if (ruangan != "")
                                {
                                    command.CommandText = " update cs_bed set use_yn = 'Y', upd_date = sysdate, upd_emp = '" + DB.vUserId + "' where bed_id = '" + ruangan + "' ";
                                    command.ExecuteNonQuery();

                                    command.CommandText = " insert into cs_room_his (room_his_id,inpatient_id,room_id,his_date,ins_date,ins_emp) values (CS_ROOM_HIS_SEQ.nextval," + seq_val2 + ",'" + ruangan + "', sysdate, sysdate,'" + DB.vUserId + "') ";
                                    command.ExecuteNonQuery();
                                }
                                else
                                {

                                }
                                
                                trans.Commit();
                                //MessageBox.Show(sql_insert);
                                //MessageBox.Show("Query Exec : " + sql);
                                MessageBox.Show("Data Berhasil disimpan.");
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                MessageBox.Show("ERROR: " + ex.Message);
                            }
                            oraConnectTrans.Close();
                        }
                    }
                }
                else if (action == "U")
                {

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

                        command.CommandText = " update cs_visit set type_patient = '" + tipe + "', POLI_CD = '" + poli + "', upd_date = sysdate, upd_emp = '" + DB.vUserId + "', PURPOSE = '" + purpose + "' where patient_no = '" + pasno + "' and ID_VISIT = " + id_visit +" ";
                        command.ExecuteNonQuery();

                        command.CommandText = " update cs_inpatient set gr_no = '" + penjamin + "', status = '" + status + "', room_id = '" + ruangan + "', date_in = to_date('" + tglin + "','yyyy-mm-dd hh24:mi:ss'), date_out = to_date('" + tglout + "','yyyy-mm-dd hh24:mi:ss'), letter_no='" + lett_no + "', upd_date = sysdate, upd_emp = '" + DB.vUserId + "' where inpatient_id = '" + id + "'  ";
                        command.ExecuteNonQuery();

                        command.CommandText = " update cs_treatment_head set insu_flag = '" + tipe + "', upd_date = sysdate, upd_emp = '" + DB.vUserId + "' where patient_no = '" + pasno + "' and ID_VISIT = " + id_visit + " ";
                        command.ExecuteNonQuery();

                        if (ruangan == "" && room_tmp == "")
                        {

                        }
                        else if (ruangan != "" && room_tmp == "")
                        {
                            command.CommandText = " update cs_bed set use_yn = 'Y', upd_date = sysdate, upd_emp = '" + DB.vUserId + "' where bed_id = '" + ruangan + "' ";
                            command.ExecuteNonQuery();
                        }
                        else if (ruangan == "" && room_tmp != "")
                        {
                            command.CommandText = " update cs_bed set use_yn = 'N', upd_date = sysdate, upd_emp = '" + DB.vUserId + "' where bed_id = '" + room_tmp + "' ";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            //command.CommandText = " update cs_bed set use_yn = 'N', upd_date = sysdate, upd_emp = '" + DB.vUserId + "' where bed_id = '" + room_tmp + "' ";
                            //command.ExecuteNonQuery();

                            command.CommandText = " update cs_bed set use_yn = 'Y', upd_date = sysdate, upd_emp = '" + DB.vUserId + "' where bed_id = '" + ruangan + "' ";
                            command.ExecuteNonQuery();

                            command.CommandText = " insert into cs_room_his (room_his_id,inpatient_id,room_id,his_date,ins_date,ins_emp) values (CS_ROOM_HIS_SEQ.nextval,"+ id + ",'" + ruangan + "', sysdate, sysdate,'" + DB.vUserId + "') ";
                            command.ExecuteNonQuery();
                        }

                        if (status == "CAN")
                        {
                            command.CommandText = " update cs_visit set status = 'CAN', upd_date = sysdate, upd_emp = '" + DB.vUserId + "' where inpatient_id = '" + id + "' ";
                            command.ExecuteNonQuery();

                            command.CommandText = " update cs_treatment_head set status = 'CAN', pay_status = 'CAN', upd_date = sysdate, upd_emp = '" + DB.vUserId + "' where patient_no = '" + pasno + "' and visit_no = '" + que + "' and to_char(visit_date,'yyyy-mm-dd') = '" + tgl + "' ";
                            command.ExecuteNonQuery();

                            command.CommandText = " update cs_bed set use_yn = 'N', upd_date = sysdate, upd_emp = '" + DB.vUserId + "' where bed_id = '" + ruangan + "' ";
                            command.ExecuteNonQuery();
                        }

                        trans.Commit();

                        //MessageBox.Show("Query Exec : " + sql_update);

                        MessageBox.Show("Data Berhasil diupdate");
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                    oraConnectTrans.Close();
                }
            }
            richTextBox1.Text = cek;
            //MessageBox.Show(action);
            initData();
            LoadData();
        }


        private void btnSaveAnam_Click(object sender, EventArgs e)
        {
            string action = "",  id = "", way_in = "", dari = "", remark = "", way_out = "", stat = "";
            string sql_update2 = "", sql_cnt = "", sql_insert = "", sql_update = "";
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                action = gridView2.GetRowCellValue(i, gridView2.Columns[0]).ToString();
                id = gridView2.GetRowCellValue(i, gridView2.Columns[1]).ToString();
                way_in = gridView2.GetRowCellValue(i, gridView2.Columns[3]).ToString();
                dari = gridView2.GetRowCellValue(i, gridView2.Columns[4]).ToString();
                remark = gridView2.GetRowCellValue(i, gridView2.Columns[5]).ToString();
                way_out = gridView2.GetRowCellValue(i, gridView2.Columns[6]).ToString();
                stat = gridView2.GetRowCellValue(i, gridView2.Columns[7]).ToString();

                if (id == "")
                {
                    MessageBox.Show("Data tidak ditemukan");
                }
                else
                {
                    if (action == "I")
                    {

                    }
                    else if (action == "U")
                    {
                        sql_update = "";

                        sql_update = sql_update + " update cs_inpatient set rs_in = '" + way_in + "', came_from = '" + dari + "', came_remark = '" + remark + "', rs_out = '" + way_out + "', patient_stat = '" + stat + "', ";
                        sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                        sql_update = sql_update + " where inpatient_id = '" + id + "'  ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);

                            MessageBox.Show("Data Berhasil diupdate");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }

            }
            LoadData();
        }

        private void btnSaveAnam_Click_1(object sender, EventArgs e)
        {
            string date = "", que = "", tensi = "", nadi = "", suhu = "", alergi = "", keluhan = "", action = "", rm_no = "", nik = "", infok = "", bb = "", tb = "";
            string sql_update2 = "", sql_cnt = "", sql_insert = "", sql_update = "", anam_cnt = "", rw = "";

            for (int i = 0; i < gridView3a.DataRowCount; i++)
            {
                date = gridView3a.GetRowCellValue(i, gridView3a.Columns[0]).ToString();
                rm_no = gridView3a.GetRowCellValue(i, gridView3a.Columns[10]).ToString();
                que = gridView3a.GetRowCellValue(i, gridView3a.Columns[2]).ToString();
                tensi = gridView3a.GetRowCellValue(i, gridView3a.Columns[3]).ToString();
                nadi = gridView3a.GetRowCellValue(i, gridView3a.Columns[4]).ToString();
                suhu = gridView3a.GetRowCellValue(i, gridView3a.Columns[5]).ToString();
                alergi = gridView3a.GetRowCellValue(i, gridView3a.Columns[6]).ToString();
                keluhan = gridView3a.GetRowCellValue(i, gridView3a.Columns[7]).ToString();
                infok = gridView3a.GetRowCellValue(i, gridView3a.Columns[8]).ToString();
                action = gridView3a.GetRowCellValue(i, gridView3a.Columns[9]).ToString();
                nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
                bb = gridView3a.GetRowCellValue(i, gridView3a.Columns[11]).ToString();
                tb = gridView3a.GetRowCellValue(i, gridView3a.Columns[12]).ToString();
                rw = gridView3a.GetRowCellValue(i, gridView3a.Columns[13]).ToString();

                if (tensi == "")
                {
                    MessageBox.Show("Tensi harus diisi");
                }
                else if (nadi == "")
                {
                    MessageBox.Show("Nadi harus diisi");
                }
                else if (bb == "")
                {
                    MessageBox.Show("BB harus diisi");
                }
                else if (tb == "")
                {
                    MessageBox.Show("TB harus diisi");
                }
                else if (keluhan == "")
                {
                    MessageBox.Show("Keluhan harus diisi");
                }
                else
                {
                    if (action == "I")
                    {
                        
                    }
                    else if (action == "U")
                    {
                        sql_update = "";

                        sql_update = sql_update + " update cs_anamnesa" +
                                     " set blood_press = '" + tensi + "', pulse = '" + nadi + "', bb = '" + bb + "', tb = '" + tb + "', " +
                                     " temperature = '" + suhu + "', allergy = '" + alergi + "', anamnesa = '" + keluhan + "', info_k = '" + infok + "',  disease_now = '" + rw + "', ";
                        sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                        sql_update = sql_update + " where rm_no = '" + rm_no + "' and to_char(insp_date,'yyyy-mm-dd') = '" + date + "' and visit_no = '" + que + "' ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);

                            MessageBox.Show("Data Berhasil diupdate");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }

            }
            LoadData();
        }

        private void btnCetak_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string p_pasno = "", p_date="";

            if (gridView1.RowCount > 0)
            {
                p_pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
                p_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();

                SQL = "";
                SQL = SQL + Environment.NewLine + "select c.name, round(((sysdate-c.birth_date)/30)/12) age, c.job, c.address,  ";
                SQL = SQL + Environment.NewLine + "d.name p_name, round(((sysdate-d.birth_date)/30)/12) p_age,  ";
                SQL = SQL + Environment.NewLine + "d.job p_job, d.relation , d.address p_address,  ";
                SQL = SQL + Environment.NewLine + "to_char(visit_date,'fmdd Month yyyy', 'nls_date_language = INDONESIAN') as ddate ";
                SQL = SQL + Environment.NewLine + "from cs_inpatient a  ";
                SQL = SQL + Environment.NewLine + "join cs_visit b on (a.inpatient_id=b.inpatient_id)  ";
                SQL = SQL + Environment.NewLine + "join cs_patient_info c on (b.patient_no=c.patient_no)  ";
                SQL = SQL + Environment.NewLine + "join cs_guarantor d on (a.gr_no=d.gr_no)  ";
                SQL = SQL + Environment.NewLine + "where 1=1 ";
                SQL = SQL + Environment.NewLine + "and a.status not in ('CAN') ";
                SQL = SQL + Environment.NewLine + "and b.patient_no = '" + p_pasno + "'  ";
                SQL = SQL + Environment.NewLine + "and to_char(visit_date,'yyyy-mm-dd') = '" + p_date + "'  ";

                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dsAgree.Tables.Clear();
                    dsAgree.Tables.Add(dt);

                    ReportAgreement report = new ReportAgreement(dsAgree);
                    report.ShowPreviewDialog();
                }
                else
                {

                }

               
            }
        }

        private void btnCetak2_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string p_pasno = "", p_date = "";

            if (gridView1.RowCount > 0)
            {
                p_pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
                p_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();

                SQL = "";
                SQL = SQL + Environment.NewLine + "select c.name, round(((sysdate-c.birth_date)/30)/12) age, c.address,   ";
                SQL = SQL + Environment.NewLine + "to_char(date_in,'fmdd Month yyyy', 'nls_date_language = INDONESIAN') bgndt, ";
                SQL = SQL + Environment.NewLine + "to_char(date_out,'fmdd Month yyyy', 'nls_date_language = INDONESIAN') enddt, ";
                SQL = SQL + Environment.NewLine + "a.letter_no, e.item_name d_name, c.company, c.company_addr, ";
                SQL = SQL + Environment.NewLine + "to_char(sysdate,'fmdd Month yyyy', 'nls_date_language = INDONESIAN') as ddate  ";
                SQL = SQL + Environment.NewLine + "from cs_inpatient a   ";
                SQL = SQL + Environment.NewLine + "join cs_visit b on (a.inpatient_id=b.inpatient_id)   ";
                SQL = SQL + Environment.NewLine + "join cs_patient_info c on (b.patient_no=c.patient_no)   ";
                SQL = SQL + Environment.NewLine + "join cs_diagnosa d on (trunc(b.visit_date)=d.visit_dt and b.que01=d.visit_no)  ";
                SQL = SQL + Environment.NewLine + "join cs_diagnosa_item e on (d.item_cd=e.item_cd) ";
                SQL = SQL + Environment.NewLine + "where 1=1  ";
                SQL = SQL + Environment.NewLine + "and a.status not in ('CAN')  ";
                SQL = SQL + Environment.NewLine + "and b.patient_no = '" + p_pasno + "'  ";
                SQL = SQL + Environment.NewLine + "and to_char(visit_date,'yyyy-mm-dd') = '" + p_date + "'  ";

                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dsKetRanap.Tables.Clear();
                    dsKetRanap.Tables.Add(dt);

                    ReportKetRanap report2 = new ReportKetRanap(dsKetRanap);
                    report2.ShowPreviewDialog();
                }
                else
                {
                    MessageBox.Show("Data diagnosa harus diisi");
                }
                
            }
        }

        private bool CheckOpened(string name)
        {
            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                if (frm.Text == name)
                {
                    return true;
                }
            }
            return false;
        }

        
    }
}
