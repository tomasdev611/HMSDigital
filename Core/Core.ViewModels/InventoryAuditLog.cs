using Audit.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class InventoryAuditLog
    {
        public int AuditId { get; set; }

        public IEnumerable<EventEntryChange> AuditData { get; set; }

        public string AuditAction { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int InventoryId { get; set; }

        public DateTime AuditDate { get; set; }

        public string ClientIPAddress { get; set; }

    }
}
