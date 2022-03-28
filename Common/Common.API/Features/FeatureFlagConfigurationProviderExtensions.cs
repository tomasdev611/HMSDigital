using HMSDigital.Common.BusinessLayer.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace HMSDigital.Common.API.Features
{
    public static class FeatureFlagConfigurationProviderExtensions
    {
        public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder,
                                                               DbConfigOptions setup)
        {
            return
                builder.Add(new FeatureFlagConfigurationSource(setup));
        }

        public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder, Func<IDbConnection> createDbConnection, string tableName, bool reloadOnChange = false, TimeSpan? reloadInterval = null)
        {
            return AddDbConfiguration(builder, new DbConfigOptions
            {
                CreateDbConnection = createDbConnection,
                TableName = tableName,
                ReloadOnChange = reloadOnChange,
                ReloadInterval = reloadInterval
            });
        }
    }
}
