using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Api.HL7
{
    public class HL7DomainResource : HL7Resource
    {
        public string text;
        public List<HL7Resource> contained;
        public List<Dictionary<string, object>> extension;
        public List<Dictionary<string, object>> modifierExtension;
    }
}
