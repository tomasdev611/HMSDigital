using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HMSDigital.Common.API.HealthCheck
{
    public static class HealthCheckExtension
    {
        public static IHealthChecksBuilder AddSqlServerHealthChecks(this IHealthChecksBuilder builder, string connectionString)
        {
            return builder.AddSqlServer(connectionString: connectionString,
                 healthQuery: "SELECT 1;",
                 name: "HMSDigital Db",
                 failureStatus: HealthStatus.Degraded,
                 tags: new string[] { "db", "sql", "SqlServer", "HMSDigitalDb" });
        }

        public static IHealthChecksBuilder AddMySqlHealthChecks(this IHealthChecksBuilder builder, string connectionString,string dbName)
        {
            return builder.AddMySql(connectionString: connectionString,
                 name: dbName,
                 failureStatus: HealthStatus.Degraded,
                 tags: new string[] { "db", "sql", "SqlServer", dbName });
        }

    }
}
