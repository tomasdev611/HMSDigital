using System;

namespace HMS2SDK.Data.Models
{
    public partial class PickupStaging
    {
        public int Id { get; set; }
        public int SourcePatientId { get; set; }
        public int HmsPatientId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public string Phone { get; set; }
        public string HospiceName { get; set; }
        public int? HmsHospiceId { get; set; }
        public int PickupId { get; set; }
        public int DeliveryId { get; set; }
        public DateTime? DeliveryOrdered { get; set; }
        public DateTime? DeliveryDispatched { get; set; }
        public DateTime? DeliveryCompleted { get; set; }
        public DateTime? PickupOrdered { get; set; }
        public DateTime? PickupDispatched { get; set; }
        public DateTime? PickupCompleted { get; set; }
        public string RrInvCode { get; set; }
    }
}
