using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblUsers
    {
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Carrier { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? HospiceId { get; set; }
        public int? CustomerId { get; set; }
        public string LegacyMitsClass { get; set; }
        public string LegacyMitsDivision { get; set; }
        public int? UserLevel { get; set; }
        public int? MgrMultiSite { get; set; }
        public string DispatchAreas { get; set; }
        public int? Inactive { get; set; }
        public string LocationId { get; set; }
        public int? InventoryAccess { get; set; }
        public string InventoryAreas { get; set; }
        public int? DmeId { get; set; }
        public int? ApprovingManager { get; set; }
        public string QuickState { get; set; }
        public string QuickFacilities { get; set; }
        public int? AlwaysEmail { get; set; }
        public int? OffCapApproval { get; set; }
        public int? FacilityManager { get; set; }
        public int? OffCapApprovalType { get; set; }
        public int? InvoicePreviewer { get; set; }
        public int? NurseManager { get; set; }
        public int? UsersManager { get; set; }
        public int? Version2 { get; set; }
        public string Temppass { get; set; }
        public string MultipleHospices { get; set; }
        public string Pwtoken { get; set; }
        public DateTime? PwtokenExpired { get; set; }
        public int? PccrPermission { get; set; }
        public string GmtOffset { get; set; }
        public int? SuperLogin { get; set; }
        public DateTime? UserCreated { get; set; }
    }
}
