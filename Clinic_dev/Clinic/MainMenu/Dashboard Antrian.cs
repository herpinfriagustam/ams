using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
namespace Clinic
{
    public partial class DashboardAntrian : DevExpress.XtraEditors.XtraForm
    {
        //private KoneksiOra koneksi;
        private ConnectDb koneksi;
        private MusicPlayer player;
        DataTable dtAntrian = null;
        DataTable dtAntrianNm = null;
        DataTable dtGridHold = null;
        DataTable dtGridAntrian = null;
        DataTable dtVideo = null;

        private int currentVideoIndex;
        public DashboardAntrian()
        {
            InitializeComponent();

            //koneksi = new KoneksiOra();
            koneksi = new ConnectDb();
            player = new MusicPlayer();

            timer.Start();

            loadAntrianNo(); //load SQL Antrian
            isiAntrian("CARD NOMOR"); //CARD NOMOR , "CARD NAMA
            //marquee
            ucMarquee1.StartMarquee();



            CultureInfo culture = new CultureInfo("id-ID");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;

            dtfi.DayNames = new string[] { "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu" };
            dtfi.AbbreviatedDayNames = new string[] { "Min", "Sen", "Sel", "Rab", "Kam", "Jum", "Sab" };


            int aturColumn = grdAntrian.Width;
            grvAntrian.Columns["C_NO"].Width = aturColumn * 10 / 100;
            grvAntrian.Columns["NO_PASIEN"].Width = aturColumn * 20 / 100;
            grvAntrian.Columns["NAMA_PASIEN"].Width = aturColumn * 70 / 100;


            int aturColumnKelewat = grdKelewat.Width;
            grvKelewat.Columns["C_NO"].Width = aturColumnKelewat * 10/100;
            grvKelewat.Columns["NO_PASIEN"].Width = aturColumnKelewat * 20 / 100;
            grvKelewat.Columns["NAMA_PASIEN"].Width = aturColumnKelewat * 30 / 100;

            //video
            WMP.uiMode = "none";
            loadVideo();
            currentVideoIndex = 0;
            PlayNextVideo();
            WMP.PlayStateChange += WMP_PlayStateChange;

        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        bool boolLoadAntrian = true;
        int loadAntrian = 5;
        int TimeLoadAntrian = 0;
        int ListCount = 0;
        int loadGrid = 100;
        int TimeLoadGrid = 50;
        int idxGridList = 0;

        int loadMarquee = 100;
        int TimeLoadMarquee = 0;
        int idxMarquee = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            TimeLoadAntrian++;
            TimeLoadGrid++;
            TimeLoadMarquee++;

            lblTime.Text = DateTime.Now.ToString("HH:mm:ss") + " WIB";

            DateTime now = DateTime.Now;
            string hariIndonesia = now.ToString("dddd", new CultureInfo("id-ID"));
            string tanggal = now.ToString("dd-MM-yyyy");
            lblDate.Text = $"{hariIndonesia} \n {tanggal}";


            try
            {
                //mun aya nu di panggil
                if (boolLoadAntrian == true && TimeLoadAntrian == loadAntrian)
                {
                    ListAntrianNm();

                    if (dtAntrianNm != null && dtAntrianNm.Rows.Count > 0)
                    {
                        timerPanggilAntrian.Start();
                        boolLoadAntrian = false;
                        Timer = 70;
                        ListCount = 0;
                    }

                    TimeLoadAntrian = 0;
                }

                
                //data grid ganti setiap 10 detik
                if (loadGrid == TimeLoadGrid)
                {
                    if (dtAntrian != null && dtAntrian.Rows.Count > 0)
                    {

                        string codeId = dtAntrian.Rows[idxGridList]["CODE_ID"]?.ToString();
                        string PoliName = dtAntrian.Rows[idxGridList]["POLI_NAME"]?.ToString();


                        lblTitleGrid.Text = PoliName;
                        loadGridAntrian(codeId);
                        loadGridHold(codeId);

                        idxGridList++;

                        if (idxGridList == dtAntrian.Rows.Count)
                        {
                            idxGridList = 0;
                        }
                    }
                    TimeLoadGrid = 0;
                }
                

            }
            catch { }
            
        }

        int TimeCall = 0;
        int TimeReset = 100;
        int TimeResetNm = 100;
        int Timer = 0;
        
