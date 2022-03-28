using Audit.EntityFramework;
using HMSDigital.Common.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HMSDigital.Report.Data
{
    public partial class HMSReportAuditContext : HMSReportContext
    {
        private static DbContextHelperExtension _helper = new DbContextHelperExtension();

        private readonly HttpContext _httpContext;

        private readonly IAuditDbContext _auditContext;

        public HMSReportAuditContext(DbContextOptions<HMSReportAuditContext> options, IHttpContextAccessor context) : base(ChangeOptionsType<HMSReportContext>(options))
        {
            _auditContext = new DefaultAuditContext(this);
            _helper.SetConfig(_auditContext);
            _httpContext = context.HttpContext;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
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

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            //var createdEntries = entries.Where(e => _modelsTrackedForCreation.Contains(e.Entity.GetType()));
            //var updatedEntries = entries.Where(e => _modelsTrackedForUpdation.Contains(e.Entity.GetType()));

            //foreach (var entry in createdEntries)
            //{
            //    switch (entry.State)
            //    {
            //        case EntityState.Added:
            //            entry.CurrentValues["CreatedDateTime"] = DateTime.UtcNow;
            //            entry.CurrentValues["CreatedByUserId"] = GetUserId();
            //            entry.CurrentValues["UpdatedDateTime"] = DateTime.UtcNow;
            //            entry.CurrentValues["UpdatedByUserId"] = GetUserId();
            //            break;
            //    }
            //}

            //foreach (var entry in updatedEntries)
            //{
            //    switch (entry.State)
            //    {
            //        case EntityState.Modified:
            //            entry.CurrentValues["UpdatedDateTime"] = DateTime.UtcNow;
            //            entry.CurrentValues["UpdatedByUserId"] = GetUserId();
            //            break;
            //    }
            //}

            //var deletedEntries = entries.Where(e => _modelsTrackedForSoftDelete.Contains(e.Entity.GetType()));
            //foreach (var entry in deletedEntries)
            //{
            //    switch (entry.State)
            //    {
            //        case EntityState.Deleted:
            //            entry.State = EntityState.Modified;
            //            entry.CurrentValues["IsDeleted"] = true;
            //            entry.CurrentValues["DeletedDateTime"] = DateTime.UtcNow;
            //            entry.CurrentValues["DeletedByUserId"] = GetUserId();
            //            break;
            //    }
            //}
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
            if(userIdClaim != null)
            {
                return int.Parse(userIdClaim.Value);
            }
            return null;
        }
    }
}
