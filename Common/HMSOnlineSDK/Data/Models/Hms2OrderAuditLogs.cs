using System;
using System.Collections.Generic;

namespace HMSOnlineSDK.Data.Models
{
    public partial class Hms2OrderAuditLogs
    {
        public int Id { get; set; }
        public int Hms2OrderId { get; set; }
        public string Username { get; set; }
        public string Parameters { get; set; }
        public string QueryString { get; set; }
        public string Action { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
