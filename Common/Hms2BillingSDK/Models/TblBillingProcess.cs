using System;
using System.Collections.Generic;

#nullable disable

namespace Hms2BillingSDK.Models
{
    public partial class TblBillingProcess
    {
        public int ProcessId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? ProcessComplete { get; set; }
        public DateTime? EmailSent { get; set; }
        public int? NoBilling { get; set; }
        public int? LimitHospiceId { get; set; }
    }
}
