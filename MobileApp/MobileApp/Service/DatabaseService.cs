using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MobileApp.Assets.Constants;
using MobileApp.Methods;
using SQLite;

namespace MobileApp.Service
{
    public class DatabaseService<T> where T : new()
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(DatabaseConstant.DatabasePath, DatabaseConstant.Flags);
        });
        private SQLiteAsyncConnection Database => lazyInitializer.Value;

        public DatabaseService()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(T).Name))
            {
                await Database.CreateTablesAsync(CreateFlags.None, typeof(T)).ConfigureAwait(false);
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await Database.Table<T>().ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return await Database.Table<T>().FirstOrDefaultAsync(where);
        }

        public async Task<List<T>> GetManyAsync(Expression<Func<T, bool>> where)
        {
            return await Database.Table<T>().Where(where).ToListAsync();
        }

        public async Task<int> SaveAllAsync(IEnumerable<T> itemList)
        {
            return await Database.InsertAllAsync(itemList);
        }

        public async Task<int> SaveAsync(T item)
        {
            return await Database.InsertAsync(item);
        }

        public async Task<int> UpdateAsync(T item)
        {
            return await Database.UpdateAsync(item);
        }

        public async Task<int> DeleteAsync(T item)
        {
            return await Database.DeleteAsync(item);
        }

        public async Task<int> DeleteAllAsync()
        {
            return await Database.DeleteAllAsync<T>();
        }
    }
}
