using System;

namespace HMSDigital.Patient.ViewModels
{
    public class FulfillmentRecordRequest
    {
        public FulfillmentRecordRequest()
        {
            StatusChangedDate = DateTime.UtcNow;
        }

        public Guid PatientUUID { get; set; }

        public bool IsDMEEquipmentLeft { get; set; }

        public DateTime StatusChangedDate { get; set; }

        public string OrderStatus { get; set; }

        public string Reason { get; set; }

        public bool HasOpenOrders { get; set; }
    }
}
