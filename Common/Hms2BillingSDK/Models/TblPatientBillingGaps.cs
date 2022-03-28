using System;
using System.Collections.Generic;

#nullable disable

namespace Hms2BillingSDK.Models
{
    public partial class TblPatientBillingGaps
    {
        public int PatientbillinggapId { get; set; }
        public int? PatientId { get; set; }
        public int? HospiceId { get; set; }
        public DateTime? BillingGapStartDate { get; set; }
        public DateTime? BillingGapEndDate { get; set; }
        public string BillingNote { get; set; }
        public DateTime? BillingSubmitted { get; set; }
        public int? UserId { get; set; }
    }
}
