using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblCustomerEmails
    {
        public int EmailId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string From { get; set; }
        public string Fromemail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int? Footer { get; set; }
        public int? All { get; set; }
        public int? CustomerId { get; set; }
    }
}
