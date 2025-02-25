using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Class.Bpjsws
{
    public class BpjswsRequestBase : Bpjsws
    {
        public static string RequestUnixTime = "";
        public static BpjswsResponse Get(string url)
        {
            string unixTime = CurrentUnixTime.ToString();

            // headers
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "x-cons-id",  CONS_ID },
                { "x-timestamp",  unixTime },
                { "x-signature",  CreateSignature(unixTime) },
                { "x-authorization",  AUTHORIZATION_PCARE },
                { "user_key",  USER_KEY },
            };

            // do request
            BpjswsResponse response = Request<BpjswsResponse>(url,
                HttpMethodMode.Get,
                PostDataType.Json,
                headers);

            if (response != null) response.RequestTimestamp = unixTime;

            return response;
        }

        public static BpjswsResponse Post(string url, JObject json)
        {
            string unixTime = CurrentUnixTime.ToString();

            // headers
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "x-cons-id",  CONS_ID },
                { "x-timestamp",  unixTime },
                { "x-signature",  CreateSignature(unixTime) },
                { "x-authorization",  AUTHORIZATION_PCARE },
                { "user_key",  USER_KEY },
            };

            // handle response
            BpjswsResponse response = Request<BpjswsResponse>(url,
                HttpMethodMode.Post,
                PostDataType.Json,
                headers,
                new Dictionary<string, string>
                {
                    { "RAW", json.ToString() }
                });

            if (response != null) response.RequestTimestamp = unixTime;

            return response;
        }

        public static BpjswsResponse Delete(string url)
        {
            string unixTime = CurrentUnixTime.ToString();

            // headers
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "x-cons-id",  CONS_ID },
                { "x-timestamp",  unixTime },
                { "x-signature",  CreateSignature(unixTime) },
                { "x-authorization",  AUTHORIZATION_PCARE },
                { "user_key",  USER_KEY },
            };

            // do request
            BpjswsResponse response = Request<BpjswsResponse>(url,
                HttpMethodMode.Delete,
                PostDataType.Json,
                headers);

            if (response != null) response.RequestTimestamp = unixTime;

            return response;
        }

        public static BpjswsResponse Put(string url, JObject json)
        {
            string unixTime = CurrentUnixTime.ToString();

            // headers
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "x-cons-id",  CONS_ID },
                { "x-timestamp",  unixTime },
                { "x-signature",  CreateSignature(unixTime) },
                { "x-authorization",  AUTHORIZATION_PCARE },
                { "user_key",  USER_KEY },
            };

            // do request
            BpjswsResponse response = Request<BpjswsResponse>(url,
                HttpMethodMode.Put,
                PostDataType.Json,
                headers,
                new Dictionary<string, string>
                {
                    { "RAW", json.ToString() }
                });

            if (response != null) response.RequestTimestamp = unixTime;

            return response;
        }
    }
}
