using System;
using System.Collections.Generic;
using System.Text;

namespace Patient.ViewModels.NetSuite
{
    public class PatientLookUp
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid PatientUuid { get; set; }
    }
}
