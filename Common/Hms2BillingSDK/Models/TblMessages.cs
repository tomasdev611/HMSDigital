using System;
using System.Collections.Generic;

#nullable disable

namespace Hms2BillingSDK.Models
{
    public partial class TblMessages
    {
        public int MessageId { get; set; }
        public int? MessageStatus { get; set; }
        public DateTime? MessageEnd { get; set; }
        public string MessageTitle { get; set; }
        public string MessageBody { get; set; }
    }
}
