using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clinic
{
    public partial class RawatInapBidan : DevExpress.XtraEditors.XtraForm
    {
        private KoneksiOra koneksi;
        DataTable dt_grdPersalinanLalu;
        DataTable dt_grdPemberianAnstesi;
        DataTable dt_grdSebelumBedah;
        DataTable dt_grdSetelahBedah;
        DataTable dt_grdSPemantauanAnastesih;
        DataTable dt_grdPemantauanIv;
        public RawatInapBidan()
        {
            InitializeComponent();
            koneksi = new KoneksiOra();
        }

        private void RawatInapBidan_Load(object sender, EventArgs e)
        {
            selectedIndexRb();
            kondisiEnable(false);

            //string aaaa = "0::Asfiksia ringan::mengeringkan::menghangatkan::rangsangan taktil::::::::lain-lain::1";
            //string[] aa = aaaa.Split(new string[] { "::" }, StringSplitOptions.None);

            //MessageBox.Show(aa.Length.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {


                string query = @"select count(*) from T2_R_INAP_BIDAN tp where anamesa_id = '" + txt_anastesi_id.Text + "'";
                object result = koneksi.GetScalar(query);

                if (Convert.ToInt32(result) >= 1)
                {
                    getData();

                }
                else
                {
                    string queryInsert = "insert all \n" ;
                    queryInsert += @"into T2_R_INAP_BIDAN (id,anamesa_id) values (r_inap_bidan_seq.nextval, '" + txt_anastesi_id.Text + "') \n";
                    queryInsert += @"into T2_R_INAP_BIDAN_1 (id,anamesa_id) values (r_inap_bidan_1_seq.nextval, '" + txt_anastesi_id.Text + "') \n";
                    queryInsert += @"into T2_R_INAP_BIDAN_2 (id,anamesa_id) values (r_inap_bidan_2_seq.nextval,'" + txt_anastesi_id.Text + "') \n";

                    queryInsert += @"into T2_DOKUMEN_SKALA_I (id,anamesa_id) values (dokumen_skala_i_seq.nextval,'" + txt_anastesi_id.Text + "') \n";
                    queryInsert += @"into T2_DOKUMEN_SKALA_II (id,anamesa_id) values (dokumen_skala_ii_seq.nextval,'" + txt_anastesi_id.Text + "') \n";
                    queryInsert += @"into T2_DOKUMEN_SKALA_III (id,anamesa_id) values (dokumen_skala_iii_seq.nextval,'" + txt_anastesi_id.Text + "') \n";
                    queryInsert += @"into T2_DOKUMEN_SKALA_IV (id,anamesa_id) values (dokumen_skala_iv_seq.nextval,'" + txt_anastesi_id.Text + "') \n";
                    queryInsert += "SELECT * FROM dual ";

                    
                    bool success = koneksi.ExecuteNonQuery(queryInsert);

                    loadDataGrid();


                }

                kondisiEnable(true);
                btnInputData.Enabled = false;
                txtSave.Enabled = true;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Load data gagal !!");

            }



        }

        private void txtSave_Click(object sender, EventArgs e)
        {
            if (txt_anastesi_id.Text == "")
            {
                MessageBox.Show("Anastesi ID Kosong !!");
                return;
            }

            updateTable();
        }
        private void getData()
        {

            string querySelect = "SELECT * FROM T2_R_INAP_BIDAN where anamesa_id = '" + txt_anastesi_id.Text + "'";
            DataTable dataTable = koneksi.GetDataTable(querySelect);

            if (dataTable.Rows.Count > 0)
            {
                txt_nama_istr.EditValue = dataTable.Rows[0]["nama_istr"].ToString();
                txt_umur_istr.EditValue = dataTable.Rows[0]["umur_istr"].ToString();
                txt_agama_istr.EditValue = dataTable.Rows[0]["agama_istr"].ToString();
                txt_pendidikan_istr.EditValue = dataTable.Rows[0]["pendidikan_istr"].ToString();
                txt_pekerjaan_istr.EditValue = dataTable.Rows[0]["pekerjaan_istr"].ToString();
                txt_suku_istr.EditValue = dataTable.Rows[0]["suku_istr"].ToString();
                txt_kawin_lama_istr.EditValue = dataTable.Rows[0]["kawin_lama_istr"].ToString();
                txt_kawin_frek_istr.EditValue = dataTable.Rows[0]["kawin_frek_istr"].ToString();
                txt_nama_suami.EditValue = dataTable.Rows[0]["nama_suami"].ToString();
                txt_umur_suami.EditValue = dataTable.Rows[0]["umur_suami"].ToString();
                txt_agama_suami.EditValue = dataTable.Rows[0]["agama_suami"].ToString();
                txt_pendidikan_suami.EditValue = dataTable.Rows[0]["pendidikan_suami"].ToString();
                txt_pekerjaan_suami.EditValue = dataTable.Rows[0]["pekerjaan_suami"].ToString();
                txt_suku_suami.EditValue = dataTable.Rows[0]["suku_suami"].ToString();
                txt_kawin_lama_suami.EditValue = dataTable.Rows[0]["kawin_lama_suami"].ToString();
                txt_kawin_frek_suami.EditValue = dataTable.Rows[0]["kawin_frek_suami"].ToString();
                txt_biodata_alamat.Text = dataTable.Rows[0]["biodata_alamat"].ToString();
                txt_biodata_keluhan.Text = dataTable.Rows[0]["biodata_keluhan"].ToString();
                txt_rwyt_hamil_a.EditValue = dataTable.Rows[0]["rwyt_hamil_a"].ToString();
                date_rwyt_hamil_hpht_fr.EditValue = dataTable.Rows[0]["rwyt_hamil_hpht_fr"].ToString();
                date_rwyt_hamil_hpht_to.EditValue = dataTable.Rows[0]["rwyt_hamil_hpht_to"].ToString();
                date_rwyt_hamil_anc.EditValue = dataTable.Rows[0]["rwyt_hamil_anc"].ToString();
                txt_rwyt_hamil_komp.EditValue = dataTable.Rows[0]["rwyt_hamil_komp"].ToString();
                txt_rwyt_kb_kontra.EditValue = dataTable.Rows[0]["rwyt_kb_kontra"].ToString();
                txt_rwyt_kb_lama.EditValue = dataTable.Rows[0]["rwyt_kb_lama"].ToString();
                txt_rwyt_kb_alasan_henti.EditValue = dataTable.Rows[0]["rwyt_kb_alasan_henti"].ToString();
                txt_rwyt_penyakit_ibu.EditValue = dataTable.Rows[0]["rwyt_penyakit_ibu"].ToString();
                txt_aktiv_nutrisi.EditValue = dataTable.Rows[0]["aktiv_nutrisi"].ToString();
                txt_aktiv_eliminasi.EditValue = dataTable.Rows[0]["aktiv_eliminasi"].ToString();
                txt_aktiv_istirahat.EditValue = dataTable.Rows[0]["aktiv_istirahat"].ToString();
                txt_k_umum.EditValue = dataTable.Rows[0]["k_umum"].ToString();
                txt_k_sadar.EditValue = dataTable.Rows[0]["k_sadar"].ToString();

                string stxt_umum_vita_td = dataTable.Rows[0]["umum_vita_td"].ToString();
                string[] Split_stxt_umum_vita_td = stxt_umum_vita_td.ToString().Split(new string[] { "::" }, StringSplitOptions.None);

                if (Split_stxt_umum_vita_td.Length >= 2)
                {
                    txt_umum_vita_td.EditValue = Split_stxt_umum_vita_td[0];
                    txt_umum_vita_td_1.EditValue = Split_stxt_umum_vita_td[1];
                }

                txt_umum_vital_n.EditValue = dataTable.Rows[0]["umum_vital_n"].ToString();
                txt_umum_vital_r.EditValue = dataTable.Rows[0]["umum_vital_r"].ToString();
                txt_umum_vital_s.EditValue = dataTable.Rows[0]["umum_vital_s"].ToString();
                txt_ekstermitas_atas.EditValue = dataTable.Rows[0]["ekstermitas_atas"].ToString();
                txt_ekstermitas_bawah.EditValue = dataTable.Rows[0]["ekstermitas_bawah"].ToString();
                txt_asesment_awal.EditValue = dataTable.Rows[0]["asesment_awal"].ToString();
                txt_planning_awal.EditValue = dataTable.Rows[0]["planning_awal"].ToString();
                txt_gen_inpeksi.EditValue = dataTable.Rows[0]["gen_inpeksi"].ToString();
                txt_gen_vulva.EditValue = dataTable.Rows[0]["gen_vulva"].ToString();
                txt_gen_portio.EditValue = dataTable.Rows[0]["gen_portio"].ToString();
                txt_gen_pembukaan.EditValue = dataTable.Rows[0]["gen_pembukaan"].ToString();
                txt_gen_penurunan.EditValue = dataTable.Rows[0]["gen_penurunan"].ToString();
                txt_gen_ketuban.EditValue = dataTable.Rows[0]["gen_ketuban"].ToString();
                txt_gen_presntasi.EditValue = dataTable.Rows[0]["gen_presntasi"].ToString();

            }


            string querySelect1 = "SELECT * FROM T2_R_INAP_BIDAN_1 where anamesa_id = '" + txt_anastesi_id.Text + "'";
            DataTable dataTable1 = koneksi.GetDataTable(querySelect1);

            if (dataTable1.Rows.Count > 0)
            {
                txt_fisik_mata.EditValue = dataTable1.Rows[0]["fisik_mata"].ToString();
                txt_fisik_leher.EditValue = dataTable1.Rows[0]["fisik_leher"].ToString();
                txt_fisik_payudara.EditValue = dataTable1.Rows[0]["fisik_payudara"].ToString();
                txt_fisik_inpeksi.EditValue = dataTable1.Rows[0]["fisik_inpeksi"].ToString();
                txt_fisik_leopold_i.EditValue = dataTable1.Rows[0]["fisik_leopold_i"].ToString();
                txt_fisik_leopold_ii.EditValue = dataTable1.Rows[0]["fisik_leopold_ii"].ToString();
                txt_fisik_leopold_iii.EditValue = dataTable1.Rows[0]["fisik_leopold_iii"].ToString();
                txt_fisik_leopold_iv.EditValue = dataTable1.Rows[0]["fisik_leopold_iv"].ToString();
                txt_fisik_tfu.EditValue = dataTable1.Rows[0]["fisik_tfu"].ToString();
                txt_fisik_djj.EditValue = dataTable1.Rows[0]["fisik_djj"].ToString();
                txt_fisik_his.EditValue = dataTable1.Rows[0]["fisik_his"].ToString();
                txt_penunjang_i.EditValue = dataTable1.Rows[0]["penunjang_i"].ToString();
                txt_penunjang_iii.EditValue = dataTable1.Rows[0]["penunjang_iii"].ToString();
                txt_protein_urine.EditValue = dataTable1.Rows[0]["protein_urine"].ToString();
                txt_glukosa_urine.EditValue = dataTable1.Rows[0]["glukosa_urine"].ToString();
                date_tgl_persalinan.EditValue = dataTable1.Rows[0]["tgl_persalinan"].ToString();
                txt_nama_bidan.EditValue = dataTable1.Rows[0]["nama_bidan"].ToString();


                functionSplitIndex_3((dataTable1.Rows[0]["tempat_persalinan"].ToString()), rb_tempat_persalinan, txt_tempat_persalinan);

                txt_alamat_persalinan.EditValue = dataTable1.Rows[0]["alamat_persalinan"].ToString();

                functionSplitIndex_2(dataTable1.Rows[0]["rujuk_kala"].ToString(), rb_rujuk_kala);

                txt_alasan_rujuk.EditValue = dataTable1.Rows[0]["alasan_rujuk"].ToString();
                txt_tempat_rujuk.EditValue = dataTable1.Rows[0]["tempat_rujuk"].ToString();

                functionSplitIndex_2(dataTable1.Rows[0]["pendamping_rujuk"].ToString(), rb_pendamping_rujuk);

                functionSplitIndex_2(dataTable1.Rows[0]["kala_i_a"].ToString(), rb_kala_i_a);

                txt_kala_i_b.EditValue = dataTable1.Rows[0]["kala_i_b"].ToString();
                txt_kala_i_c.EditValue = dataTable1.Rows[0]["kala_i_c"].ToString();
                txt_kala_i_d.EditValue = dataTable1.Rows[0]["kala_i_d"].ToString();


                functionSplitIndex_3(dataTable1.Rows[0]["kala_ii_a"].ToString(), rb_kala_ii_a, txt_kala_ii_a);
                functionSplitIndex_2(dataTable1.Rows[0]["kala_ii_b"].ToString(), rb_kala_ii_b);
                functionSplitIndex_3(dataTable1.Rows[0]["kala_ii_c"].ToString(), rb_kala_ii_c, txt_kala_ii_c);
                functionSplitIndex_3(dataTable1.Rows[0]["kala_ii_d"].ToString(), rb_kala_ii_d, txt_kala_ii_d);

                txt_kala_ii_e.EditValue = dataTable1.Rows[0]["kala_ii_e"].ToString();
                txt_kala_ii_f.EditValue = dataTable1.Rows[0]["kala_ii_f"].ToString();
                txt_kala_ii_g.EditValue = dataTable1.Rows[0]["kala_ii_g"].ToString();
                txt_kala_iii_a.EditValue = dataTable1.Rows[0]["kala_iii_a"].ToString();

                functionSplitIndex_3(dataTable1.Rows[0]["kala_iii_b"].ToString(), rb_kala_iii_b, txt_kala_iii_b);
                functionSplitIndex_3(dataTable1.Rows[0]["kala_iii_c"].ToString(), rb_kala_iii_c, txt_kala_iii_c);
                functionSplitIndex_3(dataTable1.Rows[0]["kala_iii_d"].ToString(), rb_kala_iii_d, txt_kala_iii_d);

                txt_baru_lahir_berat.EditValue = dataTable1.Rows[0]["baru_lahir_berat"].ToString();
                txt_baru_lahir_panjang.EditValue = dataTable1.Rows[0]["baru_lahir_panjang"].ToString();

                functionSplitIndex_2(dataTable1.Rows[0]["baru_lahir_jk"].ToString(), rb_baru_lahir_jk);
                functionSplitIndex_2(dataTable1.Rows[0]["baru_lahir_nilai"].ToString(), rb_baru_lahir_nilai);

                chk_baru_lahir_ket.Checked = functionChk(dataTable1.Rows[0]["baru_lahir_ket"].ToString(), "Normal");
                chk_baru_lahir_ket_1.Checked = functionChk(dataTable1.Rows[0]["baru_lahir_ket"].ToString(), "mengeringkan");
                chk_baru_lahir_ket_2.Checked = functionChk(dataTable1.Rows[0]["baru_lahir_ket"].ToString(), "menghangatkan");
                chk_baru_lahir_ket_3.Checked = functionChk(dataTable1.Rows[0]["baru_lahir_ket"].ToString(), "bungkus bayi dan tempatkan di sisi ibu");
                chk_baru_lahir_ket_4.Checked = functionChk(dataTable1.Rows[0]["baru_lahir_ket"].ToString(), "rangsangan taktil");
                chk_baru_lahir_ket_5.Checked = functionChk(dataTable1.Rows[0]["baru_lahir_ket"].ToString(), "tindakan pencegahan infeksi mata");

            }

            string querySelect2 = "SELECT * FROM T2_R_INAP_BIDAN_2 where anamesa_id = '" + txt_anastesi_id.Text + "'";
            DataTable dataTable2 = koneksi.GetDataTable(querySelect2);

            if (dataTable2.Rows.Count > 0)
            {
                functionSplitIndex_3(dataTable2.Rows[0]["rangsang_laktil"].ToString(), rb_rangsang_laktil, txt_rangsang_laktil);
                functionSplitIndex_3(dataTable2.Rows[0]["plasenta_intack"].ToString(), rb_plasenta_intack, txt_plasenta_intack);
                functionSplitIndex_3(dataTable2.Rows[0]["plasenta_tidak_lahir"].ToString(), rb_plasenta_tidak_lahir, txt_plasenta_tidak_lahir);
                functionSplitIndex_3(dataTable2.Rows[0]["laserasi"].ToString(), rb_laserasi, txt_laserasi);

                functionSplitIndex_5(dataTable2.Rows[0]["laserasi_parinium"].ToString(), rb_laserasi_parinium, rb_laserasi_parinium_tindakan, txt_laserasi_parinium);

                functionSplitIndex_3(dataTable2.Rows[0]["atonia_uteri"].ToString(), rb_atonia_uteri, txt_atonia_uteri);

                txt_jumlah_pendarahan.EditValue = dataTable2.Rows[0]["jumlah_pendarahan"].ToString();
                txt_masalah_lain.EditValue = dataTable2.Rows[0]["masalah_lain"].ToString();
                txt_penata_masalah.EditValue = dataTable2.Rows[0]["penata_masalah"].ToString();
                txt_hasilnya.EditValue = dataTable2.Rows[0]["hasilnya"].ToString();



                functionSplitIndex_10(dataTable2.Rows[0]["baru_lahir"].ToString(), rb_baru_lahir, txt_cacat_ket_lain);

                chk_cacat_ket_1.Checked = functionChk(dataTable2.Rows[0]["baru_lahir"].ToString(), "mengeringkan");
                chk_cacat_ket_2.Checked = functionChk(dataTable2.Rows[0]["baru_lahir"].ToString(), "menghangatkan");
                chk_cacat_ket_3.Checked = functionChk(dataTable2.Rows[0]["baru_lahir"].ToString(), "rangsangan taktil");
                chk_cacat_ket_4.Checked = functionChk(dataTable2.Rows[0]["baru_lahir"].ToString(), "bungkus bayi dan tempatkan di sisi ibu");
                chk_cacat_ket_5.Checked = functionChk(dataTable2.Rows[0]["baru_lahir"].ToString(), "tindakan pencegahan infeksi mata");
                chk_cacat_ket_6.Checked = functionChk(dataTable2.Rows[0]["baru_lahir"].ToString(), "bebaskan jalan napas");
                chk_cacat_ket_lain.Checked = functionChk(dataTable2.Rows[0]["baru_lahir"].ToString(), "lain-lain");


                txt_bayi_cacat.EditValue = dataTable2.Rows[0]["bayi_cacat"].ToString();
                txt_hipotermia.EditValue = dataTable2.Rows[0]["hipotermia"].ToString();


                functionSplitIndex_4_asi(dataTable2.Rows[0]["pemberian_asi"].ToString(), rb_pemberian_asi, txt_pemberian_asi_ya, txt_pemberian_asi_tdk);

                txt_masalah_lahir.EditValue = dataTable2.Rows[0]["masalah_lahir"].ToString();
                txt_hasil_lahir.EditValue = dataTable2.Rows[0]["hasil_lahir"].ToString();


            }

            string querySelect3 = "SELECT * FROM T2_TINDAKAN_BIDAN where anamesa_id = '" + txt_anastesi_id.Text + "'";
            DataTable dataTable3 = koneksi.GetDataTable(querySelect3);

            if (dataTable3.Rows.Count > 0)
            {
                cb_ada_tindakan.Checked = functionChk(dataTable3.Rows[0]["ada_tindakan"].ToString(), "Ya");
            }


            string querySelect4 = "SELECT * FROM T2_TINDAKAN_BIDAN where anamesa_id = '" + txt_anastesi_id.Text + "'";
            DataTable dataTable4 = koneksi.GetDataTable(querySelect4);

            if (dataTable4.Rows.Count > 0)
            {
                txt_nama_tindakan.EditValue = dataTable3.Rows[0]["nama_tindakan"].ToString();
                txt_dokter_pelaksana.EditValue = dataTable3.Rows[0]["dokter_pelaksana"].ToString();
                txt_pemberi_informasi.EditValue = dataTable3.Rows[0]["pemberi_informasi"].ToString();
                txt_penerima_informasi.EditValue = dataTable3.Rows[0]["penerima_informasi"].ToString();
                date_tgl_diberikan.EditValue = dataTable3.Rows[0]["tgl_diberikan"].ToString();
                txt_diagnosis.EditValue = dataTable3.Rows[0]["diagnosis"].ToString();
                txt_dasar_diagnosis.EditValue = dataTable3.Rows[0]["dasar_diagnosis"].ToString();
                txt_tindakan_medis.EditValue = dataTable3.Rows[0]["tindakan_medis"].ToString();
                txt_indikasi_tindakan.EditValue = dataTable3.Rows[0]["indikasi_tindakan"].ToString();
                txt_tata_cara.EditValue = dataTable3.Rows[0]["tata_cara"].ToString();
                txt_tujuan.EditValue = dataTable3.Rows[0]["tujuan"].ToString();
                txt_resiko.EditValue = dataTable3.Rows[0]["resiko"].ToString();
                txt_komplikasi.EditValue = dataTable3.Rows[0]["komplikasi"].ToString();
                txt_prognosis.EditValue = dataTable3.Rows[0]["prognosis"].ToString();
                txt_alternatif.EditValue = dataTable3.Rows[0]["alternatif"].ToString();
                txt_lain_lain.EditValue = dataTable3.Rows[0]["lain_lain"].ToString();
                txt_keluhan_utama.EditValue = dataTable3.Rows[0]["keluhan_utama"].ToString();
                txt_riwayat_penyakit_sekarang.EditValue = dataTable3.Rows[0]["riwayat_penyakit_sekarang"].ToString();
                txt_riwayat_penyakit_dahulu.EditValue = dataTable3.Rows[0]["riwayat_penyakit_dahulu"].ToString();
                txt_riwayat_penyakit_keluarga.EditValue = dataTable3.Rows[0]["riwayat_penyakit_keluarga"].ToString();
                txt_riwayat_alergi.EditValue = dataTable3.Rows[0]["riwayat_alergi"].ToString();
                txt_riwayat_pengobatan.EditValue = dataTable3.Rows[0]["riwayat_pengobatan"].ToString();
                txt_keadaan_umum.EditValue = dataTable3.Rows[0]["keadaan_umum"].ToString();
                txt_kesadaran.EditValue = dataTable3.Rows[0]["kesadaran"].ToString();
                txt_fisik_tanda_vital.EditValue = dataTable3.Rows[0]["fisik_tanda_vital"].ToString();
                txt_tekanan_darah.EditValue = dataTable3.Rows[0]["tekanan_darah"].ToString();
                txt_nadi.EditValue = dataTable3.Rows[0]["nadi"].ToString();
                txt_suhu.EditValue = dataTable3.Rows[0]["suhu"].ToString();
                txt_rr.EditValue = dataTable3.Rows[0]["rr"].ToString();
                txt_berat_badan.EditValue = dataTable3.Rows[0]["berat_badan"].ToString();
                txt_status_gizi.EditValue = dataTable3.Rows[0]["status_gizi"].ToString();
                txt_kepala_leher.EditValue = dataTable3.Rows[0]["kepala_leher"].ToString();
                txt_thoraks.EditValue = dataTable3.Rows[0]["thoraks"].ToString();
                txt_abdomen.EditValue = dataTable3.Rows[0]["abdomen"].ToString();
                txt_extremitas.EditValue = dataTable3.Rows[0]["extremitas"].ToString();
                txt_laboratorium.EditValue = dataTable3.Rows[0]["laboratorium"].ToString();
                txt_rontgen.EditValue = dataTable3.Rows[0]["rontgen"].ToString();
                txt_penunjang_lain_lain.EditValue = dataTable3.Rows[0]["penunjang_lain_lain"].ToString();
                txt_diagnosa_prabedah.EditValue = dataTable3.Rows[0]["diagnosa_prabedah"].ToString();
                txt_rencana_tindakan.EditValue = dataTable3.Rows[0]["rencana_tindakan"].ToString();
                txt_rencana_monitoring.EditValue = dataTable3.Rows[0]["rencana_monitoring"].ToString();
                txt_rencana_edukasi.EditValue = dataTable3.Rows[0]["rencana_edukasi"].ToString();

                functionSplitIndex_2(dataTable3.Rows[0]["pasien_alergi"].ToString(), rb_pasien_alergi);
                functionSplitIndex_2(dataTable3.Rows[0]["risiko_aspirasi"].ToString(), rb_risiko_aspirasi);
                functionSplitIndex_2(dataTable3.Rows[0]["pendarahan"].ToString(), rb_pendarahan);
                functionSplitIndex_2(dataTable3.Rows[0]["profilaksi"].ToString(), rb_profilaksi);
                functionSplitIndex_2(dataTable3.Rows[0]["hasil_imaging"].ToString(), rb_hasil_imaging);

            }



            string querySkala1 = "SELECT * FROM T2_DOKUMEN_SKALA_I where anamesa_id = '" + txt_anastesi_id.Text + "'";
            DataTable dt_skala1 = koneksi.GetDataTable(querySkala1);

            if (dt_skala1.Rows.Count > 0)
            {
                txt_skl1_tanggal.EditValue = dt_skala1.Rows[0]["tanggal"].ToString();
                txt_skl1_jam.EditValue = dt_skala1.Rows[0]["jam"].ToString();
                txt_skl1_keluhan_utama.EditValue = dt_skala1.Rows[0]["keluhan_utama"].ToString();
                txt_skl1_kesadaran.EditValue = dt_skala1.Rows[0]["kesadaran"].ToString();
                txt_skl1_td_1.EditValue = dt_skala1.Rows[0]["td_1"].ToString();
                txt_skl1_td_2.EditValue = dt_skala1.Rows[0]["td_2"].ToString();
                txt_skl1_n.EditValue = dt_skala1.Rows[0]["n"].ToString();
                txt_skl1_r.EditValue = dt_skala1.Rows[0]["r"].ToString();
                txt_skl1_s.EditValue = dt_skala1.Rows[0]["s"].ToString();
                txt_skl1_frekuensi.EditValue = dt_skala1.Rows[0]["frekuensi"].ToString();
                txt_skl1_interfal.EditValue = dt_skala1.Rows[0]["interfal"].ToString();
                txt_skl1_durasi.EditValue = dt_skala1.Rows[0]["durasi"].ToString();

                //txt_skl1_djj.EditValue              = dt_skala1.Rows[0]["djj"].ToString();

                functionSplitIndex_3(dt_skala1.Rows[0]["djj"].ToString(), rb_skl1_djj, txt_skl1_djj);

                txt_skl1_diagnosa.EditValue = dt_skala1.Rows[0]["diagnosa"].ToString();
                txt_skl1_masalah_potensial.EditValue = dt_skala1.Rows[0]["masalah_potensial"].ToString();
                txt_skl1_antisipasi_masalah.EditValue = dt_skala1.Rows[0]["antisipasi_masalah"].ToString();
                txt_skl1_vulva.EditValue = dt_skala1.Rows[0]["vulva"].ToString();
                txt_skl1_pembukaan.EditValue = dt_skala1.Rows[0]["pembukaan"].ToString();
                txt_skl1_keadaan_ketuban.EditValue = dt_skala1.Rows[0]["keadaan_ketuban"].ToString();
                txt_skl1_presentasi.EditValue = dt_skala1.Rows[0]["presentasi"].ToString();
                txt_skl1_bagian_teraba.EditValue = dt_skala1.Rows[0]["bagian_teraba"].ToString();
                txt_skl1_turunnya_bagian.EditValue = dt_skala1.Rows[0]["turunnya_bagian"].ToString();
                txt_skl1_molage.EditValue = dt_skala1.Rows[0]["molage"].ToString();

                functionSplitIndex_2(dt_skala1.Rows[0]["vesica_urineria"].ToString(), rb_skl1_vesica_urineria);

                //rb_skl1_vesica_urineria.EditValue       = dt_skala1.Rows[0]["vesica_urineria"].ToString();

                txt_skl1_planning.EditValue = dt_skala1.Rows[0]["planning"].ToString();
                txt_skl1_jam_plan.EditValue = dt_skala1.Rows[0]["jam_plan"].ToString();
                txt_skl1_tgl_plan.EditValue = dt_skala1.Rows[0]["tgl_plan"].ToString();
                txt_skl1_dipimpin.EditValue = dt_skala1.Rows[0]["dipimpin"].ToString();
                txt_skl1_jam_mulai.EditValue = dt_skala1.Rows[0]["jam_mulai"].ToString();


            }


            string querySkala2 = "SELECT * FROM T2_DOKUMEN_SKALA_II where anamesa_id = '" + txt_anastesi_id.Text + "'";
            DataTable dt_skala2 = koneksi.GetDataTable(querySkala2);

            if (dt_skala2.Rows.Count > 0)
            {
                txt_skl2_tanggal.EditValue = dt_skala2.Rows[0]["tanggal"].ToString();
                txt_skl2_jam.EditValue = dt_skala2.Rows[0]["jam"].ToString();
                txt_skl2_keluhan_utama.EditValue = dt_skala2.Rows[0]["keluhan_utama"].ToString();
                txt_skl2_kesadaran.EditValue = dt_skala2.Rows[0]["kesadaran"].ToString();
                txt_skl2_td_1.EditValue = dt_skala2.Rows[0]["td_1"].ToString();
                txt_skl2_td_2.EditValue = dt_skala2.Rows[0]["td_2"].ToString();
                txt_skl2_n.EditValue = dt_skala2.Rows[0]["n"].ToString();
                txt_skl2_r.EditValue = dt_skala2.Rows[0]["r"].ToString();
                txt_skl2_s.EditValue = dt_skala2.Rows[0]["s"].ToString();
                txt_skl2_frekuensi.EditValue = dt_skala2.Rows[0]["frekuensi"].ToString();
                txt_skl2_interfal.EditValue = dt_skala2.Rows[0]["interfal"].ToString();
                txt_skl2_durasi.EditValue = dt_skala2.Rows[0]["durasi"].ToString();

                functionSplitIndex_3(dt_skala2.Rows[0]["djj"].ToString(), rb_skl2_djj, txt_skl2_djj);
                //txt_skl2_djj.EditValue              = dt_skala2.Rows[0]["djj"].ToString();

                txt_skl2_diagnosa.EditValue = dt_skala2.Rows[0]["diagnosa"].ToString();
                txt_skl2_masalah_potensial.EditValue = dt_skala2.Rows[0]["masalah_potensial"].ToString();
                txt_skl2_antisipasi_masalah.EditValue = dt_skala2.Rows[0]["antisipasi_masalah"].ToString();
                txt_skl2_vulva.EditValue = dt_skala2.Rows[0]["vulva"].ToString();
                txt_skl2_pembukaan.EditValue = dt_skala2.Rows[0]["pembukaan"].ToString();
                txt_skl2_keadaan_ketuban.EditValue = dt_skala2.Rows[0]["keadaan_ketuban"].ToString();
                txt_skl2_presentasi.EditValue = dt_skala2.Rows[0]["presentasi"].ToString();
                txt_skl2_bagian_teraba.EditValue = dt_skala2.Rows[0]["bagian_teraba"].ToString();
                txt_skl2_turunnya_bagian.EditValue = dt_skala2.Rows[0]["turunnya_bagian"].ToString();
                txt_skl2_molage.EditValue = dt_skala2.Rows[0]["molage"].ToString();

                functionSplitIndex_2(dt_skala2.Rows[0]["vesica_urineria"].ToString(), rb_skl2_vesica_urineria);

                //rb_skl2_vesica_urineria.EditValue   = dt_skala2.Rows[0]["vesica_urineria"].ToString();

                txt_skl2_planning.EditValue = dt_skala2.Rows[0]["planning"].ToString();
                txt_skl2_lahir_jam.EditValue = dt_skala2.Rows[0]["lahir_jam"].ToString();
                txt_skl2_jk.EditValue = dt_skala2.Rows[0]["jk"].ToString();
                txt_skl2_bb.EditValue = dt_skala2.Rows[0]["bb"].ToString();
                txt_skl2_lk.EditValue = dt_skala2.Rows[0]["lk"].ToString();
                txt_skl2_ld.EditValue = dt_skala2.Rows[0]["ld"].ToString();
                txt_skl2_kadaan_lahir.EditValue = dt_skala2.Rows[0]["kadaan_lahir"].ToString();
                txt_skl2_evaluasi.EditValue = dt_skala2.Rows[0]["evaluasi"].ToString();



            }

            string querySkala3 = "SELECT * FROM T2_DOKUMEN_SKALA_III where anamesa_id = '" + txt_anastesi_id.Text + "'";
            DataTable dt_skala3 = koneksi.GetDataTable(querySkala3);

            if (dt_skala3.Rows.Count > 0)
            {
                txt_skl3_tanggal.EditValue = dt_skala3.Rows[0]["tanggal"].ToString();
                txt_skl3_jam.EditValue = dt_skala3.Rows[0]["jam"].ToString();
                txt_skl3_keluhan_utama.EditValue = dt_skala3.Rows[0]["keluhan_utama"].ToString();
                txt_skl3_kesadaran.EditValue = dt_skala3.Rows[0]["kesadaran"].ToString();
                txt_skl3_td_1.EditValue = dt_skala3.Rows[0]["td_1"].ToString();
                txt_skl3_td_2.EditValue = dt_skala3.Rows[0]["td_2"].ToString();
                txt_skl3_n.EditValue = dt_skala3.Rows[0]["n"].ToString();
                txt_skl3_r.EditValue = dt_skala3.Rows[0]["r"].ToString();
                txt_skl3_s.EditValue = dt_skala3.Rows[0]["s"].ToString();
                txt_skl3_palpasi_abdomen.EditValue = dt_skala3.Rows[0]["palpasi_abdomen"].ToString();

                //cb_skl3_kontraksi_uterus_1.EditValue    = dt_skala3.Rows[0]["kontraksi_uterus"].ToString();

                cb_skl3_kontraksi_uterus_1.Checked = functionChk(dt_skala3.Rows[0]["kontraksi_uterus"].ToString(), "ada");
                cb_skl3_kontraksi_uterus_2.Checked = functionChk(dt_skala3.Rows[0]["kontraksi_uterus"].ToString(), "tidak dan lemah");
                cb_skl3_kontraksi_uterus_3.Checked = functionChk(dt_skala3.Rows[0]["kontraksi_uterus"].ToString(), "ade kuat");

                //rb_skl3_uterus_membulat.EditValue   = dt_skala3.Rows[0]["uterus_membulat"].ToString();
                functionSplitIndex_2(dt_skala3.Rows[0]["uterus_membulat"].ToString(), rb_skl3_uterus_membulat);

                txt_skl3_tinggi_fundus.EditValue = dt_skala3.Rows[0]["tinggi_fundus"].ToString();

                //rb_skl3_semburan_darah.EditValue    = dt_skala3.Rows[0]["semburan_darah"].ToString();
                //rb_skl3_vesica_urineria.EditValue   = dt_skala3.Rows[0]["vesica_urineria"].ToString();

                functionSplitIndex_2(dt_skala3.Rows[0]["semburan_darah"].ToString(), rb_skl3_semburan_darah);
                functionSplitIndex_2(dt_skala3.Rows[0]["vesica_urineria"].ToString(), rb_skl3_vesica_urineria);

                txt_skl3_diagnosa.EditValue = dt_skala3.Rows[0]["diagnosa"].ToString();
                txt_skl3_masalah_potensial.EditValue = dt_skala3.Rows[0]["masalah_potensial"].ToString();
                txt_skl3_antisipasi_masalah.EditValue = dt_skala3.Rows[0]["antisipasi_masalah"].ToString();
                txt_skl3_planning.EditValue = dt_skala3.Rows[0]["planning"].ToString();
                txt_skl3_placenta_lahir.EditValue = dt_skala3.Rows[0]["placenta_lahir"].ToString();

                //rb_skl3_spontan.EditValue           = dt_skala3.Rows[0]["spontan"].ToString();
                //rb_skl3_lengkap.EditValue           = dt_skala3.Rows[0]["lengkap"].ToString();
                functionSplitIndex_2(dt_skala3.Rows[0]["spontan"].ToString(), rb_skl3_spontan);
                functionSplitIndex_2(dt_skala3.Rows[0]["lengkap"].ToString(), rb_skl3_lengkap);

                txt_skl3_kontraksi.EditValue = dt_skala3.Rows[0]["kontraksi"].ToString();
                txt_skl3_pendarahan.EditValue = dt_skala3.Rows[0]["pendarahan"].ToString();
                txt_skl3_keadaan_jalan.EditValue = dt_skala3.Rows[0]["keadaan_jalan"].ToString();
                //rb_skl3_bila_reptura.EditValue      = dt_skala3.Rows[0]["bila_reptura"].ToString();
                functionSplitIndex_2(dt_skala3.Rows[0]["bila_reptura"].ToString(), rb_skl3_bila_reptura);



            }


            string querySkala4 = "SELECT * FROM T2_DOKUMEN_SKALA_IV where anamesa_id = '" + txt_anastesi_id.Text + "'";
            DataTable dt_skala4 = koneksi.GetDataTable(querySkala4);

            if (dt_skala4.Rows.Count > 0)
            {
                txt_skl4_tanggal.EditValue = dt_skala4.Rows[0]["tanggal"].ToString();
                txt_skl4_jam.EditValue = dt_skala4.Rows[0]["jam"].ToString();
                txt_skl4_keluhan_utama.EditValue = dt_skala4.Rows[0]["keluhan_utama"].ToString();
                txt_skl4_kesadaran.EditValue = dt_skala4.Rows[0]["kesadaran"].ToString();
                txt_skl4_td_1.EditValue = dt_skala4.Rows[0]["td_1"].ToString();
                txt_skl4_td_2.EditValue = dt_skala4.Rows[0]["td_2"].ToString();
                txt_skl4_n.EditValue = dt_skala4.Rows[0]["n"].ToString();
                txt_skl4_r.EditValue = dt_skala4.Rows[0]["r"].ToString();
                txt_skl4_s.EditValue = dt_skala4.Rows[0]["s"].ToString();

                cb_skl4_kontraksi_uterus_1.Checked = functionChk(dt_skala4.Rows[0]["kontraksi_uterus"].ToString(), "Ada");
                cb_skl4_kontraksi_uterus_1.Checked = functionChk(dt_skala4.Rows[0]["kontraksi_uterus"].ToString(), "tidak kuat");
                cb_skl4_kontraksi_uterus_1.Checked = functionChk(dt_skala4.Rows[0]["kontraksi_uterus"].ToString(), "kuat");

                txt_skl4_tinggi_fundus.EditValue = dt_skala4.Rows[0]["tinggi_fundus"].ToString();

                functionSplitIndex_2(dt_skala3.Rows[0]["vesica_urineria"].ToString(), rb_skl4_vesica_urineria);

                txt_skl4_jumlah_darah.EditValue = dt_skala4.Rows[0]["jumlah_darah"].ToString();
                txt_skl4_diagnosa.EditValue = dt_skala4.Rows[0]["diagnosa"].ToString();
                txt_skl4_masalah_potensial.EditValue = dt_skala4.Rows[0]["masalah_potensial"].ToString();
                txt_skl4_antisipasi_masalah.EditValue = dt_skala4.Rows[0]["antisipasi_masalah"].ToString();
                txt_skl4_planning.EditValue = dt_skala4.Rows[0]["planning"].ToString();

            }

            loadDataGrid();


        }
        private void loadDataGrid()
        {

            string querySelect5 = "SELECT * FROM T2_RIWAYAT_PERSALINAN_LALU  where anamesa_id = '" + txt_anastesi_id.Text + "'";
            dt_grdPersalinanLalu = koneksi.GetDataTable(querySelect5);
            ConvertColumnNamesToUppercase(dt_grdPersalinanLalu);
            grdPersalinanLalu.DataSource = dt_grdPersalinanLalu;



            string querySelect6 = "SELECT * FROM T2_MTR_PEMBERIAN_ANAMNESA  where anamesa_id = '" + txt_anastesi_id.Text + "'";
            dt_grdPemberianAnstesi = koneksi.GetDataTable(querySelect6);

            ConvertColumnNamesToUppercase(dt_grdPemberianAnstesi);
            grdPemberianAnstesi.DataSource = dt_grdPemberianAnstesi;


            string querySelect7 = "SELECT * FROM T2_MTR_PEMBEDAHAN  where anamesa_id = '" + txt_anastesi_id.Text + "' and jenis = 'SEBELUM' ";
            dt_grdSebelumBedah = koneksi.GetDataTable(querySelect7);

            ConvertColumnNamesToUppercase(dt_grdSebelumBedah);
            grdSebelumBedah.DataSource = dt_grdSebelumBedah;

            string querySelect8 = "SELECT * FROM T2_MTR_PEMBEDAHAN  where anamesa_id = '" + txt_anastesi_id.Text + "' and jenis = 'SETELAH' ";
            dt_grdSetelahBedah = koneksi.GetDataTable(querySelect8);

            ConvertColumnNamesToUppercase(dt_grdSetelahBedah);
            grdSetelahBedah.DataSource = dt_grdSetelahBedah;


            string querySelect9 = "SELECT * FROM T2_PMT_ANASTESI  where anamesa_id = '" + txt_anastesi_id.Text + "' ";
            dt_grdSPemantauanAnastesih = koneksi.GetDataTable(querySelect9);

            ConvertColumnNamesToUppercase(dt_grdSPemantauanAnastesih);
            grdPemantauanAnastesi.DataSource = dt_grdSPemantauanAnastesih;

            if (dt_grdSPemantauanAnastesih.Rows.Count > 0)
            {
                txtJam.EditValue = dt_grdSPemantauanAnastesih.Rows[0]["JAM_MULAI"].ToString();
                date_pemantauan.EditValue = dt_grdSPemantauanAnastesih.Rows[0]["TANGGAL"].ToString();
                txtKesadaran.EditValue = dt_grdSPemantauanAnastesih.Rows[0]["KESADARAN"].ToString();
                txtJamSelesai.EditValue = dt_grdSPemantauanAnastesih.Rows[0]["JAM_SELESAI_TINDAKAN"].ToString();

            }

            string querySelect10 = "SELECT * FROM T2_PMT_PERSALINAN_IV  where anamesa_id = '" + txt_anastesi_id.Text + "' ";
            dt_grdPemantauanIv = koneksi.GetDataTable(querySelect10);

            ConvertColumnNamesToUppercase(dt_grdPemantauanIv);
            grdPemantauanIv.DataSource = dt_grdPemantauanIv;

            if (dt_grdPemantauanIv.Rows.Count > 0)
            {
                txt_MasalahKala4.EditValue = dt_grdPemantauanIv.Rows[0]["MASALAH_KALA_IV"].ToString();
                txt_PenatalaksanaKala4.EditValue = dt_grdPemantauanIv.Rows[0]["PENATALAKSANAAN_KALA_IV"].ToString();
            }
        }

        private void btnAddPersalinanIV_Click(object sender, EventArgs e)
        {
            if (dt_grdPemantauanIv == null) return;

            DataRow newRow = dt_grdPemantauanIv.NewRow();

            newRow["URUTAN_KE"] = ((gvwPemantauanIv.RowCount) + 1).ToString();
            dt_grdPemantauanIv.Rows.Add(newRow);

            grdPemantauanIv.DataSource = dt_grdPemantauanIv;
        }

        private void btnSavePersalinanIV_Click(object sender, EventArgs e)
        {

            try
            {

                bool success = false;
                foreach (DataRow row in dt_grdPemantauanIv.Rows)
                {


                    string query = @"select count(*) from T2_PMT_PERSALINAN_IV where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' ";
                    object result = koneksi.GetScalar(query);
                    if (Convert.ToInt32(result) >= 1)
                    {
                        string queryInsert = @"update  T2_PMT_PERSALINAN_IV set
                                                                urutan_ke           = '" + row["URUTAN_KE"] + @"',
                                                                jam_ke              = '" + row["JAM_KE"] + @"',
                                                                waktu               = '" + row["WAKTU"] + @"',
                                                                tekanan_darah       = '" + row["TEKANAN_DARAH"] + @"',
                                                                nadi                = '" + row["NADI"] + @"',
                                                                temperatur          = '" + row["TEMPERATUR"] + @"',
                                                                tinggi_fundus       = '" + row["TINGGI_FUNDUS"] + @"',
                                                                kontraksi_uterus    = '" + row["KONTRAKSI_UTERUS"] + @"',
                                                                kandung_kemih       = '" + row["KANDUNG_KEMIH"] + @"',
                                                                pendarahan          = '" + row["PENDARAHAN"] + @"',
                                                                masalah_kala_iv     = '" + txt_MasalahKala4.Text + @"',
                                                                penatalaksanaan_kala_iv = '" + txt_PenatalaksanaKala4.Text + @"'
                                                      where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' ";

                        success = koneksi.ExecuteNonQuery(queryInsert);

                    }
                    else
                    {
                        string queryInsert = @"insert into T2_PMT_PERSALINAN_IV(
                                                                    id,
                                                                    anamesa_id,
                                                                    urutan_ke,
                                                                    jam_ke,
                                                                    waktu,
                                                                    tekanan_darah,
                                                                    nadi,
                                                                    temperatur,
                                                                    tinggi_fundus,
                                                                    kontraksi_uterus,
                                                                    kandung_kemih,
                                                                    pendarahan,
                                                                    masalah_kala_iv,
                                                                    penatalaksanaan_kala_iv) values ( 
                                                                    pemantauan_persalinan_iv_seq.nextval,
                                                                    '" + txt_anastesi_id.Text + @"',
                                                                    '" + row["URUTAN_KE"] + @"',
                                                                    '" + row["JAM_KE"] + @"',
                                                                    '" + row["WAKTU"] + @"',
                                                                    '" + row["TEKANAN_DARAH"] + @"',
                                                                    '" + row["NADI"] + @"',
                                                                    '" + row["TEMPERATUR"] + @"',
                                                                    '" + row["TINGGI_FUNDUS"] + @"',
                                                                    '" + row["KONTRAKSI_UTERUS"] + @"',
                                                                    '" + row["KANDUNG_KEMIH"] + @"',
                                                                    '" + row["PENDARAHAN"] + @"',
                                                                    '" + txt_MasalahKala4.Text + @"',
                                                                    '" + txt_PenatalaksanaKala4.Text + @"'
                                                            ) ";

                        success = koneksi.ExecuteNonQuery(queryInsert);



                    }

                }

                if (success)
                {
                    MessageBox.Show("Data Berhasil Disimpan");
                }
                else
                {
                    MessageBox.Show("Data Gagal Disimpan !!");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Data Gagal Disimpan !!");

            }
        }

        private void btnAddMonitoring3_Click(object sender, EventArgs e)
        {

            if (dt_grdSPemantauanAnastesih == null) return;

            DataRow newRow = dt_grdSPemantauanAnastesih.NewRow();

            newRow["URUTAN_KE"] = ((gvwPemantauanAnastesi.RowCount) + 1).ToString();
            dt_grdSPemantauanAnastesih.Rows.Add(newRow);

            grdPemantauanAnastesi.DataSource = dt_grdSPemantauanAnastesih;
        }

        private void btnSaveMonitoring3_Click(object sender, EventArgs e)
        {
            try
            {

                bool success = false;
                foreach (DataRow row in dt_grdSPemantauanAnastesih.Rows)
                {


                    string query = @"select count(*) from T2_PMT_ANASTESI where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' ";
                    object result = koneksi.GetScalar(query);
                    if (Convert.ToInt32(result) >= 1)
                    {
                        string queryInsert = @"update  T2_PMT_ANASTESI set
                                                            urutan_ke   = '" + row["URUTAN_KE"] + @"',
                                                            nama_obat   = '" + row["NAMA_OBAT"] + @"',
                                                            dosis       = '" + row["DOSIS"] + @"',
                                                            rr          = '" + row["RR"] + @"',
                                                            hr          = '" + row["HR"] + @"',
                                                            tds         = '" + row["TDS"] + @"',
                                                            tdd         = '" + row["TDD"] + @"',
                                                            waktu_15    = '" + row["WAKTU_15"] + @"',
                                                            waktu_30    = '" + row["WAKTU_30"] + @"',
                                                            waktu_45    = '" + row["WAKTU_45"] + @"',
                                                            waktu_60    = '" + row["WAKTU_60"] + @"',
                                                            waktu_90    = '" + row["WAKTU_90"] + @"',
                                                            keterangan  = '" + row["KETERANGAN"] + @"',
                                                            jam_mulai   = '" + txtJam.Text + @"',
                                                            tanggal     = '" + date_pemantauan.Text + @"',
                                                            kesadaran   = '" + txtKesadaran.Text + @"',
                                                            jam_selesai_tindakan = '" + txtJamSelesai.Text + @"'
                                                      where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' ";

                        success = koneksi.ExecuteNonQuery(queryInsert);

                    }
                    else
                    {
                        string queryInsert = @"insert into T2_PMT_ANASTESI(
                                                                    id,
                                                                    anamesa_id,
                                                                    urutan_ke,
                                                                    nama_obat,
                                                                    dosis,
                                                                    rr,
                                                                    hr,
                                                                    tds,
                                                                    tdd,
                                                                    waktu_15,
                                                                    waktu_30,
                                                                    waktu_45,
                                                                    waktu_60,
                                                                    waktu_90,
                                                                    keterangan,
                                                                    jam_mulai,
                                                                    tanggal,
                                                                    kesadaran,
                                                                    jam_selesai_tindakan) values (
                                                                    pemantauan_anastesi_seq.nextval,
                                                                    '" + txt_anastesi_id.Text + @"',
                                                                    '" + row["URUTAN_KE"] + @"',
                                                                    '" + row["NAMA_OBAT"] + @"',
                                                                    '" + row["DOSIS"] + @"',
                                                                    '" + row["RR"] + @"',
                                                                    '" + row["HR"] + @"',
                                                                    '" + row["TDS"] + @"',
                                                                    '" + row["TDD"] + @"',
                                                                    '" + row["WAKTU_15"] + @"',
                                                                    '" + row["WAKTU_30"] + @"',
                                                                    '" + row["WAKTU_45"] + @"',
                                                                    '" + row["WAKTU_60"] + @"',
                                                                    '" + row["WAKTU_90"] + @"',
                                                                    '" + row["KETERANGAN"] + @"',
                                                                    '" + txtJam.Text + @"',
                                                                    '" + date_pemantauan.Text + @"',
                                                                    '" + txtKesadaran.Text + @"',
                                                                    '" + txtJamSelesai.Text + @"'
                                                            ) ";

                        success = koneksi.ExecuteNonQuery(queryInsert);



                    }

                }

                if (success)
                {
                    MessageBox.Show("Data Berhasil Disimpan");
                }
                else
                {
                    MessageBox.Show("Data Gagal Disimpan !!");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Data Gagal Disimpan !!");

            }

        }

        private void btnAddMonitoring4_Click(object sender, EventArgs e)
        {

            if (dt_grdSetelahBedah == null) return;

            DataRow newRow = dt_grdSetelahBedah.NewRow();

            newRow["URUTAN_KE"] = ((gvwSetelahBedah.RowCount) + 1).ToString();
            dt_grdSetelahBedah.Rows.Add(newRow);

            grdSetelahBedah.DataSource = dt_grdSetelahBedah;
        }

        private void btnSaveMonitoring4_Click(object sender, EventArgs e)
        {
            try
            {

                bool success = false;
                foreach (DataRow row in dt_grdSetelahBedah.Rows)
                {


                    string query = @"select count(*) from T2_MTR_PEMBEDAHAN where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' and jenis = 'SETELAH'";
                    object result = koneksi.GetScalar(query);
                    if (Convert.ToInt32(result) >= 1)
                    {
                        string queryInsert = @"update  T2_MTR_PEMBEDAHAN set
                                                                    urutan_ke       = '" + row["URUTAN_KE"] + @"',
                                                                    tanggal         = '" + row["TANGGAL"] + @"',
                                                                    jam             = '" + row["JAM"] + @"',
                                                                    tindakan        = '" + row["TINDAKAN"] + @"',
                                                                    kesadaran       = '" + row["KESADARAN"] + @"',
                                                                    td              = '" + row["TD"] + @"',
                                                                    n               = '" + row["N"] + @"',
                                                                    rr              = '" + row["RR"] + @"',
                                                                    s               = '" + row["S"] + @"',
                                                                    keterangan      = '" + row["KETERANGAN"] + @"'
                                                      where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' and jenis = 'SETELAH' ";

                        success = koneksi.ExecuteNonQuery(queryInsert);

                    }
                    else
                    {
                        string queryInsert = @"insert into T2_MTR_PEMBEDAHAN(
                                                                id,
                                                                anamesa_id,
                                                                urutan_ke,
                                                                jenis,
                                                                tanggal,
                                                                jam,
                                                                tindakan,
                                                                kesadaran,
                                                                td,
                                                                n,
                                                                rr,
                                                                s,
                                                                keterangan) values (
                                                                monitoring_pembedahan_seq.nextval,
                                                                '" + txt_anastesi_id.Text + @"',
                                                                '" + row["URUTAN_KE"] + @"',
                                                                'SETELAH',
                                                                '" + row["TANGGAL"] + @"',
                                                                '" + row["JAM"] + @"',
                                                                '" + row["TINDAKAN"] + @"',
                                                                '" + row["KESADARAN"] + @"',
                                                                '" + row["TD"] + @"',
                                                                '" + row["N"] + @"',
                                                                '" + row["RR"] + @"',
                                                                '" + row["S"] + @"',
                                                                '" + row["KETERANGAN"] + @"'
                                                            ) ";

                        success = koneksi.ExecuteNonQuery(queryInsert);



                    }

                }

                if (success)
                {
                    MessageBox.Show("Data Berhasil Disimpan");
                }
                else
                {
                    MessageBox.Show("Data Gagal Disimpan !!");
                }


            }
            catch (Exception ex)
            {

            }

        }

        private void btnAddMonitoring2_Click(object sender, EventArgs e)
        {

            if (dt_grdSebelumBedah == null) return;

            DataRow newRow = dt_grdSebelumBedah.NewRow();

            newRow["URUTAN_KE"] = ((gvwSebelumBedah.RowCount) + 1).ToString();
            dt_grdSebelumBedah.Rows.Add(newRow);

            grdSebelumBedah.DataSource = dt_grdSebelumBedah;
        }

        private void btnSaveMonitoring2_Click(object sender, EventArgs e)
        {
            try
            {

                bool success = false;
                foreach (DataRow row in dt_grdSebelumBedah.Rows)
                {


                    string query = @"select count(*) from T2_MTR_PEMBEDAHAN where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' and jenis = 'SEBELUM' ";
                    object result = koneksi.GetScalar(query);
                    if (Convert.ToInt32(result) >= 1)
                    {
                        string queryInsert = @"update  T2_MTR_PEMBEDAHAN set
                                                                    urutan_ke   = '" + row["URUTAN_KE"] + @"',
                                                                    tanggal     = '" + row["TANGGAL"] + @"',
                                                                    jam         = '" + row["JAM"] + @"',
                                                                    tindakan    = '" + row["TINDAKAN"] + @"',
                                                                    kesadaran   = '" + row["KESADARAN"] + @"',
                                                                    td          = '" + row["TD"] + @"',
                                                                    n           = '" + row["N"] + @"',
                                                                    rr          = '" + row["RR"] + @"',
                                                                    s           = '" + row["S"] + @"',
                                                                    keterangan  = '" + row["KETERANGAN"] + @"'
                                                      where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' and jenis = 'SEBELUM' ";

                        success = koneksi.ExecuteNonQuery(queryInsert);

                    }
                    else
                    {
                        string queryInsert = @"insert into T2_MTR_PEMBEDAHAN(
                                                                id,
                                                                anamesa_id,
                                                                urutan_ke,
                                                                jenis,
                                                                tanggal,
                                                                jam,
                                                                tindakan,
                                                                kesadaran,
                                                                td,
                                                                n,
                                                                rr,
                                                                s,
                                                                keterangan) values (
                                                                monitoring_pembedahan_seq.nextval,
                                                                '" + txt_anastesi_id.Text + @"',
                                                                '" + row["URUTAN_KE"] + @"',
                                                                'SEBELUM',
                                                                '" + row["TANGGAL"] + @"',
                                                                '" + row["JAM"] + @"',
                                                                '" + row["TINDAKAN"] + @"',
                                                                '" + row["KESADARAN"] + @"',
                                                                '" + row["TD"] + @"',
                                                                '" + row["N"] + @"',
                                                                '" + row["RR"] + @"',
                                                                '" + row["S"] + @"',
                                                                '" + row["KETERANGAN"] + @"'
                                                            ) ";

                        success = koneksi.ExecuteNonQuery(queryInsert);



                    }

                }

                if (success)
                {
                    MessageBox.Show("Data Berhasil Disimpan");
                }
                else
                {
                    MessageBox.Show("Data Gagal Disimpan !!");
                }


            }
            catch (Exception ex)
            {

            }

        }

        private void btnAddMonitoring1_Click(object sender, EventArgs e)
        {

            if (dt_grdPemberianAnstesi == null) return;

            DataRow newRow = dt_grdPemberianAnstesi.NewRow();

            newRow["URUTAN_KE"] = ((gvwPemberianAnstesi.RowCount) + 1).ToString();
            dt_grdPemberianAnstesi.Rows.Add(newRow);

            grdPemberianAnstesi.DataSource = dt_grdPemberianAnstesi;
        }

        private void btnSaveMonitoring1_Click(object sender, EventArgs e)
        {
            try
            {

                bool success = false;
                foreach (DataRow row in dt_grdPemberianAnstesi.Rows)
                {


                    string query = @"select count(*) from T2_MTR_PEMBERIAN_ANAMNESA where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' ";
                    object result = koneksi.GetScalar(query);
                    if (Convert.ToInt32(result) >= 1)
                    {
                        string queryInsert = @"update  T2_MTR_PEMBERIAN_ANAMNESA set
                                                                urutan_ke       = '" + row["URUTAN_KE"] + @"',
                                                                nama_obat       = '" + row["NAMA_OBAT"] + @"',
                                                                dosis           = '" + row["DOSIS"] + @"',
                                                                teknik_anastesi = '" + row["TEKNIK_ANASTESI"] + @"',
                                                                waktu           = '" + row["WAKTU"] + @"',
                                                                tensi           = '" + row["TENSI"] + @"',
                                                                nadi            = '" + row["NADI"] + @"',
                                                                rr              = '" + row["RR"] + @"'
                                                      where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' ";

                        success = koneksi.ExecuteNonQuery(queryInsert);

                    }
                    else
                    {
                        string queryInsert = @"insert into T2_MTR_PEMBERIAN_ANAMNESA(
                                                                id,
                                                                anamesa_id,
                                                                urutan_ke,
                                                                nama_obat,
                                                                dosis,
                                                                teknik_anastesi,
                                                                waktu,
                                                                tensi,
                                                                nadi,
                                                                rr,
                                                                suhu) values (
                                                                m_pemberian_anastesi_seq.nextval,
                                                                '" + txt_anastesi_id.Text + @"',
                                                                '" + row["URUTAN_KE"] + @"',
                                                                '" + row["NAMA_OBAT"] + @"',
                                                                '" + row["DOSIS"] + @"',
                                                                '" + row["TEKNIK_ANASTESI"] + @"',
                                                                '" + row["WAKTU"] + @"',
                                                                '" + row["TENSI"] + @"',
                                                                '" + row["NADI"] + @"',
                                                                '" + row["RR"] + @"',
                                                                '" + row["SUHU"] + @"'
                                                            ) ";

                        success = koneksi.ExecuteNonQuery(queryInsert);



                    }

                }

                if (success)
                {
                    MessageBox.Show("Data Berhasil Disimpan");
                }
                else
                {
                    MessageBox.Show("Data Gagal Disimpan !!");
                }


            }
            catch (Exception ex)
            {

            }

        }

        private void btnAddPersalinanLalu_Click(object sender, EventArgs e)
        {


            if (dt_grdPersalinanLalu == null) return;

            DataRow newRow = dt_grdPersalinanLalu.NewRow();
            newRow["HAMIL_KE"] = ((gvwPersalinanLalu.RowCount) + 1).ToString();

            dt_grdPersalinanLalu.Rows.Add(newRow);

            grdPersalinanLalu.DataSource = dt_grdPersalinanLalu;
        }
        private void btnSavePersalinanLalu_Click(object sender, EventArgs e)
        {
            try
            {

                bool success = false;
                foreach (DataRow row in dt_grdPersalinanLalu.Rows)
                {


                    string query = @"select count(*) from T2_RIWAYAT_PERSALINAN_LALU where anamesa_id = '" + txt_anastesi_id.Text + "' and hamil_ke = '" + row["HAMIL_KE"] + "' ";
                    object result = koneksi.GetScalar(query);
                    if (Convert.ToInt32(result) >= 1)
                    {


                        string queryInsert = @"update  T2_RIWAYAT_PERSALINAN_LALU set
                                                            hamil_ke            = '" + row["HAMIL_KE"] + @"',
                                                            umur_kehamilan      = '" + row["UMUR_KEHAMILAN"] + @"',
                                                            tahun_persalinan    = '" + row["TAHUN_PERSALINAN"] + @"',
                                                            penolong            = '" + row["PENOLONG"] + @"',
                                                            cara_persalinan     = '" + row["CARA_PERSALINAN"] + @"',
                                                            riwayat_komplikasi  = '" + row["RIWAYAT_KOMPLIKASI"] + @"',
                                                            tmpt_persalinan     = '" + row["TMPT_PERSALINAN"] + @"',
                                                            jk                  = '" + row["JK"] + @"',
                                                            bb                  = '" + row["BB"] + @"',
                                                            pb                  = '" + row["PB"] + @"',
                                                            bayi_h              = '" + row["BAYI_H"] + @"',
                                                            bayi_m              = '" + row["BAYI_M"] + @"'
                                                      where anamesa_id = '" + txt_anastesi_id.Text + "' and hamil_ke = '" + row["HAMIL_KE"] + "' ";

                        success = koneksi.ExecuteNonQuery(queryInsert);

                    }
                    else
                    {
                        string queryInsert = @"insert into T2_RIWAYAT_PERSALINAN_LALU(
                                                            id,
                                                            anamesa_id,
                                                            hamil_ke,
                                                            umur_kehamilan,
                                                            tahun_persalinan,
                                                            penolong,
                                                            cara_persalinan,
                                                            riwayat_komplikasi,
                                                            tmpt_persalinan,
                                                            jk,
                                                            bb,
                                                            pb,
                                                            bayi_h,
                                                            bayi_m) 
                                                    values (
                                                            riwayat_persalinan_lalu_seq.nextval,
                                                            '" + txt_anastesi_id.Text + @"',
                                                            '" + row["HAMIL_KE"] + @"',
                                                            '" + row["UMUR_KEHAMILAN"] + @"',
                                                            '" + row["TAHUN_PERSALINAN"] + @"',
                                                            '" + row["PENOLONG"] + @"',
                                                            '" + row["CARA_PERSALINAN"] + @"',
                                                            '" + row["RIWAYAT_KOMPLIKASI"] + @"',
                                                            '" + row["TMPT_PERSALINAN"] + @"',
                                                            '" + row["JK"] + @"',
                                                            '" + row["BB"] + @"',
                                                            '" + row["PB"] + @"',
                                                            '" + row["BAYI_H"] + @"',
                                                            '" + row["BAYI_M"] + @"'
                                                            ) ";

                        success = koneksi.ExecuteNonQuery(queryInsert);



                    }

                }

                if (success)
                {
                    MessageBox.Show("Data Berhasil Disimpan");
                }
                else
                {
                    MessageBox.Show("Data Gagal Disimpan !!");
                }


            }
            catch (Exception ex)
            {

            }
        }



        private void updateTable()
        {
            try
            {

                List<string> updateQueries = new List<string>
                {

                };

                updateQueries.Add(@" update T2_R_INAP_BIDAN set 
                                            nama_istr               = '" + txt_nama_istr.Text + @"',
                                            umur_istr               = '" + txt_umur_istr.Text + @"',
                                            agama_istr              = '" + txt_agama_istr.Text + @"',
                                            pendidikan_istr         = '" + txt_pendidikan_istr.Text + @"',
                                            pekerjaan_istr          = '" + txt_pekerjaan_istr.Text + @"',
                                            suku_istr               = '" + txt_suku_istr.Text + @"',
                                            kawin_lama_istr         = '" + txt_kawin_lama_istr.Text + @"',
                                            kawin_frek_istr         = '" + txt_kawin_frek_istr.Text + @"',
                                            nama_suami              = '" + txt_nama_suami.Text + @"',
                                            umur_suami              = '" + txt_umur_suami.Text + @"',
                                            agama_suami             = '" + txt_agama_suami.Text + @"',
                                            pendidikan_suami        = '" + txt_pendidikan_suami.Text + @"',
                                            pekerjaan_suami         = '" + txt_pekerjaan_suami.Text + @"',
                                            suku_suami              = '" + txt_suku_suami.Text + @"',
                                            kawin_lama_suami        = '" + txt_kawin_lama_suami.Text + @"',
                                            kawin_frek_suami        = '" + txt_kawin_frek_suami.Text + @"',
                                            biodata_alamat          = '" + txt_biodata_alamat.Text + @"',
                                            biodata_keluhan         = '" + txt_biodata_keluhan.Text + @"',
                                            rwyt_hamil_a            = '" + txt_rwyt_hamil_a.Text + @"',
                                            rwyt_hamil_hpht_fr      = '" + date_rwyt_hamil_hpht_fr.Text + @"',
                                            rwyt_hamil_hpht_to      = '" + date_rwyt_hamil_hpht_to.Text + @"',
                                            rwyt_hamil_anc          = '" + date_rwyt_hamil_anc.Text + @"',
                                            rwyt_hamil_komp         = '" + txt_rwyt_hamil_komp.Text + @"',
                                            rwyt_kb_kontra          = '" + txt_rwyt_kb_kontra.Text + @"',
                                            rwyt_kb_lama            = '" + txt_rwyt_kb_lama.Text + @"',
                                            rwyt_kb_alasan_henti    = '" + txt_rwyt_kb_alasan_henti.Text + @"',
                                            rwyt_penyakit_ibu       = '" + txt_rwyt_penyakit_ibu.Text + @"',
                                            aktiv_nutrisi           = '" + txt_aktiv_nutrisi.Text + @"',
                                            aktiv_eliminasi         = '" + txt_aktiv_eliminasi.Text + @"',
                                            aktiv_istirahat         = '" + txt_aktiv_istirahat.Text + @"',
                                            k_umum                  = '" + txt_k_umum.Text + @"',
                                            k_sadar                 = '" + txt_k_sadar.Text + @"',
                                            umum_vita_td            = '" + txt_umum_vita_td.Text + "::" + txt_umum_vita_td_1.Text + @"',    
                                            umum_vital_n            = '" + txt_umum_vital_n.Text + @"',
                                            umum_vital_r            = '" + txt_umum_vital_r.Text + @"',
                                            umum_vital_s            = '" + txt_umum_vital_s.Text + @"',
                                            ekstermitas_atas        = '" + txt_ekstermitas_atas.Text + @"',
                                            ekstermitas_bawah       = '" + txt_ekstermitas_bawah.Text + @"',
                                            asesment_awal           = '" + txt_asesment_awal.Text + @"',
                                            planning_awal           = '" + txt_planning_awal.Text + @"',
                                            gen_inpeksi             = '" + txt_gen_inpeksi.Text + @"',
                                            gen_vulva               = '" + txt_gen_vulva.Text + @"',
                                            gen_portio              = '" + txt_gen_portio.Text + @"',
                                            gen_pembukaan           = '" + txt_gen_pembukaan.Text + @"',
                                            gen_penurunan           = '" + txt_gen_penurunan.Text + @"',
                                            gen_ketuban             = '" + txt_gen_ketuban.Text + @"',
                                            gen_presntasi           = '" + txt_gen_presntasi.Text + @"'
                                     where anamesa_id = '" + txt_anastesi_id.Text + "' ");


                updateQueries.Add(@" update T2_R_INAP_BIDAN_1 set 
                                            fisik_mata              = '" + txt_fisik_mata.Text + @"',
                                            fisik_leher             = '" + txt_fisik_leher.Text + @"',
                                            fisik_payudara          = '" + txt_fisik_payudara.Text + @"',
                                            fisik_inpeksi           = '" + txt_fisik_inpeksi.Text + @"',
                                            fisik_leopold_i         = '" + txt_fisik_leopold_i.Text + @"',
                                            fisik_leopold_ii        = '" + txt_fisik_leopold_ii.Text + @"',
                                            fisik_leopold_iii       = '" + txt_fisik_leopold_iii.Text + @"',
                                            fisik_leopold_iv        = '" + txt_fisik_leopold_iv.Text + @"',
                                            fisik_tfu               = '" + txt_fisik_tfu.Text + @"',
                                            fisik_djj               = '" + txt_fisik_djj.Text + @"',
                                            fisik_his               = '" + txt_fisik_his.Text + @"',
                                            penunjang_i             = '" + txt_penunjang_i.Text + @"',
                                            penunjang_iii           = '" + txt_penunjang_iii.Text + @"',
                                            protein_urine           = '" + txt_protein_urine.Text + @"',
                                            glukosa_urine           = '" + txt_glukosa_urine.Text + @"',
                                            tgl_persalinan          = '" + date_tgl_persalinan.Text + @"',
                                            nama_bidan              = '" + txt_nama_bidan.Text + @"'
                                     where anamesa_id = '" + txt_anastesi_id.Text + "' ");


                updateQueries.Add(@" update T2_R_INAP_BIDAN_1 set 
                                            tempat_persalinan       = '" + rb_tempat_persalinan.SelectedIndex.ToString() + "::" + rb_tempat_persalinan.Text + "::" + txt_tempat_persalinan.Text + @"',
                                            alamat_persalinan       = '" + txt_alamat_persalinan.Text + @"',
                                            rujuk_kala              = '" + rb_rujuk_kala.SelectedIndex.ToString() + "::" + rb_rujuk_kala.Text + @"',
                                            alasan_rujuk            = '" + txt_alasan_rujuk.Text + @"',
                                            tempat_rujuk            = '" + txt_tempat_rujuk.Text + @"',
                                            pendamping_rujuk        = '" + rb_pendamping_rujuk.SelectedIndex.ToString() + "::" + rb_pendamping_rujuk.Text + @"',
                                            kala_i_a                = '" + rb_kala_i_a.SelectedIndex.ToString() + "::" + rb_kala_i_a.Text + @"',
                                            kala_i_b                = '" + txt_kala_i_b.Text + @"',
                                            kala_i_c                = '" + txt_kala_i_c.Text + @"',
                                            kala_i_d                = '" + txt_kala_i_d.Text + @"'
                                     where anamesa_id = '" + txt_anastesi_id.Text + "' ");


                updateQueries.Add(@" update T2_R_INAP_BIDAN_1 set 
                                            kala_ii_a               = '" + rb_kala_ii_a.SelectedIndex.ToString() + "::" + rb_kala_ii_a.Text + "::" + txt_kala_ii_a.Text + @"',
                                            kala_ii_b               = '" + rb_kala_ii_b.SelectedIndex.ToString() + "::" + rb_kala_ii_b.Text + @"',
                                            kala_ii_c               = '" + rb_kala_ii_c.SelectedIndex.ToString() + "::" + rb_kala_ii_c.Text + "::" + txt_kala_ii_c.Text + @"',
                                            kala_ii_d               = '" + rb_kala_ii_d.SelectedIndex.ToString() + "::" + rb_kala_ii_d.Text + "::" + txt_kala_ii_d.Text + @"',
                                            kala_ii_e               = '" + txt_kala_ii_e.Text + @"',
                                            kala_ii_f               = '" + txt_kala_ii_f.Text + @"',
                                            kala_ii_g               = '" + txt_kala_ii_g.Text + @"',
                                            kala_iii_a              = '" + txt_kala_iii_a.Text + @"',
                                            kala_iii_b              = '" + rb_kala_iii_b.SelectedIndex.ToString() + "::" + rb_kala_iii_b.Text + "::" + txt_kala_iii_b.Text + @"',
                                            kala_iii_c              = '" + rb_kala_iii_c.SelectedIndex.ToString() + "::" + rb_kala_iii_c.Text + "::" + txt_kala_iii_c.Text + @"',
                                            kala_iii_d              = '" + rb_kala_iii_d.SelectedIndex.ToString() + "::" + rb_kala_iii_d.Text + "::" + txt_kala_iii_d.Text + @"',
                                            baru_lahir_berat        = '" + txt_baru_lahir_berat.Text + @"',
                                            baru_lahir_panjang      = '" + txt_baru_lahir_panjang.Text + @"',
                                            baru_lahir_jk           = '" + rb_baru_lahir_jk.SelectedIndex.ToString() + "::" + rb_baru_lahir_jk.Text + @"',
                                            baru_lahir_nilai        = '" + rb_baru_lahir_nilai.SelectedIndex.ToString() + "::" + rb_baru_lahir_nilai.Text + @"',
                                            baru_lahir_ket          = '" + (chk_baru_lahir_ket.Checked ? chk_baru_lahir_ket.Text : "") + "::" + (chk_baru_lahir_ket_1.Checked ? chk_baru_lahir_ket_1.Text : "") + "::" + (chk_baru_lahir_ket_2.Checked ? chk_baru_lahir_ket_2.Text : "") + "::" + (chk_baru_lahir_ket_3.Checked ? chk_baru_lahir_ket_3.Text : "") + "::" + (chk_baru_lahir_ket_4.Checked ? chk_baru_lahir_ket_4.Text : "") + "::" + (chk_baru_lahir_ket_5.Checked ? chk_baru_lahir_ket_5.Text : "") + @"'
                                     where anamesa_id = '" + txt_anastesi_id.Text + "' ");



                updateQueries.Add(@" update T2_R_INAP_BIDAN_2 set 
                                            rangsang_laktil         = '" + rb_rangsang_laktil.SelectedIndex.ToString() + "::" + rb_rangsang_laktil.Text + "::" + txt_rangsang_laktil.Text + @"',
                                            plasenta_intack         = '" + rb_plasenta_intack.SelectedIndex.ToString() + "::" + rb_plasenta_intack.Text + "::" + txt_rangsang_laktil.Text + @"',
                                            plasenta_tidak_lahir    = '" + rb_plasenta_tidak_lahir.SelectedIndex.ToString() + "::" + rb_plasenta_tidak_lahir.Text + "::" + txt_rangsang_laktil.Text + @"',
                                            laserasi                = '" + rb_laserasi.SelectedIndex.ToString() + "::" + rb_laserasi.Text + "::" + txt_rangsang_laktil.Text + @"',
                                            laserasi_parinium       = '" + rb_laserasi_parinium.SelectedIndex.ToString() + "::" + rb_laserasi_parinium.Text + "::" + rb_laserasi_parinium_tindakan.SelectedIndex.ToString() + "::" + rb_laserasi_parinium_tindakan.Text + "::" + txt_rangsang_laktil.Text + @"',
                                            atonia_uteri            = '" + rb_atonia_uteri.SelectedIndex.ToString() + "::" + rb_atonia_uteri.Text + "::" + txt_atonia_uteri.Text + @"',
                                            jumlah_pendarahan       = '" + txt_jumlah_pendarahan.Text + @"',
                                            masalah_lain            = '" + txt_masalah_lain.Text + @"',
                                            penata_masalah          = '" + txt_penata_masalah.Text + @"',
                                            hasilnya                = '" + txt_hasilnya.Text + @"',
                                            baru_lahir              = '" + rb_baru_lahir.SelectedIndex.ToString() + "::" + rb_baru_lahir.Text + "::" + (chk_cacat_ket_1.Checked ? chk_cacat_ket_1.Text : "") + "::" + (chk_cacat_ket_2.Checked ? chk_cacat_ket_2.Text : "") + "::" + (chk_cacat_ket_3.Checked ? chk_cacat_ket_3.Text : "") + "::" + (chk_cacat_ket_4.Checked ? chk_cacat_ket_4.Text : "") + "::" + (chk_cacat_ket_5.Checked ? chk_cacat_ket_5.Text : "") + "::" + (chk_cacat_ket_6.Checked ? chk_cacat_ket_6.Text : "") + "::" + (chk_cacat_ket_lain.Checked ? chk_cacat_ket_lain.Text : "") + "::" + txt_cacat_ket_lain.Text + @"',
                                            bayi_cacat              = '" + txt_bayi_cacat.Text + @"',
                                            hipotermia              = '" + txt_hipotermia.Text + @"',
                                            pemberian_asi           = '" + rb_pemberian_asi.SelectedIndex.ToString() + "::" + rb_pemberian_asi.Text + "::" + txt_pemberian_asi_ya.Text + "::" + txt_pemberian_asi_tdk.Text + @"',
                                            masalah_lahir           = '" + txt_masalah_lahir.Text + @"',
                                            hasil_lahir             = '" + txt_hasil_lahir.Text + @"'
                                     where anamesa_id = '" + txt_anastesi_id.Text + "' ");



                updateQueries.Add(@" update T2_DOKUMEN_SKALA_I set 
                                                tanggal             = '" + txt_skl1_tanggal.Text + @"',
                                                jam                 = '" + txt_skl1_jam.Text + @"',
                                                keluhan_utama       = '" + txt_skl1_keluhan_utama.Text + @"',
                                                kesadaran           = '" + txt_skl1_kesadaran.Text + @"',
                                                td_1                = '" + txt_skl1_td_1.Text + @"',
                                                td_2                = '" + txt_skl1_td_2.Text + @"',
                                                n                   = '" + txt_skl1_n.Text + @"',
                                                r                   = '" + txt_skl1_r.Text + @"',
                                                s                   = '" + txt_skl1_s.Text + @"',
                                                frekuensi           = '" + txt_skl1_frekuensi.Text + @"',
                                                interfal            = '" + txt_skl1_interfal.Text + @"',
                                                durasi              = '" + txt_skl1_durasi.Text + @"',
                                                djj                 = '" + rb_skl1_djj.SelectedIndex.ToString() + "::" + rb_skl1_djj.Text + "::" + txt_skl1_djj.Text + @"',
                                                diagnosa            = '" + txt_skl1_diagnosa.Text + @"',
                                                masalah_potensial   = '" + txt_skl1_masalah_potensial.Text + @"',
                                                antisipasi_masalah  = '" + txt_skl1_antisipasi_masalah.Text + @"',
                                                vulva               = '" + txt_skl1_vulva.Text + @"',
                                                pembukaan           = '" + txt_skl1_pembukaan.Text + @"',
                                                keadaan_ketuban     = '" + txt_skl1_keadaan_ketuban.Text + @"',
                                                presentasi          = '" + txt_skl1_presentasi.Text + @"',
                                                bagian_teraba       = '" + txt_skl1_bagian_teraba.Text + @"',
                                                turunnya_bagian     = '" + txt_skl1_turunnya_bagian.Text + @"',
                                                molage              = '" + txt_skl1_molage.Text + @"',
                                                vesica_urineria     = '" + rb_skl1_vesica_urineria.SelectedIndex.ToString() + "::" + rb_skl1_vesica_urineria.Text + @"',
                                                planning            = '" + txt_skl1_planning.Text + @"',
                                                jam_plan            = '" + txt_skl1_jam_plan.Text + @"',
                                                tgl_plan            = '" + txt_skl1_tgl_plan.Text + @"',
                                                dipimpin            = '" + txt_skl1_dipimpin.Text + @"',
                                                jam_mulai           = '" + txt_skl1_jam_mulai.Text + @"'
                                     where anamesa_id = '" + txt_anastesi_id.Text + "' ");


                updateQueries.Add(@"update T2_DOKUMEN_SKALA_II set
                                                tanggal         = '" + txt_skl2_tanggal.Text + @"',
                                                jam             = '" + txt_skl2_jam.Text + @"',
                                                keluhan_utama   = '" + txt_skl2_keluhan_utama.Text + @"',
                                                kesadaran       = '" + txt_skl2_kesadaran.Text + @"',
                                                td_1            = '" + txt_skl2_td_1.Text + @"',
                                                td_2            = '" + txt_skl2_td_2.Text + @"',
                                                n               = '" + txt_skl2_n.Text + @"',
                                                r               = '" + txt_skl2_r.Text + @"',
                                                s               = '" + txt_skl2_s.Text + @"',
                                                frekuensi       = '" + txt_skl2_frekuensi.Text + @"',
                                                interfal        = '" + txt_skl2_interfal.Text + @"',
                                                durasi          = '" + txt_skl2_durasi.Text + @"',
                                                djj             = '" + rb_skl2_djj.SelectedIndex.ToString() + "::" + rb_skl2_djj.Text + "::" + txt_skl2_djj.Text + @"',
                                                diagnosa        = '" + txt_skl2_diagnosa.Text + @"',
                                                masalah_potensial   = '" + txt_skl2_masalah_potensial.Text + @"',
                                                antisipasi_masalah  = '" + txt_skl2_antisipasi_masalah.Text + @"',
                                                vulva               = '" + txt_skl2_vulva.Text + @"',
                                                pembukaan           = '" + txt_skl2_pembukaan.Text + @"',
                                                keadaan_ketuban     = '" + txt_skl2_keadaan_ketuban.Text + @"',
                                                presentasi          = '" + txt_skl2_presentasi.Text + @"',
                                                bagian_teraba       = '" + txt_skl2_bagian_teraba.Text + @"',
                                                turunnya_bagian     = '" + txt_skl2_turunnya_bagian.Text + @"',
                                                molage              = '" + txt_skl2_molage.Text + @"',
                                                vesica_urineria     = '" + rb_skl2_vesica_urineria.SelectedIndex.ToString() + "::" + rb_skl2_vesica_urineria.Text + @"',
                                                planning            = '" + txt_skl2_planning.Text + @"',
                                                lahir_jam           = '" + txt_skl2_lahir_jam.Text + @"',
                                                jk                  = '" + txt_skl2_jk.Text + @"',
                                                bb                  = '" + txt_skl2_bb.Text + @"',
                                                lk                  = '" + txt_skl2_lk.Text + @"',
                                                ld                  = '" + txt_skl2_ld.Text + @"',
                                                kadaan_lahir        = '" + txt_skl2_kadaan_lahir.Text + @"',
                                                evaluasi            = '" + txt_skl2_evaluasi.Text + @"'
                                     where anamesa_id = '" + txt_anastesi_id.Text + "' ");



                updateQueries.Add(@"update T2_DOKUMEN_SKALA_III set
                                            tanggal         = '" + txt_skl3_tanggal.Text + @"',
                                            jam             = '" + txt_skl3_jam.Text + @"',
                                            keluhan_utama   = '" + txt_skl3_keluhan_utama.Text + @"',
                                            kesadaran       = '" + txt_skl3_kesadaran.Text + @"',
                                            td_1            = '" + txt_skl3_td_1.Text + @"',
                                            td_2            = '" + txt_skl3_td_2.Text + @"',
                                            n               = '" + txt_skl3_n.Text + @"',
                                            r               = '" + txt_skl3_r.Text + @"',
                                            s               = '" + txt_skl3_s.Text + @"',
                                            palpasi_abdomen = '" + txt_skl3_palpasi_abdomen.Text + @"',
                                            kontraksi_uterus    = '" + cb_skl3_kontraksi_uterus_1.Text + "::" + cb_skl3_kontraksi_uterus_2.Text + "::" + cb_skl3_kontraksi_uterus_3.Text + @"',
                                            uterus_membulat     = '" + rb_skl3_uterus_membulat.SelectedIndex.ToString() + "::" + rb_skl3_uterus_membulat.Text + @"',
                                            tinggi_fundus       = '" + txt_skl3_tinggi_fundus.Text + @"',
                                            semburan_darah      = '" + rb_skl3_semburan_darah.SelectedIndex.ToString() + "::" + rb_skl3_semburan_darah.Text + @"',
                                            vesica_urineria     = '" + rb_skl3_vesica_urineria.SelectedIndex.ToString() + "::" + rb_skl3_vesica_urineria.Text + @"',
                                            diagnosa            = '" + txt_skl3_diagnosa.Text + @"',
                                            masalah_potensial   = '" + txt_skl3_masalah_potensial.Text + @"',
                                            antisipasi_masalah  = '" + txt_skl3_antisipasi_masalah.Text + @"',
                                            planning            = '" + txt_skl3_planning.Text + @"',
                                            placenta_lahir      = '" + txt_skl3_placenta_lahir.Text + @"',
                                            spontan             = '" + rb_skl3_spontan.SelectedIndex.ToString() + "::" + rb_skl3_spontan.Text + @"',
                                            lengkap             = '" + rb_skl3_lengkap.SelectedIndex.ToString() + "::" + rb_skl3_lengkap.Text + @"',
                                            kontraksi           = '" + txt_skl3_kontraksi.Text + @"',
                                            pendarahan          = '" + txt_skl3_pendarahan.Text + @"',
                                            keadaan_jalan       = '" + txt_skl3_keadaan_jalan.Text + @"',
                                            bila_reptura        = '" + rb_skl3_bila_reptura.SelectedIndex.ToString() + "::" + rb_skl3_bila_reptura.Text + @"'
                                     where anamesa_id = '" + txt_anastesi_id.Text + "' ");




                updateQueries.Add(@"update T2_DOKUMEN_SKALA_IV set
                                            tanggal         = '" + txt_skl4_tanggal.Text + @"',
                                            jam             = '" + txt_skl4_jam.Text + @"',
                                            keluhan_utama   = '" + txt_skl4_keluhan_utama.Text + @"',
                                            kesadaran       = '" + txt_skl4_kesadaran.Text + @"',
                                            td_1            = '" + txt_skl4_td_1.Text + @"',
                                            td_2            = '" + txt_skl4_td_2.Text + @"',
                                            n               = '" + txt_skl4_n.Text + @"',
                                            r               = '" + txt_skl4_r.Text + @"',
                                            s               = '" + txt_skl4_s.Text + @"',
                                            kontraksi_uterus = '" + cb_skl4_kontraksi_uterus_1.Text + "::" + cb_skl4_kontraksi_uterus_2.Text + "::" + cb_skl4_kontraksi_uterus_3.Text + @"',
                                            tinggi_fundus   = '" + txt_skl4_tinggi_fundus.Text + @"',
                                            vesica_urineria = '" + rb_skl4_vesica_urineria.SelectedIndex.ToString() + "::" + rb_skl4_vesica_urineria.Text + @"',
                                            jumlah_darah    = '" + txt_skl4_jumlah_darah.Text + @"',
                                            diagnosa        = '" + txt_skl4_diagnosa.Text + @"',
                                            masalah_potensial   = '" + txt_skl4_masalah_potensial.Text + @"',
                                            antisipasi_masalah  = '" + txt_skl4_antisipasi_masalah.Text + @"',
                                            planning        = '" + txt_skl4_planning.Text + @"'
                                     where anamesa_id  = '" + txt_anastesi_id.Text + "' ");


                if (cb_ada_tindakan.Checked)
                {
                    updateQueries.Add(@"update T2_TINDAKAN_BIDAN set
                                            ada_tindakan        = '" + (cb_ada_tindakan.Checked ? cb_ada_tindakan.Text : "Tidak") + @"',
                                            nama_tindakan       = '" + txt_nama_tindakan.Text + @"',
                                            dokter_pelaksana    = '" + txt_dokter_pelaksana.Text + @"',
                                            pemberi_informasi   = '" + txt_pemberi_informasi.Text + @"',
                                            penerima_informasi  = '" + txt_penerima_informasi.Text + @"',
                                            tgl_diberikan       = '" + date_tgl_diberikan.Text + @"',
                                            diagnosis           = '" + txt_diagnosis.Text + @"',
                                            dasar_diagnosis     = '" + txt_dasar_diagnosis.Text + @"',
                                            tindakan_medis      = '" + txt_tindakan_medis.Text + @"',
                                            indikasi_tindakan   = '" + txt_indikasi_tindakan.Text + @"',
                                            tata_cara           = '" + txt_tata_cara.Text + @"',
                                            tujuan              = '" + txt_tujuan.Text + @"',
                                            resiko              = '" + txt_resiko.Text + @"',
                                            komplikasi          = '" + txt_komplikasi.Text + @"',
                                            prognosis           = '" + txt_prognosis.Text + @"',
                                            alternatif          = '" + txt_alternatif.Text + @"',
                                            lain_lain           = '" + txt_lain_lain.Text + @"',
                                            keluhan_utama       = '" + txt_keluhan_utama.Text + @"',
                                            riwayat_penyakit_sekarang   = '" + txt_riwayat_penyakit_sekarang.Text + @"',
                                            riwayat_penyakit_dahulu     = '" + txt_riwayat_penyakit_dahulu.Text + @"',
                                            riwayat_penyakit_keluarga   = '" + txt_riwayat_penyakit_keluarga.Text + @"',
                                            riwayat_alergi      = '" + txt_riwayat_alergi.Text + @"',
                                            riwayat_pengobatan  = '" + txt_riwayat_pengobatan.Text + @"',
                                            keadaan_umum        = '" + txt_keadaan_umum.Text + @"',
                                            kesadaran           = '" + txt_kesadaran.Text + @"',
                                            fisik_tanda_vital   = '" + txt_fisik_tanda_vital.Text + @"',
                                            tekanan_darah       = '" + txt_tekanan_darah.Text + @"',
                                            nadi                = '" + txt_nadi.Text + @"',
                                            suhu                = '" + txt_suhu.Text + @"',
                                            rr                  = '" + txt_rr.Text + @"',
                                            berat_badan         = '" + txt_berat_badan.Text + @"',
                                            status_gizi         = '" + txt_status_gizi.Text + @"',
                                            kepala_leher        = '" + txt_kepala_leher.Text + @"',
                                            thoraks             = '" + txt_thoraks.Text + @"',
                                            abdomen             = '" + txt_abdomen.Text + @"',
                                            extremitas          = '" + txt_extremitas.Text + @"',
                                            laboratorium        = '" + txt_laboratorium.Text + @"',
                                            rontgen             = '" + txt_rontgen.Text + @"',
                                            penunjang_lain_lain = '" + txt_penunjang_lain_lain.Text + @"',
                                            diagnosa_prabedah   = '" + txt_diagnosa_prabedah.Text + @"',
                                            rencana_tindakan    = '" + txt_rencana_tindakan.Text + @"',
                                            rencana_monitoring  = '" + txt_rencana_monitoring.Text + @"',
                                            rencana_edukasi     = '" + txt_rencana_edukasi.Text + @"',
                                            pasien_alergi       = '" + rb_pasien_alergi.SelectedIndex.ToString() + "::" + rb_pasien_alergi.Text + @"',
                                            risiko_aspirasi     = '" + rb_risiko_aspirasi.SelectedIndex.ToString() + "::" + rb_risiko_aspirasi.Text + @"',
                                            pendarahan          = '" + rb_pendarahan.SelectedIndex.ToString() + "::" + rb_pendarahan.Text + @"',
                                            profilaksi          = '" + rb_profilaksi.SelectedIndex.ToString() + "::" + rb_profilaksi.Text + @"',
                                            hasil_imaging       = '" + rb_hasil_imaging.SelectedIndex.ToString() + "::" + rb_hasil_imaging.Text + @"'
                                     where anamesa_id = '" + txt_anastesi_id.Text + "' ");

                }
                else
                {

                    string query = @"select count(*) from T2_TINDAKAN_BIDAN tp where anamesa_id = '" + txt_anastesi_id.Text + "'";
                    object result = koneksi.GetScalar(query);

                    if (Convert.ToInt32(result) >= 1)
                    {
                        updateQueries.Add(@"update T2_TINDAKAN_BIDAN set
                                            ada_tindakan        = '" + (cb_ada_tindakan.Checked ? cb_ada_tindakan.Text : "Tidak") + @"'
                                     where anamesa_id = '" + txt_anastesi_id.Text + "' ");

                    }
                }



                koneksi.OpenConnection();
                koneksi.BeginTransaction();

                foreach (string updateQuery in updateQueries)
                {
                    bool success = koneksi.ExecuteNonQueryCommitRollback(updateQuery);
                    if (!success)
                    {
                        if (!success)
                        {
                            koneksi.RollbackTransaction();
                            MessageBox.Show("Data Gagal Disimpan !!");
                            return;
                        }
                    }
                }

                koneksi.CommitTransaction();
                MessageBox.Show("Data Berhasil Disimpan");

                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Data Gagal Disimpan !!");

            }
            finally
            {
                koneksi.CloseConnection();
            }

        }

        private void selectedIndexRb()
        {
            rb_tempat_persalinan.SelectedIndex = -1;
            rb_rujuk_kala.SelectedIndex = -1;
            rb_pendamping_rujuk.SelectedIndex = -1;
            rb_kala_i_a.SelectedIndex = -1;
            rb_kala_ii_a.SelectedIndex = -1;
            rb_kala_ii_b.SelectedIndex = -1;
            rb_kala_ii_c.SelectedIndex = -1;
            rb_kala_ii_d.SelectedIndex = -1;
            rb_kala_iii_b.SelectedIndex = -1;
            rb_kala_iii_c.SelectedIndex = -1;
            rb_kala_iii_d.SelectedIndex = -1;
            rb_pasien_alergi.SelectedIndex = -1;
            rb_risiko_aspirasi.SelectedIndex = -1;
            rb_pendarahan.SelectedIndex = -1;
            rb_profilaksi.SelectedIndex = -1;
            rb_hasil_imaging.SelectedIndex = -1;

            rb_rangsang_laktil.SelectedIndex = -1;
            rb_plasenta_intack.SelectedIndex = -1;
            rb_plasenta_tidak_lahir.SelectedIndex = -1;
            rb_laserasi.SelectedIndex = -1;
            rb_laserasi_parinium.SelectedIndex = -1;
            rb_atonia_uteri.SelectedIndex = -1;
            rb_baru_lahir.SelectedIndex = -1;
            rb_pemberian_asi.SelectedIndex = -1;

            rb_skl2_djj.SelectedIndex = -1;
            rb_skl2_vesica_urineria.SelectedIndex = -1;
            rb_skl3_uterus_membulat.SelectedIndex = -1;
            rb_skl3_semburan_darah.SelectedIndex = -1;
            rb_skl3_vesica_urineria.SelectedIndex = -1;
            rb_skl3_spontan.SelectedIndex = -1;
            rb_skl3_lengkap.SelectedIndex = -1;
            rb_skl3_bila_reptura.SelectedIndex = -1;
            rb_skl4_vesica_urineria.SelectedIndex = -1;



            //string aa = "aa";
            //string[] rb_tempat_persalinan_2 = aa.Split(new string[] { "::" }, StringSplitOptions.None);
            //MessageBox.Show(rb_tempat_persalinan_2.Length.ToString());

        }
        private void kondisiEnable(bool kondisi)
        {
            txt_nama_istr.Enabled = kondisi;
            txt_umur_istr.Enabled = kondisi;
            txt_agama_istr.Enabled = kondisi;
            txt_pendidikan_istr.Enabled = kondisi;
            txt_pekerjaan_istr.Enabled = kondisi;
            txt_suku_istr.Enabled = kondisi;
            txt_kawin_lama_istr.Enabled = kondisi;
            txt_kawin_frek_istr.Enabled = kondisi;
            txt_nama_suami.Enabled = kondisi;
            txt_umur_suami.Enabled = kondisi;
            txt_agama_suami.Enabled = kondisi;
            txt_pendidikan_suami.Enabled = kondisi;
            txt_pekerjaan_suami.Enabled = kondisi;
            txt_suku_suami.Enabled = kondisi;
            txt_kawin_lama_suami.Enabled = kondisi;
            txt_kawin_frek_suami.Enabled = kondisi;
            txt_biodata_alamat.Enabled = kondisi;
            txt_biodata_keluhan.Enabled = kondisi;
            txt_rwyt_hamil_a.Enabled = kondisi;
            date_rwyt_hamil_hpht_fr.Enabled = kondisi;
            date_rwyt_hamil_hpht_to.Enabled = kondisi;
            date_rwyt_hamil_anc.Enabled = kondisi;
            txt_rwyt_hamil_komp.Enabled = kondisi;
            txt_rwyt_kb_kontra.Enabled = kondisi;
            txt_rwyt_kb_lama.Enabled = kondisi;
            txt_rwyt_kb_alasan_henti.Enabled = kondisi;
            txt_rwyt_penyakit_ibu.Enabled = kondisi;
            txt_aktiv_nutrisi.Enabled = kondisi;
            txt_aktiv_eliminasi.Enabled = kondisi;
            txt_aktiv_istirahat.Enabled = kondisi;
            txt_k_umum.Enabled = kondisi;
            txt_k_sadar.Enabled = kondisi;
            txt_umum_vita_td.Enabled = kondisi;
            txt_umum_vita_td_1.Enabled = kondisi;
            txt_umum_vital_n.Enabled = kondisi;
            txt_umum_vital_r.Enabled = kondisi;
            txt_umum_vital_s.Enabled = kondisi;
            txt_ekstermitas_atas.Enabled = kondisi;
            txt_ekstermitas_bawah.Enabled = kondisi;
            txt_asesment_awal.Enabled = kondisi;
            txt_planning_awal.Enabled = kondisi;
            txt_gen_inpeksi.Enabled = kondisi;
            txt_gen_vulva.Enabled = kondisi;
            txt_gen_portio.Enabled = kondisi;
            txt_gen_pembukaan.Enabled = kondisi;
            txt_gen_penurunan.Enabled = kondisi;
            txt_gen_ketuban.Enabled = kondisi;
            txt_gen_presntasi.Enabled = kondisi;

            txt_fisik_mata.Enabled = kondisi;
            txt_fisik_leher.Enabled = kondisi;
            txt_fisik_payudara.Enabled = kondisi;
            txt_fisik_inpeksi.Enabled = kondisi;
            txt_fisik_leopold_i.Enabled = kondisi;
            txt_fisik_leopold_ii.Enabled = kondisi;
            txt_fisik_leopold_iii.Enabled = kondisi;
            txt_fisik_leopold_iv.Enabled = kondisi;
            txt_fisik_tfu.Enabled = kondisi;
            txt_fisik_djj.Enabled = kondisi;
            txt_fisik_his.Enabled = kondisi;
            txt_penunjang_i.Enabled = kondisi;
            txt_penunjang_iii.Enabled = kondisi;
            txt_protein_urine.Enabled = kondisi;
            txt_glukosa_urine.Enabled = kondisi;
            date_tgl_persalinan.Enabled = kondisi;
            txt_nama_bidan.Enabled = kondisi;
            rb_tempat_persalinan.Enabled = kondisi;
            txt_alamat_persalinan.Enabled = kondisi;
            rb_rujuk_kala.Enabled = kondisi;
            txt_alasan_rujuk.Enabled = kondisi;
            txt_tempat_rujuk.Enabled = kondisi;
            rb_pendamping_rujuk.Enabled = kondisi;
            rb_kala_i_a.Enabled = kondisi;
            txt_kala_i_b.Enabled = kondisi;
            txt_kala_i_c.Enabled = kondisi;
            txt_kala_i_d.Enabled = kondisi;
            rb_kala_ii_a.Enabled = kondisi;
            rb_kala_ii_b.Enabled = kondisi;
            rb_kala_ii_c.Enabled = kondisi;
            rb_kala_ii_d.Enabled = kondisi;
            txt_kala_ii_e.Enabled = kondisi;
            txt_kala_ii_f.Enabled = kondisi;
            txt_kala_ii_g.Enabled = kondisi;
            txt_kala_iii_a.Enabled = kondisi;
            rb_kala_iii_b.Enabled = kondisi;
            rb_kala_iii_c.Enabled = kondisi;
            rb_kala_iii_d.Enabled = kondisi;
            txt_baru_lahir_berat.Enabled = kondisi;
            txt_baru_lahir_panjang.Enabled = kondisi;
            rb_baru_lahir_jk.Enabled = kondisi;
            rb_baru_lahir_nilai.Enabled = kondisi;
            chk_baru_lahir_ket.Enabled = kondisi;

            //txt_kala_ii_a.Enabled = kondisi;
            //txt_kala_ii_c.Enabled = kondisi;
            //txt_kala_ii_d.Enabled = kondisi;
            //txt_kala_iii_b.Enabled = kondisi;
            //txt_kala_iii_c.Enabled = kondisi;
            //txt_kala_iii_d.Enabled = kondisi;

            rb_rangsang_laktil.Enabled = kondisi;
            rb_plasenta_intack.Enabled = kondisi;
            rb_plasenta_tidak_lahir.Enabled = kondisi;
            rb_laserasi.Enabled = kondisi;
            rb_laserasi_parinium.Enabled = kondisi;
            rb_atonia_uteri.Enabled = kondisi;
            txt_jumlah_pendarahan.Enabled = kondisi;
            txt_masalah_lain.Enabled = kondisi;
            txt_penata_masalah.Enabled = kondisi;
            txt_hasilnya.Enabled = kondisi;
            rb_baru_lahir.Enabled = kondisi;
            txt_bayi_cacat.Enabled = kondisi;
            txt_hipotermia.Enabled = kondisi;
            rb_pemberian_asi.Enabled = kondisi;
            txt_masalah_lahir.Enabled = kondisi;
            txt_hasil_lahir.Enabled = kondisi;

            chk_baru_lahir_ket_1.Enabled = kondisi;
            chk_baru_lahir_ket_2.Enabled = kondisi;
            chk_baru_lahir_ket_3.Enabled = kondisi;
            chk_baru_lahir_ket_4.Enabled = kondisi;
            chk_baru_lahir_ket_5.Enabled = kondisi;

            chk_cacat_ket_1.Enabled = kondisi;
            chk_cacat_ket_2.Enabled = kondisi;
            chk_cacat_ket_3.Enabled = kondisi;
            chk_cacat_ket_4.Enabled = kondisi;
            chk_cacat_ket_5.Enabled = kondisi;
            chk_cacat_ket_6.Enabled = kondisi;
            chk_cacat_ket_lain.Enabled = kondisi;
            txt_cacat_ket_lain.Enabled = kondisi;

            //txt_rangsang_laktil.Enabled = kondisi;
            //txt_plasenta_intack.Enabled = kondisi;
            //txt_plasenta_tidak_lahir.Enabled = kondisi;
            //txt_laserasi.Enabled = kondisi;
            //txt_laserasi_parinium.Enabled = kondisi;
            //txt_atonia_uteri.Enabled = kondisi;
            //txt_pemberian_asi_ya.Enabled = kondisi;
            //txt_pemberian_asi_tdk.Enabled = kondisi;

            cb_ada_tindakan.Enabled = kondisi;

            txt_MasalahKala4.Enabled = kondisi;
            txt_PenatalaksanaKala4.Enabled = kondisi;


            txt_skl1_tanggal.Enabled = kondisi;
            txt_skl1_jam.Enabled = kondisi;
            txt_skl1_keluhan_utama.Enabled = kondisi;
            txt_skl1_kesadaran.Enabled = kondisi;
            txt_skl1_td_1.Enabled = kondisi;
            txt_skl1_td_2.Enabled = kondisi;
            txt_skl1_n.Enabled = kondisi;
            txt_skl1_r.Enabled = kondisi;
            txt_skl1_s.Enabled = kondisi;
            txt_skl1_frekuensi.Enabled = kondisi;
            txt_skl1_interfal.Enabled = kondisi;
            txt_skl1_durasi.Enabled = kondisi;
            txt_skl1_djj.Enabled = kondisi;
            txt_skl1_diagnosa.Enabled = kondisi;
            txt_skl1_masalah_potensial.Enabled = kondisi;
            txt_skl1_antisipasi_masalah.Enabled = kondisi;
            txt_skl1_vulva.Enabled = kondisi;
            txt_skl1_pembukaan.Enabled = kondisi;
            txt_skl1_keadaan_ketuban.Enabled = kondisi;
            txt_skl1_presentasi.Enabled = kondisi;
            txt_skl1_bagian_teraba.Enabled = kondisi;
            txt_skl1_turunnya_bagian.Enabled = kondisi;
            txt_skl1_molage.Enabled = kondisi;
            rb_skl1_vesica_urineria.Enabled = kondisi;
            txt_skl1_planning.Enabled = kondisi;
            txt_skl1_jam_plan.Enabled = kondisi;
            txt_skl1_tgl_plan.Enabled = kondisi;
            txt_skl1_dipimpin.Enabled = kondisi;
            txt_skl1_jam_mulai.Enabled = kondisi;

            txt_skl2_tanggal.Enabled = kondisi;
            txt_skl2_jam.Enabled = kondisi;
            txt_skl2_keluhan_utama.Enabled = kondisi;
            txt_skl2_kesadaran.Enabled = kondisi;
            txt_skl2_td_1.Enabled = kondisi;
            txt_skl2_td_2.Enabled = kondisi;
            txt_skl2_n.Enabled = kondisi;
            txt_skl2_r.Enabled = kondisi;
            txt_skl2_s.Enabled = kondisi;
            txt_skl2_frekuensi.Enabled = kondisi;
            txt_skl2_interfal.Enabled = kondisi;
            txt_skl2_durasi.Enabled = kondisi;
            txt_skl2_djj.Enabled = kondisi;
            txt_skl2_diagnosa.Enabled = kondisi;
            txt_skl2_masalah_potensial.Enabled = kondisi;
            txt_skl2_antisipasi_masalah.Enabled = kondisi;
            txt_skl2_vulva.Enabled = kondisi;
            txt_skl2_pembukaan.Enabled = kondisi;
            txt_skl2_keadaan_ketuban.Enabled = kondisi;
            txt_skl2_presentasi.Enabled = kondisi;
            txt_skl2_bagian_teraba.Enabled = kondisi;
            txt_skl2_turunnya_bagian.Enabled = kondisi;
            txt_skl2_molage.Enabled = kondisi;
            rb_skl2_vesica_urineria.Enabled = kondisi;
            txt_skl2_planning.Enabled = kondisi;
            txt_skl2_lahir_jam.Enabled = kondisi;
            txt_skl2_jk.Enabled = kondisi;
            txt_skl2_bb.Enabled = kondisi;
            txt_skl2_lk.Enabled = kondisi;
            txt_skl2_ld.Enabled = kondisi;
            txt_skl2_kadaan_lahir.Enabled = kondisi;
            txt_skl2_evaluasi.Enabled = kondisi;

            txt_skl3_tanggal.Enabled = kondisi;
            txt_skl3_jam.Enabled = kondisi;
            txt_skl3_keluhan_utama.Enabled = kondisi;
            txt_skl3_kesadaran.Enabled = kondisi;
            txt_skl3_td_1.Enabled = kondisi;
            txt_skl3_td_2.Enabled = kondisi;
            txt_skl3_n.Enabled = kondisi;
            txt_skl3_r.Enabled = kondisi;
            txt_skl3_s.Enabled = kondisi;
            txt_skl3_palpasi_abdomen.Enabled = kondisi;
            cb_skl3_kontraksi_uterus_1.Enabled = kondisi;
            cb_skl3_kontraksi_uterus_2.Enabled = kondisi;
            cb_skl3_kontraksi_uterus_3.Enabled = kondisi;
            rb_skl3_uterus_membulat.Enabled = kondisi;
            txt_skl3_tinggi_fundus.Enabled = kondisi;
            rb_skl3_semburan_darah.Enabled = kondisi;
            rb_skl3_vesica_urineria.Enabled = kondisi;
            txt_skl3_diagnosa.Enabled = kondisi;
            txt_skl3_masalah_potensial.Enabled = kondisi;
            txt_skl3_antisipasi_masalah.Enabled = kondisi;
            txt_skl3_planning.Enabled = kondisi;
            txt_skl3_placenta_lahir.Enabled = kondisi;
            rb_skl3_spontan.Enabled = kondisi;
            rb_skl3_lengkap.Enabled = kondisi;
            txt_skl3_kontraksi.Enabled = kondisi;
            txt_skl3_pendarahan.Enabled = kondisi;
            txt_skl3_keadaan_jalan.Enabled = kondisi;
            rb_skl3_bila_reptura.Enabled = kondisi;


            txt_skl4_tanggal.Enabled = kondisi;
            txt_skl4_jam.Enabled = kondisi;
            txt_skl4_keluhan_utama.Enabled = kondisi;
            txt_skl4_kesadaran.Enabled = kondisi;
            txt_skl4_td_1.Enabled = kondisi;
            txt_skl4_td_2.Enabled = kondisi;
            txt_skl4_n.Enabled = kondisi;
            txt_skl4_r.Enabled = kondisi;
            txt_skl4_s.Enabled = kondisi;
            cb_skl4_kontraksi_uterus_1.Enabled = kondisi;
            cb_skl4_kontraksi_uterus_2.Enabled = kondisi;
            cb_skl4_kontraksi_uterus_3.Enabled = kondisi;
            txt_skl4_tinggi_fundus.Enabled = kondisi;
            rb_skl4_vesica_urineria.Enabled = kondisi;
            txt_skl4_jumlah_darah.Enabled = kondisi;
            txt_skl4_diagnosa.Enabled = kondisi;
            txt_skl4_masalah_potensial.Enabled = kondisi;
            txt_skl4_antisipasi_masalah.Enabled = kondisi;
            txt_skl4_planning.Enabled = kondisi;

            rb_skl1_djj.Enabled = kondisi;
            rb_skl2_djj.Enabled = kondisi;







        }

        private void cb_ada_tindakan_CheckedChanged(object sender, EventArgs e)
        {
            xtraTabPage2.PageVisible = cb_ada_tindakan.Checked;

            if (cb_ada_tindakan.Checked)
            {

                try
                {

                    string query = @"select count(*) from T2_TINDAKAN_BIDAN where anamesa_id = '" + txt_anastesi_id.Text + "' ";
                    object result = koneksi.GetScalar(query);

                    if (Convert.ToInt32(result) >= 1)
                    {
                        //get Data
                    }
                    else
                    {
                        string queryInsert = @"insert into T2_TINDAKAN_BIDAN (id, anamesa_id,ada_tindakan) values (tindakan_bidan_seq.nextval, '" + txt_anastesi_id.Text + "','" + (cb_ada_tindakan.Checked ? cb_ada_tindakan.Text : "Tidak") + "')";

                        bool success = koneksi.ExecuteNonQuery(queryInsert);

                    }


                }
                catch (Exception ex)
                {

                }

            }
        }
        static void ConvertColumnNamesToUppercase(DataTable dataTable)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                column.ColumnName = column.ColumnName.ToUpper();
            }
        }

        private bool functionChk(string data_asli, string pencarian)
        {
            string[] aa = data_asli.Split(new string[] { pencarian }, StringSplitOptions.None);
            if (aa.Length >= 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void functionSplitIndex_2(string data_asli, DevExpress.XtraEditors.RadioGroup rbt)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 2)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
            }

        }
        private void functionSplitIndex_3(string data_asli, DevExpress.XtraEditors.RadioGroup rbt, DevExpress.XtraEditors.TextEdit txt)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 3)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                txt.EditValue = (aa[2] == null) ? "" : aa[2];
            }

        }
        private void functionSplitIndex_4_asi(string data_asli, DevExpress.XtraEditors.RadioGroup rbt, DevExpress.XtraEditors.TextEdit txt, DevExpress.XtraEditors.TextEdit txt1)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 4)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                txt.EditValue = (aa[2] == null) ? "" : aa[2];
                txt1.EditValue = (aa[3] == null) ? "" : aa[3];
            }

        }
        private void functionSplitIndex_10(string data_asli, DevExpress.XtraEditors.RadioGroup rbt, DevExpress.XtraEditors.TextEdit txt)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 9)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                txt.EditValue = (aa[9] == null) ? "" : aa[9];
            }

        }
        private void functionSplitIndex_5(string data_asli, DevExpress.XtraEditors.RadioGroup rbt, DevExpress.XtraEditors.RadioGroup rbt1, DevExpress.XtraEditors.TextEdit txt)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 5)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                rbt1.SelectedIndex = Convert.ToInt32(aa[2]);
                txt.EditValue = (aa[4] == null) ? "" : aa[4];
            }

        }

        private void rb_tempat_persalinan_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_tempat_persalinan.Enabled = (rb_tempat_persalinan.SelectedIndex == 5);
        }

        private void rb_kala_ii_a_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_kala_ii_a.Enabled = (rb_kala_ii_a.SelectedIndex == 0);

        }

        private void rb_kala_ii_c_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_kala_ii_c.Enabled = (rb_kala_ii_c.SelectedIndex == 1);
        }

        private void rb_kala_ii_d_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_kala_ii_d.Enabled = (rb_kala_ii_d.SelectedIndex == 1);
        }

        private void rb_kala_iii_b_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_kala_iii_b.Enabled = (rb_kala_iii_b.SelectedIndex == 0);
        }

        private void rb_kala_iii_c_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_kala_iii_c.Enabled = (rb_kala_iii_c.SelectedIndex == 0);
        }

        private void rb_kala_iii_d_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_kala_iii_d.Enabled = (rb_kala_iii_d.SelectedIndex == 1);
        }

        private void rb_rangsang_laktil_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_rangsang_laktil.Enabled = (rb_rangsang_laktil.SelectedIndex == 1);
        }

        private void rb_plasenta_intack_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_plasenta_intack.Enabled = (rb_plasenta_intack.SelectedIndex == 1);
        }

        private void rb_plasenta_tidak_lahir_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_plasenta_tidak_lahir.Enabled = (rb_plasenta_tidak_lahir.SelectedIndex == 0);
        }

        private void rb_laserasi_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_laserasi.Enabled = (rb_laserasi.SelectedIndex == 0);
        }

        private void rb_laserasi_parinium_tindakan_SelectedIndexChanged(object sender, EventArgs e)
        {

            txt_laserasi_parinium.Enabled = (rb_laserasi_parinium_tindakan.SelectedIndex == 2);
        }

        private void rb_atonia_uteri_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_atonia_uteri.Enabled = (rb_atonia_uteri.SelectedIndex == 0);
        }

        private void rb_pemberian_asi_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_pemberian_asi_ya.Enabled = (rb_pemberian_asi.SelectedIndex == 0);
            txt_pemberian_asi_tdk.Enabled = (rb_pemberian_asi.SelectedIndex == 1);
        }

        private void groupControl5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
