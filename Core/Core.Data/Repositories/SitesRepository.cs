using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMSDigital.Core.Data.Repositories
{
    public class SitesRepository : RepositoryBase<Sites>, ISitesRepository
    {
        private static List<Sites> _allSitesCache = new List<Sites>();

        public SitesRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {
        }

        public override async Task<Sites> GetByIdAsync(int siteId)
        {
            return await GetAsync(s => s.Id == siteId);
        }

        public override async Task<Sites> GetAsync(Expression<Func<Sites, bool>> where)
        {
            await CacheAllSites();
            return GetAccessibleSites(_allSitesCache).FirstOrDefault(where.Compile());
        }

        public override async Task<IEnumerable<Sites>> GetManyAsync(Expression<Func<Sites, bool>> where)
        {
            await CacheAllSites();
            var entities = GetAccessibleSites(_allSitesCache).Where(where.Compile()).AsQueryable();
            return GetPaginatedSortedList(entities);
        }

        public override async Task<IEnumerable<Sites>> GetAllAsync()
        {
            await CacheAllSites();
            var entities = GetAccessibleSites(_allSitesCache).AsQueryable();
            return GetPaginatedSortedList(entities);
        }

        public override async Task AddAsync(Sites entity)
        {
            _allSitesCache.Clear();
            await base.AddAsync(entity);
        }

        public override async Task UpdateAsync(Sites entity)
        {
            _allSitesCache.Clear();
            await base.UpdateAsync(entity);
        }

        public override async Task<IEnumerable<Sites>> AddManyAsync(IEnumerable<Sites> entities)
        {
            _allSitesCache.Clear();
            return await base.AddManyAsync(entities);
        }

        public override async Task UpdateManyAsync(IEnumerable<Sites> entities)
        {
            _allSitesCache.Clear();
            await base.UpdateManyAsync(entities);
        }

        public override async Task DeleteAsync(Sites entity)
        {
            _allSitesCache.Clear();
            await base.DeleteAsync(entity);
        }

        public override async Task DeleteAsync(Expression<Func<Sites, bool>> where)
        {
            _allSitesCache.Clear();
            await base.DeleteAsync(where);
        }

        public new async Task<long> GetCountAsync(Expression<Func<Sites, bool>> where)
        {
            await CacheAllSites();
            return _sieveProcessor.Apply(SieveModel, GetAccessibleSites(_allSitesCache).Where(where.Compile()).AsQueryable(), applyPagination: false).Count();
        }

        public async Task RefreshSitesCache()
        {
            _allSitesCache.Clear();
            await CacheAllSites();
        }

        private IEnumerable<Sites> GetAccessibleSites(IEnumerable<Sites> allSites)
        {
            var accessibleSiteIds = _dbContext.GetUserSiteIds().ToList();
            var shouldIgnoreQueryFilters = _dbContext.ShouldIgnoreQueryFilters(GlobalFilters.Site);
            if (accessibleSiteIds.Contains("*") || shouldIgnoreQueryFilters)
            {
                return allSites.Where(s => !s.IsDeleted);
            }
            var accessibleNetSuiteLocationIds = allSites.Where(s => accessibleSiteIds.Contains(s.Id.ToString())).Select(s => s.NetSuiteLocationId);
            return allSites.Where(s => !s.IsDeleted
                                        && (accessibleSiteIds.Contains(s.Id.ToString())
                                              || (s.ParentNetSuiteLocationId.HasValue && accessibleNetSuiteLocationIds.Contains(s.ParentNetSuiteLocationId))));
        }

        private async Task CacheAllSites()
        {
            if (_allSitesCache == null || _allSitesCache.Count() == 0)
            {
                _dbContext.DisableGlobalFilter(GlobalFilters.Site);
                var entities = _dbContext.Set<Sites>()
                                    .Include(s => s.Address)
                                    .Include(s => s.SitePhoneNumber)
                                        .ThenInclude(sp => sp.PhoneNumber)
                                            .ThenInclude(p => p.NumberType)
                                    .Include(s => s.DriversCurrentVehicle)
                                        .ThenInclude(d => d.User)
                                    .AsQueryable();

                var allSites = await entities.ToListAsync();
                foreach (var site in allSites)
                {
                    site.SitePhoneNumber = site.SitePhoneNumber.OrderBy(p => p.PhoneNumberId).ToList();
                }
                _allSitesCache = allSites?.ToList();
                _dbContext.EnableGlobalFilter(GlobalFilters.Site);
            }
        }

        private IEnumerable<Sites> GetPaginatedSortedList(IQueryable<Sites> entities)
        {
            entities = _sieveProcessor.Apply(SieveModel, entities);
            return entities.ToList();
        }

    }
}
