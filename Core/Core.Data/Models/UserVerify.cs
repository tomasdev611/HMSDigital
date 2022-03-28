using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class UserVerify
    {
        public int Id { get; set; }
        public int? ContactId { get; set; }
        public string Email { get; set; }
        public long? PhoneNumber { get; set; }
        public string Nonce { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
