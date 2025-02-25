using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Class.Bpjsws
{
    public class BpjswsResponseList
    {
        public MetaData Metadata { get; set; }
        public Resp Response { get; set; }
        public T GetResponse<T>(string str)
        {
            return BpjswsResponseConvert.Convert<T>(str);
        }

        public JObject GetResponseJO()
        {
            if(Response != null && Response.Response != null)
            {
                try
                {
                    JObject obj = JObject.Parse(Response.Response);
                    return obj;
                }
                catch(Exception ex) { }
            }

            return null;
        }

        public string GetResponseString()
        {
            try
            {
                if (this.Response != null && this.Response.Response != null)
                    this.Response.Response = JObject.Parse(this.Response.Response).ToString(Formatting.Indented);
            }
            catch (Exception ex) { }

            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public class MetaData
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }

        public class Resp
        {
            public int Count { get; set; } = 0;
            public string Response { get; set; }
        }
    }

    public class BpjswsResponseList<T>
    {
        public MetaData Metadata { get; set; }
        public Resp<T> response { get; set; }

        public Resp<T> GetResponse(string str)
        {
            return BpjswsResponseConvert.Convert<Resp<T>>(str);
        }

        public class MetaData
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }

        public class Resp<T>
        {
            public int Count { get; set; } = 0;
            public string Response { get; set; }
        }
    }
}
