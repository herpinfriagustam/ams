using System;
using System.Data;
using System.Drawing;
using System.Globalization;

namespace Clinic
{
    public partial class DashboardAntrianxx : DevExpress.XtraEditors.XtraForm
    {
        private KoneksiOra koneksi;
        private MusicPlayer player;
        DataTable dtAntrian = null;
        DataTable dtAntrianNm = null;
        DataTable dtGridHold = null;
        DataTable dtGridAntrian = null;
        DataTable dtDokter = null;
        public DashboardAntrianxx()
        {
            InitializeComponent();

            koneksi = new KoneksiOra();
            player = new MusicPlayer();

            timer.Start();

            loadAntrianNo(); //load SQL Antrian
            isiAntrian("CARD NOMOR"); //CARD NOMOR , "CARD NAMA
            loadDokter();

            CultureInfo culture = new CultureInfo("id-ID");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;

            dtfi.DayNames = new string[] { "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu" };
            dtfi.AbbreviatedDayNames = new string[] { "Min", "Sen", "Sel", "Rab", "Kam", "Jum", "Sab" };


            int aturColumn = grdAntrian.Width;

            grvAntrian.Columns["C_NO"].Width = aturColumn/4;
            grvAntrian.Columns["NO_PASIEN"].Width = aturColumn/3;
            grvAntrian.Columns["NAMA_PASIEN"].Width = aturColumn - (aturColumn / 4);

            grvKelewat.Columns["C_NO"].Width = aturColumn / 4;
            grvKelewat.Columns["NO_PASIEN"].Width = aturColumn / 3;
            grvKelewat.Columns["NAMA_PASIEN"].Width = aturColumn - (aturColumn / 4);

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
        int loadImgDokter = 200;
        int TimeloadImgDokter = 150;
        int idxImgDokter = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            TimeLoadAntrian++;
            TimeLoadGrid++;
            TimeloadImgDokter++;

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

                //data dokter ganti setelah 20 detik
                if (loadImgDokter == TimeloadImgDokter)
                {
                    if (dtDokter != null && dtDokter.Rows.Count > 0)
                    {
                        Image fotoDokter = dtDokter.Rows[idxImgDokter]["IMAGE"] as Image;

                        ImageDokter.DokterImage = ResizeImage(fotoDokter, ImageDokter.DokterImageWidth, ImageDokter.DokterImageHeight);
                        ImageDokter.DokterNama = dtDokter.Rows[idxImgDokter]["NAMA"]?.ToString();
                        labelPoliDokter.Text = dtDokter.Rows[idxImgDokter]["POLI"]?.ToString();

                        idxImgDokter++;

                        if (idxImgDokter == dtDokter.Rows.Count)
                        {
                            idxImgDokter = 0;
                        }
                    }

                    TimeloadImgDokter = 0;
                }


            } catch { }
            
        }

        int TimeCall = 0;
        int TimeReset = 100;
        int TimeResetNm = 100;
        int Timer = 0;
        
