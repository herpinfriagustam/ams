using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Class.Bpjsws
{
    public class BpjswsResponseBase
    {
        /// <summary>
        /// jika status code = BadRequest harus memeriksa status code number. jika status code number = 400 berarti memang benar, jika -1 berarti itu pengkondisian
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// status code dalam int e.g. 400, 500, 404 etc
        /// jika status code number = -1 nilai property StatusCode akan BadRequest ini merupakan pengkondisian
        /// </summary>
        public int StatusCodeNumber { get; set; }

        /// <summary>
        /// Response string dari request
        /// jika response string tidak sesuai dengan format bpjs pada umum nya atau response hanya text saja
        /// akan ada penambahan format sehingga menyerupai format response bpjs pada umum nya { Response : ... MetaData : { code: ..., message: ...}}
        /// </summary>
        public string ResponseString { get; set; }
    }
}
