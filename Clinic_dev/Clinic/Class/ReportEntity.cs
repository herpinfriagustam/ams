using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic
{
    public class ReportEntity
    {

        // cover 

        public string NamaPerusahaan { get; set; }
        public string AlamatPerusahaan { get; set; }
        public string NamaPasien { get; set; }
        public string NikPasien { get; set; }
        public string NamaKKPasien { get; set; }
        public string TglLahirPasien { get; set; }
        public string UsiaPasien { get; set; }
        public string NomorTelefonPasien { get; set; }
        public string AlamatPasien { get; set; }
        public string PekerjaanPasien { get; set; }
        public string RiwayatAlergiPasien { get; set; }
        public string RiwayatAlergiObatPasien { get; set; }
        public string RiwayatAlergiMakananPasien { get; set; }
        public string NamaKepalaKeluargaPasien { get; set; }
        public string JenisKelaminPasien { get; set; }
        public string JenisJaminanKesehatan { get; set; }
        public string JenisJaminanKesehatanLainnya { get; set; }
        public string NomorJaminan { get; set; }
        public string TahunRekamMedis { get; set; }
        public string NomorRM { get; set; }


        // Surat pernyataan persetujuan pemeriksaan/pengobatan/perawatan
        public string NamaPenjamin { get; set; }
        public string UsiaPenjamin { get; set; }
        public string TglLahirPenjamin { get; set; }
        public string PekerjaanPenjamin { get; set; }
        public string HubunganKeluargaPenjamin { get; set; }
        public string AlamatPenjamin { get; set; }
        public string RuanganPengobatan { get; set; }
        public string TglPernyataanPersetujuan { get; set; }
        public string NamaPetugasPernyataanPersetujuan { get; set; }
        public string NamaPembuatPernyataanPersetujuan { get; set; }

        // hak dan kewajiban pasien
        public string TglHakDanKewajiban { get; set; }

        // Kartu Rawat Jalan

        public string NomorMedrec { get; set; }
        public string KeteranganPasien { get; set; }
        public DataTable KartuRawatJalan { get; set; }

        // Rekam Medis Gigi

        public string NomorRMGigi { get; set; }
        public DataTable RekamMedisGigi { get; set; }
        public string JenisPasien { get; set; }
        public string NomorRMRawatJalan { get; set; }

        // Surat Pernyyataan Persetujuan Pasien Rawat Inap

        public string TglPersetujuanRawatInap {get;set;}
        public string NamaPetugasRawatInap {get;set;}
        public string NamaPembuatRawatInap {get;set; }

        // Pernyataan pemberian informasi Asuhan

        public string JenisAsuhan { get; set; }
        public string NamaAsuhan { get; set; }
        public string JenisKelaminAsuhan { get; set; }
        public string UmurAsuhan { get; set; }
        public string TglLahirAsuhan { get; set; }
        public string AlamatAsuhan { get; set; }
        public string NomorTelefonASuhan { get; set; }
        public string TglPernyataanAsuhan { get; set; }
        public string NamaPetugasAsuhan { get; set; }
        public string NamaPembuatAsuhan { get; set; }

        // Pengkajian Awal Medis & Keperawatan rawat inap (PAM)

        public string PAMNama { get; set; }
        public string PAMNomorRM { get; set; }
        public string PAMTglLahir { get; set; }
        public string PAMJenisKelamin { get; set; }
        public string PAMTgl { get; set; }
        public string PAMAgama { get; set; }
        public string PAMJam { get; set; }
        public string PAMGoldar { get; set; }
        public string PAMPendidikan { get; set; }
        public string PAMSumberData { get; set; }
        public string PAMRujukan { get; set; }
        public string PAMDiagnosaRujukan { get; set; }
        public string PAMLainnya { get; set; }
        public string PAMNakes { get; set; }
        public string PAM1 { get; set; }
        public string PAM2a { get; set; }
        public string PAM2aNamaPenyakit { get; set; }
        public string PAM2aPernahDirawat { get; set; }
        public string PAM2aPernahDirawatDiagnosa { get; set; }
        public string PAM2aPernahDirawatKapan { get; set; }
        public string PAM2aPernahDirawatDi { get; set; }
        public string PAM2aPernahOperasi { get; set; }
        public string PAM2aPernahOperasiJenis { get; set; }
        public string PAM2aPernahOperasiKapan { get; set; }
        public string PAM2b { get; set; }
        public string PAM2bLainnya { get; set; }
        public string PAM2c { get; set; }
        public string PAM2cLainnya { get; set; }
        public string PAM2d { get; set; }
        public string PAM2Sebutkan { get; set; }
        public string PAM2eAlergi { get; set; }
        public string PAM3MasihPengobatan { get; set; }
        public string PAM3Obat { get; set; }
        public string PAM4Td { get; set; }
        public string PAM4Nadi { get; set; }
        public string PAM4P { get; set; }
        public string PAM4Suhu { get; set; }
        public string PAM4aKeluhan { get; set; }
        public string PAM4aKeluhanSebutkan { get; set; }
        public string PAM4aPembatanMakan { get; set; }
        public string PAM4aGigiPasalsu { get; set; }
        public string PAM4aMual { get; set; }
        public string PAM4aMuntah { get; set; }
        public string PAM4aBB { get; set; }
        public string PAM4aTB { get; set; }
        public string PAM4aIMT { get; set; }
        public string PAM4aKeterangan { get; set; }
        public string PAM4bPendengaran { get; set; }
        public string PAM4bPendengaranSebutkan { get; set; }
        public string PAM4bPenglihatan { get; set; }
        public string PAM4bPenglihatanSebutkan { get; set; }
        public string PAM4cDefekasi { get; set; }
        public string PAM4cDefekasiSebutkan { get; set; }
        public string PAM4cMiksi { get; set; }
        public string PAM4cMiksiSebutkan { get; set; }
        public string PAM4dKeadaanKulit{ get; set; }
        public string PAM4dKeadaanKulitSebutkan { get; set; }
        public string PAM4dSkorNorton{ get; set; }
        public string PAM4dResikoBekubitus{ get; set; }
        public string PAM4ePemeriksaanFisik { get; set; }
        public string PAM4eKelompokKhusus { get; set; }
        public string PAM5a { get; set; }
        public string PAM5aSebutkan { get; set; }
        public string PAM5b { get; set; }
        public string PAM5bSebutkan { get; set; }
        public string PAM5cHubunganKeluarga { get; set; }
        public string PAM5cTempatTinggal { get; set; }
        public string PAM5cKerabatNama { get; set; }
        public string PAM5cKerabatHubungan { get; set; }
        public string PAM5cKerabatTelefon { get; set; }
        public string PAM5dKegiatanKeagamaan { get; set; }
        public string PAM5dKebutuhanSpiritual { get; set; }
        public string PAM6a { get; set; }
        public string PAM6aPenerjemah { get; set; }
        public string PAM6aPenerjemahSebutkan { get; set; }
        public string PAM6aKebutuhanEdukasi { get; set; }
        public string PAM6aKebutuhanEdukasiSebutkan { get; set; }
        public string PAM6b { get; set; }
        public string PAM7 { get; set; }
        public string PAM8Aktivitas { get; set; }
        public string PAM8AktivitasSebutkan { get; set; }
        public string PAM8AlatBantuJalan { get; set; }
        public string PAM9Nyeri { get; set; }
        public string PAMKesediaanMenerimaInformasi { get; set; }



    }
}
