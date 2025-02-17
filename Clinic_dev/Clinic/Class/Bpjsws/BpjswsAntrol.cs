using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Class.Bpjsws
{
    public class BpjswsAntrol
    {
        /// <summary>
        /// mendapatkan referensi poli yang terdaftar di HFIS
        /// </summary>
        /// <param name="tgl">tanggal (mungkin tanggal sekarang/pemeriksaan)</param>
        /// <returns>BpjswsResponse Object : untuk mendapatkan list dokter gunakan fungsi GetResponse<ModelPoli>()</returns>
        public static BpjswsResponse GetReferensiPoli(string tgl)
        {
            string url = Bpjsws.WS_ANTREAN_FKTP_BPJS_REF_POLI_URL.Replace(@"{tanggal}", DateTime.Now.ToString("yyyy-MM-dd"));
            string unixTime = Bpjsws.CurrentUnixTime.ToString();

            // headers
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "x-cons-id",  Bpjsws.CONS_ID },
                { "x-timestamp",  unixTime },
                { "x-signature",  Bpjsws.CreateSignature(unixTime) },
                { "user_key",  Bpjsws.USER_KEY },
            };

            // do request
            BpjswsResponse response = Bpjsws.Request<Clinic.Class.Bpjsws.BpjswsResponse>(url, 
                Bpjsws.HttpMethodMode.Get,
                Bpjsws.PostDataType.Json, 
                headers);

            // handle response
            if (response != null)
            {
                if (response.Metadata.Code == 200) // 200 meaning success
                {
                    // decrypt message
                    string key = Bpjsws.CreateDecryptKey(unixTime);
                    string decData = Bpjsws.Decrypt(key, response.Response);
                    List<ModelPoli> listPoli = BpjswsResponseConvert.Convert<List<ModelPoli>>(decData);

                    response.Response = JsonConvert.SerializeObject(listPoli);
                }
            }

            return response;
        }

        /// <summary>
        /// Mendapatkan referensi dokter yang terdapat pada HFIS
        /// </summary>
        /// <param name="poli">Kode poli</param>
        /// <param name="checkDate">Tanggal Pemeriksaan</param>
        /// <returns>BpjswsResponse Object : untuk mendapatkan list dokter gunakan fungsi GetResponse<ModelDokter>()</returns>
        public static BpjswsResponse GetReferensiDokter(string poli, string checkDate)
        {
            string url = Bpjsws.WS_ANTREAN_FKTP_BPJS_REF_DOKTER_URL
                    .Replace(@"{kodepoli}", poli)
                    .Replace(@"{tanggal}", checkDate);
            string unixTime = Bpjsws.CurrentUnixTime.ToString();

            // headers
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "x-cons-id",  Bpjsws.CONS_ID },
                { "x-timestamp",  unixTime },
                { "x-signature",  Bpjsws.CreateSignature(unixTime) },
                { "user_key",  Bpjsws.USER_KEY },
            };

            // do request
            BpjswsResponse response = Bpjsws.Request<BpjswsResponse>(url, 
                Bpjsws.HttpMethodMode.Get, 
                Bpjsws.PostDataType.Json, 
                headers);
            
            // handle request
            if (response != null)
            {
                if (response.Metadata.Code == 200)
                {
                    string key = Bpjsws.CreateDecryptKey(unixTime);
                    string decData = Clinic.Class.Bpjsws.Bpjsws.Decrypt(key, response.Response);
                    List<ModelDokter> listDokter = BpjswsResponseConvert.Convert<List<ModelDokter>>(decData);

                    response.Response = JsonConvert.SerializeObject(listDokter);
                }
            }

            return response;
        }

        public static BpjswsResponse Post(string url, JObject json)
        {
            string unixTime = Bpjsws.CurrentUnixTime.ToString();

            // headers
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "x-cons-id",  Bpjsws.CONS_ID },
                { "x-timestamp",  unixTime },
                { "x-signature",  Bpjsws.CreateSignature(unixTime) },
                { "user_key",  Bpjsws.USER_KEY },
            };

            // handle response
            BpjswsResponse response = Bpjsws.Request<BpjswsResponse>(url,
                Bpjsws.HttpMethodMode.Post,
                Bpjsws.PostDataType.Json,
                headers,
                new Dictionary<string, string>
                {
                    { "RAW", json.ToString() }
                });

            return response;
        }

        /// <summary>
        /// Menambah antrean ke BPJS
        /// </summary>
        /// <param name="json">format
        /// {
        ///      "nomorkartu": "00012345678",
        ///      "nik": "3212345678987654",
        ///      "nohp": "085635228888",
        ///      "kodepoli": "ANA",
        ///      "namapoli": "Anak",
        ///      "norm": "123345",
        ///      "tanggalperiksa": "2021-01-28",
        ///      "kodedokter": 12345,
        ///      "namadokter": "Dr. Hendra",
        ///      "jampraktek": "08:00-16:00",
        ///      "nomorantrean": "A-12",
        ///      "angkaantrean": 12,
        ///      "keterangan": ""
        /// }
        /// </param>
        /// <returns>BpjswsResponse Object : Metadata.Code 200 Berhasil, 201 Gagal</returns>
        public static BpjswsResponse TambahAntrean(JObject json)
        {
            string url = Bpjsws.WS_ANTREAN_FKTP_BPJS_ADD_QUEUE_URL;
            return Post(url, json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json">format
        /// {
        ///    "tanggalperiksa": "2024-01-03",
        ///    "kodepoli": "001",
        ///    "nomorkartu": "0000045258563",
        ///    "alasan": "Terjadi perubahan jadwal dokter"
        /// }
        /// </param>
        /// <returns>BpjswsResponse Object : Metadata.Code 200 Berhasil, 201 Gagal</returns>
        public static BpjswsResponse BatalAntrian(JObject json)
        {
            string url = Bpjsws.WS_ANTREAN_FKTP_BPJS_CANCEL_QUEUE_URL;
            return Post(url, json);
        }

        /// <summary>
        /// Update Status / Panggil Antrean
        /// </summary>
        /// <param name="json">format
        /// {
        ///   "tanggalperiksa": "2024-03-01",
        ///   "kodepoli": "001",
        ///   "nomorkartu": "0000034563234",
        ///   "status": 1, ---> Status 1 = Hadir; Status 2 = Tidak Hadir
        ///   "waktu": 1616559330000 ---> Waktu dalam bentuk timestamp milisecond
        ///    }
        /// </param>
        /// <returns>BpjswsResponse Object : Metadata.Code 200 Berhasil, 201 Gagal</returns>
        public static BpjswsResponse PanggilAntrean(JObject json)
        {
            string url = Bpjsws.WS_ANTREAN_FKTP_BPJS_CALL_QUEUE_URL;
            return Post(url, json);
        }
    }
}
