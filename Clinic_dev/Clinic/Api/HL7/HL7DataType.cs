using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Api.HL7
{
    public class HL7Coding : HL7Element
    {
        public string system;
        public string version;
        public string code;
        public string display;
        public bool userSelected;
    }

    public class HL7CodeableConcept : HL7Element
    {
        public List<HL7Coding> coding = new List<HL7Coding>();
        public string text;
    }

    public class HL7Period : HL7Element
    {
        public string start;
        public string end;
    }

    public class HL7HumanName : HL7Element
    {
        public HL7ValueSets.NameUse use;
        public string text;
        public string family;
        public List<string> given = new List<string>();
        public List<string> prefix = new List<string>();
        public List<string> suffix = new List<string>();
        public HL7Period period;
    }

    public class HL7Identifier : HL7Element
    {
        public HL7ValueSets.IdentifierUse use;
        public HL7CodeableConcept type;
        public string system;
        public string value;
        public HL7Period period;
        public string assigner;
    }

    public class HL7ContactPoint : HL7Element
    {
        public HL7ValueSets.ContactPointSystem system;
        public string value;
        public HL7ValueSets.ContactPointUse use;
        public int rank;
        public HL7Period period;
    }

    public class HL7Address : HL7Element
    {
        public HL7ValueSets.AddressUse use;
        public string type;
        public string text;
        public List<string> line = new List<string>();
        public string city;
        public string district;
        public string state;
        public string postalCode;
        public string country;
        public HL7Period period;
    }

    public class HL7Attachment : HL7Element
    {
        public string contentType;
        public string language;
        public string data;
        public string url;
        public int size;
        public string hash;
        public string title;
        public string creation;
        public int height;
        public int width;
        public int frames;
        public double durations;
        public int pages;
    }

    public class HL7Meta : HL7Element
    {
        public string versionId;
        public string lastUpdated;
        public string source;
        public List<string> profile;
        public string security;
        public string tag;
    }

    //public class HL7Extension : HL7Element
    //{
    //    public string url;
    //    public Dictionary<string, object> value = new Dictionary<string, object>();
    //}
}
