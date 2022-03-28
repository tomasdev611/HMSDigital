using System;
using Microsoft.EntityFrameworkCore;
using HMSDigital.Core.Data.Models;
using Audit.EntityFramework;
using Audit.EntityFramework.Providers;
using Audit.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Common.Data;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Core.Test")]
namespace HMSDigital.Core.Data
{
    public partial class HMSDigitalAuditContext : HMSDigitalContext
    {
        private readonly IEnumerable<Type> _modelsTrackedForCreation;

        private readonly IEnumerable<Type> _modelsTrackedForUpdation;

        private readonly IEnumerable<Type> _modelsTrackedForSoftDelete;

        private static DbContextHelperExtension _helper = new DbContextHelperExtension();

        private readonly IAuditDbContext _auditContext;

        private readonly HttpContext _httpContext;

        private IEnumerable<string> _userHospiceIds;

        private IEnumerable<string> _userSiteIds;

        private IEnumerable<string> _userHospiceLocationIds;

        public HMSDigitalAuditContext(DbContextOptions<HMSDigitalAuditContext> options, IHttpContextAccessor context) : base(ChangeOptionsType<HMSDigitalContext>(options))
        {
            _auditContext = new DefaultAuditContext(this);
            _helper.SetConfig(_auditContext);
            _modelsTrackedForCreation = new List<Type>()
            {
                typeof(Hospices),
                typeof(HospiceLocations),
                typeof(CsvMappings), typeof(Sites),
                typeof(Facilities),
                typeof(FacilityPatientHistory),
                typeof(Inventory),
                typeof(Items),
                typeof(ItemCategories),
                typeof(Users),
                typeof(Drivers),
                typeof(ItemImageFiles),
                typeof(SiteMembers),
                typeof(SiteMembers),
                typeof(ItemTransferRequests),
                typeof(DispatchInstructions),
                typeof(SiteServiceAreas),
                typeof(OrderNotes),
                typeof(OrderHeaders),
                typeof(OrderFulfillmentLineItems),
                typeof(OrderLineItems),
                typeof(HospiceLocationMembers),
                typeof(HospiceMember),
                typeof(ItemImages),
                typeof(UserProfilePicture),
                typeof(ContractRecords)
            };
            _modelsTrackedForUpdation = new List<Type>()
            {
                typeof(Hospices),
                typeof(HospiceLocations),
                typeof(CsvMappings),
                typeof(Sites),
                typeof(Facilities),
                typeof(FacilityPatientHistory),
                typeof(Inventory),
                typeof(Items),
                typeof(ItemCategories),
                typeof(Users),
                typeof(Drivers),
                typeof(ItemImageFiles),
                typeof(SiteMembers),
                typeof(SiteMembers),
                typeof(ItemTransferRequests),
                typeof(DispatchInstructions),
                typeof(SiteServiceAreas),
                typeof(OrderNotes),
                typeof(OrderHeaders),
                typeof(OrderFulfillmentLineItems),
                typeof(OrderLineItems),
                typeof(HospiceLocationMembers),
                typeof(HospiceMember),
                typeof(UserProfilePicture),
                typeof(ContractRecords)
            };
            _modelsTrackedForSoftDelete = new List<Type>()
            {
                typeof(HospiceLocations),
                typeof(Sites),
                typeof(Inventory),
                typeof(Items),
                typeof(ItemCategories),
                typeof(Hospices),
                typeof(Drivers)
            };
            _httpContext = context.HttpContext;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Hospices>().HasQueryFilter(o => ShouldIgnoreQueryFilters(GlobalFilters.Hospice)
                                                                                        || GetUserHospiceIds().Contains("*")
                                                                                        || GetUserHospiceIds().Contains(o.Id.ToString()));
            modelBuilder.Entity<Sites>().HasQueryFilter(l => !l.IsDeleted
                                                                    && (ShouldIgnoreQueryFilters(GlobalFilters.Site)
                                                                        || GetUserSiteIds().Contains("*") || GetUserSiteIds().Contains(l.Id.ToString())));

            modelBuilder.Entity<HospiceLocations>().HasQueryFilter(l => !l.IsDeleted
                                                                    && (ShouldIgnoreQueryFilters(GlobalFilters.Hospice)
                                                                        || GetUserHospiceIds().Contains("*") || GetUserHospiceIds().Contains(l.HospiceId.ToString())
                                                                        || ShouldIgnoreQueryFilters(GlobalFilters.HospiceLocation)
                                                                        || GetUserHospiceLocationIds().Contains("*") || GetUserHospiceLocationIds().Contains(l.Id.ToString())));


            modelBuilder.Entity<Facilities>().HasQueryFilter(f => ShouldIgnoreQueryFilters(GlobalFilters.Hospice)
                                                                || GetUserHospiceIds().Contains("*") || GetUserHospiceIds().Contains(f.HospiceId.ToString())
                                                                || ShouldIgnoreQueryFilters(GlobalFilters.HospiceLocation)
                                                                || GetUserHospiceLocationIds().Contains("*") || GetUserHospiceLocationIds().Contains(f.HospiceLocationId.ToString()));

            modelBuilder.Entity<HospiceMember>().HasQueryFilter(m => ShouldIgnoreQueryFilters(GlobalFilters.Hospice)
                                                                || GetUserHospiceIds().Contains("*") || GetUserHospiceIds().Contains(m.HospiceId.ToString())
                                                                || ShouldIgnoreQueryFilters(GlobalFilters.HospiceLocation)
                                                                || GetUserHospiceLocationIds().Contains("*") || m.HospiceLocationMembers.Any(ml => GetUserHospiceLocationIds().Contains(ml.HospiceLocationId.ToString())));

            modelBuilder.Entity<Inventory>().HasQueryFilter(l => ShouldIgnoreIsDeletedQueryFilters() || (!l.IsDeleted && !l.Item.IsDeleted));

            modelBuilder.Entity<Items>().HasQueryFilter(l => !l.IsDeleted);

            modelBuilder.Entity<ItemCategories>().HasQueryFilter(l => !l.IsDeleted);

            modelBuilder.Entity<SiteMembers>().HasQueryFilter(m => ShouldIgnoreQueryFilters(GlobalFilters.Site)
                                                                || GetUserSiteIds().Contains("*") || GetUserSiteIds().Contains(m.SiteId.ToString()));

            modelBuilder.Entity<Drivers>().HasQueryFilter(m => !m.IsDeleted && (ShouldIgnoreQueryFilters(GlobalFilters.Site)
                                                            || GetUserSiteIds().Contains("*") || GetUserSiteIds().Contains(m.CurrentSiteId.ToString())));

            modelBuilder.Entity<PatientInventory>().HasQueryFilter(o => ShouldIgnoreQueryFilters(GlobalFilters.Hospice)
                                                                    || GetUserHospiceIds().Contains("*") || GetUserHospiceIds().Contains(o.HospiceId.ToString())
                                                                    || ShouldIgnoreQueryFilters(GlobalFilters.HospiceLocation)
                                                                    || GetUserHospiceLocationIds().Contains("*") || GetUserHospiceLocationIds().Contains(o.HospiceLocationId.ToString()));
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

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var createdEntries = entries.Where(e => _modelsTrackedForCreation.Contains(e.Entity.GetType()));
            foreach (var entry in createdEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["CreatedDateTime"] = DateTime.UtcNow;
                        entry.CurrentValues["CreatedByUserId"] = GetUserId();
                        break;
                }
            }

