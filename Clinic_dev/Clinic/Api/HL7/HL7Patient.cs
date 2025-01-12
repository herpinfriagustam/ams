using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Api.HL7
{
    public class HL7Patient : HL7DomainResource
    {
        public string resourceType = HL7ResourceType.PATIENT;
        public List<HL7Identifier> identifier = new List<HL7Identifier>();
        public bool active;
        public List<HL7HumanName> name = new List<HL7HumanName>();
        public List<HL7ContactPoint> telecom = new List<HL7ContactPoint>();
        public string gender;
        public string birthDate;
        public bool deceasedBoolean;
        public string deceasedDateTime;
        public List<HL7Address> address = new List<HL7Address>();
        public HL7CodeableConcept maritalStatus;
        public bool multipleBirthBoolean;
        public int multipleBirthInteger;
        public List<HL7Attachment> photo = new List<HL7Attachment>();
        public List<HL7Contact> contact = new List<HL7Contact>();
        public List<HL7Communication> communication = new List<HL7Communication>();
        public List<HL7Link> link = new List<HL7Link>(); 
    }
}
