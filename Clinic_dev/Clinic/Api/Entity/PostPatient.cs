using Clinic.Api.HL7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Api.Entity
{
    public class PostPatient : HL7Patient
    {
        public string resourceType;
    }
}
