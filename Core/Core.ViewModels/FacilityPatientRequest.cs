using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class FacilityPatientRequest
    {
        public IEnumerable<FacilityPatientRoom> FacilityPatientRooms { get; set; }

        public Guid PatientUuid { get; set; }
    }
}
