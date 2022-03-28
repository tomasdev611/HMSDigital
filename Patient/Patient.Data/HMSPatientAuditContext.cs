using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using HMSDigital.Patient.Data.Models;
using Audit.EntityFramework;
using Audit.EntityFramework.Providers;
using Audit.Core;
using HMSDigital.Common.Data;

namespace HMSDigital.Patient.Data
{
    public partial class HMSPatientAuditContext : HMSPatientContext
    {
        private readonly IEnumerable<Type> _modelsTrackedForCreation;

        private readonly IEnumerable<Type> _modelsTrackedForUpdation;

        private readonly IEnumerable<Type> _modelsTrackedForSoftDelete;

        private static DbContextHelperExtension _helper = new DbContextHelperExtension();

        private readonly HttpContext _httpContext;

        private readonly IAuditDbContext _auditContext;

        public HMSPatientAuditContext(DbContextOptions<HMSPatientAuditContext> options, IHttpContextAccessor context) : base(ChangeOptionsType<HMSPatientContext>(options))
        {
            _auditContext = new DefaultAuditContext(this);
            _helper.SetConfig(_auditContext);
            _modelsTrackedForCreation = new List<Type>() { typeof(PatientDetails), typeof(PatientNotes), typeof(PatientMergeHistory) };
            _modelsTrackedForUpdation = new List<Type>() { typeof(PatientDetails), typeof(PatientNotes) };
            _modelsTrackedForSoftDelete = new List<Type>() { typeof(PatientNotes) };
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
            var createdEntries = entries.Where(e => _modelsTrackedForCreation.Contains(e.Entity.GetType()));
            var updatedEntries = entries.Where(e => _modelsTrackedForUpdation.Contains(e.Entity.GetType()));

            foreach (var entry in createdEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["CreatedDateTime"] = DateTime.UtcNow;
                        entry.CurrentValues["CreatedByUserId"] = GetUserId();
                        entry.CurrentValues["UpdatedDateTime"] = DateTime.UtcNow;
                        entry.CurrentValues["UpdatedByUserId"] = GetUserId();
                        break;
                }
            }

            foreach (var entry in updatedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues["UpdatedDateTime"] = DateTime.UtcNow;
                        entry.CurrentValues["UpdatedByUserId"] = GetUserId();
                        break;
                }
            }

            var deletedEntries = entries.Where(e => _modelsTrackedForSoftDelete.Contains(e.Entity.GetType()));
            foreach (var entry in deletedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        entry.CurrentValues["DeletedDateTime"] = DateTime.UtcNow;
                        entry.CurrentValues["DeletedByUserId"] = GetUserId();
                        break;
                }
            }
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
