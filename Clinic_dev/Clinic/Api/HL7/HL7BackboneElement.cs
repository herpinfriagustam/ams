using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Api.HL7
{
    public class HL7Contact
    {
        public List<HL7CodeableConcept> relationship = new List<HL7CodeableConcept>();
        public HL7HumanName name;
        public List<HL7ContactPoint> telecom = new List<HL7ContactPoint>();
        public HL7Address address;
        public string gender;
        public string organization;
        public HL7Period period;
    }

    public class HL7Communication
    {
        public HL7CodeableConcept language;
        public bool preferred;
    }

    public class HL7Link
    {
        public string code;
        public string other;
    }
}
