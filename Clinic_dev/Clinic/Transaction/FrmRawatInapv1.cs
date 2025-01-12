using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clinic
{
    public partial class FrmRawatInapv1 : DevExpress.XtraEditors.XtraForm
    {
        private string anamesaID = "", visitid ="", headid ="", v_empid = "", RMNO="",pasienno="";
        ConnectDb ConnOra = new ConnectDb();
        KoneksiOra  koneksi = new KoneksiOra();
        DataTable dtJadwalObat; DataTable dtStock;
        DataTable dtObatPulang; DataTable datstock = new DataTable();
        DataTable dtCppt; DataTable dtMedis; DataTable dtVisitDokter;
        DataTable dtVital; DataTable dtGlMed = new DataTable();
        List<Layanan> listLaya2 = new List<Layanan>(); List<Layanan> listLayav = new List<Layanan>();    
        List<Dokter> listDokter = new List<Dokter>(); List<Dosis> listDosis = new List<Dosis>();

        List<Medicine> listMedicine = new List<Medicine>();
        List<MedicineInfo> listMedicineInfo = new List<MedicineInfo>();
        List<Stat> listHours = new List<Stat>();
        List<Formula> listFormula = new List<Formula>();
        List<Formula2> listFormula2 = new List<Formula2>();

        public FrmRawatInapv1()
        {
            InitializeComponent();
            //this.Location = new Point(0, 0);
            //this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            Scroll2.HorizontalScroll.Visible = true; 
            Scroll2.VerticalScroll.Visible = true;
            InitializeLookupEdit();
            txControlLanjutan.Properties.Mask.Culture = new System.Globalization.CultureInfo("id-ID");
            txControlLanjutan.Properties.Mask.EditMask = "dddd, yyyy-MM-dd HH:mm:ss";
            txControlLanjutan.Properties.Mask.UseMaskAsDisplayFormat = true; 

            foreach (Control control in scrolPulang.Controls)
            {
                if (control is LabelControl)
                {
                    LabelControl labelControl = (LabelControl)control;
                    labelControl.Padding = new Padding(3, 3, 3, 3);
                }
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US", true);
            cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
        }

        #region Main
        private void FrmRawatInapv1_Load(object sender, EventArgs e)
        {
            loadDataAnamnesa();
            LoadItemLayanan();
        }
        private void LoadItemLayanan()
        {
            string SQL = "";
            SQL = SQL + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) treat_item_name ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_treatment_item ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and treat_type_id = 'TRT01'  AND UPPER(TREAT_ITEM_NAME) NOT LIKE '%VISIT DOKTER%' "; 

            OleDbConnection oraConnectly = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraly = new OleDbDataAdapter(SQL, oraConnectly);
            DataTable dtly = new DataTable();
            adOraly.Fill(dtly);
            listLaya2.Clear();
            for (int i = 0; i < dtly.Rows.Count; i++)
            {
                listLaya2.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
            }

            string SQL1 = "";
            SQL1 = SQL1 + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) treat_item_name ";
            SQL1 = SQL1 + Environment.NewLine + "from KLINIK.cs_treatment_item ";
            SQL1 = SQL1 + Environment.NewLine + "where 1=1 ";
            SQL1 = SQL1 + Environment.NewLine + "and treat_type_id = 'TRT01'  AND UPPER(TREAT_ITEM_NAME) LIKE '%VISIT DOKTER%' ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOrav = new OleDbDataAdapter(SQL1, oraConnect);
            DataTable dtlv = new DataTable();
            adOrav.Fill(dtlv);
            listLayav.Clear();
            for (int i = 0; i < dtlv.Rows.Count; i++)
            {
                listLayav.Add(new Layanan() { layananCode = dtlv.Rows[i]["treat_item_id"].ToString(), layananName = dtlv.Rows[i]["treat_item_name"].ToString() });
            }

            dtGlMed.Clear();
            string sql_med = " select med_cd, initcap(med_name) || ' (BPJS: ' || bpjs_cover || ')' med_name from KLINIK.cs_medicine where status = 'A'  and MED_GROUP ='OBAT' order by med_name ";
            OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_med, sqlConnect3);
            DataTable dt3 = new DataTable();
            dtGlMed = dt3;
            adSql3.Fill(dt3);
            listMedicine.Clear();
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                listMedicine.Add(new Medicine() { medicineCode = dt3.Rows[i]["med_cd"].ToString(), medicineName = dt3.Rows[i]["med_name"].ToString() });
            }

            string SQL2 = "";
            SQL2 = SQL2 + Environment.NewLine + "select ID_DOKTER, initcap(NM_DOKTER) Nama_Dokter ";
            SQL2 = SQL2 + Environment.NewLine + "from KLINIK.CS_DOKTER ";
            SQL2 = SQL2 + Environment.NewLine + "where 1=1 ";
            //SQL = SQL + Environment.NewLine + "and treat_type_id = 'TRT01'  ";

            OleDbConnection oraConny = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(SQL2, oraConny);
            DataTable dtdok = new DataTable();
            adOra.Fill(dtdok);
            listDokter.Clear();
            for (int i = 0; i < dtdok.Rows.Count; i++)
            {
                listDokter.Add(new Dokter() { ID_Dokter = dtdok.Rows[i]["ID_DOKTER"].ToString(), Nama_Dokter = dtdok.Rows[i]["Nama_Dokter"].ToString() });
            }

            string sql_dosis = " select code_id, code_name from CS_CODE_DATA where code_class_id = 'DOSIS' order by SORT_ORDER ";
            OleDbConnection oraCondsd = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOrados = new OleDbDataAdapter(sql_dosis, oraCondsd);
            DataTable dtgsis = new DataTable();
            adOrados.Fill(dtgsis);
            listDosis.Clear();
            for (int i = 0; i < dtgsis.Rows.Count; i++)
            {
                listDosis.Add(new Dosis() { DosisCode = dtgsis.Rows[i]["code_id"].ToString(), DosisName = dtgsis.Rows[i]["code_name"].ToString() });
            }

            //listMedicineInfo.Clear();
            //listMedicineInfo.Add("(P.C.) Sesudah Makan");
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "B", medicineInfoName = "(A.C.) Sebelum Makan" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "C", medicineInfoName = "(P.R.N.) Bila Perlu" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "D", medicineInfoName = "(G.T.T) Diteteskan" });
            //listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "E", medicineInfoName = "(U.E) Pemakaian Luar" });
        }
        private void loadDataAnamnesa()
        {
            string sql = @"SELECT  A.ANAMNESA_ID,
                                   A.RM_NO,
                                   B.PATIENT_NO,
                                   A.INSP_DATE,
                                   C.NAME,
                                   DECODE(B.GROUP_PATIENT, 'COMM', 'Umum','BPJS') GROUP_PATIENT,
                                   DECODE(B.STATUS,'A', 'Register', 'Progress') STATUS,
                                   C.FAMILY_HEAD, A.ID_VISIT, E.HEAD_ID
                              FROM CS_ANAMNESA A, CS_PATIENT B, CS_PATIENT_INFO C, CS_VISIT D, CS_TREATMENT_HEAD E, KLINIK.cs_inpatient F
                              WHERE A.ID_VISIT = D.ID_VISIT AND D.inpatient_id=D.inpatient_id
                                and d.ID_VISIT = E.ID_VISIT
                                AND D.STATUS not in ('CLS','CAN')  and F.status in ('REG','OPN') 
                                AND B.PATIENT_NO = D.PATIENT_NO
                                AND B.PATIENT_NO = C.PATIENT_NO";
            //grdMain.DataSource = ORADB.SetData(ORADB.XE, sql);
            grdMain.DataSource = ConnOra.Data_Table_ora(sql);
            gvwMain.BestFitColumns();
        }
        private void InitializeLookupEdit()
        {
            var beratbadan = new List<FN.LookupData>
            {
                new FN.LookupData("", ""),
                new FN.LookupData("1", "1-5 Kg"),
                new FN.LookupData("2", "6-10 Kg"),
                new FN.LookupData("3", "11-15 Kg"),
                new FN.LookupData("4", ">15 Kg"),
                new FN.LookupData("5", "Tidak yakin penurunannya")
            };

            lebrtbadan.Properties.DataSource = beratbadan;
            lebrtbadan.Properties.DisplayMember = "Display";
            lebrtbadan.Properties.ValueMember = "Value";
            lebrtbadan.EditValue = "";
        }

        private void getData(string id)
        {
            try
            {
                DataTable dt1 = ConnOra.Data_Table_ora("select * from T1_RAWAT_INAP1 where anamesa_id = " + id + " ");
                DataTable dt2 = ConnOra.Data_Table_ora("select * from T1_RAWAT_INAP2 where anamesa_id = " + id + " ");
                //DataTable dt1 = ORADB.SetData(ORADB.XE, "select * from T1_RAWAT_INAP1 where anamesa_id = " + id + " ");
                //DataTable dt2 = ORADB.SetData(ORADB.XE, "select * from T1_RAWAT_INAP2 where anamesa_id = " + id + " ");
                if(dt1.Rows.Count > 0)
                {
                    
                    mmKeluhan.Text = FN.rowVal(dt1, "KELUHAN_UTAMA");
                    FN.splitVal1(FN.rowVal(dt1, "PENYAKIT_LALU"),rgSakitLalu, txSakitLalu);
                    FN.splitVal3(FN.rowVal(dt1, "PERNAH_DIRAWAT"),rgPernahRawat, txDiagnosa, txKapanRawat, txRawatDi);
                    FN.splitVal1(FN.rowVal(dt1, "PERNAH_OPERASI"), rgPrnhOperasi, txJnsOperasi);
                    FN.splitVal4(FN.rowVal(dt1, "PENYAKIT_KELUARGA"),gbRwSakitKlrg, rgRwSktKlrg, txSakitKlrga); 
                    FN.splitVal4(FN.rowVal(dt1, "TERGANTUNG_THD"),gbTergantungThdp, rgKetergantungan, txketergantungan);
                    FN.splitVal1(FN.rowVal(dt1, "RIWAYAT_PEKERJAAN"), rgRiwayatKerja, txRwytKerja);
                    FN.splitVal4(FN.rowVal(dt1, "RIWAYAT_ALERGI"), gbRwAlergi, rgAlergi, txAlergi);
                    FN.splitVal1(FN.rowVal(dt1, "RIWAYAT_OBAT"), rgRiwayatObat, txRiwayatObat);
                    txTd.Text = FN.rowVal(dt1, "TD");
                    txNadi.Text = FN.rowVal(dt1, "NADI");
                    txP.Text = FN.rowVal(dt1, "P");
                    txSuhu.Text = FN.rowVal(dt1, "SUHU");
                    FN.splitVal1(FN.rowVal(dt1, "KELUHAN"), rgKeluhan, txKeluhan);
                    txBtsMakan.Text = FN.rowVal(dt1, "BATAS_MAKAN");
                    FN.splitVal(FN.rowVal(dt1, "GIGI_PALSU"), rgGigiPalsu);
                    FN.splitVal(FN.rowVal(dt1, "MUAL"), rgMual);
                    FN.splitVal(FN.rowVal(dt1, "MUNTAH"), rgMuntah);
                    txBB.Text = FN.rowVal(dt1, "BB");
                    txTbPb.Text = FN.rowVal(dt1, "TB");
                    txImt.Text = FN.rowVal(dt1, "IMT");
                    txGstKet.Text = FN.rowVal(dt1, "GST_KET");
                    FN.splitVal1(FN.rowVal(dt1, "PENDENGARAN"), rgPendengaran, txPdngrDtl);
                    FN.splitVal1(FN.rowVal(dt1, "PENGLIHATAN"), rgPenglihatan, txPnglihtDtl);
                    FN.splitVal1(FN.rowVal(dt1, "DEFEKASI"), rgDefekasi, txDefekasiDtl);
                    FN.splitVal1(FN.rowVal(dt1, "MIKSI"), rgMiksi, txMiksiDtl);
                    FN.splitVal1(FN.rowVal(dt1, "KULIT"), rgKulit, txKulitDtl);
                    txSkorNorton.Text = FN.rowVal(dt1, "SKOR_NORTON");
                    FN.splitVal(FN.rowVal(dt1, "RESIKO_DEKUBITUS"), rbDekubitus);
                    FN.setCheckList(FN.rowVal(dt1, "LOKASI_LUKA"), ckLokasiLuka);
                    txPeriksaFisik.Text = FN.rowVal(dt1, "PERIKSA_FISIK_LAIN");
                    FN.splitVal(FN.rowVal(dt1, "FORM_PERIKSA_KHUSUS"), rgPeriksaKhusus);
                    FN.splitVal2(FN.rowVal(dt1, "STATUS_PSIKOLOGI"),gbStsPsikologi, txStsPsikologi);
                    setStsMental(FN.rowVal(dt1, "STATUS_MENTAL"));
                    FN.splitVal(FN.rowVal(dt1, "HUBUNGAN_KELUARGA"), rgHubKluarga);
                    FN.splitVal1(FN.rowVal(dt1, "TEMPAT_TINGGAL"), rgTmpTinggal, txTpTinggalDtl);
                    txNmKerabat.Text = FN.rowVal(dt1, "NAMA_KERABAT");
                    txHubKerabat.Text = FN.rowVal(dt1, "HUB_KERABAT");
                    txTlpKerabat.Text = FN.rowVal(dt1, "TLP_KERABAT");
                    txkegAgama.Text = FN.rowVal(dt1, "KEG_AGAMA");
                    txkegSpirit.Text = FN.rowVal(dt1, "KEG_SPIRITUAL");
                    FN.splitVal4(FN.rowVal(dt1, "HAMBATAN_BELAJAR"), gbHambatanBljr, rgHmbtanBljr, txHmbtan);
                    FN.splitVal1(FN.rowVal(dt1, "BUTUH_PENERJEMAH"), rgPnrjemah, txPnrjmhDtl);
                    FN.splitVal2(FN.rowVal(dt1, "KEBUTUHAN_EDUKASI"), pnlKbthnEdukasi, txKbthnEdukasi);
                    FN.splitVal4(FN.rowVal(dt1, "BERSEDIA_DIKUNJUNGI"), pnlSedia, rgSedia, txSedia);
                    FN.splitVal(FN.rowVal(dt1, "RESIKO_CEDERA"), rgResikoCedera);
                    FN.splitVal(FN.rowVal(dt1, "MENERIMA_INFO"), rgMnrimaInfo);
                }

                if(dt2.Rows.Count > 0)
                {
                    FN.splitVal1(FN.rowVal(dt2, "MOBILISASI"), rgMobilisasi, txMobilisasiDtl);
                    txAltBantujalan.Text = FN.rowVal(dt2, "ALAT_BANTU_JALAN");
                    FN.splitVal(FN.rowVal(dt2, "NYERI"), rgNyeri);
                    FN.setCheckList(FN.rowVal(dt2, "SKALA_NYERI"), chkSkalaNyeri);
                    FN.splitVal(FN.rowVal(dt2, "TINGKAT_NYERI"), rgTingkatNyeri);
                    txLokasiNyeri.Text = FN.rowVal(dt2, "LOKASI_NYERI");
                    txFrekuensi.Text = FN.rowVal(dt2, "FREKUENSI_NYERI");
                    txDurasiNyeri.Text = FN.rowVal(dt2, "DURASI_NYERI");
                    txScorNyeri.Text = FN.rowVal(dt2, "SKOR_NYERI");
                    FN.splitVal2(FN.rowVal(dt2, "NYERI_HILANG"), gbNyeriHilang, txNyeriHilang);
                    checkTurunBB(FN.rowVal(dt2, "TURUN_BERAT_BADAN"), rgTurunBB, lebrtbadan);
                    FN.splitVal(FN.rowVal(dt2, "KURANG_ASUPAN_MAKAN"), rgAsupanMakan);
                    FN.splitVal4(FN.rowVal(dt2, "DIAGNOSE_KHUSUS"), pnlDiagnoseKhusus, rgDiagnoseKh, txDiagnoseDtl);
                    FN.splitVal1(FN.rowVal(dt2, "LAPOR_TIM_TRGZ"),rgLapor_tr_Gizi, txLaporDtl);
                    mmPerawat.Text = FN.rowVal(dt2, "MSLH_PERAWAT");
                    mmDokter.Text = FN.rowVal(dt2, "MSLH_DOKTER");
                    mmTujuanTerukur.Text = FN.rowVal(dt2, "TUJUAN_TERUKUR");

                    string YN = FN.rowVal(dt2, "SUSUN_RENCANA_PERAWAT");
                    bool chk = YN == "Y" ? chkSusunRencana.Checked = true : chkSusunRencana.Checked=false;
                }
                DataTable dt3 = ConnOra.Data_Table_ora("select * from T1_RESIKO_JATUH_ANAK where anamesa_id = " + anamesaID + " ");
                //DataTable dt3 = ORADB.SetData(ORADB.XE, "select * from T1_RESIKO_JATUH_ANAK where anamesa_id = " + anamesaID + " ");
                if (dt3.Rows.Count > 0)
                {
                    FN.splitVal(FN.rowVal(dt3, "USIA"), rgUsia);
                    FN.splitVal(FN.rowVal(dt3, "JENIS_KELAMIN"), rgJenkel);
                    FN.splitVal(FN.rowVal(dt3, "DIAGNOSIS"), rgDiagnosis);
                    FN.splitVal(FN.rowVal(dt3, "GANGGUAN_FN_KOGNITIF"), rgGangguan);
                    FN.splitVal(FN.rowVal(dt3, "FAKTOR_LINGKUNGAN"), rgFlingkungan);
                    FN.splitVal(FN.rowVal(dt3, "SEDASI_ANESTESI"), rgSedasiAnestesi);
                    FN.splitVal(FN.rowVal(dt3, "PENGGUNAAN_OBAT"), rguseObat);
                    txScoreAnak.Text = FN.rowVal(dt3, "SKOR");
                    txResikoAnak.Text = FN.rowVal(dt3, "RESIKO");
                }
                DataTable dt4 = ConnOra.Data_Table_ora("select * from T1_RESIKO_JATUH_DEWASA where anamesa_id = " + anamesaID + " ");
                //DataTable dt4 = ORADB.SetData(ORADB.XE, "select * from T1_RESIKO_JATUH_DEWASA where anamesa_id = " + anamesaID + " ");
                if (dt4.Rows.Count > 0)
                {
                    FN.splitVal1(FN.rowVal(dt4, "RIWAYAT_JATUH"), rgRiwayatJatuh, txKet1);
                    FN.splitVal1(FN.rowVal(dt4, "MEMILIKI_LBH_PENYAKIT"), rgDiagnosaSekunder, txKet2);
                    FN.splitVal1(FN.rowVal(dt4, "ALAT_BANTU_JALAN"), rgAltBantuJalan, txKet3);
                    FN.splitVal1(FN.rowVal(dt4, "TERPASANG_INFUS"), rgInfus, txket4);
                    FN.splitVal1(FN.rowVal(dt4, "GAYA_BERJALAN"), rgGayaJalan, txKet5);
                    FN.splitVal1(FN.rowVal(dt4, "STATUS_MENTAL"), rgstsMental, txket6);
                    txTotalNilai.Text = FN.rowVal(dt4, "TOTAL_NILAI");
                    txResikoDewasa.Text = FN.rowVal(dt4, "RESIKO");
                    lblTindakan.Text = FN.rowVal(dt4, "TINDAKAN");
                }
                DataTable dt5 = ConnOra.Data_Table_ora("select * from T1_PERENCANAAN_PULANG where anamesa_id = " + anamesaID + " ");
                //DataTable dt5 = ORADB.SetData(ORADB.XE, "select * from T1_PERENCANAAN_PULANG where anamesa_id = " + anamesaID + " ");
                if (dt5.Rows.Count > 0)
                {
                    DateTime dte;
                    if (DateTime.TryParseExact(FN.rowVal(dt5, "tanggal_keluar"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dte))
                        dtkeluar.EditValue = dte;

                    cbKeadaanPulang.SelectedItem = FN.rowVal(dt5, "keadaan_pulang");
                    txKeadaanDtl.Text = FN.rowVal(dt5, "keadaan_pulang_dtl");
                    cbAlatTerpasang.SelectedItem = FN.rowVal(dt5, "alat_terpasang");
                    txAltTerpsang.Text = FN.rowVal(dt5, "alat_terpasang_dtl");
                    txObatRutin.Text = FN.rowVal(dt5, "obat_dihentikan");
                    txjenisPeriksa.Text = FN.rowVal(dt5, "jenis_periksa");
                    txProcedurePlan.Text = FN.rowVal(dt5, "prosedure_persiapan");
                    txAktivitas.Text = FN.rowVal(dt5, "aktivitas");
                    txPolaMakan.Text = FN.rowVal(dt5, "pola_makan");
                    txPsikologis.Text = FN.rowVal(dt5, "psikologis");
                    txKebiasaanlain.Text = FN.rowVal(dt5, "kebiasaan_lain");
                    FN.splitVal(FN.rowVal(dt5, "perawatan_lanjutan"), rgRawatLanjutan);
                    txProcRawat.Text = FN.rowVal(dt5, "prosedure_perawatan");
                    txdoRawat.Text = FN.rowVal(dt5, "pemberi_perawatan");
                    txWaktu.Text = FN.rowVal(dt5, "waktu_frekuensi_rawat");
                    txUnitKesehatan.Text = FN.rowVal(dt5, "unit_kesehatan");
                    txTindakan.Text = FN.rowVal(dt5, "tindakan_darurat");
                    txControlLanjutan.Text = FN.rowVal(dt5, "tgl_kontrol_lanjutan");
                    txDokterDituju.Text = FN.rowVal(dt5, "dokter_dituju");
                    setDocPulang(FN.rowVal(dt5, "dokumen_dibawa"));

                    DataTable dtO = ConnOra.Data_Table_ora("select * from T1_OBAT_PULANG where anamesa_id = " + anamesaID + " order by seq");
                    //DataTable dtO = ORADB.SetData(ORADB.XE, "select * from T1_OBAT_PULANG where anamesa_id = " + anamesaID + " order by seq");
                    gcObtPlng.DataSource = dtO;
                }

                DataTable dt6 = ConnOra.Data_Table_ora("select * from T1_RESUME_PULANG where anamesa_id = " + anamesaID + " ");
                //DataTable dt6 = ORADB.SetData(ORADB.XE, "select * from T1_RESUME_PULANG where anamesa_id = " + anamesaID + " ");
                if (dt6.Rows.Count > 0)
                {
                    DateTime dte;
                    if (DateTime.TryParseExact(FN.rowVal(dt6, "tanggal_keluar"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dte))
                        dtKeluarx.EditValue = dte;

                    txDokterPengirim.Text = FN.rowVal(dt6, "dokter_pengirim");
                    txDokterKonsultan.Text  = FN.rowVal(dt6, "dokter_konsultan");
                    txDiagnosaAkhir.Text  = FN.rowVal(dt6, "diagnose_akhir");
                    txAnamesa.Text = FN.rowVal(dt6, "anamesa");
                    mmPeriksaFisik.Text = FN.rowVal(dt6, "periksa_fisik_lab");
                    txPengobatan.Text = FN.rowVal(dt6, "pengobatan_dilakukan");
                    txTindakanDo.Text = FN.rowVal(dt6, "tindakan_dilakukan");
                    txTerapiLanjtan.Text = FN.rowVal(dt6, "terapi_lanjutan");
                    txAnjuran.Text = FN.rowVal(dt6, "anjuran");
                }

                DataTable dt7 = ConnOra.Data_Table_ora("select * from T1_CPPT where anamesa_id = " + anamesaID + " order by seq");
                //DataTable dt7 = ORADB.SetData(ORADB.XE, "select * from T1_CPPT where anamesa_id = " + anamesaID+" order by seq");
                gcCppt.DataSource = dt7;

                DataTable dt9 = ConnOra.Data_Table_ora("select * from T1_ASESMEN_GIZI where anamesa_id = " + anamesaID + " ");
                //DataTable dt9 = ORADB.SetData(ORADB.XE, "select * from T1_ASESMEN_GIZI where anamesa_id = " + anamesaID + " ");
                if(dt9.Rows.Count > 0)
                {
                    txBbi.Text = FN.rowVal(dt9, "bbi");
                    txStsGizi.Text = FN.rowVal(dt9, "sts_gizi");
                    txBbDw.Text = FN.rowVal(dt9, "bb_dewasa");
                    txTbDw.Text = FN.rowVal(dt9, "tb_dewasa");
                    txLilaDw.Text = FN.rowVal(dt9, "lila_dewasa");
                    txTgLutut.Text = FN.rowVal(dt9, "tinggi_lutut");
                    txImtDw.Text = FN.rowVal(dt9, "imt_dewasa");
                    txBBU.Text = FN.rowVal(dt9, "bbu");
                    txBBTB.Text = FN.rowVal(dt9, "bbtb");
                    txImtAnk.Text = FN.rowVal(dt9, "imt_anak");
                    txBiokimia.Text = FN.rowVal(dt9, "biokimia");
                    txKlinis.Text = FN.rowVal(dt9, "klinis");
                    txAlergiMkn.Text = FN.rowVal(dt9, "alergi_makan");
                    txPolaMkn.Text = FN.rowVal(dt9, "pola_makan");
                    txNlaiEnergi.Text = FN.rowVal(dt9, "nilai_energi");
                    txPercenEnergi.Text = FN.rowVal(dt9, "percen_energi");
                    txKbthnEnergi.Text = FN.rowVal(dt9, "kbthn_energi");
                    txNilaiProtein.Text = FN.rowVal(dt9, "nilai_protein");
                    txPercenProtein.Text = FN.rowVal(dt9, "percen_protein");
                    txKbthnProtein.Text = FN.rowVal(dt9, "kbthn_protein");
                    txNilaiLemak.Text = FN.rowVal(dt9, "nilai_lemak");
                    txPercenLemak.Text = FN.rowVal(dt9, "percen_lemak");
                    txKbthnLemak.Text = FN.rowVal(dt9, "kbthn_lemak");
                    txNilaiKarbo.Text = FN.rowVal(dt9, "nilai_karbo");
                    txPercenKarbo.Text = FN.rowVal(dt9, "percen_karbo");
                    txKbthnKarbo.Text = FN.rowVal(dt9, "kbthn_karbo");
                    txAsupnKurng.Text = FN.rowVal(dt9, "asupan_kurang");
                    txAsupnLbh.Text = FN.rowVal(dt9, "asupan_lebih");
                    txRiwytPerson.Text = FN.rowVal(dt9, "riwayat_personal");
                    txDiagnosGz.Text = FN.rowVal(dt9, "diagnosa_gizi");
                    txIntrvnsiGz.Text = FN.rowVal(dt9, "intervensi_gizi");
                    txMonitoring.Text = FN.rowVal(dt9, "monitoring");
                }

                //DataTable dt8 = ORADB.SetData(ORADB.XE, "select * from T1_JADWAL_BERI_OBAT where anamesa_id = " + anamesaID + " order by seq");
                //gcJadwalObat.DataSource = dt8;

                LoadDataResep(); 

                string SQL
                        = "select ROWNUM SEQ, b.detail_id, c.treat_group_id, b.treat_item_id, c.TREAT_ITEM_NAME, b.treat_qty, b.treat_item_price,  " +
                            "       b.remarks, 'S' action, a.head_id, b.treat_date  TANGGAL, TREAT_JAM JAM, a.pay_status ,a.ID_VISIT " +
                            "  from KLINIK.cs_treatment_head a  " +
                            "  join KLINIK.cs_treatment_detail b on (a.head_id=b.head_id)  " +
                            "  join KLINIK.cs_treatment_item c on (b.treat_item_id=c.treat_item_id)  " +
                            " where ID_VISIT = '" + visitid + "'   and b.ID_DOKTER is  null ";
                            //"   and a.status = 'OPN'  ";

                dtMedis = ConnOra.Data_Table_ora(SQL);   
                gridMedis.DataSource = dtMedis;

                RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
                glLaya.DataSource = listLaya2;
                glLaya.ValueMember = "layananCode";
                glLaya.DisplayMember = "layananName";

                glLaya.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glLaya.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                glLaya.ImmediatePopup = true;
                glLaya.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glLaya.NullText = "";
                gvMedis.Columns[3].ColumnEdit = glLaya;

                string SQL2 = " Select ROWNUM SEQ, b.detail_id, c.treat_group_id, b.treat_item_id, c.TREAT_ITEM_NAME, b.treat_qty, b.treat_item_price,  " +
                              "        b.remarks, 'S' action, a.head_id, b.treat_date  TANGGAL, TREAT_JAM JAM, a.pay_status, a.ID_VISIT, b.ID_DOKTER " +
                              "  from KLINIK.cs_treatment_head a  " +
                              "  join KLINIK.cs_treatment_detail b on (a.head_id=b.head_id)  " +
                              "  join KLINIK.cs_treatment_item c on (b.treat_item_id=c.treat_item_id)  " +
                              "  join KLINIK.CS_DOKTER d on (b.ID_DOKTER = d.ID_DOKTER)  " +
                              " where ID_VISIT = '" + visitid + "'  and b.ID_DOKTER is not null ";
                //"   and a.status = 'OPN'  ";

                dtVisitDokter = ConnOra.Data_Table_ora(SQL2); //ORADB.SetData(ORADB.XE, SQL2);
                gridVisitDoc.DataSource = dtVisitDokter;

                RepositoryItemGridLookUpEdit glvisit = new RepositoryItemGridLookUpEdit();
                glvisit.DataSource = listDokter;
                glvisit.ValueMember = "ID_Dokter";
                glvisit.DisplayMember = "Nama_Dokter";

                glvisit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glvisit.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                glvisit.ImmediatePopup = true;
                glvisit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glvisit.NullText = "";
                gvVisitDoc.Columns[4].ColumnEdit = glvisit; 

                RepositoryItemGridLookUpEdit glLayav = new RepositoryItemGridLookUpEdit();
                glLayav.DataSource = listLayav;
                glLayav.ValueMember = "layananCode";
                glLayav.DisplayMember = "layananName";

                glLayav.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glLayav.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                glLayav.ImmediatePopup = true;
                glLayav.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glLayav.NullText = "";
                gvVisitDoc.Columns[3].ColumnEdit = glLayav;


                dtVital = ConnOra.Data_Table_ora("select * from T1_GRAFIK_VITAL where anamesa_id = " + anamesaID + " "); //ORADB.SetData(ORADB.XE, "select * from T1_GRAFIK_VITAL where anamesa_id = " + anamesaID+" ");
                gcVt.DataSource = dtVital;
                setChart();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Info");
            }
        }

        private bool updateData()
        {
            try
            {
                //Ambil semua data pada inputan TAB-1
                Dictionary<string, string> A1_fields = new Dictionary<string, string>
                {
                    { "keluhan_utama", mmKeluhan.Text?.ToString()},
                    { "penyakit_lalu", FN.getVal(gbRwPenyakitlalu) },
                    { "pernah_dirawat",FN.getVal(gbPernahDirawat) },
                    { "pernah_operasi",FN.getVal(gbPernahOperasi) },
                    { "penyakit_keluarga", FN.getVal(gbRwSakitKlrg) },
                    { "tergantung_thd", FN.getVal(gbTergantungThdp) },
                    { "riwayat_pekerjaan", FN.getVal(gbRwkerja) },
                    { "riwayat_alergi", FN.getVal(gbRwAlergi) },
                    { "riwayat_obat", FN.getVal(gbRwObat) },
                    { "TD", txTd.Text?.ToString() },
                    { "nadi", txNadi.Text?.ToString() },
                    { "p", txP.Text?.ToString() },
                    { "suhu", txSuhu.Text?.ToString() },
                    { "keluhan", FN.getVal(pnKeluhan) },
                    { "batas_makan", txBtsMakan.Text?.ToString() },
                    { "gigi_palsu",FN.radioVal(rgGigiPalsu)},
                    { "mual", FN.radioVal(rgMual) },
                    { "muntah", FN.radioVal(rgMuntah)},
                    { "BB", txBB.Text?.ToString() },
                    { "TB", txTbPb.Text?.ToString() },
                    { "imt", txImt.Text?.ToString() },
                    { "gst_ket", txGstKet.Text?.ToString() },
                    { "pendengaran", FN.getVal(pnlPendengaran) },
                    { "penglihatan", FN.getVal(pnlPenglihatan) },
                    { "defekasi", FN.getVal(pnlDefekasi) },
                    { "miksi", FN.getVal(pnlMiksi) },
                    { "kulit", FN.getVal(pnlKulit) },
                    { "skor_norton", txSkorNorton.Text?.ToString() },
                    { "resiko_dekubitus", FN.radioVal(rbDekubitus) },
                    { "lokasi_luka", FN.chkListOf(ckLokasiLuka) },
                    { "periksa_fisik_lain", txPeriksaFisik.Text?.ToString() },
                    { "form_periksa_khusus", FN.radioVal(rgPeriksaKhusus)},
                    { "status_psikologi", FN.getVal(gbStsPsikologi) },
                    { "status_mental", getStsMental() },
                    { "hubungan_keluarga",FN.radioVal(rgHubKluarga)},
                    { "tempat_tinggal", FN.getVal(pnlTempatTinggal)},
                    { "nama_kerabat", txNmKerabat.Text?.ToString() },
                    { "hub_kerabat", txHubKerabat.Text?.ToString() },
                    { "tlp_kerabat", txTlpKerabat.Text?.ToString() },
                    { "keg_agama", txkegAgama.Text?.ToString() },
                    { "keg_spiritual", txkegSpirit.Text?.ToString() },
                    { "hambatan_belajar", FN.getVal(gbHambatanBljr) },
                    { "butuh_penerjemah", FN.getVal(pnlButuhPnrjmh) },
                    { "kebutuhan_edukasi", FN.getVal(pnlKbthnEdukasi) },
                    { "bersedia_dikunjungi", FN.getVal(pnlSedia) },
                    { "resiko_cedera", FN.radioVal(rgResikoCedera)},
                    { "menerima_info",FN.radioVal(rgMnrimaInfo)}
                };
                MD.UpdateData(ORADB.XE, "T1_RAWAT_INAP1", "anamesa_id = " + anamesaID + " ", A1_fields);

                //Ambil semua data pada inputan TAB-2
                Dictionary<string, string> A2_fields = new Dictionary<string, string>
                {
                    { "mobilisasi", FN.getVal(gbStsFungsi) },
                    { "alat_bantu_jalan", txAltBantujalan.Text?.ToString() },
                    { "nyeri", FN.radioVal(rgNyeri) },
                    { "skala_nyeri", FN.chkListOf(chkSkalaNyeri) },
                    { "tingkat_nyeri",FN.radioVal(rgTingkatNyeri)  },
                    { "lokasi_nyeri", txLokasiNyeri.Text?.ToString() },
                    { "frekuensi_nyeri", txFrekuensi.Text?.ToString() },
                    { "durasi_nyeri",txDurasiNyeri.Text?.ToString() },
                    { "skor_nyeri", txScorNyeri.Text?.ToString() },
                    { "nyeri_hilang", FN.getVal(gbNyeriHilang) },
                    { "turun_berat_badan", FN.getVal(pnlBeratBadan) },
                    { "kurang_asupan_makan", FN.radioVal(rgAsupanMakan) },
                    { "skor_trgz", "8" },
                    { "diagnose_khusus", FN.getVal(pnlDiagnoseKhusus) },
                    { "lapor_tim_trgz", FN.getVal(pnlLaporTim) },
                    { "mslh_perawat", mmPerawat.Text?.ToString() },
                    { "mslh_dokter", mmDokter.Text?.ToString() },
                    { "tujuan_terukur", mmTujuanTerukur.Text?.ToString() },
                    { "susun_rencana_perawat", chkSusunRencana.Checked?"Y":"N" }
                };
                MD.UpdateData(ORADB.XE, "T1_RAWAT_INAP2", "anamesa_id = " + anamesaID + " ", A2_fields);


                Dictionary<string, string> planingPulangData = new Dictionary<string, string>
                {
                    { "tanggal_keluar", dtkeluar.DateTime.ToString("yyyyMMdd") },
                    { "keadaan_pulang", cbKeadaanPulang.Text?.ToString()},
                    { "keadaan_pulang_dtl", txKeadaanDtl.Text?.ToString() },
                    { "alat_terpasang", cbAlatTerpasang.Text?.ToString() },
                    { "alat_terpasang_dtl", txAltTerpsang.Text?.ToString() },
                    { "obat_dihentikan", txObatRutin.Text?.ToString() },
                    { "jenis_periksa", txjenisPeriksa.Text?.ToString() },
                    { "prosedure_persiapan", txProcedurePlan.Text?.ToString() },
                    { "aktivitas", txAktivitas.Text?.ToString() },
                    { "pola_makan", txPolaMakan.Text?.ToString() },
                    { "psikologis", txPsikologis.Text?.ToString() },
                    { "kebiasaan_lain", txKebiasaanlain.Text?.ToString() },
                    { "perawatan_lanjutan", FN.radioVal(rgRawatLanjutan) },
                    { "prosedure_perawatan", txProcRawat.Text?.ToString() },
                    { "pemberi_perawatan", txdoRawat.Text?.ToString() },
                    { "waktu_frekuensi_rawat", txWaktu.Text?.ToString() },
                    { "unit_kesehatan", txUnitKesehatan.Text?.ToString() },
                    { "tindakan_darurat", txTindakan.Text?.ToString() },
                    { "tgl_kontrol_lanjutan", txControlLanjutan.Text?.ToString() },
                    { "dokter_dituju", txDokterDituju.Text?.ToString() },
                    { "dokumen_dibawa", getDocPulang() }
                };
                bool save = MD.UpdateData(ORADB.XE, "T1_PERENCANAAN_PULANG", "anamesa_id = " + anamesaID + " ", planingPulangData);
                if (save)
                {
                    //if(gvObtPlng.RowCount > 0)
                    //{
                    //    DataTable dt = ORADB.SetData(ORADB.XE, "select * from T1_OBAT_PULANG where anamesa_id = " + anamesaID+" ");
                    //    if(dt != null && dt.Rows.Count > 0)
                    //    {
                    //        ORADB.Execute(ORADB.XE, " delete from T1_OBAT_PULANG where anamesa_id = " + anamesaID + "  ");
                    //    }

                    //    string sql = "insert all ";
                    //    for (int i =0; i< gvObtPlng.RowCount; i++)
                    //    {
                    //        sql = sql + " INTO T1_OBAT_PULANG (anamesa_id, seq, nama_obat, dosis, waktu_beri, cara) values ( ";
                    //        sql = sql + " "+anamesaID+" ,";
                    //        sql = sql + " '"+FN.strVal(gvObtPlng, i, "SEQ") +"' ,";
                    //        sql = sql + " '"+FN.strVal(gvObtPlng, i, "NAMA_OBAT") +"' ,";
                    //        sql = sql + " '"+FN.strVal(gvObtPlng, i, "DOSIS") +"' ,";
                    //        sql = sql + " '"+FN.strVal(gvObtPlng, i, "WAKTU_BERI") +"' ,";
                    //        sql = sql + " '"+FN.strVal(gvObtPlng, i, "CARA") +"' ) ";
                    //    }
                    //    sql = sql + " select * from dual";
                    //    ORADB.Execute(ORADB.XE, sql);
                    //}
                }


                Dictionary<string, string> resumePulangData = new Dictionary<string, string>
                {
                    { "tanggal_keluar", dtKeluarx.DateTime.ToString("ddMMyyyy")},
                    { "dokter_pengirim", txDokterPengirim.Text?.ToString() },
                    { "dokter_konsultan", txDokterKonsultan.Text?.ToString() },
                    { "diagnose_akhir", txDiagnosaAkhir.Text?.ToString() },
                    { "anamesa",txAnamesa.Text?.ToString() },
                    { "periksa_fisik_lab", mmPeriksaFisik.Text?.ToString() },
                    { "pengobatan_dilakukan", txPengobatan.Text?.ToString() },
                    { "tindakan_dilakukan", txTindakanDo.Text?.ToString() },
                    { "terapi_lanjutan", txTerapiLanjtan.Text?.ToString() },
                    { "anjuran", txAnjuran.Text?.ToString() }
                };
                MD.UpdateData(ORADB.XE, "T1_RESUME_PULANG", "anamesa_id = " + anamesaID + " ", resumePulangData);

                Dictionary<string, string> asesmenGiziValues = new Dictionary<string, string>
                {
                    { "bbi", txBbi.Text?.ToString() },
                    { "sts_gizi", txStsGizi.Text?.ToString() },
                    { "bb_dewasa", txBbDw.Text?.ToString() },
                    { "tb_dewasa", txTbDw.Text?.ToString() },
                    { "lila_dewasa", txLilaDw.Text?.ToString() },
                    { "tinggi_lutut", txTgLutut.Text?.ToString() },
                    { "imt_dewasa", txImtDw.Text?.ToString() },
                    { "bbu", txBBU.Text?.ToString() },
                    { "bbtb", txBBTB.Text?.ToString() },
                    { "imt_anak", txImtAnk.Text?.ToString() },
                    { "biokimia", txBiokimia.Text?.ToString() },
                    { "klinis", txKlinis.Text?.ToString() },
                    { "alergi_makan", txAlergiMkn.Text?.ToString() },
                    { "pola_makan", txPolaMkn.Text?.ToString() },
                    { "nilai_energi", txNlaiEnergi.Text?.ToString() },
                    { "percen_energi", txPercenEnergi.Text?.ToString() },
                    { "kbthn_energi", txKbthnEnergi.Text?.ToString() },
                    { "nilai_protein", txNilaiProtein.Text?.ToString() },
                    { "percen_protein", txPercenProtein.Text?.ToString() },
                    { "kbthn_protein", txKbthnProtein.Text?.ToString() },
                    { "nilai_lemak", txNilaiLemak.Text?.ToString() },
                    { "percen_lemak", txPercenLemak.Text?.ToString() },
                    { "kbthn_lemak", txKbthnLemak.Text?.ToString() },
                    { "nilai_karbo", txNilaiKarbo.Text?.ToString() },
                    { "percen_karbo", txPercenKarbo.Text?.ToString() },
                    { "kbthn_karbo", txKbthnKarbo.Text?.ToString() },
                    { "asupan_kurang", txAsupnKurng.Text?.ToString() },
                    { "asupan_lebih", txAsupnLbh.Text?.ToString() },
                    { "riwayat_personal", txRiwytPerson.Text?.ToString() },
                    { "diagnosa_gizi", txDiagnosGz.Text?.ToString() },
                    { "intervensi_gizi", txIntrvnsiGz.Text?.ToString() },
                    { "monitoring", txMonitoring.Text?.ToString() }
                };
                MD.UpdateData(ORADB.XE, "T1_ASESMEN_GIZI", "anamesa_id = " + anamesaID + " ", asesmenGiziValues);

            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Failed to save");
                return false;
            }
            return true;
        }
        #endregion


        #region Controls Actions

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            bool save = updateData();
            if (save)
            {
                MessageBox.Show("Data berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                FN.errosMsg("Data gagal disimpan!", "Error");
            }
        }

        //Event untuk mengatur saat radioGroup atau CheckBox dipilih
        private Control lastSender;
        private void EnableTextEdit(object sender, EventArgs e)
        {
            Control parentControl = null;

            if (sender is RadioGroup)
            {
                RadioGroup radioGroup = (RadioGroup)sender;
                lastSender = radioGroup;
                parentControl = radioGroup.Parent;
                if (radioGroup.EditValue != null && radioGroup.EditValue?.ToString() == "1") {
                    if (parentControl != null) FN.EnableControls(parentControl, true, lastSender);
                }
                else{
                    if (parentControl != null) FN.EnableControls(parentControl, false, lastSender);
                }
            }
            else if (sender is CheckEdit)
            {
                CheckEdit checkEdit = (CheckEdit)sender;
                lastSender = checkEdit;
                parentControl = checkEdit.Parent;
                if (checkEdit.Checked){
                    if (parentControl != null) FN.EnableControls(parentControl, true, lastSender);
                }
                else{
                    if (parentControl != null) FN.EnableControls(parentControl, false, lastSender);
                }
            }
        }


        private void rgKatgriPasien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(rgKatgriPasien.SelectedIndex == 0)
            {
                pnlRJDewasa.Visible = false;
                pnlRJAnak.Visible = true;
            }
            else
            {
                pnlRJAnak.Visible = false;
                pnlRJDewasa.Visible = true;
            }
        }

        private void cbKeadaanPulang_SelectedValueChanged(object sender, EventArgs e)
        {
            string val = cbKeadaanPulang.SelectedIndex.ToString();
            if (val == "0" || val == "1" || val == "2") txKeadaanDtl.Enabled = false;
            else txKeadaanDtl.Enabled = true;

            if (val == "3") lblKeadaan.Text = "Rujuk ke";
            if (val == "4") lblKeadaan.Text = "Alasan";
            if (val == "5") lblKeadaan.Text = "Lainnya";
        }

        private void cbAlatTerpasang_SelectedValueChanged(object sender, EventArgs e)
        {
            string val = cbAlatTerpasang.Text?.ToString();
            if (val == "Lainnya") txAltTerpsang.Enabled = true;
            else txAltTerpsang.Enabled = false;

        }


        private void btnAddJadwalObat_Click(object sender, EventArgs e)
        {
            //if (dtJadwalObat == null) return;

            //DataRow newRow = dtJadwalObat.NewRow();

            //newRow["SEQ"] = ((gvJadwalObat.RowCount) + 1).ToString();
            //dtJadwalObat.Rows.Add(newRow);

            //gcJadwalObat.DataSource = dtJadwalObat;

            gvJadwalObat.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gvJadwalObat.AddNewRow();
        }

        private void btnAddCppt_Click(object sender, EventArgs e)
        {
            if (dtCppt == null) return;

            DataRow newRow = dtCppt.NewRow();

            newRow["SEQ"] = ((gvCppt.RowCount) + 1).ToString();
            dtCppt.Rows.Add(newRow);

            gcCppt.DataSource = dtCppt;
        }

        private void addObat_Click(object sender, EventArgs e)
        {
            //if (dtObatPulang == null) return;

            //DataRow newRow = dtObatPulang.NewRow();

            //newRow["SEQ"] = ((gvObtPlng.RowCount) + 1).ToString();
            //dtObatPulang.Rows.Add(newRow);

            //gcObtPlng.DataSource = dtObatPulang;

            gvObtPlng.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gvObtPlng.AddNewRow();

        }
        #endregion




        private string getStsMental()
        {
            string a = "";string b = "";string c = "";
            if (ckStsMental1.Checked) a = ckStsMental1.Text?.ToString();
            if (chkStsMental2.Checked) b = chkStsMental2.Text?.ToString();
            if (chkStasMental3.Checked) c = chkStasMental3.Text?.ToString();

            return a + "::" + b + "=>" + txStsMental2.Text?.ToString() + "::" + c + "=>" + txStsMental3.Text?.ToString();
        }

        private void setStsMental(string dt)
        {
            string[] val = dt.Split(new string[] { "::" }, StringSplitOptions.None);
            if(val.Length > 1)
            {
                if (val[0].ToString() != "")
                    ckStsMental1.Checked = true;
                else return;

                if (val[1].ToString() != "=>")
                {
                    string[] aa = val[1].ToString().Split(new string[] { "=>" }, StringSplitOptions.None);
                    txStsMental2.Text = aa[1];
                    chkStsMental2.Checked = true;
                }

                if (val[2].ToString() != "=>")
                {
                    string[] aa = val[2].ToString().Split(new string[] { "=>" }, StringSplitOptions.None);
                    txStsMental3.Text = aa[1];
                    chkStasMental3.Checked = true;
                }
            } 
        }

        private string getDocPulang()
        {
            string a = "";string b = "";string c = "";string d = "";
            string e = "";string f = "";string g = "";string h = "";
            if (chkDoc1.Checked) a = chkDoc1.Text?.ToString();
            if (chkDoc2.Checked) b = chkDoc2.Text?.ToString();
            if (chkDoc3.Checked) c = chkDoc3.Text?.ToString();
            if (chkDoc4.Checked) d = chkDoc4.Text?.ToString();
            if (chkDoc5.Checked) e = chkDoc5.Text?.ToString();
            if (chkDoc6.Checked) f = chkDoc6.Text?.ToString();
            if (chkDoc7.Checked) g = chkDoc7.Text?.ToString();
            if (chkDoc8.Checked) h = chkDoc8.Text?.ToString();

           string x = a + "=>"+txDoc1.Text?.ToString()+"::"+b+"=>"+txDoc2.Text.ToString()+"=>"+txDoc3.Text.ToString()+"::";
           string y = c + "=>" + txDoc4.Text.ToString() + "::" + d + "=>" + txDoc5.Text.ToString() + "::" + e + "=>" + txDoc6.Text?.ToString()+"::";
           string z = f+"::"+g+"::"+ h;

           return x + y + z;
        }

        private void setDocPulang(string dt)
        {
            string[] val = dt.Split(new string[] { "::" }, StringSplitOptions.None);
            if(val.Length > 1)
            {
                if (val[0].ToString() == "")
                    return;

                if (val[0].ToString() != "=>")
                {
                    string[] aa = val[0].ToString().Split(new string[] { "=>" }, StringSplitOptions.None);
                    txDoc1.Text = aa[1];
                    chkDoc1.Checked = true;
                }

                if (val[1].ToString() != "=>=>")
                {
                    string[] aa = val[1].ToString().Split(new string[] { "=>" }, StringSplitOptions.None);
                    chkDoc2.Checked = true;
                    txDoc2.Text = aa[1];
                    txDoc3.Text = aa[2];
                }

                if (val[2].ToString() != "=>")
                {
                    string[] aa = val[2].ToString().Split(new string[] { "=>" }, StringSplitOptions.None);
                    txDoc4.Text = aa[1];
                    chkDoc3.Checked = true;
                }
                if (val[3].ToString() != "=>")
                {
                    string[] aa = val[3].ToString().Split(new string[] { "=>" }, StringSplitOptions.None);
                    txDoc5.Text = aa[1];
                    chkDoc4.Checked = true;
                }
                if (val[4].ToString() != "=>")
                {
                    string[] aa = val[4].ToString().Split(new string[] { "=>" }, StringSplitOptions.None);
                    txDoc6.Text = aa[1];
                    chkDoc5.Checked = true;
                }
                if (val[5].ToString() != "")
                {
                    chkDoc6.Checked = true;
                }
                if (val[6].ToString() != "")
                {
                    chkDoc7.Checked = true;
                }
                if (val[7].ToString() != "")
                {
                    chkDoc8.Checked = true;
                }
            }
        }

        

        private void checkTurunBB(string dt, RadioGroup rg, LookUpEdit le)
        {
            string[] val = dt.Split(new string[] { "::" }, StringSplitOptions.None);
            if(val.Length == 3)
            {
                rg.SelectedIndex = Convert.ToInt32(val[0]);
                if(val[2] != "")
                {
                    le.EditValue = val[2];
                }
            }
        }

        private void btnInputData_Click(object sender, EventArgs e)
        {
            
        }


        private void btnSaveX_Click(object sender, EventArgs e)
        {
            bool save = false;
            if(rgKatgriPasien.SelectedIndex == 0)
            {
                DataTable dt = ConnOra.Data_Table_ora("select * from T1_RESIKO_JATUH_ANAK where anamesa_id = " + anamesaID + " ");
                //ORADB.SetData(ORADB.XE, "select * from T1_RESIKO_JATUH_ANAK where anamesa_id = " + anamesaID + " ");
                if(dt.Rows.Count > 0)
                {
                    Dictionary<string, string> resiko_jatuh_anak = new Dictionary<string, string>
                    {
                        { "usia", FN.radioVal(rgUsia) },
                        { "jenis_kelamin", FN.radioVal(rgJenkel) },
                        { "diagnosis", FN.radioVal(rgDiagnosis) },
                        { "gangguan_fn_kognitif", FN.radioVal(rgGangguan) },
                        { "faktor_lingkungan", FN.radioVal(rgFlingkungan) },
                        { "sedasi_anestesi", FN.radioVal(rgSedasiAnestesi) },
                        { "penggunaan_obat", FN.radioVal(rguseObat) },
                        { "skor", txScoreAnak.Text?.ToString() },
                        { "resiko", txResikoAnak.Text?.ToString() }
                    };
                    save = MD.UpdateData(ORADB.XE, "T1_RESIKO_JATUH_ANAK", "anamesa_id = " + anamesaID + " ", resiko_jatuh_anak);
                }
                else
                {
                    string sql = @"INSERT INTO T1_RESIKO_JATUH_ANAK (id, anamesa_id, usia, jenis_kelamin, diagnosis,  gangguan_fn_kognitif, faktor_lingkungan,
	                                sedasi_anestesi,penggunaan_obat,skor,resiko) VALUES (
                                    resiko_jatuh_seq.NEXTVAL,
                                    " + anamesaID + @",
                                    '"+ FN.radioVal(rgUsia) + @"',
                                    '"+ FN.radioVal(rgJenkel) + @"',
                                    '"+ FN.radioVal(rgDiagnosis) + @"',
                                    '"+ FN.radioVal(rgGangguan) + @"',
                                    '"+ FN.radioVal(rgFlingkungan) + @"',
                                    '"+ FN.radioVal(rgSedasiAnestesi) + @"',
                                    '"+ FN.radioVal(rguseObat) + @"',
                                    "+ txScoreAnak.Text?.ToString()+@",
                                    '"+ txResikoAnak.Text?.ToString() + @"' )";
                   save = ORADB.Execute(ORADB.XE, sql);
                }

            }else
            {
                DataTable dt = ConnOra.Data_Table_ora("select * from T1_RESIKO_JATUH_DEWASA where anamesa_id = " + anamesaID + " ");
                //ORADB.SetData(ORADB.XE, "select * from T1_RESIKO_JATUH_DEWASA where anamesa_id = " + anamesaID + " ");
                if (dt.Rows.Count > 0)
                {
                    Dictionary<string, string> resiko_jatuh_dewasa = new Dictionary<string, string>
                    {
                        { "riwayat_jatuh", FN.joinVal(rgRiwayatJatuh, txKet1) },
                        { "memiliki_lbh_penyakit", FN.joinVal(rgDiagnosaSekunder, txKet2) },
                        { "alat_bantu_jalan", FN.joinVal(rgAltBantuJalan, txKet3) },
                        { "terpasang_infus", FN.joinVal(rgInfus, txket4) },
                        { "gaya_berjalan", FN.joinVal(rgGayaJalan, txKet5) },
                        { "status_mental", FN.joinVal(rgstsMental, txket6) },
                        { "total_nilai", txTotalNilai.Text?.ToString() },
                        { "resiko", txResikoDewasa.Text?.ToString() },
                        { "tindakan", lblTindakan.Text?.ToString() }
                    };
                   save = MD.UpdateData(ORADB.XE, "T1_RESIKO_JATUH_DEWASA", "anamesa_id = " + anamesaID + " ", resiko_jatuh_dewasa);
                }
                else
                {
                    string sql = @"INSERT INTO T1_RESIKO_JATUH_DEWASA (id, anamesa_id, riwayat_jatuh, memiliki_lbh_penyakit, alat_bantu_jalan, terpasang_infus, 
                                    gaya_berjalan, status_mental, total_nilai, resiko, tindakan) VALUES (
                                     resiko_jatuh_seq.NEXTVAL,
                                    " + anamesaID + @",
                                    '"+ FN.joinVal(rgRiwayatJatuh, txKet1) + @"',
                                    '"+ FN.joinVal(rgDiagnosaSekunder, txKet2) + @"',
                                    '"+ FN.joinVal(rgAltBantuJalan, txKet3) + @"',
                                    '"+ FN.joinVal(rgInfus, txket4) + @"',
                                    '"+ FN.joinVal(rgGayaJalan, txKet5) + @"',
                                    '"+ FN.joinVal(rgstsMental, txket6) + @"',
                                    "+ txTotalNilai.Text?.ToString() + @",
                                    '"+ txResikoDewasa.Text?.ToString() + @"',
                                    '"+ lblTindakan.Text?.ToString() + @"') ";
                    save = ORADB.Execute(ORADB.XE, sql);
                }

            }

            if (save)
            {
                MessageBox.Show("Formulir Resiko Jatuh berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                FN.errosMsg("Simpan data gagal!", "Error");
            }

        }

        private void btnSaveCppt_Click(object sender, EventArgs e)
        {
            try
            {
                if(gvCppt.RowCount > 0)
                {
                    DataTable dt = ConnOra.Data_Table_ora("select * from T1_CPPT where anamesa_id =" + anamesaID + " ");
                    //ORADB.SetData(ORADB.XE, "select * from T1_CPPT where anamesa_id =" + anamesaID + " ");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ORADB.Execute(ORADB.XE, "delete from T1_CPPT where anamesa_id = " + anamesaID + " ");
                    }

                    string sql = "insert all ";
                    for (int i = 0; i < gvCppt.RowCount; i++)
                    {
                        string dte = "";
                        object tgl = gvCppt.GetRowCellValue(i,"TANGGAL");
                        if (tgl != null && tgl is DateTime)
                        {
                            DateTime selectedDateTime = (DateTime)tgl;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime selectedDateTime = DateTime.Now;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }

                        sql = sql + " into T1_CPPT (anamesa_id, tanggal, jam, kode_ppa, CTYPE, hasil_asesmen, instruksi, nama_terang, seq) values ( ";
                        sql = sql + " " + anamesaID + " ,";
                        sql = sql + " TO_DATE('"+ dte + "', 'yyyy-MM-dd') ,";
                        sql = sql + " '" + FN.strVal(gvCppt, i, "JAM") + "' ,";
                        sql = sql + " '" + FN.strVal(gvCppt, i, "KODE_PPA") + "' ,";
                        sql = sql + " '" + FN.strVal(gvCppt, i, "CTYPE") + "' ,";
                        sql = sql + " '" + FN.strVal(gvCppt, i, "HASIL_ASESMEN") + "' ,";
                        sql = sql + " '" + FN.strVal(gvCppt, i, "INSTRUKSI") + "' ,";
                        sql = sql + " '" + FN.strVal(gvCppt, i, "NAMA_TERANG") + "' ,";
                        sql = sql + " " + FN.strVal(gvCppt, i, "SEQ") + " ) ";
                    }
                    sql = sql + " select * from dual";
                    bool save = ORADB.Execute(ORADB.XE, sql);
                    if (save)
                    {
                        MessageBox.Show("Data Pelayanan Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }

            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
        }

        private void btnSimpanObat_Click(object sender, EventArgs e)
        {
            
            string kode = "", dosis = "", info = "", jumlah = "", id = "", stok = "", con = "", action = "", RECEIPT_ID="";
            string sql_cnt = "", med_cnt = "", sql_update = "", sql_diag = "", diag_cnt = "", harga = "", hari = "", jph = "", info_dosis = "";
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            //for (int i = 0; i < gvJadwalObat.DataRowCount; i++)
            //{
            //    id = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[0]).ToString();
            //    kode = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[1]).ToString();
            //    dosis = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[4]).ToString();
            //    info = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[5]).ToString();
            //    jumlah = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[7]).ToString();
            //    stok = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[6]).ToString();
            //    con = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[10]).ToString();
            //    action = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[9]).ToString();
            //    harga = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[12]).ToString();
            //    hari = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[11]).ToString();
            //    jph = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[13]).ToString();
            //    info_dosis = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[14]).ToString();

            //    if (con == "Y")
            //    {
            //        MessageBox.Show("Data tidak bisa dirubah.");
            //    }
            //    else if (stok == "0")
            //    {
            //        MessageBox.Show("Stok obat tidak tersedia.");
            //    }
            //    else if (jumlah == "" || jumlah == "0")
            //    {
            //        MessageBox.Show("Jumlah obat harus diisi.");
            //    }
            //    else if (Convert.ToInt16(jumlah) > Convert.ToInt16(stok))
            //    {
            //        MessageBox.Show("Jumlah melebihi stok");
            //    }
            //    else if (kode == "")
            //    {
            //        MessageBox.Show("Kode obat harus diisi.");
            //    }
            //    else if (dosis == "")
            //    {
            //        MessageBox.Show("Kode Dosis harus diisi.");
            //    }
            //    else if (hari == "")
            //    {
            //        MessageBox.Show("Jumlah harus diisi.");
            //    }
            //    else if (info == "")
            //    {
            //        MessageBox.Show("Info harus diisi.");
            //    }
            //    else if (info_dosis == "")
            //    {
            //        MessageBox.Show("Dosis harus diisi.");
            //    }
            //    else
            //    {
            //        int queue = 0;
            //        string tmp_queue = "", que = "", cnt = "";
            //        string sql_check = " select  nvl(max(to_number(substr(que02,2,3))),0) que from KLINIK.cs_visit where trunc(visit_date)= to_date('" + today + "','yyyy-MM-dd')   ";
            //        string sql_check2 = " select  count(0) cnt from KLINIK.cs_receipt where ID_VISIT = '" + visitid + "'  ";

            //        try
            //        {
            //            OleDbConnection oraConnecta = ConnOra.Create_Connect_Ora();
            //            OleDbDataAdapter adOraa = new OleDbDataAdapter(sql_check, oraConnecta);
            //            DataTable dta = new DataTable();
            //            adOraa.Fill(dta);

            //            tmp_queue = dta.Rows[0]["que"].ToString();
            //            queue = Convert.ToInt32(tmp_queue) + 1;
            //            que = queue.ToString();
            //            if (queue < 10)
            //            {
            //                que = que.PadLeft(que.Length + 2, '0');
            //            }
            //            else if (queue < 100)
            //            {
            //                que = que.PadLeft(que.Length + 1, '0');
            //            }

            //            OleDbConnection oraConnectb = ConnOra.Create_Connect_Ora();
            //            OleDbDataAdapter adOrab = new OleDbDataAdapter(sql_check2, oraConnectb);
            //            DataTable dtb = new DataTable();
            //            adOrab.Fill(dtb);
            //            cnt = dtb.Rows[0]["cnt"].ToString();

            //            if (cnt == "0")
            //            {
            //                sql_update = "";

            //                sql_update = sql_update + " update KLINIK.cs_visit" +
            //                                          " set que02 = 'R" + que + "', ";
            //                sql_update = sql_update + " upd_emp = '" + v_empid + "', upd_date = sysdate ";
            //                sql_update = sql_update + " where patient_no = '" + lMedNik.Text + "' and to_char(visit_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and que01 = '" + lMedQue.Text + "' ";

            //                try
            //                {
            //                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            //                    OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
            //                    oraConnect.Open();
            //                    cm.ExecuteNonQuery();
            //                    oraConnect.Close();
            //                    cm.Dispose();

            //                    //MessageBox.Show("Query Exec : " + sql_update);
            //                }
            //                catch (Exception ex)
            //                {
            //                    MessageBox.Show("ERROR: " + ex.Message);
            //                }
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("ERROR: " + ex.Message);
            //        }

            //        if (action == "I")
            //        {
            //            sql_diag = " select count(0) cnt from KLINIK.cs_diagnosa where to_char(insp_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and visit_no = '" + lMedQue.Text + "' and rm_no = '" + lMedRm.Text + "' ";
            //            OleDbConnection oraConnectd = ConnOra.Create_Connect_Ora();
            //            OleDbDataAdapter adOrad = new OleDbDataAdapter(sql_diag, oraConnectd);
            //            DataTable dtd = new DataTable();
            //            adOrad.Fill(dtd);
            //            diag_cnt = dtd.Rows[0]["cnt"].ToString();


            //            sql_cnt = " select count(0) cnt from KLINIK.cs_receipt where to_char(insp_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and visit_no = '" + lMedQue.Text + "' and rm_no = '" + lMedRm.Text + "' " + " and med_cd = '" + kode + "' ";
            //            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            //            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
            //            DataTable dt = new DataTable();
            //            adOra.Fill(dt);
            //            med_cnt = dt.Rows[0]["cnt"].ToString();

            //            if (Convert.ToInt32(med_cnt) > 0)
            //            {
            //                //MessageBox.Show("Gagal Disimpan.");
            //            }
            //            else if (diag_cnt == "0")
            //            {
            //                MessageBox.Show("Gagal Disimpan. Diagnosa belum diinput.");
            //            }
            //            else
            //            {
            //                OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
            //                OleDbCommand command = new OleDbCommand();
            //                OleDbTransaction trans = null;

            //                command.Connection = oraConnectTrans;
            //                oraConnectTrans.Open();

            //                try
            //                {
            //                    trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
            //                    command.Connection = oraConnectTrans;
            //                    command.Transaction = trans;

            //                    command.CommandText = " insert into KLINIK.cs_receipt (receipt_id, rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, visit_no, ins_date, ins_emp) " +
            //                                          " values(cs_receipt_seq.nextval, '" + lMedRm.Text + "', to_date('" + lMedDate.Text + "', 'yyyy-mm-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "', '" + lMedQue.Text + "', sysdate, '" + v_empid + "') ";
            //                    command.ExecuteNonQuery();

            //                    //command.CommandText = " update cs_visit set status = 'MED', time_inspection=sysdate, upd_emp = '" + v_empid + "', upd_date = sysdate where patient_no = '" + lMedNik.Text + "' and to_char(visit_date,'yyyy-mm-dd') = '" + lMedDate.Text + "' and que01 = '" + lMedQue.Text + "' ";
            //                    //command.ExecuteNonQuery();

            //                    trans.Commit();
            //                    //MessageBox.Show(sql_insert);
            //                    //MessageBox.Show("Query Exec : " + sql_insert);

            //                    MessageBox.Show("Data Berhasil disimpan.");
            //                }
            //                catch (Exception ex)
            //                {
            //                    trans.Rollback();
            //                    MessageBox.Show("ERROR: " + ex.Message);
            //                }

            //                oraConnectTrans.Close();
            //            }
            //        }
            //        else if (action == "U")
            //        {
            //            sql_update = "";

            //            sql_update = sql_update + " update KLINIK.cs_receipt" +
            //                                      " set med_cd = '" + kode + "', formula = '" + dosis + "', med_qty = '" + jumlah + "', type_drink = '" + info + "', " +
            //                                      "     price = '" + harga + "', days = '" + hari + "', qty_day = '" + jph + "', dosis = '" + info_dosis + "',";
            //            sql_update = sql_update + " upd_emp = '" + v_empid + "', upd_date = sysdate ";
            //            sql_update = sql_update + " where receipt_id = '" + id + "' and confirm='N' ";

            //            try
            //            {
            //                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            //                OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
            //                oraConnect.Open();
            //                cm.ExecuteNonQuery();
            //                oraConnect.Close();
            //                cm.Dispose();

            //                //MessageBox.Show("Query Exec : " + sql_update);
            //                LoadDataResep();
            //                MessageBox.Show("Data Berhasil diupdate");
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show("ERROR: " + ex.Message);
            //            }
            //        }
            //    }
            //}



            try
            {
                if (gvJadwalObat.RowCount > 0)
                { 

                    DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_receipt where ID_VISIT = '" + visitid + "'  ");
                    //ORADB.SetData(ORADB.XE, "Select * from KLINIK.cs_receipt where ID_VISIT = '" + visitid + "'  ");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        RECEIPT_ID = dt.Rows[0]["RECEIPT_ID"].ToString();
                        ORADB.Execute(ORADB.XE, " insert into KLINIK.cs_receipt_del select a.*, sysdate, '" + v_empid + "' as emp from KLINIK.cs_receipt a  where  ID_VISIT = '" + visitid + "'  and GRID_NAME = 'gvJadwalObat' ");
                        ORADB.Execute(ORADB.XE, " Delete from KLINIK.cs_receipt  where ID_VISIT = '" + visitid + "' and GRID_NAME = 'gvJadwalObat' ");
                    }

                    string sql = "insert all ";
                    for (int i = 0; i < gvJadwalObat.RowCount; i++)
                    {

                        //id = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[0]).ToString();
                        kode = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[1]).ToString();
                        dosis = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[4]).ToString();
                        info = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[5]).ToString();
                        jumlah = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[7]).ToString();
                        stok = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[6]).ToString();
                        con = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[10]).ToString();
                        action = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[9]).ToString();
                        harga = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[12]).ToString();
                        hari = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[11]).ToString();
                        jph = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[13]).ToString();
                        info_dosis = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[14]).ToString();

                        string dte = "";
                        object tgl = gvJadwalObat.GetRowCellValue(i, "TANGGAL");
                        if (tgl != null && tgl is DateTime)
                        {
                            DateTime selectedDateTime = (DateTime)tgl;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime selectedDateTime = DateTime.Now;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        if (RECEIPT_ID.ToString().Equals(""))
                        {
                            sql = sql + " into KLINIK.cs_receipt (receipt_id, rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, INS_JAM, ID_VISIT, ins_date, ins_emp, GRID_NAME) ";
                            sql = sql + " values(cs_receipt_seq.nextval, '" + RMNO + "', to_date('" + today + "', 'yyyy-MM-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', ";
                            sql = sql + "   '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "',   '" + FN.strVal(gvJadwalObat, i, "INS_JAM") + "' , '" + visitid + "',sysdate, '" + v_empid + "' , 'gvJadwalObat' ) ";
                        }
                        else
                        {
                            sql = sql + " into KLINIK.cs_receipt (receipt_id, rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, INS_JAM, ID_VISIT, ins_date, ins_emp, GRID_NAME) ";
                            sql = sql + " values("+ RECEIPT_ID.ToString() +", '" + RMNO + "', to_date('" + today + "', 'yyyy-MM-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', ";
                            sql = sql + "   '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "',   '" + FN.strVal(gvJadwalObat, i, "INS_JAM") + "' , '" + visitid + "',sysdate, '" + v_empid + "' , 'gvJadwalObat' ) ";
                        }
                       

                        //sql = sql + " into KLINIK.cs_receipt (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME) values ( ";
                        //sql = sql + " CS_TREATMENT_DETAIL_SEQ.nextval ,'" + FN.strVal(gvJadwalObat, i, "HEAD_ID") + "','" + FN.strVal(gvJadwalObat, i, "TREAT_ITEM_ID") + "'  ,";
                        //sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd'), '" + FN.strVal(gvJadwalObat, i, "TREAT_QTY") + "', '" + FN.strVal(gvJadwalObat, i, "TREAT_ITEM_PRICE") + "', " + Convert.ToInt32(FN.strVal(gvJadwalObat, i, "TREAT_QTY")) * Convert.ToInt32(FN.strVal(gvJadwalObat, i, "TREAT_ITEM_PRICE")) + ", ";
                        //sql = sql + " '" + FN.strVal(gvJadwalObat, i, "REMARKS") + "' ,  sysdate, '" + v_empid + "', '" + FN.strVal(gvJadwalObat, i, "JAM") + "' , 'gvMedis' )";
                    }
                    sql = sql + " select * from dual";
                    bool save = ORADB.Execute(ORADB.XE, sql);
                    if (save)
                    {
                        MessageBox.Show("Jadwal Pemberian Obat Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }


            //try
            //{
            //    if (gvJadwalObat.RowCount > 0)
            //    {
            //        DataTable dt = ORADB.SetData(ORADB.XE, "select * from T1_JADWAL_BERI_OBAT where anamesa_id =" + anamesaID + " ");
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            ORADB.Execute(ORADB.XE, "delete from T1_JADWAL_BERI_OBAT where anamesa_id = " + anamesaID + " ");
            //        }

            //        string sql = "insert all ";
            //        for (int i = 0; i < gvJadwalObat.RowCount; i++)
            //        {
            //            string dte = "";
            //            object tgl = gvJadwalObat.GetRowCellValue(i, "TANGGAL");
            //            if (tgl != null && tgl is DateTime)
            //            {
            //                DateTime selectedDateTime = (DateTime)tgl;
            //                dte = selectedDateTime.ToString("yyyy-MM-dd");
            //            }
            //            else
            //            {
            //                DateTime selectedDateTime = DateTime.Now;
            //                dte = selectedDateTime.ToString("yyyy-MM-dd");
            //            }

            //            sql = sql + " into T1_JADWAL_BERI_OBAT (anamesa_id, seq, jenis_obat, nama_obat, dosis, tanggal, jam1, jam2, jam3, jam4, EXTRA, ttd) values ( ";
            //            sql = sql + " " + anamesaID + " ,";
            //            sql = sql + " " + FN.strVal(gvJadwalObat, i, "SEQ") + " ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JENIS_OBAT") + "' ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "NAMA_OBAT") + "' ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "DOSIS") + "' ,";
            //            sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd') ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JAM1") + "' ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JAM2") + "' ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JAM3") + "' ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "JAM4") + "' ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "EXTRA") + "' ,";
            //            sql = sql + " '" + FN.strVal(gvJadwalObat, i, "TTD") + "' ) ";
            //        }
            //        sql = sql + " select * from dual";
            //        bool save = ORADB.Execute(ORADB.XE, sql);
            //        if (save)
            //        {
            //            MessageBox.Show("Jadwal Pemberian obat berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }

            //}
            //catch(Exception ex)
            //{
            //    FN.errosMsg(ex.Message, "Error");
            //}
        }

        private void txLain2_CheckedChanged(object sender, EventArgs e)
        {
            if (txLain2.Checked) txAlergi.Enabled = true;
            else txAlergi.Enabled = false;
        }

        private void chkEtc7_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEtc7.Checked) txNyeriHilang.Enabled = true;
            else
            {
                txNyeriHilang.Enabled = false;
                txNyeriHilang.Text = "";
            }
        }

        private void getScoreDewasa(object sender, EventArgs e)
        {
            int totalNilai = 0;

            if (rgRiwayatJatuh.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgRiwayatJatuh.EditValue);
            if (rgDiagnosaSekunder.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgDiagnosaSekunder.EditValue);
            if (rgAltBantuJalan.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgAltBantuJalan.EditValue);
            if (rgInfus.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgInfus.EditValue);
            if (rgGayaJalan.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgGayaJalan.EditValue);
            if (rgstsMental.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgstsMental.EditValue);
            
            txTotalNilai.Text = totalNilai.ToString();

            if (totalNilai < 25)
            {
                txResikoDewasa.Text = "TIDAK BERESIKO";
                lblTindakan.Text = "Perawatan Dasar";
            }
            else if (totalNilai < 51)
            {
                txResikoDewasa.Text = "RESIKO RENDAH";
                lblTindakan.Text = @"Pelaksanaan Intervensi Pencegahan Jatuh Standar";
            }
            else if (totalNilai >= 51)
            {
                txResikoDewasa.Text = "RESIKO TINGGI";
                lblTindakan.Text = @"Pelaksanaan Intervensi Pencegahan Jatuh Resiko Tinggi";
            }
        }

        private void getScoreAnak(object sender, EventArgs e)
        {
            int totalNilai = 0;

            if (rgUsia.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgUsia.EditValue);
            if (rgJenkel.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgJenkel.EditValue);
            if (rgDiagnosis.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgDiagnosis.EditValue);
            if (rgGangguan.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgGangguan.EditValue);
            if (rgFlingkungan.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgFlingkungan.EditValue);
            if (rgSedasiAnestesi.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgSedasiAnestesi.EditValue);
            if (rguseObat.SelectedIndex != -1) totalNilai += Convert.ToInt32(rguseObat.EditValue);

            txScoreAnak.Text = totalNilai.ToString();

            if (totalNilai < 12){
                txResikoAnak.Text = "RESIKO RENDAH";
            }
            else if (totalNilai >= 12){
                txResikoAnak.Text = "RESIKO TINGGI";
            }
        }

        private void getScoreScriningGizi(object sender, EventArgs e)
        {
            if (rgTurunBB.SelectedIndex == 2)
                lebrtbadan.Enabled = true;
            else
            {
                lebrtbadan.Enabled = false;
                lebrtbadan.SelectedText = "";
            }


            int totalNilai = 0;
            if(rgTurunBB.SelectedIndex == 2){
                totalNilai += Convert.ToInt32(lebrtbadan.EditValue.ToString()==""?"0": lebrtbadan.EditValue);
            }
            else{
                if (rgTurunBB.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgTurunBB.EditValue);
            }
            
            if (rgAsupanMakan.SelectedIndex != -1) totalNilai += Convert.ToInt32(rgAsupanMakan.EditValue);
            txScoreScrining.Text = totalNilai.ToString();
        }

        private void chkStsMental2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStsMental2.Checked)
                txStsMental2.Enabled = true;
            else
            {
                txStsMental2.Enabled = false;
                txStsMental2.Text = "";
            }


        }

        private void chkStasMental3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStasMental3.Checked)
                txStsMental3.Enabled = true;
            else
            {
                txStsMental3.Enabled = false;
                txStsMental3.Text = "";
            }
        }

        private void chkEtc12_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEtc12.Checked)
                txKbthnEdukasi.Enabled = true;
            else
            {
                txKbthnEdukasi.Enabled = false;
                txKbthnEdukasi.Text = "";
            }
        }

        private void chkSkalaNyeri_SelectedIndexChanged(object sender, EventArgs e)
        {
            int totalNilai = 0;
            int jumlahCheckboxDipilih = 0;

            for (int i = 0; i < chkSkalaNyeri.Items.Count; i++)
            {
                if (chkSkalaNyeri.GetItemChecked(i))
                {
                    int nilaiCheckbox = Convert.ToInt32(chkSkalaNyeri.Items[i]);
                    totalNilai += nilaiCheckbox;
                    jumlahCheckboxDipilih++;
                }
            }

            if (jumlahCheckboxDipilih > 0)
            {
                double rataRata = (double)totalNilai / jumlahCheckboxDipilih;
                txScorNyeri.Text = rataRata.ToString("0.##");
            }
            else
            {
                txScorNyeri.Text = "0";
            }
        }

        private void chkEtc14_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEtc14.Checked)
                txDiagnoseDtl.Enabled = true;
            else
            {
                txDiagnoseDtl.Enabled = false;
                txDiagnoseDtl.Text = "";
            }
        }

        private void setChart()
        {
            string sqlx = @"SELECT '[' || TO_CHAR (TANGGAL, 'yyyy-MM-dd') || ']' AS TANGGAL,
                                       SUBSTR (TENSI, 1, INSTR (TENSI, '/') - 1) AS SIS,
                                       SUBSTR (TENSI, INSTR (TENSI, '/') + 1) AS DIA,
                                       SUHU
                                  FROM T1_GRAFIK_VITAL WHERE ANAMESA_ID = " + anamesaID+"";
            DataTable dtChart = ConnOra.Data_Table_ora(sqlx); //ORADB.SetData(ORADB.XE, sqlx);
            createChart(dtSelect(dtChart, "TANGGAL", "SIS"), chrVital, 0);
            createChart(dtSelect(dtChart, "TANGGAL", "DIA"), chrVital, 1);
            createChart(dtSelect(dtChart, "TANGGAL", "SUHU"), chrVital, 2);
        }

        private void btnAddVt_Click(object sender, EventArgs e)
        {
            if (dtVital == null) return;

            DataRow newRow = dtVital.NewRow();

            newRow["SEQ"] = ((gvVt.RowCount) + 1).ToString();
            dtVital.Rows.Add(newRow);

            gcVt.DataSource = dtVital;
        }

        private void btnSaveV_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ConnOra.Data_Table_ora("select * from T1_GRAFIK_VITAL where anamesa_id = " + anamesaID + " ");
                //ORADB.SetData(ORADB.XE, "select * from T1_GRAFIK_VITAL where anamesa_id = " + anamesaID+" ");
                if (dt != null && dt.Rows.Count > 0)
                {
                    ORADB.Execute(ORADB.XE, "delete from T1_GRAFIK_VITAL where anamesa_id = " + anamesaID+" ");
                }

                string sql = "insert all ";
                for (int i = 0; i < gvVt.RowCount; i++)
                {
                    string dte = "";
                    object tgl = gvVt.GetRowCellValue(i,"TANGGAL");
                    if (tgl != null && tgl is DateTime)
                    {
                        DateTime selectedDateTime = (DateTime)tgl;
                        dte = selectedDateTime.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        DateTime selectedDateTime = DateTime.Now;
                        dte = selectedDateTime.ToString("yyyy-MM-dd");
                    }

                    sql = sql + " into T1_GRAFIK_VITAL (anamesa_id, tanggal, suhu, tensi, seq) values ( ";
                    sql = sql + " "+anamesaID+", ";
                    sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd'), ";
                    sql = sql + " '" + gvVt.GetRowCellDisplayText(i, "SUHU") + "', ";
                    sql = sql + " '" + gvVt.GetRowCellDisplayText(i, "TENSI") + "', ";
                    sql = sql + " " + gvVt.GetRowCellDisplayText(i, "SEQ") + " ) ";
                }

                sql = sql + " select * from dual ";
                bool save = ORADB.Execute(ORADB.XE, sql);
                if (save)
                {
                    MessageBox.Show("Berhasil disimpan", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    setChart();
                }
            }
            catch
            {

            }
        }

        private void createChart(DataTable dt, ChartControl chart, int sr)
        {
            int srsCount = chart.Series.Count;
            if (dt != null)
            {
                try
                {
                    if (srsCount > 0)
                    {
                        Series srs = chart.Series[sr];
                        addSeries(dt, srs);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                chart.Series[sr].Points.Clear();
            }
        }

        private void addSeries(DataTable dt, Series sr)
        {
            sr.Points.Clear();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    string lbl = dt.Rows[i][0].ToString();
                    string qty = dt.Rows[i][1].ToString();
                    sr.Points.Add(new SeriesPoint(lbl, Convert.ToDouble(qty)));
                }
                //sr.Label.TextPattern = "{V}"; /* "{A}:{V}~{VP:P0}";*/
            }
        }

        private DataTable dtSelect(DataTable dt, params string[] column)
        {
            if (dt.Rows.Count > 0)
            {
                DataView view = new DataView(dt);
                DataTable selected = view.ToTable("DGM", false, column);
                return selected;
            }
            else return null;

        }

        private void gcVt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                gvVt.DeleteRow(gvVt.FocusedRowHandle);
            }
        }

        private void gcJadwalObat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                gvJadwalObat.DeleteRow(gvJadwalObat.FocusedRowHandle);
            }
        }

        private void gcCppt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                gvCppt.DeleteRow(gvCppt.FocusedRowHandle);
            }
        }

        private void gcObtPlng_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                gvObtPlng.DeleteRow(gvObtPlng.FocusedRowHandle);
            }
        }

        private void chkDoc1_CheckedChanged(object sender, EventArgs e)
        {
            txDoc1.Enabled = chkDoc1.Checked;
            if (chkDoc1.Checked == false)
                txDoc1.Text = "";
        }

        private void chkDoc2_CheckedChanged(object sender, EventArgs e)
        {
            txDoc2.Enabled = chkDoc2.Checked;
            txDoc3.Enabled = chkDoc2.Checked;

            if (chkDoc2.Checked == false)
            {
                txDoc2.Text = "";
                txDoc3.Text = "";
            }

        }

        private void chkDoc3_CheckedChanged(object sender, EventArgs e)
        {
            txDoc4.Enabled = chkDoc3.Checked;
            if (chkDoc3.Checked == false)
                txDoc4.Text = "";
        }

        private void chkDoc4_CheckedChanged(object sender, EventArgs e)
        {
            txDoc5.Enabled = chkDoc4.Checked;
            if (chkDoc4.Checked == false)
                txDoc5.Text = "";
        }

        private void chkDoc5_CheckedChanged(object sender, EventArgs e)
        {
            txDoc6.Enabled = chkDoc5.Checked;
            if (chkDoc5.Checked == false)
                txDoc6.Text = "";
        }

        private void chkEtc5_CheckedChanged(object sender, EventArgs e)
        {
            txStsPsikologi.Enabled = chkEtc5.Checked;
            if (chkEtc5.Checked == false)
                txStsPsikologi.Text = "";
        }

        private void grdMain_DoubleClick(object sender, EventArgs e)
        {
            mainTab.Enabled = true;
            FN.ResetInput(mainTab);
            LoadItemLayanan();
            anamesaID = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "ANAMNESA_ID");
            visitid = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "ID_VISIT");
            headid = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "HEAD_ID");
            RMNO = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "RM_NO");
            pasienno = FN.strVal(gvwMain, gvwMain.FocusedRowHandle, "PATIENT_NO");

           // dtJadwalObat = ORADB.SetData(ORADB.XE, "select * from T1_JADWAL_BERI_OBAT where anamesa_id =" + anamesaID + "");
            dtCppt = ConnOra.Data_Table_ora("select * from T1_CPPT where anamesa_id =" + anamesaID + " ");
            dtObatPulang = ConnOra.Data_Table_ora("select * from T1_OBAT_PULANG where anamesa_id =" + anamesaID + " ");
            dtVital = ConnOra.Data_Table_ora("select * from T1_GRAFIK_VITAL where anamesa_id =" + anamesaID + " ");
            //ORADB.SetData(ORADB.XE, "select * from T1_CPPT where anamesa_id =" + anamesaID + " "); 
            //ORADB.SetData(ORADB.XE,  "select * from T1_OBAT_PULANG where anamesa_id =" + anamesaID + " ");  
            //ORADB.SetData(ORADB.XE, "select * from T1_GRAFIK_VITAL where anamesa_id =" + anamesaID + " ");

            try
            {
                if (ConnOra.Data_Table_ora("select * from T1_RAWAT_INAP1 where anamesa_id =" + anamesaID + " ").Rows.Count > 0)
                //if (ORADB.SetData(ORADB.XE, "select * from T1_RAWAT_INAP1 where anamesa_id = " + anamesaID + "").Rows.Count > 0)
                {
                    getData(anamesaID);
                }
                else
                {
                    string newId = ORADB.getData(ORADB.XE, "select rawat_inap_seq.NEXTVAL new_id from dual ", "NEW_ID");
                    string newId2 = ORADB.getData(ORADB.XE, "select resiko_jatuh_seq.NEXTVAL new_id from dual ", "NEW_ID");
                    List<string> sql = new List<string>();
                    sql.Add("insert into T1_RAWAT_INAP1 (id, anamesa_id) values (" + newId + "," + anamesaID + ")");
                    sql.Add("insert into T1_RAWAT_INAP2 (id, anamesa_id) values (" + newId + "," + anamesaID + ")");
                    sql.Add("insert into T1_PERENCANAAN_PULANG (id, anamesa_id) values (" + newId + "," + anamesaID + ")");
                    sql.Add("insert into T1_RESUME_PULANG (id, anamesa_id) values (" + newId + "," + anamesaID + ")");
                    sql.Add("insert into T1_ASESMEN_GIZI (anamesa_id) values (" + anamesaID + ")");
                    ORADB.DbTrans(ORADB.XE, sql);
                }
                btnInputData.Enabled = false;
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
        }

        private void gvwMain_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void baddmedis_Click(object sender, EventArgs e)
        {
            if (dtMedis == null) return;

            DataRow newRow = dtMedis.NewRow();

            newRow["SEQ"] = ((gvMedis.RowCount) + 1).ToString();
            newRow["HEAD_ID"] = headid;
            newRow["ID_VISIT"] = visitid;
            newRow["ACTION"] = "I";
            dtMedis.Rows.Add(newRow);

            gridMedis.DataSource = dtMedis;
        }

        private void baddnone_Click(object sender, EventArgs e)
        {
            if (dtVisitDokter == null) return;

            DataRow newRow = dtVisitDokter.NewRow();

            newRow["SEQ"] = ((gvVisitDoc.RowCount) + 1).ToString();
            newRow["HEAD_ID"] = headid;
            newRow["ID_VISIT"] = visitid;
            newRow["ACTION"] = "I";
            dtVisitDokter.Rows.Add(newRow);

            gridVisitDoc.DataSource = dtVisitDokter;
        }

        private void bdelmedis_Click(object sender, EventArgs e)
        {

        } 

        private void bdelnone_Click(object sender, EventArgs e)
        {

        }

        private void bsavenone_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvVisitDoc.RowCount > 0)
                {
                    DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_treatment_detail where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvVisitDoc' ");
                    //ORADB.SetData(ORADB.XE, "Select * from KLINIK.cs_treatment_detail where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvVisitDoc' ");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ORADB.Execute(ORADB.XE, " insert into KLINIK.cs_treatment_detail_del select a.*, sysdate, '" + v_empid + "' as emp from KLINIK.cs_treatment_detail a  where  HEAD_ID = '" + headid + "'  and GRID_NAME = 'gvVisitDoc' ");
                        ORADB.Execute(ORADB.XE, " Delete from KLINIK.cs_treatment_detail  where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvVisitDoc' ");
                    }

                    string sql = "insert all ";
                    for (int i = 0; i < gvVisitDoc.RowCount; i++)
                    {
                        string dte = "";
                        object tgl = gvVisitDoc.GetRowCellValue(i, "TANGGAL");
                        if (tgl != null && tgl is DateTime)
                        {
                            DateTime selectedDateTime = (DateTime)tgl;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime selectedDateTime = DateTime.Now;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }

                        //                    command.CommandText = " insert into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp) values
                        //  ( '" + seq_val + "', '" + head + "', '" + nama_laya + "', to_date('" + ldate + "', 'yyyy-MM-dd'), " + qty + ", " + item_price + ", " + price + ", '" + remarks + "', sysdate, '" + v_empid + "') ";
                        //                    command.ExecuteNonQuery();

                        sql = sql + " into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME, ID_DOKTER) values ( ";
                        sql = sql + " CS_TREATMENT_DETAIL_SEQ.nextval ,'" + FN.strVal(gvVisitDoc, i, "HEAD_ID") + "','" + FN.strVal(gvVisitDoc, i, "TREAT_ITEM_ID") + "'  ,";
                        sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd'), '" + FN.strVal(gvVisitDoc, i, "TREAT_QTY") + "', '" + FN.strVal(gvVisitDoc, i, "TREAT_ITEM_PRICE") + "', " + Convert.ToInt32(FN.strVal(gvVisitDoc, i, "TREAT_QTY")) * Convert.ToInt32(FN.strVal(gvVisitDoc, i, "TREAT_ITEM_PRICE")) + ", ";
                        sql = sql + " '" + FN.strVal(gvVisitDoc, i, "REMARKS") + "' ,  sysdate, '" + v_empid + "', '" + FN.strVal(gvVisitDoc, i, "JAM") + "' , 'gvVisitDoc' , '" + FN.strVal(gvVisitDoc, i, "ID_DOKTER") + "' )";
                    }
                    sql = sql + " select * from dual";
                    bool save = ORADB.Execute(ORADB.XE, sql);
                    if (save)
                    {
                        MessageBox.Show("Data Kunjungan Dokter Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } 
                } 
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
        }

        private void gvVisitDoc_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            string a = "", tmp_stat = "", date = "", que = "", rm_no = "", no_visit = "";
               
            if (e.Column.Caption == "Nama Pelayanan")
            {
                a = view.GetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_ID"]).ToString();
                no_visit = view.GetRowCellValue(e.RowHandle, view.Columns["ID_VISIT"]).ToString();
                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();

                string sql_ = "", sql_head = "", group_id = "", price = "", head_id = "", stbyr = "";
                sql_ = " select treat_group_id, treat_item_price from KLINIK.cs_treatment_item where treat_item_id = " + a + " ";

                DataTable dt0 = ConnOra.Data_Table_ora(sql_);

                //OleDbConnection oraConnect0 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_, oraConnect0);
                //DataTable dt0 = new DataTable();
                //adOra0.Fill(dt0);
                if (dt0.Rows.Count > 0)
                {
                    group_id = dt0.Rows[0]["TREAT_GROUP_ID"].ToString();
                    price = dt0.Rows[0]["TREAT_ITEM_PRICE"].ToString();
                }

                sql_head = " select head_id, pay_status from KLINIK.cs_treatment_head where ID_VISIT = '" + visitid + "'  ";
                DataTable dt1 = ConnOra.Data_Table_ora(sql_head);
                //OleDbConnection oraConnect1 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra1 = new OleDbDataAdapter(sql_head, oraConnect1);
                //DataTable dt1 = new DataTable();
                //adOra1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    head_id = dt1.Rows[0]["HEAD_ID"].ToString();
                    stbyr = dt1.Rows[0]["PAY_STATUS"].ToString();
                }

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns["HEAD_ID"], head_id);
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_GROUP_ID"], group_id);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[2], a);
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_QTY"], "1");
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_PRICE"], price);
                    view.SetRowCellValue(e.RowHandle, view.Columns["PAY_STATUS"], stbyr);
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
                }

            }

            if (e.Column.Caption == "Note Visit")
            {
                string tmp_stat2 = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                if (tmp_stat2 == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
                }
            }
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gvJadwalObat_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
             
            view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
        }

        private void gvJadwalObat_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Kode Dosis" || e.Column.Caption == "Dosis" || e.Column.Caption == "Info" || e.Column.Caption == "Jml")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }

            if (e.Column.Caption == "Stok")
            {
                string stok = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);

                if (stok != "")
                {
                    if (Convert.ToInt16(stok) == 0)
                    {
                        e.Appearance.BackColor = Color.Crimson;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (Convert.ToInt16(stok) <= 20)
                    {
                        e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }

            }

            if (e.Column.Caption == "Confirm")
            {
                string con = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);

                if (con == "Y")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
        }

        private void gvJadwalObat_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {

            string kode = "", dosis = "", info = "", jumlah = "", id = "", stok = "", con = "", action = "", info_cara ="", RECEIPT_ID = "";
            string sql_cnt = "", med_cnt = "", sql_update = "", sql_diag = "", diag_cnt = "", harga = "", hari = "", jph = "", info_dosis = "";
            string today = DateTime.Now.ToString("yyyy-MM-dd");
              
            try
            {
                if (gvObtPlng.RowCount > 0)
                {

                    DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_receipt where ID_VISIT = '" + visitid + "'  ");
                    //ORADB.SetData(ORADB.XE, "Select * from KLINIK.cs_receipt where ID_VISIT = '" + visitid + "'  ");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        RECEIPT_ID = dt.Rows[0]["RECEIPT_ID"].ToString();
                    }

                    string sql = "insert all  ";
                    for (int i = 0; i < gvObtPlng.RowCount; i++)
                    { 
                        //id = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[0]).ToString();
                        kode = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[1]).ToString();
                        dosis = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[4]).ToString();
                        info = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[5]).ToString();
                        jumlah = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[7]).ToString();
                        stok = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[6]).ToString();
                        con = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[10]).ToString();
                        action = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[9]).ToString();
                        harga = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[12]).ToString();
                        hari = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[11]).ToString();
                        jph = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[13]).ToString();
                        info_dosis = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[14]).ToString();
                        info_cara = gvObtPlng.GetRowCellValue(i, gvObtPlng.Columns[21]).ToString();

                        string dte = "";
                        object tgl = gvObtPlng.GetRowCellValue(i, "TANGGAL");
                        if (tgl != null && tgl is DateTime)
                        {
                            DateTime selectedDateTime = (DateTime)tgl;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime selectedDateTime = DateTime.Now;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                         
                        DataTable dt2 = ConnOra.Data_Table_ora("Select * from KLINIK.cs_receipt where ID_VISIT = '" + visitid + "'  and med_cd =  '" + kode + "' and confirm = '" + con + "' and formula =  '" + dosis + "' and GRID_NAME = 'gvObtPlng' ");
                        //ORADB.SetData(ORADB.XE, "Select * from KLINIK.cs_receipt where ID_VISIT = '" + visitid + "'  and med_cd =  '" + kode + "' and confirm = '" + con + "' and formula =  '" + dosis + "' and GRID_NAME = 'gvObtPlng' ");
                        if (dt2 == null || dt2.Rows.Count == 0)
                        { 
                            if (RECEIPT_ID.ToString().Equals(""))
                            {
                                sql = sql + " into KLINIK.cs_receipt (receipt_id, rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, INS_JAM, ID_VISIT, ins_date, ins_emp, GRID_NAME, CARA, FLAG_PULANG) ";
                                sql = sql + " values(cs_receipt_seq.nextval, '" + RMNO + "', to_date('" + today + "', 'yyyy-MM-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', ";
                                sql = sql + "   '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "',   '" + FN.strVal(gvObtPlng, i, "INS_JAM") + "' , '" + visitid + "',sysdate, '" + v_empid + "' , 'gvObtPlng', '" + info_cara + "','Y' ) ";
                            }
                            else
                            {
                                sql = sql + " into KLINIK.cs_receipt (receipt_id, rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, INS_JAM, ID_VISIT, ins_date, ins_emp, GRID_NAME, CARA, FLAG_PULANG) ";
                                sql = sql + " values(" + RECEIPT_ID + ", '" + RMNO + "', to_date('" + today + "', 'yyyy-MM-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', ";
                                sql = sql + "   '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "',   '" + FN.strVal(gvObtPlng, i, "INS_JAM") + "' , '" + visitid + "',sysdate, '" + v_empid + "' , 'gvObtPlng', '" + info_cara + "','Y' ) ";
                            }
                        }

                        //sql = sql + " into KLINIK.cs_receipt (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME) values ( ";
                        //sql = sql + " CS_TREATMENT_DETAIL_SEQ.nextval ,'" + FN.strVal(gvObtPlng, i, "HEAD_ID") + "','" + FN.strVal(gvObtPlng, i, "TREAT_ITEM_ID") + "'  ,";
                        //sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd'), '" + FN.strVal(gvObtPlng, i, "TREAT_QTY") + "', '" + FN.strVal(gvObtPlng, i, "TREAT_ITEM_PRICE") + "', " + Convert.ToInt32(FN.strVal(gvObtPlng, i, "TREAT_QTY")) * Convert.ToInt32(FN.strVal(gvObtPlng, i, "TREAT_ITEM_PRICE")) + ", ";
                        //sql = sql + " '" + FN.strVal(gvObtPlng, i, "REMARKS") + "' ,  sysdate, '" + v_empid + "', '" + FN.strVal(gvObtPlng, i, "JAM") + "' , 'gvMedis' )";
                    }
                    sql = sql + " select * from dual";
                    bool save = ORADB.Execute(ORADB.XE, sql);
                    if (save)
                    {
                        MessageBox.Show("Jadwal Pemberian Obat Pulang Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }

        }

        private void gvObtPlng_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
        }

        private void gvObtPlng_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvObtPlng_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Kode Dosis" || e.Column.Caption == "Dosis" || e.Column.Caption == "Info" || e.Column.Caption == "Jml")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }

            if (e.Column.Caption == "Stok")
            {
                string stok = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);

                if (stok != "")
                {
                    if (Convert.ToInt16(stok) == 0)
                    {
                        e.Appearance.BackColor = Color.Crimson;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (Convert.ToInt16(stok) <= 20)
                    {
                        e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }

            }

            if (e.Column.Caption == "Confirm")
            {
                string con = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);

                if (con == "Y")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
        }

        private void gvObtPlng_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();

            if (a.ToString().Equals(""))
                return;

            string dte = "";

            DateTime selectedDateTime = DateTime.Now;
            dte = selectedDateTime.ToString("yyyy-MM-dd");

            if (e.Column.Caption == "Nama Obat" && (a.Substring(0, 2) == "BP" || a.Substring(0, 2) == "UM"))
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                string sql_medcd = " ", sql_med = "", med_cd = "", med_name = "", med_group = "", med_stok = "", med_uom = "", cek_stok = "", sql_for = "";
                 
                sql_medcd = " select " +
                            " max(klinik.FN_CS_INIT_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') +  " +
                            " klinik.FN_CS_TRX_IN(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') -  " +
                            " klinik.FN_CS_TRX_OUT(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') - " +
                            " klinik.FN_CS_REQ_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "')) stock from dual ";

                datstock = koneksi.GetDataTable(sql_medcd);

                if (datstock.Rows.Count > 0)
                    cek_stok = datstock.Rows[0]["stock"].ToString();
                else
                    cek_stok = "0"; 

                sql_med = " select med_cd, initcap(med_name) med_name, med_group, '" + cek_stok + "' stock, initcap(uom) uom " + 
                          " from KLINIK.cs_medicine a  " +
                          " where status = 'A'  " +
                          " and med_cd = '" + a + "' ";

                DataTable dt = koneksi.GetDataTable(sql_med);

                //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra = new OleDbDataAdapter(sql_med, oraConnect);
                //DataTable dt = new DataTable();
                //adOra.Fill(dt);

                med_cd = dt.Rows[0]["med_cd"].ToString();
                med_name = dt.Rows[0]["med_name"].ToString();
                med_group = dt.Rows[0]["med_group"].ToString();
                med_stok = dt.Rows[0]["stock"].ToString();
                med_uom = dt.Rows[0]["uom"].ToString();

                sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1  and  b.med_cd = '" + med_cd + "' ";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for);
                //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
                //DataTable dtf = new DataTable();
                //adOraf.Fill(dtf);
                listFormula.Clear();
                listFormula2.Clear();
                for (int i = 0; i < dtf.Rows.Count; i++)
                {
                    listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                }

                //view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[4], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[3], med_name);
                    view.SetRowCellValue(e.RowHandle, view.Columns[2], med_group);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[7], "0");
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
            }

            if (e.Column.Caption == "Formula")
            {
                string medicine_cd = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string formula_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string reg_dt = dte.ToString();
                string rm = dte.ToString();
                string que = dte.ToString();
                string stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                string kode = "", sql_pilihan = "";

                if (stat == "I")
                {
                    //view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                }
                else
                {
                    sql_pilihan = " select med_cd from KLINIK.cs_formula where formula_id = '" + formula_cd + "' ";
                    DataTable dtf = ConnOra.Data_Table_ora(sql_pilihan);
                    //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                    //OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_pilihan, oraConnectf);
                    //DataTable dtf = new DataTable();
                    //adOraf.Fill(dtf);

                    if (dtf.Rows.Count > 0)
                    {
                        kode = dtf.Rows[0]["med_cd"].ToString();

                    }
                    else
                    {
                        kode = "";
                    }

                    if (kode == medicine_cd)
                    {
                        view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                        view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                    }
                    else
                    {
                        MessageBox.Show("Kode Formula tidak valid");
                        return;
                        //LoadDataResep();
                    }
                } 
            }

            if (e.Column.Caption == "Qty")
            {
                string sql_for = "", med_price = "", qty = "", tmp_stat = "";
                string for_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string tmp_hari = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                int tot_hari = 0, tot_harga = 0;

                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                sql_for = " select med_price, qty from KLINIK.cs_formula where formula_id = '" + for_cd + "' ";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for);

                //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
                //DataTable dtf = new DataTable();
                //adOraf.Fill(dtf);

                if (dtf.Rows.Count > 0)
                {
                    med_price = dtf.Rows[0]["med_price"].ToString();
                    qty = dtf.Rows[0]["qty"].ToString();
                }
                else
                {
                    med_price = "0";
                    qty = "0";
                }

                if (tmp_hari == "")
                {
                    tmp_hari = "1";
                }

                tot_hari = Convert.ToInt16(tmp_hari); //Convert.ToInt16(tmp_hari) * Convert.ToInt16(qty);
                tot_harga = Convert.ToInt16(med_price); //Convert.ToInt16(tmp_hari) *

                if (tmp_stat == "I")
                {
                    //view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                }
                else
                {
                    //view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                }
            }
        }

        private void txTbPb_TextChanged(object sender, EventArgs e)
        {
            if (txBB.Text == "" || txBB.Text == string.Empty)
                return;
            else if (txTbPb.Text == "" || txTbPb.Text == string.Empty)
                return;
            else if (Convert.ToInt32(txBB.Text) < 0)
                return;
            else if (Convert.ToInt32(txTbPb.Text) < 0)
                return; 
            else
                txImt.Text = ((Convert.ToDouble(txBB.Text) / (Convert.ToDouble(txTbPb.Text) * Convert.ToDouble(txTbPb.Text))) * 10000).ToString("0.00");

            if (Convert.ToDouble(txImt.Text) < 18.5)
                txGstKet.Text = "Berat Badan Kurang";
            else if (Convert.ToDouble(txImt.Text) >= 18.5 && Convert.ToDouble(txImt.Text) < 23)
                txGstKet.Text = "Berat Badan Normal";
            else if (Convert.ToDouble(txImt.Text) >= 23 && Convert.ToDouble(txImt.Text) < 25)
                txGstKet.Text = "Kelebihan Berat Badan";
            else if (Convert.ToDouble(txImt.Text) >= 25 && Convert.ToDouble(txImt.Text) < 30)
                txGstKet.Text = "Obesitas 1";
            else if (Convert.ToDouble(txImt.Text) >= 30)
                txGstKet.Text = "Obesitas 2";
            else
                txGstKet.Text = "Tidak Terklasifikasi";
             
        }

        private void gvJadwalObat_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();

            if (a.ToString().Equals(""))
                return;

            string dte = "";
            
            DateTime selectedDateTime = DateTime.Now;
            dte = selectedDateTime.ToString("yyyy-MM-dd");

            if (e.Column.Caption == "Nama Obat" && (a.Substring(0, 2) == "BP" || a.Substring(0, 2) == "UM"))
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                string sql_medcd = " ", sql_med = "", med_cd = "", med_name = "", med_group = "", med_stok = "", med_uom = "", cek_stok = "", sql_for = "";

                //dtStock = null;
                sql_medcd = " select " +
                            " max(klinik.FN_CS_INIT_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') +  " +
                            " klinik.FN_CS_TRX_IN(to_date('" + dte.ToString()  + "','yyyy-mm-dd'),'" + a + "') -  " +
                            " klinik.FN_CS_TRX_OUT(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') - " +
                            " klinik.FN_CS_REQ_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "')) stock from dual ";

                datstock = koneksi.GetDataTable(sql_medcd);

                if (datstock.Rows.Count > 0)
                    cek_stok = datstock.Rows[0]["stock"].ToString();
                else
                    cek_stok = "0";
                //cb_ada_tindakan.Checked = functionChk(dataTable3.Rows[0]["ada_tindakan"].ToString(), "Ya");


                //    OleDbConnection oraConn = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_medcd, oraConn);
                ////dtStock.Clear();
                //adOra0.Fill(dtStock);
                //cek_stok = dtStock.Rows[0]["stock"].ToString();

                sql_med = " select med_cd, initcap(med_name) med_name, med_group, '" + cek_stok + "' stock, initcap(uom) uom " +
                          //" stock - (select nvl(SUM(med_qty),0) from cs_receipt  " +
                          //"           where TO_CHAR(insp_date, 'yyyy-mm-dd') = '" + lMedDate.Text + "'  " +
                          //"             and confirm = 'N'  " +
                          //"             and med_cd = a.med_cd) stock, uom  " +
                          //" klinik.FN_CS_INIT_STOCK(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'"+ medcd + "') +  " +
                          //" klinik.FN_CS_TRX_IN(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + medcd + "') -  " +
                          //" klinik.FN_CS_TRX_OUT(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + medcd + "') - " +
                          //" klinik.FN_CS_REQ_STOCK(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + medcd + "') stock, uom " +
                          " from KLINIK.cs_medicine a  " +
                          " where status = 'A'  " +
                          " and med_cd = '" + a + "' ";

                DataTable dt = ConnOra.Data_Table_ora(sql_med);

                //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra = new OleDbDataAdapter(sql_med, oraConnect);
                //DataTable dt = new DataTable();
                //adOra.Fill(dt);

                med_cd = dt.Rows[0]["med_cd"].ToString();
                med_name = dt.Rows[0]["med_name"].ToString();
                med_group = dt.Rows[0]["med_group"].ToString();
                med_stok = dt.Rows[0]["stock"].ToString();
                med_uom = dt.Rows[0]["uom"].ToString();

                sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1  and  b.med_cd = '" + med_cd + "' ";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for);

                //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
                //DataTable dtf = new DataTable();
                //adOraf.Fill(dtf);
                listFormula.Clear();
                listFormula2.Clear();
                for (int i = 0; i < dtf.Rows.Count; i++)
                {
                    listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                }

                //view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[4], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[3], med_name);
                    view.SetRowCellValue(e.RowHandle, view.Columns[2], med_group);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[7], "0");
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
            }

            if (e.Column.Caption == "Formula")
            {
                string medicine_cd = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string formula_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string reg_dt = dte.ToString();
                string rm = dte.ToString();
                string que = dte.ToString();
                string stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                string kode = "", sql_pilihan = "";

                if (stat == "I")
                {
                    //view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                }
                else
                {
                    sql_pilihan = " select med_cd from KLINIK.cs_formula where formula_id = '" + formula_cd + "' ";
                    DataTable dtf = ConnOra.Data_Table_ora(sql_pilihan);

                    //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                    //OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_pilihan, oraConnectf);
                    //DataTable dtf = new DataTable();
                    //adOraf.Fill(dtf);

                    if (dtf.Rows.Count > 0)
                    {
                        kode = dtf.Rows[0]["med_cd"].ToString();

                    }
                    else
                    {
                        kode = "";
                    }

                    if (kode == medicine_cd)
                    {
                        view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                        view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                    }
                    else
                    {
                        MessageBox.Show("Kode Formula tidak valid");
                        return;
                        //LoadDataResep();
                    }
                }


            }

            if (e.Column.Caption == "Qty")
            {
                string sql_for = "", med_price = "", qty = "", tmp_stat = "";
                string for_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string tmp_hari = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                int tot_hari = 0, tot_harga = 0;

                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                sql_for = " select med_price, qty from KLINIK.cs_formula where formula_id = '" + for_cd + "' ";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for);

                //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
                //DataTable dtf = new DataTable();
                //adOraf.Fill(dtf);

                if (dtf.Rows.Count > 0)
                {
                    med_price = dtf.Rows[0]["med_price"].ToString();
                    qty = dtf.Rows[0]["qty"].ToString();
                }
                else
                {
                    med_price = "0";
                    qty = "0";
                }

                if (tmp_hari == "")
                {
                    tmp_hari = "1";
                }

                tot_hari = Convert.ToInt16(tmp_hari); //Convert.ToInt16(tmp_hari) * Convert.ToInt16(qty);
                tot_harga =  Convert.ToInt16(med_price); //Convert.ToInt16(tmp_hari) *

                if (tmp_stat == "I")
                {
                    //view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                }
                else
                {
                    //view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                }
            }

            //if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Info" || e.Column.Caption == "Dosis")
            //{
            //    string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();

            //    if (tmp_stat == "I")
            //    {
            //        view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
            //    }
            //    else
            //    {
            //        view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
            //    }
            //}
        }

        private void gvMedis_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            string a = "", tmp_stat = "", date = "", que = "", rm_no = "", no_visit = "";

            //date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            //que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            //rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString(); 

            if (e.Column.Caption == "Nama Pelayanan" )
            {
                a = view.GetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_ID"]).ToString();
                no_visit = view.GetRowCellValue(e.RowHandle, view.Columns["ID_VISIT"]).ToString();
                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();

                string sql_ = "", sql_head = "", group_id = "", price = "", head_id = "", stbyr = "";
                sql_ = " select treat_group_id, treat_item_price from KLINIK.cs_treatment_item where treat_item_id = " + a + " ";
                DataTable dt0 = ConnOra.Data_Table_ora(sql_);

                //OleDbConnection oraConnect0 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_, oraConnect0);
                //DataTable dt0 = new DataTable();
                //adOra0.Fill(dt0);
                if (dt0.Rows.Count > 0)
                {
                    group_id = dt0.Rows[0]["TREAT_GROUP_ID"].ToString();
                    price = dt0.Rows[0]["TREAT_ITEM_PRICE"].ToString();
                }

                sql_head = " select head_id, pay_status from KLINIK.cs_treatment_head where ID_VISIT = '" + visitid + "'  ";
                DataTable dt1 = ConnOra.Data_Table_ora(sql_head);
                //OleDbConnection oraConnect1 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra1 = new OleDbDataAdapter(sql_head, oraConnect1);
                //DataTable dt1 = new DataTable();
                //adOra1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    head_id = dt1.Rows[0]["HEAD_ID"].ToString();
                    stbyr = dt1.Rows[0]["PAY_STATUS"].ToString();
                }

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns["HEAD_ID"], head_id);
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_GROUP_ID"], group_id);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[2], a);
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_QTY"], "1");
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_PRICE"], price);
                    view.SetRowCellValue(e.RowHandle, view.Columns["PAY_STATUS"], stbyr);
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
                } 
            }

            if (e.Column.Caption == "Remark")
            {
                string tmp_stat2 = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                if (tmp_stat2 == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
                }
            }
        }

        private void dtDataPelayanan()
        {
            string sql_tind_load = "";

            sql_tind_load = sql_tind_load + Environment.NewLine + "select b.detail_id, c.treat_group_id, b.treat_item_id, b.treat_qty, b.total_price, ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "b.remarks, 'S' action, a.head_id, to_char(b.treat_date,'yyyy-MM-dd') treat_date, a.pay_status, b.treat_item_price ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "from KLINIK.cs_treatment_head a ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "join KLINIK.cs_treatment_detail b on (a.head_id=b.head_id) ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "join KLINIK.cs_treatment_item c on (b.treat_item_id=c.treat_item_id) ";
            //sql_tind_load = sql_tind_load + Environment.NewLine + "where rm_no='" + pub_rm_no + "' ";
            //sql_tind_load = sql_tind_load + Environment.NewLine + "and to_char(visit_date,'yyyy-MM-dd')='" + pub_reg_date + "' ";
            //sql_tind_load = sql_tind_load + Environment.NewLine + "and visit_no='" + pub_que + "' ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "and a.status='OPN' ";
            DataTable dtPmedis = ConnOra.Data_Table_ora(sql_tind_load);
            //DataTable dtPmedis = ORADB.SetData(ORADB.XE, sql_tind_load);
            gridMedis.DataSource = null;
            gvMedis.Columns.Clear();
            gridMedis.DataSource = dtPmedis;

            gvMedis.OptionsView.ColumnAutoWidth = true;
            gvMedis.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gvMedis.Appearance.HeaderPanel.FontSizeDelta = 0;
            gvMedis.IndicatorWidth = 30; 
            gvMedis.BestFitColumns();

        }

        private void bsavemedis_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvMedis.RowCount > 0)
                {
                    DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_treatment_detail where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvMedis' ");
                    //ORADB.SetData(ORADB.XE, "Select * from KLINIK.cs_treatment_detail where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvMedis' ");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ORADB.Execute(ORADB.XE, " insert into KLINIK.cs_treatment_detail_del select a.*, sysdate, '" + v_empid + "' as emp from KLINIK.cs_treatment_detail a  where  HEAD_ID = '" + headid + "'  and GRID_NAME = 'gvMedis' ");
                        ORADB.Execute(ORADB.XE, " Delete from KLINIK.cs_treatment_detail  where HEAD_ID = '" + headid + "' and GRID_NAME = 'gvMedis' ");
                    }

                    string sql = "insert all ";
                    for (int i = 0; i < gvMedis.RowCount; i++)
                    {
                        string dte = "";
                        object tgl = gvMedis.GetRowCellValue(i, "TANGGAL");
                        if (tgl != null && tgl is DateTime)
                        {
                            DateTime selectedDateTime = (DateTime)tgl;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime selectedDateTime = DateTime.Now;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }

                        //                    command.CommandText = " insert into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp) values
                        //  ( '" + seq_val + "', '" + head + "', '" + nama_laya + "', to_date('" + ldate + "', 'yyyy-MM-dd'), " + qty + ", " + item_price + ", " + price + ", '" + remarks + "', sysdate, '" + v_empid + "') ";
                        //                    command.ExecuteNonQuery();

                        sql = sql + " into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME) values ( ";
                        sql = sql + " CS_TREATMENT_DETAIL_SEQ.nextval ,'" + FN.strVal(gvMedis, i, "HEAD_ID") + "','" + FN.strVal(gvMedis, i, "TREAT_ITEM_ID") + "'  ,";
                        sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd'), '" + FN.strVal(gvMedis, i, "TREAT_QTY") + "', '" + FN.strVal(gvMedis, i, "TREAT_ITEM_PRICE") + "', " + Convert.ToInt32(FN.strVal(gvMedis, i, "TREAT_QTY")) * Convert.ToInt32(FN.strVal(gvMedis, i, "TREAT_ITEM_PRICE")) + ", ";
                        sql = sql + " '" + FN.strVal(gvMedis, i, "REMARKS") + "' ,  sysdate, '" + v_empid + "', '" + FN.strVal(gvMedis, i, "JAM") + "' , 'gvMedis' )"; 
                    }
                    sql = sql + " select * from dual";
                    bool save = ORADB.Execute(ORADB.XE, sql);
                    if (save)
                    {
                        MessageBox.Show("Data Pelayanan Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } 
                } 
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            } 
        }
        private void LoadDataResep()
        {
            string sql_med_load = "", s_rm = "", s_date = "", s_que = "";

            //s_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            //s_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            //s_date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();

            sql_med_load = " select a.receipt_id, a.med_cd, b.med_group, a.med_cd med_cd1, formula, type_drink,  " +
                           " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +  " +
                           " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -  " +
                           " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) - " +
                           " klinik.FN_CS_REQ_STOCK(a.insp_date,a.med_cd) stock, " +
                           " med_qty, initcap(uom) uom, 'S' action, a.confirm, a.days, a.price, a.qty_day, a.dosis, a.insp_date, a.INS_JAM " +
                           " from KLINIK.cs_receipt a  " +
                           " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)  " +
                           " where b.status = 'A'  " +
                           " and rm_no = '" + RMNO + "'  " +
                           " and ID_VISIT = '" + visitid + "' AND A.FLAG_PULANG ='N' ";

            DataTable dt2 =  ConnOra.Data_Table_ora(sql_med_load);

            //OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_med_load, oraConnect2);
            //DataTable dt2 = new DataTable();
            //adOra2.Fill(dt2);

            gcJadwalObat.DataSource = null;
            //gvJadwalObat.Columns.Clear();
            gcJadwalObat.DataSource = dt2;

            gvJadwalObat.OptionsView.ColumnAutoWidth = true;
            gvJadwalObat.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gvJadwalObat.Appearance.HeaderPanel.FontSizeDelta = 0;
            gvJadwalObat.IndicatorWidth = 30;
            //gvJadwalObat.OptionsBehavior.Editable = false;
            gvJadwalObat.BestFitColumns();

            s_que = " select a.receipt_id, a.med_cd, b.med_group, a.med_cd med_cd1, formula, type_drink,  " +
                           " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +  " +
                           " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -  " +
                           " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) - " +
                           " klinik.FN_CS_REQ_STOCK(a.insp_date,a.med_cd) stock, " +
                           " med_qty, initcap(uom) uom, 'S' action, a.confirm, a.days, a.price, a.qty_day, a.dosis, a.insp_date, a.INS_JAM, a.cara" +
                           " from KLINIK.cs_receipt a  " +
                           " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)  " +
                           " where b.status = 'A'  " +
                           " and rm_no = '" + RMNO + "'  " +
                           " and ID_VISIT = '" + visitid + "' AND A.FLAG_PULANG ='Y' ";

            DataTable dt = ConnOra.Data_Table_ora(s_que);

            //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adOra = new OleDbDataAdapter(s_que, oraConnect);
            //DataTable dt = new DataTable();
            //adOra.Fill(dt);

            gcObtPlng.DataSource = null;
            //gvJadwalObat.Columns.Clear();
            gcObtPlng.DataSource = dt;

            gvObtPlng.OptionsView.ColumnAutoWidth = true;
            gvObtPlng.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gvObtPlng.Appearance.HeaderPanel.FontSizeDelta = 0;
            gvObtPlng.IndicatorWidth = 30;
            //gvJadwalObat.OptionsBehavior.Editable = false;
            gvObtPlng.BestFitColumns();

            ////gvJadwalObat.Columns[0].Caption = "ID";
            ////gvJadwalObat.Columns[1].Caption = "Kode";
            ////gvJadwalObat.Columns[2].Caption = "Group";
            ////gvJadwalObat.Columns[15].Caption = "Tanggal";
            ////gvJadwalObat.Columns[16].Caption = "Jam";
            ////gvJadwalObat.Columns[3].Caption = "Nama Obat";
            ////gvJadwalObat.Columns[4].Caption = "Kode Dosis";
            ////gvJadwalObat.Columns[5].Caption = "Info";
            ////gvJadwalObat.Columns[6].Caption = "Stok";
            ////gvJadwalObat.Columns[7].Caption = "Jumlah";
            ////gvJadwalObat.Columns[8].Caption = "Satuan";
            ////gvJadwalObat.Columns[9].Caption = "Action";
            ////gvJadwalObat.Columns[10].Caption = "Confirm";
            ////gvJadwalObat.Columns[11].Caption = "Jml";
            ////gvJadwalObat.Columns[12].Caption = "Harga";
            ////gvJadwalObat.Columns[13].Caption = "Jumlah per Hari";
            ////gvJadwalObat.Columns[14].Caption = "Dosis";


            ////gvJadwalObat.Columns[14].VisibleIndex = 5;
            ////gvJadwalObat.Columns[11].VisibleIndex = 6;

            ////gvJadwalObat.Columns[4].MinWidth = 80;
            ////gvJadwalObat.Columns[4].MaxWidth = 80;
            ////gvJadwalObat.Columns[5].MinWidth = 120;
            ////gvJadwalObat.Columns[5].MaxWidth = 120;
            ////gvJadwalObat.Columns[6].MinWidth = 60;
            ////gvJadwalObat.Columns[6].MaxWidth = 60;
            ////gvJadwalObat.Columns[7].MinWidth = 60;
            ////gvJadwalObat.Columns[7].MaxWidth = 60;
            ////gvJadwalObat.Columns[8].MinWidth = 60;
            ////gvJadwalObat.Columns[8].MaxWidth = 60;
            ////gvJadwalObat.Columns[10].MinWidth = 60;
            ////gvJadwalObat.Columns[10].MaxWidth = 60;
            ////gvJadwalObat.Columns[11].MinWidth = 60;
            ////gvJadwalObat.Columns[11].MaxWidth = 60;
            ////gvJadwalObat.Columns[14].MinWidth = 60;
            ////gvJadwalObat.Columns[15].MaxWidth = 80;
            ////gvJadwalObat.Columns[16].MaxWidth = 60;

            ////gvJadwalObat.Columns[0].Visible = false;
            ////gvJadwalObat.Columns[1].Visible = false;
            ////gvJadwalObat.Columns[2].Visible = false;
            ////gvJadwalObat.Columns[5].Visible = false;
            ////gvJadwalObat.Columns[7].Visible = false;
            ////gvJadwalObat.Columns[8].Visible = false;
            ////gvJadwalObat.Columns[9].Visible = false;
            ////gvJadwalObat.Columns[12].Visible = false;
            ////gvJadwalObat.Columns[13].Visible = false;
            //////gvJadwalObat.Columns[10].Visible = false;

            //////gvJadwalObat.Columns[3].OptionsColumn.ReadOnly = true;
            ////gvJadwalObat.Columns[2].OptionsColumn.ReadOnly = true;
            ////gvJadwalObat.Columns[5].OptionsColumn.ReadOnly = true;
            ////gvJadwalObat.Columns[6].OptionsColumn.ReadOnly = true;
            ////gvJadwalObat.Columns[7].OptionsColumn.ReadOnly = true;
            ////gvJadwalObat.Columns[8].OptionsColumn.ReadOnly = true;
            ////gvJadwalObat.Columns[9].OptionsColumn.ReadOnly = true;
            ////gvJadwalObat.Columns[10].OptionsColumn.ReadOnly = true;

            //RepositoryItemLookUpEdit medicineLookup = new RepositoryItemLookUpEdit();
            //medicineLookup.DataSource = listMedicine;
            //medicineLookup.ValueMember = "medicineCode";
            //medicineLookup.DisplayMember = "medicineName";

            //medicineLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //medicineLookup.DropDownRows = listMedicine.Count;
            //medicineLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            //medicineLookup.AutoSearchColumnIndex = 1;
            //medicineLookup.NullText = "";
            //gvJadwalObat.Columns[3].ColumnEdit = medicineLookup;
             
            string sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1 ";
            DataTable dtf = ConnOra.Data_Table_ora(sql_for);

            //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
            //DataTable dtf = new DataTable();
            //adOraf.Fill(dtf);
            listFormula.Clear();
            listFormula2.Clear();
            for (int i = 0; i < dtf.Rows.Count; i++)
            {
                listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
            }

            RepositoryItemGridLookUpEdit glmed = new RepositoryItemGridLookUpEdit();
            glmed.DataSource = listMedicine;
            glmed.ValueMember = "medicineCode";
            glmed.DisplayMember = "medicineName";

            glmed.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glmed.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glmed.ImmediatePopup = true;
            glmed.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glmed.NullText = "";
            gvJadwalObat.Columns[1].ColumnEdit = glmed;
            gvObtPlng.Columns[1].ColumnEdit = glmed;

            RepositoryItemGridLookUpEdit glfor = new RepositoryItemGridLookUpEdit();
            glfor.DataSource = listFormula2;
            glfor.ValueMember = "formulaCode";
            glfor.DisplayMember = "formulaName";

            glfor.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glfor.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glfor.ImmediatePopup = true;
            glfor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glfor.NullText = "";
            gvJadwalObat.Columns["FORMULA"].ColumnEdit = glfor;
            gvObtPlng.Columns["FORMULA"].ColumnEdit = glfor;

            RepositoryItemTextEdit rpjam = new RepositoryItemTextEdit();
            rpjam.Mask.EditMask = "90:00";
            rpjam.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            gvJadwalObat.Columns["INS_JAM"].ColumnEdit = rpjam;
            gvObtPlng.Columns["INS_JAM"].ColumnEdit = rpjam;

            RepositoryItemDateEdit rptanggal = new RepositoryItemDateEdit();
            rptanggal.DisplayFormat.FormatString = "yyyy-MM-dd";
            rptanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            gvJadwalObat.Columns["INSP_DATE"].ColumnEdit = rptanggal;
            gvObtPlng.Columns["INSP_DATE"].ColumnEdit = rptanggal;

            //RepositoryItemLookUpEdit medicineInfoLookup = new RepositoryItemLookUpEdit();
            //medicineInfoLookup.DataSource = listMedicineInfo;
            //medicineInfoLookup.ValueMember = "medicineInfoCode";
            //medicineInfoLookup.DisplayMember = "medicineInfoName";

            //medicineInfoLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //medicineInfoLookup.DropDownRows = listMedicineInfo.Count;
            //medicineInfoLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            //medicineInfoLookup.AutoSearchColumnIndex = 1;
            //medicineInfoLookup.NullText = "";
            //gvJadwalObat.Columns[5].ColumnEdit = medicineInfoLookup;

            RepositoryItemLookUpEdit dosisLookup = new RepositoryItemLookUpEdit();
            dosisLookup.DataSource = listDosis;
            dosisLookup.ValueMember = "DosisCode";
            dosisLookup.DisplayMember = "DosisName";
            dosisLookup.NullText = "";
            gvJadwalObat.Columns["DOSIS"].ColumnEdit = dosisLookup;
            gvObtPlng.Columns["DOSIS"].ColumnEdit = dosisLookup;
            //btnMedAdd.Enabled = true;
            //btnNoReceipt.Enabled = true;

            //if (gvJadwalObat.RowCount > 0)
            //{
            //    btnMedDel.Enabled = true;
            //    btnMedCan.Enabled = true;
            //}
            //else
            //{
            //    btnMedDel.Enabled = false;
            //    btnMedCan.Enabled = true;
            //}
        }
    }
}
