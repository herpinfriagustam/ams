using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Api.HL7
{
    public class SatuSehat
    {
        public const string IDENTIFIER_SYSTEM_NIK = "https://fhir.kemkes.go.id/id/nik";
        public const string IDENTIFIER_SYSTEM_KK = "https://fhir.kemkes.go.id/id/kk";
        public const string IDENTIFIER_SYSTEM_PASPOR = "https://fhir.kemkes.go.id/id/paspor";

        public const string META_PROFILE_PATIENT = "https://fhir.kemkes.go.id/r4/StructureDefinition/Patient";

        public const string EXTENSION_URL_PROVINCE = "province";
        public const string EXTENSION_URL_CITY = "city";
        public const string EXTENSION_URL_DISTRICT = "district";
        public const string EXTENSION_URL_VILLAGE = "village";
        public const string EXTENSION_URL_RT = "rt";
        public const string EXTENSION_URL_RW = "rw";

        public const string EXTENSION_URL_BIRTH_PLACE = "https://fhir.kemkes.go.id/r4/StructureDefinition/birthPlace";
        public const string EXTENSION_URL_CITIZENSHIP_STATUS = "https://fhir.kemkes.go.id/r4/StructureDefinition/citizenshipStatus";
        public const string EXTENSION_URL_ADMINISTRATIVE_CODE = "https://fhir.kemkes.go.id/r4/StructureDefinition/administrativeCode";

        
    }
}
