using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Api.Entity
{
    public class PatientPost
    {
        public string resourceType = "Patient";
        public Meta meta = new Meta();
        public List<Identifier> identifier = new List<Identifier>();
        public bool active;
        public List<Name> name = new List<Name>();
        public List<Telecom> telecom = new List<Telecom>();
        public string gender;
        public string birthDate;
        public bool deceasedBoolean;
        public List<Address> address = new List<Address>();
        public MaritalStatus maritalStatus = new MaritalStatus();
        public int multipleBirthInteger;
        public List<Contact> contact = new List<Contact>();
        public List<Communication> communication = new List<Communication>();
        public List<Extension> extension = new List<Extension>();

        public class Meta
        {
            public string[] profile = new string[] { "https://fhir.kemkes.go.id/r4/StructureDefinition/Patient" };
        }

        public class Identifier
        {
            public string use;
            public string system;
            public string value;
        }

        public class Address
        {
            public string use;
            public string[] line;
            public string city;
            public string postalCode;
            public string country;
            public Extension extension;

            public class Extension
            {
                public string url;
                public List<ExtensionExt> extension = new List<ExtensionExt>();

                public class ExtensionExt
                {
                    public string url;
                    public string valueCode;
                }
            }
        }

        public class MaritalStatus
        {
            public string text;
            public List<Coding> coding = new List<Coding>();
        }

        public class Contact
        {
            public List<Relationship> relationship;
            public Name name = new Name();
            public List<Telecom> telecom = new List<Telecom>();

            public class Relationship
            {
                public List<Coding> coding = new List<Coding>();
            }
        }

        public class Communication
        {

            public bool preferred;
            public Language language = new Language();

            public class Language
            {
                public string text;
                public List<Coding> coding = new List<Coding>();
            }
        }

        // shared entity
        public class Coding
        {
            public string system;
            public string code;
            public string display;
        }

        public class Name
        {
            public string use;
            public string text;
        }

        public class Telecom
        {
            public string use;
            public string system;
            public string value;
        }

        public class Extension
        {
            public string url;
            public string valueCode;
            public ValueAddress valueAddress = new ValueAddress();

            public class ValueAddress
            {
                public string city;
                public string country;
            }
        }
    }

}