            var updatedEntries = entries.Where(e => _modelsTrackedForUpdation.Contains(e.Entity.GetType()));
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
            var auditUserIdClaim = _httpContext.User.FindFirst("auditUserId");
            if (auditUserIdClaim != null)
            {
                return int.Parse(auditUserIdClaim.Value);
            }
            return null;
        }

        internal bool ShouldIgnoreQueryFilters(GlobalFilters ignoreFilterType)
        {
            if (_httpContext.Items.TryGetValue("IgnoreGlobalFilters", out object ignoreFilterObj) && (ignoreFilterObj is GlobalFilters ignoreFilterValue))
            {
                return ignoreFilterValue.HasFlag(ignoreFilterType);

            }
            return false;
        }

        internal void DisableGlobalFilter(GlobalFilters ignoreFilterType)
        {
            if (!ShouldIgnoreQueryFilters(GlobalFilters.All))
            {
                _httpContext.Items.Remove("IgnoreGlobalFilters");
                _httpContext.Items.Add("IgnoreGlobalFilters", ignoreFilterType);
            }
        }

        internal void EnableGlobalFilter(GlobalFilters ignoreFilterType)
        {
            if (_httpContext.Items.TryGetValue("IgnoreGlobalFilters", out object ignoreFilterObj)
                && (ignoreFilterObj is GlobalFilters ignoreFilterValue)
                && ignoreFilterValue == ignoreFilterType)
            {
                _httpContext.Items.Remove("IgnoreGlobalFilters");
            }
        }

        private bool ShouldIgnoreIsDeletedQueryFilters()
        {
            if (_httpContext.Items.TryGetValue("IgnoreIsDeletedFilters", out object ignoreFilterObj) && (ignoreFilterObj is bool ignoreFilterValue))
            {
                return ignoreFilterValue;
            }
            return false;
        }

        private IEnumerable<string> GetUserHospiceIds()
        {
            if (_userHospiceIds == null)
            {
                var userId = GetUserId();
                var userRoles = Set<UserRoles>().Where(ur => ur.ResourceType == ResourceTypes.Hospice.ToString() && ur.UserId == userId);
                if (userRoles == null)
                {
                    return new List<string>();
                }
                _userHospiceIds = userRoles.Select(ur => ur.ResourceId);
            }
            return _userHospiceIds;
        }

        private IEnumerable<string> GetUserHospiceLocationIds()
        {
            if (_userHospiceLocationIds == null)
            {
                var userId = GetUserId();
                var userRoles = Set<UserRoles>().Where(ur => ur.ResourceType == ResourceTypes.HospiceLocation.ToString() && ur.UserId == userId);
                if (userRoles == null)
                {
                    return new List<string>();
                }
                _userHospiceLocationIds = userRoles.Select(ur => ur.ResourceId);
            }
            return _userHospiceLocationIds;
        }

        internal IEnumerable<string> GetUserSiteIds()
        {
            if (_userSiteIds == null || _userSiteIds.Count() == 0)
            {
                var userId = GetUserId();
                var userRoles = Set<UserRoles>().Where(ur => ur.ResourceType == ResourceTypes.Site.ToString() && ur.UserId == userId);
                if (userRoles == null)
                {
                    return new List<string>();
                }
                _userSiteIds = userRoles.Select(ur => ur.ResourceId);
            }
            return _userSiteIds;
        }
    }
}
