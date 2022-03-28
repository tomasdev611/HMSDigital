using HMSDigital.Common.BusinessLayer.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;

namespace HMSDigital.Common.API.Features
{
    public class FeatureFlagConfigurationProvider : ConfigurationProvider, IDisposable
    {
        private readonly DbConfigOptions _options;

        //allow multi reading and single writing
        private readonly ReaderWriterLockSlim _lockObj = new();
        private bool _isDisposed;
        public FeatureFlagConfigurationProvider(DbConfigOptions options)
        {
            _options = options;
            var interval = TimeSpan.FromSeconds(2);
            if (options.ReloadInterval != null)
            {
                interval = options.ReloadInterval.Value;
            }
            if (options.ReloadOnChange)
            {
                ThreadPool.QueueUserWorkItem(obj =>
                {
                    while (!_isDisposed)
                    {
                        Load();
                        Thread.Sleep(interval);
                    }
                });
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
        }

        public override void Load()
        {
            base.Load();
            var clonedData = Data.Clone();
            var tableName = _options.TableName;
            try
            {
                _lockObj.EnterWriteLock();
                Data.Clear();
                using (var conn = _options.CreateDbConnection())
                {
                    conn.Open();
                    DoLoad(tableName, conn);
                }
            }
            catch (DbException)
            {
                //if DbException is thrown, restore to the original data.
                Data = clonedData;
                throw;
            }
            finally
            {
                _lockObj.ExitWriteLock();
            }
            //OnReload cannot be between EnterWriteLock and ExitWriteLock, or "A read lock may not be acquired with the write lock held in this mode" will be thrown.
            if (Helper.IsChanged(clonedData, Data))
            {
                OnReload();
            }
        }

        private void DoLoad(string tableName, IDbConnection conn)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"select 'FeatureManagement:' + Name, IsEnabled from {tableName}";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader.GetString(0);
                        var value = reader.GetBoolean(1);

                        Data[name] = value.ToString();
                    }
                }
            }
        }
    }

    internal static class Helper
    {
        public static IDictionary<string, string> Clone(this IDictionary<string, string> dict)
        {
            IDictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (var kv in dict)
            {
                newDict[kv.Key] = kv.Value;
            }
            return newDict;
        }

        public static bool IsChanged(IDictionary<string, string> oldDict,
                                     IDictionary<string, string> newDict)
        {
            if (oldDict.Count != newDict.Count)
            {
                return true;
            }
            foreach (var oldKv in oldDict)
            {
                var oldKey = oldKv.Key;
                var oldValue = oldKv.Value;
                var newValue = newDict[oldKey];
                if (oldValue != newValue)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
