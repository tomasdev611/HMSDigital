using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblPatientRespite
    {
        public int RespiteId { get; set; }
        public int? OrderIdPickup { get; set; }
        public int OrderIdDelivery { get; set; }
        public int? PatientId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Phone1 { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int UserId { get; set; }
        public DateTime MoveDate { get; set; }
        public string ContactPerson { get; set; }
        public int Active { get; set; }
    }
}
