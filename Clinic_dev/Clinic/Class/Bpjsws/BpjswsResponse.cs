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
        public string Response { get; set; }
        public T GetResponse<T>(string str) {
            return BpjswsResponseConvert.Convert<T>(str);
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

        public T GetResponse(string str)
        {
            return BpjswsResponseConvert.Convert<T>(str);
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
                Console.WriteLine($"GetResponse Exceptin: { ex.Message }");
            }

            return default(T);
        }
    }
}