        private void timerPanggilAntrian_Tick(object sender, EventArgs e)
        {
            TimeCall = 90;
            Timer++;
            TimeReset++;
            TimeResetNm++;

            if (Timer == (TimeCall))
            {

                try
                {
                    if (dtAntrianNm != null && dtAntrianNm.Rows.Count > 0)
                    {
                        string callId = dtAntrianNm.Rows[ListCount]["CALL_ID"]?.ToString();
                        string param = dtAntrianNm.Rows[ListCount]["PARAM"]?.ToString();
                        string typeIns = dtAntrianNm.Rows[ListCount]["TYPE_INS"]?.ToString();

                        player.CallPasien(param);
                        //player.WaitForPlaybackCompletion();

                        updateAntrianFlag(callId);

                        loadAntrianNo(); //load SQL Antrian
                        isiAntrian("CARD NOMOR"); //CARD NOMOR , "CARD NAMA
                        isiAntrianNama(ListCount); // CNomor Antrian Yang ada namanya
                        colorPoliBlue();
                        colorPoliOrange(typeIns);


                        TimeReset = 0;
                        TimeResetNm = 0;
                        ListCount++;
                    }

                    Timer = 0;
                } catch { }
            }

            if (TimeReset == 10 || TimeReset == 20 || TimeReset == 30 )
            {
                isiAntrian("CARD NOMOR"); //CARD NOMOR , CARD NAMA
                isiAntrianNama(ListCount-1); // CNomor Antrian Yang ada namanya
            }

            if (TimeReset == 15 || TimeReset == 25 )
            {
                resetAntrian("CARD NOMOR"); //CARD NOMOR , "CARD NAMA
                resetAntrian("CARD NAMA"); //CARD NOMOR , CARD NAMA
            }

            if (TimeResetNm == 35 || TimeResetNm == 45 || TimeResetNm == 55 || TimeResetNm == 65)
            {
                resetAntrian("CARD NAMA"); //CARD NOMOR , "CARD NAMA
            }
            if (TimeResetNm == 40 || TimeResetNm == 50 || TimeResetNm == 60 || TimeResetNm == 70)
            {
                isiAntrianNama(ListCount - 1); // CNomor Antrian Yang ada namanya
            }

            //STOP JIKA LIST SUDAH DI PANGGIL SEMUA
            if (ListCount == dtAntrianNm.Rows.Count && TimeResetNm == 70)
            {
                boolLoadAntrian = true;
                dtAntrianNm = null;
                TimeLoadAntrian = 0;
                timerPanggilAntrian.Stop();
            }
            

        }

        private void loadAntrianNo()
        {
            try
            {
                dtAntrian = null;
                string sql = " ";

                sql = @" SELECT CODE_ID, 
                                        DECODE (CODE_ID,  'DOC','POLI UMUM',
                                                          'DGI','POLI GIGI',
                                                          'MID','KEBIDANAN',
                                                          'PKB','POLI KB',
                                                          'LAB','LABORATORIUM',
                                                          'MED','FARMASI',
                                                          'PWT','PERAWAT',
                                                          'PAY','KASIR',
                                                          'REG','REGISTRASI') POLI_NAME, 
                                     CASE WHEN NVL (MAX(TO_CHAR(B.INS_DATE,'YYYYMMDDHH24MISS')||QUE), '-') = '-' THEN '-'
                                          ELSE SUBSTR(MAX(TO_CHAR(B.INS_DATE,'YYYYMMDDHH24MISS')||QUE),-4)
                                     END ANTRIAN_NO
                                FROM KLINIK.CS_CODE_DATA A, KLINIK.CS_CALL_LOG B
                               WHERE A.CODE_ID = B.TYPE_INS(+)
                                 AND B.FLAG(+) = 'Y'
                                 AND A.CODE_CLASS_ID = 'CALL_ANTRIAN' 
                                 AND TRUNC(B.INS_DATE(+)) = TRUNC(SYSDATE)
                            GROUP BY CODE_ID,A.SORT_ORDER
                            ORDER BY A.SORT_ORDER";

                dtAntrian = koneksi.Data_Table_ora(sql);
            }
            catch { }

        }
        private void ListAntrianNm()
        {
            try
            {
                dtAntrianNm = null;
                string sql2 = " ";

                sql2 = @"  SELECT A.CALL_ID,
                                         A.QUE,
                                         A.TYPE_INS,
                                         A.PARAM,
                                         C.NAME ANTRIAN_NM
                                    FROM KLINIK.CS_CALL_LOG A, 
                                         KLINIK.cs_visit B, 
                                         KLINIK.CS_PATIENT_INFO C
                                   WHERE A.FLAG  = 'N'  
                                     AND A.QUE     = B.QUE01(+)
                                     AND TRUNC(A.INS_DATE) = TRUNC(VISIT_DATE(+))
                                     AND A.POLI_CD = B.POLI_CD(+)
                                     AND B.PATIENT_NO = C.PATIENT_NO(+)  
                                     AND TRUNC(A.INS_DATE) = TRUNC(SYSDATE)  
                                     AND ROWNUM = 1
                                ORDER BY A.CALL_ID ";

                dtAntrianNm = koneksi.Data_Table_ora(sql2);
            }
            catch
            {
            }

        }
        private void updateAntrianFlag(string call_id)
        {
            try
            {
                string sql = @"UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'Y' WHERE CALL_ID = '" + call_id + "' AND TRUNC(INS_DATE) = TRUNC(SYSDATE)";
                koneksi.ExeNonQuery(sql);
            } catch
            {
            }

        }

