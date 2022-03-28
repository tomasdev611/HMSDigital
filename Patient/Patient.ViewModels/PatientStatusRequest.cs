using System;

namespace HMSDigital.Patient.ViewModels
{
    public class PatientStatusRequest
    {
        public Guid PatientUuid { get; set; }
        public string Status { get; set; }

        public DateTime StatusChangedDate { get; set; }

        public string Reason { get; set; }

        public bool IsDMEEquipmentLeft { get; set; }

        public bool HasOpenOrders { get; set; }

        public bool HasOrders { get; set; }
    }
}
