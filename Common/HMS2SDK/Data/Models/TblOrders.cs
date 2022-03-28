using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblOrders
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderArray { get; set; }
        public DateTime Timestamp { get; set; }
        public int Status { get; set; }
        public int PatientId { get; set; }
        public int? DriverId { get; set; }
        public string Instructions { get; set; }
        public DateTime? DispatchTimestamp { get; set; }
        public DateTime? DeliveredTimestamp { get; set; }
        public DateTime? PickedupTimestamp { get; set; }
        public DateTime? CancelledTimestamp { get; set; }
        public int? DeliveryTiming { get; set; }
        public int? PickupType { get; set; }
        public string DeliveryType { get; set; }
        public DateTime? FutureDate { get; set; }
        public string FutureTimeRange { get; set; }
        public int? HospiceId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderingNurse { get; set; }
        public int? Pickup { get; set; }
        public int? MailCustomer { get; set; }
        public string FacilityName { get; set; }
        public int? InvProcessed { get; set; }
        public int? MigrationOrder { get; set; }
        public string PickupReason { get; set; }
        public DateTime? OrgOrderDate { get; set; }
        public string OffCapArray { get; set; }
        public string OrderingNursePhone { get; set; }
        public DateTime? AutoApprovalTimestamp { get; set; }
        public int? OffCapOrder { get; set; }
        public int? OffCapApproval { get; set; }
        public int? OrderSource { get; set; }
        public int? DmeId { get; set; }
        public DateTime? DmeAcknowledgement { get; set; }
        public int? DmeuserId { get; set; }
        public int? OffCapApprovalOrgUserId { get; set; }
        public int? OffCapApprovalUserId { get; set; }
        public int? OffCapAutoApproval { get; set; }
        public int? TransitionOrder { get; set; }
        public int? BypassApproval { get; set; }
        public string BypassApprovalNotes { get; set; }
        public string AckComments { get; set; }
        public DateTime? AutoAckTimestamp { get; set; }
        public int? AckEmailSent { get; set; }
        public string OrgOrderArray { get; set; }
        public int? Hsapproval { get; set; }
        public string AppComments { get; set; }
        public DateTime? HsApproval1 { get; set; }
        public int? AppuserId { get; set; }
        public DateTime? HsAutoApproval { get; set; }
        public int? ModAutoApproval { get; set; }
        public int? ApprCustomer { get; set; }
        public int? RekeyOrder { get; set; }
        public int? Exchange { get; set; }
        public string ExchangeReason { get; set; }
        public int? MoveId { get; set; }
        public string MoveReason { get; set; }
        public int? Respite { get; set; }
        public string DeliveryNotes { get; set; }
        public int? BopOrder { get; set; }
        public int? ExchangeOrderId { get; set; }
        public int? HospDischarge { get; set; }
        public string HospDischargeTime { get; set; }
        public int DelAddress { get; set; }
        public string TimingOverride { get; set; }
        public string TimingOverrideNotes { get; set; }
    }
}
