using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class FacilityPatientSearchResponse : FacilityPatientResponse
    {
        public bool Active { get; set; }

        public DateTime AssignedDateTime { get; set; }
    }
}
