using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Helpers
{
    public class ApiHelper
    {
        static IniHelper ini;
        static IniHelper Ini
        {
            get
            {
                if (ini == null)
                    ini = new IniHelper(AppDomain.CurrentDomain.BaseDirectory + @"KlinikSantosa.ini");

                return ini;
            }
        }

        public static string GetEnv()
        {
            try
            {
                return Ini.Read("satusehat", "env");
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static string IniGet(string key)
        {
            try
            {
                string env = GetEnv();
                if(env == "dev")
                {
                    return Ini.Read("satusehatdev", key);
                }
                else if (env == "prod")
                {
                    return Ini.Read("satusehatprod", key);
                }

                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static bool IniSet(string key)
        {
            try
            {
                string env = GetEnv();
                if (env == "dev")
                {
                    Ini.Write("satusehatdev", "token", key);
                }
                else if (env == "prod")
                {
                    Ini.Write("satusehatprod", "token", key);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool FetchToken(bool forceFetch = false)
        {
            string token = IniGet("token");
            if(forceFetch == false && token != null && token != "")
                return true;

            string authUrl = IniGet("auth_url");
            string clientId = IniGet("client_id");
            string clientSecret = IniGet("client_secret");

            RestClient client = new RestClient(authUrl);

            RestRequest request = new RestRequest("accesstoken", Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddQueryParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            IRestResponse response = client.Execute(request);

            JObject jResponse = JObject.Parse(response.Content);
            if (jResponse.ContainsKey("access_token"))
            {
                IniSet(jResponse["access_token"]?.ToString());
                return true;
            }

            return false;
        }
    }
}