        private void loadGridAntrian(string codePoli)
        {
            try
            {
                string sql = "";
                dtGridAntrian = null;

                if (codePoli.ToString().Equals("REG"))
                {
                    sql = @"      SELECT ROWNUM AS C_NO, A.* FROM (
                                            SELECT A.QUE  NO_PASIEN, 
                                               ''      NAMA_PASIEN,
                                               STAT PURPOSE
                                          FROM KLINIK.CS_CALL_LOG A 
                                         WHERE     1 = 1
                                               AND TRUNC(A.INS_DATE) = TRUNC(SYSDATE) 
                                               AND A.TYPE_INS = '" + codePoli + @"' 
                                        ORDER BY INS_DATE
                                       ) A";
                }
                else
                {
                    sql = @"      SELECT ROWNUM AS C_NO, A.* FROM (
                                            SELECT A.QUE01  NO_PASIEN, 
                                               B.NAME       NAMA_PASIEN,
                                               A.PURPOSE
                                          FROM KLINIK.cs_visit A, KLINIK.CS_PATIENT_INFO B
                                         WHERE     1 = 1
                                               AND TRUNC(A.VISIT_DATE) = TRUNC(SYSDATE) and a.STATUS not in('CAN','CLS')
                                               AND A.PURPOSE = '" + codePoli + @"'
                                               AND NOT EXISTS
                                                      (SELECT ''
                                                         FROM KLINIK.CS_CALL_LOG Z
                                                        WHERE     Z.TYPE_INS = A.PURPOSE
                                                              AND Z.QUE = A.QUE01
                                                              AND Z.TYPE_INS = '" + codePoli + @"'
                                                              AND TRUNC(Z.INS_DATE) = TRUNC(SYSDATE))
                                               AND A.PATIENT_NO = B.PATIENT_NO(+)
                                        ORDER BY NO_PASIEN
                                       ) A";
                } 

