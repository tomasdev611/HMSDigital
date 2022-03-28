using Audit.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class AuditLog
    {
        public int AuditId { get; set; }

        public IEnumerable<EventEntryChange> AuditData { get; set; }

        public string AuditAction { get; set; }

        public int UserId { get; set; }

        public int EntityId { get; set; }

        public User User { get; set; }

        public DateTime AuditDate { get; set; }

        public string ClientIpaddress { get; set; }
    }
}
