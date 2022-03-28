using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblHospices
    {
        public int HospiceId { get; set; }
        public string HospiceName { get; set; }
        public string HospiceAddress { get; set; }
        public string HospiceCity { get; set; }
        public string HospiceState { get; set; }
        public string HospiceZip { get; set; }
        public string HospicePhone { get; set; }
        public int? CustomerId { get; set; }
        public string HospiceBillingId { get; set; }
        public string HospiceNotifications { get; set; }
        public string LegacyMitsClass { get; set; }
        public string LegacyMitsDivision { get; set; }
        public string LocationCode { get; set; }
        public int? UserId { get; set; }
        public string CostCenter { get; set; }
        public decimal? PerDiemOverride { get; set; }
        public string InvoiceEmail { get; set; }
        public int? NoPerdiemLines { get; set; }
        public int? CallInDays { get; set; }
        public string BillingContact { get; set; }
        public string BillingContactPhone { get; set; }
        public int? SendExcel { get; set; }
        public int? PdfInvoice { get; set; }
        public int? DetailBilling { get; set; }
        public int? RevenueOnly { get; set; }
        public int? CreditHold { get; set; }
        public int? PaymentPlan { get; set; }
        public int? EmailType { get; set; }
        public int? HospiceInactive { get; set; }
        public int AutoApproval { get; set; }
        public int OffCapApproval { get; set; }
        public string DmeId { get; set; }
        public int? Version2 { get; set; }
        public int? TestHospice { get; set; }
        public string PccrEmailCopy { get; set; }
        public decimal? CpdBudget { get; set; }
        public string RegionTag { get; set; }
        public string HospiceShortName { get; set; }
        public int? IncludeCoverSheet { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
        public int? UpdateUserId { get; set; }
        public string PaymentTerms { get; set; }
    }
}
