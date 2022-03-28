using System;
using HMSDigital.Common.API.Features;
using HMSDigital.Common.BusinessLayer.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace HMSDigital.Report.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostCtx, configBuilder) =>
                    {
                        var configRoot = configBuilder.Build();
                        string connStr = configRoot.GetConnectionString("HMSDigitalDbConnection");
                        configBuilder.AddDbConfiguration(() => new SqlConnection(connStr), FeatureFlagConstants.FEATURE_TABLE, reloadOnChange: true, reloadInterval: TimeSpan.FromSeconds(1));
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
