using Audit.Core;
using Audit.EntityFramework;
using Audit.EntityFramework.Providers;
using HMSDigital.Common.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HMSDigital.Notification.Data
{
    public class HMSNotificationAuditContext : HMSNotificationContext
    {
        private static DbContextHelper _helper = new DbContextHelper();

        private readonly HttpContext _httpContext;

        private readonly IAuditDbContext _auditContext;

        private static EntityFrameworkDataProvider _efDataProvider = new EntityFrameworkDataProvider()
        {
            AuditTypeMapper = (t, e) => GetAuditTableType(t, e),
            AuditEntityAction = HandleEventLog(),
        };

        public HMSNotificationAuditContext(DbContextOptions<HMSNotificationAuditContext> options, IHttpContextAccessor context) : base(ChangeOptionsType<HMSNotificationContext>(options))
        {
            _auditContext = new DefaultAuditContext(this);
            _auditContext.AuditDataProvider = _efDataProvider;
            _helper.SetConfig(_auditContext);
            _httpContext = context.HttpContext;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            return _helper.SaveChanges(_auditContext, () => base.SaveChanges());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _helper.SaveChangesAsync(_auditContext, () => base.SaveChangesAsync(cancellationToken));
        }

        protected static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext
        {
            var sqlExt = options.Extensions.FirstOrDefault(e => e is SqlServerOptionsExtension);

            if (sqlExt == null)
                throw (new Exception("Failed to retrieve SQL connection string for base Context"));

            return new DbContextOptionsBuilder<T>()
                        .UseSqlServer(((SqlServerOptionsExtension)sqlExt).ConnectionString)
                        .Options;
        }

        private int? GetUserId()
        {
            var userIdClaim = _httpContext.User.FindFirst("userId");
            if (userIdClaim != null)
            {
                return int.Parse(userIdClaim.Value);
            }
            return null;
        }

        private static Type GetAuditTableType(Type type, EventEntry eventEntry)
        {
            return null;
        }
        internal static Func<AuditEvent, EventEntry, object, bool> HandleEventLog()
        {
            return (auditEvent, eventEntry, auditLogInput) =>
            {
                var auditLog = auditLogInput as dynamic;

                AuditContextExtensions.ConfigureAuditContext(auditEvent, eventEntry, auditLog);

                if (string.IsNullOrEmpty(auditLog.AuditData))
                {
                    return false;
                }
                auditLog.EntityId = int.Parse(eventEntry.PrimaryKey.First().Value.ToString());
                return true;
            };
        }
    }
}

