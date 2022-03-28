using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class Emails
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public bool IsVerified { get; set; }
        public string EmailType { get; set; }
        public bool? IsPrimary { get; set; }
        public int? AdditionalField1 { get; set; }
    }
}
