using Audit.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMSDigital.Common.Data
{
    public class DbContextHelperExtension : DbContextHelper
    {
        public new async Task<int> SaveChangesAsync(IAuditDbContext context, Func<Task<int>> baseSaveChanges)
        {
            if (context.AuditDisabled)
            {
                return await baseSaveChanges();
            }
            var auditEvent = CreateAuditEvent(context);
            if (auditEvent == null)
            {
                return await baseSaveChanges();
            }
            var entries = new List<EventEntry>(auditEvent.Entries)
                            .Where(e => !string.Equals(e.Action, "Update", StringComparison.OrdinalIgnoreCase)
                                        || e.Changes.Any(ch => !Equals(ch.NewValue, ch.OriginalValue)));

            if (entries == null || entries.Count() == 0)
            {
                return await baseSaveChanges();
            }

            foreach (var entry in entries)
            {
                auditEvent.Entries = new List<EventEntry>() { entry };
                var scope = await CreateAuditScopeAsync(context, auditEvent);
                auditEvent.Result = await baseSaveChanges();
                try
                {
                    await SaveScopeAsync(context, scope, auditEvent);
                    auditEvent.Success = true;
                }
                catch
                {
                }
            }
            return auditEvent.Result;
        }

        public new int SaveChanges(IAuditDbContext context, Func<int> baseSaveChanges)
        {
            if (context.AuditDisabled)
            {
                return baseSaveChanges();
            }
            var auditEvent = CreateAuditEvent(context);
            if (auditEvent == null)
            {
                return baseSaveChanges();
            }

            var scope = CreateAuditScope(context, auditEvent);
            auditEvent.Result = baseSaveChanges();
            try
            {
                SaveScope(context, scope, auditEvent);
                auditEvent.Success = true;
            }
            catch
            {
            }
            return auditEvent.Result;
        }
    }
}

