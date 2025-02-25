using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Class.Bpjsws
{
    public class Bpjsws
    {
        public enum HttpMethodMode
        {
            Post, Get, Delete, Put
        }

        public enum PostDataType
        {
            Form, Json
        }

        public const string CONS_ID = "2555";
        public const string CONS_SECRET = "3sO2B087D0";
        public const string USER_KEY = "580c0ca60ed68122d4943f7e1d32a609";
        public const string AUTHORIZATION = "Basic c2FudG9zYV9zYmdTYW50b3NhITAwMQ==";
        public const string AUTHORIZATION_PCARE = "Basic c2FudG9zYTohXzN1eiNwKlNyVVE6MDk1";

        public const string BASE_URL = "https://apijkn-dev.bpjs-kesehatan.go.id";

        // Antro
        public const string BASE_URL_ANTREAN_FKTP = BASE_URL + "/antreanfktp_dev";
        public const string WS_ANTREAN_FKTP_BPJS_REF_POLI_URL = BASE_URL_ANTREAN_FKTP + "/ref/poli/tanggal/{tanggal}";
        public const string WS_ANTREAN_FKTP_BPJS_REF_DOKTER_URL = BASE_URL_ANTREAN_FKTP + "/ref/dokter/kodepoli/{kodepoli}/tanggal/{tanggal}";
        public const string WS_ANTREAN_FKTP_BPJS_ADD_QUEUE_URL = BASE_URL_ANTREAN_FKTP + "/antrean/add";
        public const string WS_ANTREAN_FKTP_BPJS_CANCEL_QUEUE_URL = BASE_URL_ANTREAN_FKTP + "/antrean/batal";
        public const string WS_ANTREAN_FKTP_BPJS_CALL_QUEUE_URL = BASE_URL_ANTREAN_FKTP + "/antrean/panggil";

        // pcare
        public const string BASE_URL_PCARE = BASE_URL + "/pcare-rest-dev";
        public const string WS_PCARE_DIAGNOSA_GET_DIAGNOSA_URL = BASE_URL_PCARE + "/diagnosa/{Parameter 1}/{Parameter 2}/{Parameter 3}";
        public const string WS_PCARE_DOKTER_GET_URL = BASE_URL_PCARE + "/dokter/{Parameter 1}/{Parameter 2}";
        public const string WS_PCARE_GROUP_GET_CLUB_PROTANIS_URL = BASE_URL_PCARE + "/kelompok/club/{Parameter 1}";
        public const string WS_PCARE_GROUP_GET_ACTIVITY_URL = BASE_URL_PCARE + "/kelompok/kegiatan/{Parameter 1}";
        public const string WS_PCARE_GROUP_GET_PATIENT_ACTIVITY_URL = BASE_URL_PCARE + "/kelompok/peserta/{Parameter 1}";
        public const string WS_PCARE_GROUP_POST_ACTIVITY_URL = BASE_URL_PCARE + "/kelompok/kegiatan";
        public const string WS_PCARE_GROUP_POST_PATIENT_ACTIVITY_URL = BASE_URL_PCARE + "/kelompok/peserta";
        public const string WS_PCARE_GROUP_DELETE_ACTIVITY_URL = BASE_URL_PCARE + "//kelompok/kegiatan/{Parameter 1}";
        public const string WS_PCARE_GROUP_DELETE_PATIENT_ACTIVITY_URL = BASE_URL_PCARE + "/kelompok/peserta/{Parameter 1}/{Parameter 2}";
        public const string WS_PCARE_KESADARAN_GET_URL = BASE_URL_PCARE + "/kesadaran";

        public const string WS_PCARE_KUNJUNGAN_RUJUKAN_GET_URL = BASE_URL_PCARE + "/kunjungan/rujukan/{Parameter 1}";
        public const string WS_PCARE_KUNJUNGAN_RIWAYAT_GET_URL = BASE_URL_PCARE + "/kunjungan/peserta/{Parameter 1}";
        public const string WS_PCARE_KUNJUNGAN_ADD_URL = BASE_URL_PCARE + "/kunjungan";
        public const string WS_PCARE_KUNJUNGAN_EDIT_URL = BASE_URL_PCARE + "//kunjungan";
        public const string WS_PCARE_KUNJUNGAN_DELETE_URL = BASE_URL_PCARE + "/kunjungan/{Parameter 1}";

        public const string WS_PCARE_MCU_GET_URL = BASE_URL_PCARE + "/MCU/kunjungan/{Parameter 1}";
        public const string WS_PCARE_MCU_ADD_URL = BASE_URL_PCARE + "/MCU";
        public const string WS_PCARE_MCU_EDIT_URL = BASE_URL_PCARE + "//MCU";
        public const string WS_PCARE_MCU_DELETE_URL = BASE_URL_PCARE + "/MCU/{Parameter 1}/kunjungan/{Parameter 2}";

        public const string WS_PCARE_OBAT_DPHO_GET_URL = BASE_URL_PCARE + "/obat/dpho/{Parameter 1}/{Parameter 2}/{Parameter 3}";
        public const string WS_PCARE_OBAT_KUNJUNGAN_GET_URL = BASE_URL_PCARE + "/obat/kunjungan/{Parameter 1}";
        public const string WS_PCARE_OBAT_ADD_URL = BASE_URL_PCARE + "/obat/kunjungan";
        public const string WS_PCARE_OBAT_DELETE_URL = BASE_URL_PCARE + "/obat/{Parameter 1}/kunjungan/{Parameter 2}";

        public const string WS_PCARE_DAFT_BY_NO_GET_URL = BASE_URL_PCARE + "/pendaftaran/noUrut/{Parameter 1}/tglDaftar/{Parameter 2}";
        public const string WS_PCARE_DAFT_PROVIDER_GET_URL = BASE_URL_PCARE + "/pendaftaran/tglDaftar/{Parameter 1}/{Parameter 2}/{Parameter 3}";
        public const string WS_PCARE_DAFT_ADD_URL = BASE_URL_PCARE + "/pendaftaran";
        public const string WS_PCARE_DAFT_DELETE_URL = BASE_URL_PCARE + "/pendaftaran/peserta/{Parameter 1}/tglDaftar/{Parameter 2}/noUrut/{Parameter 3}/kdPoli/{Parameter 4}";

        public const string WS_PCARE_PESERTA_GET_URL = BASE_URL_PCARE + "/peserta/{Parameter 1}";
        public const string WS_PCARE_PESERTA_BY_GET_URL = BASE_URL_PCARE + "/peserta/{Parameter 1}/{Parameter 2}";

        public const string WS_PCARE_POLI_GET_URL = BASE_URL_PCARE + "/poli/fktp/{Parameter 1}/{Parameter 2}";
        public const string WS_PCARE_PROVIDER_RAYONISASI_GET_URL = BASE_URL_PCARE + "/provider/{Parameter 1}/{Parameter 2}";
        public const string WS_PCARE_STATUS_PULANG_GET_URL = BASE_URL_PCARE + "/statuspulang/rawatInap/{Parameter 1}";
        public const string WS_PCARE_ALERGI_GET_URL = BASE_URL_PCARE + "/alergi/jenis/{parameter 1}";
        public const string WS_PCARE_PROGNOSA_GET_URL = BASE_URL_PCARE + "/prognosa";

        public const string WS_PCARE_SPESIALIS_REF_GET_URL = BASE_URL_PCARE + "/spesialis";
        public const string WS_PCARE_SPESIALIS_SUB_REF_GET_URL = BASE_URL_PCARE + "/spesialis/{Parameter 1}/subspesialis";
        public const string WS_PCARE_SPESIALIS_SARANA_REF_GET_URL = BASE_URL_PCARE + "/spesialis/sarana";
        public const string WS_PCARE_SPESIALIS_KHUSUS_REF_GET_URL = BASE_URL_PCARE + "/spesialis/khusus";
        public const string WS_PCARE_SPESIALIS_FRSS_GET_URL = BASE_URL_PCARE + "/spesialis/rujuk/subspesialis/{Parameter 1}/sarana/{Parameter 2}/tglEstRujuk/{Parameter 3}";
        public const string WS_PCARE_SPESIALIS_FRK1_GET_URL = BASE_URL_PCARE + "/spesialis/rujuk/khusus/{Parameter 1}/noKartu/{Parameter 2}/tglEstRujuk/{Parameter 3}";
        public const string WS_PCARE_SPESIALIS_FRK2_GET_URL = BASE_URL_PCARE + "/spesialis/rujuk/khusus/{Parameter 1}/subspesialis/{Parameter 2}/noKartu/{Parameter 3}/tglEstRujuk/{Parameter 4}";

        public const string WS_PCARE_TINDAKAN_BY_KUNJUNGAN_GET_URL = BASE_URL_PCARE + "/tindakan/kunjungan/{Parameter 1}";
        public const string WS_PCARE_TINDAKAN_REF_GET_URL = BASE_URL_PCARE + "/tindakan/kdTkp/{Parameter 1}/{Parameter 2}/{Parameter 3}";
        public const string WS_PCARE_TINDAKAN_ADD_URL = BASE_URL_PCARE + "/tindakan";
        public const string WS_PCARE_TINDAKAN_EDIT_URL = BASE_URL_PCARE + "/tindakan";
        public const string WS_PCARE_TINDAKAN_DELETE_URL = BASE_URL_PCARE + "/tindakan/{Parameter 1}/kunjungan/{Parameter 2}";


        public static long CurrentUnixTime
        {
            get
            {
                DateTime currentTime = DateTime.UtcNow;
                DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return ((long)(currentTime - unixEpoch).TotalSeconds);
            }
        }

        public static string CreateDecryptKey(string unixTime)
        {
            return CONS_ID + CONS_SECRET + unixTime;
        }

        public static string CreateSignature()
        {
            return CreateSignature(CONS_ID, CONS_SECRET, CurrentUnixTime.ToString());
        }

        public static string CreateSignature(long unixTime)
        {
            return CreateSignature(unixTime.ToString());
        }

        public static string CreateSignature(string consId, string consSecreat, long unixTime)
        {
            return CreateSignature(consId, consSecreat, unixTime.ToString());
        }

        public static string CreateSignature(string consId, string consSecreat, string unixTime)
        {
            try
            {
                string data = $"{ consId }&{unixTime}";
                string secretKey = consSecreat;

                HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));

                byte[] signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));

                string encodedSignature = Convert.ToBase64String(signature);
                return encodedSignature;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"CreateSignature Exception: { ex.Message }");
                return null;
            }
        }

        public static string CreateSignature(string unixTime)
        {
            try
            {
                string data = $"{ CONS_ID }&{unixTime}";
                string secretKey = CONS_SECRET;

                HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));

                byte[] signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));

                string encodedSignature = Convert.ToBase64String(signature);
                return encodedSignature;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateSignature Exception: { ex.Message }");
                return null;
            }
        }

        public static T Request<T>(string url, HttpMethodMode method, PostDataType dataType = PostDataType.Json, Dictionary<string, string> headers = null, Dictionary<string, string> dataOrQParams = null)
        {
            string respStr = Request(url, method, dataType, headers, dataOrQParams);
            try
            {
                T r = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(respStr);
                if(typeof(T) == typeof(BpjswsResponse))
                {
                    string unixTime = headers.ContainsKey("x-timestamp") ? headers["x-timestamp"]?.ToString() : "";
                    foreach (PropertyInfo prop in r.GetType().GetProperties())
                    {
                        if (prop.Name == "ResponseRaw") prop.SetValue(r, respStr);
                    }
                }

                return r;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Request Exception: { ex.Message }");
                if (typeof(T) == typeof(BpjswsResponse))
                {
                    Type type = typeof(T);
                    T obj = (T)Activator.CreateInstance(type);
                    
                    foreach(PropertyInfo prop in type.GetProperties())
                    {
                        if (prop.Name == "Response") prop.SetValue(obj, respStr);
                        else if (prop.Name == "Metadata")
                        {
                            try
                            {
                                BpjswsResponse.MetaData meta = new BpjswsResponse.MetaData();
                                meta.Code = -1;
                                meta.Message = respStr == "" || respStr == null ? ex.Message : respStr;

                                prop.SetValue(obj, meta);
                            }
                            catch(Exception exx)
                            {
                                Console.WriteLine($"Request Exception: { exx.Message }");
                            }
                        }
                    }

                    return obj;
                }
            }

            return default(T);
        }

        public static string Request(string url, HttpMethodMode method, PostDataType dataType = PostDataType.Json, Dictionary<string, string> headers = null, Dictionary<string, string> dataOrQParams = null) 
        {
            HttpWebRequest request = null;

            string content = "";
            if (dataOrQParams != null)
            {
                if(dataOrQParams.ContainsKey("RAW")) content = dataOrQParams["RAW"];
                else content = string.Join("&", dataOrQParams.Select(x => string.Join("=", x.Key, Uri.EscapeDataString(x.Value))));
            }


            if (method == HttpMethodMode.Post)
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentLength = content.Length;

                if (headers != null && headers.Count > 0)
                    foreach(KeyValuePair<string, string> kv in headers)
                        request.Headers.Add(kv.Key, kv.Value);

                byte[] dataBytes = Encoding.UTF8.GetBytes(content);
                if (dataType == PostDataType.Form)
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                    
                }
                else if (dataType == PostDataType.Json)
                {
                    request.ContentType = "application/json";
                    request.GetRequestStream().Write(dataBytes, 0, dataBytes.Length);
                }
            }
            else
            {
                string urlWithQueryParam = content.Length > 0 ? url + "?" + content : url;
                request = (HttpWebRequest)WebRequest.Create(urlWithQueryParam);
                request.Method = "GET";

                if (headers != null && headers.Count > 0)
                    foreach (KeyValuePair<string, string> kv in headers)
                        request.Headers.Add(kv.Key, kv.Value);
            }


            // getting response
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return responseString;
            }
            catch(WebException wex)
            {
                if(wex.Response != null)
                {
                    string jr = "";
                    using (HttpWebResponse r = wex.Response as HttpWebResponse)
                    {
                        using (StreamReader reader = new StreamReader(wex.Response.GetResponseStream()))
                        {
                            if(reader != null) jr = reader.ReadToEnd();
                            else jr = "{ \"response\": null, \"metadata\": { \"code\": " + (int)r.StatusCode + ", \"message\": \"" + wex.Message + "\"}}";
                        }
                        
                    }

                    return jr;
                }

                Console.WriteLine($"Request Exception: { wex.Message }");
                return "{ \"response\": null, \"metadata\": { \"code\": -1, \"message\": \"Unknown Error!\"}}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request Exception: { ex.Message }");
                return "{ \"response\": null, \"metadata\": { \"code\": -1, \"message\": \"Exception: " + ex.Message + "\"}}";
            }
        }

        public static string Decrypt(string key, string data)
        {
            string decData = null;
            byte[][] keys = GetHashKeys(key);

            try
            {
                decData = DecryptStringFromBytes_Aes(data, keys[0], keys[1]);

                string d = LZStringCSharp.LZString.DecompressFromEncodedURIComponent(decData);

                return d;
            }
            catch (CryptographicException) { }
            catch (ArgumentNullException) { }
            catch(Exception ex) { }

            return data;
        }

        private static string DecryptStringFromBytes_Aes(string cipherTextString, byte[] Key, byte[] IV)
        {
            byte[] cipherText = Convert.FromBase64String(cipherTextString);

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt =
                            new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        private static byte[][] GetHashKeys(string key)
        {
            byte[][] result = new byte[2][];
            Encoding enc = Encoding.UTF8;

            SHA256 sha2 = new SHA256CryptoServiceProvider();

            byte[] rawKey = enc.GetBytes(key);
            byte[] rawIV = enc.GetBytes(key);

            byte[] hashKey = sha2.ComputeHash(rawKey);
            byte[] hashIV = sha2.ComputeHash(rawIV);

            Array.Resize(ref hashIV, 16);

            result[0] = hashKey;
            result[1] = hashIV;

            return result;
        }
    }
}
