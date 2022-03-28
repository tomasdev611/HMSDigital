using System;
using System.Collections.Generic;

namespace HMSDigital.Patient.ViewModels
{
    public class PatientCreateRequest : PatientMinimal
    {
        public IEnumerable<PatientAddressRequest> PatientAddress { get; set; }

        public Guid? DataBridgeRunUuid { get; set; }

        public DateTime? DataBridgeRunDateTime { get; set; }

        public DateTime? LastOrderDateTime { get; set; }

        public int? StatusId { get; set; }
    }
}
