using System;

namespace HMS2SDK.Data.Models
{
    public partial class Inventoryserialnumbers
    {
        public string Da { get; set; }
        public string CurAcctNumber { get; set; }
        public int? CurHours { get; set; }
        public string Division { get; set; }
        public int? InvId { get; set; }
        public string LastAcctNumber { get; set; }
        public DateTime LastEdit { get; set; }
        public string MaintDue { get; set; }
        public string Manufacture { get; set; }
        public DateTime? PreMaintDue { get; set; }
        public string SerialNumber { get; set; }
        public string Status { get; set; }
        public int Id { get; set; }
        public string MfgSerialNumber { get; set; }
    }
}
