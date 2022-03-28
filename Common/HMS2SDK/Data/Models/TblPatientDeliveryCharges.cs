using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblPatientDeliveryCharges
    {
        public int PatientdeliverychargeId { get; set; }
        public int? PatientId { get; set; }
        public int? HospiceId { get; set; }
        public DateTime? ChargeStartDate { get; set; }
        public DateTime? ChargeEndDate { get; set; }
        public string ChargeNote { get; set; }
        public int? UserId { get; set; }
        public int? DoNotBill { get; set; }
        public int? OrderId { get; set; }
        public string OrderType { get; set; }
        public decimal? ChargeAmount { get; set; }
    }
}
