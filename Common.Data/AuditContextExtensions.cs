using Audit.Core;
using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMSDigital.Common.Data
{
    public static class AuditContextExtensions
    {
        public static void ConfigureAuditContext(AuditEvent auditEvent, EventEntry eventEntry, dynamic auditLog)
        {
            if (auditEvent.CustomFields.TryGetValue(Constants.AUDIT_USER_ID, out var userIdField))
            {
                if (int.TryParse(userIdField.ToString(), out var userId))
                {
                    auditLog.UserId = userId;
                }
            }
            if (auditEvent.CustomFields.TryGetValue(Constants.AUDIT_CLIENT_IP_ADDRESS, out var clientIPAddress))
            {
                auditLog.ClientIpaddress = clientIPAddress.ToString();
            }

            if (eventEntry.Action.Equals("Update", StringComparison.InvariantCultureIgnoreCase))
            {
                eventEntry.Changes = eventEntry.Changes.Where(ch => !Equals(ch.NewValue, ch.OriginalValue)).ToList();
                if (eventEntry.Changes.Count() <= 0)
                {
                    auditLog.AuditData = null;
                }
                else
                {
                    auditLog.AuditData = eventEntry.Changes;
                }
            }
            else if (eventEntry.Action.Equals("Insert", StringComparison.InvariantCultureIgnoreCase)
                     || eventEntry.Action.Equals("Delete", StringComparison.InvariantCultureIgnoreCase))
            {
                List<EventEntryChange> changes = new List<EventEntryChange>();
                EventEntryChange eventEntryChange = null;
                foreach (PropertyEntry propertyEntry in eventEntry.GetEntry().Properties)
                {
                    if (propertyEntry.Metadata.IsPrimaryKey())
                    {
                        continue;
                    }
                    eventEntryChange = new EventEntryChange();
                    eventEntryChange.ColumnName = propertyEntry.Metadata.Name;
                    if (eventEntry.Action.Equals("Delete", StringComparison.InvariantCultureIgnoreCase))
                    {
                        eventEntryChange.OriginalValue = propertyEntry.OriginalValue;
                    }
                    else
                    {
                        eventEntryChange.NewValue = propertyEntry.CurrentValue;
                    }
                    changes.Add(eventEntryChange);
                }
                auditLog.AuditData = changes;
            }
            else
            {
                auditLog.AuditData = string.Empty;
            }
            auditLog.AuditAction = eventEntry.Action;
            auditLog.AuditDate = DateTime.UtcNow;
            
        }
    }
}
