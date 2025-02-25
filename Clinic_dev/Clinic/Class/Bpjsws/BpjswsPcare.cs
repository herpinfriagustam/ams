using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Class.Bpjsws
{
    public class BpjswsPcare : BpjswsRequestBase
    {
        #region Diagnosa

        public static BpjswsResponse GetDiagnosa(string codeOrName, int offset = 1, int limit = 10)
        {
            string url = WS_PCARE_DIAGNOSA_GET_DIAGNOSA_URL
                .Replace(@"{Parameter 1}", codeOrName)
                .Replace(@"{Parameter 2}", offset+"")
                .Replace(@"{Parameter 3}", limit+"");

            return Get(url);
        }

        #endregion Diagnosa

        #region Dokter

        public static BpjswsResponse GetDokter(int offset = 1, int limit = 10)
        {
            string url = WS_PCARE_DOKTER_GET_URL
                .Replace(@"{Parameter 1}", offset + "")
                .Replace(@"{Parameter 2}", limit + "");

            return Get(url);
        }

        #endregion Dokter

        #region kelompok

        public static BpjswsResponse GetClubProtanis(string groupType)
        {
            string url = WS_PCARE_GROUP_GET_CLUB_PROTANIS_URL
                .Replace(@"{Parameter 1}", groupType + "");

            return Get(url);
        }

        public static BpjswsResponse GetKegiatanKelompok(string ddmmyyyy)
        {
            string url = WS_PCARE_GROUP_GET_ACTIVITY_URL
                .Replace(@"{Parameter 1}", ddmmyyyy + "");

            return Get(url);
        }

        public static BpjswsResponse GetPesertaKegiatanKelompok(string eduId)
        {
            string url = WS_PCARE_GROUP_GET_PATIENT_ACTIVITY_URL
                .Replace(@"{Parameter 1}", eduId + "");

            return Get(url);
        }

        public static BpjswsResponse AddKegiatanKelompok(JObject json)
        {
            string url = WS_PCARE_GROUP_POST_ACTIVITY_URL;
            return Post(url, json);
        }

        public static BpjswsResponse AddPesertaKegiatanKelompok(JObject json)
        {
            string url = WS_PCARE_GROUP_POST_PATIENT_ACTIVITY_URL;
            return Post(url, json);
        }

        public static BpjswsResponse DeleteKegiatanKelompok(string eduid)
        {
            string url = WS_PCARE_GROUP_DELETE_ACTIVITY_URL
                .Replace(@"{Parameter 1}", eduid);

            return Delete(url);
        }

        public static BpjswsResponse DeletePesertaKegiatanKelompok(string eduid, string bpjsno)
        {
            string url = WS_PCARE_GROUP_DELETE_PATIENT_ACTIVITY_URL
                .Replace(@"{Parameter 1}", eduid)
                .Replace(@"{Parameter 2}", bpjsno);

            return Delete(url);
        }

        #endregion kelompok

        #region Kesadaran

        public static BpjswsResponse GetKesadaran()
        {
            string url = WS_PCARE_GROUP_GET_PATIENT_ACTIVITY_URL;

            return Get(url);
        }

        #endregion Kesadaran

        #region Kunjungan

        public static BpjswsResponse GetRujukan(string noKunjungan)
        {
            string url = WS_PCARE_KUNJUNGAN_RUJUKAN_GET_URL.Replace("{Parameter 1}", noKunjungan);

            return Get(url);
        }

        public static BpjswsResponse GetRiwayatKunjungan(string noBpjs)
        {
            string url = WS_PCARE_KUNJUNGAN_RIWAYAT_GET_URL.Replace("{Parameter 1}", noBpjs);

            return Get(url);
        }

        public static BpjswsResponse AddKunjungan(JObject json)
        {
            string url = WS_PCARE_KUNJUNGAN_ADD_URL;

            return Post(url, json);
        }

        public static BpjswsResponse EditKunjungan(JObject json)
        {
            string url = WS_PCARE_KUNJUNGAN_EDIT_URL;

            return Put(url, json);
        }

        public static BpjswsResponse DeleteKunjungan(string noKunjungan)
        {
            string url = WS_PCARE_KUNJUNGAN_DELETE_URL.Replace("{Parameter 1}", noKunjungan);

            return Delete(url);
        }

        #endregion Kunjungan

        #region MCU

        public static BpjswsResponse GetMCU(string noKunjungan)
        {
            string url = WS_PCARE_MCU_GET_URL.Replace("{Parameter 1}", noKunjungan);

            return Get(url);
        }

        public static BpjswsResponse AddMCU(JObject json)
        {
            string url = WS_PCARE_MCU_ADD_URL;

            return Post(url, json);
        }

        public static BpjswsResponse EditMCU(JObject json)
        {
            string url = WS_PCARE_MCU_EDIT_URL;

            return Put(url, json);
        }

        public static BpjswsResponse DeleteMCU(string kodeMCU, string noKunjungan)
        {
            string url = WS_PCARE_MCU_DELETE_URL
                .Replace("{Parameter 1}", kodeMCU)
                .Replace("{Parameter 2}", noKunjungan);

            return Delete(url);
        }

        #endregion MCU

        #region Obat

        public static BpjswsResponse GetDPHO(string kodeOrNameDPHO, int offset, int limit)
        {
            string url = WS_PCARE_OBAT_DPHO_GET_URL
                .Replace("{Parameter 1}", kodeOrNameDPHO)
                .Replace("{Parameter 2}", offset.ToString())
                .Replace("{Parameter 3}", limit.ToString());

            return Get(url);
        }

        public static BpjswsResponse GetObatByKunjungan(string noKunjungan)
        {
            string url = WS_PCARE_OBAT_KUNJUNGAN_GET_URL.Replace("{Parameter 1}", noKunjungan);

            return Get(url);
        }

        public static BpjswsResponse AddObat(JObject json)
        {
            string url = WS_PCARE_OBAT_ADD_URL;

            return Post(url, json);
        }

        public static BpjswsResponse DeleteObat(string kodeObatSK, string noKunjungan)
        {
            string url = WS_PCARE_OBAT_DELETE_URL
                .Replace("{Parameter 1}", kodeObatSK)
                .Replace("{Parameter 2}", noKunjungan);

            return Delete(url);
        }

        #endregion Obat

        #region Pendaftaran

        public static BpjswsResponse GetPendaftaranByNoUrut(string noUrutDaftar, string tglDaftar)
        {
            string url = WS_PCARE_DAFT_BY_NO_GET_URL
                .Replace(@"{Parameter 1}", noUrutDaftar)
                .Replace(@"{Parameter 2}", tglDaftar);

            return Get(url);
        }

        public static BpjswsResponse GetPendaftaranProvider(string tglDaftar, int offset, int limit)
        {
            string url = WS_PCARE_DAFT_PROVIDER_GET_URL
                .Replace("{Parameter 1}", tglDaftar)
                .Replace("{Parameter 2}", offset.ToString())
                .Replace("{Parameter 3}", limit.ToString());

            return Get(url);
        }

        public static BpjswsResponse AddPendaftaran(JObject json)
        {
            string url = WS_PCARE_DAFT_ADD_URL;

            return Post(url, json);
        }

        public static BpjswsResponse DeletePendaftaran(string noBpjs, string tglDaftar, string noDaftar, string kodePoli)
        {
            string url = WS_PCARE_DAFT_DELETE_URL
                .Replace("{Parameter 1}", noBpjs)
                .Replace("{Parameter 2}", tglDaftar)
                .Replace("{Parameter 3}", noDaftar)
                .Replace("{Parameter 4}", kodePoli);

            return Delete(url);
        }

        #endregion Pendaftaran

        #region Peserta

        public static BpjswsResponse GetPeserta(string noBpjs)
        {
            string url = WS_PCARE_PESERTA_GET_URL
                .Replace(@"{Parameter 1}", noBpjs);

            return Get(url);
        }

        /// <summary>
        /// Mendapatkan daftar peserta berdasarkan jenis kartu
        /// </summary>
        /// <param name="jenisKartu">nik / noka (Nomor Kartu)</param>
        /// <param name="nomorKartu">Nomor NIK atau Nomor Kartu BPJS</param>
        /// <returns></returns>
        public static BpjswsResponse GetPesertaByJenisKartu(string jenisKartu, string nomorKartu)
        {
            string url = WS_PCARE_PESERTA_BY_GET_URL
                .Replace(@"{Parameter 1}", jenisKartu)
                .Replace(@"{Parameter 2}", nomorKartu);

            return Get(url);
        }

        #endregion Peserta

        #region Poli

        public static BpjswsResponse GetPoliFKTP(int offset, int limit)
        {
            string url = WS_PCARE_POLI_GET_URL
                .Replace("{Parameter 1}", offset.ToString())
                .Replace("{Parameter 2}", limit.ToString());

            return Get(url);
        }

        #endregion Poli

        #region Provider

        public static BpjswsResponse GetProviderRayonisasi(int offset, int limit)
        {
            string url = WS_PCARE_PROVIDER_RAYONISASI_GET_URL
                .Replace("{Parameter 1}", offset.ToString())
                .Replace("{Parameter 2}", limit.ToString());

            return Get(url);
        }

        #endregion Provider

        #region Spesialis

        public static BpjswsResponse GetReferensiSpesialis()
        {
            string url = WS_PCARE_SPESIALIS_REF_GET_URL;

            return Get(url);
        }

        public static BpjswsResponse GetReferensiSubSpesialis(string kdSpesialis)
        {
            string url = WS_PCARE_SPESIALIS_SUB_REF_GET_URL.Replace("{Paramter 1}", kdSpesialis);

            return Get(url);
        }


        public static BpjswsResponse GetReferensiSaran()
        {
            string url = WS_PCARE_SPESIALIS_SARANA_REF_GET_URL;

            return Get(url);
        }

        public static BpjswsResponse GetReferensiKhusus()
        {
            string url = WS_PCARE_SPESIALIS_KHUSUS_REF_GET_URL;

            return Get(url);
        }

        public static BpjswsResponse GetFRSS(string kdSubSpesialis, string kdSarana, string tglEstRujuk)
        {
            string url = WS_PCARE_SPESIALIS_FRSS_GET_URL
                .Replace("{Parameter 1}", kdSubSpesialis)
                .Replace("{Parameter 2}", kdSarana)
                .Replace("{Parameter 3}", tglEstRujuk);

            return Get(url);
        }

        public static BpjswsResponse GetFRK1(string kdKhusus, string noBpjs, string tglEstRujuk)
        {
            string url = WS_PCARE_SPESIALIS_FRK1_GET_URL
                .Replace("{Parameter 1}", kdKhusus)
                .Replace("{Parameter 2}", noBpjs)
                .Replace("{Parameter 3}", tglEstRujuk);

            return Get(url);
        }

        public static BpjswsResponse GetFRK2(string kdKhusus, string kdSubSpesialis, string noBpjs, string tglEstRujuk)
        {
            string url = WS_PCARE_SPESIALIS_FRK2_GET_URL
                .Replace("{Parameter 1}", kdKhusus)
                .Replace("{Parameter 2}", kdSubSpesialis)
                .Replace("{Parameter 3}", noBpjs)
                .Replace("{Parameter 4}", tglEstRujuk);

            return Get(url);
        }

        #endregion Spesialis


        #region Status Pulang

        public static BpjswsResponse GetStatusPulang(bool isRawatInap)
        {
            string url = WS_PCARE_STATUS_PULANG_GET_URL
                .Replace("{Parameter 1}", isRawatInap.ToString());

            return Get(url);
        }

        #endregion Status Pulang

        #region Tindakan

        public static BpjswsResponse GetTindakanByKunjungan(string noKunjungan)
        {
            string url = WS_PCARE_TINDAKAN_BY_KUNJUNGAN_GET_URL
                .Replace("{Parameter 1}", noKunjungan.ToString());

            return Get(url);
        }

        public static BpjswsResponse GetReferensiTindakan(string kdTkp, int offset, int limit)
        {
            string url = WS_PCARE_SPESIALIS_FRK1_GET_URL
                .Replace("{Parameter 1}", kdTkp)
                .Replace("{Parameter 2}", offset.ToString())
                .Replace("{Parameter 3}", limit.ToString());

            return Get(url);
        }

        public static BpjswsResponse AddTindakan(JObject json)
        {
            string url = WS_PCARE_TINDAKAN_ADD_URL;

            return Post(url, json);
        }

        public static BpjswsResponse EditTindakan(JObject json)
        {
            string url = WS_PCARE_TINDAKAN_EDIT_URL;

            return Put(url, json);
        }

        public static BpjswsResponse DeleteTindakan(string kdTindakanSK, string noKunjungan)
        {
            string url = WS_PCARE_TINDAKAN_DELETE_URL
                .Replace("{Parameter 1}", kdTindakanSK)
                .Replace("{Parameter 2}", noKunjungan);

            return Delete(url);
        }

        #endregion Tindakan


        #region Alergi

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jenisAlergi">String 01=Makanan, 02=Udara, 03=Obat</param>
        /// <returns></returns>
        public static BpjswsResponse GetAlergi(string jenisAlergi)
        {
            string url = WS_PCARE_ALERGI_GET_URL
                .Replace("{Parameter 1}", jenisAlergi.ToString());

            return Get(url);
        }

        #endregion Alergi

        #region Prognosa

        public static BpjswsResponse GetPrognosa()
        {
            string url = WS_PCARE_PROGNOSA_GET_URL;

            return Get(url);
        }

        #endregion Prognosa
    }
}
