using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Class.Bpjsws
{
    public class BpjswsResponse
    {
        public MetaData Metadata { get; set; }
        public object Response { get; set; }
        public string ResponseRaw { get; set; }
        public string ResponseRawDecrypted { get; set; }
        public T GetResponse<T>(string str) {
            return BpjswsResponseConvert.Convert<T>(str);
        }

        public string RequestTimestamp { get; set; }

        public JObject GetResponseJO()
        {
            if (Response != null && Response != null)
            {
                try
                {
                    JObject obj = JObject.Parse(this.GetResponseString());
                    return obj;
                }
                catch (Exception ex) { }
            }

            return null;
        }

        public string GetResponseString()
        {
            try { 
                if(this.Response != null)
                {
                    string key = Bpjsws.CreateDecryptKey(this.RequestTimestamp);
                    string decrypt = Bpjsws.Decrypt(key, this.Response?.ToString());
                    JToken jt = JToken.Parse(decrypt);
                    this.Response = jt;
                }
                    
            }
            catch (Exception ex) { }

            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public class MetaData
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }
    }

    public class BpjswsResponse<T>
    {
        public MetaData Metadata { get; set; }
        public T response { get; set; }

        public string RequestTimestamp { get; set; }

        public T GetResponse(string str)
        {
            return BpjswsResponseConvert.Convert<T>(str);
        }

        public string GetResponseString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public class MetaData
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }
    }

    public class BpjswsResponseConvert
    {
        public static T Convert<T>(string str)
        {
            try
            {
                T resp = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);

                return resp;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetResponse Exception: { ex.Message }");
            }

            return default(T);
        }
    }
}