                dtGridAntrian = koneksi.Data_Table_ora(sql);
                grdAntrian.DataSource = dtGridAntrian;
            }
            catch { }

        }
        private void loadGridHold(string codePoli)
        {
            try
            {
                dtGridHold = null;

                string sql = @"  SELECT ROWNUM AS C_NO, A.* FROM (
                                        SELECT A.QUE01  NO_PASIEN, 
                                               B.NAME   NAMA_PASIEN,
                                               A.PURPOSE
                                          FROM KLINIK.cs_visit A, KLINIK.CS_PATIENT_INFO B
                                         WHERE     1 = 1
                                               AND TRUNC(A.VISIT_DATE) = TRUNC(SYSDATE) and a.STATUS not in('CAN','CLS')
                                               AND A.PURPOSE = '" + codePoli + @"'
                                               AND EXISTS
                                                      (SELECT ''
                                                         FROM KLINIK.CS_CALL_LOG Z
                                                        WHERE     Z.TYPE_INS    = A.PURPOSE
                                                              AND Z.QUE         = A.QUE01
                                                              AND Z.TYPE_INS    = '" + codePoli + @"'
                                                              AND Z.FLAG        = 'H'
                                                              AND TRUNC(Z.INS_DATE) = TRUNC(SYSDATE))
                                               AND A.PATIENT_NO = B.PATIENT_NO(+)
                                        ORDER BY NO_PASIEN
                                   ) A";

                dtGridHold = koneksi.Data_Table_ora(sql);
                grdKelewat.DataSource = dtGridHold;
            }
            catch { }

        }

        private void loadVideo()
        {
            try
            {
                dtVideo = null;

                string sql = @" SELECT NREMARK, FPATH
                                    FROM KLINIK.TABLE_ANTRIAN_FILE
                                   WHERE FFLAG = 'Y' AND NFILE = 'VIDEO'
                                ORDER BY SEQ
                              ";

                dtVideo = koneksi.Data_Table_ora(sql);
            }
            catch { }

        }


        private void isiAntrian(string Antrian)
        {
            try {
                if (Antrian == "CARD NOMOR")
                {
                    if (dtAntrian != null && dtAntrian.Rows.Count > 0)
                    {
                        //Dokter Poli Umum
                        AntrianPoliUmum.PoliText = dtAntrian.Rows[0]["POLI_NAME"]?.ToString();
                        AntrianPoliUmum.AntrianText = dtAntrian.Rows[0]["ANTRIAN_NO"]?.ToString();
                        //Dokter Poli Gigi
                        AntrianPoliGigi.PoliText = dtAntrian.Rows[1]["POLI_NAME"]?.ToString();
                        AntrianPoliGigi.AntrianText = dtAntrian.Rows[1]["ANTRIAN_NO"]?.ToString();
                        //Bidan
                        AntrianKebidanan.PoliText = dtAntrian.Rows[2]["POLI_NAME"]?.ToString();
                        AntrianKebidanan.AntrianText = dtAntrian.Rows[2]["ANTRIAN_NO"]?.ToString();
                        //Bidan KB
                        AntrianPoliKB.PoliText = dtAntrian.Rows[3]["POLI_NAME"]?.ToString();
                        AntrianPoliKB.AntrianText = dtAntrian.Rows[3]["ANTRIAN_NO"]?.ToString();
                        //Laboratorium
                        AntrianLaboratorium.PoliText = dtAntrian.Rows[4]["POLI_NAME"]?.ToString();
                        AntrianLaboratorium.AntrianText = dtAntrian.Rows[4]["ANTRIAN_NO"]?.ToString();
                        //Farmasi
                        AntrianFarmasi.PoliText = dtAntrian.Rows[5]["POLI_NAME"]?.ToString();
                        AntrianFarmasi.AntrianText = dtAntrian.Rows[5]["ANTRIAN_NO"]?.ToString();
                        //Perawat
                        AntrianPerawat.PoliText = dtAntrian.Rows[6]["POLI_NAME"]?.ToString();
                        AntrianPerawat.AntrianText = dtAntrian.Rows[6]["ANTRIAN_NO"]?.ToString();
                        //Kasir
                        AntrianKasir.PoliText = dtAntrian.Rows[7]["POLI_NAME"]?.ToString();
                        AntrianKasir.AntrianText = dtAntrian.Rows[7]["ANTRIAN_NO"]?.ToString();
                        //Pendaftaran
                        AntrianRegistrasi.PoliText = dtAntrian.Rows[8]["POLI_NAME"]?.ToString();
                        AntrianRegistrasi.AntrianText = dtAntrian.Rows[8]["ANTRIAN_NO"]?.ToString();

                    }
                }
            } catch { }
            
            
        }
        private void isiAntrianNama(int indexDatatable)
        {
            try
            {
                if (dtAntrianNm != null && dtAntrianNm.Rows.Count > 0)
                {
                    AntrianNow.AntrianText = dtAntrianNm.Rows[indexDatatable]["QUE"]?.ToString();
                    AntrianNow.PasienText = dtAntrianNm.Rows[indexDatatable]["ANTRIAN_NM"]?.ToString();
                }
            } catch { }
        }

        private void WMP_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8)
            {
                if (dtVideo != null)
                {
                    if (dtVideo.Rows.Count > 0)
                    {
                        currentVideoIndex = (currentVideoIndex + 1) % dtVideo.Rows.Count;
                        PlayNextVideo();
                    }

                }
            }
        }
        private async void PlayNextVideo()
        {
            if (dtVideo != null)
            {
                if (dtVideo.Rows.Count > 0)
                {
                    string nextVideoUrl = dtVideo.Rows[currentVideoIndex]["FPATH"]?.ToString();
                    if (!string.IsNullOrEmpty(nextVideoUrl))
                    {
                        try
                        {
                            WMP.URL = nextVideoUrl;
                            WMP.settings.mute = true;
                            await Task.Delay(30);
                            WMP.Ctlcontrols.play();

                            //WindowsMediaPlayer player = new WindowsMediaPlayer();
                            //player.URL = nextVideoUrl;
                            //player.controls.play();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                        
                    }
                }

            }
            

        }
        
        private void resetAntrian(string Antrian)
        {
            if (Antrian == "CARD NOMOR")
            {
                AntrianPoliUmum.PoliText = "";
                AntrianPoliUmum.AntrianText = "";
                AntrianPoliGigi.PoliText = "";
                AntrianPoliGigi.AntrianText = "";
                AntrianKebidanan.PoliText = "";
                AntrianKebidanan.AntrianText = "";
                AntrianPoliKB.PoliText = "";
                AntrianPoliKB.AntrianText = "";
                AntrianLaboratorium.PoliText = "";
                AntrianLaboratorium.AntrianText = "";
                AntrianFarmasi.PoliText = "";
                AntrianFarmasi.AntrianText = "";
                AntrianPerawat.PoliText = "";
                AntrianPerawat.AntrianText = "";
                AntrianRegistrasi.PoliText = "";
                AntrianRegistrasi.AntrianText = "";
                AntrianKasir.PoliText = "";
                AntrianKasir.AntrianText = "";
            }
            else if(Antrian == "CARD NAMA")
            {
                AntrianNow.AntrianText = "";
                AntrianNow.PasienText = "";
            }
        }
        private void colorPoliBlue()
        {
            BgColor(AntrianPoliUmum);
            BgColor(AntrianPoliGigi);
            BgColor(AntrianKebidanan);
            BgColor(AntrianPoliKB);
            BgColor(AntrianLaboratorium);
            BgColor(AntrianFarmasi);
            BgColor(AntrianPerawat);
            BgColor(AntrianRegistrasi);
            BgColor(AntrianKasir);
            BgColorNow(AntrianNow);
        }
       
        private void BgColor(ControllerAntrian.UcAntrianNo UcAntrian)
        {
            UcAntrian.PoliBgColor = System.Drawing.Color.FromArgb(0, 127, 187);
            UcAntrian.AntrianTextColor = System.Drawing.Color.FromArgb(0, 127, 187);
        }
        private void BgColorNow(ControllerAntrian.UcAntrianNoNm UcAntrian)
        {
            UcAntrian.PoliBgColor = System.Drawing.Color.FromArgb(0, 127, 187);
            UcAntrian.AntrianTextColor = System.Drawing.Color.FromArgb(0, 127, 187);
        }
        private void colorPoliOrange(string poli)
        {
            //OliveDrab
            if (poli == "DOC")
            {
                BgColorTomato(AntrianPoliUmum);
            }
            if (poli == "DGI")
            {
                BgColorTomato(AntrianPoliGigi);
            }
            if (poli == "MID")
            {
                BgColorTomato(AntrianKebidanan);
            }
            if (poli == "PKB")
            {
                BgColorTomato(AntrianPoliKB);
            }
            if (poli == "LAB")
            {
                BgColorTomato(AntrianLaboratorium);
            }
            if (poli == "MED")
            {
                BgColorTomato(AntrianFarmasi);
            }
            if (poli == "PWT")
            {
                BgColorTomato(AntrianPerawat);
            }
            if (poli == "REG")
            {
                BgColorTomato(AntrianRegistrasi);
            }
            if (poli == "PAY")
            {
                BgColorTomato(AntrianKasir);
            }

            BgColorNowTomato(AntrianNow);
        }

        private void BgColorTomato(ControllerAntrian.UcAntrianNo UcAntrian)
        {
            UcAntrian.PoliBgColor = System.Drawing.Color.Tomato;
            UcAntrian.AntrianTextColor = System.Drawing.Color.Tomato;
        }
        private void BgColorNowTomato(ControllerAntrian.UcAntrianNoNm UcAntrian)
        {
            UcAntrian.PoliBgColor = System.Drawing.Color.Tomato;
            UcAntrian.AntrianTextColor = System.Drawing.Color.Tomato;
            UcAntrian.PasienTextColor = System.Drawing.Color.Tomato;
        }

    }

}



/*
255; 222; 115
*/
