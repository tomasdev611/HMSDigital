using HMSDigital.Patient.Data;
using HMSDigital.Patient.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMSDigital.Patient.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        #region Public Properties

        public HMSPatientAuditContext _dbContext { get; private set; }

        public SieveModel SieveModel { get; set; }

        public ISieveProcessor _sieveProcessor { get; set; }

        #endregion

        #region Constructor
        protected RepositoryBase(HMSPatientAuditContext dbContext, ISieveProcessor sieveProcessor)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessor;
        }
        #endregion

        #region Repository Implementation
        public async virtual Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async virtual Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
            return entities;

        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async virtual Task UpdateManyAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().AttachRange(entities);
            await _dbContext.SaveChangesAsync();

        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Expression<Func<T, bool>> where)
        {
            var dbSet = _dbContext.Set<T>();
            IEnumerable<T> objects = dbSet.Where(where).AsEnumerable();
            foreach (T obj in objects)
            {
                dbSet.Remove(obj);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async virtual Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return await GetManyAsync(x => true);
        }

        public async virtual Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where)
        {
            var entities = _dbContext.Set<T>().Where(where).AsQueryable();

            return await GetPaginatedSortedListAsync(entities);
        }

        public async virtual Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return (await GetManyAsync(where)).FirstOrDefault();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> where)
        {
            return await _dbContext.Set<T>().AnyAsync(where);
        }

        public async Task<decimal> GetSumAsync(Expression<Func<T, bool>> where, Expression<Func<T, decimal>> sumCol)
        {
            return await _dbContext.Set<T>().Where(where).SumAsync(sumCol);

        }

        /// <summary>
        /// Method for pagination and sorting S
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E>> GetPaginatedSortedListAsync<E>(IQueryable<E> entities)
        {
            entities = _sieveProcessor.Apply(SieveModel, entities);
            return await entities.ToListAsync();
        }

        public async Task<long> GetCountAsync(Expression<Func<T, bool>> where)
        {
            return await _sieveProcessor.Apply(SieveModel, _dbContext.Set<T>().Where(where), applyPagination: false).CountAsync();
        }
        #endregion
    }
}
