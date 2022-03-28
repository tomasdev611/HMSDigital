using System;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class MovePatientToHospiceLocationRequest
    {
        public int HospiceId { get; set; }

        public int HospiceLocationId { get; set; }

        public IEnumerable<FacilityPatientRoom> Facilities { get; set; }

        public DateTime MovementDate { get; set; }
    }
}