        private void timerPanggilAntrian_Tick(object sender, EventArgs e)
        {
            TimeCall = 80;
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

                string sql = @"  SELECT CODE_ID, 
                                        DECODE (CODE_ID,  'DOC','POLI UMUM',
                                                          'DGI','POLI GIGI',
                                                          'MID','KEBIDANAN',
                                                          'PKB','POLI KB',
                                                          'LAB','LABORATORIUM',
                                                          'MED','FARMASI',
                                                          'REG','REGISTRASI',
                                                          'PAY','KASIR') POLI_NAME, 
                                     NVL (MAX (QUE), '-') ANTRIAN_NO
                                FROM KLINIK.TBL_COMMON_DATA A, KLINIK.TBL_ANTRIAN B
                               WHERE A.CODE_ID = B.TYPE_INS(+)
                                 AND B.FLAG(+) = 'Y'
                                 AND TRUNC(B.INS_DATE(+)) = TRUNC(SYSDATE)
                            GROUP BY CODE_ID,A.SORT_ORDER
                            ORDER BY A.SORT_ORDER";

                dtAntrian = koneksi.GetDataTable(sql);
            }
            catch { }

        }
        private void ListAntrianNm()
        {
            try
            {
                dtAntrianNm = null;
                string sql = @"  SELECT A.CALL_ID,
                                         A.QUE,
                                         A.TYPE_INS,
                                         A.PARAM,
                                         C.NAME ANTRIAN_NM
                                    FROM KLINIK.TBL_ANTRIAN A, 
                                         KLINIK.TBL_KUNJUNGAN B, 
                                         KLINIK.TBL_PATIENT_INFO C
                                   WHERE A.FLAG(+) = 'N'
                                     AND A.QUE     = B.QUE01(+)
                                     AND A.TYPE_INS = B.PURPOSE(+)
                                     AND B.PATIENT_NO = C.PATIENT_NO(+) 
                                     AND TRUNC(A.INS_DATE) = TRUNC(SYSDATE)
                                     AND TRUNC(B.VISIT_DATE) = TRUNC(SYSDATE)
                                ORDER BY A.CALL_ID";

                dtAntrianNm = koneksi.GetDataTable(sql);
            }
            catch
            {
            }

        }
        private void updateAntrianFlag(string call_id)
        {
            try
            {
                string sql = @"UPDATE KLINIK.TBL_ANTRIAN SET FLAG = 'Y' WHERE CALL_ID = '" + call_id + "' AND TRUNC(INS_DATE) = TRUNC(SYSDATE)";
                koneksi.ExecuteNonQuery(sql);
            } catch
            {
            }

        }

        private void loadGridAntrian(string codePoli)
        {
            try
            {
                dtGridAntrian = null;

                string sql = @"      SELECT ROWNUM AS C_NO, A.* FROM (
                                            SELECT A.QUE01  NO_PASIEN, 
                                               B.NAME       NAMA_PASIEN,
                                               A.PURPOSE
                                          FROM KLINIK.TBL_KUNJUNGAN A, KLINIK.TBL_PATIENT_INFO B
                                         WHERE     1 = 1
                                               AND TRUNC(A.VISIT_DATE) = TRUNC(SYSDATE)
                                               AND A.PURPOSE = '" + codePoli + @"'
                                               AND NOT EXISTS
                                                      (SELECT ''
                                                         FROM KLINIK.TBL_ANTRIAN Z
                                                        WHERE     Z.TYPE_INS = A.PURPOSE
                                                              AND Z.QUE = A.QUE01
                                                              AND Z.TYPE_INS = '" + codePoli + @"'
                                                              AND TRUNC(Z.INS_DATE) = TRUNC(SYSDATE))
                                               AND A.PATIENT_NO = B.PATIENT_NO(+)
                                        ORDER BY NO_PASIEN
                                       ) A";

                dtGridAntrian = koneksi.GetDataTable(sql);
                grdAntrian.DataSource = dtGridAntrian;
            }
            catch { }

        }
        private void loadGridHold(string codePoli)
        {
            try
            {
                dtGridHold = null;

                string sql = @" SELECT ROWNUM AS C_NO, A.* FROM (
                                        SELECT A.QUE01  NO_PASIEN, 
                                               B.NAME   NAMA_PASIEN,
                                               A.PURPOSE
                                          FROM KLINIK.TBL_KUNJUNGAN A, KLINIK.TBL_PATIENT_INFO B
                                         WHERE     1 = 1
                                               AND TRUNC(A.VISIT_DATE) = TRUNC(SYSDATE)
                                               AND A.PURPOSE = '" + codePoli + @"'
                                               AND EXISTS
                                                      (SELECT ''
                                                         FROM KLINIK.TBL_ANTRIAN Z
                                                        WHERE     Z.TYPE_INS    = A.PURPOSE
                                                              AND Z.QUE         = A.QUE01
                                                              AND Z.TYPE_INS    = '" + codePoli + @"'
                                                              AND Z.FLAG        = 'H'
                                                              AND TRUNC(Z.INS_DATE) = TRUNC(SYSDATE))
                                               AND A.PATIENT_NO = B.PATIENT_NO(+)
                                        ORDER BY NO_PASIEN
                                   ) A";

                dtGridHold = koneksi.GetDataTable(sql);
                grdKelewat.DataSource = dtGridHold;
            }
            catch { }

        }

        private void loadDokter()
        {
            try
            {
                dtDokter = null;

                string sql = @" SELECT ID_DOKTER, NAMA, POLI, LINK_FOTO FROM TBL_DOKTER WHERE FLAG = 'Y' ";

                dtDokter = koneksi.GetDataTable(sql);
                
                if(dtDokter != null && dtDokter.Rows.Count > 0)
                {
                    dtDokter.Columns.Add("IMAGE", typeof(Image));

                    foreach (DataRow row in dtDokter.Rows)
                    {
                        try
                        {
                            string linkFoto = row["LINK_FOTO"].ToString();
                            Image image = Image.FromFile(linkFoto);
                            row["IMAGE"] = image;
                        }
                        catch
                        {
                            row["IMAGE"] = null;
                        }


                    }
                }

            }
            catch { }

        }

        private static Image ResizeImage(Image image, int newWidth, int newHeight)
        {
            try
            {

                if (image != null)
                {
                    Bitmap newImage = new Bitmap(newWidth, newHeight);

                    using (Graphics graphics = Graphics.FromImage(newImage))
                    {
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                    }
                    return newImage;
                }
                else
                {
                    return null;
                }
                
            }
            catch
            {
                return null;

            }

        }



        private void isiAntrian(string Antrian)
        {
            try {
                if (Antrian == "CARD NOMOR")
                {
                    if (dtAntrian != null && dtAntrian.Rows.Count > 0)
                    {
                        AntrianPoliUmum.PoliText = dtAntrian.Rows[0]["POLI_NAME"]?.ToString();
                        AntrianPoliUmum.AntrianText = dtAntrian.Rows[0]["ANTRIAN_NO"]?.ToString();
                        AntrianPoliGigi.PoliText = dtAntrian.Rows[1]["POLI_NAME"]?.ToString();
                        AntrianPoliGigi.AntrianText = dtAntrian.Rows[1]["ANTRIAN_NO"]?.ToString();
                        AntrianKebidanan.PoliText = dtAntrian.Rows[2]["POLI_NAME"]?.ToString();
                        AntrianKebidanan.AntrianText = dtAntrian.Rows[2]["ANTRIAN_NO"]?.ToString();
                        AntrianPoliKB.PoliText = dtAntrian.Rows[3]["POLI_NAME"]?.ToString();
                        AntrianPoliKB.AntrianText = dtAntrian.Rows[3]["ANTRIAN_NO"]?.ToString();
                        AntrianLaboratorium.PoliText = dtAntrian.Rows[4]["POLI_NAME"]?.ToString();
                        AntrianLaboratorium.AntrianText = dtAntrian.Rows[4]["ANTRIAN_NO"]?.ToString();
                        AntrianFarmasi.PoliText = dtAntrian.Rows[5]["POLI_NAME"]?.ToString();
                        AntrianFarmasi.AntrianText = dtAntrian.Rows[5]["ANTRIAN_NO"]?.ToString();
                        AntrianRegistrasi.PoliText = dtAntrian.Rows[6]["POLI_NAME"]?.ToString();
                        AntrianRegistrasi.AntrianText = dtAntrian.Rows[6]["ANTRIAN_NO"]?.ToString();
                        AntrianKasir.PoliText = dtAntrian.Rows[7]["POLI_NAME"]?.ToString();
                        AntrianKasir.AntrianText = dtAntrian.Rows[7]["ANTRIAN_NO"]?.ToString();

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
