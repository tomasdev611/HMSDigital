using System;

namespace HospiceSource.Digital.Patient.SDK.ViewModels
{
    public class PatientStatusValidationRequest
    {
        public Guid PatientUuid { get; set; }

        public int? StatusId { get; set; }

        public bool IsDMEEquipmentLeft { get; set; }

        public bool HasOpenOrders { get; set; }

        public bool HasOrders { get; set; }
    }
}
