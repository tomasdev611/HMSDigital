using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class FacilityPatientResponse
    {
        public int FacilityId { get; set; }

        public string PatientRoomNumber { get; set; }

        public Guid PatientUuid { get; set; }

        public Facility Facility { get; set; }
    }
}
