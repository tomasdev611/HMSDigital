using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class DispatchAuditLog
    {
        public Guid AuditUuid { get; set; }
        public string AuditData { get; set; }
        public string AuditAction { get; set; }
        public int? UserId { get; set; }
        public int EntityId { get; set; }
        public DateTime AuditDate { get; set; }
        public string ClientIpaddress { get; set; }
        public Guid? PatientUuid { get; set; }

        public virtual Users User { get; set; }
    }
}
