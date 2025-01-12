using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Api.HL7
{
    public class HL7Element
    {
        public string id;
        public List<Dictionary<string, object>> extension = new List<Dictionary<string, object>>();
    }
}
