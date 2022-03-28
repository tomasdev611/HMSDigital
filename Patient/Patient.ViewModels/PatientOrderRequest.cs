using System;

namespace HMSDigital.Patient.ViewModels
{
    public class PatientOrderRequest
    {
        public Guid PatientUUID { get; set; }

        public string OrderNumber { get; set; }

        public bool HasDMEEquipment { get; set; }
    }
}
