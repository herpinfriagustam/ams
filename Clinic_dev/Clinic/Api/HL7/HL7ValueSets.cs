using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Api.HL7
{
    public class HL7ValueSets
    {
        public enum AdministrativeGender
        {
            male, female, other, unknown
        }

        public enum LinkType
        {
            replaced_by, replaces, refer, seealso
        }

        public enum IdentifierUse
        {
            usual, official, temp, secondary, old
        }

        public enum IdentifierType
        {
            DL, PPN, BRN, MR, MCN, TAX, NIIP, PRN,
            MD, DR, ACSN, UDI, SNO, SB, PLAC, FILL, JHN
        }

        public enum NameUse
        {
            usual, official, temp, nickname, anonymous, old, maiden
        }

        public enum ContactPointSystem {
            phone, fax, email, pager, url, sms, other
        }

        public enum ContactPointUse
        {
            home, work, temp, old, mobile
        }

        public enum MaritalStatus
        {
            A, D, I, L, M, C, P, T, U, S, W, UNK
        }

        public enum PatientRelationship {
            BP, CP, EP, PR, E, C, F, I, N, S, U
        }

        public enum AddressUse
        {
            home, work, temp, old, billing
        }

        public enum AddressType
        {
            postal, physical, both
        }
    }
}
