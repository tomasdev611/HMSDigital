using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Models
{
    public class PatientDetail
    {
        public int Id { get; set; }

        public string UniqueId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<PhoneNumberMinimal> PhoneNumbers { get; set; }

        public IEnumerable<NoteResponse> PatientNotes { get; set; }

        public bool IsInfectious { get; set; }
    }
}
