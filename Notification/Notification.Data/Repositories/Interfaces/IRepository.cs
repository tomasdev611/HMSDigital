using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMSDigital.Notification.Data.Repositories.Interfaces
{
    /// <summary>
    /// Base members all repositories should support
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        SieveModel SieveModel { get; set; }
        /// <summary>
        /// Adds an entity
        /// </summary>
        /// <param name="entity"></param>
        Task AddAsync(T entity);

        /// <summary>
        /// Add Many entities
        /// </summary>
        /// <param name="entities"></param>
        Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> entities);

        /// <summary>
        /// Updates an entity
        /// </summary>
        /// <param name="entity"></param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Update Many entities
        /// </summary>
        /// <param name="entities"></param>
        Task UpdateManyAsync(IEnumerable<T> entities);

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="entity"></param>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Deletes an entity based on a where expression
        /// </summary>
        /// <param name="where"></param>
        Task DeleteAsync(Expression<Func<T, bool>> where);

        /// <summary>
        /// Get an entity by long id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(int id);


        /// <summary>
        /// Get an entity using a where expression
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<T> GetAsync(Expression<Func<T, bool>> where);

        /// <summary>
        /// Gets all
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets entities using a where expression
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> where);

        Task<decimal> GetSumAsync(Expression<Func<T, bool>> where, Expression<Func<T, decimal>> sumCol);

        Task<long> GetCountAsync(Expression<Func<T, bool>> where);
    }
}
