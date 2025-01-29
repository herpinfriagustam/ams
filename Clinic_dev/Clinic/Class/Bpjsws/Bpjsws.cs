using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Class.Bpjsws
{
    public class Bpjsws
    {
        public enum HttpMethodMode
        {
            Post, Get
        }

        public enum PostDataType
        {
            Form, Json
        }

        public const string CONS_ID = "2555";
        public const string CONS_SECRET = "3sO2B087D0";
        public const string USER_KEY = "580c0ca60ed68122d4943f7e1d32a609";
        public const string AUTHORIZATION = "c2FudG9zYV9zYmdTYW50b3NhITAwMQ==";

        public const string BASE_URL = "https://apijkn-dev.bpjs-kesehatan.go.id";
        public const string BASE_URL_ANTREAN_FKTP = BASE_URL + "/antreanfktp_dev";
        public const string WS_ANTREAN_FKTP_BPJS_REF_POLI_URL = BASE_URL_ANTREAN_FKTP + "/ref/poli/tanggal/{tanggal}";
        public const string WS_ANTREAN_FKTP_BPJS_REF_DOKTER_URL = BASE_URL_ANTREAN_FKTP + "/ref/dokter/kodepoli/{kodepoli}/tanggal/{tanggal}";
        public const string WS_ANTREAN_FKTP_BPJS_ADD_QUEUE_URL = BASE_URL_ANTREAN_FKTP + "/antrean/add";
        public const string WS_ANTREAN_FKTP_BPJS_CANCEL_QUEUE_URL = BASE_URL_ANTREAN_FKTP + "/antrean/batal";
        public const string WS_ANTREAN_FKTP_BPJS_CALL_QUEUE_URL = BASE_URL_ANTREAN_FKTP + "/antrean/panggil";

        public static long CurrentUnixTime
        {
            get
            {
                DateTime currentTime = DateTime.UtcNow;
                DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return ((long)(currentTime - unixEpoch).TotalSeconds);
            }
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
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(respStr);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Request Exception: { ex.Message }");
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

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            }


            // getting response
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return responseString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request Exception: { ex.Message }");
                return "";
            }
        }

        public static string Decrypt(string key, string data)
        {
            string decData = null;
            byte[][] keys = GetHashKeys(key);

            try
            {
                decData = DecryptStringFromBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException) { }
            catch (ArgumentNullException) { }

            string d = LZStringCSharp.LZString.DecompressFromEncodedURIComponent(decData);

            return d;
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
