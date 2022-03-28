using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblCustomers
    {
        public int CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyState { get; set; }
        public string CompanyZip { get; set; }
        public int? CompanyReportingNational { get; set; }
        public int? CompanyReportingRegions { get; set; }
        public int? CompanyReportingQuality { get; set; }
        public int? CompanyInactive { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
        public int? UpdateUserId { get; set; }
    }
}
