using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Class.Bpjsws
{
    public class BpjswsPcare
    {
        public static BpjswsResponseList GetList(string url)
        {
            string unixTime = Bpjsws.CurrentUnixTime.ToString();

            // headers
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "x-cons-id",  Bpjsws.CONS_ID },
                { "x-timestamp",  unixTime },
                { "x-signature",  Bpjsws.CreateSignature(unixTime) },
                { "x-authorization",  Bpjsws.AUTHORIZATION },
                { "user_key",  Bpjsws.USER_KEY },
            };

            // do request
            BpjswsResponseList response = Bpjsws.Request<BpjswsResponseList>(url,
                Bpjsws.HttpMethodMode.Get,
                Bpjsws.PostDataType.Json,
                headers);

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
                { "x-authorization",  Bpjsws.AUTHORIZATION },
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

        public static BpjswsResponse Delete(string url)
        {
            string unixTime = Bpjsws.CurrentUnixTime.ToString();

            // headers
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "x-cons-id",  Bpjsws.CONS_ID },
                { "x-timestamp",  unixTime },
                { "x-signature",  Bpjsws.CreateSignature(unixTime) },
                { "x-authorization",  Bpjsws.AUTHORIZATION },
                { "user_key",  Bpjsws.USER_KEY },
            };

            // do request
            BpjswsResponse response = Bpjsws.Request<BpjswsResponse>(url,
                Bpjsws.HttpMethodMode.Delete,
                Bpjsws.PostDataType.Json,
                headers);

            return response;
        }

        #region Diagnosa

        public static BpjswsResponseList GetDiagnosa(string codeOrName, int offset = 1, int limit = 10)
        {
            string url = Bpjsws.WS_PCARE_DIAGNOSA_GET_DIAGNOSA_URL
                .Replace(@"{Parameter 1}", codeOrName)
                .Replace(@"{Parameter 2}", offset+"")
                .Replace(@"{Parameter 3}", limit+"");

            return GetList(url);
        }

        #endregion Diagnosa

        #region Dokter

        public static BpjswsResponseList GetDokter(int offset = 1, int limit = 10)
        {
            string url = Bpjsws.WS_PCARE_DOKTER_GET_URL
                .Replace(@"{Parameter 1}", offset + "")
                .Replace(@"{Parameter 2}", limit + "");

            return GetList(url);
        }

        #endregion Dokter

        #region kelompok

        public static BpjswsResponseList GetClubProtanis(string groupType)
        {
            string url = Bpjsws.WS_PCARE_GROUP_GET_CLUB_PROTANIS_URL
                .Replace(@"{Parameter 1}", groupType + "");

            return GetList(url);
        }

        public static BpjswsResponseList GetKegiatanKelompok(string ddmmyyyy)
        {
            string url = Bpjsws.WS_PCARE_GROUP_GET_ACTIVITY_URL
                .Replace(@"{Parameter 1}", ddmmyyyy + "");

            return GetList(url);
        }

        public static BpjswsResponseList GetPesertaKegiatanKelompok(string eduId)
        {
            string url = Bpjsws.WS_PCARE_GROUP_GET_PATIENT_ACTIVITY_URL
                .Replace(@"{Parameter 1}", eduId + "");

            return GetList(url);
        }

        public static BpjswsResponse AddKegiatanKelompok(JObject json)
        {
            string url = Bpjsws.WS_PCARE_GROUP_POST_ACTIVITY_URL;
            return Post(url, json);
        }

        public static BpjswsResponse AddPesertaKegiatanKelompok(JObject json)
        {
            string url = Bpjsws.WS_PCARE_GROUP_POST_PATIENT_ACTIVITY_URL;
            return Post(url, json);
        }

        public static BpjswsResponse DeleteKegiatanKelompok(string eduid)
        {
            string url = Bpjsws.WS_PCARE_GROUP_DELETE_ACTIVITY_URL
                .Replace(@"{Parameter 1}", eduid);

            return Delete(url);
        }

        public static BpjswsResponse DeletePesertaKegiatanKelompok(string eduid, string bpjsno)
        {
            string url = Bpjsws.WS_PCARE_GROUP_DELETE_PATIENT_ACTIVITY_URL
                .Replace(@"{Parameter 1}", eduid)
                .Replace(@"{Parameter 2}", bpjsno);

            return Delete(url);
        }

        #endregion kelompok

    }
}
