using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class SitePhoneNumber
    {
        public int Id { get; set; }
        public int? SiteId { get; set; }
        public int? PhoneNumberId { get; set; }

        public virtual PhoneNumbers PhoneNumber { get; set; }
        public virtual Sites Site { get; set; }
    }
}
